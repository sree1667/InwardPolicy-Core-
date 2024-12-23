using InwardPolicy.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InwardPolicy_ViewComponent
{
    public class ErrorCodeMasterViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke(ErrorCodeMasterModel model)
        {
            try
            {
                
                return View(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


    }
}
