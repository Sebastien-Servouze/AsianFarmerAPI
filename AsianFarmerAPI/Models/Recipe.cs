using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AsianFarmerAPI.Models
{
    public class Recipe
    {
        [Key]
        public int RecipeID { get; set; }

        public int IngredientID { get; set; }
        public Ingredient Craftable { get; set; }
        public virtual ICollection<RecipeLine> Lines { get; set; }
    }
}