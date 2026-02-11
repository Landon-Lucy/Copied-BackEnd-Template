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

public class EmployeeService
{
    // Database context injected via Dependency Injection
    private readonly AppDbContext _db;

    public EmployeeService(AppDbContext db)
    {
        _db = db;
    }

    // Returns characters as DTOs (not entities)
    public async Task<List<CharacterDTO>> GetCharactersAsync()
    {
        // Query the database and PROJECT directly into DTOs
        // EF Core generates optimized SQL that selects only needed columns
        return await _db.Characters
            .Select(e => new CharacterDTO
            {
                Id = e.Id,
                Name = e.Name,
                Class = e.Class,
                Level = e.Level,
                Health = e.Health,
                Mana = e.Mana
            })
            .ToListAsync();
    }

    public async Task<CharacterDTO> PostCharacterAsync(CharacterDTO newCharacter)
    {

        var entity = new CharacterEntity
        {
            Name = newCharacter.Name,
            Class = newCharacter.Class,
            Level = newCharacter.Level,
            Health = newCharacter.Health,
            Mana = newCharacter.Mana
        };

        _db.Characters.Add(entity);
        await _db.SaveChangesAsync();


        return new CharacterDTO
        {
            Id = entity.Id,
            Name = entity.Name,
            Class = entity.Class,
            Level = entity.Level,
            Health = entity.Health,
            Mana = entity.Mana
        };
    }

    public async Task<CharacterDTO?> GetCharacterByIdAsync(Guid id)
    {
        var entity = await _db.Characters.FindAsync(id);
        if (entity == null) return null;

        return new CharacterDTO
        {
            Id = entity.Id,
            Name = entity.Name,
            Class = entity.Class,
            Level = entity.Level,
            Health = entity.Health,
            Mana = entity.Mana
        };
    }

    public async Task<CharacterDTO> PutCharacterAsync(CharacterDTO character)
    {
        var entity = await _db.Characters.FindAsync(character.Id);

        if (entity == null) return null;

        entity.Name = character.Name;
        entity.Class = character.Class;
        entity.Level = character.Level;
        entity.Health = character.Health;
        entity.Mana = character.Mana;

        await _db.SaveChangesAsync();

        return new CharacterDTO
        {
            Id = entity.Id,
            Name = entity.Name,
            Class = entity.Class,
            Level = entity.Level,
            Health = entity.Health,
            Mana = entity.Mana
        };
    }

    public async Task<bool> DeleteCharacterAsync(Guid id)
    {
        var entity = await _db.Characters.FindAsync(id);
        if (entity == null) return false;

        _db.Characters.Remove(entity);
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
