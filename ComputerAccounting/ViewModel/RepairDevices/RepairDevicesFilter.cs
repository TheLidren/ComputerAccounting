using ComputerAccounting.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ComputerAccounting.ListViewModel
{
    public class RepairDevicesFilter
    {
        public RepairDevicesFilter(List<Device> devices, int? device, List<CatalogParts> catalogParts, int? catalogpart, DateTime? daterepair)
        {
            devices.Insert(0, new Device { Tittle = "Все", Id = 0 });
            Devices = new SelectList(devices, "Id", "Tittle", device);
            SelectedDevice = device;
            catalogParts.Insert(0, new CatalogParts { Tittle = "Все", Id = 0 });
            CatalogParts = new SelectList(catalogParts, "Id", "Tittle", catalogpart);
            SelectedCatalogPart = catalogpart;
            DateRepair = daterepair;
        }

        public SelectList Devices { get; set; }
        
        public int? SelectedDevice { get; set; }
        
        public SelectList CatalogParts { get; set; }

        public int? SelectedCatalogPart { get; set; }

        public DateTime? DateRepair { get; set; }

    }
}
