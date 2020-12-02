using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Text;

namespace Entities
{
    public class Recipe
    {
        #region Properties 
        /// <summary>
        ///  Primary key auto seeded
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecipeId { get; set; }

        /// <summary>
        /// Name of the recipe
        /// </summary>
        [Required]
        public string Name { get; set; }

        #endregion

        #region Relationships
        
        /// <summary>
        /// One to many relationship with Ingredient
        /// </summary>
        public ICollection<Ingredient> Ingredients { get; set; }

        /// <summary>
        /// One to many relationship with CookingInstruction
        /// </summary>
        public ICollection<CookingInstruction> CookingInstructions { get; set; }

        #endregion 
    }
}
