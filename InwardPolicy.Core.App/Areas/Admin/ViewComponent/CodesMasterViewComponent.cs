using Microsoft.AspNetCore.Mvc;
using System;
using InwardPolicy.Admin.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InwardPolicy_ViewComponent
{
    public class CodesMasterViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(CodesMasterModel model)
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
