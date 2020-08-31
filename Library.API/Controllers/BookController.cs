using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Library.API.Entities;
using Library.API.Filters;
using Library.API.Models;
using Library.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [Route("api/authors/{authorId}/books")]
    [ApiController]
    [ServiceFilter(typeof(CheckAuthorExistFilterAttribute))]
    public class BookController : ControllerBase
    {
        private IMapper _mapper;
        private IRepositoryWrapper _repositoryWrapper;

        public BookController(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<List<BookDto>>> GetBooksAsync(Guid authorId)
        {
            var books = await _repositoryWrapper.Book.GetByConditionAsync(book => book.AuthorId == authorId);
            var bookDtoList = _mapper.Map<IEnumerable<BookDto>>(books);
            return bookDtoList.ToList();
        }

        [HttpGet("{bookId}", Name = nameof(GetBook))]
        public async Task<ActionResult<BookDto>> GetBook(Guid authorId, Guid bookId)
        {
            var book = await _repositoryWrapper.Book.GetByConditionAsync(book => book.AuthorId == bookId);
            if (book == null)
            {
                return NotFound();
            }

            var bookDto = _mapper.Map<BookDto>(book);
            return bookDto;
        }

        [HttpPost]
        public async Task<ActionResult> AddBookAsync(Guid authorId, BookForCreationDto bookForCreationDto)
        {
            var book = _mapper.Map<Book>(bookForCreationDto);
            book.AuthorId = authorId;
            _repositoryWrapper.Book.Create(book);
            var result = await _repositoryWrapper.Book.SaveAsync();
            if (!result)
            {
                throw new Exception("创建book资源失败");
            }

            var bookDto = _mapper.Map<BookDto>(book);
            return CreatedAtRoute(nameof(GetBooksAsync), new {authorId, bookId = bookDto.Id}, bookDto);
        }

        [HttpDelete("{bookId}")]
        public async Task<IActionResult> DeleteBook(Guid authorId, Guid bookId)
        {
            var book = await _repositoryWrapper.Book.GetByIdAsync(bookId);
            if (book == null)
            {
                return NotFound();
            }

            _repositoryWrapper.Book.Delete(book);
            var result = await _repositoryWrapper.Book.SaveAsync();
            if (!result)
            {
                throw new Exception("删除book资源失败");
            }

            return NoContent();
        }

        [HttpPut("{bookId}")]
        public async Task<IActionResult> UpdateBook(Guid authorId, Guid bookId, BookForUpdateDto updateBook)
        {
            var book =await _repositoryWrapper.Book.GetByIdAsync(bookId);
            if (book == null)
            {
                return NotFound();
            }

            _mapper.Map(updateBook, book, typeof(BookForUpdateDto), typeof(Book));
            _repositoryWrapper.Book.Update(book);
            if (!await _repositoryWrapper.Book.SaveAsync())
            {
                throw new Exception("更新book错误");
            }
            return NoContent();
            
        }
    }
}