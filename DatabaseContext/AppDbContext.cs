using Microsoft.EntityFrameworkCore;
using TicTacToe.DatabaseContext.Converters;
using TicTacToe.Models;

namespace TicTacToe.DatabaseContext;

public class AppDbContext: DbContext
{
    public DbSet<Player> Players { get; set; } = null!;
    public DbSet<Game> Games { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var converter = new CellsArrayToStringConverter();

        modelBuilder
            .Entity<Player>()
            .Property(p => p.Cells)
            .HasConversion(converter);
    }
}
