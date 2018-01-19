using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Threading;
using MaterialDesignThemes.Wpf;
using System.IO;

namespace QicheClient
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        #region Constructor
        
        public LoginWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Window Load

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowToolTip.Opacity = 0;
        }

        #endregion

        #region Login Click


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //启动线程登录
            //验证输入
            if (string.IsNullOrEmpty(NameTextBox.Text))
            {
                SetInformationString("请输入用户名");
                NameTextBox.Focus();
                return;
            }

            if (string.IsNullOrEmpty(PasswordBox.Password))
            {
                SetInformationString("请输入密码");
                PasswordBox.Focus();
                return;
            }

            SetInformationString("正在验证维护状态...");
            UISettings(false);

            UserName = NameTextBox.Text;
            UserPassword = PasswordBox.Password;
            IsChecked = (bool)Remember.IsChecked;

            ThreadAccountLogin = new Thread(ThreadCheckAccount);
            ThreadAccountLogin.IsBackground = true;
            ThreadAccountLogin.Start();
        }


        #endregion

        #region 账户验证的逻辑块

        private string UserName = string.Empty;
        private string UserPassword = string.Empty;
        private bool IsChecked = false;

        /// <summary>
        /// 用于验证的后台线程
        /// </summary>
        private Thread ThreadAccountLogin = null;
        /// <summary>
        /// 用户账户验证的后台端
        /// </summary>
        private void ThreadCheckAccount()
        {
            //定义委托
            Action<string> message_show = delegate (string message)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    SetInformationString(message);
                }));
            };
            Action start_update = delegate
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    //需要该exe支持，否则将无法是实现自动版本控制
                    string update_file_name = AppDomain.CurrentDomain.BaseDirectory + @"软件自动更新.exe";
                    try
                    {
                        System.Diagnostics.Process.Start(update_file_name);
                        Environment.Exit(0);//退出系统
                    }
                    catch
                    {
                        MessageBox.Show("更新程序启动失败，请检查文件是否丢失，联系管理员获取。");
                    }
                }));
            };
            Action thread_finish = delegate
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    UISettings(true);
                }));
            };


            //// 启动密码验证
            //if (AccountLogin.AccountLoginServer(
            //    message_show,
            //    start_update,
            //    thread_finish,
            //    UserName,
            //    UserPassword,
            //    IsChecked,
            //    "wpf"))
            //{
                //启动主窗口
                Dispatcher.Invoke(new Action(() =>
                {                    
                    this.DialogResult = true;
                    return;
                }));
            //}
            
        }


        #endregion

        #region User Interface


        private void UISettings(bool enable)
        {
            NameTextBox.IsEnabled = enable;
            PasswordBox.IsEnabled = enable;
            Remember.IsEnabled = enable;
            LoginButton.IsEnabled = enable;
        }

        private void SetInformationString(string str)
        {
            if (WindowToolTip.Opacity == 1)
            {
                DoubleAnimation hidden = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(100));
                hidden.Completed += delegate
                {
                    DoubleAnimation show = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(100));
                    WindowToolTip.Text = str;
                    WindowToolTip.BeginAnimation(OpacityProperty, show);
                };
                WindowToolTip.BeginAnimation(OpacityProperty, hidden);
            }
            else
            {
                DoubleAnimation show = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(100));
                WindowToolTip.Text = str;
                WindowToolTip.BeginAnimation(OpacityProperty, show);
            }
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            LoginButton.Focus();
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            LoginButton.Focus();
        }



        private void NameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter)  PasswordBox.Focus();
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Button_Click(null, new RoutedEventArgs());
        }


        #endregion



    }
}
