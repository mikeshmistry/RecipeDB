using Entities;
using NUnit.Framework;
using Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace RecipeDB.UnitTests
{
    /// <summary>
    /// Unit test class for testing ingredients repository
    /// </summary>
    class IngredientRepositoryTests : TestHelper
    {
        #region field 

        /// <summary>
        /// field for the repository 
        /// </summary>
        private IngredientRepository ingredientRepository;

        #endregion

        #region Setup and Cleanup Methods

        /// <summary>
        /// Method to perform setup procedures.Runs per test case
        /// </summary>
        [SetUp]
        public void Setup()
        {
            //setup the connections and context 
            SetupConnectionAndContext();

            //create the repository for ingredients
            ingredientRepository = new IngredientRepository(Context);
        }

        /// <summary>
        /// Method to perform cleanup procedures. Runs per test case
        /// </summary>
        [TearDown]
        public void CleanUp()
        {
            //close the connection
            Connection.Close();
        }

        #endregion

        #region Add Update Delete Tests

        /// <summary>
        /// Test to add an ingredient to the database
        /// </summary>
        [TestCase]
        public void AddIngrident()
        {
            //create the ingredient
            var ingredient = new Ingredient()
            {
                Name = "Whole Chicken"

            };

            //add the new ingredient to the database 
            ingredientRepository.Add(ingredient);

            //find the added ingredient 
            var foundIngredient = ingredientRepository.GetAll().ToList();

            //if found check to see if the names are matching
            if (foundIngredient != null)
                Assert.AreEqual(ingredient.Name, foundIngredient[0].Name);
        }

        /// <summary>
        /// Test method to update an ingredient
        /// </summary>
        [TestCase]
        public void UpdateIngrident()
        {
            //create the ingredient
            var ingredient = new Ingredient()
            {
                Name = "Whole Chicken"

            };

            //add the new ingredient to the database 
            ingredientRepository.Add(ingredient);

            //find the added ingredient 
            var foundIngredient = ingredientRepository.GetAll().ToList();

            //if found check to see if the names are matching
            if (foundIngredient != null)
                Assert.AreEqual(ingredient.Name, foundIngredient[0].Name);

            //change the name of the ingredient
            foundIngredient[0].Name = " 1/4 of a chicken";

            //update the database
            ingredientRepository.Update(foundIngredient[0]);

            //get the updated ingredient
            var updatedIngredient = ingredientRepository.GetId(foundIngredient[0].IngredientId);

            if (updatedIngredient != null)
            {
                Assert.AreEqual(updatedIngredient.IngredientId, foundIngredient[0].IngredientId);
                Assert.AreEqual(updatedIngredient.Name, foundIngredient[0].Name);
            }
        }

        /// <summary>
        /// Test to Delete an Ingredient from the database
        /// </summary>
        [TestCase]
        public void DeleteIngredient()
        {
            //create the ingredient
            var ingredient = new Ingredient()
            {
                Name = "Whole Chicken"

            };

            //add the ingredient
            ingredientRepository.Add(ingredient);

            //find the added ingredient
            var foundIngredient = ingredientRepository.GetAll();

            //check to see if the counts are the same should be one
            if (foundIngredient != null)
                Assert.AreEqual(1, foundIngredient.Count());

            //delete the ingredient
            ingredientRepository.Remove(ingredient);

            //now count should be zero
            foundIngredient = ingredientRepository.GetAll();

            if (foundIngredient != null)
                Assert.AreEqual(0, foundIngredient.Count());

        }

        #endregion

        #region Other Method Tests

        #region AddIngredientToRecipe Tests
       
        /// <summary>
        /// Test to add ingredient to a recipe when both entities are not found
        /// </summary>
        [TestCase]
        public void AddIngredientToRecipe_BothNotFound_ReturnsFalse()
        {
            //add an ingredient where recipe and ingredient are not found
            var added = ingredientRepository.AddIngredientToRecipe(12245, 456456);
           
            //should return false
            Assert.IsFalse(added);
        }


        /// <summary>
        /// Test to add ingredient to a recipe when recipe exists and ingredient does not exist 
        /// </summary>
        [TestCase]
        public void AddIngredientToRecipe_RecipeFoundIngredientNotFound_ReturnsFalse()
        {
            var recipe = new Recipe()
            {
                Name = "Fried Chicken"
            };
            
            //add the recipe to the database
            var recipeRepository = new RecipeRepository(Context);
            recipeRepository.Add(recipe);

            var foundRecipe = recipeRepository.GetAll().ToList();

            //there should only be one added record in the database
            Assert.AreEqual(1, foundRecipe.Count());

            //add an ingredient where recipe is found and the ingredient is not
            if (foundRecipe != null)
            {
                var added = ingredientRepository.AddIngredientToRecipe(foundRecipe[0].RecipeId, 456456);
                //should return false
                Assert.IsFalse(added);
            }
            
        }

        /// <summary>
        /// Test to add ingredient to a recipe when recipe does not exist and ingredient does exist 
        /// </summary>
        [TestCase]
        public void AddIngredientToRecipe_IngredientFoundRecipeNotFound_ReturnsFalse()
        {
            var ingredient = new Ingredient()
            {
                Name = "Beef"
                
            };

           
            //add the ingredient to the database
            var ingredientRepository = new IngredientRepository(Context);
            ingredientRepository.Add(ingredient);

            var foundIngredient = ingredientRepository.GetAll().ToList();

            //there should only be one added record in the database
            Assert.AreEqual(1, foundIngredient.Count());

            //add an ingredient where recipe is not found and the ingredient is 
            if (foundIngredient != null)
            {
                var added = ingredientRepository.AddIngredientToRecipe(456456, foundIngredient[0].IngredientId);
                //should return false
                Assert.IsFalse(added);
            }

        }

        /// <summary>
        /// Test add an ingredient to a recipe both are found
        /// </summary>
        [TestCase]
        public void AddIngredientToRecipe_BothFound_ReturnsTrue()
        {
            //create the new recipe 
            var recipe = new Recipe()
            {
                Name = "Butter Chicken"
            };


            //find the added recipe
            var recipeRepository = new RecipeRepository(Context);
            var foundRecipe = recipeRepository.Find(recipe => recipe.Name == "Butter Chicken")
                                        .FirstOrDefault();
            //create ingredients 
            var ingredients = new List<Ingredient>()
            {

                new Ingredient() { Name="Whole Chicken" },
                new Ingredient() { Name= "Sour Cream" },
                new Ingredient() {Name = "Gee"}
            };

            //add the ingredients
            ingredientRepository.AddRange(ingredients);

            //get all ingredients and now add it to the recipe 
            var ingredientsList = ingredientRepository.GetAll().ToList();

            //three ingredients should be added to the database
            Assert.AreEqual(3, ingredientsList.Count());

            //found the recipe and ingredients list
            if (foundRecipe != null && ingredientsList != null)
            {
                var addedToRecipe = false;


                foreach (var ingredient in ingredientsList)
                {

                    //add the ingredients to the recipe
                    addedToRecipe = ingredientRepository.AddIngredientToRecipe(foundRecipe.RecipeId, ingredient.IngredientId);
                    Assert.IsTrue(addedToRecipe);

                }

                //Insure that the foundRecipe has the same count as the ingredients list
                Assert.AreEqual(foundRecipe.Ingredients.Count(), ingredientsList.Count());
            }
        }

        #endregion

        #endregion

    }
}
