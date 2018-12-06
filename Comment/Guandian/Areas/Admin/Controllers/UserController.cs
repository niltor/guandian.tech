using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Guandian.Data;
using Guandian.Data.Entity;
using Guandian.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Guandian.Areas.Admin.Controllers
{

    public class UserController : CommonController
    {
        readonly UserManager<IdentityUser> _userManager;
        public UserController(UserManager<IdentityUser> userManager, ApplicationDbContext context) : base(context)
        {
            _userManager = userManager;
        }
        public ActionResult Index()
        {
            var users = _context.Users.ToList();
            Console.WriteLine(StringTools.ToJson(users));
            return View(users);
        }

        /// <summary>
        /// 初始化管理员
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<ActionResult> InitAdminUser(string email, [DataType(DataType.Password)]string password)
        {
            if (email == null || password == null)
            {
                return Ok("无效的用户名或密码");
            }

            // 只有不存在管理员账号的时候执行
            if (!_context.Roles.Any(r => r.Name.Equals("Admin")))
            {
                // 添加角色
                _context.Roles.Add(new IdentityRole()
                {
                    Name = "Admin",
                    NormalizedName = "Admin",
                    Id = Guid.NewGuid().ToString()
                });

                // 添加用户
                var newUser = new User()
                {
                    Email = email,
                    UserName = email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(newUser, password);
                if (result.Succeeded)
                {
                    _context.SaveChanges();

                    // 用户添加到角色
                    var result1 = await _userManager.AddToRoleAsync(newUser, "Admin");
                    if (result1.Succeeded)
                    {
                        return Ok("初始化成功");
                    }
                }

                return Ok(result.Errors);
            }
            return Ok("已有管理员账号，无需初始化");
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}