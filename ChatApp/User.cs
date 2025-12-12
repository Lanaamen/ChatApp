namespace ChatApp;

public class User
{
    public string UserName { get; }

    public User(string userName)
    {
        while (string.IsNullOrWhiteSpace(userName))
        {
            Console.WriteLine("Your username can't be empty! Choose wisely -.-");
            userName = Console.ReadLine();
        }

        UserName = userName;
    }
}