using Microsoft.EntityFrameworkCore;
using System;


public class GameDbContext : DbContext
{

    public DbSet<Score> Scores { get; set; }
    public string DbPath { get; }
    public GameDbContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "game.db");
    }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
     => options.UseSqlite($"Data Source={DbPath}");
}