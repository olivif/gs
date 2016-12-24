// <copyright file="DatabaseGoalStorage.cs" company="olivif">
// Copyright (c) olivif 2016
// </copyright>

namespace GoalSetter.Service.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using GoalSetter.ModelsLogic;

    /// <summary>
    /// GoalStorage implemented using a db underneath
    /// </summary>
    public class DatabaseGoalStorage : IGoalStorage
    {
        private readonly GoalsDbContext dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseGoalStorage"/> class.
        /// </summary>
        /// <param name="dbContext">Goals db context</param>
        public DatabaseGoalStorage(GoalsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <inheritdoc />
        public Goal Create(Goal goal)
        {
            var addedGoal = this.dbContext.Goals.Add(goal);
            this.dbContext.SaveChanges();

            return addedGoal.Entity;
        }

        /// <inheritdoc />
        public IList<Goal> Read(Guid userId)
        {
            var goals = new List<Goal>();
            foreach (var goal in this.dbContext.Goals)
            {
                goals.Add(goal);
            }

            // Todo - do this in the database layer
            return goals.Where(x => x.UserId == userId).ToList();
        }
    }
}
