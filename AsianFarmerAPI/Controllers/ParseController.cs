using AsianFarmerAPI.Business;
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
        [Route("{name}")]
        public void Parse(string name)
        {
            List<Recipe> recipes = Parser.ParseRecipes(name);
        }
    }
}
