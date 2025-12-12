using System.ComponentModel;
using System.Net.Sockets;

namespace ChatApp;
class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Write your username, be creative!");
        string? inputUser = Console.ReadLine();
        
        while (string.IsNullOrWhiteSpace(inputUser))
        {
            Console.WriteLine("Your username can't be empty! Choose wisely -.-");
            inputUser = Console.ReadLine();
        }
        
        User chitchatUser = new User(inputUser);
        Console.Clear();
        
        await SocketManager.Connect();
        await SocketManager._client.EmitAsync(SocketManager.EventJoined, new { chitchatUser.UserName });

        while (true)
        {
            SocketManager.ClearChat();
            string? input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
                continue;

            if (input.ToLower() == "quit")
            {
                await SocketManager._client.EmitAsync(SocketManager.EventLeft, new { chitchatUser.UserName });
                break;
            }

            var chatMessage = new ChatMessage(inputUser, input);
            SocketManager.messages.Add(chatMessage);
            await SocketManager.SendMessage(chitchatUser, input);
        }
    }
}