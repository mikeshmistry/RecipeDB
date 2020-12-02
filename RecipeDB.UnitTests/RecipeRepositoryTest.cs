using Entities;
using NUnit.Framework;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeDB.UnitTests
{
    /// <summary>
    /// Unit test class to test recipe repository 
    /// </summary>
    [TestFixture]
    class RecipeRepositoryTest : TestHelper
    {
        #region field 

        /// <summary>
        /// field for the repository 
        /// </summary>
        private RecipeRepository recipeRepository;

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

            //create the repository for  recipe 
            recipeRepository = new RecipeRepository(Context);
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

        #region Add Update Remove Tests

        /// <summary>
        /// Test to add a recipe to the database
        /// </summary>
        [TestCase]
        public void AddRecipe()
        {
            //create the new recipe 
            var recipe = new Recipe()
            {
                Name = "Butter Chicken"
            };

            //Add to the database
            recipeRepository.Add(recipe);

            //retrieve using the find method
            var foundRecipe = recipeRepository.Find(recipe => recipe.Name == recipe.Name)
                              .FirstOrDefault();

            //found it 
            if (foundRecipe != null)
                Assert.AreEqual(recipe.Name, foundRecipe.Name);
        }

        /// <summary>
        /// Test to update a recipe
        /// </summary>
        [TestCase]
        public void UpdateRecipe()
        {

            //create the new recipe 
            var recipe = new Recipe()
            {
                Name = "Butter Chicken"
            };

            //Add to the database
            recipeRepository.Add(recipe);

            //retrieve using the get all method returns should return on recipe to list
            var foundRecipe = recipeRepository.GetAll().ToList();

            //found it 
            if (foundRecipe != null)
                Assert.AreEqual(recipe.Name, foundRecipe[0].Name);

            //change the name
            foundRecipe[0].Name = "Goat Curry";
            recipeRepository.Update(foundRecipe[0]);

            //get the updated recipe 
            var updatedRecipe = recipeRepository.GetId(foundRecipe[0].RecipeId);

            //updated recipe is found
            if (updatedRecipe != null)
            {
                Assert.AreEqual(updatedRecipe.RecipeId, foundRecipe[0].RecipeId);
                Assert.AreEqual(updatedRecipe.Name, foundRecipe[0].Name);
            }



        }

        /// <summary>
        /// Test to delete a recipe
        /// </summary>
        [TestCase]
        public void DeleteRecipe()
        {

            //create the new recipe 
            var recipe = new Recipe()
            {
                Name = "Butter Chicken"
            };

            //add the recipe
            recipeRepository.Add(recipe);

            //should be one
            var countBeforeDelete = recipeRepository.GetAll().Count();

            Assert.AreEqual(1, countBeforeDelete);

            //remove it
            recipeRepository.Remove(recipe);

            var countAfterDelete = recipeRepository.GetAll().Count();

            Assert.AreEqual(0, countAfterDelete);
        }

        #endregion

        #region Other Method Tests

        #region GetIngredients

        /// <summary>
        /// Test to get ingredients for a recipe that does not exist 
        /// </summary>
        [TestCase]
        public void GetIngredients_RecipeNotFound_ReturnsNull()
        {
            //get ingredients for a recipe that does not exist
            var ingredients = recipeRepository.GetIngredients(12234);
            
            //Should return null list
            Assert.IsNull(ingredients);
        }


        /// <summary>
        /// Test to get ingredients for a recipe that does exist 
        /// </summary>
        [TestCase]
        public void GetIngredients_RecipeFound_ReturnsIngredients()
        {

            //create the new recipe 
            var recipe = new Recipe()
            {
                Name = "Butter Chicken"
            };

            //find the added recipe
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
            var ingredientRepository = new IngredientRepository(Context);
            ingredientRepository.AddRange(ingredients);

            //get all ingredients and now add it to the recipe 
            var ingredientsList = ingredientRepository.GetAll().ToList();

            //three ingredients should be added to the database
            Assert.AreEqual(3, ingredientsList.Count());

            //found the recipe and ingredients list
            if(foundRecipe!= null && ingredientsList != null)
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

        #region GetCookingInstructions Tests

        /// <summary>
        /// Test to get cooking instructions for a recipe that does not exist
        /// </summary>
        [TestCase]
        public void GetCookingInstructions_RecipeNotFound_ReturnsNull()
        {
            //get the cooking instructions for a recipe that does not exist
            var instuctions = recipeRepository.GetCookingInstructions(12345);
           
            //returns a null list
            Assert.IsNull(instuctions);
        }


        /// <summary>
        /// Test to get cooking instructions for a recipe that does exist
        /// </summary>
        [TestCase]
        public void GetCookingInstructions_RecipeFound_ReturnsInstructions()
        {

            //create the new recipe 
            var recipe = new Recipe()
            {
                Name = "Butter Chicken"
            };

            //find the added recipe
            var foundRecipe = recipeRepository.Find(recipe => recipe.Name == "Butter Chicken")
                                        .FirstOrDefault();
            //create instructions 
            var instructions = new List<CookingInstruction>()
            {

                new CookingInstruction() { Name="Prepare The Chicken", Instruction ="Cut the chicken into small cubes" },
                new CookingInstruction() { Name="Creating The Curry", Instruction ="Mix all the dry ingredient together and when mussy add the chicken" },
                new CookingInstruction() { Name="Cooking Instructions", Instruction ="Cook the chicken in the curry for 20 minutes or until the chicken is done" }

            };

            //add the instructions 
            var instructionsRepository = new CookingInstructionRepository(Context);
            instructionsRepository.AddRange(instructions);

            //get all instructions and now add it to the recipe 
            var instructionList = instructionsRepository.GetAll().ToList();

            //three instructions should be added to the database
            Assert.AreEqual(3, instructionList.Count());

            //found the recipe and instructions list
            if (foundRecipe != null && instructionList != null)
            {
                var addedToRecipe = false;


                foreach (var instruction in instructionList)
                {

                    //add the instructions to the recipe
                    addedToRecipe = instructionsRepository.AssignCookingInstructionsToRecipe(foundRecipe.RecipeId, instruction.CookingInstructionId);
                    Assert.IsTrue(addedToRecipe);

                }

                //Insure that the foundRecipe has the same count as the ingredients list
                Assert.AreEqual(foundRecipe.Ingredients.Count(), instructionList.Count());
            }



        }


        #endregion




        #endregion
    }
}
