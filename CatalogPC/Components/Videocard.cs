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
            [JsonPropertyName("power_connectors")] public Power PowerInfo { get; set; }

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

        public class Power
        {
            [JsonIgnore]
            public string PowerConnectors
            {
                get
                {
                    var connectors = new List<string>();

                    if (Pin_6 > 0)
                        connectors.Add(" 6-Pin");
                    if (Pin_8 > 0)
                        connectors.Add(" 8-Pin");
                    if (Pin_12 > 0)
                        connectors.Add(" 12VHPWR");
                    if (Pin_12_2x6 > 0)
                        connectors.Add(" 12V-2x6");
                    return connectors.Count == 0
                        ? "Нет питания"
                        : string.Join(", ", connectors);
                }
            }
            [JsonPropertyName("pcie_6_pin")] public int? Pin_6 { get; set; }
            [JsonPropertyName("pcie_8_pin")] public int? Pin_8 { get; set; }
            [JsonPropertyName("pcie_12VHPWR")] public int? Pin_12 { get; set; }
            [JsonPropertyName("pcie_12V_2x6")] public int? Pin_12_2x6 { get; set; }
        }
    }
}
