using CatalogPC.Components;
using CommunityToolkit.Mvvm.ComponentModel;
using FuzzySharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using static CatalogPC.Components.Processor;

namespace CatalogPC.Models
{
    internal class ModelOne
    {
        public ModelOne()
        {

            var cpuPath = Path.Combine(AppContext.BaseDirectory, "Data", "CPU");
            var gpuPath = Path.Combine(AppContext.BaseDirectory, "Data", "GPU");
            var boardPath = Path.Combine(AppContext.BaseDirectory, "Data", "Motherboard");
            
            SuitableComponents.Clear();
            if (Directory.Exists(cpuPath))
            {
                foreach (string file in Directory.GetFiles(cpuPath))
                {
                    string jsonText = File.ReadAllText(file);
                    var component = JsonSerializer.Deserialize<Processor.CPU>(jsonText);
                    if (component != null)
                        ComponentsCPU.Add(component);
                }
            }

            if (Directory.Exists(gpuPath))
            {
                foreach (string file in Directory.GetFiles(gpuPath))
                {
                    string jsonText = File.ReadAllText(file);
                    var component = JsonSerializer.Deserialize<Videocard.GPU>(jsonText);
                    if (component != null)
                        ComponentsGPU.Add(component);
                }
            }

            if (Directory.Exists(boardPath))
            {
                foreach (string file in Directory.GetFiles(boardPath))
                {
                    string jsonText = File.ReadAllText(file);
                    var component = JsonSerializer.Deserialize<Motherboard.Board>(jsonText);
                    if (component != null)
                        ComponentsBoard.Add(component);
                }
            }
        }
        //список параметров по которым судить совместимоть комплектующих

        //модель и сокет процессора
        //поддержка поколения и сокета процессора материнской платой
        //поддержка частоты и DDR оперативной памяти материнской платой

        // List<Component> components = new List<Component>();
        //List<Processor> processors = new List<Processor>();
        //List<Motherboard> motherboards = new List<Motherboard>();
        //List<RAM> rams = new List<RAM>();
        public abstract class Component
        {
            public string Name { get; set; }
        }

        public List<Component> ComponentsCPU = new List<Component>();

        public List<Component> ComponentsGPU = new List<Component>();

        public List<Component> ComponentsBoard = new List<Component>();


        public List<Component> SuitableComponents = new List<Component>();

        
    }
}
