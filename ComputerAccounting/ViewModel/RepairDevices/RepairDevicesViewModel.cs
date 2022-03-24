using ComputerAccounting.Models;
using System.Collections.Generic;

namespace ComputerAccounting.ListViewModel
{
    public class RepairDevicesViewModel
    {
        public IEnumerable<RepairDevices> RepairDevices { get; set; }

        public RepairDevicesFilter RepairDevicesFilter { get; set; }

        public RepairDevicesSort RepairDevicesSort { get; set; }
    }
}
