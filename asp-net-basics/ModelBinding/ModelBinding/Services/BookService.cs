using System.Collections.Generic;
using ModelBinding.Models; 
namespace ModelBinding.Services;

public interface IBookService
{
    List<Book> GetAllBooks();
    Book AddBook(Book book);
}

public class BookService : IBookService
{
    private readonly List<Book> _books = new List<Book>();
    private int _nextId = 1;

    public List<Book> GetAllBooks()
    {
        return _books;
    }

    public Book AddBook(Book book)
    {
        book.Id = _nextId++;
        _books.Add(book);
        return book;
    }
}