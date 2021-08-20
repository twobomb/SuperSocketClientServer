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

        public static void Main(string[] args) {
            MyServer server = new MyServer();
            
            if (!server.Setup(new ServerConfig()
            {
                Port = 2021,
                KeepAliveTime = 10,
                Name = "MyServer",
                SendTimeOut = 10000,//ms
                MaxConnectionNumber = 150,//Максимальное количество одновременных подключений
                MaxRequestLength = 1024*1024,//Максимально допустимая длина запроса 
                SendingQueueSize = 100,//максимальный размер очереди отправки
                DisableSessionSnapshot = true,// ВАЖНО если не отключить, то снимок сеансов делается раз в sessionSnapshotInterval (5сек по умолчанию), и метод GetAllSession() выдает не актуальные данные пока не сделется новый снимок
                Mode = SocketMode.Tcp,
                TextEncoding = "UTF-8",
                Ip = "any"
                },null,null,
                new ConsoleLogFactory()))//Логирование в консоль
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
                switch (keyInfo) {
                    case ConsoleKey.C:
                        Console.WriteLine("");
                        Console.WriteLine("Активных сессий: {0} ",server.SessionCount);
                        foreach (var sessionXe in server.GetAllSessions()) 
                            Console.WriteLine("\t{0} {1}",sessionXe.SessionID,sessionXe.SocketSession.Client.RemoteEndPoint);
                        break;
                }
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
