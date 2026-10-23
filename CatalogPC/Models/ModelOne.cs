using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogPC.Models
{
    internal class ModelOne
    {
        //список параметров по которым судить совместимоть комплектующих

        //модель и сокет процессора
        //поддержка поколения и сокета процессора материнской платой
        //поддержка частоты и DDR оперативной памяти материнской платой

        // List<Component> components = new List<Component>();
        //List<Processor> processors = new List<Processor>();
        //List<Motherboard> motherboards = new List<Motherboard>();
        //List<RAM> rams = new List<RAM>();

        public bool CompatibilityProcessor = false;
        public bool CompatibilityMotherBoard = false;
    }
}
