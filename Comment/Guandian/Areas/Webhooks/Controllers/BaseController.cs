using Guandian.Data;
using Microsoft.AspNetCore.Mvc;

namespace Guandian.Areas.Webhooks.Controllers
{
    [Route("webhooks/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {

        protected readonly ApplicationDbContext _context;
        public BaseController(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
