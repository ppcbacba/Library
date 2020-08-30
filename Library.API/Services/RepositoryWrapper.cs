using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.API.Entities;

namespace Library.API.Services
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private IAuthorRepository _authorRepository;
        private IBookRepository _bookRepository;
        private LibraryDbContext LibraryDbContext;

        public RepositoryWrapper(LibraryDbContext libraryDbContext)
        {
            LibraryDbContext = libraryDbContext;
        }

        public IBookRepository Book => _bookRepository ?? new BookRepository(LibraryDbContext);
        public IAuthorRepository Author => _authorRepository ?? new AuthorRepository(LibraryDbContext);
    }
}