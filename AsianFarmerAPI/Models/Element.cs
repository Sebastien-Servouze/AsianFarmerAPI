using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AsianFarmerAPI.Models
{
    public class Element
    {
        [Key]
        public int ElementID { get; set; }

        public string Name { get; set; }
        public string Image { get; set; }
    }
}