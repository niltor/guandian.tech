using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Guandian.Areas.Webhooks.Controllers
{
    public class GithubController : BaseController
    {
        [HttpGet("test")]
        public string Test()
        {
            return "test";
        }
    }
}
