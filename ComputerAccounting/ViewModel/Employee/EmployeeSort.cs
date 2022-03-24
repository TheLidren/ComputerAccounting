namespace ComputerAccounting.ListViewModel
{
    public enum SortState
    {
        SurnameAsc,    
        SurnameDesc,  
        DateWorkAsc, 
        DateWorkDesc,   
        PositionAsc,
        PositionDesc
    }

    public class EmployeeSort
    {
        public SortState SurnameSort { get; set; }
        public SortState DateWorkSort { get; set; }
        public SortState PositionSort { get; set; }
        public SortState Current { get; set; }

        public EmployeeSort(SortState sortOrder)
        {
            SurnameSort = sortOrder == SortState.SurnameAsc ? SortState.SurnameDesc : SortState.SurnameAsc;
            DateWorkSort = sortOrder == SortState.DateWorkAsc ? SortState.DateWorkDesc : SortState.DateWorkAsc;
            PositionSort = sortOrder == SortState.PositionAsc ? SortState.PositionDesc : SortState.PositionAsc;
            Current = sortOrder;
        }
    }
}
