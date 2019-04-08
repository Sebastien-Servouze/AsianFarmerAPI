using AsianFarmerAPI.Business;
using AsianFarmerAPI.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace AsianFarmerAPI.Controllers
{
    [RoutePrefix("api/parse")]
    public class ParseController : ApiController
    {
        private const string URL = "https://conanexiles.gamepedia.com/";

        [HttpGet]
        [Route("{name}")]
        public IEnumerable<Recipe> Parse(string name)
        {
            List<Recipe> recipes = Parser.ParseRecipes(name);

            return recipes;
        }
    }
}
