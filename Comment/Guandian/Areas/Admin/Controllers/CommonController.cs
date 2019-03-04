using Guandian.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Guandian.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Admin")]
    public class CommonController : Controller
    {
        protected readonly ApplicationDbContext _context;
        public CommonController(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}