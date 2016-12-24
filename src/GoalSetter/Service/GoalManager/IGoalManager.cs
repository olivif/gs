// <copyright file="IGoalManager.cs" company="olivif">
// Copyright (c) olivif 2016
// </copyright>

namespace GoalSetter.Service.GoalManager
{
    using System;
    using System.Collections.Generic;
    using GoalSetter.ModelsLogic;

    /// <summary>
    /// Business logic manager for goal operations
    /// </summary>
    public interface IGoalManager
    {
        /// <summary>
        /// Create a goal
        /// </summary>
        /// <param name="goal">The goal to be created</param>
        /// <returns>True if operation suceeded, false otherwise.</returns>
        bool Create(Goal goal);

        /// <summary>
        /// Reads all goals for a given user id
        /// </summary>
        /// <param name="userId">The id of the user to retreive the goals for</param>
        /// <returns>The list of goals</returns>
        IList<Goal> Read(Guid userId);
    }
}
