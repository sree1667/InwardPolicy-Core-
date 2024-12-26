using System;
using System.Collections.Generic;
using System.Linq;
using BusinessEntity;
using System.Threading.Tasks;

namespace InwardPolicy.Models
{
    public class DashboardModel
    {
        public string ApprovedPolicy { get; set; }
        public string TotalPolicy { get; set; }
        public string TotalAmount { get; set; }
    }
}
