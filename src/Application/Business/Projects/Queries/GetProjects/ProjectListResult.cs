namespace Application.Business.Projects.Queries.GetProjects
{
    using System.Collections.Generic;
    using Application.Shared.Models;

    public class ProjectListResult : CollectionResponse<ProjectVm>
    {
        public ProjectListResult(IReadOnlyCollection<ProjectVm> items, int count, int pageIndex, int pageSize)
            : base(items, count, pageIndex, pageSize)
        {
        }
    }
}