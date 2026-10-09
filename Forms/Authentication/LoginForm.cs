using System.Drawing;
using System.Windows.Forms;
using mt_network_monitoring.Services;

namespace mt_network_monitoring.Forms.Authentication;

public class LoginForm : Form
{
    private readonly AuthenticationService _authService = new();
    private readonly TextBox _username = new();
    private readonly TextBox _password = new();
    private readonly Label _status = new();

    public int LoggedInUserID { get; private set; }
    public string LoggedInFullName { get; private set; } = "";
    public string LoggedInRole { get; private set; } = "";

    public LoginForm()
    {
        Text = "Login - MikroTik Network Monitoring";
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ClientSize = new Size(440, 390);
        BackColor = Color.FromArgb(245, 247, 250);
        Font = new Font("Segoe UI", 10);

        var title = new Label
        {
            Text = "MIKROTIK MONITORING",
            Font = new Font("Segoe UI", 19, FontStyle.Bold),
            ForeColor = Color.FromArgb(25, 43, 70),
            AutoSize = true,
            Location = new Point(48, 28)
        };

        var subtitle = new Label
        {
            Text = "Silakan login untuk melanjutkan",
            AutoSize = true,
            Location = new Point(50, 72)
        };

        var userLabel = new Label
        {
            Text = "Username",
            AutoSize = true,
            Location = new Point(50, 120)
        };

        _username.SetBounds(50, 145, 340, 34);

        var passLabel = new Label
        {
            Text = "Password",
            AutoSize = true,
            Location = new Point(50, 195)
        };

        _password.SetBounds(50, 220, 340, 34);
        _password.UseSystemPasswordChar = true;

        var loginButton = new Button
        {
            Text = "LOGIN",
            Bounds = new Rectangle(50, 275, 340, 42),
            BackColor = Color.FromArgb(25, 43, 70),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };

        loginButton.Click += async (_, _) => await LoginAsync();
        _password.KeyDown += async (_, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                await LoginAsync();
            }
        };

        _status.SetBounds(50, 330, 340, 40);
        _status.ForeColor = Color.Firebrick;
        _status.Text = "Masukkan username dan password.";

        Controls.AddRange(new Control[]
        {
            title, subtitle, userLabel, _username,
            passLabel, _password, loginButton, _status
        });

        AcceptButton = loginButton;
    }

    private async Task LoginAsync()
    {
        string username = _username.Text.Trim();
        string password = _password.Text;

        if (string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrEmpty(password))
        {
            _status.Text = "Username dan password wajib diisi.";
            return;
        }

        try
        {
            _status.Text = "Memeriksa akun...";
            var user = await _authService.LoginAsync(username, password);

            if (user is null)
            {
                _status.Text = "Login gagal. Periksa akun kamu.";
                return;
            }

            LoggedInUserID = user.UserID;
            LoggedInFullName = user.FullName;
            LoggedInRole = user.Role;

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Kesalahan Login",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
