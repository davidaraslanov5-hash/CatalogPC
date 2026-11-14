
using CatalogPC.Components;
using CatalogPC.Models;
using CatalogPC.Services;
using CatalogPC.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
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
        ObservableCollection<ModelOne.Component>? resultsSearch;

        List<ModelOne.Component> _resultsSearch = new List<ModelOne.Component>();

        [ObservableProperty]
        ObservableCollection<ModelOne.Component>? selectedComponentsList = new ObservableCollection<ModelOne.Component>();

        [ObservableProperty]
        public string? nameFile;
        [ObservableProperty]
        public string selectedComponents;



        //отображение выбранных компонентов
        [ObservableProperty]
        public string cpu_;

        [ObservableProperty]
        public string gpu_;

        [ObservableProperty]
        public string board_;

        [ObservableProperty]
        public string ram_;



        [ObservableProperty]
        public string compatibilityMessage;

        //данные для теста совместимости
        Processor.CPU CPU;
        Motherboard.Board BOARD;
        Videocard.GPU GPU;
        RAM.Memory RAM;

        List<RAM.Memory> Memories = new List<RAM.Memory>();

        private List<ModelOne.Component> _foundComponents = new();

        private const int PageSize = 50;
        private bool _isLoadingMore = false;

        [RelayCommand]
        public void Test()
        {

            if (string.IsNullOrWhiteSpace(NameFile))
            {
                CompatibilityMessage = "Введите название компонента";
                return;
            }

            searchService.NameSearch(NameFile, CompatibilityMessage);

            Debug.WriteLine($"Поиск совершен");
            _foundComponents = searchService.SuitableComponents;
            ResultsSearch = new ObservableCollection<ModelOne.Component>(_foundComponents.Take(PageSize));
            NameFile = string.Empty;
        }

        [ObservableProperty]
        public bool advancedInfo = false;

        [ObservableProperty]
        public string showTextInfo = "Показать дополнительную информацию";

        [RelayCommand]
        public void ExtraInfo()
        {
            AdvancedInfo = !AdvancedInfo;
            switch (AdvancedInfo)
            {
                case true:
                    ShowTextInfo = "Скрыть дополнительную информацию";
                    break;
                case false:
                    ShowTextInfo = "Показать дополнительную информацию";
                    break;
            }
            Debug.WriteLine($"AdvancedInfo: {AdvancedInfo}");
        }

        [RelayCommand]
        private void LoadMore()
        {
            if (_isLoadingMore || ResultsSearch.Count >= _foundComponents.Count)
                return;

            _isLoadingMore = true;
            try
            {
                foreach (var item in _foundComponents.Skip(ResultsSearch.Count).Take(PageSize))
                    ResultsSearch.Add(item);
            }
            finally
            {
                _isLoadingMore = false;
            }
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
                Memories.Add(ram);
                Ram_ = $"{ram.Metadata.Name} {Memories.Count}-х";
            }     
        }

        [RelayCommand]
        public void CompatibilityTest()
        {
            ComputerService.PC PC = new ComputerService.PC();
            PC.board = BOARD;
            PC.CPU = CPU;
            PC.GPU = GPU;
            PC.Memories = Memories;


            CompatibilityMessage = computer.СompatibilityTest(PC);
            Debug.WriteLine($"Кол-во плашек ОЗУ: {PC.Memories.Count}");
            Debug.WriteLine($"Сообщение {CompatibilityMessage}");
        }

        [RelayCommand]
        public void DeleteOneRAM()
        {
            if (Memories.Count > 0)
            {
                Memories.RemoveAt(Memories.Count - 1);
                if (Memories.Count > 0)
                {
                    Ram_ = $"{Memories[0].Metadata.Name} {Memories.Count}-х";
                }
                else
                {
                    Ram_ = string.Empty;
                }
            }
        }

        [RelayCommand]
        public void AddOneRAM()
        {
            if (Memories.Count > 0)
            {
                Memories.Add(Memories[Memories.Count - 1]);
                if (Memories.Count > 0)
                {
                    Ram_ = $"{Memories[0].Metadata.Name} {Memories.Count}-х";
                }
                else
                {
                    Ram_ = string.Empty;
                }
            }
        }



        partial void OnSelectedComponentsChanged(string value)
        {
            LoadComponent(value);
        }

        //данные для отображения выбранного компонента


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
        }

        

    }
}
