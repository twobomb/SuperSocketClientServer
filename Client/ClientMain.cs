using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using Client.SuperSocketObjects;
using Core;
using SuperSocket.ClientEngine;
using ErrorEventArgs = SuperSocket.ClientEngine.ErrorEventArgs;

namespace Client
{
    class ClientMain
    {
        private static EasyClient client;
        static void Main(string[] args)
        {
            client = new EasyClient();
            client.ReceiveBufferSize = 10240;
            client.NoDelay = true;
            client.Closed += EasyClient_Closed;
            client.Connected += EasyClient_Connected;
            client.Error += EasyClient_Error;
            
            client.Initialize(new FixedRecieveFilterX(), MessageRecieved);
            Console.WriteLine("Клиент инициализирован");
            client.ConnectAsync(new DnsEndPoint("localhost", 2021));
            while (Console.ReadKey().Key != ConsoleKey.Q)
            {
                SendMessage(new Message()
                {
                    data = "testmsg",
                    key = "test"
                });
            }
        }

        public static void SendMessage(Message msg){
            if (!client.IsConnected)
            {
                Console.WriteLine("Клиент отключен, не могу послать сообщение!");
                return;
            }
            using (MemoryStream ms = new MemoryStream()) {
                ms.WriteByte(0);
                ms.WriteByte(0);
                ms.WriteByte(0);
                ms.WriteByte(0);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, msg);
                ms.Seek(0, SeekOrigin.Begin);
                ms.Write(BitConverter.GetBytes(ms.Length - 4), 0, 4);
                ms.Seek(0, SeekOrigin.End);
                client.Send(ms.ToArray());
            }
        }
        public static void SendMessages(List<Message> msgs){
            if (!client.IsConnected)
            {
                Console.WriteLine("Клиент отключен, не могу послать сообщение!");
                return;
            }
            using (MemoryStream ms = new MemoryStream()) {
                foreach (var message in msgs) {
                    int pos = (int)ms.Length;
                    ms.Seek(pos, SeekOrigin.Begin);
                    ms.WriteByte(0);
                    ms.WriteByte(0);
                    ms.WriteByte(0);
                    ms.WriteByte(0);
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(ms, message);
                    ms.Seek(pos, SeekOrigin.Begin);
                    ms.Write(BitConverter.GetBytes(ms.Length - 4 - pos), 0, 4);
                    ms.Seek(0, SeekOrigin.End);
                }
                client.Send(ms.ToArray());
            }
        }


        private static void MessageRecieved(DataPackageInfo obj)
        {
            Console.WriteLine("I reecieve message {0} - {1}", (obj.Data as Message).key, (obj.Data as Message).data);
        }

        private static void EasyClient_Error(object sender, ErrorEventArgs e)
        {
            Console.WriteLine("Возникла ошибка: {0}",e.Exception.Message);
        }

        private static void EasyClient_Connected(object sender, EventArgs e){
            Console.WriteLine("Клиент подключен");
            Random rnd = new Random();
            for (int i = 0; i < 100; i++)
            {
                SendMessage(new Message()
                {
                    key = "myheader" + rnd.Next(100000),
                    data = i
                });
            }

        }

        private static void EasyClient_Closed(object sender, EventArgs e) {
            Console.WriteLine("Клиент отключен");
        }
    }
}
