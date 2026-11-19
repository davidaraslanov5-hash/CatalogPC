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
            bool GraphicsTrue = true; //по умолчанию значение верны
            bool SocketTrue = true;
            bool MemoryTrue = true; 
            int TotalRAM_GB = 0;


            //по пути проверки будем менять на False если не устраивает совместимость
            if (pc == null || pc.board == null || pc.CPU == null) //проверка на ввод основных данных
            {             
                return "Нехватка данных";
            }

            var messages = new List<string>(); 
            var boardSocket = pc.board.Socket?.Replace("2 x ", "")?.Trim(); //если сокет содержит "2 x" то вырезаем фрагмент (к примеру 2 x LGA 2011)
            var boardMemoryType = pc.board.Memory?.RamType?.Trim();

            if (pc.CPU.Specs.Graphics.GraphicsCore == "None") //записываем если у процессора есть графическое ядро
                GraphicsTrue = false;
            else
                GraphicsTrue = true;

            if (!string.Equals(boardSocket, pc.CPU.Socket, StringComparison.OrdinalIgnoreCase)) //ищем в сокете платы название сокета процессора, не находим
            {
                messages.Add($"Процессор должен иметь сокет {boardSocket}");
                SocketTrue = false; //отмечаем что совместимости нет
            }
            if (pc.board.Memory.RamType != null && pc.CPU.Specs.Memory.Types?.Contains(pc.board.Memory.RamType) != true) //проверка на совместимость с оперативкой
            {
                messages.Add($"Процессор не поддерживает {pc.board.Memory.RamType}");
                MemoryTrue = false; //отмечаем что совместимости нет
            }
            if (pc.board.Memory != null && pc.RAM != null)
            {
                for (int i = 0; i < pc.Memories.Count; i++)
                {
                    TotalRAM_GB += pc.Memories[i].Module.CapacityGB_x1; //отмечаем сколько всего ГБ озу в сборке
                    if (pc.board.Memory.RamType != pc.Memories[i].MemoryType) //проверка по типу памяти
                    {
                        Debug.WriteLine($"Память {pc.Memories[i].Metadata.Name} не подходит к плате по типу");
                        messages.Add($"Память не подходит к плате по типу");
                        MemoryTrue = false;
                    }
                    if (TotalRAM_GB > pc.board.Memory.Max) //Озу не слишком много?
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
                if (string.IsNullOrWhiteSpace(pc.board.FormFactor) != true)
                    messages.Add($"Необходим корпус с размером {pc.board.FormFactor}");
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
                    messages.Add($"Необходим БП мощностью в {(int)(load * 1.4)}W"); //умножнаем на 1.4 так как мы не учитывали другие затраты энергии
                }
                else if (GraphicsTrue && pc.GPU == null)
                {
                    load = pc.CPU.Specs.Tdp + 100;
                    messages.Add($"Необходим БП мощностью в {(int)(load * 1.4)}W");
                }
            }
            return string.Join(Environment.NewLine, messages); //возвращаем значение в строку   
        }

    }
}
