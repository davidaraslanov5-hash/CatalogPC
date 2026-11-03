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
        private Models.Processor processor;
        private Models.ModelOne modelOne;


        [ObservableProperty]
        ObservableCollection<Processor.CPU>? resultsSearch = new ObservableCollection<Processor.CPU>();

        [ObservableProperty]
        public string? nameFileCPU;

        [RelayCommand]
        public void Test()
        {
            resultsSearch.Clear();
            if (processor == null) processor = new Models.Processor();
            if (processor == null) return;
            //processor.test(nameFileCPU);
            processor.test(NameFileCPU);
            foreach (var i in processor.SuitableOptions)
            {
                resultsSearch.Add(i);
            }
            NameFileCPU = "";

        }
    }
}
