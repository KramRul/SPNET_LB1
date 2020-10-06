using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task1;

namespace SPNET_LB1
{
    class Program
    {
        static void Main(string[] args)
        {
            var taxOffice = new TaxOffice();
            taxOffice.GetAllTaxPayers();
            taxOffice.CollectTaxes();

            InfoDomainApp();
            Console.ReadLine();
        }

        static void InfoDomainApp()
        {
            AppDomain defaultD = AppDomain.CurrentDomain;

            Console.WriteLine("*** Информация о домене приложения ***\n");
            Console.WriteLine("-> Имя: {0}\n-> ID: {1}\n-> По умолчанию? {2}\n-> Путь: {3}\n",
                defaultD.FriendlyName, defaultD.Id, defaultD.IsDefaultAppDomain(), defaultD.BaseDirectory);

            Console.WriteLine("Загружаемые сборки: \n");
            
            var infAsm = from asm in defaultD.GetAssemblies()
                         orderby asm.GetName().Name
                         select asm;
            foreach (var a in infAsm)
                Console.WriteLine("-> Имя: \t{0}\n-> Версия: \t{1}", a.GetName().Name, a.GetName().Version);
        }
    }
}
