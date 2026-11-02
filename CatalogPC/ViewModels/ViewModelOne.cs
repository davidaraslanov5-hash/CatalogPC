using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using CatalogPC.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CatalogPC.ViewModels
{
    internal partial class ViewModelOne : ObservableObject
    {
        private Models.Computer computer;
        private Models.Processor processor;
        private Models.ModelOne modelOne;


        [ObservableProperty]
        ObservableCollection<ModelOne.Component>? resultsSearch = new ObservableCollection<ModelOne.Component>();

        [ObservableProperty]
        public string? nameFile;

        [RelayCommand]
        public void Test()
        {
            resultsSearch.Clear();
            if (modelOne == null) modelOne = new Models.ModelOne();
            if (modelOne == null) return;
            //processor.test(nameFileCPU);
            //modelOne.ExceptionHandling(NameFile);
            modelOne.NameSearch(nameFile);
            foreach (var i in modelOne.SuitableComponents)
            {
                resultsSearch.Add(i);
            }
            NameFile = "";
        }

        Processor.CPU CPU;
        Motherboard.Board BOARD;
        Videocard.GPU GPU;

        [RelayCommand]
        public void AddPart(ModelOne.Component component)
        {
            if (component is Processor.CPU cpu)
            {
                CPU = cpu;
                Debug.WriteLine(cpu.Metadata.Name);
            }
            if (component is Videocard.GPU gpu)
            {
                GPU = gpu;
                Debug.WriteLine(gpu.Metadata.Name);
            }
            if (component is Motherboard.Board board)
            {
                BOARD = board;
                Debug.WriteLine(board.Metadata.Name);
            }
            //modelOne.PCassembly();       
        }

        [RelayCommand]
        public void CompatibilityTest()
        {
            if (computer == null) computer = new Models.Computer();
            if (computer == null) return;

            Computer.PC PC = new Computer.PC();
            PC.board = BOARD;
            PC.CPU = CPU;
            PC.GPU = GPU;

            computer.СompatibilityTest(PC);
        }
    }
}
