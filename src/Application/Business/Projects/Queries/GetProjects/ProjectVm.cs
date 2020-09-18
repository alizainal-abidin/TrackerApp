namespace Application.Business.Projects.Queries.GetProjects
{
    using System;
    using Application.Common.Helpers.Extensions;
    using Application.Common.Helpers.Mappings;
    using AutoMapper;
    using Domain.Entities;

    public partial class ProjectVm : IMapFrom<Project>
    {
        public Guid Id { get; set; }

        public string Key { get; set; }

        public string Name { get; set; }

        public string OwnerId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Project, ProjectVm>().IgnoreAllNonExisting();
            profile.CreateMap<ProjectVm, Project>().IgnoreAllNonExisting();
        }
    }
}