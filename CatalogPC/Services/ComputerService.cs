using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.ComponentModel.Design;
using CatalogPC.Components;

namespace CatalogPC.Services
{
    internal class ComputerService
    {
        public class PC
        {
            public RAM.Memory RAM = new RAM.Memory();
            public Motherboard.Board board = new Motherboard.Board();
            public Processor.CPU CPU = new Processor.CPU();
            public Videocard.GPU GPU = new Videocard.GPU();
        }
        public string СompatibilityTest(PC pc)
        {
            if (pc == null || pc.board == null || pc.CPU == null || pc.GPU == null) 
            {
                return "Нехватка данных :(";
            }

            bool SocketTrue = true;
            bool MemoryTrue = true;

            var messages = new List<string>();
            var boardSocket = pc.board.Socket?.Replace("2 x ", "")?.Trim();

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
            if (SocketTrue && MemoryTrue)
            {
                messages.Add("Процессор полностью подходит к плате");
            }

            int? load = pc.CPU.Specs.Tdp + pc.GPU.Tdp + 100;
                messages.Add($"Рекомендуемый блок мощностью в {(int)(load * 1.4)}W");

            return string.Join(Environment.NewLine, messages);
            
        }

    }
}
