using OrderSystemApp.Models;

namespace OrderSystemApp.Factories.UserFactory;

public sealed class UserFactory : IUserFactory
{
    public User Create(string email, string password)
    {
        return new()
        {
            Email = email,
            Password = password,
        };
    }
}
