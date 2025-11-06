using System.ComponentModel;

namespace ChatApp;

using SocketIOClient;

/*
    - [] Ange användarnamn vid start (validera att det inte är tomt).
    - [x] Ansluta till chatten och se status (ansluten/urkopplad).
    - [x] Skicka och ta emot meddelanden i realtid.
    - [x] Se tidsstämpel, avsändare och meddelandetext för varje meddelande.
    - [ ] Se händelser i chatten, t.ex. när någon joinar eller lämnar.
    - [ ] Avsluta programmet snyggt (koppla ner och städa resurser).
 */

class Program
{
    static async Task Main(string[] args)
    {
        // Vi ansluter till Socket servern.
        await SocketManager.Connect();

        Console.WriteLine("Write your username, be creative!");
        string? inputUser = Console.ReadLine();
        User chitchatUser = new User(inputUser);
        Console.Clear();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("---Chitchat---\n");

            foreach (var message in SocketManager.messages)
            {
                Console.WriteLine($"[{message.TimeStamp}] {message.UserName}: {message.UserMessage}");
            }

            Console.Write("\nEnter your message (or 'quit' to exit): ");
            string? input = Console.ReadLine();

            var chatMessage = new ChatMessage(inputUser, input);
            SocketManager.messages.Add(chatMessage);

            if (string.IsNullOrEmpty(input))
                continue;

            if (input.ToLower() == "quit")
                break;

            await SocketManager.SendMessage(chitchatUser, input);
        }
    }
}