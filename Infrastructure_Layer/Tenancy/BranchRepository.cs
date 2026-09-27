using Application_Layer.Tenancy.DTOs;
using Application_Layer.Tenancy.Interfaces;
using Infrastructure_Layer.Common.Database;
using Microsoft.AspNetCore.Connections;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Infrastructure_Layer.Tenancy;

public class BranchRepository : IBranchRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly string _connectionString;
    public BranchRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<List<BranchDto>> GetAllBranchAsync(int tenantId)
    {
        var branches = new List<BranchDto>();
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand("ManageBranchMaster", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@Action", "1");
        command.Parameters.AddWithValue("@TenantId", tenantId);

        await connection.OpenAsync();

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            branches.Add(MapBranch(reader));
        }

        return branches;
    }

    public async Task<BranchDto?> GetBranchByIdAsync(int branchId,int tenantId)
    {
        await using var connection = new SqlConnection(_connectionString);

        await using var command = new SqlCommand("ManageBranchMaster", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Action", "2");
        command.Parameters.AddWithValue("@BranchId", branchId);
        command.Parameters.AddWithValue("@TenantId", tenantId);

        await connection.OpenAsync();

        await using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return MapBranch(reader);
        }

        return null;
    }

    public async Task<int> SaveBranchAsync(BranchDto branch)
    {
        await using var connection =new SqlConnection(_connectionString);

        await using var command = new SqlCommand("ManageBranchMaster", connection);

        command.CommandType = CommandType.StoredProcedure;
        if(branch.BranchId == 0)
        {
            command.Parameters.AddWithValue("@Action", 3); // Insert
        }
        else
        {
            command.Parameters.AddWithValue("@Action", 4); // Update
        }
        command.Parameters.AddWithValue("@BranchId",branch.BranchId);
        command.Parameters.AddWithValue("@TenantId", branch.TenantId);  
        command.Parameters.AddWithValue("@BranchName",branch.BranchName);
        command.Parameters.AddWithValue("@BranchCode",branch.BranchCode);

        command.Parameters.AddWithValue("@BranchEmail", (object?)branch.BranchEmail ?? DBNull.Value);   
        command.Parameters.AddWithValue("@BranchPhone",(object?)branch.BranchPhone ?? DBNull.Value);

        command.Parameters.AddWithValue("@BranchAddress", (object?)branch.BranchAddress ?? DBNull.Value);

        command.Parameters.AddWithValue("@BranchCity", (object?)branch.BranchCity ?? DBNull.Value);

        command.Parameters.AddWithValue("@BranchState", (object?)branch.BranchState ?? DBNull.Value);

        command.Parameters.AddWithValue("@BranchPincode", (object?)branch.BranchPincode ?? DBNull.Value);

        await connection.OpenAsync();

        var result = await command.ExecuteScalarAsync();
        return result == null || result == DBNull.Value ? 0 : Convert.ToInt32(result);
    }

    public async Task<bool> ActivateDeactivateBranchAsync(int branchId, int tenantId, bool isActive)
    {
        await using var connection = new SqlConnection(_connectionString);

        await using var command = new SqlCommand("ManageBranchMaster", connection);

        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@Action", 5);
        command.Parameters.AddWithValue("@BranchId", branchId);
        command.Parameters.AddWithValue("@TenantId", tenantId);
        await connection.OpenAsync();
        var result = await command.ExecuteScalarAsync();

        return result != null && result != DBNull.Value && Convert.ToInt32(result) > 0;
    }

    private static BranchDto MapBranch(SqlDataReader reader)
    {
        return new BranchDto
        {
            BranchId = Convert.ToInt32(reader["BranchId"]),
            TenantId = Convert.ToInt32(reader["TenantId"]),

            BranchName = reader["BranchName"]?.ToString() ?? string.Empty,
            BranchCode = reader["BranchCode"]?.ToString() ?? string.Empty,
            BranchEmail = reader["BranchEmail"] == DBNull.Value ? null : reader["BranchEmail"].ToString(),
            BranchPhone = reader["BranchPhone"] == DBNull.Value ? null : reader["BranchPhone"].ToString(),
            BranchAddress = reader["BranchAddress"] == DBNull.Value ? null : reader["BranchAddress"].ToString(),
            BranchCity = reader["BranchCity"] == DBNull.Value ? null : reader["BranchCity"].ToString(),
            BranchState = reader["BranchState"] == DBNull.Value ? null : reader["BranchState"].ToString(),
            BranchPincode = reader["BranchPincode"] == DBNull.Value ? null : reader["BranchPincode"].ToString(),
            IsActive = Convert.ToBoolean(reader["IsActive"]),
            CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
        };
    }
}