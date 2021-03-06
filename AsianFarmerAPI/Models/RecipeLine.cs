﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AsianFarmerAPI.Models
{
    public class RecipeLine
    {
        [Key]
        public int RecipeLineID { get; set; }

        public int RecipeID { get; set; }
        public Recipe Recipe { get; set; }
        [ForeignKey("Ingredient")]
        public int IngredientID { get; set; }
        public virtual Element Ingredient { get; set; }
        public int Quantity { get; set; }
    }
}