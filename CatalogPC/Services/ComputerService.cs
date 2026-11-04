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
            public Motherboard.Board board = new Motherboard.Board();
            public Processor.CPU CPU = new Processor.CPU();
            public Videocard.GPU GPU = new Videocard.GPU();
        }

        public void СompatibilityTest(PC pc)
        {
            if (pc != null)
            {
                {
                    if (pc.board.Socket == pc.CPU.Socket)
                    {
                        Debug.WriteLine($"Процессор {pc.CPU.Metadata.Name} подходит к плате {pc.board.Metadata.Name}");
                    }
                    else
                    {
                        Debug.WriteLine($"Процессор не подходит к плате ");
                    }
                }

            }
            else
            {
                Debug.WriteLine("Комплектующие не выбраны");
                return;
            }

        }
        
    }
}
