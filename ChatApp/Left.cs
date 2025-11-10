namespace ChatApp;

public class Left : ChatMessage
{    
    public Left(string userName) :base (userName, userName)
    {
        Console.WriteLine($"{userName} has left the chitchat!");
    }
}