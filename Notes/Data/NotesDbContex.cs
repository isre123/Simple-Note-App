using Microsoft.EntityFrameworkCore;

namespace Notes.Data
{
    public class NotesDbContex : DbContext
    {
        public NotesDbContex (DbContextOptions<NotesDbContex> options) : base(options) { }

        public DbSet<Models.Entities.Note> Notes { get; set; }

    }
}
