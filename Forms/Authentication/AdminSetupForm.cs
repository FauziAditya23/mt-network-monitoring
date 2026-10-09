using System.Drawing;
using System.Windows.Forms;
using mt_network_monitoring.Services;

namespace mt_network_monitoring.Forms.Authentication;

public class AdminSetupForm : Form
{
    private readonly AdminSetupService _service = new();

    private readonly TextBox _companyCode = new();
    private readonly TextBox _username = new();
    private readonly TextBox _fullName = new();
    private readonly TextBox _password = new();
    private readonly TextBox _confirmPassword = new();

    public AdminSetupForm()
    {
        Text = "Setup Admin - MikroTik Monitoring";
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        ClientSize = new Size(450, 480);
        Font = new Font("Segoe UI", 10);
        BackColor = Color.FromArgb(245, 247, 250);

        var title = new Label
        {
            Text = "SETUP ADMIN",
            Font = new Font("Segoe UI", 20, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(35, 20)
        };
        Controls.Add(title);

        AddField("Kode Perusahaan", _companyCode, 75);
        AddField("Username", _username, 145);
        AddField("Nama Lengkap", _fullName, 215);

        AddField("Password", _password, 285);
        _password.UseSystemPasswordChar = true;

        AddField("Konfirmasi Password", _confirmPassword, 355);
        _confirmPassword.UseSystemPasswordChar = true;

        var createButton = new Button
        {
            Text = "Buat Akun Admin",
            Location = new Point(35, 420),
            Size = new Size(370, 40),
            BackColor = Color.FromArgb(25, 43, 70),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };

        createButton.Click += async (_, _) => await CreateAdminAsync();
        Controls.Add(createButton);
    }

    private void AddField(string label, TextBox box, int top)
    {
        Controls.Add(new Label
        {
            Text = label,
            Location = new Point(35, top),
            AutoSize = true
        });

        box.Location = new Point(35, top + 23);
        box.Size = new Size(370, 30);
        Controls.Add(box);
    }

    private async Task CreateAdminAsync()
    {
        if (_password.Text.Length < 8)
        {
            MessageBox.Show("Gunakan password minimal 8 karakter.");
            return;
        }

        if (_password.Text != _confirmPassword.Text)
        {
            MessageBox.Show("Konfirmasi password tidak cocok.");
            return;
        }

        try
        {
            await _service.CreateFirstAdminAsync(
                _companyCode.Text,
                _username.Text,
                _fullName.Text,
                _password.Text);

            MessageBox.Show(
                "Akun admin berhasil dibuat. Silakan login.",
                "Berhasil",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Gagal Membuat Admin",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
