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

            #region jwtУ��  HS
            JWTTokenOptions tokenOptions = new JWTTokenOptions();
            Configuration.Bind("JWTTokenOptions", tokenOptions);

            _ = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)//Scheme
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    //JWT��һЩĬ�ϵ����ԣ����Ǹ���Ȩʱ�Ϳ���ɸѡ��
                    ValidateIssuer = true,//�Ƿ���֤Issuer
                    ValidateAudience = true,//�Ƿ���֤Audience
                    ValidateLifetime = true,//�Ƿ���֤ʧЧʱ��
                    ValidateIssuerSigningKey = true,//�Ƿ���֤SecurityKey
                    ValidAudience = tokenOptions.Audience,//
                    ValidIssuer = tokenOptions.Issuer,//Issuer���������ǰ��ǩ��jwt������һ��
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey)),//�õ�SecurityKey
                    //AudienceValidator = (m, n, z) =>
                    //{
                    //    //��ͬ��ȥ��չ����Audience��У�����---��Ȩ
                    //    return m != null && m.FirstOrDefault().Equals(this.Configuration["audience"]);
                    //},
                    LifetimeValidator = (notBefore, expires, securityToken, validationParameters) =>
                    {

                        return notBefore <= DateTime.Now
                        && expires >= DateTime.Now;
                        //&& validationParameters
                    }//�Զ���У�����
                };
            });
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("AdminPolicy",
            //        policyBuilder => policyBuilder
            //        .RequireRole("Admin")//Claim��Role��Admin
            //        .RequireUserName("Eleven")//Claim��Name��Eleven
            //        .RequireClaim("EMail")//������ĳ��Cliam
            //         .RequireClaim("Account")
            //        //.Combine(qqEmailPolicy)
            //        .AddRequirements(new CustomExtendRequirement())
            //        );//����

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

            //����������ע��
            services.AddTransient<CurrentUserModel, CurrentUserModel>();//˲ʱ
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
            app.UseAuthentication();//��Ȩ��������Ϣ--���Ƕ�ȡtoken������token
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


            //ʵ������ʱִ�У���ִֻ��һ��
            this.Configuration.ConsulRegist();
        }
    }
}
