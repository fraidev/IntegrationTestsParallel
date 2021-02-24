using System.Linq;
using System.Threading.Tasks;
using Dapper;
using FluentAssertions;
using Microsoft.Data.SqlClient;

namespace IntegrationTestsParallel
{
    public static class ExampleTest
    {
        public static async Task Execute(string sqlConnectionString)
        {
            const string? insertSql = "INSERT INTO TODO (id, done, [name]) VALUES (NEWID(), 1, 'abc')";
            const string? selectSql = "select id, done, [name] from TODO";
            await using SqlConnection connection = new(sqlConnectionString);
            await connection.ExecuteAsync(insertSql);
            var result = (await connection.QueryAsync<Todo>(selectSql)).ToList();
            await connection.CloseAsync();


            result.Should().HaveCount(1);
            result[0].Done.Should().BeTrue();
            result[0].Name.Should().Be("abc");
        }
    }
}