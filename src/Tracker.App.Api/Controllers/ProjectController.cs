namespace Tracker.App.Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Application.Common.Interfaces;
    using Application.Business.Projects.Commands.CreateProject;
    using Application.Business.Projects.Commands.UpdateProject;
    using Application.Business.Projects.Queries.GetProjects;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Tracker.App.Api.Base;

    [Authorize]
    [Route("api/[controller]")]
    public class ProjectController : BaseApiController
    {
        public ProjectController(IMediator mediator, ICurrentUserService currentUserService)
            : base(mediator, currentUserService)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ProjectListResult>> ListAsync([FromQuery] GetProjectsQuery request)
        {
            return await this.Mediator.Send(request);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Guid>> AddAsync([FromBody] CreateProjectCommand request)
        {
            request.OwnerId = this.CurrentUser.UserId;
            return await this.Mediator.Send(request);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(Guid id, [FromBody] UpdateProjectCommand request)
        {
            request.Id = id;
            return this.Ok(await this.Mediator.Send(request));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid id, [FromBody] UpdateProjectCommand request)
        {
            request.Id = id;
            return this.Ok(await this.Mediator.Send(request));
        }
    }
}