using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;


public class GameDbContext : DbContext
{

    public DbSet<Score> Scores { get; set; }
    public string DbPath { get; }

    public GameDbContext(DbContextOptions<GameDbContext> options)
        : base(options)
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!options.IsConfigured)
        {
            options.UseSqlite("Data Source=battleship.db");
        }
    }
}

public class GameDbContextFactory : IDesignTimeDbContextFactory<GameDbContext>
{
    public GameDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<GameDbContext>();
        optionsBuilder.UseSqlite("Data Source=battleship.db");
        return new GameDbContext(optionsBuilder.Options);
    }
}