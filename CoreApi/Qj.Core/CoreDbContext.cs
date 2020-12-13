using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Qj.Models;

namespace Qj.Core
{
    public class CoreDbContext : DbContext
    {
        //public static string ConnectionString = "User Id=system;Password=123456;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=orcl)))";//oracle
        public static string ConnectionString = "server=slocalhost;port=63816;database=dongtou_kefu;uid=root;pwd=123123;CharSet=utf8";//mysql
        //public static string ConnectionString = "server=.;user id=sa;password=123456;database=tempCode;";//sqlserver
        //public static string ConnectionString { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(ConnectionString).ReplaceService<IMigrationsSqlGenerator, MyMigrationsSqlGenerator>();//mysql
            //optionsBuilder.UseLazyLoadingProxies().UseSqlServer(ConnectionString).ReplaceService<IMigrationsSqlGenerator, SqlMigrationsSqlGenerator>();//sqlserver
            //optionsBuilder.UseLazyLoadingProxies().UseOracle(ConnectionString);//oracle
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Query<UserStaff>().ToView("UserStaff");
            base.OnModelCreating(modelBuilder);
        }


        public DbSet<Sys_OperationLog> Sys_OperationLog { get; set; }
        public DbSet<SysUser> SysUser { get; set; }
        public DbSet<Sys_Config> Sys_Config { get; set; }


    }
}