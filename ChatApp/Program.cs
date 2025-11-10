using System.ComponentModel;
using System.Net.Sockets;

namespace ChatApp;

using SocketIOClient;

/*
    - [x] Ange användarnamn vid start (validera att det inte är tomt).
    - [x] Ansluta till chatten och se status (ansluten/urkopplad).
    - [x] Skicka och ta emot meddelanden i realtid.
    - [x] Se tidsstämpel, avsändare och meddelandetext för varje meddelande.
    - [ ] Se händelser i chatten, t.ex. när någon joinar eller lämnar.
    - [ ] Avsluta programmet snyggt (koppla ner och städa resurser).
    
   De funktionella krav som kräver arv för G är:
   – Skicka och ta emot meddelanden i realtid
   – Visa tidsstämpel, avsändare och meddelandetext för varje meddelande
   – Visa händelser i chatten, t.ex. när någon ansluter eller lämnar
 */

class Program
{
    static async Task Main(string[] args)
    {
        // Vi ansluter till Socket servern.
        await SocketManager.Connect();
        EventHandler.EventListeners();

        Console.WriteLine("Write your username, be creative!");
        string? inputUser = Console.ReadLine();
        User chitchatUser = new User(inputUser);
        Console.Clear();

        while (true)
        {
            Console.Clear();
            Console.WriteLine($"[-----ChitChat-----]\n");

            foreach (var message in SocketManager.messages)
            {
                Console.WriteLine($"[{message.TimeStamp}] {message.UserName}: {message.UserMessage}");
            }

            Console.Write("\nEnter your message (or 'quit' to exit): ");
            string? input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
                continue;

            if (input.ToLower() == "quit")
                break;
            
            var chatMessage = new ChatMessage(inputUser, input);
            SocketManager.messages.Add(chatMessage);
            await SocketManager.SendMessage(chitchatUser, input);
        }
    }
}