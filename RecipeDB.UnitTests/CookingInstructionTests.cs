using Entities;
using Newtonsoft.Json.Bson;
using NUnit.Framework;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeDB.UnitTests
{
    /// <summary>
    /// Unit Test class for cooking instruction repository 
    /// </summary>
    class CookingInstructionTests : TestHelper
    {

        #region field 

        /// <summary>
        /// field for the repository 
        /// </summary>
        private CookingInstructionRepository cookingInstructionRepository;

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

            //create the repository for cooking instructions
            cookingInstructionRepository = new CookingInstructionRepository(Context);
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
        /// Test to add a cooking instruction
        /// </summary>
        [TestCase]
        public void AddInstruction()
        {
            //create a new cooking instruction 
            var instruction = new CookingInstruction()
            {
                Name = "Prepare The Chicken",
                Instruction = "Cut the chicken into small cubes"
            };

            //add the cooking instruction to the database
            cookingInstructionRepository.Add(instruction);

            var foundInstruction = cookingInstructionRepository.GetAll().ToList();

            if (foundInstruction != null)
            {
                Assert.AreEqual(1, foundInstruction.Count());
                Assert.AreEqual(foundInstruction[0].Name, instruction.Name);
                Assert.AreEqual(foundInstruction[0].Instruction, instruction.Instruction);
            }

        }


        /// <summary>
        /// Test to update a cooking instruction
        /// </summary>
        [TestCase]
        public void UpdateInstruction()
        {

            //create a new cooking instruction 
            var instruction = new CookingInstruction()
            {
                Name = "Prepare The Chicken",
                Instruction = "Cut the chicken into small cubes"
            };

            //add the cooking instruction to the database
            cookingInstructionRepository.Add(instruction);

            //find the added instruction 
            var foundInstruction = cookingInstructionRepository.Find(instruction => instruction.Name == "Prepare The Chicken")
                                                               .FirstOrDefault();
            //found it
            if (foundInstruction != null)
            {
                //change the values
                foundInstruction.Name = "Cool Down";
                foundInstruction.Instruction = "Let the chicken cool for 15 minutes before serving";

                //update it
                cookingInstructionRepository.Update(foundInstruction);

                var updatedInstruction = cookingInstructionRepository.GetId(foundInstruction.CookingInstructionId);

                if (updatedInstruction != null)
                {
                    Assert.AreEqual(updatedInstruction.CookingInstructionId, foundInstruction.CookingInstructionId);
                    Assert.AreEqual(updatedInstruction.Name, foundInstruction.Name);
                    Assert.AreEqual(updatedInstruction.Instruction, foundInstruction.Instruction);
                }
            }
        }

        /// <summary>
        /// Test to delete a cooking instruction
        /// </summary>
        [TestCase]
        public void DeleteInstruction()
        {

            //create a new cooking instruction 
            var instruction = new CookingInstruction()
            {
                Name = "Prepare The Chicken",
                Instruction = "Cut the chicken into small cubes"
            };

            //add the cooking instruction to the database
            cookingInstructionRepository.Add(instruction);

            //find the added instruction 
            var foundInstruction = cookingInstructionRepository.GetAll().ToList();

            //found it
            if (foundInstruction != null)
                Assert.AreEqual(1, foundInstruction.Count());

            cookingInstructionRepository.Remove(foundInstruction[0]);

            //get the count after deletion
            foundInstruction = cookingInstructionRepository.GetAll().ToList();

            //count should be zero
            Assert.AreEqual(0, foundInstruction.Count());


        }
        #endregion

        #region Other Method Tests

        #region AssignCookingInstructionsToRecipe Tests


        /// <summary>
        /// Test to add cooking instruction to a recipe when both entities are not found
        /// </summary>
        [TestCase]
        public void AssignCookingInstructionsToRecipe_BothNotFound_ReturnsFalse()
        {
            //add a cooking instruction where recipe and cooking instruction are not found
            var added = cookingInstructionRepository.AssignCookingInstructionsToRecipe(12245, 456456);

            //should return false
            Assert.IsFalse(added);
        }

        /// <summary>
        /// Test to add a cooking instruction to a recipe when the recipe exists and the cooking instruction is not found 
        /// </summary>
        [TestCase]
        public void AssignCookingInstructionToRecipe_RecipeFoundInstructionNotFound_ReturnsFalse()
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

            //add a cooking instruction where recipe is found and the cooking instruction is not
            if (foundRecipe != null)
            {
                var added = cookingInstructionRepository.AssignCookingInstructionsToRecipe(foundRecipe[0].RecipeId, 456456);
                //should return false
                Assert.IsFalse(added);
            }
        }

        /// <summary>
        /// Test to add a cooking instruction to a recipe when recipe is not found and cooking instruction is found
        /// </summary>
        [TestCase]
        public void AssignCookingInstructionsToRecipe_RecipeNotFoundInstructionFound_ReturnsFalse()
        {
            var instruction = new CookingInstruction()
            {
                Name = "Deep Fry Chicken",
                Instruction = "Deep fry the chicken in hot oil until golden brown"
            };

            cookingInstructionRepository.Add(instruction);

            var foundInstuction = cookingInstructionRepository.GetAll().ToList();

            //there should only be one added record in the database
            Assert.AreEqual(1, foundInstuction.Count());

            //add a cooking instruction where cooking instruction is found and recipe is not
            if (foundInstuction != null)
            {
                var added = cookingInstructionRepository.AssignCookingInstructionsToRecipe(456456, foundInstuction[0].CookingInstructionId);
                //should return false
                Assert.IsFalse(added);
            }
        }

        /// <summary>
        /// Test adding a cooking instruction to a recipe when both exist 
        /// </summary>
        [TestCase]
        public void AssignCookingInstructionsToRecipe_BothExists_ReturnsTrue()
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
            //create instructions 
            var instructions = new List<CookingInstruction>()
            {

                new CookingInstruction() { Name="Prepare The Chicken", Instruction ="Cut the chicken into small cubes" },
                new CookingInstruction() { Name="Creating The Curry", Instruction ="Mix all the dry ingredient together and when mussy add the chicken" },
                new CookingInstruction() { Name="Cooking Instructions", Instruction ="Cook the chicken in the curry for 20 minutes or until the chicken is done" }

            };

            cookingInstructionRepository.AddRange(instructions);

            //get all instructions and now add it to the recipe 
            var instructionList = cookingInstructionRepository.GetAll().ToList();

            //three instructions should be added to the database
            Assert.AreEqual(3, instructionList.Count());

            //found the recipe and instructions list
            if (foundRecipe != null && instructionList != null)
            {
                var addedToRecipe = false;


                foreach (var instruction in instructionList)
                {

                    //add the instructions to the recipe
                    addedToRecipe = cookingInstructionRepository.AssignCookingInstructionsToRecipe(foundRecipe.RecipeId, instruction.CookingInstructionId);
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
