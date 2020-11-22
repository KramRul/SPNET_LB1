using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ClientApp.Services
{
    public class ClientService
    {
        private const string host = "127.0.0.1";
        private const int port = 8888;
        static TcpClient client;
        static NetworkStream stream;
        public string ReceiveMessageData = String.Empty;

        public ClientService()
        {
            Init();
        }
        void Init()
        {
            client = new TcpClient();
            try
            {
                client.Connect(host, port); //подключение клиента
                stream = client.GetStream(); // получаем поток

                // запускаем новый поток для получения данных
                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start(); //старт потока
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        // отправка сообщений
        public void SendMessage(string message)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }
        // получение сообщений
        private void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    Debug.WriteLine("Start reading messages!");
                    byte[] data = new byte[64]; // буфер для получаемых данных
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = builder.ToString();
                    ReceiveMessageData = message;//вывод сообщения
                    Debug.WriteLine("End reading messages!");
                }
                catch
                {
                    Debug.WriteLine("Подключение прервано!"); //соединение было прервано
                    Disconnect();
                }
            }
        }

        public void Disconnect()
        {
            if (stream != null)
                stream.Close();//отключение потока
            if (client != null)
                client.Close();//отключение клиента
            Environment.Exit(0); //завершение процесса
        }
    }
}