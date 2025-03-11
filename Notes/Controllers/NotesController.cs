using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notes.Data;
using Notes.Models.Entities;

namespace Notes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : Controller
    {
        private readonly NotesDbContex notesDbContex;

        public NotesController(NotesDbContex notesDbContex)
        {
            this.notesDbContex = notesDbContex;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllNotes()
        {
            //Get the notes from database
            return Ok(await notesDbContex.Notes.ToListAsync());
        }
        [HttpGet]
        [Route("{id:Guid}")]
        [ActionName("GetNotesById")]
        public async Task<IActionResult> GetNotesById([FromRoute] Guid id)
        {
            //Get the notes from database with id
          var notes= await notesDbContex.Notes.FirstOrDefaultAsync(c => c.Id == id );
            if (notes == null)
            {
                return NotFound();
            }
            return Ok(notes);
        }
        [HttpPost]
        public async Task<IActionResult> AddNotes(Note note)
        {
            //to create note id ourself not the client
            note.Id = Guid.NewGuid();
            await notesDbContex.Notes.AddAsync(note);
            //to save changes
            await notesDbContex.SaveChangesAsync();
            //id saved it will return the response below
            return CreatedAtAction(nameof(GetNotesById), new {id = note.Id}, note);


        }
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateNote([FromRoute]Guid id, [FromBody] Note UpdatedNote)
        {
            //we first check if the id exists first
            var existingNote = await notesDbContex.Notes.FindAsync(id);
            if (existingNote == null)
            {
                return NotFound();
            }
            existingNote.Title = UpdatedNote.Title;
            existingNote.Description = UpdatedNote.Description;
            existingNote.isVisible = UpdatedNote.isVisible;

            // we need to save the changes
            notesDbContex.SaveChangesAsync();
            return Ok(existingNote);
        }
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult>DeleteNote([FromRoute] Guid id)
        {
            //first we find the note by id
            var existingNote = await notesDbContex.Notes.FindAsync(id);
            if(existingNote == null)
            {
                    return NotFound();
            }
            //then we remove it
             notesDbContex.Notes.Remove(existingNote);
            //save changes
            await notesDbContex.SaveChangesAsync();
            //give a response to client that we deleted
            return Ok();

        }
    }
}
