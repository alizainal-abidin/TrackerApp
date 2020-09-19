namespace Application.Tests.Business.Projects.Commands.UpdateProject
{
    using System;
    using System.Threading.Tasks;
    using Application.Business.Projects.Commands.CreateProject;
    using Application.Business.Projects.Commands.UpdateProject;
    using Domain.Entities;
    using FluentAssertions;
    using FluentValidation;
    using NUnit.Framework;

    using static TestHelpers;

    public class UpdateProjectCommandTest : TestBase
    {
        [Test]
        public async Task ShouldUpdateProjectSuccessfully()
        {
            var userId = RunAsDefaultUser();

            var projectId = await SendAsync(new CreateProjectCommand
            {
                Key = "TKWZ",
                Name = "Test Project",
                OwnerId = userId
            });
            
            var command = new UpdateProjectCommand
            {
                Id = projectId,
                IsDeleted = true,
                Name = "Deleted Project",
                OwnerId = userId
            };

            await SendAsync(command);
            var item = await FindAsync<Project>(projectId);

            item.Should().NotBeNull();
            item.IsDeleted.Should().Be(command.IsDeleted);
            item.Name.Should().Be(command.Name);
            item.LastModifiedBy.Should().Be(userId);
            item.LastModified.Should().BeCloseTo(DateTime.Now, 10000);
        }

        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new UpdateProjectCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }
    }
}
