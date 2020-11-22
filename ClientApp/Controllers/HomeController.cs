using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using ClientApp.Services;
using Models;
using Newtonsoft.Json;
using Task1;

namespace ClientApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Messages = new List<string>();
            return View();
        }

        public ActionResult Revenues()
        {
            ViewBag.Revenues = new List<object>() { };
            var clientService = new ClientService();
            var modelToSend = new ClientObjectResolveMethodToRunMessageModel();
            modelToSend.Methods.Add("GetRevenues", null);
            string json = JsonConvert.SerializeObject(modelToSend);
            clientService.SendMessage(json);
            Thread.Sleep(1000);
            ViewBag.Revenues = JsonConvert.DeserializeObject<List<Revenue>>(clientService.ReceiveMessageData);
            return View("Revenues");
        }

        [HttpPost]
        public ActionResult CollectTaxes()
        {
            if (ModelState.IsValid)
            {
                var clientService = new ClientService();
                var modelToSend = new ClientObjectResolveMethodToRunMessageModel();
                modelToSend.Methods.Add("CollectTaxes", null);
                string json = JsonConvert.SerializeObject(modelToSend);
                clientService.SendMessage(json);
                Thread.Sleep(1000);
                ViewBag.Messages = JsonConvert.DeserializeObject<List<string>>(clientService.ReceiveMessageData);
                return View("Index");
            }
            ViewBag.Message = "Collect Taxes failed!";
            return View();
        }
    }
}