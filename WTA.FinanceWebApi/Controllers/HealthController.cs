using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WTA.Redis.Service;

namespace Zhaoxi.NetCore31WebApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly ILogger<HealthController> _logger;
        private readonly IConfiguration _iConfiguration;
        public HealthController(ILogger<HealthController> logger, IConfiguration configuration)
        {
            _logger = logger;
            this._iConfiguration = configuration;
        }

        [HttpGet]
        [Route("Index")]//拼接到控制器上的route
        public IActionResult Index()
        {
            this._logger.LogWarning($"This is HealthController {this._iConfiguration["Port"]}");
            return Ok();//HttpStatusCode--200
        }

        public IActionResult Get()
        {
            this._logger.LogWarning($"服务启动成功");
            string res = "";
            using (RedisStringService service = new RedisStringService())
            {
                res = service.Get("test1");
                if (res==null)
                {
                    for (int i = 0; i < 10000; i++)
                    {
                        res += "梦的翅膀梦的翅膀梦的翅膀梦的翅膀梦的翅膀梦的翅膀梦的翅膀梦的翅膀梦的翅膀梦的翅膀梦的翅膀梦的翅膀梦的翅膀梦的翅膀梦的翅膀梦的翅膀梦的翅膀梦的翅膀梦的翅膀梦的翅膀梦的翅膀";
                    }
                    service.Set<string>("test1", res);
                }
            }
            return Ok($"当前ip：{_iConfiguration["ip"]}，端口：{_iConfiguration["port"]}，res:{res}");//HttpStatusCode--200
        }
    }
}