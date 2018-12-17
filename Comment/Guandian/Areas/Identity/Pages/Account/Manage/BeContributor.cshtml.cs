using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Guandian.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Guandian.Areas.Identity.Pages.Account.Manage
{
    public class BeContributorModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public BeContributorModel(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public bool IsLoginGithub { get; set; } = false;

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var CurrentLogins = await _userManager.GetLoginsAsync(user);
            var hasGithub = CurrentLogins.Any(c => c.LoginProvider.ToLower().Equals("github"));
            var loginName = User.FindFirstValue(ClaimTypes.Name);

            if (hasGithub && !string.IsNullOrEmpty(loginName))
            {
                // TODO:·¢ËÍÑûÇë

                IsLoginGithub = true;
            }
            else
            {
                IsLoginGithub = false;
            }
            return Page();
        }
    }
}