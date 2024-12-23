using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InwardPolicy.BusinessEntity
{
    public class ErrorCodeMaster
    {
        public string Code { get; set; }
        public string Type { get; set; }
        public string Desc { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpOrCrBy { get; set; }
        
    }
}
