using ComputerAccounting.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComputerAccounting.App_Data
{
    public class ChartEmployee
    {
        public string PositionId { get; set; }
        public int CountSurname { get; set; }


        public List<ChartEmployee> GetChartEmployee(string connectionString)
        {
            List<ChartEmployee> chartEmployeeList = new List<ChartEmployee>();
            MySqlConnection con = new(connectionString);
            string selectSQL = "SELECT PositionId, COUNT(Surname) as CountSurname FROM employees GROUP BY PositionId";
            con.Open();
            MySqlCommand cmd = new(selectSQL, con);
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr != null)
            {
                while (dr.Read())
                {
                    CompContent db = new();
                    int pr = Convert.ToInt32(dr["PositionId"]);
                    Position position = db.Positions.Where(x => x.Id == pr).FirstOrDefault();
                    ChartEmployee chartEmployee = new();
                    chartEmployee.PositionId = position.Tittle;
                    chartEmployee.CountSurname = Convert.ToInt32(dr["CountSurname"]);
                    chartEmployeeList.Add(chartEmployee);
                }
            }
            return chartEmployeeList;
        }
    }
}
