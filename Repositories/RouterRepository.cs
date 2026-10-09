using Microsoft.Data.SqlClient;
using mt_network_monitoring.Data;
using mt_network_monitoring.Models;

namespace mt_network_monitoring.Repositories;

public class RouterRepository
{
    public async Task<List<Router>> GetAllAsync()
    {
        var routers = new List<Router>();

        const string sql = """
            SELECT
                RouterID,
                CompanyCode,
                RouterName,
                Host,
                RouterOSVersion,
                Uptime,
                Status,
                IsDeleted,
                CreatedBy,
                CreatedDate,
                LastUpdatedBy,
                LastUpdatedDate
            FROM Routers
            WHERE IsDeleted = 0
            ORDER BY RouterID DESC;
            """;

        await using var connection =
            SqlConnectionFactory.CreateConnection();

        await connection.OpenAsync();

        await using var command = new SqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            routers.Add(new Router
            {
                RouterID = reader.GetInt32(reader.GetOrdinal("RouterID")),
                CompanyCode = reader.GetString(reader.GetOrdinal("CompanyCode")),
                RouterName = reader.GetString(reader.GetOrdinal("RouterName")),
                Host = reader.GetString(reader.GetOrdinal("Host")),
                RouterOSVersion = GetNullableString(reader, "RouterOSVersion"),
                Uptime = GetNullableString(reader, "Uptime"),
                Status = GetNullableString(reader, "Status"),
                IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
                CreatedBy = GetNullableString(reader, "CreatedBy"),
                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                LastUpdatedBy = GetNullableString(reader, "LastUpdatedBy"),
                LastUpdatedDate = GetNullableDateTime(reader, "LastUpdatedDate")
            });
        }

        return routers;
    }

    private static string? GetNullableString(
        SqlDataReader reader, string column)
    {
        int index = reader.GetOrdinal(column);
        return reader.IsDBNull(index) ? null : reader.GetString(index);
    }

    private static DateTime? GetNullableDateTime(
        SqlDataReader reader, string column)
    {
        int index = reader.GetOrdinal(column);
        return reader.IsDBNull(index) ? null : reader.GetDateTime(index);
    }
}
