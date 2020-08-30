using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Services
{
    public class BookRepository:RepositoryBase<Book,Guid>,IBookRepository

    {
        public BookRepository(DbContext dbContext):base(dbContext)
        {
            
        }
    }
}
