
using CatalogPC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using static CatalogPC.Components.RAM;

namespace CatalogPC.Components
{
    internal class Videocard
    {
        public class GPU : ModelOne.Component
        {
            [JsonPropertyName("opendb_id")] public string GPUid { get; set; }

            [JsonPropertyName("chipset")] public string Chipset { get; set; }
            [JsonPropertyName("core_count")] public int CoreCount { get; set; }
            [JsonPropertyName("core_boost_clock")] public int CoreBoost { get; set; }
            [JsonPropertyName("effective_memory_clock")] public int EffectiveMemoryClock { get; set; }
            [JsonPropertyName("memory")] public int Memory { get; set; }
            [JsonPropertyName("memory_bus")] public int MemoryBus_ { get; set; }
            public string MemoryBus => MemoryBus_ != 0 ? $"Шина памяти: {MemoryBus_}" : $"Нет данных о шине памяти";

            [JsonPropertyName("memory_type")] public string MemoryType { get; set; }

            [JsonPropertyName("interface")] public string Interface { get; set; }
            [JsonPropertyName("tdp")] public int? Tdp { get; set; }
            [JsonPropertyName("metadata")] public Meta Metadata { get; set; }
            [JsonPropertyName("general_product_information")] public GPUInfo Info { get; set; }
            [JsonPropertyName("power_connectors")] public Power PowerInfo { get; set; }

            [JsonIgnore]
            public IndexPower IndexPower { get; }

            public GPU()
            {
                IndexPower = new IndexPower(this);
            }
        }

        public class Meta
        {
            [JsonPropertyName("name")] public string Name { get; set; }
            [JsonPropertyName("releaseYear")] public int? ReleaseYear_ { get; set; }
            public string ReleaseYear => ReleaseYear_ != null ? $"Год выпуска: {ReleaseYear_}" : "Неизвестный год выпуска";
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
            private readonly GPU _gpu;

            public static double MaxReferenceScore = 340;
            public IndexPower(GPU gpu)
            {
                _gpu = gpu;
            }

            public double TFLOPS => (_gpu.CoreCount * _gpu.CoreBoost * 2.0) / 1_000_000.0; // делим на миллион потому что нужно получить очень маленькое число (в размере 15+-)
            public string Score
            {
                get
                {
                    if (RawScore <= 0)
                        return "Индекс мощности: Нет данных";
                    double ratio = RawScore / MaxReferenceScore;
                    // Степень 0.55 сглаживает отрыв ультра-флагманов, чтобы карты среднего класса получали адекватные 45-65 баллов вместо 20
                    double finalScore = Math.Pow(ratio, 0.55) * 100;
                    return $"Индекс мощности: {Math.Clamp((int)finalScore, 1, 100)}"; //ужимаем наш ответ в 1 / 100 чтобы было понятнее
                }
            }

            public int MTM => _gpu.MemoryType switch //MemoryTypeMultiplier, нужен чтобы увеличивать балл за современный тип памяти
            {
                "GDDR6X" or "GDDR7" => 16,
                "GDDR6" or "GDDR5X" => 8,
                "GDDR5" => 4,
                _ => 2
            };

            public double EffectiveMemoryClock //в базе данных все записано по разному, встречаются значения в Гбит/с (21), базовые МГц (1313) и эффективные (21000)
            {
                get
                {
                    double clockMt;
                    if (_gpu.EffectiveMemoryClock < 100) //тут парсер например записывал данные в гбит сек
                        clockMt = _gpu.EffectiveMemoryClock * 1000.0; // умножаем на 1000, получаем нашу рабочую частоту
                    else if (_gpu.EffectiveMemoryClock >= 100 && _gpu.EffectiveMemoryClock <= 3500) // тут значение это базовая физическая частота, умножаем на наш множитель MTM
                    {
                        clockMt = _gpu.EffectiveMemoryClock * MTM;
                    }
                    else
                        clockMt = _gpu.EffectiveMemoryClock; //парсер записал все верно

                    return ValidationMemoryClock(_gpu.MemoryType, clockMt); 
                }
            }

            public double Bandwidth => (EffectiveMemoryClock * _gpu.MemoryBus_) / 8000.0; //пропускная способность, могли бы поделить сначала на 8 (перевести биты в байты), а потом на 1000 (MB\s в GB\s)
            public double RawScore => CaclulateRawScore(TFLOPS, ArchitectureMultiplier(_gpu.Chipset), Bandwidth, _gpu.Memory);

            //второй параметр это балл за чипсет, чем новее тем больше балл
            private static double CaclulateRawScore(double tflops, double kArch, double bandwidth, int vramGb)
            {
                if (tflops <= 0 || bandwidth <= 0)
                    return 0;

                double effectiveCompute = tflops * kArch;
                double basePerfomance = Math.Pow(effectiveCompute, 0.65) * Math.Pow(bandwidth, 0.35);
                double kVram = VramMultiplier(vramGb);

                return basePerfomance * kVram;
            }

            //доп проверка которая приравнивает значение к нормальным если данные изза ошибок улетают в небеса или падают в ноль
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
                string name = chipset.ToUpperInvariant();

                if (name.Contains("RTX 50")) return 1.15; //проверка на чипсет, множитель зависит от серии
                if (name.Contains("RTX 40")) return 1.00;
                if (name.Contains("RTX 30")) return 0.75;
                if (name.Contains("RTX 20") || name.Contains("GTX 16")) return 0.85;
                if (name.Contains("GTX 10")) return 0.65;
                if (name.Contains("GTX 9") || name.Contains("GTX 7") || name.Contains("GT ")) return 0.45;

                if (name.Contains("RX 90") || name.Contains("RX 80")) return 0.95;
                if (name.Contains("RX 7")) return 0.82;
                if (name.Contains("RX 6")) return 0.85;
                if (name.Contains("RX 5700") || name.Contains("RX 5600") || name.Contains("RX 5500")) return 0.72;
                if (name.Contains("RX 5") || name.Contains("RX 4") || name.Contains("VEGA")) return 0.50;

                if (name.Contains("ARC B")) return 0.78;
                if (name.Contains("ARC A")) return 0.62;

                return 0.65;
            }

            //штрафы за количество памяти, больше 12 это уже маленький бонус за современность
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
}
