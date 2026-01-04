using Microsoft.EntityFrameworkCore;
using MultipleDBSource.Data;
using MultipleDBSource.Helpers;
using MultipleDBSource.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection_1")));

builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IDbConnectionFactory, SQLConnectionFactory>();
builder.Services.AddOpenApi();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Get the connection string for multiple databases and apply the migration to all
    List<string> connectionStrings = [.. builder.Configuration.GetSection("ConnectionStrings").GetChildren().Select(a => a.Value ?? string.Empty)];

    foreach (string connectionString in connectionStrings)
    {
        if(string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidDataException("Found empty connection string, please check once !!");
        }

       DbContextOptionsBuilder<AppDbContext> optionsBuilder = new();
        optionsBuilder.UseSqlServer(connectionString);

        using AppDbContext context = new (optionsBuilder.Options);

        await context.Database.MigrateAsync();
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
