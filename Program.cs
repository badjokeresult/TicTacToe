using Microsoft.EntityFrameworkCore;
using TicTacToe.DatabaseContext;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders().AddConsole();

builder.Services.AddControllers();

var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(connection));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();