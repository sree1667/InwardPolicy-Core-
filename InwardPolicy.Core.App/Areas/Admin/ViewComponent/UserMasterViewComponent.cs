using InwardPolicy.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InwardPolicy_ViewComponent
{
    public class UserMasterViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(UserMasterModel model)
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
