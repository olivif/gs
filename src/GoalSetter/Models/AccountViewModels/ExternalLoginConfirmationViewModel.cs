// <copyright file="ExternalLoginConfirmationViewModel.cs" company="olivif">
// Copyright (c) olivif 2016
// </copyright>

namespace GoalSetter.Models.AccountViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// External login view model
    /// </summary>
    public class ExternalLoginConfirmationViewModel
    {
        /// <summary>
        /// Gets or sets the email address
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
