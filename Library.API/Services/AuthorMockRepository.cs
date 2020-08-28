using System;
using System.Collections.Generic;
using System.Linq;
using Library.API.Data;
using Library.API.Models;

namespace Library.API.Services
{
    public class AuthorMockRepository:IAuthorRepository
    {
        public IEnumerable<AuthorDto> GetAuthors()
        {
            return LibraryMockData.Current.Authors;
        }

        public AuthorDto GetAuthor(Guid authorId)
        {
            return LibraryMockData.Current.Authors.FirstOrDefault(au => au.Id == authorId);
        }

        public bool IsAuthorExists(Guid authorId)
        {
            return LibraryMockData.Current.Authors.Any(au => au.Id == authorId);

        }
    }
}