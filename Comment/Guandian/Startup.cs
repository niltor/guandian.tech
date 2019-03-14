using Guandian.Areas.Webhooks.Manager;
using Guandian.Data;
using Guandian.Data.Entity;
using Guandian.Manager;
using Guandian.Services;
using Guandian.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Senparc.CO2NET;
using Senparc.CO2NET.RegisterServices;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP;
using Senparc.Weixin.RegisterServices;
using System;
using System.Security.Claims;
using System.Text;

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
            services.Configure<GithubOption>(options =>
            {
                options.PAT = Configuration.GetSection("OAuth")["PAT:TechviewsManageToken"];
                options.WebHookSecret = Configuration.GetSection("OAuth")["WebHookSecret"];
            });
            // 数据库
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            //services.AddDbContext<MSDevContext>(options =>
            //   options.UseSqlServer(
            //       Configuration.GetConnectionString("MSDev")));
            // 身份验证服务 
            services.AddIdentity<User, IdentityRole>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
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

            // 身份验证服务
            services.AddSingleton<IdentityVerifyService>();
            services.AddSingleton<GithubManageService>();
            services.AddScoped<GithubEventManager>();

            // 添加policy角色
            services.AddAuthorization(options =>
            {
                // TODO:相应绑定注册逻辑需要添加角色
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
                options.AddPolicy("GitHub", policy => policy.RequireRole("GitHub"));
                options.AddPolicy("RealName", policy => policy.RequireRole("RealName"));
                options.AddPolicy("Register", policy => policy.RequireRole("Register"));
            });


            services.AddAuthentication()
                .AddGitHub(options =>
                {
                    options.ClientId = Configuration.GetSection("OAuth")["Github:ClientId"];
                    options.ClientSecret = Configuration.GetSection("OAuth")["Github:ClientSecret"];
                    options.ClaimActions.MapJsonKey("urn:github:avatar", "avatar_url");
                    options.ClaimActions.MapJsonKey("urn:github:id", "id");
                    options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
                    options.Scope.Add("read:user");
                    options.Scope.Add("public_repo");
                    options.Scope.Add("repo:invite");
                    options.Scope.Add("user:email");
                    options.SaveTokens = true;
                });

            services.AddMvc();

            services.AddStackExchangeRedisCache(option =>
            {
                option.Configuration = "localhost";
                option.InstanceName = "guandian";

            });

            services.AddMemoryCache();// 使用本地缓存必须添加
            services.AddSession();// 使用Session 
            services.AddSingleton(typeof(GithubService));
            services.AddSingleton(typeof(GithubManageService));

            services.AddScoped(typeof(ReviewManager));

            services.AddSenparcGlobalServices(Configuration)//Senparc.CO2NET 全局注册
                    .AddSenparcWeixinServices(Configuration);//Senparc.Weixin 注册

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IOptions<SenparcSetting> senparcSetting, IOptions<SenparcWeixinSetting> senparcWeixinSetting, IApplicationLifetime lifetime, IDistributedCache cache)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            // 缓存设置
            lifetime.ApplicationStarted.Register(() =>
            {
                var currentTimeUTC = DateTime.UtcNow.ToString();
                byte[] encodedCurrentTimeUTC = Encoding.UTF8.GetBytes(currentTimeUTC);
                var options = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(20));
                cache.Set("cachedTimeUTC", encodedCurrentTimeUTC, options);
            });

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
