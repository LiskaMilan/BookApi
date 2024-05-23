using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Books.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Books.Infrastructure;
using Swashbuckle.AspNetCore.Annotations;
using Books.Service;

namespace Books.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBooksService _booksService;

        public BooksController(IBooksService booksService)
        {
            _booksService = booksService;
        }

        /// <summary>
        /// Získa všetky knihy.
        /// </summary>
        /// <returns>Zoznam kníh.</returns>
        [HttpGet]
        [SwaggerOperation(Summary = "Získa všetky knihy.", Description = "Vráti zoznam všetkých kníh v databáze.")]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks()
        {
            return await _booksService.GetAll();
        }

        /// <summary>
        /// Získa konkrétnu knihu podľa ID.
        /// </summary>
        /// <param name="id">ID knihy.</param>
        /// <returns>Detail knihy.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Získa konkrétnu knihu podľa ID.", Description = "Vráti detail knihy na základe zadaného ID.")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _booksService.GetById(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        /// <summary>
        /// Vytvorí novú knihu.
        /// </summary>
        /// <param name="book">Detaily knihy.</param>
        /// <returns>Vytvorená kniha.</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Vytvorí novú knihu.", Description = "Vytvorí novú knihu do databázy a vráti jej detail.")]
        public async Task<ActionResult<Book>> CreateBook(Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _booksService.SaveBook(book);

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        /// <summary>
        /// Aktualizuje existujúcu knihu.
        /// </summary>
        /// <param name="id">ID knihy</param>
        /// <param name="book">Aktualizované detaily knihy</param>
        /// <returns>Aktualizovaná kniha</returns>
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Aktualizuje existujúcu knihu.", Description = "Aktualizuje detaily existujúcej knihy v databáze.")]
        public async Task<ActionResult<Book>> UpdateBook(int id, Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Book updatedBook;

            try
            {
                updatedBook = await _booksService.UpdateBook(id, book);
            }
            catch (DbUpdateConcurrencyException)
            {
                var bookExists = await BookExists(id);
                if (bookExists)
                {
                    return NotFound($"Kniha s id: {id} neexistuje");
                }
                else
                {
                    throw;
                }
            }

            if (updatedBook == null)
            {
                return NotFound($"Kniha s id: {id} sa nenašla");
            }

            return updatedBook;
        }

        /// <summary>
        /// Vymaže existujúcu knihu podľa ID.
        /// </summary>
        /// <param name="id">ID knihy.</param>
        /// <returns>Správa o výsledku operácie.</returns>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Vymaže existujúcu knihu podľa ID.", Description = "Vymaže knihu z databázy na základe zadaného ID.")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _booksService.DeleteBook(id);
            if (book == null)
            {
                return NotFound(new { message = $"Kniha s id '{id}' sa nenašla." });
            }

            return Ok(new { message = $"Kniha '{book.Title}' bola úspešne vymazaná." });
        }

        private async Task<bool> BookExists(int id)
        {
            var books = await _booksService.GetAll();
            return books.Any(e => e.Id == id);
        }
    }
}
