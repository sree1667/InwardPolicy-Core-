using InwardPolicy.BusinessEntity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InwardPolicy.Models
{
    public class ErrorCodeMasterModel
    {
        public ErrorCodeMaster ErrorCodeMaster {get; set;}
        public string Mode {get; set;}
        public List<SelectListItem> ErrTypeList { get; set; }
    }
}
