using Microsoft.Data.SqlClient;
using mt_network_monitoring.Data;

namespace mt_network_monitoring.Services;

public static class DatabaseTestService
{
    public static async Task<string> TestConnectionAsync()
    {
        try
        {
            await using var connection =
                SqlConnectionFactory.CreateConnection();

            await connection.OpenAsync();

            await using var command =
                new SqlCommand("SELECT DB_NAME()", connection);

            var databaseName = await command.ExecuteScalarAsync();

            return $"Koneksi berhasil! Database: {databaseName}";
        }
        catch (Exception ex)
        {
            return $"Koneksi gagal: {ex.Message}";
        }
    }
}
