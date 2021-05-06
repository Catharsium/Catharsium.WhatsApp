using Catharsium.WhatsApp.Entities.Models;
using System.Collections.Generic;

namespace Catharsium.WhatsApp.Data._Configuration
{
    public class WhatsAppDataSettings
    {
        public string DataFolder { get; set; }
        public List<User> Users { get; set; }
    }
}