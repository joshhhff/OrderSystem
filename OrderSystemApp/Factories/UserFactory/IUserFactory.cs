using OrderSystemApp.Models;

namespace OrderSystemApp.Factories.UserFactory;

public interface IUserFactory
{
    public User Create(string email, string password);
}
