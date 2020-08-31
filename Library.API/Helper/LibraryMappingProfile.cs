using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Library.API.Entities;
using Library.API.Extensions;
using Library.API.Models;

namespace Library.API.Helper
{
    public class LibraryMappingProfile:Profile
    {
        public LibraryMappingProfile()
        {
            CreateMap<Author, AuthorDto>().ForMember(dest => dest.Age,
                config => config.MapFrom(src => src.BirthDate.GetCurrentAge()));
            CreateMap<Book, BookDto>();
            CreateMap<AuthorForCreationDto, Author>();
            CreateMap<BookForCreationDto, Book>();
            CreateMap<BookForUpdateDto, Book>();

        }
    }
}
