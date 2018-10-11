using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Comment.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Comment.Areas.Identity.Pages.Account.Manage
{
    public class RealNameModel : PageModel
    {
        readonly IHostingEnvironment _env;
        readonly IdentityVerifyService _service;
        public RealNameModel(IHostingEnvironment env, IdentityVerifyService service)
        {
            _env = env;
            _service = service;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Display(Name = "姓名")]
            [Required(ErrorMessage = "姓名为必填项")]
            public string RealName { get; set; }
            [Display(Name = "身份证号码")]
            [Required(ErrorMessage = "身份证为必填项")]
            [StringLength(18, ErrorMessage = "需要18位的有效身份证格式", MinimumLength = 18)]
            public string IdentityCard { get; set; }
            [Display(Name = "出生日期")]
            [Required(ErrorMessage = "出生日期为必填项")]
            [DataType(DataType.Date)]
            public DateTime Birthday { get; set; }
            [Required(ErrorMessage = "需要上传身份证信息")]
            [Display(Name = "身份证")]
            public IFormFile File { get; set; }
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var dirPath = Path.Combine(_env.WebRootPath, "uploads");
            // 创建上传目录
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            // TODO 文件名称处理，调用接口验证，之后删除文件
            var ext = new FileInfo(Input.File.FileName).Extension;
            var filePath = Path.Combine(dirPath, Input.File.Name + ext);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await Input.File.CopyToAsync(fileStream);
            }

            await _service.RecognitionIdentityAsync(filePath);
            //await _service.VerifyIdentityAsync(Input.RealName, Input.IdentityCard);

            return RedirectToPage();
        }
    }
}