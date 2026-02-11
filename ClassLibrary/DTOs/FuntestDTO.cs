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

    public class FuntestDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Info { get; set; } = string.Empty;
    }
}
