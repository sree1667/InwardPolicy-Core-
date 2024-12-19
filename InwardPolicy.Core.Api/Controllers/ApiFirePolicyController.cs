using Microsoft.AspNetCore.Mvc;
using System;
using BusinessLayer;
using BusinessEntity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Newtonsoft.Json;
using System.Globalization;

namespace InwardPolicy.Core.Api.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class ApiFirePolicyController : ControllerBase
    {
        [HttpGet]
        [Route("LoadDashboardData")]
        public IActionResult LoadDashboardData()
        {
            try
            {
                FirePolicyManager objFirePolicyManager = new FirePolicyManager();
                DataTable dt = objFirePolicyManager.GetCounterValues();
                string json = JsonConvert.SerializeObject(dt);
                return Ok(json);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        [Route("GetPolicyCount")]
        public int[] GetPolicyCount()
        {
            try
            {
                FirePolicyManager objFirePolicyManager = new FirePolicyManager();
                int[] policyCount = objFirePolicyManager.GetPolicyCount();
                return policyCount;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpGet]
        [Route("ApproveFirePolicy/{polUid?}/{apprBy?}")]
        public IActionResult ApproveFirePolicy(string polUid,string apprBy)
        {
            try
            {
                FirePolicyManager objFirePolicyManager = new FirePolicyManager();
                int status = objFirePolicyManager.ApprovePolicy(polUid, apprBy);
                return Ok(status);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("CopyPolicy/{polUid?}/{createdBy?}")]
        public IActionResult CopyPolicy(string polUid,string createdBy)
        {
            try
            {
                FirePolicyManager objFirePolicyManager = new FirePolicyManager();
                long status = objFirePolicyManager.CopyPolicy(polUid, createdBy);
                return Ok(status);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("FirePolicyBind")]
        public IActionResult FirePolicyBind()
        {
            try
            {
                FirePolicyManager objFirePolicyManager = new FirePolicyManager();
                DataTable dt = objFirePolicyManager.BindGrid();
                string json = JsonConvert.SerializeObject(dt);
                return Ok(json);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpGet]
        [Route("LoadControl/{uid}")]
        public IActionResult LoadControl(string uid)
        {
            try
            {
                FirePolicyManager objFirePolicyManager = new FirePolicyManager();
                FirePolicy objfirePolicy = objFirePolicyManager.FetchPolicyDetails(uid);
                string json = JsonConvert.SerializeObject(objfirePolicy);
                return Ok(json);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpGet]
        [Route("CheckApprovalStatus/{uid?}")]
        public IActionResult CheckApprovalStatus(string uid)
        {
            try
            {
                FirePolicyManager objFirePolicyManager = new FirePolicyManager();
                bool apprStatus = objFirePolicyManager.CheckApprstatus(uid);
                return Ok(apprStatus);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //[HttpGet]
        //[Route("LoadInwardControl/{uid}")]
        //public IActionResult LoadInwardControl(string uid)
        //{
        //    try
        //    {
        //        FireInwardPolicyManager objFireInwardPolicyManager = new FireInwardPolicyManager();
        //        FireInwardPolicy objfireInwardPolicy = objFireInwardPolicyManager.FetchInwardDetails(uid);
        //        string json = JsonConvert.SerializeObject(objfireInwardPolicy);
        //        return Ok(json);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        [HttpPost]
        [Route("AddFirePolicy/{mode?}")]
        public IActionResult AddFirePolicy(FirePolicy objFirePolicy, string mode)
        {
            FirePolicyManager objFirePolicyManager = new FirePolicyManager();
            string uid = objFirePolicyManager.AddFirePolicy(objFirePolicy, mode);
            return Ok(uid);
        }
        [HttpGet]
        [Route("FillAll/{polUid?}")]
        public IActionResult FillAll(string polUid)
        {
            FirePolicyManager objFirePolicyManager = new FirePolicyManager();
            string[] json = objFirePolicyManager.GetRate(polUid);
            return Ok(json);
        }
    }

}
