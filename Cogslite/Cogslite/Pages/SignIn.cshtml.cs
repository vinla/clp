﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CogsLite.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cogslite.Pages
{
    public class SignInModel : CogsPageModel
    {
        private readonly IUserStore _userStore;

        public SignInModel(IUserStore userStore)
        {
            _userStore = userStore ?? throw new ArgumentNullException(nameof(userStore));
        }

        public void OnGet(string message)
        {
            ViewData["Message"] = message;
        }

        public async Task<IActionResult> OnPostAsync(string username, string password)
        {
            var user = _userStore.Get(username);

            if (user != null && user.Password == password)
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Email, username), new Claim("CogsMember", "Yes") };
                var claimsIdentity = new ClaimsIdentity(claims, "login");
                var claimPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(claimPrincipal);
                return Redirect("Home");
            }
            else
                return RedirectToAction("SignIn", new { message = "Invalid username or password" });
        }
    }
}