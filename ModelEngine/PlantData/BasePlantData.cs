using ModelEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelEngine
{
    public class BasePlantData
    {
        Port_Material port = new();
        public BasePlantData()
        {
        }

        public Port_Material Port { get => port; set => port = value; }
    }
}
