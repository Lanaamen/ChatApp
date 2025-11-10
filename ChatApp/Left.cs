namespace ChatApp;

public class Left : ChatMessage
{    
    public Left(string userName) :base (userName, userName)
    {
       var msg= new ChatMessage(userName, "has left the chitchat!");
       SocketManager.messages.Add(msg);
       Console.WriteLine(msg);
    }
}