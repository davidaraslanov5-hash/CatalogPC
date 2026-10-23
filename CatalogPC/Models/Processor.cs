using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using static CatalogPC.Models.Processor;
namespace CatalogPC.Models
{
    internal class Processor
    {

        string count_cpu = @"C:\Users\User\source\repos\CatalogPC\CatalogPC\Data\CPU";
        int file_count;
        public Processor() {


        }
        public class CPU
        {
            [JsonPropertyName("opendb_id")] public string ProcessorId { get; set; }
            [JsonPropertyName("socket")] public string Socket {  get; set; }
            [JsonPropertyName("cores")] public CpuCores Cores { get; set; }

            [JsonPropertyName("specifications")] public CpuSpecs Specs { get; set; }
            [JsonPropertyName("metadata")] public Meta Metadata { get; set; }

            [JsonPropertyName("series")] public string Series { get; set; }
        }

        public class CpuCores
        {
            [JsonPropertyName("total")] public int Total { get; set; }
            [JsonPropertyName("threads")] public int Threads { get; set; }
        }

        public class CpuSpecs
        {
            [JsonPropertyName("memory")] public CpuMemory Memory { get; set; }
        }

        public class CpuMemory
        {
            [JsonPropertyName("types")] public List<string> Types { get; set; }
        }

        public class Meta
        {
            [JsonPropertyName("name")] public string Name { get; set; }
        }

        public List<CPU> SuitableOptions = new List<CPU>();
        public void NameSearch(string? NameCPU)
        {
            SuitableOptions.Clear();
            file_count = Directory.EnumerateFiles(count_cpu).Count();
            string[] files_cpu = Directory.GetFiles(count_cpu);
            for(int i = 0; i <= file_count; i++)
            {
                var cpu = JsonSerializer.Deserialize<CPU>(File.ReadAllText(files_cpu[i]));
                if (cpu.Metadata.Name == NameCPU)  // такс, тут мы добавляем в список подходящий процессор по введенному названию 
                {
                    SuitableOptions.Add(cpu); 
                    break;
                }else if(cpu.Series == NameCPU || cpu.Socket == NameCPU) // а тут добавляем все процессоры подходящие по сокету или по серии
                {
                    SuitableOptions.Add(cpu);
                }
            }
        }

        public void test(string? NameCPU)
        {
            try {
                NameSearch(NameCPU);
            }
            catch {
                Debug.WriteLine("Позиция не найдена");
            }
            finally
            {
                foreach (var i in SuitableOptions)
                {
                    Debug.WriteLine($"{i.Metadata.Name}: {i.Socket}: Ядра:{i.Cores.Total}/ Потоки:{i.Cores.Threads}, {string.Join(",", i.Specs.Memory.Types)}");
                }
                //Debug.WriteLine("Результаты");
            }
        }

        //public void Search(string? NameCPU)
        //{
        //    var path = Path.Combine(AppContext.BaseDirectory, "Data", "CPU", NameCPU);
        //    var cpu = JsonSerializer.Deserialize<CPU>(File.ReadAllText(path));
        //    Debug.WriteLine($"{cpu.Metadata.Name}: {cpu.Socket}: {cpu.Cores.Total}/{cpu.Cores.Threads}, {string.Join(",", cpu.Specs.Memory.Types)}");
        //}

    }
}
