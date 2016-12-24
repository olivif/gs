// <copyright file="GoalManager.cs" company="olivif">
// Copyright (c) olivif 2016
// </copyright>

namespace GoalSetter.Service.Manager
{
    using System;
    using System.Collections.Generic;
    using GoalSetter.ModelsLogic;
    using Storage;

    /// <summary>
    /// GoalManager
    /// </summary>
    public class GoalManager : IGoalManager
    {
        private readonly IGoalStorage storage;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoalManager"/> class.
        /// </summary>
        /// <param name="storage">Goals storage</param>
        public GoalManager(IGoalStorage storage)
        {
            this.storage = storage;
        }

        /// <inheritdoc />
        public bool Create(Goal goal)
        {
            this.storage.Create(goal);

            return true;
        }

        /// <inheritdoc />
        public IList<Goal> Read(Guid userId)
        {
            return this.storage.Read(userId);
        }
    }
}
