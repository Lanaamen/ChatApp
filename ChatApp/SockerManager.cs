namespace ChatApp;

using SocketIOClient;

public class SocketManager
{
    public static SocketIO _client;
    private static readonly string Path = "/sys25d";
    public static List<ChatMessage> messages = new List<ChatMessage>();

    // Här kan vi välja ett unikt event namn för meddelanden.
    public static readonly string EventMessage = "ChatMessage";
    public static readonly string EventJoined = "Joined";
    public static readonly string EventLeft = "Left";

    // Här ska vi ansluta till socketio servern.
    public static async Task Connect()
    {
        // Vi skapar en instans av SocketIO och konfigurerar den med
        // våran server url och path.
        _client = new SocketIO("wss://api.leetcode.se", new SocketIOOptions
        {
            Path = Path
        });

        // Kod vi kan köra när vi etablerar en anslutning
        _client.OnConnected += (sender, args) => { Console.WriteLine("Connected to the Chitchat!"); };

        // Kod vi kan köra när vi tappar anslutningen
        _client.OnDisconnected += (sender, args) => { Console.WriteLine("Disconnected to the Chitchat!"); };

        _client.On(EventMessage, response =>
        {
            try
            {
                var msg = response.GetValue<ChatMessage>();
                messages.Add(msg);
                Console.WriteLine($"[{msg.TimeStamp}] {msg.UserName}: {msg.UserMessage}");
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
                var msg= new ChatMessage(userName, "has joined the chitchat!");
                messages.Add(msg);
                //Console.WriteLine(msg);
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
                var msg= new ChatMessage(userName, "has left the chitchat!");
                messages.Add(msg);
                Console.WriteLine(msg);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        });
        
        await _client.ConnectAsync();

        // Vi lägger en fördröjning på 2000ms (2s) för att se till att klienten har anslutit och satt upp allt.
        await Task.Delay(1000);
    }

    public static async Task SendMessage(User user, string userMessage)
    {
        var message = new ChatMessage(user.UserName, userMessage);
        await _client.EmitAsync("ChatMessage", message);
    }
}