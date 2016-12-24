// <copyright file="HomeController.cs" company="olivif">
// Copyright (c) olivif 2016
// </copyright>

namespace GoalSetter.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Models.Goals;

    /// <summary>
    /// Home Controller
    /// </summary>
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="userManager">User manager</param>
        public HomeController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        /// <summary>
        /// Index
        /// </summary>
        /// <returns>The action result</returns>
        public IActionResult Index()
        {
            return this.View();
        }

        /// <summary>
        /// Error
        /// </summary>
        /// <returns>The action result</returns>
        public IActionResult Error()
        {
            return this.View();
        }

        /// <summary>
        /// Data
        /// </summary>
        /// <returns>The action result</returns>
        [Authorize]
        public IActionResult Data()
        {
            return this.View();
        }

        /// <summary>
        /// POST: /Home/Data
        /// </summary>
        /// <param name="model">The goal view model</param>
        /// <returns>The action result</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Data(GoalViewModel model)
        {
            var userId = this.userManager.GetUserId(this.User);
            var userIdGuid = Guid.Parse(userId);

            model.UserId = userIdGuid;
            model.GoalId = Guid.NewGuid();

            // save?
            return this.View();
        }
    }
}
