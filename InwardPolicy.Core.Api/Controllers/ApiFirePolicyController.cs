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
                DataRow dr = objFirePolicyManager.FetchPolicyDetails(uid);
                FirePolicy objfirePolicy = new FirePolicy();


                objfirePolicy.PolNo = dr["POL_NO"].ToString();

                //DateTime fromDate = Convert.ToDateTime(dr["POL_FM_DT"]);
                //txtPolFmDt.Text = fromDate.ToString("dd-MM-yyyy");
                //DateTime toDate = Convert.ToDateTime(dr["POL_TO_DT"]);
                //txtPolToDt.Text = toDate.ToString("dd-MM-yyyy");
                //ddlProductCode.SelectedValue = dr["POL_PROD_CODE"].ToString();
                ////
                //txtAssrName.Text = dr["POL_ASSR_NAME"].ToString();
                //txtAssrAddress.Text = dr["POL_ASSR_ADDRESS"].ToString();
                //txtAssrMobile.Text = dr["POL_ASSR_MOBILE"].ToString();
                //txtAssrEmail.Text = dr["POL_ASSR_EMAIL"].ToString();
                ////
                //DateTime dob = Convert.ToDateTime(dr["POL_ASSR_DOB"]);
                //txtAssrDOB.Text = dob.ToString("dd-MM-yyyy");
                //ddlAssrOccupation.SelectedValue = dr["POL_ASSR_OCCUPATION"].ToString();
                //ddlAssrType.SelectedValue = dr["POL_ASSR_TYPE"].ToString();
                //txtAssrCivilID.Text = dr["POL_ASSR_CIVIL_ID"].ToString();
                ////
                //ddlPolSICurrency.SelectedValue = dr["POL_SI_CURRENCY"].ToString();
                //txtPolSICurrencyRate.Text = dr["POL_SI_CURR_RATE"].ToString();
                //ddlPolPremCurrency.SelectedValue = dr["POL_PREM_CURRENCY"].ToString();
                //txtPolPremCurrencyRate.Text = dr["POL_PREM_CURR_RATE"].ToString();
                ////
                //txtPolFCSI.Text = dr["POL_FC_SI"] != DBNull.Value ? Convert.ToDecimal(dr["POL_FC_SI"]).ToString("N2", new CultureInfo("en-US")) : "0.00";
                //txtPolLCSI.Text = dr["POL_LC_SI"] != DBNull.Value ? Convert.ToDecimal(dr["POL_LC_SI"]).ToString("N2", new CultureInfo("en-US")) : "0.00";
                //txtPolGrossFCPrem.Text = dr["POL_GROSS_FC_PREM"] != DBNull.Value ? Convert.ToDecimal(dr["POL_GROSS_FC_PREM"]).ToString("N2", new CultureInfo("en-US")) : "0.00";
                //txtPolGrossLCPrem.Text = dr["POL_GROSS_LC_PREM"] != DBNull.Value ? Convert.ToDecimal(dr["POL_GROSS_LC_PREM"]).ToString("N2", new CultureInfo("en-US")) : "0.00";
                ////
                //txtPolNetFCPrem.Text = dr["POL_NET_FC_PREM"] != DBNull.Value ? Convert.ToDecimal(dr["POL_NET_FC_PREM"]).ToString("N2", new CultureInfo("en-US")) : "0.00";
                //txtPolNetLCPrem.Text = dr["POL_NET_LC_PREM"] != DBNull.Value ? Convert.ToDecimal(dr["POL_NET_LC_PREM"]).ToString("N2", new CultureInfo("en-US")) : "0.00";
                //txtPolVATFCAmt.Text = dr["POL_VAT_FC_AMT"] != DBNull.Value ? Convert.ToDecimal(dr["POL_VAT_FC_AMT"]).ToString("N2", new CultureInfo("en-US")) : "0.00";
                //txtPolVATLCAmt.Text = dr["POL_VAT_LC_AMT"] != DBNull.Value ? Convert.ToDecimal(dr["POL_VAT_LC_AMT"]).ToString("N2", new CultureInfo("en-US")) : "0.00";
                // Assuming objFirePolicy is your object

                //Binding DateTime fields
                string fmdt= Convert.ToDateTime( dr["POL_FM_DT"]).ToString("yyyy-MM-dd");
                objfirePolicy.PolFmDt = fmdt != null ? Convert.ToDateTime(fmdt) : DateTime.MinValue;
                objfirePolicy.PolToDt = dr["POL_TO_DT"] != DBNull.Value ? Convert.ToDateTime(dr["POL_TO_DT"]) : DateTime.MinValue;
                objfirePolicy.PolIssDt = Convert.ToDateTime(dr["POL_ISS_DT"]);
                objfirePolicy.PolAssrDob = dr["POL_ASSR_DOB"] != DBNull.Value ? Convert.ToDateTime(dr["POL_ASSR_DOB"]) : DateTime.MinValue;



                // Binding string fields
                objfirePolicy.PolProdCode = dr["POL_PROD_CODE"] != DBNull.Value ? dr["POL_PROD_CODE"].ToString() : string.Empty;
                objfirePolicy.PolAssrName = dr["POL_ASSR_NAME"] != DBNull.Value ? dr["POL_ASSR_NAME"].ToString() : string.Empty;
                objfirePolicy.PolAssrAddress = dr["POL_ASSR_ADDRESS"] != DBNull.Value ? dr["POL_ASSR_ADDRESS"].ToString() : string.Empty;
                objfirePolicy.PolAssrMobile = dr["POL_ASSR_MOBILE"] != DBNull.Value ? dr["POL_ASSR_MOBILE"].ToString() : string.Empty;
                objfirePolicy.PolAssrEmail = dr["POL_ASSR_EMAIL"] != DBNull.Value ? dr["POL_ASSR_EMAIL"].ToString() : string.Empty;

                // Binding DateTime for Assured's DOB


                objfirePolicy.PolAssrOccupation = dr["POL_ASSR_OCCUPATION"] != DBNull.Value ? dr["POL_ASSR_OCCUPATION"].ToString() : string.Empty;
                objfirePolicy.PolAssrType = dr["POL_ASSR_TYPE"] != DBNull.Value ? dr["POL_ASSR_TYPE"].ToString() : string.Empty;
                objfirePolicy.PolAssrCivilId = dr["POL_ASSR_CIVIL_ID"] != DBNull.Value ? dr["POL_ASSR_CIVIL_ID"].ToString() : string.Empty;
                // Binding currency fields
                objfirePolicy.PolSICurrency = dr["POL_SI_CURRENCY"] != DBNull.Value ? dr["POL_SI_CURRENCY"].ToString() : string.Empty;
                objfirePolicy.PolPremCurrency = dr["POL_PREM_CURRENCY"] != DBNull.Value ? dr["POL_PREM_CURRENCY"].ToString() : string.Empty;
                objfirePolicy.PolSICurrencyRate = dr["POL_SI_CURR_RATE"] != DBNull.Value ? Convert.ToDouble(dr["POL_SI_CURR_RATE"]) : 0.00D;
                objfirePolicy.PolPremCurrencyRate = dr["POL_PREM_CURR_RATE"] != DBNull.Value ? Convert.ToDouble(dr["POL_PREM_CURR_RATE"]) : 0.00D;


                // Binding monetary fields with conditional checks for DBNull and formatting decimal
                objfirePolicy.PolFcSi = dr["POL_FC_SI"] != DBNull.Value ? Convert.ToDouble(dr["POL_FC_SI"]) : 0.00;
                objfirePolicy.PolLcSi = dr["POL_LC_SI"] != DBNull.Value ? Convert.ToDouble(dr["POL_LC_SI"]) : 0.00;
                objfirePolicy.PolGrossFcPrem = dr["POL_GROSS_FC_PREM"] != DBNull.Value ? Convert.ToDouble(dr["POL_GROSS_FC_PREM"]) : 0.00;
                objfirePolicy.PolGrossLcPrem = dr["POL_GROSS_LC_PREM"] != DBNull.Value ? Convert.ToDouble(dr["POL_GROSS_LC_PREM"]) : 0.00;
                objfirePolicy.PolNetFcPrem = dr["POL_NET_FC_PREM"] != DBNull.Value ? Convert.ToDouble(dr["POL_NET_FC_PREM"]) : 0.00;
                objfirePolicy.PolNetLcPrem = dr["POL_NET_LC_PREM"] != DBNull.Value ? Convert.ToDouble(dr["POL_NET_LC_PREM"]) : 0.00;
                objfirePolicy.PolVatFcAmt = dr["POL_VAT_FC_AMT"] != DBNull.Value ? Convert.ToDouble(dr["POL_VAT_FC_AMT"]) : 0.00;
                objfirePolicy.PolVatLcAmt = dr["POL_VAT_LC_AMT"] != DBNull.Value ? Convert.ToDouble(dr["POL_VAT_LC_AMT"]) : 0.00;



                string json = JsonConvert.SerializeObject(objfirePolicy);
                return Ok(json);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("AddFirePolicy/{mode?}")]
        public IActionResult AddFirePolicy(FirePolicy objFirePolicy, string mode)
        {
            FirePolicyManager objFirePolicyManager = new FirePolicyManager();
            string uid = objFirePolicyManager.AddFirePolicy(objFirePolicy, mode);
            return Ok(uid);
        }
    }

}
