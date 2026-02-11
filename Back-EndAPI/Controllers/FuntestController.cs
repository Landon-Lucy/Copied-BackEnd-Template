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
[Route("api/funtest")] // Base route for this controller
public class FuntestController : ControllerBase
{
    // Service that contains the business/data logic.
    private readonly FuntestService _funtestService;

    // Constructor Injection:
    // ASP.NET gives us CharacterService automatically via Dependency Injection
    public FuntestController(FuntestService funtestService)
    {
        _funtestService = funtestService;
    }

    // GET: api/characters
    // Returns a list of characters to the client
    [HttpGet]
    public async Task<ActionResult<List<FuntestDTO>>> GetFuntests()
    {
        // Ask the service for character data
        var funtest = await _funtestService.GetFuntestsAsync();

        // Return HTTP 200 OK with JSON data
        return Ok(funtest);
    }

    [HttpPost]
    public async Task<ActionResult<FuntestDTO>> PostFuntest(FuntestDTO newFuntest)
    {
        var createdFuntest = await _funtestService.PostFuntestAsync(newFuntest);

        return CreatedAtAction(nameof(GetFuntests), new { id = createdFuntest.Id }, createdFuntest);
    }

    [HttpPut]
    public async Task<ActionResult<CharacterDTO>> PutFuntest(FuntestDTO updatedFuntest)
    {
        var funtest = await _funtestService.PutFuntestAsync(updatedFuntest);

        return Ok(funtest);
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteFuntest(Guid id)
    {
        await _funtestService.DeleteFuntestAsync(id);

        return NoContent();
    }
}
