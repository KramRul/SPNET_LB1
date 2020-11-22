using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Web;
using System.Text;
using Newtonsoft.Json;
using Models;
using System.Diagnostics;
using System.Threading.Tasks;
using Task1;
using LoggerApp;

namespace SocketObjects
{
    public class ClientObject
    {
        protected internal string Id { get; private set; }
        protected internal NetworkStream Stream { get; private set; }
        TcpClient client;
        ServerObject server; // объект сервера

        public ClientObject(TcpClient tcpClient, ServerObject serverObject)
        {
            Id = Guid.NewGuid().ToString();
            client = tcpClient;
            server = serverObject;
            serverObject.AddConnection(this);
        }

        public void Process()
        {
            try
            {
                Debug.WriteLine("ClientObject Process");
                Console.WriteLine("ClientObject Process");
                Stream = client.GetStream();
                // получаем имя пользователя
                string message = String.Empty;

                // в бесконечном цикле получаем сообщения от клиента
                while (true)
                {
                    try
                    {
                        Debug.WriteLine("ClientObject Process чтение сообщения");
                        Console.WriteLine("ClientObject Process чтение сообщения");
                        message = GetMessage();
                        Debug.WriteLine(message);
                        Console.WriteLine(message);
                        var model = JsonConvert.DeserializeObject<ClientObjectResolveMethodToRunMessageModel>(message);
                        List<object> resultModel = new List<object>();
                        Logger.Info("Start to handle Methods");
                        foreach (var method in model.Methods)
                        {
                            switch (method.Key)
                            {
                                case "GetRevenues":
                                    Logger.Info("Handle GetRevenues");
                                    var taxOffice = new TaxOffice();
                                    var result = taxOffice.GetAllRevenues();
                                    Logger.Info($"result {result.Count}");
                                    foreach (var item in result)
                                    {
                                        resultModel.Add(item);
                                    }
                                    Logger.Info($"Handled GetRevenues");
                                    break;
                                case "CollectTaxes":
                                    Logger.Info("Handle CollectTaxes");
                                    taxOffice = new TaxOffice();
                                    var collectTaxesResult = taxOffice.CollectTaxes();
                                    Logger.Info($"collectTaxesResult {collectTaxesResult.Count}");
                                    foreach (var item in collectTaxesResult)
                                    {
                                        resultModel.Add(item);
                                    }
                                    Logger.Info($"Handled CollectTaxes");
                                    break;
                                default:
                                    break;
                            }
                        }
                        Logger.Info($"Ended handle Methods");
                        var resultModelString = JsonConvert.SerializeObject(resultModel);

                        Debug.WriteLine("Подготовка к отправке ответа");
                        Console.WriteLine("Подготовка к отправке ответа");

                        Debug.WriteLine("Отправка ответа");
                        Console.WriteLine("Отправка ответа");
                        server.BroadcastMessage(resultModelString, this.Id);

                        Debug.WriteLine("Завершение отправки");
                        Console.WriteLine("Завершение отправки");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        Console.WriteLine(ex.Message);
                        Logger.Info(ex.Message);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                // в случае выхода из цикла закрываем ресурсы
                server.RemoveConnection(this.Id);
                Close();
            }
        }

        // чтение входящего сообщения и преобразование в строку
        private string GetMessage()
        {
            byte[] data = new byte[64]; // буфер для получаемых данных
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (Stream.DataAvailable);

            return builder.ToString();
        }

        // закрытие подключения
        protected internal void Close()
        {
            if (Stream != null)
                Stream.Close();
            if (client != null)
                client.Close();
        }
    }
}