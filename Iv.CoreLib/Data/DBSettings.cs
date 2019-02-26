using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Iv.Data
{
    public class DBSettings
    {

        public DBSettings()
        {
            DBServer = string.Empty;
            DBName = string.Empty;
            IsIntegratedAuth = false;
            UserName = string.Empty;
            Password = string.Empty;
        }

        public string DBServer { get; set; }
        public string DBName { get; set; }
        public bool IsIntegratedAuth { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

    }
}
