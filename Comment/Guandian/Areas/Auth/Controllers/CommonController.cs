using Guandian.Data;
using Microsoft.AspNetCore.Mvc;
namespace Guandian.Areas.Auth.Controllers
{
    [Area("Auth")]
    public class CommonController : Controller
    {
        protected string ClientId { get; set; }
        protected string ClientSecret { get; set; }
        protected readonly ApplicationDbContext _context;
        public CommonController(string clientId, string clientSecret, ApplicationDbContext context)
        {
            _context = context;
            ClientId = clientId;
            ClientSecret = clientSecret;
        }
        public CommonController()
        {

        }

    }
}