// <copyright file="HomeController.cs" company="olivif">
// Copyright (c) olivif 2016
// </copyright>

namespace GoalSetter.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Home Controller
    /// </summary>
    public class HomeController : Controller
    {
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
    }
}
