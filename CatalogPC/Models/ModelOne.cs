using CommunityToolkit.Mvvm.ComponentModel;
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

        public bool? OnlyCPU = false;

        public bool? OnlyGPU = false;

        public bool? OnlyBoard = false;


        public void NameSearch(string? Name)
        {
            Debug.WriteLine($"Ищем: '{Name}'");

            SuitableComponents.Clear();

            if (OnlyCPU == true) { SearchCPUs(Name);  }
            if (OnlyGPU == true) { SearchGPUs(Name); }
            if (OnlyBoard == true) { SearchBoards(Name); }
            else
            {
                Debug.WriteLine("Ничего не выбрано");
                return;
            }

        }

        public void SearchCPUs(string? Name)
        {
            foreach (Component component in ComponentsCPU)
            {
                if (component is Processor.CPU cpu)
                {
                    var name = Name ?? string.Empty;
                    string cpuName = cpu.Metadata?.Name ?? string.Empty;
                    string cpuSocket = cpu.Socket ?? string.Empty;

                    var result = Fuzz.PartialRatio(Name ?? string.Empty, cpuName);
                    var result_2 = Fuzz.PartialRatio(Name ?? string.Empty, cpuSocket);
                    if (result >= 80 || result_2 >= 90) SuitableComponents.Add(component);

                }       
            }
            Debug.WriteLine($"количество {ComponentsCPU.Count}");
            Debug.WriteLine($"количество в списке ответов {SuitableComponents.Count}");
        }

        public void SearchGPUs(string? Name)
        {
            foreach (Component component in ComponentsGPU)
            {
                if (component is Videocard.GPU gpu)
                {
                    var name = Name ?? string.Empty;
                    string gpuName = gpu.Metadata?.Name ?? string.Empty;
                    string gpuInterface = gpu.Interface ?? string.Empty;

                    var result = Fuzz.PartialRatio(Name ?? string.Empty, gpuName);
                    var result_2 = Fuzz.PartialRatio(Name ?? string.Empty, gpuInterface);
                    if (result >= 70 || result_2 >= 80) SuitableComponents.Add(component);
                }
            }
            Debug.WriteLine($"количество {ComponentsGPU.Count}");
            Debug.WriteLine($"количество в списке ответов {SuitableComponents.Count}");
        }

        public void SearchBoards(string? Name)
        {
            foreach (Component component in ComponentsBoard)
            {
                if (component is Motherboard.Board board)
                {
                    var name = Name ?? string.Empty;
                    string boardName = board.Metadata?.Name ?? string.Empty;
                    string BoardSocket = board.Socket ?? string.Empty;

                    var result = Fuzz.PartialRatio(Name ?? string.Empty, boardName);
                    var result_2 = Fuzz.PartialRatio(Name ?? string.Empty, BoardSocket);
                    if (result >= 80 || result_2 >= 90) SuitableComponents.Add(component); 
                }
            }
            Debug.WriteLine($"количество {ComponentsBoard.Count}");
            Debug.WriteLine($"количество в списке ответов {SuitableComponents.Count}");
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
                Debug.WriteLine($"количество в списке ответов {SuitableComponents.Count}");
            }
            finally
            {
                Debug.WriteLine("Позиция успешно найдена");
            }
        }
    }
}
