using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace CatalogPC.Models
{
    internal class Computer
    {
        public class PC
        {
            public Motherboard.Board board = new Motherboard.Board();
            public Processor.CPU CPU = new Processor.CPU();
            public Videocard.GPU GPU = new Videocard.GPU();
        }

        public void СompatibilityTest(PC pc)
        {
            if (pc.board.Socket == pc.CPU.Socket)
            {
                Debug.WriteLine($"Процессор {pc.CPU.Name} подходит к плате {pc.board.Name}");
            }
            else {
                Debug.WriteLine($"Процессор не подходит к плате ");
            }
        }
    }
}
