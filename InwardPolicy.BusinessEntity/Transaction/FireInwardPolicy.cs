using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class FireInwardPolicy
    {
        public string InwPolUid { get; set; }
        public int InwUid { get; set; }
        public string InwOrgPolNo { get; set; }
        public string InwCedingSource { get; set; }
        public string InwRiskClass { get; set; }
        public string InwSiCurr { get; set; }
        public double InwOrgSiFc { get; set; }
        public double InwOrgSiLc { get; set; }
        public string InwPremCurr { get; set; }
        public double InwOrgPremFc { get; set; }
        public double InwOrgPremLc { get; set; }
        public string InwOrgInsName { get; set; }
        public double InwSharePerc { get; set; }
        public double InwSiShareFc { get; set; }
        public double InwSiShareLc { get; set; }
        public double InwPremShareFc { get; set; }
        public double InwPremShareLc { get; set; }
        public double InwCommPerc { get; set; }
        public double InwCommLcAmt { get; set; }
        public double InwCommFcAmt { get; set; }
        public string InwCrBy { get; set; }
        public DateTime? InwCrDt { get; set; }
        public string InwUpBy { get; set; }
        public DateTime InwUpDt { get; set; }
        public object InwRiskId { get; set; }
    }
}
