namespace ChatApp;

public class ChatMessage
{
    public string UserName { get; set; }
    public string UserMessage { get; set; }
    public string TimeStamp { get; set; }

    public ChatMessage(string userName, string userMessage)
    {
        UserName = userName;
        UserMessage = userMessage;
        TimeStamp = DateTime.Now.ToString("HH:mm:ss");
    }

    public override string ToString()
    {
        return $"[{TimeStamp}] {UserName}: {UserMessage}";
    }
}