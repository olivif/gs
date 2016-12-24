// <copyright file="IDbContext.cs" company="olivif">
// Copyright (c) olivif 2016
// </copyright>

namespace GoalSetter.Data
{
    using System;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// IDbContext
    /// </summary>
    public interface IDbContext : IDisposable
    {
        /// <summary>
        /// Entity set
        /// </summary>
        /// <typeparam name="TEntity">Type of the entity in the set</typeparam>
        /// <returns>The entity set</returns>
        DbSet<TEntity> Set<TEntity>()
            where TEntity : class;

        /// <summary>
        /// Save changes for this context
        /// </summary>
        /// <returns>Error code</returns>
        int SaveChanges();
    }
}
