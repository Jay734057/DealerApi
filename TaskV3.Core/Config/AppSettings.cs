using System;
using System.Collections.Generic;
using System.Text;

namespace TaskV3.Core.Config
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public int ExpirationInDays { get; set; }
    }
}
