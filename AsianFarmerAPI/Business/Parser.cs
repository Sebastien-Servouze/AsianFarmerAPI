using AsianFarmerAPI.DBContexts;
using AsianFarmerAPI.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace AsianFarmerAPI.Business
{
    public class Parser
    {
        private const string MAIN_URL = "https://conanexiles.gamepedia.com/";
        private static AsianFarmerAPIContext db = new AsianFarmerAPIContext();
        private static HtmlWeb web = new HtmlWeb();
        private static HtmlDocument currentDoc;

        public static HtmlDocument GetHtml(string appendix)
        {
            return web.Load(MAIN_URL + appendix);
        }

        public static async Task<Element> ParseElement(string elementName)
        {
            // Tentative de récupération en base
            Element element = await db.Elements.Where(e => e.Name == elementName).FirstOrDefaultAsync();

            // L'élément existe il déjà ?
            if (element != null)
                return element;
            else
                element = new Element();

            // Chargement de la page associée
            HtmlDocument doc = GetHtml(elementName);

            // Récupération des différentes informations nécessaires
            element.Name = doc.DocumentNode.SelectSingleNode("//*[@id=\"mw-content-text\"]/div/table[1]/tbody/tr[1]/th").InnerText.Replace("\n", "");
            element.Image = doc.DocumentNode.SelectSingleNode("//*[@id=\"mw-content-text\"]/div/table[1]/tbody/tr[2]/td/a/img").Attributes[1].Value;

            Debug.WriteLine("Création de l'élément '" + elementName + "'");

            return element;
        }

        public static async void ParseRecipeLines(HtmlNodeCollection recipeLineNodes, Recipe recipe)
        {
            // Pour chaque ligne de recette (5 élements par ligne de recette) [txt  a   txt a   br]
            //                                                                  o   n    n  o   n
            int quantity;
            string ingredientName;
            for (int i = 0; i < recipeLineNodes.Count; i += 5)
            {
                quantity = int.Parse(recipeLineNodes[i].InnerText);
                ingredientName = recipeLineNodes[i + 3].InnerText;

                // Recherche en base
                RecipeLine recipeLine = await db.RecipeLines.Where(rl => rl.Ingredient.Name == ingredientName && rl.Quantity == quantity).FirstOrDefaultAsync();

                // Si il existe on ne le reparse pas
                if (recipeLine == null)
                {
                    recipeLine = new RecipeLine();
                    recipeLine.Recipe = recipe;
                    recipeLine.Quantity = quantity;
                    recipeLine.Ingredient = await ParseElement(ingredientName);
                }

                recipe.Lines.Add(recipeLine);
            }
        }

        public static async Task<List<Recipe>> ParseRecipes(string productName)
        {
            List<Recipe> recipes = new List<Recipe>();

            Debug.WriteLine("Création de la recette pour l'élément '" + productName + "'");

            // On parse ensuite les lignes de recette nécessaires pour crafter le produit
            HtmlDocument doc = GetHtml(productName);

            // On récupère l'élément en lui même
            Element product = await ParseElement(productName);

            // Récupérations des recettes possibles pour le produit
            HtmlNode sourceNode = doc.DocumentNode.SelectSingleNode("//*[@id=\"Source\"]");
            HtmlNodeCollection recipesNodes = null;
            HtmlNode recipesTable = null;
            try
            {
                // Bypass jusqu'au tableau
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
            Element craftingTool = null;
            foreach (HtmlNode recipeNode in recipesNodes)
            {
                HtmlNode lineNode = recipeNode.SelectSingleNode("th/big/b/a");
                // Si la ligne est de type gros titre 
                if (lineNode != null)
                {
                    // On est alors sur une ligne d'outil de craft (par logique) et on crée une nouvelle recette
                    recipe = new Recipe();

                    // Ajout du produit et de l'outil
                    recipe.Product = product;
                    recipe.CraftingTool = await ParseElement(lineNode.InnerText);
              
                    craftingTool = recipe.CraftingTool;

                    continue;
                }
                // Si la ligne n'est pas un gros titre
                else
                {
                    // On cherche alors à récupérer la première colonne de la ligne
                    lineNode = recipeNode.SelectSingleNode("td");

                    // Si ce n'est pas une colonne, on passe à la ligne suivante
                    if (lineNode == null)
                    {
                        continue;
                    }
                    // Sinon, on crée un recette 
                    else
                    {
                        recipe = new Recipe();

                        // Ajout du produit et de l'outil
                        recipe.Product = product;
                        recipe.CraftingTool = craftingTool;
                    }
                }

                // On récupère toutes les lignes de recette
                recipeContentNodes = lineNode.ChildNodes;

                // On parse les lignes de recettes
                ParseRecipeLines(recipeContentNodes, recipe);

                // Ajout en base
                recipes.Add(db.Recipes.Add(recipe));
                db.SaveChanges();
            }

            return recipes;
        }
    }
}