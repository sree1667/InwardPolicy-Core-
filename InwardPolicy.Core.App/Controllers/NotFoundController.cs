using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InwardPolicy.Core.App.Controllers
{
    public class NotFoundController : Controller
    {
        public IActionResult NotFound()
        {
            return View();
        }
    }
}
