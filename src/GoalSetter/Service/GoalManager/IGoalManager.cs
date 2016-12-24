// <copyright file="IGoalManager.cs" company="olivif">
// Copyright (c) olivif 2016
// </copyright>

namespace GoalSetter.Service.GoalManager
{
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
        /// Reads all goals for the currently signed in user
        /// </summary>
        /// <returns>The list of goals</returns>
        IList<Goal> Read();
    }
}
