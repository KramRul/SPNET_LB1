using ServerApp;
using SocketObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WinService
{
    public partial class Service1 : ServiceBase
    {
        private ServerObject serverApp;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            serverApp = new ServerObject(8888);
            var listenThread = new Thread(new ThreadStart(serverApp.Listen));
            listenThread.Start(); //старт потока
        }

        protected override void OnStop()
        {
            serverApp.Disconnect();
        }
    }
}
