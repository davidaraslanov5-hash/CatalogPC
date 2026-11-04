using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using CatalogPC.Models;
using FuzzySharp;

namespace CatalogPC.Components
{
    internal class Processor
    {
        public class CPU : ModelOne.Component
        {
            [JsonPropertyName("opendb_id")] public string ProcessorId { get; set; }
            [JsonPropertyName("socket")] public string Socket { get; set; }
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
    }

}