using System.Data.SqlClient;
using System.Threading.Tasks;
using SharePay.Models;

namespace SharePay.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly string connectionString = Database.ConnectionString;

        public async Task AddUser(UserModel userModel)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string query = "INSERT INTO Users (name, email) VALUES (@Name, @Email)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", userModel.Name);
                    command.Parameters.AddWithValue("@Email", userModel.Email);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<UserModel?> GetUserByEmail(string email)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            string query = "SELECT user_id, name, email FROM Users WHERE email = @Email";
            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email", email);
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new UserModel
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1),
                    Email = reader.GetString(2)
                };
            }
            return null;
        }
    }
}