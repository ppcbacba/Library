using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Library.API.Entities;
using Library.API.Models;
using Library.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private IRepositoryWrapper RepositoryWrapper { get; }
        private IMapper Mapper { get; }

        public AuthorController(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            RepositoryWrapper = repositoryWrapper;
            Mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<AuthorDto>> GetAuthorAsync()
        {
            var authors = (await RepositoryWrapper.Author.GetAllAsync()).OrderBy(author => author.Name);
            var authorDtoList = Mapper.Map<IEnumerable<AuthorDto>>(authors);
            return authorDtoList.ToList();
        }

        [HttpGet("{authorId}")]
        public async Task<ActionResult<AuthorDto>> GetAuthor(Guid authorId)
        {
            var author = await RepositoryWrapper.Author.GetByIdAsync(authorId);
            if (author == null) return NotFound();
            var authorDto = Mapper.Map<AuthorDto>(author);
            return authorDto;
        }

        [HttpPost]
        public IActionResult CreateAuthor(AuthorForCreationDto authorForCreationDto)
        {
            var author = Mapper.Map<Author>(authorForCreationDto);
            RepositoryWrapper.Author.Create(author);
            var result = RepositoryWrapper.Author.SaveAsync();
            if (!result.IsCompletedSuccessfully)
            {
                throw new Exception("创建资源author失败");
            }

            var authorCreated = Mapper.Map<AuthorDto>(author);
            return CreatedAtRoute(nameof(GetAuthorAsync), new {authorId = authorCreated.Id}, authorCreated);
        }

        [HttpDelete("{authorId}")]
        public async Task<IActionResult> DeleteAuthor(Guid authorId)
        {
            var author =await RepositoryWrapper.Author.GetByIdAsync(authorId);
            if (author == null)
            {
                return NotFound();
            }
            RepositoryWrapper.Author.Delete(author);
            var result =await RepositoryWrapper.Author.SaveAsync();
            if (!result)
            {
                throw new Exception("无法删除");
            }

            return NoContent();
        }
    }
}