using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;

namespace QicheClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            LoginWindow lw = new LoginWindow();
            bool? result = lw.ShowDialog();
            
            if (result.Value == true)
            {
                MainWindow mw = new MainWindow();
                MainWindow = mw;

                mw.ShowDialog();

                Current.Shutdown();//关闭当前的应用程序
            }
            else
            {
                Environment.Exit(0);
            }

        }


        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}
