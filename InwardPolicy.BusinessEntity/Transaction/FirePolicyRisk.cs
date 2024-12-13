using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class FirePolicyRisk
    {
        public string RiskUid { get; set; }

        public string RiskPolUid { get; set; }
        public string RiskClass { get; set; }
        public string RiskDesc { get; set; }
        public string RiskSICurr { get; set; }
        public string RiskPremCurr { get; set; }
        public double RiskFcSi { get; set; }
        public double RiskLcSi { get; set; }
        public double RiskPremRate { get; set; }
        public double RiskFcPrem { get; set; }
        public double RiskLcPrem { get; set; }
        public string RiskCrBy { get; set; }
        public string RiskUpBy { get; set; }


    }
}
