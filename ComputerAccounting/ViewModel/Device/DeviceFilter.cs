using ComputerAccounting.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ComputerAccounting.ListViewModel
{
    public class DeviceFilter
    {
        public DeviceFilter(List<TypeDevice> typeDevices, int? typedevice, List<Provider> providers, int? provider, string param)
        {
            typeDevices.Insert(0, new TypeDevice { Tittle = "Все", Id = 0 });
            TypeDevices = new SelectList(typeDevices, "Id", "Tittle", typedevice);
            SelectedTypeDevice = typedevice;
            providers.Insert(0, new Provider { Surname = "Все", Id = 0 });
            Providers = new SelectList(providers, "Id", "Surname", provider);
            SelectedProvider = provider;
            Params = param;
        }

        public SelectList TypeDevices { get; set; }

        public int? SelectedTypeDevice { get; set; }

        public SelectList Providers { get; set; }

        public int? SelectedProvider { get; set; }

        public string Params { get; set; }
    }
}
