using System.Security.Cryptography;
using Microsoft.Data.SqlClient;
using mt_network_monitoring.Data;
using mt_network_monitoring.Models;

namespace mt_network_monitoring.Services;

public class AuthenticationService
{
    public async Task<User?> LoginAsync(string username, string password)
    {
        const string sql = """
            SELECT UserID, CompanyCode, Username, PasswordHash, FullName, Role
            FROM Users
            WHERE Username = @Username
              AND IsActive = 1
              AND IsDeleted = 0;
            """;

        await using var connection = SqlConnectionFactory.CreateConnection();
        await connection.OpenAsync();

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Username", username);

        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        string storedHash = reader.GetString(reader.GetOrdinal("PasswordHash"));

        if (!VerifyPassword(password, storedHash))
            return null;

        return new User
        {
            UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
            CompanyCode = reader.GetString(reader.GetOrdinal("CompanyCode")),
            Username = reader.GetString(reader.GetOrdinal("Username")),
            PasswordHash = storedHash,
            FullName = reader.GetString(reader.GetOrdinal("FullName")),
            Role = reader.GetString(reader.GetOrdinal("Role"))
        };
    }

    public static string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(16);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            password, salt, 100_000, HashAlgorithmName.SHA256, 32);

        return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
    }

    private static bool VerifyPassword(string password, string storedHash)
    {
        string[] parts = storedHash.Split(':');

        if (parts.Length != 2)
            return false;

        try
        {
            byte[] salt = Convert.FromBase64String(parts[0]);
            byte[] expectedHash = Convert.FromBase64String(parts[1]);

            byte[] actualHash = Rfc2898DeriveBytes.Pbkdf2(
                password, salt, 100_000, HashAlgorithmName.SHA256, 32);

            return CryptographicOperations.FixedTimeEquals(
                actualHash, expectedHash);
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
