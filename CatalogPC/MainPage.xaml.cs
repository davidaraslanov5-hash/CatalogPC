using CatalogPC.ViewModels;
using Microsoft.Maui.Controls.Shapes;

namespace CatalogPC
{
    public partial class MainPage : ContentPage
    {
        public MainPage() 
        {
            BindingContext = new ViewModels.ViewModelOne();
            InitializeComponent();
            
        }

        private void OnResultsScrolled(object? sender, ItemsViewScrolledEventArgs e)
        {
            // реагируем только на прокрутку вниз
            if (e.VerticalDelta <= 0) return;

            if (BindingContext is not ViewModelOne vm) return;

            // при Span=3 запас в 6 элементов = две последние строки
            if (e.LastVisibleItemIndex >= vm.ResultsSearch.Count - 6)
                vm.LoadMoreCommand.Execute(null);
        }
    }
}
