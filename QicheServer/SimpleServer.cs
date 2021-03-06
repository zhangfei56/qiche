﻿using CommonLibrary;
using CommonLibrary.Model;
using HslCommunication;
using HslCommunication.BasicFramework;
using HslCommunication.Enthernet;
using HslCommunication.LogNet;
using MongoDB.Bson.Serialization.Serializers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QicheServer
{
    class SimpleServer
    {
        VehicleManager vehicleManager;
        private NetSimplifyServer net_simplify_server ;

        private string LogSavePath { get; set; }


        public SimpleServer()
        {
            this.net_simplify_server = new NetSimplifyServer();
            this.vehicleManager = new VehicleManager();
            InitLogPath();
            //vehicleManager.AddUser();
            //vehicleManager.AddVehicle();
            //List<Vehicle> vs = vehicleManager.GetAllVehicle();
            
        }

        /// <summary>
        /// 同步传送数据的初始化
        /// </summary>
        public void StartServer()
        {
            try
            {
                net_simplify_server.KeyToken = UserSystem.KeyToken;//设置身份令牌
                net_simplify_server.LogNet = new LogNetSingle(LogSavePath + @"\simplify_log.txt");//日志路径
                net_simplify_server.LogNet.SetMessageDegree(HslMessageDegree.INFO);//默认debug及以上级别日志均进行存储，根据需要自行选择
                net_simplify_server.ReceiveStringEvent += Net_simplify_server_ReceiveStringEvent;//接收到字符串触发
                net_simplify_server.ReceivedBytesEvent += Net_simplify_server_ReceivedBytesEvent;//接收到字节触发
                net_simplify_server.ServerStart(UserSystem.Port_Second_Net);
                net_simplify_server.ConnectTimeout = 5200;
            }
            catch (Exception ex)
            {
                SoftBasic.ShowExceptionMessage(ex);
            }
        }


        /// <summary>
        /// 接收到来自客户端的数据，此处需要放置维护验证，更新验证等等操作
        /// </summary>
        /// <param name="state">客户端的地址</param>
        /// <param name="handle">用于自定义的指令头，可不用，转而使用data来区分</param>
        /// <param name="data">接收到的服务器的数据</param>
        private void Net_simplify_server_ReceiveStringEvent(AsyncStateOne state, NetHandle handle, string data)
        {

            /*******************************************************************************************
             * 
             *     说明：同步消息处理总站，应该根据不同的消息设置分流到不同的处理方法
             *     
             *     注意：处理完成后必须调用 net_simplify_server.SendMessage(state, customer, "处理结果字符串，可以为空");
             *
             *******************************************************************************************/

            if (handle.CodeMajor == 1 && handle.CodeMinor == 1)
            {
                DataProcessingWithStartA(state, handle, data);
            }
            else if (handle.CodeMajor == 1 && handle.CodeMinor == 2)
            {
                DataProcessingWithStartB(state, handle, data);
            }
            else
            {
                net_simplify_server.SendMessage(state, handle, data);
            }
        }

        /// <summary>
        /// 1.1.x指令块，处理系统基础运行的消息
        /// </summary>
        /// <param name="state">网络状态对象</param>
        /// <param name="handle">用户自定义的指令头</param>
        /// <param name="data">实际的数据</param>
        private void DataProcessingWithStartA(AsyncStateOne state, NetHandle handle, string data)
        {
            if (handle == CommonHeadCode.SimplifyHeadCode.ConnectCheck)
            {
                net_simplify_server.SendMessage(state, handle, SoftResources.SystemResouce.Success);
                
            }else if (handle == CommonHeadCode.SimplifyHeadCode.UserLogin)
            {
                JObject json = JObject.Parse(data.Trim());

                string name = SoftBasic.GetValueFromJsonObject(json, SoftResources.StringResouce.UserNameText, "");
                string password = SoftBasic.GetValueFromJsonObject(json, SoftResources.StringResouce.PasswordText, "");
                if(name == "admin" && password == "admin")
                {
                    net_simplify_server.SendMessage(state, handle, SoftResources.SystemResouce.Success);
                }else
                {
                    net_simplify_server.SendMessage(state, handle, SoftResources.SystemResouce.Failed);
                }
            }else if(handle == CommonHeadCode.SimplifyHeadCode.AccountUserList)
            {
                List<UserAccount> users = vehicleManager.GetAllUserAccountInfo();
                
                net_simplify_server.SendMessage(state, handle, JsonConvert.SerializeObject(users));
                
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.VehicleList)
            {
                List<Vehicle> vehicles = vehicleManager.GetAllVehicle();

                net_simplify_server.SendMessage(state, handle, JsonConvert.SerializeObject(vehicles));

            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.VehicleUpdate)
            {
                Vehicle vehicle = JsonConvert.DeserializeObject<Vehicle>(data);
                vehicleManager.UpdateVehicle(vehicle);

                net_simplify_server.SendMessage(state, handle, SoftResources.SystemResouce.Success);

            }

        }

        /// <summary>
        /// B指令块，处理日志相关的消息
        /// </summary>
        /// <param name="state">网络状态对象</param>
        /// <param name="handle">用户自定义的命令头</param>
        /// <param name="data">实际的数据</param>
        private void DataProcessingWithStartB(AsyncStateOne state, NetHandle handle, string data)
        {

        }


        /// <summary>
        /// 接收来自客户端的字节数据
        /// </summary>
        /// <param name="state">网络状态</param>
        /// <param name="customer">字节数据，根据实际情况选择是否使用</param>
        /// <param name="data">来自客户端的字节数据</param>
        private void Net_simplify_server_ReceivedBytesEvent(AsyncStateOne state, NetHandle customer, byte[] data)
        {
            
        }



        private void InitLogPath()
        {
            LogSavePath = System.Environment.CurrentDirectory + @"\Logs";
            if (!System.IO.Directory.Exists(LogSavePath))
            {
                System.IO.Directory.CreateDirectory(LogSavePath);
            }
        }
    }
}
