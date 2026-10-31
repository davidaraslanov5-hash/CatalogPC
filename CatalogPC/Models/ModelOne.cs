using FuzzySharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using static CatalogPC.Models.Processor;

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
        private Models.Processor processor;
        public abstract class Component
        {
            public string Name { get; set; }
        }

        public List<Component> Components = new List<Component>();
        public List<Component> SuitableComponents = new List<Component>();

        int file_count;

        public void NameSearch(string? Name)
        {
            var cpuPath = Path.Combine(AppContext.BaseDirectory, "Data", "CPU");
            var gpuPath = Path.Combine(AppContext.BaseDirectory, "Data", "GPU");

            if (Directory.Exists(cpuPath))
            {
                foreach (string file in Directory.GetFiles(cpuPath, "*.json"))
                {
                    string jsonText = File.ReadAllText(file);
                    var component = JsonSerializer.Deserialize<Processor.CPU>(jsonText);
                    if (component != null)
                        Components.Add(component);
                }
            }

            if (Directory.Exists(gpuPath))
            {
                foreach (string file in Directory.GetFiles(gpuPath, "*.json"))
                {
                    string jsonText = File.ReadAllText(file);
                    var component = JsonSerializer.Deserialize<Videocard.GPU>(jsonText);
                    if (component != null)
                        Components.Add(component);
                }
            }

            foreach (Component component in Components)
            { 
                if (component is Processor.CPU cpu)
                {
                    string cpuName = cpu.Metadata?.Name ?? string.Empty;
                    string cpuSocket = cpu.Socket ?? string.Empty;

                    var result = Fuzz.PartialRatio(Name, cpuName);
                    var result_2 = Fuzz.PartialRatio(Name, cpuSocket);
                    if (result >= 80 || result_2 >= 90)
                    {
                        SuitableComponents.Add(component);
                    }
                }
                else if (component is Videocard.GPU gpu)
                {
                    string gpuName = gpu.Metadata?.Name ?? string.Empty;
                    string gpuInterface = gpu.Interface ?? string.Empty;

                    var result = Fuzz.PartialRatio(Name, gpuName);
                    var result_2 = Fuzz.PartialRatio(Name, gpuInterface);
                    if (result >= 80 || result_2 >= 90)
                    {
                        SuitableComponents.Add(component);
                    }
                }
            }
        }
        public void ExceptionHandling(string? NameCPU)
        {
            try
            {
                NameSearch(NameCPU);
                
            }
            catch
            {
                Debug.WriteLine("Позиция не найдена");
                Debug.WriteLine($"количество JSON {file_count}");
                Debug.WriteLine($"количество {Components.Count}");
                Debug.WriteLine($"количество в списке ответов {SuitableComponents.Count}");
            }
            finally
            {
                Debug.WriteLine("Позиция успешно найдена");
            }
        }
    }
}
