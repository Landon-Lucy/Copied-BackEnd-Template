using ClassLibrary.DTOs;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class EmployeeRoleController : ControllerBase
{
    private readonly EmployeeRoleService _service;

    public EmployeeRoleController(EmployeeRoleService service)
    {
        _service = service;
    }

    // GET api/employeeRole/{id}/roles
    [HttpGet("{id}/roles")]
    public async Task<ActionResult<EmployeeRoleDTO?>> GetEmployeeRoles(int id)
    {
        var result = await _service.GetEmployeeWithRolesAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }
}