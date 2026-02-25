using ClassLibrary.DTOs;
using ClassLibrary.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

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

public class CharacterService
{
    // Database context injected via Dependency Injection
    private readonly AppDbContext _db;

    public CharacterService(AppDbContext db)
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
        //Normalize
        newCharacter.Name = newCharacter.Name.Trim();
        newCharacter.Class = newCharacter.Class.Trim();
        //Capitalize first letter of each word
        newCharacter.Name = Regex.Replace(newCharacter.Name, @"\b\w", m => m.Value.ToUpper());
        //Capitalize only first letter of class
        newCharacter.Class = char.ToUpper(newCharacter.Class[0]) + newCharacter.Class.Substring(1).ToLower();

        // Validate - structure
        if (!Regex.IsMatch(newCharacter.Name, @"^[a-zA-Z0-9\s]+$"))
            throw new ArgumentException("Name must only contain letters, numbers, and spaces.");

        var validClasses = new[] { "Warrior", "Mage", "Rogue", "Archer" };
        if (!validClasses.Contains(newCharacter.Class))
            throw new ArgumentException($"{newCharacter.Class} is not a valid class");

        //Business rules

        var exists = await _db.Characters.AnyAsync(c => c.Name.ToLower() == newCharacter.Name.ToLower());

        if (exists)
        {
            throw new ArgumentException($"A character with the name '{newCharacter.Name}' already exists.");
        }

        if(newCharacter.Class == "Rogue" && newCharacter.Level > 40)
        {
            throw new ArgumentException("Rogues cannot be above level 40.");
        }

        if(newCharacter.Level < 1 || newCharacter.Level > 50 ) {
            throw new ArgumentException("Level must be between 1 and 50.");
        }

        //Using health as a substitute for gold on the assignment
        if(newCharacter.Health < 0 || newCharacter.Health > 10000) {
            throw new ArgumentException("Health must be between 0 and 10000.");
        }

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
        //Normalize
        character.Name = character.Name.Trim();
        character.Class = character.Class.Trim();
        //Capitalize first letter of each word
        character.Name = Regex.Replace(character.Name, @"\b\w", m => m.Value.ToUpper());
        //Capitalize only first letter of class
        character.Class = char.ToUpper(character.Class[0]) + character.Class.Substring(1).ToLower();

        // Validate - structure
        if (!Regex.IsMatch(character.Name, @"^[a-zA-Z0-9\s]+$"))
            throw new ArgumentException("Name must only contain letters, numbers, and spaces.");

        var validClasses = new[] { "Warrior", "Mage", "Rogue", "Archer" };
        if (!validClasses.Contains(character.Class))
            throw new ArgumentException($"{character.Class} is not a valid class");

        //Business rules

        var exists = await _db.Characters.AnyAsync(c => c.Name.ToLower() == character.Name.ToLower());

        if (exists)
        {
            throw new ArgumentException($"A character with the name '{character.Name}' already exists.");
        }

        if (character.Class == "Rogue" && character.Level > 40)
        {
            throw new ArgumentException("Rogues cannot be above level 40.");
        }

        if (character.Level < 1 || character.Level > 50)
        {
            throw new ArgumentException("Level must be between 1 and 50.");
        }

        //Using health as a substitute for gold on the assignment
        if (character.Health < 0 || character.Health > 10000)
        {
            throw new ArgumentException("Health must be between 0 and 10000.");
        }

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
