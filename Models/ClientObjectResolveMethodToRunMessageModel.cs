using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ClientObjectResolveMethodToRunMessageModel
    {
        public Dictionary<string, dynamic> Methods { get; set; }
        public ClientObjectResolveMethodToRunMessageModel()
        {
            Methods = new Dictionary<string, dynamic>();
        }
    }
}
