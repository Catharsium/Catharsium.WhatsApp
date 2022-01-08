using Catharsium.Util.IO.Interfaces;
namespace Catharsium.WhatsApp.Terminal.Models;

public class Conversation
{
    public string Name { get; set; }

    public List<IFile> EportFiles { get; set; }

    public List<Message> Messages { get; set; }


    public override string ToString()
    {
        return $"{this.Name.Replace("WhatsApp Chat with ", "")} ({this.EportFiles.Count} files)";
    }
}