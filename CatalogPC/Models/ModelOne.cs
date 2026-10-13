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
        List<Processor> processors = new List<Processor>();
        List<Motherboard> motherboards = new List<Motherboard>();
        List<RAM> rams = new List<RAM>();

        public bool CompatibilityProcessor = false;
        public bool CompatibilityMotherBoard = false;

        public void CompatibilityTest(Processor processor, Motherboard motherboard)
        {
            if (processors.Count == 0 || motherboards.Count == 0)
            {
                Console.WriteLine("Нет комплектующих для проверки совместимости");
                return;
            }
            else if (processor.Socket == motherboard.Socket)
            {
                Console.WriteLine("Сокет подходит");
            }
            foreach (int i in motherboard.Gen) {
                if (processor.Gen == i) {
                    Console.WriteLine("Поколение подходит");
                    break;
                }
                else
                {
                    continue;
                }
            }
        }

        public class Processor
        {
            public string Name { get; set; }
            public string Socket { get; set; }
            public int Gen { get; set; }
            public string Frequency { get; set; }
            public string DDR { get; set; }
        }

        public class Motherboard
        {
            public string Name { get; set; }
            public string Socket { get; set; }
            public int[] Gen { get; set; }
            public string Frequency { get; set; }
            public string DDR { get; set; }
        }
        new class RAM
        {
            public string Name { get; set; }
            public string Frequency { get; set; }
            public string DDR { get; set; }
        }
        public void AddProcessor(string name, string socket, int gen, string frequency, string ddr)
        {
            processors.Add(new Processor
            {
                Name = name,
                Socket = socket,
                Gen = gen,
                Frequency = frequency,
                DDR = ddr
            });
        }
        public void AddMotherboard(string name, string socket, int[] gen, string frequency, string ddr)
        {
            motherboards.Add(new Motherboard
            {
                Name = name,
                Socket = socket,
                Gen = gen,
                Frequency = frequency,
                DDR = ddr
            });
        }
        public void AddRAM(string name, string frequency, string ddr)
        {
            rams.Add(new RAM
            {
                Name = name,
                Frequency = frequency,
                DDR = ddr
            });
        }
    }
}
