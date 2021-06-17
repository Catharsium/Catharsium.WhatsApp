using System.Collections.Generic;

namespace Catharsium.WhatsApp.Data._Configuration
{
    public class WhatsAppDataSettings
    {
        public string DataFolder { get; set; }
        public Dictionary<string, string> ActiveUsers { get; set; }
    }
}