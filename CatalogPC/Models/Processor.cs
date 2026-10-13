using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
namespace CatalogPC.Models
{
    internal class Processor
    {
        public class GPU
        {
            [JsonPropertyName("opendb_id")] public string ProcessorId { get; set; }
            [JsonPropertyName("socket")] public string Socket {  get; set; }
            [JsonPropertyName("cores")] public CpuCores Cores { get; set; }

        }
    }
}
