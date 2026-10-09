using Microsoft.Data.SqlClient;
using mt_network_monitoring.Data;
using mt_network_monitoring.Forms.Authentication;
using mt_network_monitoring.Forms.Main;

namespace mt_network_monitoring;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        try
        {
            if (!HasActiveUser())
            {
                using var setup = new AdminSetupForm();

                if (setup.ShowDialog() != DialogResult.OK)
                    return;
            }

            using var login = new LoginForm();

            if (login.ShowDialog() == DialogResult.OK)
            {
                Application.Run(
                    new MainForm(
                        login.LoggedInFullName,
                        login.LoggedInRole));
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Kesalahan Aplikasi",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private static bool HasActiveUser()
    {
        using var connection = SqlConnectionFactory.CreateConnection();
        connection.Open();

        using var command = new SqlCommand(
            """
            SELECT COUNT(1)
            FROM Users
            WHERE IsActive = 1 AND IsDeleted = 0;
            """,
            connection);

        return Convert.ToInt32(command.ExecuteScalar()) > 0;
    }
}
