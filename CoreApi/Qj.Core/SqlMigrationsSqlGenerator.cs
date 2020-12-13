using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure.Internal;
using Qj.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Qj.Core
{
    public class SqlMigrationsSqlGenerator : SqlServerMigrationsSqlGenerator
    {
        public SqlMigrationsSqlGenerator(
            MigrationsSqlGeneratorDependencies dependencies,
            IMigrationsAnnotationProvider migrationsAnnotations)
            : base(dependencies, migrationsAnnotations)
        {
        }

        protected override void Generate(MigrationOperation operation, IModel model, MigrationCommandListBuilder builder)
        {
            base.Generate(operation, model, builder);



            try
            {
                if (operation is CreateTableOperation || operation is AlterTableOperation)
                {

                    CreateTableComment(operation, model, builder);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("生成注释错误：" + ex.Message);
            }


            try
            {
                if (operation is AddColumnOperation || operation is AlterColumnOperation)
                    CreateColumnComment(operation, model, builder);
            }
            catch (Exception ex)
            {
                Console.WriteLine("生成注释错误：" + ex.Message);
            }

        }

        /// <summary>
        /// Create table comment.
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="builder"></param>
        private void CreateTableComment(MigrationOperation operation, IModel model, MigrationCommandListBuilder builder)
        {
            string tableName = string.Empty;
            string description = string.Empty;
            if (operation is AlterTableOperation)
            {
                var t = operation as AlterColumnOperation;
                tableName = (operation as AlterTableOperation).Name;
            }

            if (operation is CreateTableOperation)
            {
                var t = operation as CreateTableOperation;
                var addColumnsOperation = t.Columns;
                tableName = (operation as CreateTableOperation).Name;

                foreach (var item in addColumnsOperation)
                {
                    CreateColumnComment(item, model, builder);
                }
            }
            description = DbDescriptionHelper.GetDescription(tableName);


            if (tableName.IsNullOrWhiteSpace())
                throw new Exception("Create table comment error.");

            var sqlHelper = Dependencies.SqlGenerationHelper;
            builder
            .Append($@"
              

                   
	            if exists(
	            		SELECT tbs.name 表名,ds.value 描述   FROM sys.extended_properties ds  
	            					LEFT JOIN sysobjects tbs ON ds.major_id=tbs.id  WHERE  ds.minor_id=0 and  tbs.name='{tableName}')
	            	begin
	            	
	            		if(1=1)
	            			begin
	            					EXEC sp_updateextendedproperty 'MS_Description','{description}','user',dbo,'table','{tableName}',null,null
	            			end
	            	end
	            else
	            	begin
	            					EXECUTE sp_addextendedproperty N'MS_Description','{description}', N'user', N'dbo', N'table', '{tableName}', NULL, NULL
	            	end")

            .EndCommand();

        }

        /// <summary>
        /// Create column comment.
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="builder"></param>
        private void CreateColumnComment(MigrationOperation operation, IModel model, MigrationCommandListBuilder builder)
        {
            //alter table a1log modify column UUID VARCHAR(26) comment '修改后的字段注释';
            string tableName = string.Empty;
            string columnName = string.Empty;
            string columnType = string.Empty;
            string description = string.Empty;
            if (operation is AlterColumnOperation)
            {
                var t = (operation as AlterColumnOperation);
                tableName = t.Table;
                columnName = t.Name;
                columnType = GetColumnType(t.Schema, t.Table, t.Name, t.ClrType, t.IsUnicode, t.MaxLength, t.IsFixedLength, t.IsRowVersion, model);
                description = DbDescriptionHelper.GetDescription(tableName, columnName);
            }

            if (operation is AddColumnOperation)
            {
                var t = (operation as AddColumnOperation);
                tableName = t.Table;
                columnName = t.Name;
                columnType = GetColumnType(t.Schema, t.Table, t.Name, t.ClrType, t.IsUnicode, t.MaxLength, t.IsFixedLength, t.IsRowVersion, model);
                description = DbDescriptionHelper.GetDescription(tableName, columnName);
            }

            if (columnName.IsNullOrWhiteSpace() || tableName.IsNullOrWhiteSpace() || columnType.IsNullOrWhiteSpace())
                throw new Exception("Create columnt comment error." + columnName + "/" + tableName + "/" + columnType);

            var sqlHelper = Dependencies.SqlGenerationHelper;

            builder
            .Append($@"

             if exists(
					 SELECT t.[name] AS 表名,c.[name] AS 字段名,cast(ep.[value] 
						as varchar(100)) AS [字段说明]
						FROM sys.tables AS t
						INNER JOIN sys.columns 
						AS c ON t.object_id = c.object_id
						 LEFT JOIN sys.extended_properties AS ep 
						ON ep.major_id = c.object_id AND ep.minor_id = c.column_id WHERE ep.class =1 
						AND t.name='{tableName}'  and c.name='{columnName}'
	        	  )
	        	begin
	        	
	        		if(1=1)
	        			begin
	        					EXECUTE sp_updateextendedproperty N'MS_Description', '{description}', N'user', N'dbo', N'table','{tableName}', N'column', '{columnName}'
	        			end
	        	end
	        else
	        	begin
	        					EXECUTE sp_addextendedproperty N'MS_Description', '{description}', N'user', N'dbo', N'table', '{tableName}', N'column', '{columnName}'
	        	end")
            .EndCommand();

        }


    }


    public class DbDescriptionHelper
    {
        public static string b { get; set; } = "Qj.Models";
        //public static string c { get; set; } = @"D:\工作\Core模板\代码模板\Core\core_template\CoreApi\Qj.Models\sys";

        public static List<DbDescription> list { get; set; }

        public static string GetDescription(string table, string column = "")
        {
            if (list == null || list.Count() == 0)
            {
                list = GetDescription();
            }

            if (!string.IsNullOrWhiteSpace(table))
            {
                if (string.IsNullOrWhiteSpace(column))
                {
                    var x = list.FirstOrDefault(p => p.Name == table);
                    if (x != null)
                        return x.Description;
                    return string.Empty;
                }
                else
                {
                    var x = list.FirstOrDefault(p => p.Name == table);
                    if (x != null)
                    {
                        var y = x.Column;
                        if (y.IsNotNull())
                        {
                            var z = y.FirstOrDefault(p => p.Name == column);
                            if (z != null)
                                return z.Description;
                        }
                    }
                    return string.Empty;
                }
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// 判断自增列
        /// </summary>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static int Getzzpk(string table, string column)
        {
            if (list == null || list.Count() == 0)
            {
                list = DbDescriptionHelper.GetDescription();
            }

            if (!string.IsNullOrWhiteSpace(table))
            {
                if (!string.IsNullOrWhiteSpace(column))
                {
                    var x = list.FirstOrDefault(p => p.Name == table);
                    if (x != null)
                    {
                        var y = x.Column;
                        if (y.IsNotNull())
                        {
                            var z = y.FirstOrDefault(p => p.Name == column);
                            if (z != null)
                                return z.IsZZPK;
                        }
                    }
                    return 0;
                }
            }

            return 0;
        }
        public static List<DbDescription> GetDescription()
        {
            var d = new List<DbDescription>();

            var e = Assembly.Load(b);
            var f = e?.GetTypes();
            var g = f?
                .Where(t => t.IsClass
                && !t.IsGenericType
                && !t.IsAbstract
                //&& t.GetInterfaces().Any(m => m.GetGenericTypeDefinition() == typeof(IBaseModel<>))
                ).ToList();

            foreach (var h in g)
            {
                var i = new DbDescription();

                System.Type t = e.GetType(h.FullName);
                DescriptionAttribute attributeTB = Attribute.GetCustomAttribute(t, typeof(DescriptionAttribute)) as DescriptionAttribute;
                var z = "";
                if (attributeTB != null)
                {
                    z = attributeTB.Description;
                }
                var n = new List<DbDescription>();

                //3.2.3 获取实体类所有的公共属性
                List<PropertyInfo> propertyInfos = t.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
                propertyInfos.ForEach(p =>
                {
                    //Console.WriteLine(p.Name);
                    DescriptionAttribute attribute = Attribute.GetCustomAttribute(p, typeof(DescriptionAttribute)) as DescriptionAttribute;

                    DatabaseGeneratedAttribute dbattribute = Attribute.GetCustomAttribute(p, typeof(DatabaseGeneratedAttribute)) as DatabaseGeneratedAttribute;


                    if (attribute != null || (dbattribute != null && dbattribute.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity))
                    {
                        var dect = attribute != null ? attribute.Description : "";
                        int iszzpk = 0;
                        if (dbattribute != null && dbattribute.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity)
                        {
                            iszzpk = 1;
                        }
                        n.Add(new DbDescription { Name = p.Name, Description = dect, IsZZPK = iszzpk });
                    }



                    #region MyRegion
                    //var j = c + "\\" + h.Name + ".cs";
                    //var k = File.ReadAllText(j);
                    //k = k.Substring(k.IndexOf("{") + 1, k.LastIndexOf("}") - k.IndexOf("{") - 1).Replace("\n", "");

                    //var l = k.Substring(k.IndexOf(" {") + 2, k.LastIndexOf(" }") - k.IndexOf(" {") - 1).Replace("\n", "");
                    //string[] slipt = { "}\r" };
                    //var m = l.Split(slipt, StringSplitOptions.None).ToList();

                    //var n = new List<DbDescription>();
                    //foreach (var o in m)
                    //{
                    //    var p = o.Replace("///", "");

                    //    var q = p.IndexOf("<summary>");
                    //    var r = p.LastIndexOf("</summary>");

                    //    var s = p.IndexOf("public");
                    //    var t = p.IndexOf("{");

                    //    var u = (q > 0 && r > 0) ? p.Substring(q + 9, r - q - 10).Replace("\r", "").Replace(" ", "") : "";
                    //    var v = (s > 0 && t > 0) ? p.Substring(s, t - s).Split(' ')[2] : "";

                    //    n.Add(new DbDescription()
                    //    {
                    //        Description = u,
                    //        Name = v
                    //    });
                    //}

                    //var w = k.Substring(0, k.IndexOf("{\r") - 1);
                    //w = w.Replace("///", "");

                    //var x = w.IndexOf("<summary>");
                    //var y = w.LastIndexOf("</summary>");
                    //var z = (x > 0 && y > 0) ? w.Substring(x + 9, y - x - 10).Replace("\r", "").Replace(" ", "") : "";
                    #endregion

                });

                d.Add(new DbDescription()
                {
                    Name = h.Name,
                    Description = z,
                    Column = n
                });


            }
            return d;
        }



    }

    public class DbDescriptionHelperBysummary
    {
        public static string b { get; set; } = "Qj.Models";
        public static string c { get; set; } = @"D:\工作\Core模板\代码模板\Core\core_template\CoreApi\Qj.Models\";

        public static List<DbDescription> list { get; set; }

        public static List<DbDescription> zzpklist { get; set; }


        public static string GetDescription(string table, string column = "")
        {
            if (list == null || list.Count() == 0)
            {
                list = GetDescription();
            }

            if (!string.IsNullOrWhiteSpace(table))
            {
                if (string.IsNullOrWhiteSpace(column))
                {
                    var x = list.FirstOrDefault(p => p.Name == table);
                    if (x != null)
                        return x.Description;
                    return string.Empty;
                }
                else
                {
                    var x = list.FirstOrDefault(p => p.Name == table);
                    if (x != null)
                    {
                        var y = x.Column;
                        if (y.IsNotNull())
                        {
                            var z = y.FirstOrDefault(p => p.Name == column);
                            if (z != null)
                                return z.Description;
                        }
                    }
                    return string.Empty;
                }
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// 判断自增列
        /// </summary>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static int Getzzpk(string table, string column)
        {
            if (zzpklist == null || zzpklist.Count() == 0)
            {
                zzpklist = DbDescriptionHelper.GetDescription();
            }

            if (!string.IsNullOrWhiteSpace(table))
            {
                if (!string.IsNullOrWhiteSpace(column))
                {
                    var x = zzpklist.FirstOrDefault(p => p.Name == table);
                    if (x != null)
                    {
                        var y = x.Column;
                        if (y.IsNotNull())
                        {
                            var z = y.FirstOrDefault(p => p.Name == column);
                            if (z != null)
                                return z.IsZZPK;
                        }
                    }
                    return 0;
                }
            }

            return 0;
        }


        public static List<DbDescription> GetDescription()
        {
            var d = new List<DbDescription>();

            List<FileListModel> filelist = new List<FileListModel>();
            getDirectory(filelist, c);

            var e = Assembly.Load(b);
            var f = e?.GetTypes();
            var g = f?
                .Where(t => t.IsClass
                && !t.IsGenericType
                && !t.IsAbstract
                //&& t.GetInterfaces().Any(m => m.GetGenericTypeDefinition() == typeof(IBaseModel<>))
                ).ToList();

            foreach (var h in g)
            {
                var i = new DbDescription();

                System.Type tp = e.GetType(h.FullName);
                DescriptionAttribute attributeTB = Attribute.GetCustomAttribute(tp, typeof(DescriptionAttribute)) as DescriptionAttribute;
                var z = "";
                if (attributeTB != null)
                {
                    z = attributeTB.Description;
                }
                var n = new List<DbDescription>();

                //3.2.3 获取实体类所有的公共属性
                List<PropertyInfo> propertyInfos = tp.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
                //propertyInfos.ForEach(p =>
                //{
                //    //Console.WriteLine(p.Name);
                //    DescriptionAttribute attribute = Attribute.GetCustomAttribute(p, typeof(DescriptionAttribute)) as DescriptionAttribute;

                //    if (attribute != null)
                //    {
                //        var dect = attribute.Description;
                //        n.Add(new DbDescription { Name = p.Name, Description = dect });
                //    }
                //});


                if (filelist.Count(xx => xx.FileName == h.Name + ".cs") == 0)
                {
                    continue;
                }

                #region MyRegion
                var j = filelist.FirstOrDefault(xx => xx.FileName == h.Name + ".cs").FullName;

                //var j = c + "\\" + h.Name + ".cs";
                var k = File.ReadAllText(j);
                k = k.Substring(k.IndexOf("{") + 1, k.LastIndexOf("}") - k.IndexOf("{") - 1).Replace("\n", "");

                var l = k.Substring(k.IndexOf(" {") + 2, k.LastIndexOf(" }") - k.IndexOf(" {") - 1).Replace("\n", "");
                string[] slipt = { "}\r" };
                var m = l.Split(slipt, StringSplitOptions.None).ToList();

                //var n = new List<DbDescription>();
                foreach (var o in m)
                {
                    var p = o.Replace("///", "");

                    var q = p.IndexOf("<summary>");
                    var r = p.LastIndexOf("</summary>");

                    var s = p.IndexOf("public");
                    var t = p.IndexOf("{");

                    var u = (q > 0 && r > 0) ? p.Substring(q + 9, r - q - 10).Replace("\r", "").Replace(" ", "") : "";
                    var v = (s > 0 && t > 0) ? p.Substring(s, t - s).Split(' ')[2] : "";

                    n.Add(new DbDescription()
                    {
                        Description = u,
                        Name = v
                    });
                }

                var w = k.Substring(0, k.IndexOf("{\r") - 1);
                w = w.Replace("///", "");

                var x = w.IndexOf("<summary>");
                var y = w.LastIndexOf("</summary>");
                z = (x > 0 && y > 0) ? w.Substring(x + 9, y - x - 10).Replace("\r", "").Replace(" ", "") : "";
                #endregion


                d.Add(new DbDescription()
                {
                    Name = h.Name,
                    Description = z,
                    Column = n
                });


            }
            return d;
        }


        /// <summary>
        /// 获得指定路径下所有文件名
        /// </summary>
        public static void getFileName(List<FileListModel> list, string path)
        {
            DirectoryInfo root = new DirectoryInfo(path);
            foreach (FileInfo f in root.GetFiles("*.cs"))
            {
                list.Add(new FileListModel(f.Name, f.FullName, f.FullName));
            }
        }

        /// <summary>
        /// 获得指定路径下所有子目录名
        /// </summary>
        public static void getDirectory(List<FileListModel> list, string path)
        {
            getFileName(list, path);
            DirectoryInfo root = new DirectoryInfo(path);
            foreach (DirectoryInfo d in root.GetDirectories())
            {
                getDirectory(list, d.FullName);
            }
        }


    }

    public class FileListModel
    {
        public FileListModel()
        {

        }
        public FileListModel(string filename, string fullname, string path)
        {
            this.FileName = filename;
            this.FullName = fullname;
            this.Path = path;
        }
        public string FileName { get; set; }
        public string FullName { get; set; }
        public string Path { get; set; }
    }


    public class DbDescription
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int IsZZPK { get; set; }
        public List<DbDescription> Column { get; set; }
    }
}
