using CatalogPC.Components;
using CatalogPC.Models;
using CatalogPC.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using CatalogPC.Services;
using static CatalogPC.Services.ComputerService;

namespace CatalogPC.ViewModels
{
    internal partial class ViewModelOne : ObservableObject
    {

        private ComputerService computer;

        private SearchService searchService = new SearchService();
        private Processor processor = new Processor();
        private Models.ModelOne modelOne = new ModelOne();

        public ViewModelOne()
        {
            if (modelOne == null) modelOne = new Models.ModelOne();
            if (modelOne == null) return;

            if (computer == null) computer = new Services.ComputerService();
            if (computer == null) return;

            if (searchService == null) searchService = new Services.SearchService();
            if (searchService == null) return;
        }
        [ObservableProperty]
        ObservableCollection<ModelOne.Component>? resultsSearch = new ObservableCollection<ModelOne.Component>();

        [ObservableProperty]
        public string? nameFile;

        [ObservableProperty]
        public string selectedComponents;
        

        [ObservableProperty]
        public string cpu_;

        [ObservableProperty]
        public string gpu_;

        [ObservableProperty]
        public string board_;

        [ObservableProperty]
        public string ram_;


        [ObservableProperty]
        public string message;

        Processor.CPU CPU;
        Motherboard.Board BOARD;
        Videocard.GPU GPU;
        RAM.Memory RAM;

        [RelayCommand]
        public void Test()
        {
            ResultsSearch.Clear();
            
            searchService.NameSearch(NameFile, Message);
            foreach (var i in searchService.SuitableComponents)
            {
                ResultsSearch.Add(i);
            }
            
            NameFile = string.Empty;
        }

        [RelayCommand]
        public void AddPart(ModelOne.Component component)
        {
            if (component is Processor.CPU cpu)
            {
                CPU = cpu;
                Cpu_ = cpu.Metadata.Name;
            }
            if (component is Videocard.GPU gpu)
            {
                GPU = gpu;
                Gpu_ = gpu.Metadata.Name;
            }
            if (component is Motherboard.Board board)
            {
                BOARD = board;
                Board_ = board.Metadata.Name;
            }
            if (component is RAM.Memory ram)
            {
                RAM = ram;
                Ram_ = ram.Metadata.Name;
            }
            //modelOne.PCassembly();       
        }

        [RelayCommand]
        public void CompatibilityTest()
        {
            ComputerService.PC PC = new ComputerService.PC();
            PC.board = BOARD;
            PC.CPU = CPU;
            PC.GPU = GPU;

            Message = computer.СompatibilityTest(PC);
            Debug.WriteLine($"Сообщение {Message}");
        }

        partial void OnSelectedComponentsChanged(string value)
        {
            LoadComponent(value);
        }

        [ObservableProperty]
        public bool isCPU;

        [ObservableProperty]
        public bool isGPU;

        [ObservableProperty]
        public bool isBoard;

        [ObservableProperty]
        public bool isRAM;


        public void LoadComponent(string Component)
        {
            if (!Enum.TryParse<ComponentType>(
                Component,
                ignoreCase: true,
                out var selectedType))
            {
                selectedType = ComponentType.None;
            }

            searchService.SelectedType = selectedType;

            (IsCPU, IsGPU, IsBoard, IsRAM) = selectedType switch
            {
                ComponentType.CPU => (true, false, false, false),
                ComponentType.GPU => (false, true, false, false),
                ComponentType.Board => (false, false, true, false),
                ComponentType.RAM => (false, false, false, true),
                _ => (false, false, false, false)
            };

            /*switch (Component)
            {
                case "CPU":
                    searchService.OnlyCPU = true; IsCPU = true; IsGPU = false; IsBoard = false; searchService.OnlyGPU = false; searchService.OnlyBoard = false;
                    break;
                case "GPU":
                    searchService.OnlyGPU = true; IsGPU = true; IsCPU = false; IsBoard = false; searchService.OnlyCPU = false; searchService.OnlyBoard = false; 
                    break;
                case "Board":
                    searchService.OnlyBoard = true; IsBoard = true; IsGPU = false; IsCPU = false; searchService.OnlyGPU = false; searchService.OnlyCPU = false;
                    break;
                case "RAM":
                    searchService.OnlyRAM = true; IsBoard = false; IsGPU = false; IsCPU = false; searchService.OnlyGPU = false; searchService.OnlyCPU = false;
                    break;
            }    
            */
            //нужно заменить всю эту белеберду на ENUM, пока что временно
        }

    }
}
