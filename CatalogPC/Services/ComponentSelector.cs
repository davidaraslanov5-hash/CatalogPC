using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogPC.Services
{

    public class ComponentSelector : DataTemplateSelector
    {
        private SearchService searchService = new SearchService();
        public DataTemplate CPUDataTemplate { get; set; }
        public DataTemplate GPUDataTemplate { get; set; }
        public DataTemplate BoardDataTemplate { get; set; }
        public DataTemplate RAMDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return item switch
            {
                Components.Processor.CPU => CPUDataTemplate,
                Components.Videocard.GPU => GPUDataTemplate,
                Components.Motherboard.Board => BoardDataTemplate,
                Components.RAM.Memory => RAMDataTemplate,
                _ => throw new ArgumentException("Unknown component type", nameof(item)),
            };
        }
    }
}
