using ComputerAccounting.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ComputerAccounting.ListViewModel
{
    public class CompAccountingFilter
    {
        public CompAccountingFilter(List<Employee> employees, int? employee, List<Device> devices, int? device, DateTime? datereceive)
        {
            employees.Insert(0, new Employee { Surname = "Все", Id = 0 });
            Employees = new SelectList(employees, "Id", "Surname", employee);
            SelectedEmployee = employee;
            devices.Insert(0, new Device { Tittle = "Все", Id = 0 });
            Devices = new SelectList(devices, "Id", "Tittle", device);
            SelectedDevice = device;
            DateRecieve = datereceive;
        }

        public IEnumerable<CompAccounting> CompAccountings { get; set; }

        public SelectList Employees { get; set; }
        
        public int? SelectedEmployee { get; set; }
        
        public SelectList Devices { get; set; }

        public int? SelectedDevice { get; set; }

        public DateTime? DateRecieve { get; set; }
    }
}
