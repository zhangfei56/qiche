using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QicheClient
{

    public partial class MainWindow : Window
    {
        private Dictionary<string, Uri> allViews = new Dictionary<string, Uri>(); //包含所有页面

        public MainWindow()
        {
            InitializeComponent();
            allViews.Add("userList", new Uri("pages/userList.xaml", UriKind.Relative));
            allViews.Add("addUser", new Uri("pages/addUser.xaml", UriKind.Relative));
            allViews.Add("vehicleList", new Uri("pages/vehicleList.xaml", UriKind.Relative));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        public void clickFindUsers(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(allViews["userList"]);                    //Frame类的导航函数，参数时页面的Uri
        }

        /*
        *页面二按钮的响应事件函数，实现导航到page2
        */
        public void clickAddUser(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(allViews["addUser"]);                    //Frame导航函数，导航到page2
        }

        public void clickVehicleList(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(allViews["vehicleList"]);                    //Frame导航函数，导航到page2
        }

        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}
