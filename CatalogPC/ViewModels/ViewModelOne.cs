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

        public ViewModelOne()
        {
            if (modelOne == null) modelOne = new Models.ModelOne();
            if (modelOne == null) return;

            if (computer == null) computer = new Models.Computer();
            if (computer == null) return;
        }
        [ObservableProperty]
        ObservableCollection<ModelOne.Component>? resultsSearch = new ObservableCollection<ModelOne.Component>();

        [ObservableProperty]
        public string? nameFile;



        [ObservableProperty]
        public string selectedComponents;


        [RelayCommand]
        public void Test()
        {
            resultsSearch.Clear();
            
            //processor.test(nameFileCPU);
            //modelOne.ExceptionHandling(NameFile);
            modelOne.NameSearch(nameFile);
            foreach (var i in modelOne.SuitableComponents)
            {
                resultsSearch.Add(i);
            }
            NameFile = "";
        }
        [ObservableProperty]
        public string cpu_;

        [ObservableProperty]
        public string gpu_;

        [ObservableProperty]
        public string board_;

        Processor.CPU CPU;
        Motherboard.Board BOARD;
        Videocard.GPU GPU;

        [RelayCommand]
        public void AddPart(ModelOne.Component component)
        {
            if (component is Processor.CPU cpu)
            {
                CPU = cpu;
                cpu_ = cpu.Metadata.Name;
            }
            if (component is Videocard.GPU gpu)
            {
                GPU = gpu;
                gpu_ = gpu.Metadata.Name;
            }
            if (component is Motherboard.Board board)
            {
                BOARD = board;
                board_ = board.Metadata.Name;
            }
            //modelOne.PCassembly();       
        }

        [RelayCommand]
        public void CompatibilityTest()
        {


            Computer.PC PC = new Computer.PC();
            PC.board = BOARD;
            PC.CPU = CPU;
            PC.GPU = GPU;

            computer.СompatibilityTest(PC);
        }

        partial void OnSelectedComponentsChanged(string value)
        {
            LoadComponent(value);
        }

        public void LoadComponent(string Component)
        {
            if (Component == "CPU") { modelOne.OnlyCPU = true; modelOne.OnlyGPU = false; modelOne.OnlyBoard = false; }
            else if (Component == "GPU") { modelOne.OnlyGPU = true; modelOne.OnlyCPU = false; modelOne.OnlyBoard = false; }
            else if (Component == "Board") { modelOne.OnlyBoard = true; modelOne.OnlyGPU = false; modelOne.OnlyCPU = false; }
            else
            {
                Debug.WriteLine("Вы ничего не выбрали");
                return;
            }
                               
        }

    }
}
