namespace OrderSystemApp.Models;

public class User
{
    public int ID { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
}