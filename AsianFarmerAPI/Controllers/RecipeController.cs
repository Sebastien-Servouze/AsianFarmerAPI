using AsianFarmerAPI.Business;
using AsianFarmerAPI.DBContexts;
using AsianFarmerAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Http;
using System;
using System.Web.Http.Cors;

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
            string loweredName = name.ToLower();
            string trimmedAndLoweredName = loweredName.Replace(" ", "");

            List<Recipe> recipes = await db.Recipes.Where(r => r.Product.Name.ToLower() == loweredName || r.Product.Name.ToLower().Replace(" ", "") == trimmedAndLoweredName).ToListAsync();
            if (!recipes.Any())
            {
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
            else
            {
                return Ok(recipes);
            }
        }

    }
}
