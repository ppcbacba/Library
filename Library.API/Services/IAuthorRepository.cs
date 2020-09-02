using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Library.API.Entities;
using Library.API.Helper;
using Library.API.Models;

namespace Library.API.Services
{
    public interface IAuthorRepository:IRepositoryBase<Author>,IRepositoryBase2<Author,Guid>
    {
        /*IEnumerable<AuthorDto> GetAuthors();
        AuthorDto GetAuthor(Guid authorId);
        bool IsAuthorExists(Guid authorId);
        void AddAuthor(AuthorDto author);
        void DeleteAuthor(AuthorDto author);*/
        Task<PagedList<Author>> GetAllAsync(AuthorResourceParameters parameters);
    }
}