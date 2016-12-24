// <copyright file="GoalsDbContext.cs" company="olivif">
// Copyright (c) olivif 2016
// </copyright>

namespace GoalSetter.Data
{
    using Microsoft.EntityFrameworkCore;
    using ModelsLogic;

    /// <summary>
    /// Database context for goals data
    /// </summary>
    public class GoalsDbContext : DbContext, IDbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GoalsDbContext"/> class.
        /// </summary>
        /// <param name="options">Db context options</param>
        public GoalsDbContext(DbContextOptions<GoalsDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the goals
        /// </summary>
        public DbSet<Goal> Goals { get; set; }

        /// <summary>
        /// OnModelCreating
        /// </summary>
        /// <param name="builder">The builder</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder
                .Entity<Goal>()
                .HasKey(g => new { g.UserId, g.GoalId });
        }
    }
}
