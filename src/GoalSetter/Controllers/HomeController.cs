﻿// <copyright file="HomeController.cs" company="olivif">
// Copyright (c) olivif 2016
// </copyright>

namespace GoalSetter.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Models.Goals;
    using ModelsLogic;

    /// <summary>
    /// Home Controller
    /// </summary>
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly GoalsDbContext goalsDbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="userManager">User manager</param>
        /// <param name="goalsDbContext">Goals database context</param>
        public HomeController(
            UserManager<ApplicationUser> userManager,
            GoalsDbContext goalsDbContext)
        {
            this.userManager = userManager;
            this.goalsDbContext = goalsDbContext;
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

            var goal = new Goal()
            {
                UserId = userIdGuid,
                GoalId = Guid.NewGuid(),
                Data = model.Data
            };

            // Save to db
            this.goalsDbContext.Goals.Add(goal);
            this.goalsDbContext.SaveChanges();

            return this.View();
        }
    }
}
