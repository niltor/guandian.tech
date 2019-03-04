using Microsoft.AspNetCore.Mvc;

namespace Guandian.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CnBlogController : ControllerBase
    {
        //readonly MSDevContext _context;
        //public CnBlogController(MSDevContext context)
        //{
        //    _context = context;
        //}

        //// GET: api/CnBlog
        //[HttpGet]
        //public ActionResult<IEnumerable<CategoryInfo>> Get()
        //{
        //    string blogUrl = "http://www.cnblogs.com";
        //    string metablogUrl = "https://rpc.cnblogs.com/metaweblog/msdeveloper";
        //    var blogcon = new BlogConnectionInfo(blogUrl, metablogUrl, "320557", "niltor", "$54NilTor");

        //    var client = new Client(blogcon);
        //    var userBlogs = client.GetPost("9768158");
        //    //blogcon.BlogID = userBlogs.FirstOrDefault().BlogID;

        //    System.Console.WriteLine(JsonConvert.SerializeObject(userBlogs));
        //    var result = client.GetCategories();
        //    return result;
        //}

    }
}
