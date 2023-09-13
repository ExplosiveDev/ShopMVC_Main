using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Helpers
{
    public class MailData
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string EmailFrom { get; set; }
        public string Password { get; set; }
    }
}
