using System.Threading.Tasks;
using NUnit.Framework;

namespace IntegrationTestsParallel
{
    public class Test3: IntegrationTestBase
    {
        [Test]
        public async Task Test()
        {
            await ExampleTest.Execute(SqlConnectionString);
        }
    }
}