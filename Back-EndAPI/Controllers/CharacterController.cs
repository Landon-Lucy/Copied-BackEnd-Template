using ClassLibrary.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
[Route("api/character")] // Base route for this controller
public class CharacterController : ControllerBase
{
    // Service that contains the business/data logic.
    private readonly CharacterService _characterService;

    // Constructor Injection:
    // ASP.NET gives us CharacterService automatically via Dependency Injection
    public CharacterController(CharacterService characterService)
    {
        _characterService = characterService;
    }

    // GET: api/characters
    // Returns a list of characters to the client
    [HttpGet]
    public async Task<ActionResult<List<CharacterDTO>>> GetCharacters()
    {
        // Ask the service for character data
        var characters = await _characterService.GetCharactersAsync();

        // Return HTTP 200 OK with JSON data
        return Ok(characters);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CharacterDTO>> GetCharacterById(Guid id)
    {
        var character = await _characterService.GetCharacterByIdAsync(id);
        if (character == null)
            return NotFound();
        return Ok(character);
    }

    [HttpPost]
    public async Task<ActionResult<CharacterDTO>> PostCharacter(CharacterDTO newCharacter)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(Error("Invalid Data"));
        }

        if(newCharacter.Id != Guid.Empty)
        {
            return BadRequest(Error("Id cannot be set"));
        }

        try
        {
        var createdCharacter = await _characterService.PostCharacterAsync(newCharacter);

        return CreatedAtAction(nameof(GetCharacters), new { id = createdCharacter.Id }, createdCharacter);
        }
        catch (ValidationException ex) 
        {
            return BadRequest(Error(ex.Message));
        }


    }

    [HttpPut]
    public async Task<ActionResult<CharacterDTO>> PutCharacter(CharacterDTO updatedCharacter)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(Error("Invalid Data"));
        }

        if (updatedCharacter.Id != Guid.Empty)
        {
            return BadRequest(Error("Id cannot be set"));
        }

        var character = await _characterService.PutCharacterAsync(updatedCharacter);

        return Ok(character);
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteCharacter(Guid id)
    {
        await _characterService.DeleteCharacterAsync(id);

        return NoContent();
    }

// An Object Error with a message and a timestamp
    private object Error(string message)
    {
        return new
        {
            Message = message,
            Timestamp = DateTime.UtcNow
        };
    }
}
