﻿using AsianFarmerAPI.Business;
using AsianFarmerAPI.DBContexts;
using AsianFarmerAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Http;
using System;

namespace AsianFarmerAPI.Controllers
{
    [RoutePrefix("api/recipe")]
    public class RecipeController : ApiController
    {
        private AsianFarmerAPIContext db = new AsianFarmerAPIContext();

        [HttpGet]
        [Route("{name}")]
        public async Task<IHttpActionResult> GetRecipes(string name)
        {
            List<Recipe> recipes;
            try
            {
                recipes = await Parser.ParseRecipes(name);
                return Ok(recipes);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

    }
}
