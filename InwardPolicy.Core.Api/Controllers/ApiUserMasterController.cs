using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer;
using BusinessEntity;
using System.Threading.Tasks;
using System.Data;
using Newtonsoft.Json;

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
        [HttpGet]
        [Route("UserMasterBind")]
        public IActionResult UserMasterBind()
        {
            try
            {
                UserMasterManager objUserMasterManager = new UserMasterManager();
                DataTable dt = objUserMasterManager.UserMasterBind();
                string json = JsonConvert.SerializeObject(dt);
                return Ok(json);
               
            }
            catch (Exception ex)
            {

                throw ex; 
            }
        }
    }
}
