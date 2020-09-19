namespace Application.Tests.Business.Participants.Commands.UpdateProjectParticipant
{
    using System;
    using System.Threading.Tasks;
    using Application.Business.Participants.Commands.CreateProjectParticipant;
    using Application.Business.Participants.Commands.UpdateProjectParticipant;
    using Application.Business.Projects.Commands.CreateProject;
    using Domain.Entities;
    using FluentAssertions;
    using FluentValidation;
    using NUnit.Framework;
    using static TestHelpers;

    public class UpdateProjectParticipantCommandTest : TestBase
    {
        [Test]
        public async Task ShouldDeleteProjectParticipantSuccessfully()
        {
            var userId = RunAsDefaultUser();
            var projectId = await SendAsync(new CreateProjectCommand
            {
                Key = "TKWZ",
                Name = "Test Project",
                OwnerId = userId
            });

            var itemId = await SendAsync(new CreateProjectParticipantCommand
            {
                AddedBy = userId,
                ProjectId = projectId,
                UserId = userId
            });

            var command = new UpdateProjectParticipantCommand
            {
                Id = itemId,
                IsDeleted = true,
                UserId = userId
            };
            await SendAsync(command);

            var item = await FindAsync<ProjectParticipant>(itemId);

            item.Should().NotBeNull();
            item.IsDeleted.Should().BeTrue();
            item.UserId.Should().Be(command.UserId);            
            item.LastModified.Should().BeCloseTo(DateTime.Now, 10000);
        }

        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new UpdateProjectParticipantCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }
    }
}