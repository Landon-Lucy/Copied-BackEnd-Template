using Back_EndAPI.Data;
using ClassLibrary.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class EmployeeRoleService
{
    private readonly AppDbContext _context;

    public EmployeeRoleService(AppDbContext context)
    {
        _context = context;
    }

    // Returns a single employee with their roles and each role's permissions
    public async Task<EmployeeRoleDTO?> GetEmployeeWithRolesAsync(int id)
    {
        var employee = await _context.Employees
            .Include(e => e.EmployeeRoles!)
                .ThenInclude(er => er.Role!)
                    .ThenInclude(r => r.RolePermissions!)
                        .ThenInclude(rp => rp.Permission!)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (employee == null) return null;

        var dto = new EmployeeRoleDTO
        {
            Id = employee.Id,
            Name = employee.Name,
            Roles = employee.EmployeeRoles?
                .Where(er => er.Role != null)
                .Select(er => new RoleDTO
                {
                    Id = er.Role.Id,
                    Name = er.Role.Name,
                    Description = er.Role.Description,
                    Permissions = er.Role.RolePermissions?
                        .Where(rp => rp.Permission != null)
                        .Select(rp => new PermissionDTO
                        {
                            Id = rp.Permission.Id,
                            PermissionName = rp.Permission.PermissionName,
                            PermissionDescription = rp.Permission.PermissionDescription
                        }).ToList() ?? new List<PermissionDTO>()
                }).ToList() ?? new List<RoleDTO>()
        };

        return dto;
    }
}