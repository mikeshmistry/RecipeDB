using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Entities;
using System.Dynamic;

namespace DBContext
{
    /// <summary>
    /// Database Context class for a recipe database
    /// </summary>
    class RecipeContext : DbContext
    {
        #region Entities 

        /// <summary>
        /// Db set for the recipe entity
        /// </summary>
        public virtual DbSet<Recipe> Recipes { get; set; }

        /// <summary>
        /// Db set for the ingredient entity
        /// </summary>
        public virtual DbSet<Ingredient> Ingredients { get; set; }

        /// <summary>
        /// Db set for the CookingInstruction entity    
        /// </summary>
        public virtual DbSet<CookingInstruction> CookingInstructions { get; set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public RecipeContext()
        {

        }


        /// <summary>
        /// Constructor that takes in a context options
        /// </summary>
        /// <param name="options">The options object</param>
        public RecipeContext(DbContextOptions<RecipeContext> options) : base(options)
        {

        }

        #endregion 
    }
}
