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
    /// Repository class for the ingredients entity
    /// </summary>
   public class IngredientRepository : Repository<Ingredient>
    {

        #region Property

        // <summary>
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
        /// Constructor that takes in a db context
        /// </summary>
        /// <param name="context"></param>
        public IngredientRepository(DbContext context):base(context)
        {

        }

        #endregion

        #region Other Queries

        /// <summary>
        /// Add an ingredient to a recipe
        /// </summary>
        /// <param name="recipeId">The id of the recipe to add the ingredient to</param>
        /// <param name="ingredientId">The id of the ingredient to</param>
        /// <returns>True if ingredient was added to the recipe. False other wise</returns>
        public bool AddIngredientToRecipe(int recipeId, int ingredientId)
        {
            var added = false;

            //find the recipe
            var recipe = RecipeContext.Recipes
                            .Where(recipe => recipe.RecipeId == recipeId)
                            .Include(recipe => recipe.Ingredients)
                            .FirstOrDefault();

            //find the ingredient
            var ingredient = RecipeContext.Ingredients
                            .Where(ingredient => ingredient.IngredientId == ingredientId)
                            .FirstOrDefault();

            //recipe and ingredient found
            if(recipe !=null && ingredient !=null)
            {
                //make sure that the ingredient to be added is not already added
                var foundIngredient = recipe.Ingredients
                                     .Where(ingredient => ingredient.Recipe.RecipeId == recipeId
                                            && ingredient.IngredientId == ingredientId)
                                    .FirstOrDefault(); 

                //if ingredient was not found for the recipe then add it
                if(foundIngredient == null)
                {
                    recipe.Ingredients.Add(ingredient);
                    RecipeContext.SaveChanges();
                    added = true;
                }

            }

            return added;
        }

       
        #endregion 
    }
}
