using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.IO;

public class GameDbContext : DbContext
{
    public DbSet<Player_Data> Players_Data { get; set; }
    public DbSet<MatchPlayer> MatchPlayers { get; set; }
    public DbSet<Match> Matches { get; set; }

    public GameDbContext(DbContextOptions<GameDbContext> options)
        : base(options)
    {
    }

    public static string GetDatabasePath()
    {
        return Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "battleship.db")
        );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!options.IsConfigured)
        {
            var dbPath = GetDatabasePath();
            options.UseSqlite($"Data Source={dbPath}");
        }
    }
}

public class GameDbContextFactory : IDesignTimeDbContextFactory<GameDbContext>
{
    public GameDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<GameDbContext>();
        var dbPath = GameDbContext.GetDatabasePath();
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
        return new GameDbContext(optionsBuilder.Options);
    }
}