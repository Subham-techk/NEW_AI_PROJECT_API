using Application_Layer.User.DTOs;
using Application_Layer.User.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Infrastructure_Layer.User.Repositories;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(IConfiguration configuration)
    {
        _connectionString =configuration.GetConnectionString("DefaultConnection")?? throw new InvalidOperationException("DefaultConnection not found.");
    }

    public async Task<UserDto?> GetByEmailAsync(string email)
    {
        await using SqlConnection connection = new(_connectionString);

        await using SqlCommand command = new("SP_User_GetByEmail", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 150){Value = email});

        await connection.OpenAsync();

        await using SqlDataReader reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return new UserDto
        {
            UserId = Convert.ToInt32(reader["UserId"]),
            FullName = reader["FullName"].ToString()!,
            Email = reader["Email"].ToString()!,
            PasswordHash = reader["PasswordHash"]?.ToString(),
            IsActive = Convert.ToBoolean(reader["IsActive"]),
            CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
        };
    }
}