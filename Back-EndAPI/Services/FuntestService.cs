using ClassLibrary.DTOs;
using ClassLibrary.Entities;
using Microsoft.EntityFrameworkCore;

//
// SERVICE ROLE
// -------------
// Services contain BUSINESS LOGIC and DATA ACCESS.
// Controllers should never talk directly to the database.
//
//
// Services:
// - Decide WHAT data to fetch
// - Decide HOW data is shaped
// - Return DTOs (safe for UI)
//
// This keeps controllers simple and testable.
//

public class FuntestService
{
    // Database context injected via Dependency Injection
    private readonly AppDbContext _db;

    public FuntestService(AppDbContext db)
    {
        _db = db;
    }

    // Returns characters as DTOs (not entities)
    public async Task<List<FuntestDTO>> GetFuntestsAsync()
    {
        // Query the database and PROJECT directly into DTOs
        // EF Core generates optimized SQL that selects only needed columns
        return await _db.Funtest
            .Select(e => new FuntestDTO
            {
                Id = e.Id,
                Name = e.Name,
                Info = e.Info
            })
            .ToListAsync();
    }

    public async Task<FuntestDTO> PostFuntestAsync(FuntestDTO newFuntest)
    {

        var entity = new FuntestEntity
        {
            Name = newFuntest.Name,
            Info = newFuntest.Info
        };

        _db.Funtest.Add(entity);
        await _db.SaveChangesAsync();


        return new FuntestDTO
        {
            Id = entity.Id,
            Name = entity.Name,
            Info = entity.Info
        };
    }

    public async Task<FuntestDTO?> GetFuntestByIdAsync(Guid id)
    {
        var entity = await _db.Funtest.FindAsync(id);
        if (entity == null) return null;

        return new FuntestDTO
        {
            Id = entity.Id,
            Name = entity.Name,
            Info = entity.Info
        };
    }

    public async Task<FuntestDTO> PutFuntestAsync(FuntestDTO funtest)
    {
        var entity = await _db.Funtest.FindAsync(funtest.Id);

        if (entity == null) return null;

        entity.Name = funtest.Name;
        entity.Info = funtest.Info;

        await _db.SaveChangesAsync();

        return new FuntestDTO
        {
            Id = entity.Id,
            Name = entity.Name,
            Info = entity.Info
        };
    }

    public async Task<bool> DeleteFuntestAsync(Guid id)
    {
        var entity = await _db.Funtest.FindAsync(id);
        if (entity == null) return false;

        _db.Funtest.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }







    // Example: SAME DATA, but using raw SQL instead of EF LINQ
    // This is for learning / reference purposes
    public async Task<List<CharacterDTO>> GetCharactersWithSqlAsync()
    {
        var results = new List<CharacterDTO>();

        // Get the raw database connection EF is using
        using var conn = _db.Database.GetDbConnection();
        await conn.OpenAsync();

        // Create a SQL command
        using var cmd = conn.CreateCommand();
        cmd.CommandText = """
            SELECT id, name, class, level, health, mana
            FROM character
        """;

        // Execute query and read results
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            results.Add(new CharacterDTO
            {
                Id = reader.GetGuid(0),
                Name = reader.GetString(1),
                Class = reader.GetString(2),
                Level = reader.GetInt32(3),
                Health = reader.GetInt32(4),
                Mana = reader.GetInt32(5)
            });
        }

        return results;
    }
}
