using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using WTA.Finance.Model;
using WTA.FinanceWebApi.Utility;
using WTA.FinanceWebApi.Utility.Auth;

namespace WtaFinanceService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<WtaDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("WtaDbConnection"));
            });

            #region jwt校验  HS
            JWTTokenOptions tokenOptions = new JWTTokenOptions();
            Configuration.Bind("JWTTokenOptions", tokenOptions);

            _ = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)//Scheme
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    //JWT有一些默认的属性，就是给鉴权时就可以筛选了
                    ValidateIssuer = true,//是否验证Issuer
                    ValidateAudience = true,//是否验证Audience
                    ValidateLifetime = true,//是否验证失效时间
                    ValidateIssuerSigningKey = true,//是否验证SecurityKey
                    ValidAudience = tokenOptions.Audience,//
                    ValidIssuer = tokenOptions.Issuer,//Issuer，这两项和前面签发jwt的设置一致
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey)),//拿到SecurityKey
                    //AudienceValidator = (m, n, z) =>
                    //{
                    //    //等同于去扩展了下Audience的校验规则---鉴权
                    //    return m != null && m.FirstOrDefault().Equals(this.Configuration["audience"]);
                    //},
                    LifetimeValidator = (notBefore, expires, securityToken, validationParameters) =>
                    {

                        return notBefore <= DateTime.Now
                        && expires >= DateTime.Now;
                        //&& validationParameters
                    }//自定义校验规则
                };
            });
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("AdminPolicy",
            //        policyBuilder => policyBuilder
            //        .RequireRole("Admin")//Claim的Role是Admin
            //        .RequireUserName("Eleven")//Claim的Name是Eleven
            //        .RequireClaim("EMail")//必须有某个Cliam
            //         .RequireClaim("Account")
            //        //.Combine(qqEmailPolicy)
            //        .AddRequirements(new CustomExtendRequirement())
            //        );//内置

            //    //options.AddPolicy("QQEmail", policyBuilder => policyBuilder.Requirements.Add(new QQEmailRequirement()));
            //    options.AddPolicy("DoubleEmail", policyBuilder => policyBuilder
            //    .AddRequirements(new CustomExtendRequirement())
            //    .Requirements.Add(new DoubleEmailRequirement()));
            //});
            //services.AddSingleton<IAuthorizationHandler, ZhaoxiMailHandler>();
            //services.AddSingleton<IAuthorizationHandler, QQMailHandler>();
            //services.AddSingleton<IAuthorizationHandler, CustomExtendRequirementHandler>();
            //services.AddTransient<>
            #endregion

            //这里做依赖注入
            services.AddTransient<CurrentUserModel, CurrentUserModel>();//瞬时
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            #region  JWT
            app.UseAuthentication();//鉴权：解析信息--就是读取token，解密token
            #endregion

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            Console.WriteLine(this.Configuration["ip"]);
            Console.WriteLine(this.Configuration["port"]);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            //实例启动时执行，且只执行一次
            this.Configuration.ConsulRegist();
        }
    }
}
