using OrderSystemApp.Services.Auth;
using OrderSystemApp.Utils;
using Microsoft.EntityFrameworkCore;
using OrderSystemApp.Data;
using OrderSystemApp.Factories.UserFactory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserFactory, UserFactory>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<SystemContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SystemContext") ?? throw new InvalidOperationException("Connection string 'SystemContext' not found.")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddSession();  // Add session service

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<SystemContext>();
    //context.Database.EnsureCreated();
    //DbInitializer.Initialize(context);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();