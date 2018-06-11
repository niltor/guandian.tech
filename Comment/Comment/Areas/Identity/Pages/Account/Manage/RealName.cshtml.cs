using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Comment.Areas.Identity.Pages.Account.Manage
{
    public class RealNameModel : PageModel
    {
        public IActionResult OnGet()
        {
            return Page();
        }
    }
}