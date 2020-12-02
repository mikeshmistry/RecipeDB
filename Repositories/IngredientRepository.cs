using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories
{
    /// <summary>
    /// Repository class for the ingredients entity
    /// </summary>
   public class IngredientRepository : Repository<Ingredient>
    {

        #region Constructor

        /// <summary>
        /// Constructor that takes in a db context
        /// </summary>
        /// <param name="context"></param>
        public IngredientRepository(DbContext context):base(context)
        {

        }

        #endregion 

    }
}
