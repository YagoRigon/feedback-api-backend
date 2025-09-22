using FeedbackApi.Data;
using FeedbackApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// No arquivo Program.cs

// Adicione os serviços de CORS
builder.Services.AddCors(options =>
{
  options.AddDefaultPolicy(
      policy =>
      {
        policy.WithOrigins("http://localhost:4200") 
                .AllowAnyHeader()
                .AllowAnyMethod();
      });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
  // A string de conexão aponta para um arquivo chamado "app.db"
  options.UseSqlite("Data Source=app.db");
});

builder.Services.AddControllers();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddSingleton<EmailService>();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.MapControllers();
app.Run();
