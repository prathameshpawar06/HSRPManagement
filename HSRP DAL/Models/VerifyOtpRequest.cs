using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSRP_DAL.Models
{
    public class VerifyOtpRequest
    {
        public int OtpId { get; set; }
        public string Subject { get; set; }
        public string Purpose { get; set; }
        public string OtpCode { get; set; }
    }
}
