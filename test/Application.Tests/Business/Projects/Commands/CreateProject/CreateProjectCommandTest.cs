namespace Application.Tests.Business.Projects.Commands.CreateProject
{
    using System;
    using System.Threading.Tasks;
    using Application.Business.Projects.Commands.CreateProject;
    using Domain.Entities;
    using FluentAssertions;
    using FluentValidation;
    using NUnit.Framework;

    using static TestHelpers;

    public class CreateProjectCommandTest : TestBase
    {
        [Test]
        public async Task ShouldUpdateProjectSuccessfully()
        {
            var userId = RunAsDefaultUser();
            var command = new CreateProjectCommand
            {
                Key = "TKWZ",
                Name = "Test Project",
                OwnerId = userId
            };

            var projectId = await SendAsync(command);            

            var item = await FindAsync<Project>(projectId);

            item.Should().NotBeNull();
            item.IsDeleted.Should().BeFalse();
            item.Key.Should().Be(command.Key);
            item.Name.Should().Be(command.Name);
            item.CreatedBy.Should().Be(userId);
            item.CreatedDate.Should().BeCloseTo(DateTime.Now, 10000);
            item.LastModifiedBy.Should().BeNull();
            item.LastModified.Should().BeNull();
        }

        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new CreateProjectCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }
    }
}