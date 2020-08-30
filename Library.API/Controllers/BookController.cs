using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.API.Models;
using Library.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [Route("api/authors/{authorId}/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        public BookController(IBookRepository bookRepository,IAuthorRepository authorRepository)
        {
            BookRepository = bookRepository;
            AuthorRepository = authorRepository;
        }

        public IAuthorRepository AuthorRepository { get; }

        public IBookRepository BookRepository { get;  }

        [HttpGet]
        public ActionResult<List<BookDto>> GetBooks(Guid authorId)
        {
            if (!AuthorRepository.IsAuthorExists(authorId))
            {
                return NotFound();

            }

            return BookRepository.GetBooksForAuthor(authorId).ToList();
        }

        [HttpGet("{bookId}",Name = nameof(GetBook))]
        public ActionResult<BookDto> GetBook(Guid authorId, Guid bookId)
        {
            if (!AuthorRepository.IsAuthorExists(authorId))
            {
                return NotFound();

            }

            var targetBook = BookRepository.GetBookForAuthor(authorId, bookId);
            if (targetBook == null)
            {
                return NotFound();

            }

            return targetBook;
        }

        [HttpPost]
        public IActionResult AddBook(Guid authorId, BookForCreationDto book)
        {
            if (!AuthorRepository.IsAuthorExists(authorId))
            {
                return NotFound();
            }
            var newbook=new BookDto
            {
                Id=Guid.NewGuid(),
                Title = book.Title,
                Description = book.Description,
                Pages = book.Pages,
                AuthorId = authorId

            };
            BookRepository.AddBook(newbook);
            return CreatedAtRoute(nameof(GetBook),new {authorId,bookId=newbook.Id},newbook);
        }

        [HttpDelete("{bookId}")]
        public IActionResult DeleteBook(Guid authorId, Guid bookId)
        {
            if (!AuthorRepository.IsAuthorExists(authorId))
            {
                return NotFound();

            }

            var book = BookRepository.GetBookForAuthor(authorId, bookId);
            if (book == null)
            {
                return NotFound();

            }
            BookRepository.DeleteBook(book);
            return NoContent();
        }

        [HttpPut("{bookId}")]
        public IActionResult UpdateBook(Guid authorId, Guid bookId, BookForUpdateDto book)
        {
            if (!AuthorRepository.IsAuthorExists(authorId))
            {
                return NotFound();
            }

            var bookToUpdate = BookRepository.GetBookForAuthor(authorId, bookId);
            if(bookToUpdate==null){
                return NotFound();}
            BookRepository.UpdateBook(authorId,bookId,book);
            return Ok();
                
        }
    }
}
