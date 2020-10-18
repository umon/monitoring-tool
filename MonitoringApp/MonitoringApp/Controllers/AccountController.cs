using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MonitoringApp.Models;
using MonitoringApp.Services;
using MonitoringApp.ViewModels;

namespace MonitoringApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMailService _mailService;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IMailService mailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = mailService;
        }

        public IActionResult Login(string returnUrl = "/")
        {
            if (User.Identity.IsAuthenticated)
            {
                if (Url.IsLocalUrl(returnUrl))
                    return LocalRedirect(returnUrl);

                return RedirectToAction("Index", "TargetApplication");
            }

            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserModelForLogin loginViewModel, string returnUrl = "/")
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(loginViewModel.UserName);

                if (user != null)
                {
                    var result = await _signInManager
                        .CheckPasswordSignInAsync(user, loginViewModel.Password, true);

                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, loginViewModel.RememberMe);

                        return LocalRedirect(returnUrl);
                    }
                }

                ModelState.AddModelError("", "Wrong Username or Password !");
            }

            return View(loginViewModel);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserModelForRegister userModelForRegister)
        {
            if (ModelState.IsValid)
            {
                if (await UserExists(userModelForRegister.UserName))
                {
                    ModelState.AddModelError("", "User name already exists.");
                    return View(userModelForRegister);
                }

                var user = new ApplicationUser
                {
                    UserName = userModelForRegister.UserName,
                    Email = userModelForRegister.Email,
                    FirstName = userModelForRegister.FirstName,
                    LastName = userModelForRegister.LastName
                };

                var result = await _userManager.CreateAsync(user, userModelForRegister.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "TargetApplication");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }

            return View(userModelForRegister);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Redirect("/");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                ModelState.AddModelError("", "You must specify a user name");
            }
            else
            {
                var user = await _userManager.FindByNameAsync(userName);

                if (user != null)
                {
                    var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var resetPasswordLink = Url.Action("ResetPassword", "Account",
                        new { uid = user.Id, token = resetPasswordToken },
                        protocol: Request.Scheme);


                    await _mailService.SendMail(user.Email, "Monitoring Tool - Reset Password", $"Reset Password Link : {resetPasswordLink}");

                    return RedirectToAction(nameof(Login));
                }

                ModelState.AddModelError("", "User not found");
            }

            return View();
        }

        public async Task<IActionResult> ResetPassword(string uid, string token)
        {
            if (string.IsNullOrEmpty(uid) || string.IsNullOrEmpty(token))
                return RedirectToAction("Index");

            var user = await _userManager.FindByIdAsync(uid);

            if (user == null)
            {
                return RedirectToAction("Register");
            }

            var model = new UserModelForResetPassword
            {
                UserId = uid,
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(UserModelForResetPassword resetPasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(resetPasswordViewModel.UserId);

                if (user == null)
                {
                    return RedirectToAction("Register");
                }

                var result = await _userManager.ResetPasswordAsync(user,
                    resetPasswordViewModel.Token,
                    resetPasswordViewModel.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(resetPasswordViewModel);
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(UserModelForChangePassword changePasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var userName = User.Identity.Name;

                var user = await _userManager.FindByNameAsync(userName);

                if (user != null)
                {
                    var result = await _userManager
                        .ChangePasswordAsync(user,
                            changePasswordViewModel.OldPassword,
                            changePasswordViewModel.NewPassword);

                    if (result.Succeeded)
                    {
                        ViewData["Succeed"] = true;
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View();
        }

        [NonAction]
        private async Task<bool> UserExists(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            return user != null;
        }

    }
}
