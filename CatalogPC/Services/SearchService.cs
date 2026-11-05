using CatalogPC.Components;
using CatalogPC.Models;
using FuzzySharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CatalogPC.Services
{
    internal class SearchService
    {
        private Models.ModelOne modelOne = new();

        public List<Models.ModelOne.Component> SuitableComponents =>
           modelOne.SuitableComponents;

        public ComponentType SelectedType { get; set; } = ComponentType.CPU;
        public bool? OnlyCPU = false;

        public bool? OnlyGPU = false;

        public bool? OnlyBoard = false;

        public bool? OnlyRAM = false;
        public void NameSearch(string? Name, string Message)
        {
            Debug.WriteLine($"Ищем: '{Name}'");

            modelOne.SuitableComponents.Clear();

            switch (SelectedType)
            {
                case ComponentType.CPU:
                    SearchCPUs(Name);
                    break;
                case ComponentType.GPU:
                    SearchGPUs(Name);
                    break;
                case ComponentType.Board:
                    SearchBoards(Name);
                    break;
                case ComponentType.RAM:
                    SearchRAMs(Name);
                    break;
                case ComponentType.None:
                default:
                    Message = "ничего не выбрано";
                    break;
            }

            Debug.WriteLine(OnlyCPU);
            Debug.WriteLine(OnlyGPU);
            Debug.WriteLine(OnlyBoard);

            if (OnlyCPU == true) { SearchCPUs(Name); }
            else if (OnlyGPU == true) { SearchGPUs(Name); }
            else if (OnlyBoard == true) { SearchBoards(Name); }
            else
            {
                Message = "ничего не выбрано";
                //return;
            }
        }

        public void SearchCPUs(string? Name)
        {
            foreach (ModelOne.Component component in modelOne.ComponentsCPU)
            {
                if (component is Processor.CPU cpu)
                {
                    var name = Name ?? string.Empty;
                    string cpuName = cpu.Metadata?.Name ?? string.Empty;
                    string cpuSocket = cpu.Socket ?? string.Empty;

                    var result = Fuzz.PartialRatio(Name ?? string.Empty, cpuName);
                    var result_2 = Fuzz.PartialRatio(Name ?? string.Empty, cpuSocket);
                    if (result >= 80 || result_2 >= 90 || cpuName.Contains(Name) ) modelOne.SuitableComponents.Add(component);

                }
            }
            Debug.WriteLine($"количество {modelOne.ComponentsCPU.Count}");
            Debug.WriteLine($"количество в списке ответов {modelOne.SuitableComponents.Count}");
        }

        public void SearchGPUs(string? Name)
        {
            foreach (ModelOne.Component component in modelOne.ComponentsGPU)
            {
                if (component is Videocard.GPU gpu)
                {
                    var name = Name ?? string.Empty;
                    string gpuName = gpu.Metadata?.Name ?? string.Empty;
                    string gpuInterface = gpu.Interface ?? string.Empty;

                    var result = Fuzz.PartialRatio(Name ?? string.Empty, gpuName);
                    var result_2 = Fuzz.PartialRatio(Name ?? string.Empty, gpuInterface);
                    if (gpuName.Contains(Name) || result >= 88 || result_2 >= 80 ) modelOne.SuitableComponents.Add(component);
                }
            }
            Debug.WriteLine($"количество {modelOne.ComponentsGPU.Count}");
            Debug.WriteLine($"количество в списке ответов {modelOne.SuitableComponents.Count}");
        }

        public void SearchBoards(string? Name)
        {
            foreach (ModelOne.Component component in modelOne.ComponentsBoard)
            {
                if (component is Motherboard.Board board)
                {
                    var name = Name ?? string.Empty;
                    string boardName = board.Metadata?.Name ?? string.Empty;
                    string BoardSocket = board.Socket ?? string.Empty;

                    var result = Fuzz.PartialRatio(Name ?? string.Empty, boardName);
                    var result_2 = Fuzz.PartialRatio(Name ?? string.Empty, BoardSocket);
                    if (result >= 80 || result_2 >= 90 || boardName.Contains(Name)) modelOne.SuitableComponents.Add(component);
                }
            }
            Debug.WriteLine($"количество {modelOne.ComponentsBoard.Count}");
            Debug.WriteLine($"количество в списке ответов {modelOne.SuitableComponents.Count}");
        }

        public void SearchRAMs(string? Name)
        {
            foreach (ModelOne.Component component in modelOne.ComponentsRAM)
            {
                if (component is RAM.Memory ram)
                {
                    var name = Name ?? string.Empty;
                    string ramName = ram.Metadata?.Name ?? string.Empty;
                    string ramSocket = ram.MemoryType ?? string.Empty;

                    var result = Fuzz.PartialRatio(Name ?? string.Empty, ramName);
                    var result_2 = Fuzz.PartialRatio(Name ?? string.Empty, ramSocket);

                    if (result >= 80 || result_2 >= 90 || ramName.Contains(Name)) modelOne.SuitableComponents.Add(component);
                }
            }
            Debug.WriteLine($"количество {modelOne.ComponentsBoard.Count}");
            Debug.WriteLine($"количество в списке ответов {modelOne.SuitableComponents.Count}");
        }
    }
}
