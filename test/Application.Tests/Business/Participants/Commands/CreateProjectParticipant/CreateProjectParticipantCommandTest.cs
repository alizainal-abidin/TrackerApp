namespace Application.Tests.Business.Participants.Commands.CreateProjectParticipant
{
    using System;
    using System.Threading.Tasks;
    using Application.Business.Participants.Commands.CreateProjectParticipant;
    using Application.Business.Projects.Commands.CreateProject;
    using Domain.Entities;
    using FluentAssertions;
    using FluentValidation;
    using NUnit.Framework;

    using static TestHelpers;

    public class CreateProjectParticipantCommandTest : TestBase
    {
        [Test]
        public async Task ShouldCreateProjectParticipantSuccessfully()
        {
            var userId = RunAsDefaultUser();
            var projectId = await SendAsync(new CreateProjectCommand
            {
                Key = "TKWZ",
                Name = "Test Project",
                OwnerId = userId
            });

            var command = new CreateProjectParticipantCommand
            {
                AddedBy = userId,
                ProjectId = projectId,
                UserId = userId
            };
            var itemId = await SendAsync(command);

            var item = await FindAsync<ProjectParticipant>(itemId);

            item.Should().NotBeNull();
            item.IsDeleted.Should().BeFalse();
            item.UserId.Should().Be(userId);
            item.AddedBy.Should().Be(userId);
            item.CreatedBy.Should().Be(userId);
            item.CreatedDate.Should().BeCloseTo(DateTime.Now, 10000);
            item.LastModifiedBy.Should().BeNull();
            item.LastModified.Should().BeNull();
        }

        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new CreateProjectParticipantCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }
    }
}
