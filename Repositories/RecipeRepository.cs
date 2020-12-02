using DatabaseContext;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repositories
{
   public class RecipeRepository : Repository<Recipe>
    {
        #region Properties
        
        private RecipeContext RecipeContext
        {
            get
            {
                return Context as RecipeContext;
            }
        }

        #endregion 

        #region Constructor
        
        /// <summary>
        /// Constructor that takes in a db context object
        /// </summary>
        /// <param name="context"></param>
        public RecipeRepository(DbContext context):base(context)
        {

        }

        #endregion

        #region Other Queries

        /// <summary>
        /// Query that gets all the ingredients that are linked to the recipe 
        /// </summary>
        /// <param name="recipeId">The id of the recipe to find the ingredients for</param>
        /// <returns></returns>
        public List<Ingredient> GetIngredients(int recipeId)
        {
            List<Ingredient> ingredients = null;

            //get all the ingredients for the recipe 

            var allIngredients = RecipeContext.Recipes
                                 .Where(recipe => recipe.RecipeId == recipeId)
                                 .Include(ingredient => ingredient.Ingredients)
                                 .FirstOrDefault();

            //found the ingredients
            if (allIngredients != null)
                ingredients = allIngredients.Ingredients.ToList();

            return ingredients;
        }

        /// <summary>
        /// Query to get all cooking instructions that are linked to the recipe 
        /// </summary>
        /// <param name="recipeId">The id of the recipe to find the cooking instructions for</param>
        /// <returns>A list of cooking instruction objects. Null list otherwise</returns>
        public List<CookingInstruction> GetCookingInstructions(int recipeId)
        {
            List<CookingInstruction> cookingInstructions = new List<CookingInstruction>();

            var allcookingInstructions = RecipeContext.Recipes
                                 .Where(recipe => recipe.RecipeId == recipeId)
                                 .Include(instructions => instructions.CookingInstructions)
                                 .FirstOrDefault();

            //found the cooking instructions
            if (allcookingInstructions != null)
                cookingInstructions = allcookingInstructions.CookingInstructions.ToList();

            return cookingInstructions;
        }

        #endregion

    }
}
