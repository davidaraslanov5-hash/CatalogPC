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

        public bool? OnlyCPU = false;

        public bool? OnlyGPU = false;

        public bool? OnlyBoard = false;


        public void NameSearch(string? Name)
        {
            Debug.WriteLine($"Ищем: '{Name}'");

            modelOne.SuitableComponents.Clear();

            Debug.WriteLine(OnlyCPU);
            Debug.WriteLine(OnlyGPU);
            Debug.WriteLine(OnlyBoard);

            if (OnlyCPU == true) { SearchCPUs(Name); }
            else if (OnlyGPU == true) { SearchGPUs(Name); }
            else if (OnlyBoard == true) { SearchBoards(Name); }
            else
            {
                Debug.WriteLine("Ничего не выбрано");
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
                    if (result >= 80 || result_2 >= 90) modelOne.SuitableComponents.Add(component);

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
                    if (result >= 70 || result_2 >= 80) modelOne.SuitableComponents.Add(component);
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
                    if (result >= 80 || result_2 >= 90) modelOne.SuitableComponents.Add(component);
                }
            }
            Debug.WriteLine($"количество {modelOne.ComponentsBoard.Count}");
            Debug.WriteLine($"количество в списке ответов {modelOne.SuitableComponents.Count}");
        }
    }
}
