using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CatalogPC.Models
{
    internal class Motherboard
    {
        public class Board : ModelOne.Component
        {
            [JsonPropertyName("socket")] public string Socket {  get; set; }
            [JsonPropertyName("memory")] public Memory Memory { get; set; }
            [JsonPropertyName("metadata")] public Meta Metadata { get; set; }

            [JsonPropertyName("pcie_slots")] public List<PCIe> Pci_express { get; set; } = new List<PCIe>();
        }

        public class Memory
        {
            [JsonPropertyName("ram_type")] public string RamType {  get; set; }
            [JsonPropertyName("slots")] public int Slots { get; set; }
            [JsonPropertyName("max")] public int Max { get; set; }
        }

        public class Meta
        {
            [JsonPropertyName("name")] public string Name { get; set; }
            [JsonPropertyName("manufacturer")] public string Manufacturer { get; set; }
        }

        public class PCIe
        {
            [JsonPropertyName("gen")] public string Gen { get; set; }
            [JsonPropertyName("quantity")] public int Quantity { get; set; }
            [JsonPropertyName("lanes")] public int Lanes { get; set; }
        }
    }
}
