namespace ComputerAccounting.ListViewModel
{
    public enum RepairDevicesState
    {
        DeviceAsc,
        DeviceDesc,
        CatalogPartsAsc,
        CatalogPartsDesc,
        DateRepairAsc,
        DateRepairDesc,
    }

    public class RepairDevicesSort
    {
        public RepairDevicesSort(RepairDevicesState repairDevicesState)
        {
            DeviceSort = repairDevicesState == RepairDevicesState.DeviceAsc ? RepairDevicesState.DeviceDesc : RepairDevicesState.DeviceAsc;
            CatalogPartsSort = repairDevicesState == RepairDevicesState.CatalogPartsAsc ? RepairDevicesState.CatalogPartsDesc : RepairDevicesState.CatalogPartsAsc;
            DateRepairSort = repairDevicesState == RepairDevicesState.DateRepairAsc ? RepairDevicesState.DateRepairDesc : RepairDevicesState.DateRepairAsc;
        }

        public RepairDevicesState DeviceSort { get; set; }
        public RepairDevicesState CatalogPartsSort { get; set; }
        public RepairDevicesState DateRepairSort { get; set; }
    }
}
