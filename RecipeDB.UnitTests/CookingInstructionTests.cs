using NUnit.Framework;
using Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeDB.UnitTests
{
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
        #endregion

        #region Other Method Tests
        #endregion

    }
}
