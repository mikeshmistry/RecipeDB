using NUnit.Framework;
using Repositories;
using System;
using System.Collections.Generic;
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
        #endregion

        #region Other Method Tests
        #endregion

    }
}
