namespace Application.Tests.Business.Issues.Commands.CreateIssue
{
    using System;
    using System.Threading.Tasks;
    using Application.Business.Issues.Commands.CreateIssue;
    using Application.Business.Projects.Commands.CreateProject;
    using Domain.Entities;
    using Domain.Enums;
    using FluentAssertions;
    using FluentValidation;
    using NUnit.Framework;

    using static TestHelpers;

    public class CreateIssueCommandTest : TestBase
    {
        [Test]
        public async Task ShouldCreateIssueSuccessfully()
        {
            var userId = RunAsDefaultUser();

            var projectId = await SendAsync(new CreateProjectCommand
            {
                Key = "TKWZ",
                Name = "Test Project",
                OwnerId = userId
            });

            var command = new CreateIssueCommand
            {
                Assignee = userId,
                Title = "Tasks",
                Type = IssueType.Task,
                ProjectId = projectId,
                Reporter = userId
            };

            var itemId = await SendAsync(command);

            var item = await FindAsync<Issue>(itemId);

            item.Should().NotBeNull();            
            item.Title.Should().Be(command.Title);
            item.CreatedBy.Should().Be(userId);
            item.CreatedDate.Should().BeCloseTo(DateTime.Now, 10000);
            item.LastModifiedBy.Should().BeNull();
            item.LastModified.Should().BeNull();
        }

        [Test]
        public async Task ShouldNotHaveStoryPointAndStepToReproduceTogether()
        {
            var userId = RunAsDefaultUser();

            var projectId = await SendAsync(new CreateProjectCommand
            {
                Key = "TKWZ",
                Name = "Test Project",
                OwnerId = userId
            });

            var command = new CreateIssueCommand
            {
                Assignee = userId,
                Title = "Tasks",
                Type = IssueType.Bug,
                ProjectId = projectId,
                Reporter = userId,
                StoryPoint = 5,
                StepsToReplicate = "-StepsToReplicate-"
            };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task StoryShouldNotHaveStepToReproduceTogether()
        {
            var userId = RunAsDefaultUser();

            var projectId = await SendAsync(new CreateProjectCommand
            {
                Key = "TKWZ",
                Name = "Test Project",
                OwnerId = userId
            });

            var command = new CreateIssueCommand
            {
                Assignee = userId,
                Title = "Tasks",
                Type = IssueType.Story,
                ProjectId = projectId,
                Reporter = userId,                
                StepsToReplicate = "-StepsToReplicate-"
            };            

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new CreateIssueCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }
    }
}