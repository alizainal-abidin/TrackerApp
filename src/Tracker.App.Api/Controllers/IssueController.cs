namespace Tracker.App.Api.Controllers
{
    using System.Threading.Tasks;
    using Application.Common.Interfaces;
    using Application.Business.Issues.Commands.CreateIssue;
    using Application.Business.Issues.Commands.UpdateIssue;
    using Application.Business.Issues.Queries.GetIssueList;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Tracker.App.Api.Base;
    using Microsoft.AspNetCore.Authorization;

    [Authorize]
    [Route("api/[controller]")]
    public class IssueController : BaseApiController
    {
        public IssueController(IMediator mediator, ICurrentUserService currentUserService)
            : base(mediator, currentUserService)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IssueDetailsVm.IssueListResult>> ListAsync([FromQuery] GetIssueListQuery request)
        {
            return await this.Mediator.Send(request);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<string>> AddAsync([FromBody] CreateIssueCommand request)
        {
            return await this.Mediator.Send(request);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(string id, [FromBody] UpdateIssueCommand request)
        {
            request.Id = id;
            return this.Ok(await this.Mediator.Send(request));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(string id, [FromBody] UpdateIssueCommand request)
        {
            request.Id = id;
            return this.Ok(await this.Mediator.Send(request));
        }
    }
}