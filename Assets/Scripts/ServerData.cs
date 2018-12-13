using UnityEngine;
using System.Collections;
using System;
using System.Threading;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace AssemblyCSharp
{
    public class SocketClientTest
    {
        //Socket客户端对象
        private Socket clientSocket;
        //单例模式
        private static SocketClientTest instance;
        public string receive_msg = "";
        public static SocketClientTest GetInstance()
        {
            if (instance == null)
            {
                instance = new SocketClientTest();
            }
            return instance;
        }

        //单例的构造函数
        SocketClientTest()
        {
            //创建Socket对象， 这里我的连接类型是TCP
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //服务器IP地址
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            //服务器端口
            IPEndPoint ipEndpoint = new IPEndPoint(ipAddress, 3306);
            //这是一个异步的建立连接，当连接建立成功时调用connectCallback方法
            IAsyncResult result = clientSocket.BeginConnect(ipEndpoint, new AsyncCallback(connectCallback), clientSocket);
            //这里做一个超时的监测，当连接超过5秒还没成功表示超时
            bool success = result.AsyncWaitHandle.WaitOne(5000, true);
            if (!success)
            {
                //超时
                Closed();
                Debug.Log("connect Time Out");
            }
            else
            {
                //Debug.Log ("与socket建立连接成功，开启线程接受服务端数据");
                //与socket建立连接成功，开启线程接受服务端数据。
                Thread thread = new Thread(new ThreadStart(ReceiveSorketMsg));
                thread.IsBackground = true;
                thread.Start();
            }
        }

        private void connectCallback(IAsyncResult asyncConnect)
        {
            Debug.Log("connectSuccess");
        }

        private void ReceiveSorketMsg()
        {
            Console.WriteLine("wait---");
            //在这个线程中接受服务器返回的数据
            while (true)
            {
                if (!clientSocket.Connected)
                {
                    //与服务器断开连接跳出循环
                    Debug.Log("Failed to clientSocket server.");
                    clientSocket.Close();
                    break;
                }
                try
                {
                    //接受数据保存至bytes当中
                    byte[] bytes = new byte[4096];
                    //Receive方法中会一直等待服务端回发消息
                    //如果没有回发会一直在这里等着。
                    int i = clientSocket.Receive(bytes);
                    if (i <= 0)
                    {
                        clientSocket.Close();
                        break;
                    }
                    Debug.Log(Encoding.ASCII.GetString(bytes, 0, i));
                    if (bytes.Length > 8)
                    {
                        //Console.WriteLine("接收服务器消息：{0}", Encoding.ASCII.GetString(bytes, 0, i));
                        receive_msg = Encoding.ASCII.GetString(bytes, 0, i);
                    }
                    else
                    {
                        Debug.Log("length is not > 8");
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("Failed to clientSocket error." + e);
                    clientSocket.Close();
                    break;
                }
            }
        }

        //关闭Socket
        public void Closed()
        {
            if (clientSocket != null && clientSocket.Connected)
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
            clientSocket = null;
        }
    }
}