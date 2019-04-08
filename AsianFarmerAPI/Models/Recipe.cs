using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AsianFarmerAPI.Models
{
    public class Recipe
    {
        [Key]
        public int RecipeID { get; set; }

        [ForeignKey("Product")]
        public int ProductID { get; set; }
        public virtual Element Product { get; set; }

        [ForeignKey("CraftingTool")]
        public int CraftingToolID { get; set; }
        public virtual Element CraftingTool { get; set; }
        
        public virtual ICollection<RecipeLine> Lines { get; set; }

        public Recipe()
        {
            Lines = new List<RecipeLine>();
        }
    }
}