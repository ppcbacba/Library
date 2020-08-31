using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Library.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Library.API.Filters
{
    /// <summary>
    /// 因在bookcontroller中，每个逻辑要先检查author的存在性，重复工作用一个filter实现
    /// </summary>
    public class CheckAuthorExistFilterAttribute:ActionFilterAttribute

    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public CheckAuthorExistFilterAttribute(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;

        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var authorIdParameter = context.ActionArguments.Single(m => m.Key == "authorId");
            var authorId = (Guid) authorIdParameter.Value;
            var isExist = await _repositoryWrapper.Author.IsExistAsync(authorId);
            if (!isExist)
            {
                context.Result=new NotFoundResult();
            }
            await base.OnActionExecutionAsync(context, next);

        }
    }
}
