using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class CodesMaster
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public double Value { get; set; }
        public string CrBy { get; set; }
        public DateTime CrDate { get; set; }
        public string UpBy { get; set; }
        public DateTime UpDate { get; set; }
        public string Active { get; set; }

        public bool IsActiveYN

        {

            get => this.Active == "Y";

            set => this.Active = (value) ? "Y" : "N";

        }
        public string UpOrCrBy { get; set; }
    }
}
