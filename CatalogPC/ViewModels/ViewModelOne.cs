using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CatalogPC.ViewModels
{
    internal partial class ViewModelOne : ObservableObject
    {
        private Models.Processor processor;



        [ObservableProperty]
        public string? nameFileCPU;

        [RelayCommand]
        public void Test()
        {
            if (processor == null) processor = new Models.Processor();
            if (processor == null) return;
            //processor.test(nameFileCPU);
            processor.test(NameFileCPU);
            NameFileCPU = "";
        }
    }
}
