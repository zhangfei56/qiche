using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HslCommunication;

namespace CommonLibrary
{

    /*********************************************************************************************
     * 
     *    说明：用于同步网络和异步网络的各种消息头的区别。
     *    
     *    关于 NetHandle：
     *         一个数据结构值，无论实际数据还是用法上等同于int，可以和int数据无缝的转化
     *         本质上将4个字节的int数据拆分成了三个属性，一个ushort和两个byte，可以分别访问
     *         
     *         2个低字节         第二高字节    最高的字节
     *         [byte1][byte2]    [byte3]       [byte4]
     *         CodeIdentifier    CodeMinor     CodeMajor     
     * 
     *********************************************************************************************/





    /// <summary>
    /// 用于网络通信的二级协议头说明
    /// </summary>
    public class CommonHeadCode
    {
        /// <summary>
        /// 同步通信的指令头说明，从1.1.x到1.255.x
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        public class SimplifyHeadCode
        {

            /*******************************************************************************************
             * 
             *     1.1.* 的指令为系统相关
             *
             *******************************************************************************************/

            #region 1.1.X 指令块


            public static NetHandle ConnectCheck { get; } =  new NetHandle(1, 1, 00001);
            public static NetHandle UserLogin { get; } = new NetHandle(1, 1, 00002);
            public static NetHandle AccountUserList { get; } = new NetHandle(1, 1, 00003);
            public static NetHandle VehicleList { get; } = new NetHandle(1, 1, 00004);
            public static NetHandle VehicleUpdate { get; } = new NetHandle(1, 1, 00005);
            public static NetHandle VehicleCreate { get; } = new NetHandle(1, 1, 00006);

            #endregion

        }



        /// <summary>
        /// 异步通信的头说明，从2.1.x到2.255.x
        /// </summary>
        public class MultiNetHeadCode
        {

           
        }


        //可以在下面进行扩展，需要保证长度都是统一的，新建您自己的类型 可以用3.1.x 4.1.x开头的指令
    }
}
