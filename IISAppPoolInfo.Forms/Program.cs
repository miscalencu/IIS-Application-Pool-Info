using IISAppPoolInfo.Implementations;
using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Windows.Forms;

namespace IISAppPoolInfo.Forms
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            CheckAdmin.RequireAdministrator();

            Startup.RegisterServices();
            Startup.SetupErrorLogger();

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Application.Run(new MainForm());
            Application.Run(new IISApplicationContext());

            Startup.ServiceProvider.Dispose();
        }
    }
}
