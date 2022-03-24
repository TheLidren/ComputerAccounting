using ComputerAccounting.Models;
using System.Collections.Generic;

namespace ComputerAccounting.ListViewModel
{
    public class ProviderViewModel
    {
        public IEnumerable<Provider> Providers { get; set; }

        public ProviderFilter FilterViewModel { get; set; }

        public ProviderSort SortViewModel { get; set; }
    }
}
