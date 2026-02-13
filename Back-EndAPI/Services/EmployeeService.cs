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
    public async Task<List<EmployeeDTO>> GetEmployeesAsync()
    {
        // Query the database and PROJECT directly into DTOs
        // EF Core generates optimized SQL that selects only needed columns
        return await _db.Employee
            .Select(e => new EmployeeDTO
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                Phone = e.Phone,
                JobTitle = e.JobTitle,
                Salary = e.Salary,
                HireDate = e.HireDate,
                IsActive = e.IsActive
            })
            .ToListAsync();
    }

        public async Task<EmployeeDTO> PostEmployeeAsync(EmployeeDTO newEmployee)
    {

        var entity = new EmployeeEntity
        {
            FirstName = newEmployee.FirstName,
            LastName = newEmployee.LastName,
            Email = newEmployee.Email,
            Phone = newEmployee.Phone,
            JobTitle = newEmployee.JobTitle,
            Salary = newEmployee.Salary,
            HireDate = newEmployee.HireDate,
            IsActive = newEmployee.IsActive
        };

        _db.Employee.Add(entity);
        await _db.SaveChangesAsync();


        return new EmployeeDTO
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            Phone = entity.Phone,
            JobTitle = entity.JobTitle,
            Salary = entity.Salary,
            HireDate = entity.HireDate,
            IsActive = entity.IsActive
        };
    }

    public async Task<EmployeeDTO?> GetEmployeeByIdAsync(Guid id)
    {
        var entity = await _db.Employee.FindAsync(id);
        if (entity == null) return null;

        return new EmployeeDTO
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            Phone = entity.Phone,
            JobTitle = entity.JobTitle,
            Salary = entity.Salary,
            HireDate = entity.HireDate,
            IsActive = entity.IsActive
        };
    }

    public async Task<EmployeeDTO> PutEmployeeAsync(EmployeeDTO employee)
    {
        var entity = await _db.Employee.FindAsync(employee.Id);

        if (entity == null) return null;

        entity.FirstName = employee.FirstName;
        entity.LastName = employee.LastName;
        entity.Email = employee.Email;
        entity.Phone = employee.Phone;
        entity.JobTitle = employee.JobTitle;
        entity.Salary = employee.Salary;
        entity.HireDate = employee.HireDate;
        entity.IsActive = employee.IsActive;

        await _db.SaveChangesAsync();

        return new EmployeeDTO
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            Phone = entity.Phone,
            JobTitle = entity.JobTitle,
            Salary = entity.Salary,
            HireDate = entity.HireDate,
            IsActive = entity.IsActive
        };
    }

    public async Task<bool> DeleteEmployeeAsync(Guid id)
    {
        var entity = await _db.Employee.FindAsync(id);
        if (entity == null) return false;

        _db.Employee.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }
}
