using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User_Management_System.OutlookSmtpConfigurations
{
    public class EmailSettings
    {
        public string PrimaryDomain { get; set; }
        public int PrimaryPort { get; set; }

        public string UsernameEmail { get; set; }
        public string UsernamePassword { get; set; }

        public string FromEmail { get; set; }
    }
}
