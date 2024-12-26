using BusinessEntity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InwardPolicy.Models
{
    public class FirePolicyRiskModel
    {
        
        public FirePolicyRisk FirePolicyRisk { get; set; }
        public string ApprStatus { get; internal set; }
        public string Mode { get; internal set; }
        public List<SelectListItem> RiskCurrency { get; internal set; }
        public List<SelectListItem> RiskClass { get; internal set; }
    }
}
