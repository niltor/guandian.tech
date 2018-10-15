using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MSDev.DB;
using MSDev.MetaWeblog;
using Newtonsoft.Json;

namespace Comment.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CnBlogController : ControllerBase
    {
        readonly MSDevContext _context;
        public CnBlogController(MSDevContext context)
        {
            _context = context;
        }

        // GET: api/CnBlog
        [HttpGet]
        public ActionResult<IEnumerable<CategoryInfo>> Get()
        {
            string blogUrl = "http://www.cnblogs.com";
            string metablogUrl = "https://rpc.cnblogs.com/metaweblog/msdeveloper";
            var blogcon = new BlogConnectionInfo(blogUrl, metablogUrl, "320557", "niltor", "$54NilTor");

            var client = new Client(blogcon);
            var userBlogs = client.GetPost("9768158");
            //blogcon.BlogID = userBlogs.FirstOrDefault().BlogID;

            System.Console.WriteLine(JsonConvert.SerializeObject(userBlogs));
            var result = client.GetCategories();
            return result;

            //var result = _context.Blog
            //    .Include(b => b.Catalog)
            //    .Take(10).ToList();
            //return result;
        }

        // GET: api/CnBlog/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/CnBlog
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/CnBlog/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
