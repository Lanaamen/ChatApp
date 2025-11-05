namespace ChatApp;

public class ChatMessage
{
    public string UserName { get; set; }
    public string UserMessage { get; set; }
    public DateTime TimeStamp { get; set; }

    public ChatMessage (string userName, string userMessage)
    {
        UserName = userName;
        UserMessage = userMessage;
        TimeStamp = DateTime.Now;
    }
}
