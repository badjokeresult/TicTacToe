using Microsoft.EntityFrameworkCore;

using TicTacToe.Models;

namespace TicTacToe.DatabaseContext;

public class AppDbContext: DbContext
{
    public DbSet<Player> Players { get; set; } = null!;
    public DbSet<Game> Games { get; set; } = null!;

    public AppDbContext()
    {
        Database.EnsureCreated();
    }
}
