using System.Diagnostics;
using OrderSystemApp.Models;
using OrderSystemApp.Data;

namespace OrderSystemApp.Data;

public static class DbInitializer
{
    public static void Initialize(SystemContext context)
    {
        // Seed role data
        var roles = new Role[]
        {
            new (){RoleName="User"},
            new (){RoleName="Administrator"}
        };
        context.Role.AddRange(roles);
        context.SaveChanges();
    }
}
