using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer;
using BusinessEntity;
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
        [HttpPost]
        [Route("CheckUserId/{UserId?}")]
        public IActionResult CheckUserId(string UserId, string Password)
        {
            UserMasterManager objUserMasterManager = new UserMasterManager();
            bool status = objUserMasterManager.CheckUserId(UserId);
            return Ok(status);
        }

        [HttpPost]
        [Route("UserMasterInsert/{mode?}")]
        public IActionResult UserMasterInsert(UserMaster objUserMaster,string mode)
        {
            UserMasterManager objUserMasterManager = new UserMasterManager();
            bool InsertStatus = objUserMasterManager.InsertUserMaster(objUserMaster, mode);
            return Ok(InsertStatus);
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
        [HttpDelete]
        [Route("DeleteUserMaster/{userId?}")]
        public IActionResult DeleteUserMaster(string userId)
        {
            try
            {
                UserMasterManager objUserMasterManager = new UserMasterManager();
                bool status = objUserMasterManager.DeleteUserMaster(userId);
                string json = JsonConvert.SerializeObject(status);
                return Ok(json);
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }
        
        [HttpGet]
        [Route("GetUserMasterDetails/{userId?}")]
        public IActionResult GetUserMasterDetails(string userId)
        {
            try
            {
                UserMasterManager objUserMasterManager = new UserMasterManager();
                DataTable dt = objUserMasterManager.GetUserDetails(userId);
                UserMaster objUserMaster = new UserMaster();
                objUserMaster.UserId = dt.Rows[0]["USER_ID"].ToString();
                objUserMaster.Password = dt.Rows[0]["USER_PASSWORD"].ToString();              
                objUserMaster.UserName =dt.Rows[0]["USER_NAME"].ToString();
                objUserMaster.Active = dt.Rows[0]["USER_ACTIVE_YN"].ToString();
                return Ok(objUserMaster);
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }
    }
}
