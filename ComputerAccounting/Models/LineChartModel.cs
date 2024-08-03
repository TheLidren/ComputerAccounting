using ComputerAccounting.App_Data;
using System.Collections.Generic;

namespace ComputerAccounting.Models
{
    public class LineChartModel
    {
        public List<ChartData> chartDataList = new();
        public List<ChartEmployee> chartEmployeeList = new();
        
        public void OnGet()
        {
            chartDataList = ChartData();
        }

        public void OnEmployee()
        {
            chartEmployeeList = ChartEmployee();
        }

        public List<ChartEmployee> ChartEmployee()
        {
            List<ChartEmployee> chartEmployeeList = new();
            ChartEmployee chartEmployee = new();
            chartEmployeeList = chartEmployee.GetChartEmployee("server=localhost;port=3306;username=root;password=root;database=BDCompAccounting");
            return chartEmployeeList;
        }

        public List<ChartData> ChartData()
        {
            List<ChartData> chartDataList = new();
            ChartData chartData = new();
            chartDataList = chartData.GetChartData("server=localhost;port=3306;username=root;password=root;database=BDCompAccounting");
            return chartDataList;
        }
    }
}
