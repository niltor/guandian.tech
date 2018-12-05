using Guandian.Data;
using Guandian.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Guandian.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ApplicationDbContext _context;
        protected readonly UserManager<IdentityUser> _userManager;
        protected readonly ILogger _logger;

        public BaseController(UserManager<IdentityUser> userManager, ApplicationDbContext context, ILogger logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }
    }
}
