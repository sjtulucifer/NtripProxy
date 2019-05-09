using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;

namespace NtripForward.ConfigNode
{
    class ProxySetting : ConfigurationSection
    {
        [ConfigurationProperty("localPort", IsRequired = false, DefaultValue = 8003)]
        public int LocalPort
        {
            get { return (int)base["localPort"]; }
            set { base["localPort"] = value; }
        }

        [ConfigurationProperty("remotePort", IsRequired = false, DefaultValue = 8003)]
        public int RemotePort
        {
            get { return (int)base["remotePort"]; }
            set { base["remotePort"] = value; }
        }

        [ConfigurationProperty("remoteIP", IsRequired = false, DefaultValue = "60.205.8.49")]
        public string RemoteIP
        {
            get { return (string)base["remoteIP"]; }
            set { base["remoteIP"] = value; }
        }

        [ConfigurationProperty("timeout", IsRequired = false, DefaultValue = 60000)]
        public int Timeout
        {
            get { return (int)base["timeout"]; }
            set { base["timeout"] = value; }
        }

        [ConfigurationProperty("ggaRepeatRate", IsRequired = false, DefaultValue = 3000)]
        public int GGARepeatRate
        {
            get { return (int)base["ggaRepeatRate"]; }
            set { base["ggaRepeatRate"] = value; }
        }

        [ConfigurationProperty("ggaRecordRate", IsRequired = false, DefaultValue = 5)]
        public int GGARecordRate
        {
            get { return (int)base["ggaRecordRate"]; }
            set { base["ggaRecordRate"] = value; }
        }

        [ConfigurationProperty("enableLogGGA", IsRequired = false, DefaultValue = false)]
        public bool EnableLogGGA
        {
            get { return (bool)base["enableLogGGA"]; }
            set { base["enableLogGGA"] = value; }
        }

        [ConfigurationProperty("enabled", IsRequired = false, DefaultValue = true)]
        public bool Enable
        {
            get { return (bool)base["enabled"]; }
            set { base["enabled"] = value; }
        }
    }
}
