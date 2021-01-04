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
    /// Repository class for the recipe entity 
    /// </summary>
   public class RecipeRepository : Repository<Recipe>
    {
        #region Property
        
        /// <summary>
        /// Property to the Recipe Context
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
        
        /// <summary>
        /// Constructor that takes in a db context object
        /// </summary>
        /// <param name="context"></param>
        public RecipeRepository(DbContext context):base(context)
        {

        }

        #endregion

        #region Other Methods

        /// <summary>
        /// Method to delete a recipe from the database
        /// </summary>
        /// <param name="recipeId">The id of the recipe to delete</param>
        public void DeleteRecipe(int recipeId)
        {
            var recipe = RecipeContext.Recipes
                         .Where(recipe => recipe.RecipeId == recipeId)
                         .Include(ingredient => ingredient.Ingredients)
                         .Include(instructions => instructions.CookingInstructions)
                         .FirstOrDefault();

            if (recipe != null)
                Remove(recipe);
        }


        /// <summary>
        /// Method that gets all the ingredients that are linked to the recipe 
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
                                 .Where(ingredient => ingredient.RecipeId == recipeId)
                                 .AsNoTracking()
                                 .FirstOrDefault();

            //found the ingredients
            if (allIngredients != null)
                ingredients = allIngredients.Ingredients.ToList();

            return ingredients;
        }

        /// <summary>
        /// Method to get all cooking instructions that are linked to the recipe 
        /// </summary>
        /// <param name="recipeId">The id of the recipe to find the cooking instructions for</param>
        /// <returns>A list of cooking instruction objects. Null list otherwise</returns>
        public List<CookingInstruction> GetCookingInstructions(int recipeId)
        {
            List<CookingInstruction> cookingInstructions = null;

            var allcookingInstructions = RecipeContext.Recipes
                                 .Where(recipe => recipe.RecipeId == recipeId)
                                 .Include(instructions => instructions.CookingInstructions)
                                 .Where(instructions => instructions.RecipeId == recipeId)
                                 .AsNoTracking()
                                 .FirstOrDefault();

            //found the cooking instructions
            if (allcookingInstructions != null)
                cookingInstructions = allcookingInstructions.CookingInstructions.ToList();

            return cookingInstructions;
        }

        /// <summary>
        /// Method to get a recipe with all ingredients and cooking instructions
        /// </summary>
        /// <param name="recipeId">The id of the recipe to be found</param>
        /// <returns>A recipe with all the ingredients and cooking instructions</returns>
        public Recipe GetRecipeWithIngredientsAndInstructions(int recipeId)
        {
            var recipe = RecipeContext.Recipes
                         .Where(recipe => recipe.RecipeId == recipeId)
                         .Include(ingredients => ingredients.Ingredients)
                         .Include(instructions => instructions.CookingInstructions)
                         .AsNoTracking()
                         .FirstOrDefault();
                         

            return recipe;
        }


        #endregion

    }
}
