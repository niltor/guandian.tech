using System;
using System.Security.Claims;
using Guandian.Data;
using Guandian.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
//using MSDev.DB;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Senparc.CO2NET;
using Senparc.CO2NET.RegisterServices;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP;
using Senparc.Weixin.RegisterServices;

namespace Guandian
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // 配置项
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            // 数据库
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            //services.AddDbContext<MSDevContext>(options =>
            //   options.UseSqlServer(
            //       Configuration.GetConnectionString("MSDev")));
            // 身份验证服务 
            // TODO:这里之后改成自定义的User，已生成UI之后改model会出错 https://github.com/aspnet/Docs/issues/7764
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            //.AddEntityFrameworkStores<MSDevContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 6;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            services.AddSingleton<IdentityVerifyService>();
            services.AddTransient<IEmailSender, EmailSender>();

            // 添加policy角色
            services.AddAuthorization(options =>
            {
                // TODO:相应绑定注册逻辑需要添加角色
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
                options.AddPolicy("GitHub", policy => policy.RequireRole("GitHub"));
                options.AddPolicy("RealName", policy => policy.RequireRole("RealName"));
                options.AddPolicy("Guest", policy => policy.RequireRole("Guest"));
            });


            services.AddAuthentication()
                .AddGitHub(options =>
                {
                    options.ClientId = Configuration.GetSection("OAuth")["Github:ClientId"];
                    options.ClientSecret = Configuration.GetSection("OAuth")["Github:ClientSecret"];
                    options.CallbackPath = Configuration.GetSection("Oauth")["Github:CallbackPath"];
                    options.ClaimActions.MapJsonKey("urn:github:avatar", "avatar_url");
                    options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
                    options.Scope.Add("read:user");
                    options.Scope.Add("public_repo");
                    options.Scope.Add("user:email");
                    options.SaveTokens = true;

                    options.Events = new OAuthEvents
                    {

                    };
                });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                 .AddJsonOptions(options =>
                 {
                     options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                     options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                 }); ;

            services.AddMemoryCache();// 使用本地缓存必须添加
            services.AddSession();// 使用Session
            services.AddSingleton(typeof(GithubService));

            services.AddSenparcGlobalServices(Configuration)//Senparc.CO2NET 全局注册
                    .AddSenparcWeixinServices(Configuration);//Senparc.Weixin 注册

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IOptions<SenparcSetting> senparcSetting, IOptions<SenparcWeixinSetting> senparcWeixinSetting)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                //app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseCookiePolicy();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            // 启动 CO2NET 全局注册，必须！
            IRegisterService register = RegisterService.Start(env, senparcSetting.Value).UseSenparcGlobal();
            register.UseSenparcWeixin(senparcWeixinSetting.Value, senparcSetting.Value)
                .RegisterMpAccount(senparcWeixinSetting.Value, string.Empty);
        }
    }
}
