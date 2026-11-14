using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.ComponentModel.Design;
using CatalogPC.Components;
using System.Security.Cryptography.X509Certificates;

namespace CatalogPC.Services
{
    internal class ComputerService
    {
        public class PC
        {
            public RAM.Memory RAM = new RAM.Memory();
            public List<RAM.Memory> Memories = new List<RAM.Memory>();
            public Motherboard.Board board = new Motherboard.Board();
            public Processor.CPU CPU = new Processor.CPU();
            public Videocard.GPU GPU = new Videocard.GPU();
        }
        public string СompatibilityTest(PC pc)
        {
            bool GraphicsTrue = true;
            bool SocketTrue = true;
            bool MemoryTrue = true;
            int TotalRAM_GB = 0;

            if (pc == null || pc.board == null || pc.CPU == null)
            {             
                return "Нехватка данных";
            }

            var messages = new List<string>();
            var boardSocket = pc.board.Socket?.Replace("2 x ", "")?.Trim();
            var boardMemoryType = pc.board.Memory?.RamType?.Trim();

            if (pc.CPU.Specs.Graphics.GraphicsCore == "None")
                GraphicsTrue = false;
            else
                GraphicsTrue = true;
            if (!string.Equals(boardSocket, pc.CPU.Socket, StringComparison.OrdinalIgnoreCase))
            {
                messages.Add("Процессор не подходит к плате по сокету");
                SocketTrue = false;
            }
            if (pc.board.Memory.RamType != null && pc.CPU.Specs.Memory.Types?.Contains(pc.board.Memory.RamType) != true)
            {
                messages.Add($"Процессор не поддерживает {pc.board.Memory.RamType}");
                MemoryTrue = false;
            }
            if (pc.board.Memory != null && pc.RAM != null)
            {
                for (int i = 0; i < pc.Memories.Count; i++)
                {
                    TotalRAM_GB += pc.Memories[i].Module.CapacityGB_x1;
                    if (pc.board.Memory.RamType != pc.Memories[i].MemoryType)
                    {
                        Debug.WriteLine($"Память {pc.Memories[i].Metadata.Name} не подходит к плате по типу");
                        messages.Add($"Память не подходит к плате по типу");
                        MemoryTrue = false;
                    }
                    if (TotalRAM_GB > pc.board.Memory.Max)
                    {
                        Debug.WriteLine($"Память не подходит к плате по обьему {TotalRAM_GB}GB у ОЗУ vs {pc.board.Memory.Max}GB у платы");
                        messages.Add($"Память не подходит к плате по обьему");
                        MemoryTrue = false;
                    }
                    if (MemoryTrue == false) break;
                }
               
            }
            
            if (pc.Memories != null && pc.Memories.Count > pc.board.Memory.Slots)
            {
                messages.Add($"Количество планок памяти превышает количество слотов на материнской плате");
                MemoryTrue = false;
            }
            if (SocketTrue && MemoryTrue)
            {
                messages.Add("Сборка совместима");
            }


            int? load;

            if (pc.CPU == null || pc.CPU.Specs.Tdp == null )
            {
                messages.Add("Не удалось рассчитать рекомендуемую мощность блока питания");
            }
            else
            {
                if (pc.GPU != null && pc.GPU.Tdp != null)
                {
                    load = pc.CPU.Specs.Tdp + pc.GPU.Tdp + 100;
                    messages.Add($"Необходим БП мощностью в {(int)(load * 1.4)}W");
                }
                else if (GraphicsTrue && pc.GPU == null)
                {
                    load = pc.CPU.Specs.Tdp + 100;
                    messages.Add($"Необходим БП мощностью в {(int)(load * 1.4)}W");
                }
            }
            return string.Join(Environment.NewLine, messages);       
        }

    }
}
