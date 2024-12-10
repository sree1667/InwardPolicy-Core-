using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InwardPolicy.Core.App.Areas.Transaction.Controllers
{
    [Area("Transaction")]
    public class FirePolicyController : Controller
    {
        public IActionResult FirePolicy()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SetToDate(string? fromDate)
        {
            try
            {
                
                if (!DateTime.TryParse(fromDate, out DateTime parsedDate))
                {
                    return Ok(null);
                }
                DateTime today = DateTime.Today;
                DateTime FromDate = Convert.ToDateTime(fromDate);
                DateTime maxDate = today.AddYears(50);
                if (FromDate < today)
                {
                   
                    return Ok(null);
                }
                if (FromDate > maxDate)
                {
                    
                    return Ok(null);
                }
                DateTime toDate = FromDate.AddYears(1).AddDays(-1);
                return Ok(toDate.ToString("d"));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpGet]
        public async Task<IActionResult> ValidateDoB(string? dob)
        {
            try
            {
                if (!DateTime.TryParse(dob, out DateTime parsedDate))
                {
                    return Ok(null); 
                }
                DateTime today = DateTime.Today;
                if (parsedDate > today)
                {
                    return Ok(null); 
                }
                int age = today.Year - parsedDate.Year;
                if (parsedDate.Date > today.AddYears(-age)) age--; 
                if (age < 18 || age > 120)
                {
                    return Ok(null);
                }
                return Ok(parsedDate.ToString("d"));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
