using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer;
using System.Threading.Tasks;

namespace InwardPolicy.Core.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApiUserMasterController : Controller
    {
        [HttpGet]
        public IActionResult CheckLogin(string userId,string password)
        {
            UserMasterManager objUserMasterManager = new UserMasterManager();
            bool loginStatus = objUserMasterManager.CheckLogin(userId, password);
            return Ok(loginStatus);
        }
    }
}
