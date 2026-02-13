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
[Route("api/employee")] // Base route for this controller
public class EmployeeController : ControllerBase
{
    // Service that contains the business/data logic.
    private readonly EmployeeService _employeeService;

    // Constructor Injection:
    // ASP.NET gives us CharacterService automatically via Dependency Injection
    public EmployeeController(EmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    // GET: api/characters
    // Returns a list of characters to the client
    [HttpGet]
    public async Task<ActionResult<List<EmployeeDTO>>> GetEmployees()
    {
        // Ask the service for character data
        var employees = await _employeeService.GetEmployeesAsync();

        // Return HTTP 200 OK with JSON data
        return Ok(employees);
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeDTO>> PostEmployee(EmployeeDTO newEmployee)
    {
        var createdEmployee = await _employeeService.PostEmployeeAsync(newEmployee);

        return CreatedAtAction(nameof(GetEmployees), new { id = createdEmployee.Id }, createdEmployee);
    }

    [HttpPut]
    public async Task<ActionResult<EmployeeDTO>> PutCharacter(EmployeeDTO updatedEmployee)
    {
        var employee = await _employeeService.PutEmployeeAsync(updatedEmployee);

        return Ok(employee);
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteEmployee(Guid id)
    {
        await _employeeService.DeleteEmployeeAsync(id);

        return NoContent();
    }
}
