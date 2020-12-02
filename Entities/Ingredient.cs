using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Text;

namespace Entities
{
    /// <summary>
    /// Entity class to represent an ingredient object
    /// </summary>
    public class Ingredient
    {
        #region Properties

        /// <summary>
        /// Primary key auto seeded
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IngredientId { get; set; }

        /// <summary>
        /// Name for the ingredient. This is a required field
        /// </summary>
        [Required]
        public string Name { get; set; }

        #endregion

        #region RelationShips

        /// <summary>
        /// Many to one relationship between recipe and Ingredients 
        /// </summary>
        public Recipe Recipe { get; set; }

        #endregion
    }
}
