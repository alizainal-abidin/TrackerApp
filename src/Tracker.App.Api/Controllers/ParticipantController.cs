namespace Tracker.App.Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Application.Common.Interfaces;
    using Application.Business.Participants.Commands.CreateProjectParticipant;
    using Application.Business.Participants.Commands.UpdateProjectParticipant;
    using Application.Business.Participants.Queries.GetParticipants;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Tracker.App.Api.Base;
    using Microsoft.AspNetCore.Authorization;

    [Authorize]
    [Route("api/[controller]")]
    public class ParticipantController : BaseApiController
    {
        public ParticipantController(IMediator mediator, ICurrentUserService currentUserService)
            : base(mediator, currentUserService)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ParticipantListResult>> ListAsync([FromQuery] GetParticipantsQuery request)
        {
            return await this.Mediator.Send(request);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Guid>> AddAsync([FromBody] CreateProjectParticipantCommand request)
        {
            return await this.Mediator.Send(request);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(Guid id, [FromBody] UpdateProjectParticipantCommand request)
        {
            request.Id = id;
            return this.Ok(await this.Mediator.Send(request));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid id, [FromBody] UpdateProjectParticipantCommand request)
        {
            request.Id = id;
            return this.Ok(await this.Mediator.Send(request));
        }
    }
}