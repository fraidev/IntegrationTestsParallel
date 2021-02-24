using System.IO;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using NUnit.Framework;

namespace IntegrationTestsParallel
{
    [Parallelizable]
    public class IntegrationTestBase
    {
        public string SqlConnectionString;
        public const string DatabaseName = "integration_tests";
        private string _dockerContainerId;
        private string _dockerSqlPort;
        private string _initialScript;

        public IntegrationTestBase()
        {
            _initialScript = File.ReadAllText(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "/" + DatabaseName + ".sql");
        }
        
        [SetUp]
        public async Task InitializeAsync()
        {
            (_dockerContainerId, _dockerSqlPort) = await DockerSqlDatabaseUtilities.EnsureDockerStartedAndGetContainerIdAndPortAsync();
            SqlConnectionString = GetSqlConnectionString();
            await ExecuteSqlScript();
        }

        [TearDown]
        public Task DisposeAsync()
        {
            return DockerSqlDatabaseUtilities.EnsureDockerStoppedAndRemovedAsync(_dockerContainerId);
        }

        private string GetSqlConnectionString()
        {
            return $"Data Source=localhost,{_dockerSqlPort};" +
                   $"Initial Catalog={DatabaseName};" +
                   "Integrated Security=False;" +
                   "User ID=SA;" +
                   $"Password={DockerSqlDatabaseUtilities.SQLSERVER_SA_PASSWORD}";
        }
        
        private string GetMasterSqlConnectionString()
        {
            return
                $"Data Source=localhost,{_dockerSqlPort};Integrated Security=False;User ID=SA;Password={DockerSqlDatabaseUtilities.SQLSERVER_SA_PASSWORD}";
        }
        
        private async Task ExecuteSqlScript()
        {
            await using SqlConnection connection = new(GetMasterSqlConnectionString());
            Server server = new(new ServerConnection(connection));
            server.ConnectionContext.ExecuteNonQuery(_initialScript);
        }
    }
}