namespace Catharsium.WhatsApp.Data.Interfaces;

public interface IMessageAnalyzer
{
    List<string> GetCharacters(string text);
    List<char> GetEmoticons(string text);
    List<string> GetUrls(string text);
    List<string> GetWords(string text);
}