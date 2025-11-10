namespace ChatApp;
public class EventHandler : SocketManager
{
    // Här nedan anger vi de events vi vill lyssna på                                           
    // samt en handler för varje event som ska köras.

    public static void EventListeners()
    {
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
                var msg = response.GetValue<Joined>();
                messages.Add(msg);
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
                var msg = response.GetValue<Left>();
                messages.Add(msg);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        });

        
    }
}