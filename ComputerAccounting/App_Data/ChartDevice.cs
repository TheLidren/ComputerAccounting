using ComputerAccounting.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerAccounting.App_Data
{
    public class ChartDevice
    {
        public string Tittle { get; set; }
        public float Price { get; set; }


        public List<ChartDevice> GetChartDevice(string connectionString)
        {
            List<ChartDevice> chartDeviceList = new List<ChartDevice>();
            MySqlConnection con = new(connectionString);
            string selectSQL = "SELECT Tittle, Price FROM devices";
            con.Open();
            MySqlCommand cmd = new(selectSQL, con);
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr != null)
            {
                while (dr.Read())
                {
                    ChartDevice chartEmployee = new();
                    chartEmployee.Tittle = dr["Tittle"].ToString();
                    chartEmployee.Price = float.Parse(dr["Price"].ToString());
                    chartDeviceList.Add(chartEmployee);
                }
            }
            return chartDeviceList;
        }
    }
}
