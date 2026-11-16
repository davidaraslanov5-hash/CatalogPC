
using CatalogPC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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

            public IndexPower IndexPower { get; set; }

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

        public class IndexPower
        {
            [JsonIgnore]
            public double TFLOPS => (CoreCount * CoreBoost * 2) / Math.Pow(10, 6);
            public double Bandwidth;
            [JsonIgnore]
            public double RawScore => (Math.Pow(TFLOPS * ArchitectureMultiplier(Chipset), 0.65) *
                                        Math.Pow(Bandwidth, 0.35) * VramMultiplier(Memory));

            public int MTM; //MemoryTypeMultiplier

            [JsonIgnore]
            public double EffectiveMemoryClock
            {
                get
                {
                    double result;
                    if (EffectiveMemoryClock_ < 100)
                        result = EffectiveMemoryClock_ * 1000;
                    if (EffectiveMemoryClock_ > 100 && EffectiveMemoryClock_ < 3500)
                    {
                        if (MemoryType == "GDDR6X" || MemoryType == "GDDR7")
                            MTM = 16;
                        else if (MemoryType == "GDDR6" || MemoryType == "GDDR5X")
                            MTM = 8;
                        else if (MemoryType == "GDDR5")
                            MTM = 4;
                        else
                            MTM = 2;

                        Bandwidth = (EffectiveMemoryClock_ * MTM * MemoryBus) / 8 * 1000;
                        result = Bandwidth;
                    }
                    if (EffectiveMemoryClock_ > 3500)
                        result = EffectiveMemoryClock_;
                    else
                        result = EffectiveMemoryClock_;

                    return ValidationMemoryClock(MemoryType, result);
                }

            }

            [JsonPropertyName("memory")] public int Memory { get; set; }
            [JsonPropertyName("chipset")] public string Chipset { get; set; }
            [JsonPropertyName("memory_bus")] public int MemoryBus { get; set; }
            [JsonPropertyName("memory_type")] public string MemoryType { get; set; }
            [JsonPropertyName("core_count")] public int CoreCount { get; set; }
            [JsonPropertyName("core_boost_clock")] public int CoreBoost { get; set; }
            [JsonPropertyName("effective_memory_clock")] public int EffectiveMemoryClock_ { get; set; }
        }

        public static double ValidationMemoryClock(string memoryType, double effectiveMemoryClock)
        {
            switch (memoryType)
            {
                case "GDDR7":
                    return (effectiveMemoryClock < 26000 || effectiveMemoryClock > 34000) ? 28000 : effectiveMemoryClock;

                case "GDDR6X":
                    return (effectiveMemoryClock < 18000 || effectiveMemoryClock > 25000) ? 21000 : effectiveMemoryClock;

                case "GDDR6":
                    return (effectiveMemoryClock < 12000 || effectiveMemoryClock > 22000) ? 16000 : effectiveMemoryClock;

                case "GDDR5X":
                    return (effectiveMemoryClock < 9000 || effectiveMemoryClock > 13000) ? 11000 : effectiveMemoryClock;

                case "GDDR5":
                    return (effectiveMemoryClock < 3500 || effectiveMemoryClock > 9000) ? 7000 : effectiveMemoryClock;

                case "GDDR3":
                    return (effectiveMemoryClock < 800 || effectiveMemoryClock > 2500) ? 1600 : effectiveMemoryClock;

                case "HBM2":
                    return (effectiveMemoryClock < 1400 || effectiveMemoryClock > 3200) ? 2000 : effectiveMemoryClock;

                case "HBM":
                    return (effectiveMemoryClock < 800 || effectiveMemoryClock > 1200) ? 1000 : effectiveMemoryClock;

                case "DDR5":
                    return (effectiveMemoryClock < 4000 || effectiveMemoryClock > 7200) ? 5200 : effectiveMemoryClock;

                case "DDR4":
                    return (effectiveMemoryClock < 1600 || effectiveMemoryClock > 3600) ? 2133 : effectiveMemoryClock;

                case "DDR3":
                    return (effectiveMemoryClock < 800 || effectiveMemoryClock > 2133) ? 1600 : effectiveMemoryClock;

                case "DDR2":
                    return (effectiveMemoryClock < 400 || effectiveMemoryClock > 1066) ? 800 : effectiveMemoryClock;
                default:
                    return effectiveMemoryClock > 0 ? effectiveMemoryClock : 1600;
            }
        }

        private static double ArchitectureMultiplier(string chipset)
        {
            if (string.IsNullOrWhiteSpace(chipset))
                return 0.60;
            string name = chipset.ToLowerInvariant();

            if (name.Contains("RTX 50")) return 1.15;
            if (name.Contains("RTX 40")) return 1.00;
            if (name.Contains("RTX 30")) return 0.75;
            if (name.Contains("RTX 20") || name.Contains("GTX 16")) return 0.85;
            if (name.Contains("GTX 10")) return 0.65;
            if (name.Contains("GTX 9") || name.Contains("GTX 7") || name.Contains("GT")) return 0.45;

            if (name.Contains("RX 90") || name.Contains("RX 80")) return 0.95;
            if (name.Contains("RX 7")) return 0.82;
            if (name.Contains("RX 6")) return 0.85;
            if (name.Contains("RX 5700") || name.Contains("RX 5600") || name.Contains("RX 5500")) return 0.72;
            if (name.Contains("RX 5") || name.Contains("RX 4") || name.Contains("VEGA")) return 0.50;

            if (name.Contains("ARC B")) return 0.78;
            if (name.Contains("ARC A")) return 0.62;

            return 0.65;
        }

        private static double VramMultiplier(int vRamGB)
        {
            if (vRamGB <= 4) return 0.75;
            if (vRamGB <= 6) return 0.90;
            if (vRamGB <= 8) return 0.96;
            if (vRamGB <= 12) return 1.00;

            return 1.03;
        }
    }
}
