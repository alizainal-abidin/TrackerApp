namespace Application.Tests
{
    using System.Threading.Tasks;
    using NUnit.Framework;
    using static TestHelpers;

    public class TestBase
    {
        [SetUp]
        public async Task TestSetUp()
        {
            await ResetState();
        }
    }
}