using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities
{
    /// <summary>
    /// Entity class to represent cooking instructions object
    /// </summary>
   public class CookingInstruction
    {
        #region Properties

        /// <summary>
        /// Primary key auto seeded
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CookingInstructionId { get; set; }

        /// <summary>
        /// Name for the instruction 
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The Instruction for cooking
        /// </summary>
        [Required]
        public string Instruction { get; set; }

        #endregion

        #region RelationShips
        
        /// <summary>
        /// Many to one relationship to recipe
        /// </summary>
        public Recipe Recipe { get; set; }

        #endregion 
    }
}
