namespace ComputerAccounting.ListViewModel
{
    public enum CompAccountingState
    {
        EmployeeAsc,
        EmployeeDesc,
        DeviceAsc,
        DeviceDesc,
        DateReceiveAsc,
        DateReceiveDesc,
        DateDeleteAsc,
        DateDeleteDesc,
    }

    public class CompAccountingSort
    {
        public CompAccountingSort(CompAccountingState compAccountingState)
        {
            EmployeeSort = compAccountingState == CompAccountingState.EmployeeAsc ? CompAccountingState.EmployeeDesc : CompAccountingState.EmployeeAsc;
            DeviceSort = compAccountingState == CompAccountingState.DeviceAsc ? CompAccountingState.DeviceDesc : CompAccountingState.DeviceAsc;
            DateReceiveSort = compAccountingState == CompAccountingState.DateReceiveAsc ? CompAccountingState.DateReceiveDesc : CompAccountingState.DateReceiveAsc;
            DateDeleteSort = compAccountingState == CompAccountingState.DateDeleteAsc ? CompAccountingState.DateDeleteDesc : CompAccountingState.DateDeleteAsc;
        }

        public CompAccountingState EmployeeSort { get; set; }
        public CompAccountingState DeviceSort { get; set; }
        public CompAccountingState DateReceiveSort { get; set; }
        public CompAccountingState DateDeleteSort { get; set; }
    }
}
