using BookStore.Dtos;
using BookStore.Enums;
using BookStore.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _bookService.GetByIdAsync(id);
            if (book == null) return NotFound();

            return Ok(book);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var books = await _bookService.GetAllAsync();
            return Ok(books);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string title, [FromQuery] string author, [FromQuery] Genre? genre)
        {
            var books = await _bookService.SearchAsync(title, author, genre);
            return Ok(books);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookDto bookCreateDto)
        {
            await _bookService.AddAsync(bookCreateDto);
            return CreatedAtAction(nameof(GetById), new { id = bookCreateDto.Id }, bookCreateDto);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] BookDto bookDto)
        {
            await _bookService.UpdateAsync(bookDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _bookService.DeleteAsync(id);
            return NoContent();
        }
    }
}
