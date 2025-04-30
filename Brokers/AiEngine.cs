using Microsoft.Extensions.AI;
namespace SubtitleGeneratorWeb.Brokers;
public class AiEngine(IChatClient ChatClient)
{
    public List<ChatMessage> Conversation { get; private set; } = [new(ChatRole.System, "You are a translator who write nothing but basic translation. When I give you an English message you do nothing but to translate it to the selected language.")];
    public IAsyncEnumerable<ChatResponseUpdate> StreamResponseAsync(string message)
    {
        Conversation.Add(new(ChatRole.User, message));
        return ChatClient.GetStreamingResponseAsync(Conversation);
    }
    public async ValueTask<string?> GetResponseAsync(string message)
    {
        Conversation.Add(new(ChatRole.User, message));
        return (await ChatClient.GetResponseAsync(Conversation)).Text?.Trim();
    }
    public async ValueTask<string?> ChangeLanguageAsync(string message)
    {
        Conversation.Add(new(ChatRole.User, $"Translate to {message}"));
        return (await ChatClient.GetResponseAsync(Conversation)).Text?.Trim();
    }
}