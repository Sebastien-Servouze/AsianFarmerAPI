using AsianFarmerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AsianFarmerAPI.DBContexts
{
    public class AsianFarmerAPIContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public AsianFarmerAPIContext() : base("name=AsianFarmerAPIContext")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<AsianFarmerAPIContext>());
        }

        public DbSet<Recipe> Recipes { get; set; }

        public DbSet<Element> Elements { get; set; }

        public DbSet<RecipeLine> RecipeLines { get; set; }
    }
}