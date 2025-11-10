namespace ChatApp;

public class Joined : ChatMessage
{
    
    public Joined(string userName) :base (userName, userName)
    {
        Console.WriteLine($"{userName} has joined the chitchat!");
    }
}