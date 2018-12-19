using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Guandian.Areas.Webhooks.Controllers
{
    [Route("webhooks/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
    }
}
