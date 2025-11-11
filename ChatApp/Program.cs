using System.ComponentModel;
using System.Net.Sockets;

namespace ChatApp;

using SocketIOClient;

/*
    - [x] Ansluta till chatten och se status (ansluten/urkopplad).
    - [x] Skicka och ta emot meddelanden i realtid.
    - [x] Se tidsstämpel, avsändare och meddelandetext för varje meddelande.
    - [x] Se händelser i chatten, t.ex. när någon joinar eller lämnar.
    - [x] Avsluta programmet snyggt (koppla ner och städa resurser).
 */
class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Write your username, be creative!");
        string? inputUser = Console.ReadLine();
        User chitchatUser = new User(inputUser);
        Console.Clear();

        // Vi ansluter till Socket servern.
        await SocketManager.Connect();

        // emita ut en event här för att visa att en användare joinat.
        await SocketManager._client.EmitAsync(SocketManager.EventJoined, new { chitchatUser.UserName });

        while (true)
        {
            SocketManager.ClearChat();
            string? input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
                continue;

            if (input.ToLower() == "quit")
            {
                //emit left-event
                await SocketManager._client.EmitAsync(SocketManager.EventLeft, new { chitchatUser.UserName });
                break;
            }

            var chatMessage = new ChatMessage(inputUser, input);
            SocketManager.messages.Add(chatMessage);
            await SocketManager.SendMessage(chitchatUser, input);
        }
    }
}