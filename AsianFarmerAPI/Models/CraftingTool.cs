using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AsianFarmerAPI.Models
{
    public class CraftingTool
    {
        [Key]
        public int CraftingToolID { get; set; }

        public string Name { get; set; }
        public string Image { get; set; }
    }
}