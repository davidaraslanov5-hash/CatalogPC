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
            modelOne.ExceptionHandling(NameFile);
            foreach (var i in modelOne.Components)
            {
                resultsSearch.Add(i);
            }
            NameFile = "";

        }
    }
}
