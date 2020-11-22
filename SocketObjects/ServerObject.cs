using LoggerApp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketObjects
{
    public class ServerObject
    {
        private readonly int port;
        private TcpClient _tcpClient;

        public ServerObject(int _port)
        {
            port = _port;
        }

        static TcpListener tcpListener; // сервер для прослушивания
        List<ClientObject> clients = new List<ClientObject>(); // все подключения

        protected internal void AddConnection(ClientObject clientObject)
        {
            clients.Add(clientObject);
        }
        protected internal void RemoveConnection(string id)
        {
            // получаем по id закрытое подключение
            ClientObject client = clients.FirstOrDefault(c => c.Id == id);
            // и удаляем его из списка подключений
            if (client != null)
                clients.Remove(client);
        }
        // прослушивание входящих подключений
        public void Listen()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, port);
                tcpListener.Start();
                Console.WriteLine("Сервер запущен. Ожидание подключений...");
                Debug.WriteLine("Сервер запущен. Ожидание подключений...");
                Logger.Info("Сервер запущен. Ожидание подключений...");
                while (true)
                {
                    _tcpClient = tcpListener.AcceptTcpClient();

                    ClientObject clientObject = new ClientObject(_tcpClient, this);
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Debug.WriteLine(ex.Message);
                Disconnect();
            }
        }

        // трансляция сообщения подключенным клиентам
        protected internal void BroadcastMessage(string message, string id)
        {
            Debug.WriteLine("трансляция сообщения подключенным клиентам");
            Console.WriteLine("трансляция сообщения подключенным клиентам");
            Debug.WriteLine($"message {message}");
            Console.WriteLine($"message {message}");
            byte[] data = Encoding.Unicode.GetBytes(message);
            Debug.WriteLine(clients.Count);
            Console.WriteLine(clients.Count);
            for (int i = 0; i < clients.Count; i++)
            {
                Debug.WriteLine($"трансляция {i} клиенту");
                Console.WriteLine($"трансляция {i} клиенту");
                Debug.WriteLine($"data.Length {data.Length}");
                Console.WriteLine($"data.Length {data.Length}");
                //if (clients[i].Id != id) // если id клиента не равно id отправляющего
                //{
                var stream = _tcpClient.GetStream();
                stream.Write(data, 0, data.Length);
                //clients[i].Stream.Write(data, 0, data.Length); //передача данных
                //}
            }
        }
        // отключение всех клиентов
        public void Disconnect()
        {
            tcpListener.Stop(); //остановка сервера

            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Close(); //отключение клиента
            }
            Environment.Exit(0); //завершение процесса
        }
    }
}
