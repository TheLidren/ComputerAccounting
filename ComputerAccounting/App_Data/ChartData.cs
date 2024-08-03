using ComputerAccounting.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComputerAccounting.App_Data
{
    public class ChartData
    {
        public string EmployeeId { get; set; }
        public int CountDevice { get; set; }


        public List<ChartData> GetChartData(string connectionString)
        {
            List<ChartData> chartDataList = new List<ChartData>();
            MySqlConnection con = new(connectionString);
            string selectSQL = "SELECT EmployeeId, COUNT(DeviceId) as CountDevice FROM compaccountings GROUP BY EmployeeId";
            con.Open();
            MySqlCommand cmd = new(selectSQL, con);
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr != null)
            {
                while (dr.Read())
                {
                    CompContent db = new();
                    int pr = Convert.ToInt32(dr["EmployeeId"]);
                    Employee employee = db.Employees.Where(x => x.Id == pr).FirstOrDefault();
                    ChartData chartData = new ChartData();
                    chartData.EmployeeId = employee.Surname;
                    chartData.CountDevice = Convert.ToInt32(dr["CountDevice"]);
                    chartDataList.Add(chartData);
                }
            }
            return chartDataList;
        }
    }
}
