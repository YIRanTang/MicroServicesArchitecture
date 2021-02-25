using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace WTA.AuthenticationCenter.Controllers
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
    }
}
