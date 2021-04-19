using IISAppPoolInfo.Core.Models;
using IISAppPoolInfo.Core.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using IISAppPoolInfo.Forms.Properties;

namespace IISAppPoolInfo.Forms
{
    public partial class MainForm : Form
    {
        const int WIDTH = 550;
        const int HEIGHT = 300;
        
        private readonly IIISAppPoolService _iisService;
        private readonly ILogger<IISApplicationContext> _logger;
        private readonly AppSettings _config;

        private TabControl _tabs;
        private ListView _lvWorkerProcesses;
        private ListView _lvAppPools;
        private Button _btnRefresh;

        public MainForm()
        {
            // initialize component
            InitializeComponent();

            // initialize services
            _config = Startup.ConfigurationRoot.Get<AppSettings>();
            _logger = Startup.ServiceProvider.GetService<ILogger<IISApplicationContext>>();
            _iisService = Startup.ServiceProvider.GetService<IIISAppPoolService>();

            // set up how the form should be displayed.
            this.ClientSize = new Size(WIDTH, HEIGHT);
            this.Padding = new Padding(5);
            this.Text = Resources.AppName;
            this.WindowState = FormWindowState.Minimized;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.Icon = Resources.appicon_black;
            this.Activated += MainForm_Activated;
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            BtnRefresh_Click(null, null);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this._tabs = AddTabs();
            this._lvAppPools = AddAppPools();
            this._lvWorkerProcesses = AddWorkerProcesses();
            this._btnRefresh = AddRefreshButton();
        }

        private ListView AddWorkerProcesses()
        {
            var listView = new ListView
            {
                Bounds = new Rectangle(new Point(0, 0), new Size(WIDTH - 10, HEIGHT - 10)),
                View = View.Details
            };

            listView.FullRowSelect = true;
            listView.GridLines = true;
            listView.Sorting = SortOrder.Ascending;

            listView.Columns.Add("Process Id", -2, HorizontalAlignment.Left);
            listView.Columns.Add("Application Pool Name", -2, HorizontalAlignment.Left);

            LoadWorkerProcessItems(listView);

            this._tabs.TabPages[0].Controls.Add(listView);
            return listView;
        }

        private void LoadWorkerProcessItems(ListView listView)
        {
            listView.Items.Clear();
            foreach (var item in _iisService.GetWorkerProcesses(_config.Filter))
            {
                var lvItem = new ListViewItem(new string[] { item.ProcessId.ToString(), item.AppPoolName });
                listView.Items.Add(lvItem);
            }
        }

        private ListView AddAppPools()
        {
            var listView = new ListView
            {
                Bounds = new Rectangle(new Point(0, 0), new Size(WIDTH - 10, HEIGHT - 10)),
                View = View.Details
            };

            listView.FullRowSelect = true;
            listView.GridLines = true;
            listView.Sorting = SortOrder.Ascending;

            listView.Columns.Add("Application Pool Name", -2, HorizontalAlignment.Left);
            listView.Columns.Add("Status", -2, HorizontalAlignment.Left);

            LoadApplicationPoolsItems(listView);

            this._tabs.TabPages[1].Controls.Add(listView);
            return listView;
        }

        private void LoadApplicationPoolsItems(ListView listView)
        {
            listView.Items.Clear();
            foreach (var item in _iisService.GetApplicationPools(_config.Filter))
            {
                var lvItem = new ListViewItem(new string[] { item.Name, item.StateName });
                listView.Items.Add(lvItem);
            }
        }

        private Button AddRefreshButton()
        {
            var btnRefresh = new Button
            {
                Text = "Refresh",
                TextAlign = ContentAlignment.MiddleRight,
                Left = 10,
                Top = HEIGHT - 45,
                Height = 35,
                Image = Resources.refresh,
                ImageAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(5, 0, 0, 0),
                Width = 90,
                Cursor = Cursors.Hand
            };

            btnRefresh.Click += BtnRefresh_Click;
            this.Controls.Add(btnRefresh);
            return btnRefresh;
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            if (_tabs.SelectedTab.Text == "Worker Processes") {
                LoadWorkerProcessItems(_lvWorkerProcesses);
            }

            if (_tabs.SelectedTab.Text == "Application Pools") {
                LoadApplicationPoolsItems(_lvAppPools);
            }
        }

        private TabControl AddTabs()
        {
            var tabs = new TabControl
            {
                Width = WIDTH - 10,
                Height = HEIGHT - 60, // to leave room for the refresh button
                Left = 5,
                Top = 5
            };

            tabs.TabPages.Add("Worker Processes");
            tabs.TabPages.Add("Application Pools");
            this.Controls.Add(tabs);

            return tabs;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }
    }
}
