namespace ChatApp;

public class User
{
    public string UserName { get; }

    public User(string userName)
    {
        if (string.IsNullOrEmpty(userName))
            throw new ArgumentException("Your username can't be empty! Choose wisely -.-");

        UserName = userName;
    }
}