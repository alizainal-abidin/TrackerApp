namespace Application.Business.Projects.Queries.GetProjects
{
    using MediatR;

    public class GetProjectsQuery : IRequest<ProjectListResult>
    {
        public string Name { get; set; }

        public string OwnerId { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}