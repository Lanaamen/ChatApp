namespace ChatApp;
using SocketIOClient;

public class SocketManager
{
    public static SocketIO _client;
    private static readonly string Path = "/sys25d";
    public static List<ChatMessage> messages = new List<ChatMessage>();
    
    public static readonly string EventMessage = "ChatMessage";
    public static readonly string EventJoined = "Joined";
    public static readonly string EventLeft = "Left";

    public static async Task Connect()
    {
        _client = new SocketIO("wss://api.leetcode.se", new SocketIOOptions
        {
            Path = Path
        });

        _client.OnConnected += (sender, args) => { Console.WriteLine("Connected to the Chitchat!"); };
        _client.OnDisconnected += (sender, args) => { Console.WriteLine("Disconnected to the Chitchat!"); };

        _client.On(EventMessage, response =>
        {
            try
            {
                var msg = response.GetValue<ChatMessage>();
                messages.Add(msg);
                ClearChat();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        });

        _client.On(EventJoined, response =>
        {
            try
            {
                var userName = response.GetValue<Joined>().UserName;
                var msg= new ChatMessage(userName, "has joined the Chitchat!");
                messages.Add(msg);
                ClearChat();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        });

        _client.On(EventLeft, response =>
        {
            try
            {
                var userName = response.GetValue<Left>().UserName;
                var msg= new ChatMessage(userName, "has left the Chitchat!");
                messages.Add(msg);
                ClearChat();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        });
        
        await _client.ConnectAsync();
        await Task.Delay(1000);
    }
    
    public static void ClearChat()
    {
        Console.Clear();
        Console.WriteLine($"[-----ChitChat-----]\n");
            
        foreach (var message in SocketManager.messages)
        {
            Console.WriteLine($"[{message.TimeStamp}] {message.UserName}: {message.UserMessage}");
        }

        Console.Write($"\r");
        Console.Write("\nEnter your message (or 'quit' to exit): ");
    }

    public static async Task SendMessage(User user, string userMessage)
    {
        var message = new ChatMessage(user.UserName, userMessage);
        await _client.EmitAsync("ChatMessage", message);
        Console.WriteLine(message);
    }
}