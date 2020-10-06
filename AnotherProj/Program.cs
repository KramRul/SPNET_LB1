using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AnotherProj
{
    class Program
    {
        static void Main(string[] args)
        {
            MakeNewDomain();
            Console.ReadLine();

        }
        static void MakeNewDomain()
        {
            // Создадим новый домен приложения
            AppDomain newD = AppDomain.CreateDomain("SPNETLAB1WebAppDomain");
            var assemblyName = AssemblyName.GetAssemblyName(@"C:\Users\KramRul\source\repos\SPNET_LB1\SPNET_LB1\bin\Debug\SPNET_LB1.exe");
            newD.Load(assemblyName);
            newD.ExecuteAssemblyByName(assemblyName);
            InfoDomainApp(newD);
            // Уничтожение домена приложения
            AppDomain.Unload(newD);
        }

        static void InfoDomainApp(AppDomain defaultD)
        {
            Console.WriteLine("*** Информация о домене приложения ***\n");
            Console.WriteLine("-> Имя: {0}\n-> ID: {1}\n-> По умолчанию? {2}\n-> Путь: {3}\n",
                defaultD.FriendlyName, defaultD.Id, defaultD.IsDefaultAppDomain(), defaultD.BaseDirectory);

            Console.WriteLine("Загружаемые сборки: \n");
            // Извлекаем информацию о загружаемых сборках с помощью LINQ-запроса
            var infAsm = from asm in defaultD.GetAssemblies()
                         orderby asm.GetName().Name
                         select asm;
            foreach (var a in infAsm)
                Console.WriteLine("-> Имя: \t{0}\n-> Версия: \t{1}", a.GetName().Name, a.GetName().Version);
        }
    }
}
