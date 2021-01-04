using DatabaseContext;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repositories
{
    /// <summary>
    /// Repository for the CookingInstruction entity
    /// </summary>
   public class CookingInstructionRepository : Repository<CookingInstruction>
    {
        #region Property
        
        /// <summary>
        /// Property to get the context for the recipe database
        /// </summary>
        private RecipeContext RecipeContext 
        {
            get 
            {
                return Context as RecipeContext;
            }
        }

        #endregion 
        
        #region Constructor

        public CookingInstructionRepository(DbContext context):base(context)
        {

        }

        #endregion

        #region Other Methods

        /// <summary>
        /// Method to add a cooking instruction to a recipe
        /// </summary>
        /// <param name="recipeId">The id of the recipe to add to</param>
        /// <param name="instructionId">The id of the cooking instruction to add to the recipe</param>
        /// <returns>True if the cooking instruction was added to the recipe.False otherwise</returns>
        public bool AssignCookingInstructionsToRecipe(int recipeId,int instructionId)
        {
            var added = false;
            
            //find the recipe to see if it exists
            var recipe = RecipeContext.Recipes
                         .Where(recipe => recipe.RecipeId == recipeId)
                         .Include(instructions => instructions.CookingInstructions)
                         .FirstOrDefault();
            
            //find the cooking instructions to see if it exists
            var cookingInstruction = RecipeContext.CookingInstructions
                                     .Where(instructions => instructions.CookingInstructionId == instructionId)
                                     .FirstOrDefault();

            //both are found 
            if(recipe !=null && cookingInstruction !=null)
            {
                var foundInstuctions = recipe.CookingInstructions
                                     .Where(instruction => instruction.Recipe.RecipeId == recipeId && instruction.CookingInstructionId == instructionId)
                                     .FirstOrDefault();

                //not found so add it to the recipe
                if(foundInstuctions == null)
                {
                    recipe.CookingInstructions.Add(cookingInstruction);
                    RecipeContext.SaveChanges();
                    added = true;
                }
            }

            return added;
        }


        /// <summary>
        /// Method to delete a cooking instruction
        /// </summary>
        /// <param name="instructionId">the id of the cooking instruction to be deleted</param>
        public void DeleteCookingInstruction(int instructionId)
        {
            var foundCookingInstruction = RecipeContext.CookingInstructions
                                          .Where(instruction => instruction.CookingInstructionId == instructionId)
                                          .Include(instruction => instruction.Recipe)
                                          .FirstOrDefault();

            if (foundCookingInstruction != null)
                Remove(foundCookingInstruction);
        }

        #endregion 
    }
}
