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

        [Route("{ingredientName:string}")]
        public void Parse(string ingredientName)
        {
            // Récupération du document de la page @url
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(URL + ingredientName);


        }
    }
}
