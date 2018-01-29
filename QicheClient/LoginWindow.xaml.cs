using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Threading;
using MaterialDesignThemes.Wpf;
using System.IO;
using ClientsLibrary;
using CommonLibrary;
using Newtonsoft.Json.Linq;
using HslCommunication;
using CommonLibrary.Model;

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
            UISettings(false);

        }

        #endregion

        #region Window Load

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowToolTip.Opacity = 0;

            Dispatcher.Invoke(new Action(() =>
            {
                if (UserClient.CheckServerUserful())
                {
                    UISettings(true);
                    SetInformationString("连接服务器成功！");
                }
                else
                {
                    SetInformationString("连接服务器失败！");
                }
            }));
        }


        #endregion

        #region Login Click


        private void LoginButton_Click(object sender, RoutedEventArgs e)
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

            SetInformationString("正在验证用户名和密码...");
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
            JObject json = new JObject
            {
                { SoftResources.StringResouce.UserNameText, new JValue(UserName) },                                    // 用户名
                { SoftResources.StringResouce.PasswordText, new JValue(UserPassword) },                                    // 密码
            };
            OperateResult<string> result = UserClient.Net_simplify_client.ReadFromServer(CommonHeadCode.SimplifyHeadCode.UserLogin, json.ToString());

            if (result.IsSuccess)
            {
                if (result.Content == SoftResources.SystemResouce.Success)
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        this.DialogResult = true;
                        return;
                    }));
                }
                else
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        UISettings(true);
                        SetInformationString("用户名或者密码错误");
                    }));


                }
            }
            else
            {
                SetInformationString(result.Message);
            }
            
            
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
            if (e.Key == Key.Enter) LoginButton_Click(null, new RoutedEventArgs());
        }


        #endregion



    }
}
