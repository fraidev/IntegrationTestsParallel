﻿using System.Threading.Tasks;
using NUnit.Framework;

namespace IntegrationTestsParallel
{
    public class Test2: IntegrationTestBase
    {
        [Test]
        public async Task Test()
        {
            await ExampleTest.Execute(SqlConnectionString);
        }
    }
}