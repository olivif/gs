// <copyright file="Goal.cs" company="olivif">
// Copyright (c) olivif 2016
// </copyright>

namespace GoalSetter.ModelsLogic
{
    using System;

    /// <summary>
    /// Goal business logic model
    /// </summary>
    public class Goal
    {
        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        public Guid GoalId { get; set; }

        /// <summary>
        /// Gets or sets the goal data
        /// </summary>
        public string Data { get; set; }
    }
}
