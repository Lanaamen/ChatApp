namespace ChatApp;

public class Left : ChatMessage
{    
    public Left(string userName) :base (userName, $"{userName} has left the Chitchat!")
    {

    }
}