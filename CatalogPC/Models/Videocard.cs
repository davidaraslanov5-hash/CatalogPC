using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CatalogPC.Models
{
    internal class Videocard
    {
        public class GPU
        {
            [JsonPropertyName("opendb_id")] public string GPUid { get; set; }
            [JsonPropertyName("memory")] public int Memory { get; set; }

            [JsonPropertyName("metadata")] public string Metadata { get; set; }
        }
    }
}
