using BusinessEntity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InwardPolicy.Transaction.Models
{
    public class FirePolicyModel
    {
        public FirePolicy FirePolicy { get; set; }
        public FireInwardPolicy FireInwardPolicy { get; set; }

        public string Mode { get; set; }
        public string InwardMode { get; set; }
        
        public string ApprStatus { get; set; }
        public List<SelectListItem> PolCurrencyList { get; set; }
        public List<SelectListItem> PolOccupationList { get; set; }
        public List<SelectListItem> PolProductCodeList { get; set; }
        public List<SelectListItem> PolAssuredTypeList { get; set; }
        public List<SelectListItem> RiskClassList { get;  set; }
        public List<SelectListItem> CedingSourceList { get;  set; }
    }
}
