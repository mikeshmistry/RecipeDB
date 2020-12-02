using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories
{
    /// <summary>
    /// Repository for the CookingInstruction entity
    /// </summary>
   public class CookingInstructionRepository : Repository<CookingInstructionRepository>
    {
        #region Constructor

        public CookingInstructionRepository(DbContext context):base(context)
        {

        }

        #endregion 
    }
}
