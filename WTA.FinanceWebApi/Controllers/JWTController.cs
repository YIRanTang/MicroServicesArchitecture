using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WTA.FinanceWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JWTController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            List<string> res = new List<string>();
            foreach (var item in base.HttpContext.User.Identities.First().Claims)
            {
                res.Add($"{item.Type}:{item.Value}");
            }
            return new JsonResult(res);
        }
    }
}
