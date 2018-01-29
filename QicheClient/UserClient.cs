using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HslCommunication.Enthernet;
using HslCommunication.BasicFramework;
using System.Net;
using CommonLibrary;
using Newtonsoft.Json.Linq;
using System.Configuration;
using CommonLibrary.Model;

namespace ClientsLibrary
{

    /// <summary>
    /// 一个通用的用户客户端类, 包含了一些静态的资源
    /// </summary>
    public class UserClient
    {

        public static string ServerIp { get; set; } = ConfigurationManager.AppSettings["ServerIp"];

        public static Boolean CheckServerUserful()
        {
            return Net_simplify_client.ReadFromServer(CommonHeadCode.SimplifyHeadCode.ConnectCheck).IsSuccess;
        }


        public static UserAccount UserAccount { get; set; } = new UserAccount();



        public static NetSimplifyClient Net_simplify_client { get; set; } = new NetSimplifyClient(
            new IPEndPoint(IPAddress.Parse(ServerIp), UserSystem.Port_Second_Net))
        {
            KeyToken = UserSystem.KeyToken,
            ConnectTimeout = 5000,
            
        };

        public static HslCommunication.LogNet.ILogNet LogNet { get; set; }


        /// <summary>
        /// 用来处理客户端发生的未捕获的异常，将通过网络组件发送至服务器存储，用于更好的跟踪错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception)
            {
                // 使用TCP方法传送回服务器
                //string info = HslCommunication.LogNet.LogNetManagment.GetSaveStringFromException(null, ex);
                //Net_simplify_client.ReadFromServer(CommonHeadCode.SimplifyHeadCode.异常消息, info);
            }
        }
        

    }
}
