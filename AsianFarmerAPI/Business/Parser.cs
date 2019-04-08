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

        public static Element ParseElement(string elementName)
        {
            Element element = new Element();

            // Chargement de la page associée
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(MAIN_URL + elementName);

            // Récupération des différentes informations nécessaires
            element.Name = doc.DocumentNode.SelectSingleNode("//*[@id=\"mw-content-text\"]/div/table[1]/tbody/tr[1]/th").InnerText.Replace("\n", "");
            element.Image = doc.DocumentNode.SelectSingleNode("//*[@id=\"mw-content-text\"]/div/table[1]/tbody/tr[2]/td/a/img").Attributes[1].Value;

            Debug.WriteLine("Création de l'élément '" + elementName + "'");

            return element;
        }

        public static List<Recipe> ParseRecipes(string productName)
        {
            List<Recipe> recipes = new List<Recipe>();

            Debug.WriteLine("Création de la recette pour l'élément '" + productName + "'");

            // On parse ensuite les lignes de recette nécessaires pour crafter le produit
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(MAIN_URL + productName);

            // On récupère l'élément en lui même
            Element product = ParseElement(productName);

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
            Recipe recipe = null;
            RecipeLine recipeLine;
            foreach (HtmlNode recipeNode in recipesNodes)
            {
                // Si on est sur l'outil de fabrication, on crée la recette
                if (recipeNode.SelectSingleNode("th/big/b/a") != null)
                {
                    recipes.Add(new Recipe());
                    recipe = recipes.Last();
                    recipe.Product = product;

                    recipe.CraftingTool = ParseElement(recipeNode.SelectSingleNode("th/big/b/a").InnerText);

                    continue;
                }
                // Si on est pas sur une ligne normale (avec des td)
                if (recipeNode.SelectNodes("td") == null)
                {
                    continue;
                }

                // On récupère toutes les lignes de recettes
                recipeContentNodes = recipeNode.SelectSingleNode("td").ChildNodes;

                // Pour chaque ligne de recette (5 élements par ligne de recette) [txt  a   txt a   br]
                //                                                                  o   n    n  o   n
                for (int i = 0; i < recipeContentNodes.Count; i += 5)
                {
                    recipe.Lines.Add(new RecipeLine());
                    recipeLine = recipe.Lines.Last();
                    recipeLine.Recipe = recipe;
                    recipeLine.Quantity = int.Parse(recipeContentNodes[i].InnerText);
                    recipeLine.Ingredient = ParseElement(recipeContentNodes[i + 3].InnerText);
                }
            }

            return recipes;
        }
    }
}