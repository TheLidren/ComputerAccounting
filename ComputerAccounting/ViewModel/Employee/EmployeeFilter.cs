using ComputerAccounting.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ComputerAccounting.ListViewModel
{
    public class EmployeeFilter
    { 
        public EmployeeFilter(List<Position> positions, int? position, string param)
        {
            positions.Insert(0, new Position { Tittle = "Все", Id = 0 });
            Positions = new SelectList(positions, "Id", "Tittle", position);
            SelectedPosition = position;
            Params = param;
        }

        public SelectList Positions { get; set; }
       
        public int? SelectedPosition { get; private set; }

        public string Params { get; set; }

    }
}
