namespace ChatApp;

public class Joined : ChatMessage
{
    public Joined(string userName) :base (userName, $"{userName} has joined the Chitchat!")
    {
       // var msg= new ChatMessage(userName, "has joined the chitchat!");
       // SocketManager.messages.Add(msg);
       // Console.WriteLine(msg);
    }
}
