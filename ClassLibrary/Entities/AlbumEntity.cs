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


    [Table("album", Schema = "chinook")] // Maps this class to the "character" table. Could specify schema with something like [Table("character", Schema = "backend_rpg")]
    public class AlbumEntity
    {
        [Key] // Primary key
        [Column("albumid")]
        public int Id { get; set; }

        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [Column("artistid")]
        public int ArtistId { get; set; }

    }
}
