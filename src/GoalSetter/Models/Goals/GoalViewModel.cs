// <copyright file="GoalViewModel.cs" company="olivif">
// Copyright (c) olivif 2016
// </copyright>

namespace GoalSetter.Models.Goals
{
    using System;

    /// <summary>
    /// Goal data model
    /// </summary>
    public class GoalViewModel
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
