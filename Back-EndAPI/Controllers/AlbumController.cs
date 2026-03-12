using ClassLibrary.DTOs;
using Microsoft.AspNetCore.Mvc;

//
// CONTROLLER ROLE
// ----------------
// Controllers are the "front door" of your backend.
// They receive HTTP requests, call services to do the work,
// and translate results into HTTP responses.
//
// Controllers should be THIN:
// - No database logic
// - No business rules
// - No data transformation logic
//

[ApiController] // Enables automatic model validation & API behavior
[Route("api/album")] // Base route for this controller
public class AlbumController : ControllerBase
{
    // Service that contains the business/data logic.
    private readonly AlbumService _albumService;

    // Constructor Injection:
    // ASP.NET gives us CharacterService automatically via Dependency Injection
    public AlbumController(AlbumService albumService)
    {
        _albumService = albumService;
    }

    // GET: api/characters
    // Returns a list of characters to the client
    [HttpGet]
    public async Task<ActionResult<List<AlbumDTO>>> GetAlbums()
    {
        // Ask the service for character data
        var albums = await _albumService.GetAlbumsAsync();

        // Return HTTP 200 OK with JSON data
        return Ok(albums);
    }

    [HttpPost]
    public async Task<ActionResult<AlbumDTO>> PostAlbum(AlbumDTO newAlbum)
    {
        var createdAlbum = await _albumService.PostAlbumAsync(newAlbum);

        return CreatedAtAction(nameof(GetAlbums), new { id = createdAlbum.Id }, createdAlbum);
    }

    [HttpPut]
    public async Task<ActionResult<AlbumDTO>> PutCharacter(AlbumDTO updatedAlbum)
    {
        var album = await _albumService.PutAlbumAsync(updatedAlbum);

        return Ok(album);
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteAlbum(Guid id)
    {
        await _albumService.DeleteAlbumAsync(id);

        return NoContent();
    }
}
