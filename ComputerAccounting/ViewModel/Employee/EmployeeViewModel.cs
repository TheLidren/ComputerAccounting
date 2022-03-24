using ComputerAccounting.Models;
using System.Collections.Generic;

namespace ComputerAccounting.ListViewModel
{
    public class EmployeeViewModel
    {
        public IEnumerable<Employee> Employees { get; set; }

        public EmployeeFilter FilterViewModel { get; set; }
        
        public EmployeeSort SortViewModel { get; set; }
    }
}
