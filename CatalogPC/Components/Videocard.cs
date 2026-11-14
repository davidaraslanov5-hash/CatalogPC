using CatalogPC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CatalogPC.Components
{
    internal class Videocard
    {
        public class GPU : ModelOne.Component
        {
            [JsonPropertyName("opendb_id")] public string GPUid { get; set; }

            [JsonPropertyName("memory")] public int Memory { get; set; }
            [JsonPropertyName("memory_bus")] public int MemoryBus { get; set; }
            [JsonPropertyName("memory_type")] public string MemoryType { get; set; }

            [JsonPropertyName("interface")] public string Interface { get; set; }

            [JsonPropertyName("tdp")] public int? Tdp { get; set; }
            [JsonPropertyName("metadata")] public Meta Metadata { get; set; }
            [JsonPropertyName("general_product_information")] public GPUInfo Info { get; set; }

        }

        public class Meta
        {
            [JsonPropertyName("name")] public string Name { get; set; }
            [JsonPropertyName("releaseYear")] public int? ReleaseYear { get; set; }
        }

        public class GPUInfo
        {
            [JsonPropertyName("manufacturer_url")] public Uri URL { get; set; }
        }
    }
}
