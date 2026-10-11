using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CatalogPC.ViewModels
{
    internal partial class ViewModelOne : ObservableObject
    {
        [ObservableProperty]
        private string labelText = "Hello, World!";
    }
}
