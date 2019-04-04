using AsianFarmerAPI.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AsianFarmerAPI.Controllers
{
    public class ParseController : ApiController
    {
        private const string URL = "https://conanexiles.gamepedia.com/";

        [HttpGet]
        [Route("{ingredientName}")]
        public void Parse(string ingredientName)
        {
            // Récupération du document de la page @url
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(URL + ingredientName);

            // Récupération de l'image
            string imgUrl = doc.DocumentNode.SelectNodes("//body/div/div/div/div/div/div/table/tbody")[2].SelectSingleNode("//td/a/img").Attributes[1].Value;

            // Récupération des recettes possible pour créer l'ingrédient
            HtmlNodeCollection recipesNodes = doc.DocumentNode.SelectNodes("//body/div/div/div/div/div/div/table")[2].ChildNodes[3].ChildNodes;
            recipesNodes.Remove(0); 
            recipesNodes.Remove(0);
            recipesNodes.Remove(0);
            recipesNodes.Remove(0); 

            List<Recipe> recipes = new List<Recipe>();
            Recipe tmp;
            foreach (HtmlNode node in recipesNodes)
            {
                tmp = new Recipe();
                tmp.Lines = new List<RecipeLine>();
            }

        }
    }
}
