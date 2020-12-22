
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Repositories
{
    /// <summary>
    /// Generic repository class 
    /// </summary>
    /// <typeparam name="TEntity">The name of the strongly typed entity</typeparam>
    public class Repository<TEntity> where TEntity : class
    {
        #region fields 

        /// <summary>
        /// Reference to the db context object 
        /// </summary>
        protected readonly DbContext Context;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor that takes in db context
        /// </summary>
        /// <param name="dbContext">the database connection to use</param>
        public Repository(DbContext dbContext)
        {
            Context = dbContext;
            Context.Database.EnsureCreated();
        }

        #endregion

        #region Get/Find Methods 
        /// <summary>
        /// Method to find an entity by the id or the primary key
        /// </summary>
        /// <param name="id"> the id or primary key of the entity to find</param>
        /// <returns>A single entity with the found object null otherwise</returns>
        public TEntity GetId(int id)
        {
            return Context.Set<TEntity>().Find(id);
        }

        /// <summary>
        /// Method to get all the data for a given entity 
        /// </summary>
        /// <returns>A list of the entities</returns>
        public IEnumerable<TEntity> GetAll()
        {
            return Context.Set<TEntity>().ToList();
        }

        /// <summary>
        /// Method to find an entity based on a predicate
        /// </summary>
        /// <param name="predicate">the criteria to find an entity one</param>
        /// <returns>an enumerable collection of the entities that match the given criteria</returns>
        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
        }

        #endregion

        #region Add Methods

        /// <summary>
        /// Adds a single entity to the database 
        /// </summary>
        /// <param name="entity">the entity to be added</param>
        public void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
            Context.SaveChanges();
        }


        /// <summary>
        /// Adds multiple entities to the database 
        /// </summary>
        /// <param name="entities">the entities to be added</param>
        public void AddRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
            Context.SaveChanges();
        }

        #endregion

        #region Remove Methods

        /// <summary>
        /// Removes a single entity from the database 
        /// </summary>
        /// <param name="entity">the entity to be removed</param>
        public void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
            Context.SaveChanges();
        }


        /// <summary>
        /// Removes multiple entities from database 
        /// </summary>
        /// <param name="entities">the entities to be removed</param>
        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
            Context.SaveChanges();
        }


        #endregion

        #region Update Methods

        /// <summary>
        /// Updates an entity to the database
        /// </summary>
        /// <param name="entity"></param>
        public void Update(TEntity entity)
        {
            Context.Set<TEntity>().Update(entity);
            Context.SaveChanges();
        }

        /// <summary>
        /// Updates multiple entities from database 
        /// </summary>
        /// <param name="entities">the entities to be updated</param>
        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().UpdateRange(entities);
            Context.SaveChanges();
        }

        #endregion 
    }
}

