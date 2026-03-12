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

public class AlbumService
{
    // Database context injected via Dependency Injection
    private readonly SecondAppDbContext _db;

    public AlbumService(SecondAppDbContext db)
    {
        _db = db;
    }

    // Returns characters as DTOs (not entities)
    public async Task<List<AlbumDTO>> GetAlbumsAsync()
    {
        // Query the database and PROJECT directly into DTOs
        // EF Core generates optimized SQL that selects only needed columns
        return await _db.Album
            .Select(e => new AlbumDTO
            {
                Id = e.Id,
                Title = e.Title,
                ArtistId = e.ArtistId
            })
            .ToListAsync();
    }

        public async Task<AlbumDTO> PostAlbumAsync(AlbumDTO newAlbum)
    {

        var entity = new AlbumEntity
        {
            Title = newAlbum.Title,
            ArtistId = newAlbum.ArtistId
        };

        _db.Album.Add(entity);
        await _db.SaveChangesAsync();


        return new AlbumDTO
        {
            Id = entity.Id,
            Title = entity.Title,
            ArtistId = entity.ArtistId
        };
    }

    public async Task<AlbumDTO?> GetAlbumByIdAsync(Guid id)
    {
        var entity = await _db.Album.FindAsync(id);
        if (entity == null) return null;

        return new AlbumDTO
        {
            Id = entity.Id,
            Title = entity.Title,
            ArtistId = entity.ArtistId
        };
    }

    public async Task<AlbumDTO> PutAlbumAsync(AlbumDTO album)
    {
        var entity = await _db.Album.FindAsync(album.Id);

        if (entity == null) return null;

        entity.Title = album.Title;
        entity.ArtistId = album.ArtistId;

        await _db.SaveChangesAsync();

        return new AlbumDTO
        {
            Id = entity.Id,
            Title = entity.Title,
            ArtistId = entity.ArtistId
        };
    }

    public async Task<bool> DeleteAlbumAsync(Guid id)
    {
        var entity = await _db.Album.FindAsync(id);
        if (entity == null) return false;

        _db.Album.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }
}
