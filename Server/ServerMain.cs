using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Core;
using log4net;
using Server.SuperSocketObjects;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Logging;

namespace Server
{
    class ServerMain
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void Main(string[] args) {
            MyServer server = new MyServer();
            
            if (!server.Setup(new ServerConfig()
            {
                Port = 2021,
                Name = "MyServer",
                MaxRequestLength = 1024*1024*100,
                Mode = SocketMode.Tcp,
                TextEncoding = "UTF-8",
                Ip = "any"
                },null,null,
                new ConsoleLogFactory()))
            {

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ошибка инициализации сервера!");
                Console.ResetColor();
                Console.ReadKey();
                return;
            }
            server.NewRequestReceived += Server_NewRequestReceived;

            if (server.Start())
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("{0}!", "Сервер запущен");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("{0}!", "Неудачный запуск сервера");
                return;
            }
            Console.ResetColor();


            Console.WriteLine();
            Console.WriteLine("-----------------------------");
            Console.WriteLine("- Нажми 'Q' для остановки сервера! -");
            Console.WriteLine("-----------------------------");
            Console.WriteLine();

            ConsoleKey keyInfo = new ConsoleKey();
            do
            {
                keyInfo = Console.ReadKey().Key;
                /*switch (keyInfo)
                {
                    
                }*/
            }
            while (keyInfo != ConsoleKey.Q) ;


            server.Stop();

            Console.WriteLine("Сервер остановлен!");
            Console.ReadKey();
        }

        private static void Server_NewRequestReceived(SessionX session, DataRequestInfo requestInfo)
        {
            Console.WriteLine("Клиент {0} прислал данные {1}  {2} ",session.SessionID,requestInfo.Key,requestInfo.message.data);

            session.SendMessage(requestInfo.message);

        }
    }
}
