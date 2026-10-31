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
            string Name { get; set; }
        }
        public List<Processor.CPU> proc = new List<Processor.CPU>();
        public List<Component> Components = new List<Component>();
        public List<Component> SuitableComponents = new List<Component>();

        int file_count;

        public void NameSearch(string? Name)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "Data", "CPU");
            Components.Clear();
            file_count = Directory.EnumerateFiles(path).Count();
            string[] files_ = Directory.GetFiles(path);

            for (int i = 0; i < file_count; i++) {
                var component = JsonSerializer.Deserialize<Processor.CPU>(File.ReadAllText(files_[i]));
                Components.Add(component);                                                                
            }

            file_count = 0;

            path = Path.Combine(AppContext.BaseDirectory, "Data", "GPU");
            file_count = Directory.EnumerateFiles(path).Count();
            files_ = Directory.GetFiles(path);

            for (int i = 0; i < file_count; i++)                                                          
            {                                                                                             
                var component = JsonSerializer.Deserialize<Videocard.GPU>(File.ReadAllText(files_[i]));
                Components.Add(component);                                                                
            }

            //foreach (var CPU in proc) { 
            //    Components.Add(CPU);
            //}



            for (int i = 0; i < file_count; i++)
            {
                var component = JsonSerializer.Deserialize<Component>(File.ReadAllText(files_[i]));
                //var result = Fuzz.PartialRatio(Name, File.ReadAllText(files_[i]));
                var result = 0;
                var result_2 = 0;

                if (component is Processor.CPU cpu)
                {
                    result = Fuzz.PartialRatio(Name, cpu.Metadata.Name);
                    result_2 = Fuzz.PartialRatio(Name, cpu.Socket);
                    if (result >= 80 || result_2 >= 90)
                    {
                        SuitableComponents.Add(component);
                    }
                }
                if (component is Videocard.GPU gpu)
                {
                    result = Fuzz.PartialRatio(Name, gpu.Metadata.Name);
                    result_2 = Fuzz.PartialRatio(Name, gpu.Interface);
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
