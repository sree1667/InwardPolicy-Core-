using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer;
using BusinessEntity;
using System.Threading.Tasks;

namespace InwardPolicy.Core.Api.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class ApiUserMasterController : ControllerBase
    {
        [HttpPost]
        [Route("CheckLogin/{UserId?}/{Password?}")]
        public IActionResult CheckLogin(string UserId, string Password)
        {
            UserMasterManager objUserMasterManager = new UserMasterManager();
            bool loginStatus = objUserMasterManager.CheckLogin(UserId, Password);
            return Ok(loginStatus);
        }
    }
}
