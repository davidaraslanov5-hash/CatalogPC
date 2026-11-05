using CatalogPC.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CatalogPC.Components
{
    class RAM 
    {
        public class Memory : ModelOne.Component
        {
            [JsonPropertyName("speed")] public int Speed { get; set; }
            [JsonPropertyName("ram_type")] public string MemoryType { get; set; }
            [JsonPropertyName("metadata")] public Meta Metadata { get; set; }
        }
        public class Module
        {
            [JsonPropertyName("capacity_gb")] public int CapacityGB_x1 { get; set; }
        }

        public class Meta
        {
            [JsonPropertyName("name")] public string Name { get; set; }
            [JsonPropertyName("manufacturer")] public string Manufacturer { get; set; }
        }
    }
}
