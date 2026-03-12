using ClassLibrary.Entities;
using Microsoft.EntityFrameworkCore;

public class SecondAppDbContext : DbContext
{
    public SecondAppDbContext(DbContextOptions<SecondAppDbContext> options)
        : base(options)
    {
    }

    // Only declare the DbSets you need from the second database.
    public DbSet<AlbumEntity> Album => Set<AlbumEntity>();
}