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
    }
}
