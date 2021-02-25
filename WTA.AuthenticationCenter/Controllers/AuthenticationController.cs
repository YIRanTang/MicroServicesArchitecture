using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WTA.AuthenticationCenter.Utility;
using WTA.Finance.Model;

namespace WTA.AuthenticationCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {

        #region MyRegion
        private ILogger<AuthenticationController> _logger = null;
        private IJWTService _iJWTService = null;
        private readonly IConfiguration _iConfiguration;
        public AuthenticationController(ILoggerFactory factory,
            ILogger<AuthenticationController> logger,
            IConfiguration configuration
            , IJWTService service)
        {
            this._logger = logger;
            this._iConfiguration = configuration;
            this._iJWTService = service;
        }
        #endregion

        [Route("Get")]
        [HttpGet]
        public IEnumerable<int> Get()
        {
            return new List<int>() { 1, 2, 3, 4, 6, 7 };
        }


        [Route("Login")]
        [HttpPost]
        public string Login(string name, string password)
        {
            if ("System".Equals(name) && "zjzt#123456".Equals(password))//应该数据库
            {
                CurrentUserModel currentUser = new CurrentUserModel()
                {
                    Id = 123,
                    Account = "zhongjiazhitong@wtafinance.Net",
                    EMail = "zhongjiazhitong@qq.com",
                    Mobile = "173****8159",
                    Sex = 1,
                    Age = 33,
                    Name = "ZJZT",
                    Role = "Admin"
                };

                string token = this._iJWTService.GetToken(currentUser);
                return JsonConvert.SerializeObject(new
                {
                    result = true,
                    token
                });
            }
            else
            {
                return JsonConvert.SerializeObject(new
                {
                    result = false,
                    token = ""
                });
            }
        }
    }
}
