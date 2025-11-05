namespace ChatApp;

using SocketIOClient;

/*
    - [ ] Ange användarnamn vid start (validera att det inte är tomt).
   - [ ] Ansluta till chatten och se status (ansluten/urkopplad).
   - [ ] Skicka och ta emot meddelanden i realtid.
   - [ ] Se tidsstämpel, avsändare och meddelandetext för varje meddelande.
   - [ ] Se händelser i chatten, t.ex. när någon joinar eller lämnar.
   - [ ] Avsluta programmet snyggt (koppla ner och städa resurser).
 */

class Program
{
    private static SocketIO _client;
    private static bool _isConnected = false;
    
    static async Task Main(string[] args)
    {
        _client = new SocketIO("wss://api.leetcode.se", new SocketIOOptions
        {
            Path = "/sys25d"
        });
        
        
        _client.OnConnected += async (sender, eventArgs) => 
        {
            Console.WriteLine("Connected to the ChitChat!");
            _isConnected = true;
        };
        
        _client.OnDisconnected += async (sender, eventArgs) => 
        {
            Console.WriteLine("Disconnected to the ChitChat.");
            _isConnected = false;
        };
        
        try
        {
            await _client.ConnectAsync();    
            
            await Task.Delay(1000);

            if (!_isConnected)
            {
                Console.WriteLine("Not connected to the server.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Connection failed: {e.Message}");
            throw;
        }
        
        Console.WriteLine("Write your username, be creative!");
        string? inputUser = Console.ReadLine();
        User chitchatUser = new User(inputUser);
                
        Console.WriteLine($"Great choice {chitchatUser.UserName}");
        Console.WriteLine("Start chitchatting by tying messages or type 'quit' to exit!");
        

        _client.On("ChatMessage", response =>
        {
            Console.WriteLine("Received ChatMessage event.");
            try
            {
                var msg = response.GetValue<ChatMessage>();
                var time = msg.TimeStamp == default ? DateTime.Now : msg.TimeStamp;
                var user = string.IsNullOrEmpty(msg.UserName) ? "Unknown Chitchatter" : msg.UserName;
                Console.WriteLine($"[{time:T}] {user}: {msg.UserMessage}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error parsing message: {e.Message}");
                Console.WriteLine($"Raw response: {response}");
            }
        });

        
        while (true)
        {
            if (_client.Connected)
            {
                string? input = Console.ReadLine();
                
                if(string.IsNullOrEmpty(input)) continue;

                if (input.ToLower() == "quit") break;
                
                try
                {
                    await SendMessage(chitchatUser, input);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to send message: {e.Message}");
                }
            }
            else
            {
                Console.WriteLine("Not connected to the server. Attempting to reconnect...");
                try
                {
                    await _client.ConnectAsync();
                    await Task.Delay(1000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Reconnection failed: {ex.Message}");
                    await Task.Delay(5000);
                }
            }
        }
        await _client.DisconnectAsync();
    }

    static async Task SendMessage(User user, string userMessage)
    {
        var message = new ChatMessage(user.UserName, userMessage);
        await _client.EmitAsync("ChatMessage", message);
    }
    
}
