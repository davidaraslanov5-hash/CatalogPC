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

        public string СompatibilityTest(PC pc)
        {
            if (pc.board != null && pc.board != null)
            {
                {
                    if (pc.board.Socket == pc.CPU.Socket)
                    {
                        return "Процессор подходит к плате";
                    }
                    else
                    {
                        return "Процессор не подходит к плате";
                    }
                }

            }
            else
            {
                return "Комплектующие не выбраны";
            }

        }
        
    }
}
