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

        public int ElementID { get; set; }
        public Element Product { get; set; }

        public int CraftingToolID { get; set; }
        public CraftingTool CraftingTool { get; set; }
        
        public virtual ICollection<RecipeLine> Lines { get; set; }

        public Recipe()
        {
            Lines = new List<RecipeLine>();
        }
    }
}