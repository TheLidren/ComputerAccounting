namespace ComputerAccounting.ListViewModel
{
    public enum DeviceState
    {
        TittleAsc,
        TittleDesc,
        TypeDeviceAsc,
        TypeDeviceDesc,
        ProviderAsc,
        ProviderDesc,
        DateBuyAsc,
        DateBuyDesc,
        PriceAsc,
        PriceDesc,
    }

    public class DeviceSort
    {
        public DeviceSort(DeviceState deviceState)
        {
            TittleSort = deviceState == DeviceState.TittleAsc ? DeviceState.TittleDesc : DeviceState.TittleAsc;
            TypeDeviceSort = deviceState == DeviceState.TypeDeviceAsc ? DeviceState.TypeDeviceDesc : DeviceState.TypeDeviceAsc;
            ProviderSort = deviceState == DeviceState.ProviderAsc ? DeviceState.ProviderDesc : DeviceState.ProviderAsc;
            DateBuySort = deviceState == DeviceState.DateBuyAsc ? DeviceState.DateBuyDesc : DeviceState.DateBuyAsc;
            PriceSort = deviceState == DeviceState.PriceAsc ? DeviceState.PriceDesc : DeviceState.PriceAsc;
        }

        public DeviceState TittleSort { get; set; }
        public DeviceState TypeDeviceSort { get; set; }
        public DeviceState ProviderSort { get; set; }
        public DeviceState DateBuySort { get; set; }
        public DeviceState PriceSort { get; set; }
    }
}
