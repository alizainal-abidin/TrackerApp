namespace Application.Business.Projects.Commands.CreateProject
{
    using System;
    using MediatR;

    public class CreateProjectCommand : IRequest<Guid>
    {
        public string Name { get; set; }

        public string Key { get; set; }

        public string OwnerId { get; set; }
    }
}