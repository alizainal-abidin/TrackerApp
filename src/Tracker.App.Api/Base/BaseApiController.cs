namespace Tracker.App.Api.Base
{
    using Application.Common.Interfaces;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;

    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        protected BaseApiController(IMediator mediator, ICurrentUserService currentUserService)
        {
            this.Mediator = mediator ?? this.HttpContext.RequestServices.GetService<IMediator>();
            this.CurrentUser = currentUserService ?? this.HttpContext.RequestServices.GetService<ICurrentUserService>();
        }

        protected ICurrentUserService CurrentUser { get; }

        protected IMediator Mediator { get; }
    }
}