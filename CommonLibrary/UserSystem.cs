using HslCommunication.BasicFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CommonLibrary
{
    /***********************************************************************************
     * 
     *    说明：用来客户端和服务器都能够直达访问的一些静态资源
     *          专门放在这下面的数据是需要支持winform和wpf共同访问的
     * 
     ***********************************************************************************/



    public class UserSystem
    {
        static UserSystem()
        {
            SoftBasic.FrameworkVersion = new SystemVersion("1.7.14");        
        }


        /************************************************************************************************
         * 
         *    注意：您在准备二次开发时，应该重新生成一个自己的GUID码
         * 
         ************************************************************************************************/


        /// <summary>
        /// 用于整个网络服务交互的身份令牌，可有效的防止来自网络的攻击，其他系统的恶意的连接
        /// 重新生成令牌后就无法更改，否则不支持自动升级
        /// </summary>
        public static Guid KeyToken { get; set; } = new Guid("1275BB9A-14B2-4A96-9673-B0AF0463D474");


        /// <summary>
        /// 主网络端口，此处随机定义了一个数据
        /// </summary>
        public static int Port_Main_Net { get; } = 17652;
        /// <summary>
        /// 同步网络访问的端口，此处随机定义了一个数据
        /// </summary>
        public static int Port_Second_Net { get; } = 14568;
        /// <summary>
        /// 用于软件系统更新的端口，此处随机定义了一个数据
        /// </summary>
        public static int Port_Update_Net { get; } = 17538;
        /// <summary>
        /// 共享文件的端口号
        /// </summary>
        public static int Port_Ultimate_File_Server { get; } = 34261;
        /// <summary>
        /// 用于UDP传输的端口号
        /// </summary>
        public static int Port_Udp_Server { get; } = 32566;
        /// <summary>
        /// 用于服务器版本更新的端口
        /// </summary>
        public static int Port_Advanced_File_Server { get; } = 24672;



        /// <summary>
        /// 整个系统的加密解密密码
        /// </summary>
        public static string Security { get; } = "qwertyui";



        /// <summary>
        /// 统一的窗体图标显示
        /// </summary>
        /// <returns></returns>
        public static Icon GetFormWindowIcon()
        {
            return Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        }



    }
}
