using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class UInfoAttribute : System.Attribute
    {
        public string Desc;
        public UInfoAttribute() { }
        public UInfoAttribute(string str)
        {
            Desc = str;
        }
    }
}
