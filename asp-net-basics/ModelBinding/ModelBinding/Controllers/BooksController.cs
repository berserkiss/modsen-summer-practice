using ModelBinding.Models;
using ModelBinding.Services;
using Microsoft.AspNetCore.Mvc;

namespace ModelBinding.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpPost("add")]
    public IActionResult AddBook([FromBody] Book book)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var addedBook = _bookService.AddBook(book);
        return CreatedAtAction(nameof(GetAllBooks), new { id = addedBook.Id }, addedBook);
    }

    [HttpGet("all")]
    public ActionResult<List<Book>> GetAllBooks()
    {
        return Ok(_bookService.GetAllBooks());
    }
}
