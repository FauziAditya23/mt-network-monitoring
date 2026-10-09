using Microsoft.Data.SqlClient;
using mt_network_monitoring.Data;

namespace mt_network_monitoring.Services;

public class AdminSetupService
{
    public async Task CreateFirstAdminAsync(
        string companyCode,
        string username,
        string fullName,
        string password)
    {
        if (string.IsNullOrWhiteSpace(companyCode) ||
            string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(fullName) ||
            string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Semua field wajib diisi.");
        }

        string passwordHash = AuthenticationService.HashPassword(password);

        await using var connection = SqlConnectionFactory.CreateConnection();
        await connection.OpenAsync();

        await using var transaction = (SqlTransaction)
            await connection.BeginTransactionAsync();

        try
        {
            const string sql = """
                IF EXISTS (
                    SELECT 1 FROM Users
                    WHERE IsDeleted = 0 AND IsActive = 1
                )
                    THROW 50001, 'Sudah ada akun aktif.', 1;

                INSERT INTO Users
                    (CompanyCode, Username, PasswordHash, FullName, Role,
                     IsActive, IsDeleted, CreatedBy)
                VALUES
                    (@CompanyCode, @Username, @PasswordHash, @FullName,
                     'Admin', 1, 0, @Username);
                """;

            await using var command = new SqlCommand(sql, connection, transaction);
            command.Parameters.AddWithValue("@CompanyCode", companyCode.Trim());
            command.Parameters.AddWithValue("@Username", username.Trim());
            command.Parameters.AddWithValue("@PasswordHash", passwordHash);
            command.Parameters.AddWithValue("@FullName", fullName.Trim());

            await command.ExecuteNonQueryAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
