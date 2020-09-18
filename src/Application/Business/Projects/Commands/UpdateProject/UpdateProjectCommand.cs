namespace Application.Business.Projects.Commands.UpdateProject
{
    using System;
    using MediatR;

    public class UpdateProjectCommand : IRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string OwnerId { get; set; }

        public bool IsDeleted { get; set; }
    }
}