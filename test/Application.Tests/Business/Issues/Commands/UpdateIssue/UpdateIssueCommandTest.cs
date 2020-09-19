namespace Application.Tests.Business.Issues.Commands.UpdateIssue
{
    using System;
    using System.Threading.Tasks;
    using Application.Business.Issues.Commands.CreateIssue;
    using Application.Business.Issues.Commands.UpdateIssue;
    using Application.Business.Projects.Commands.CreateProject;
    using Domain.Entities;
    using Domain.Enums;
    using FluentAssertions;
    using FluentValidation;
    using NUnit.Framework;

    using static TestHelpers;

    public class UpdateIssueCommandTest : TestBase
    {
        [Test]
        public async Task ShouldUpdateIssueSuccessfully()
        {
            var userId = RunAsDefaultUser();

            var projectId = await SendAsync(new CreateProjectCommand
            {
                Key = "TKWZ",
                Name = "Test Project",
                OwnerId = userId
            });

            var itemId = await SendAsync(new CreateIssueCommand
            {
                Assignee = "user@updated.cc",
                Title = "Tasks",
                Type = IssueType.Task,
                ProjectId = projectId,
                Reporter = userId
            });

            var command = new UpdateIssueCommand
            {
                Assignee = userId,
                Title = "Tasks Updated",
                Type = IssueType.Task,
                Id = itemId
            };

            await SendAsync(command);
            var item = await FindAsync<Issue>(itemId);

            item.Should().NotBeNull();
            item.Title.Should().Be(command.Title);
            item.Assignee.Should().Be(command.Assignee);
            item.LastModifiedBy.Should().Be(userId);
            item.LastModified.Should().BeCloseTo(DateTime.Now, 10000);            
        }        

        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new UpdateIssueCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }
    }
}