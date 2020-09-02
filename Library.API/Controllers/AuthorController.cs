using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Library.API.Entities;
using Library.API.Helper;
using Library.API.Models;
using Library.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;



namespace Library.API.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly ILogger<AuthorController> _logger;
        private IRepositoryWrapper RepositoryWrapper { get; }
        private IMapper Mapper { get; }

        public AuthorController(IRepositoryWrapper repositoryWrapper, IMapper mapper,ILogger<AuthorController>logger)
        {
            RepositoryWrapper = repositoryWrapper;
            Mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// 当Action方法中使用了复杂的数据对象，而该参数的值又要从url中获取时，应该显式设置[FromQuery]特性
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthorAsync(
            [FromQuery] AuthorResourceParameters parameters)
        {
           // _logger.LogInformation("Hello,serilog!");
            var pagedList = await RepositoryWrapper.Author.GetAllAsync(parameters);
            var paginationMetadata = new
            {
                totalCount = pagedList.TotalCount,
                pageSize = pagedList.PageSize,
                currentPage = pagedList.CurrentPage,
                totalPages = pagedList.TotalPages,
                previousPageLink = pagedList.HasPrevious
                    ? Url.Link(nameof(GetAuthorAsync), new
                    {
                        pageNumber = pagedList.CurrentPage - 1,
                        pageSize = pagedList.PageSize,
                        birthPlace = parameters.BirthPlace,
                        searchQuery = parameters.SearchQuery,
                        sortBy = parameters.Sortby
                    })
                    : null,
                nextPageLink = pagedList.HasNext
                    ? Url.Link(nameof(GetAuthorAsync), new
                    {
                        pageNumber = pagedList.CurrentPage + 1,
                        pageSize = pagedList.PageSize,
                        birthPlace = parameters.BirthPlace,
                        searchQuery = parameters.SearchQuery,
                        sortBy = parameters.Sortby
                    })
                    : null
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));
            var authorDtoList = Mapper.Map<IEnumerable<AuthorDto>>(pagedList);
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
            var author = await RepositoryWrapper.Author.GetByIdAsync(authorId);
            if (author == null)
            {
                return NotFound();
            }

            RepositoryWrapper.Author.Delete(author);
            var result = await RepositoryWrapper.Author.SaveAsync();
            if (!result)
            {
                throw new Exception("无法删除");
            }

            return NoContent();
        }
    }
}