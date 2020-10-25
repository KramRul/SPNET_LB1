using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Task1;

namespace LateBinding
{
    class Program
    {
        static void Main(string[] args)
        {
            DynamicData();

            Binding();
        }
        static void Binding()
        {
            Assembly assembly = null;
            try
            {
                var assemblyName = AssemblyName.GetAssemblyName(@"C:\Users\KramRul\source\repos\SPNET_LB1\SPNET_LB1\bin\Debug\Task1.dll");
                assembly = Assembly.Load(assemblyName);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            if (assembly != null)
                CreateBinding(assembly);
            Console.ReadLine();
        }
        static void CreateBinding(Assembly assembly)
        {
            try
            {
                Type taxOffice = assembly.GetType("Task1.TaxOffice");

                // Используем позднее связывание
                object obj = Activator.CreateInstance(taxOffice);
                Console.WriteLine(obj.ToString());
                Console.WriteLine("Объект создан!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void DynamicData()
        {
            dynamic taxOffice = "taxOffice";
            Console.WriteLine(taxOffice);
            taxOffice = new TaxOffice();
            taxOffice.GetAllTaxPayers();
            dynamic taxPayer = new System.Dynamic.ExpandoObject();
            taxPayer.Id = Guid.NewGuid();
            taxPayer.Name = "Tax Payer 1";
            taxPayer.Address = "Address 1";
            taxPayer.Age = 20;
            
            taxPayer.IncrementAge = (Action<int>)(x => taxPayer.Age += x);
            taxPayer.IncrementAge(6);
            Console.WriteLine($"{taxPayer.Name} - {taxPayer.Age} - {taxPayer.Address}");

            Console.ReadLine();
        }
    }
}
