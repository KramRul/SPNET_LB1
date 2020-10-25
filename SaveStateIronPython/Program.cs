using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;

namespace SaveStateIronPython
{
    class Program
    {
        static void Main(string[] args)
        {
            ScriptEngine engine = Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();
            engine.ExecuteFile(@"C:\Users\KramRul\source\repos\SPNET_LB1\SaveStateIronPython\SaveState.py", scope);
            dynamic fileLines = scope.GetVariable("fileLines");
            dynamic fileLinesWrite = scope.GetVariable("fileLinesWrite");
            
            foreach (var line in fileLines)
            {
                Console.WriteLine(line);
            }
            foreach (var line in fileLinesWrite)
            {
                Console.WriteLine(line);
            }
            Console.ReadLine();
        }
    }
}
