using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary.Entities
{
    //
    // ENTITY ROLE
    // ------------
    // Entities represent DATABASE TABLES.
    //
    //
    // Entities:
    // - Match the database schema exactly
    // - Use attributes for column mapping
    // - Are NOT safe to return to the frontend
    //
    // Changing this class usually means changing the database.
    //

    [Table("funtest")] // Maps this class to the "character" table. Could specify schema with something like [Table("character", Schema = "backend_rpg")]
    public class FuntestEntity
    {
        [Key] // Primary key
        [Column("id")]
        public Guid Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("data")]
        public string Info { get; set; } = string.Empty;
    }
}
