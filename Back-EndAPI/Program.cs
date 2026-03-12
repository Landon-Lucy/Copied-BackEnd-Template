using Back_EndAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// REGISTER EF CORE (use SecondConnection instead of DefaultConnection)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("SecondConnection"))
);

// Register only existing application services (add others only if those classes exist)
builder.Services.AddScoped<EmployeeRoleService>();

// If these service classes exist in your project, register them. Otherwise remove or add the missing classes:
// builder.Services.AddScoped<CharacterService>();
// builder.Services.AddScoped<FuntestService>();
// builder.Services.AddScoped<EmployeeService>();
// builder.Services.AddScoped<AlbumService>();

var app = builder.Build();

// TEMP DB TEST (optional)
using (var scope = app.Services.CreateScope())
{
    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var connString = config.GetConnectionString("SecondConnection");
    try
    {
        using var conn = new Npgsql.NpgsqlConnection(connString);
        conn.Open();
        Console.WriteLine("Connected to Postgres (SecondConnection)!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to open DefaultConnection: {ex.Message}");
    }
}

app.UseCors();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty;  // <-- makes Swagger open at root "/"
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
