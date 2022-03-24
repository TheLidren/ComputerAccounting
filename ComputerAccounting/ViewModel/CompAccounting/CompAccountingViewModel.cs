using ComputerAccounting.Models;
using System.Collections.Generic;

namespace ComputerAccounting.ListViewModel
{
    public class CompAccountingViewModel
    {
        public IEnumerable<CompAccounting> CompAccountings { get; set; }

        public CompAccountingFilter CompAccountingFilter { get; set; }

        public CompAccountingSort CompAccountingSort { get; set; }
    }
}
