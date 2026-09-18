using Dapper;
using MvcAuth.Data;
using MvcAuth.Models;

namespace MvcAuth.Repositories
{
    public class UserRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public UserRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<User?> GetByEmail(string email)
        {
            using var connection = _connectionFactory.CreateConnection();

            string sql = @"
                SELECT Id, Name, Email, PasswordHash, CreatedAt
                FROM Users
                WHERE Email = @Email";

            return await connection.QuerySingleOrDefaultAsync<User>(
                sql,
                new { Email = email }
            );
        }

        public async Task<int> Create(User user)
        {
            using var connection = _connectionFactory.CreateConnection();

            string sql = @"
                INSERT INTO Users (Name, Email, PasswordHash)
                VALUES (@Name, @Email, @PasswordHash);

                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            return await connection.ExecuteScalarAsync<int>(
                sql,
                user
            );
        }
    }
}