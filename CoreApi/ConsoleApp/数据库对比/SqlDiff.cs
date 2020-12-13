using ConsoleApp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsoleApp
{
    public class SqlDiff
    {


        public void AppStart()
        {
            Shell.WriteLine("-----------------启动数据库字段对比工具----------------");
            Shell.WriteLine("----------------------------------------------------");
            Shell.WriteLine("");

            var dt1 = new Sqlserverdb("Server=qds163264300.my3w.com; Database=qds163264300_db; User Id =qds163264300; Password=lHgqa5nEmX;").GetMysqlDBData();//测试有
            var dt2 = new Sqlserverdb("Server=qds177963126.my3w.com; Database=qds177963126_db; User Id =qds177963126; Password=dfdf-oHZdfU-3P1ovvn-woty;").GetMysqlDBData();//正式没有的
            //var dt2=new Mysqldb("server=gz-cdb-n3t2q9al.sql.tencentcdb.com;port=60373;database=loantest;uid=root;pwd=fdhx@2020;CharSet=utf8").GetMysqlDBData("loantest");

            List<string> listadd = new List<string>();
            List<string> listedit = new List<string>();

            foreach (DataRow item in dt1.Rows)
            {
                var tablename = item["表名"].ToString();
                var columns = item["字段名"].ToString();
                var datatype = item["类型"].ToString();
                var dataleng = item["长度"].ToString();
                var isnull = item["允许空值"].ToString();

                DataRow[] rows = dt2.Select($"表名='{tablename}' and  字段名='{columns}'");
                if (rows.Count() == 0)
                {
                    string addsql = $@"  alter table {tablename} add {columns} {getDataType(datatype, dataleng, isnull)}";
                    listadd.Add(addsql);
                    Console.WriteLine(addsql);
                    continue;
                }

                var datatype2 = rows[0]["类型"].ToString();
                if (datatype != datatype2)
                {
                    string addsql = $@" /*{datatype2}==> {datatype}*/ 
                                        alter table {tablename} alter COLUMN {getDataType(datatype, dataleng, isnull)}";
                    listedit.Add(addsql);
                    Console.WriteLine(addsql);
                }

            }

            string dir = AppDomain.CurrentDomain.BaseDirectory + "/Logs";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var path = dir + "\\" + "sqlexe.txt";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            FileStream fs = File.Create(path);
            fs.Close();

            StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default);
            sw.WriteLine("--------------------Start添加--------------------");
            listadd.ForEach(e =>
            {
                sw.WriteLine(e);
            });
            sw.WriteLine("--------------------End添加--------------------");

            sw.WriteLine("--------------------Star修改--------------------");
            listedit.ForEach(e =>
            {
                sw.WriteLine(e);
            });
            sw.WriteLine("--------------------End修改--------------------");

            sw.Close();

            Console.WriteLine("对比结束");

        }



        public string getDataType(string datatype, string dataleng, string isnull)
        {
            string resylt = "varchar(200)";

            switch (datatype)
            {
                case "varchar":
                    if (dataleng == "-1")
                    {
                        resylt = $" {datatype}(max) ";
                        break;
                    }
                    resylt = $" {datatype}({dataleng}) ";
                    break;
                case "nvarchar":
                    if (dataleng == "-1")
                    {
                        resylt = $" {datatype}(max) ";
                        break;
                    }
                    resylt = $" {datatype}({dataleng}) ";
                    break;
                case "decimal":
                    resylt = $" {datatype}(18,2) ";
                    break;
                default:
                    resylt = $" {datatype} ";
                    break;
            }

            if (isnull == "1" || isnull == "YES")
            {
                resylt = resylt + "  null";
            }

            return resylt;
        }
    }
}
