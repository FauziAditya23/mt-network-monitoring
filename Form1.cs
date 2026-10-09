using System.Drawing;
using System.Windows.Forms;
using mt_network_monitoring.Models;
using mt_network_monitoring.Services;

namespace mt_network_monitoring;

public partial class Form1 : Form
{
    private readonly RouterService _routerService = new();
    private readonly DataGridView _grid = new();
    private readonly Label _countLabel = new();
    private readonly Label _statusLabel = new();
    private readonly Button _refreshButton = new();

    public Form1()
    {
        InitializeComponent();

        Text = "MikroTik Network Monitoring";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(850, 500);
        Size = new Size(1100, 650);
        BackColor = Color.FromArgb(245, 247, 250);
        Font = new Font("Segoe UI", 10);

        BuildDashboard();
        Load += async (_, _) => await LoadRoutersAsync();
    }

    private void BuildDashboard()
    {
        Controls.Clear();

        var header = new Panel
        {
            Dock = DockStyle.Top,
            Height = 100,
            BackColor = Color.FromArgb(25, 43, 70)
        };

        header.Controls.Add(new Label
        {
            Text = "MikroTik Network Monitoring",
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 18, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(24, 16)
        });

        header.Controls.Add(new Label
        {
            Text = "Dashboard pemantauan router dan jaringan",
            ForeColor = Color.Gainsboro,
            AutoSize = true,
            Location = new Point(27, 57)
        });

        var toolbar = new Panel
        {
            Dock = DockStyle.Top,
            Height = 65,
            BackColor = Color.White
        };

        _countLabel.Text = "Jumlah router: 0";
        _countLabel.AutoSize = true;
        _countLabel.Location = new Point(20, 23);

        _refreshButton.Text = "Refresh Data";
        _refreshButton.Size = new Size(130, 38);
        _refreshButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _refreshButton.Location = new Point(Width - 180, 13);
        _refreshButton.Click += async (_, _) => await LoadRoutersAsync();

        toolbar.Controls.Add(_countLabel);
        toolbar.Controls.Add(_refreshButton);

        _statusLabel.Text = "Status: Menunggu data";
        _statusLabel.Dock = DockStyle.Bottom;
        _statusLabel.Height = 35;
        _statusLabel.Padding = new Padding(20, 8, 0, 0);
        _statusLabel.BackColor = Color.White;

        _grid.Dock = DockStyle.Fill;
        _grid.ReadOnly = true;
        _grid.AllowUserToAddRows = false;
        _grid.AllowUserToDeleteRows = false;
        _grid.MultiSelect = false;
        _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _grid.AutoGenerateColumns = false;
        _grid.BackgroundColor = Color.White;
        _grid.BorderStyle = BorderStyle.None;
        _grid.RowHeadersVisible = false;
        _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        AddColumn("Router ID", nameof(Router.RouterID));
        AddColumn("Nama Router", nameof(Router.RouterName));
        AddColumn("Host / IP", nameof(Router.Host));
        AddColumn("Versi RouterOS", nameof(Router.RouterOSVersion));
        AddColumn("Status", nameof(Router.Status));

        Controls.Add(_grid);
        Controls.Add(_statusLabel);
        Controls.Add(toolbar);
        Controls.Add(header);
    }

    private void AddColumn(string title, string property)
    {
        _grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = title,
            DataPropertyName = property,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        });
    }

    private async Task LoadRoutersAsync()
    {
        _refreshButton.Enabled = false;
        _statusLabel.Text = "Status: Mengambil data dari SQL Server...";

        try
        {
            var routers = await _routerService.GetAllAsync();

            _grid.DataSource = null;
            _grid.DataSource = routers;
            _countLabel.Text = $"Jumlah router: {routers.Count}";

            _statusLabel.Text = routers.Count == 0
                ? "Database terhubung, tetapi belum ada data router."
                : $"Berhasil memuat {routers.Count} router.";
        }
        catch (Exception ex)
        {
            _statusLabel.Text = "Gagal mengambil data router.";

            MessageBox.Show(
                ex.Message,
                "Kesalahan Database",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            _refreshButton.Enabled = true;
        }
    }
}
