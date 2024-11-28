using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class UserMaster
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string CrBy { get; set; }
        public DateTime CrDate { get; set; }
        public string UpBy { get; set; }
        public DateTime UpDate { get; set; }
        public string Active { get; set; }
    }
}
