using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Library.API.Entities;
using Library.API.Models;

namespace Library.API.Services
{
    public interface IBookRepository:IRepositoryBase<Book>,IRepositoryBase2<Book,Guid>
    {
      // Task<IEnumerable<Book>> GetBooksAsync(Guid authorId);
       /* BookDto GetBookForAuthor(Guid authorId, Guid bookId);
        void AddBook(BookDto book);
        void DeleteBook(BookDto book);
        void UpdateBook(Guid authorId, Guid bookId, BookForUpdateDto book);*/
    }

    
}