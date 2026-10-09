using Microsoft.Data.SqlClient;

namespace mt_network_monitoring.Data;

public static class SqlConnectionFactory
{
    private const string ConnectionString =
        "Server=localhost;" +
        "Database=mt-monitoring;" +
        "Integrated Security=True;" +
        "Encrypt=True;" +
        "TrustServerCertificate=True;";

    public static SqlConnection CreateConnection()
    {
        return new SqlConnection(ConnectionString);
    }
}
