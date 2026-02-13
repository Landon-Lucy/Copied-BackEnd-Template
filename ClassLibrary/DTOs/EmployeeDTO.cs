using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary.DTOs
{
    //
    // DTO ROLE
    // ---------
    // DTOs define the SHAPE of data sent to or received from clients.
    //
    // DTOs:
    // - Do NOT reference EF Core
    // - Do NOT match the database exactly
    // - Protect internal fields from being exposed(easier to hack if they are known)
    //
    // Think of DTOs as your API contract.
    //

    public class EmployeeDTO
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = string.Empty;
        
        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public string JobTitle { get; set; }

        public int Salary { get; set; }

        public DateTime HireDate { get; set; }

        public bool IsActive { get; set; }
    }
}
//[Key] // Primary key
//[Column("employee_id")]
//public Guid Id { get; set; }

//[Column("first_name")]
//public string FirstName { get; set; } = string.Empty;

//[Column("last_name")]
//public string LastName { get; set; } = string.Empty;

//[Column("email")]
//public string Email { get; set; } = string.Empty;

//[Column("phone")]
//public string Phone { get; set; }

//[Column("job_title")]
//public string JobTitle { get; set; }

//[Column("salary")]
//public int Salary { get; set; }

//[Column("hire_date")]
//public DateTime HireDate { get; set; }

//[Column("is_active")]
//public bool IsActive { get; set; }

//[Column("created_at")]
//public DateTime CreatedAt { get; set; }

//[Column("updated_at")]
//public DateTime UpdatedAt { get; set; }