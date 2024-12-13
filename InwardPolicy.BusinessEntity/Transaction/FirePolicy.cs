using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class FirePolicy
    {
        public string PolNo { get; set; }
        public int InwCount { get; set; }
        public DateTime? PolFmDt { get; set; }
        public DateTime? PolIssDt { get; set; }
        public DateTime? PolToDt { get; set; }
        public string PolProdCode { get; set; }
        public string PolAssrName { get; set; }
        public string PolAssrAddress { get; set; }
        public string PolAssrMobile { get; set; }
        public string PolAssrEmail { get; set; }
        public DateTime? PolAssrDob { get; set; }
        public string PolAssrOccupation { get; set; }
        public string PolAssrType { get; set; }
        public string PolAssrCivilId { get; set; }
        public string PolSICurrency { get; set; }
        public double PolSICurrencyRate { get; set; }
        public double PolFcSi { get; set; }
        public double PolLcSi { get; set; }
        public string PolPremCurrency { get; set; }
        public double PolPremCurrencyRate { get; set; }
        public double PolGrossFcPrem { get; set; }
        public double PolGrossLcPrem { get; set; }
        public double PolNetFcPrem { get; set; }
        public double PolNetLcPrem { get; set; }
        public double PolVatFcAmt { get; set; }
        public double PolVatLcAmt { get; set; }
        public int Poluid { get; set; }
        public string PolApprStatus { get; set; }
        public string CrBy { get; set; }
        public string UpBy { get; set; }
        public string CrOrUpBy { get; set; }
    }
}
