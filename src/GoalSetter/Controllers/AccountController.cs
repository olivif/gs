// <copyright file="AccountController.cs" company="olivif">
// Copyright (c) olivif 2016
// </copyright>

namespace GoalSetter.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using Models;
    using Models.AccountViewModels;

    /// <summary>
    /// Account Controller
    /// </summary>
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly SignInManager<ApplicationUser> signInManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="userManager">User manager</param>
        /// <param name="signInManager">Sign in manager</param>
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        /// <summary>
        /// GET: /Account/Login
        /// </summary>
        /// <param name="returnUrl">Return url, defaults to null</param>
        /// <returns>The action result</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            this.ViewData["ReturnUrl"] = returnUrl;
            return this.View();
        }

        /// <summary>
        /// POST: /Account/Login
        /// </summary>
        /// <param name="model">The login view model</param>
        /// <param name="returnUrl">Return url, defaults to null</param>
        /// <returns>The action result</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            this.ViewData["ReturnUrl"] = returnUrl;

            if (this.ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await this.signInManager.PasswordSignInAsync(
                    model.Email,
                    model.Password,
                    isPersistent: false,
                    lockoutOnFailure: false);

                if (result != null && result.Succeeded)
                {
                    return this.RedirectToLocal(returnUrl);
                }
            }

            // If we got this far, something failed
            throw new InvalidOperationException();
        }

        /// <summary>
        /// POST: /Account/LogOff
        /// </summary>
        /// <returns>The action result</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await this.signInManager.SignOutAsync();
            return this.RedirectToAction(nameof(HomeController.Index), "Home");
        }

        /// <summary>
        /// POST: /Account/ExternalLogin
        /// </summary>
        /// <param name="provider">The provider</param>
        /// <param name="returnUrl">Return url, defaults to null</param>
        /// <returns>The action result</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = this.Url.Action(
                "ExternalLoginCallback",
                "Account",
                new { ReturnUrl = returnUrl });

            var properties = this.signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return this.Challenge(properties, provider);
        }

        /// <summary>
        /// GET: /Account/ExternalLoginCallback
        /// </summary>
        /// <param name="returnUrl">Return url, defaults to null</param>
        /// <param name="remoteError">The remote error</param>
        /// <returns>The action result</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (returnUrl != null && remoteError == null)
            {
                var info = await this.signInManager.GetExternalLoginInfoAsync();

                if (info == null)
                {
                    return this.RedirectToAction(nameof(this.Login));
                }

                // Sign in the user with this external login provider if the user already has a login.
                var result = await this.signInManager.ExternalLoginSignInAsync(
                    info.LoginProvider,
                    info.ProviderKey,
                    isPersistent: false);

                if (result != null && result.Succeeded)
                {
                    return this.RedirectToLocal(returnUrl);
                }
            }

            // If we got this far, something failed
            throw new InvalidOperationException();
        }

        /// <summary>
        /// POST: /Account/ExternalLoginConfirmation
        /// </summary>
        /// <param name="model">The login view model</param>
        /// <param name="returnUrl">Return url, defaults to null</param>
        /// <returns>The action result</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl = null)
        {
            if (this.ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await this.signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return this.View("ExternalLoginFailure");
                }

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                var result = await this.userManager.CreateAsync(user);

                if (result != null && result.Succeeded)
                {
                    result = await this.userManager.AddLoginAsync(user, info);

                    if (result.Succeeded)
                    {
                        await this.signInManager.SignInAsync(user, isPersistent: false);
                        return this.RedirectToLocal(returnUrl);
                    }
                }
            }

            // If we got this far, something failed
            throw new InvalidOperationException();
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (this.Url.IsLocalUrl(returnUrl))
            {
                return this.Redirect(returnUrl);
            }
            else
            {
                return this.RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
    }
}
