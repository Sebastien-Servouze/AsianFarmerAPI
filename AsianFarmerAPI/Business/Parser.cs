using AsianFarmerAPI.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;

namespace AsianFarmerAPI.Business
{
    public class Parser
    {
        private const string MAIN_URL = "https://conanexiles.gamepedia.com/";
        public static List<Element> elements = new List<Element>();

        public static Element ParseElement(string elementName)
        {
            if (elements.Where(e => e.Name == elementName).Count() > 0)
                return elements.Where(e => e.Name == elementName).First();

            Element element = new Element();

            // Chargement de la page associée
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(MAIN_URL + elementName);

            // Récupération des différentes informations nécessaires
            element.Name = doc.DocumentNode.SelectSingleNode("//*[@id=\"mw-content-text\"]/div/table[1]/tbody/tr[1]/th").InnerText.Replace("\n", "");
            element.Image = doc.DocumentNode.SelectSingleNode("//*[@id=\"mw-content-text\"]/div/table[1]/tbody/tr[2]/td/a/img").Attributes[1].Value;

            Debug.WriteLine("Création de l'élément '" + elementName + "'");

            Thread.Sleep(100);

            elements.Add(element);

            return element;
        }

        public static List<Recipe> ParseRecipes(string productName)
        {
            List<Recipe> recipes = new List<Recipe>();
            
            // On crée d'abord l'élement en lui même
            Element product = ParseElement(productName);

            Debug.WriteLine("Création de la recette pour l'élément '" + productName + "'");

            // On parse ensuite les lignes de recette nécessaires pour crafter le produit
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(MAIN_URL + productName);

            // Récupération de l'outil de fabrication
            //recipe.CraftingTool = new CraftingTool();
            //recipe.CraftingTool.Name = doc.DocumentNode.SelectSingleNode("*[@id=\"mw-content-text\"]/div/table[2]/tbody/tr[1]/th/span/a").InnerText;
            //recipe.CraftingTool.Image = doc.DocumentNode.SelectSingleNode("//*[@id=\"mw-content-text\"]/div/table[2]/tbody/tr[2]/td/a/img").Attributes[1].Value;

            // Récupérations des recettes possibles pour le produit
            HtmlNode sourceNode = doc.DocumentNode.SelectSingleNode("//*[@id=\"Source\"]");
            HtmlNodeCollection recipesNodes = null;
            HtmlNode recipesTable = null;
            try
            {
                recipesTable = sourceNode.ParentNode.NextSibling;
                do
                {
                    recipesTable = recipesTable.NextSibling;
                } while (recipesTable.OriginalName != "table");

                recipesNodes = recipesTable.SelectNodes("tbody/tr");
            }
            catch (NullReferenceException)
            {
                // On aura l'exception si l'élément est une matière première
                return recipes;
            }

            // Pour chaque ligne de tableau de recette
            HtmlNodeCollection recipeContentNodes;
            Recipe recipe;
            RecipeLine recipeLine;
            foreach (HtmlNode recipeNode in recipesNodes)
            {
                if (recipeNode.SelectSingleNode("td") == null)
                    continue;

                recipes.Add(new Recipe());
                recipe = recipes.Last();
                recipe.Product = product;

                // On récupère toutes les lignes de recettes
                recipeContentNodes = recipeNode.SelectSingleNode("td").ChildNodes;

                // Pour chaque ligne de recette (5 élements par ligne de recette) [txt  a   txt a   br]
                //                                                                  o   n    n  o   n
                for (int i = 0; i < recipeContentNodes.Count; i += 5)
                {
                    recipe.Lines.Add(new RecipeLine());
                    recipeLine = recipe.Lines.Last();
                    recipeLine.Quantity = int.Parse(recipeContentNodes[i].InnerText);
                    recipeLine.Ingredient = ParseElement(recipeContentNodes[i + 3].InnerText);

                    // On en profite pour parser sa recette
                    // TODO: si on ne la connait pas déjà
                    recipes.AddRange(ParseRecipes(recipeContentNodes[i + 3].InnerText));
                }
            }

            return recipes;
        }
    }
}