using ComputerAccounting.Models;
using System.Data.Entity;

namespace ComputerAccounting.App_Data
{
    [DbConfigurationType(typeof(MySql.Data.EntityFramework.MySqlEFConfiguration))]
    public class CompContent: DbContext
    {
        public CompContent() : base("CompContext")
        {
        }

        public DbSet<Device> Devices { get; set; }
        public DbSet<TypeDevice> TypeDevices { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<CatalogParts> CatalogParts { get; set; }
        public DbSet<RepairDevices> RepairDevices { get; set; }
        public DbSet<CompAccounting> CompAccountings { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
