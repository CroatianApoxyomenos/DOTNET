using TFE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TFE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookContext _bookContext;

        public BooksController(BookContext bookContext)
        {
            _bookContext = bookContext;
        }

        // Get : api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            if (_bookContext.Books == null)
            {
                return NotFound();
            }
            return await _bookContext.Books.ToListAsync();
        }

        // Get : api/Books/2
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            if (_bookContext.Books == null)
            {
                return NotFound();
            }
            var book = await _bookContext.Books.FindAsync(id);
            if (book is null)
            {
                return NotFound();
            }
            return book;
        }

        // Post : api/Books
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            _bookContext.Books.Add(book);
            await _bookContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        // PUT : api/Books/2
        [HttpPut]
        public async Task<ActionResult<Book>> PutBook(int id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }
            _bookContext.Entry(book).State = EntityState.Modified;
            try
            {
                await _bookContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id)) { return NotFound(); }
                else { throw; }
            }
            return NoContent();
        }

        private bool BookExists(long id)
        {
            return (_bookContext.Books?.Any(book => book.Id == id)).GetValueOrDefault();
        }

        // Delete : api/Books/2
        [HttpDelete("{id}")]
        public async Task<ActionResult<Book>> DeleteBook(int id)
        {
            if (_bookContext.Books is null)
            {
                return NotFound();
            }
            var book = await _bookContext.Books.FindAsync(id);
            if (book is null)
            {
                return NotFound();
            }
            _bookContext.Books.Remove(book);
            await _bookContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
