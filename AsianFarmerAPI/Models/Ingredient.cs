using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AsianFarmerAPI.Models
{
    public class Ingredient
    {
        [Key]
        public int IngredientID { get; set; }

        public string Name { get; set; }
        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}