using ComputerAccounting.Models;
using System.Collections.Generic;

namespace ComputerAccounting.ListViewModel
{
    public class DeviceViewModel
    {
        public IEnumerable<Device> Devices { get; set; }

        public DeviceFilter DeviceFilter { get; set; }

        public DeviceSort DeviceSort { get; set; }

        public User User { get; set; }
    }
}
