using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace CatalogPC.Models
{
    internal class Processor
    {
        public class CPU
        {
            [JsonPropertyName("opendb_id")] public string ProcessorId { get; set; }
            [JsonPropertyName("socket")] public string Socket {  get; set; }
            [JsonPropertyName("cores")] public CpuCores Cores { get; set; }

            [JsonPropertyName("specifications")] public CpuSpecs Specs { get; set; }
            [JsonPropertyName("metadata")] public Meta Metadata { get; set; }
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
        //016d1180-e254-43da-ad22-67895ac633b6.json
        //007a5a23-1e12-45b7-90d0-565b80876539.json
        public void Search(string? NameCPU)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "Data", "CPU", NameCPU);
            var cpu = JsonSerializer.Deserialize<CPU>(File.ReadAllText(path));
            Debug.WriteLine($"{cpu.Metadata.Name}: {cpu.Socket}: {cpu.Cores.Total}/{cpu.Cores.Threads}, {string.Join(",", cpu.Specs.Memory.Types)}");
        }

        public void test(string? NameCPU)
        {
            try {
                Search(NameCPU);
            }
            catch {
                Debug.WriteLine("У нас исключение братуха");
            }
            finally {
                Debug.WriteLine("Успешный успех");
            }
        }

    }
}
