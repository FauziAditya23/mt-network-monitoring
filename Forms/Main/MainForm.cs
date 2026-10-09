using System.Drawing;
using System.Windows.Forms;

namespace mt_network_monitoring.Forms.Main;

public class MainForm : Form
{
    public MainForm(string fullName, string role)
    {
        Text = "MikroTik Network Monitoring - Main Menu";
        WindowState = FormWindowState.Maximized;
        BackColor = Color.FromArgb(245, 247, 250);
        Font = new Font("Segoe UI", 10);

        var header = new Panel
        {
            Dock = DockStyle.Top,
            Height = 100,
            BackColor = Color.FromArgb(25, 43, 70)
        };

        header.Controls.Add(new Label
        {
            Text = "MIKROTIK NETWORK MONITORING",
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 20, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(28, 18)
        });

        header.Controls.Add(new Label
        {
            Text = $"Login sebagai: {fullName} | Role: {role}",
            ForeColor = Color.Gainsboro,
            AutoSize = true,
            Location = new Point(31, 62)
        });

        var menu = new FlowLayoutPanel
        {
            Dock = DockStyle.Left,
            Width = 250,
            Padding = new Padding(15),
            BackColor = Color.FromArgb(32, 49, 75),
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false
        };

        var content = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(35)
        };

        content.Controls.Add(new Label
        {
            Text = "Selamat Datang",
            Font = new Font("Segoe UI", 24, FontStyle.Bold),
            ForeColor = Color.FromArgb(25, 43, 70),
            AutoSize = true,
            Location = new Point(35, 35)
        });

        content.Controls.Add(new Label
        {
            Text = "Pilih modul dari menu di sebelah kiri.",
            AutoSize = true,
            Location = new Point(38, 90)
        });

        AddMenuButton(menu, "Dashboard");
        AddMenuButton(menu, "CRUD - Routers");
        AddMenuButton(menu, "CRUD - Interfaces");
        AddMenuButton(menu, "Reports");
        AddMenuButton(menu, "Keluar", () => Close());

        Controls.Add(content);
        Controls.Add(menu);
        Controls.Add(header);
    }

    private static void AddMenuButton(
        FlowLayoutPanel menu, string text, Action? action = null)
    {
        var button = new Button
        {
            Text = text,
            Width = 215,
            Height = 45,
            Margin = new Padding(0, 5, 0, 5),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(45, 65, 95),
            ForeColor = Color.White,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(12, 0, 0, 0)
        };

        button.Click += (_, _) =>
        {
            if (action is not null)
                action();
            else
                MessageBox.Show(
                    $"Modul {text} akan kita buat pada tahap berikutnya.",
                    "Informasi");
        };

        menu.Controls.Add(button);
    }
}
