namespace Catharsium.WhatsApp.Data._Configuration;

public class WhatsAppDataSettings
{
    public string ExportFilesFolder { get; set; }
    public string ConversationsFolder { get; set; }
    public Dictionary<string, string> ActiveUsers { get; set; }
}