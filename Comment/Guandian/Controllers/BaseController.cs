using Guandian.Data;
using Guandian.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Guandian.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ApplicationDbContext _context;
        protected readonly UserManager<User> _userManager;

        public BaseController(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
    }
}
