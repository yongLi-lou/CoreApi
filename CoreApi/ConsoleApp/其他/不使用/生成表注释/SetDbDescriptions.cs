using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ConsoleApp.其他.不使用
{
    class SetDbDescriptions
    {
        public void send()
        {
            string dbtype = "sqlserver";
            string dbconnStr = "server=sh-cdb-3ql7v8s2.sql.tencentcdb.com;port=63816;database=dongtou_kefu;uid=qjroot;pwd=qijin=mysql;CharSet=utf8";

            //读取dll
            System.Reflection.Assembly a = System.Reflection.Assembly.LoadFrom(@"Qj.Models.dll");
            var listmodel = a.DefinedTypes;

            List<TableModel> listtm = new List<TableModel>();

            //循环类
            foreach (var item in listmodel)
            {
                System.Type t = a.GetType(item.FullName);

                //3.2.2 获取实体类类型对象
                //Type t = typeof(obj["Sys_Config"]);
                //3.2.3 获取实体类所有的公共属性
                List<PropertyInfo> propertyInfos = t.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
                //3.2.4 创建实体属性字典集合
                Dictionary<string, PropertyInfo> dicPropertys = new Dictionary<string, PropertyInfo>();
                //3.2.5 将实体属性中要修改的属性名 添加到字典集合中  键：属性名  值：属性对象
                DescriptionAttribute attributeTab = Attribute.GetCustomAttribute(t, typeof(DescriptionAttribute)) as DescriptionAttribute;
                //获取表名
                TableAttribute attributeTabName = Attribute.GetCustomAttribute(t, typeof(TableAttribute)) as TableAttribute;

                var tm = new TableModel();

                if (attributeTabName == null)
                {
                    Shell.WriteLineYellow(t.Name + "  表名未维护跳出");
                    Shell.WriteLine("");
                    continue; ;
                }
                else
                {
                    tm.TableName = attributeTabName.Name;
                }

                if (attributeTab != null)
                {
                    tm.TableName = attributeTab.Description;
                }
                tm.listTCmodel = new List<TColumnModel>();

                propertyInfos.ForEach(p =>
                {
                    //Console.WriteLine(p.Name);
                    DescriptionAttribute attribute = Attribute.GetCustomAttribute(p, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    var tc = new TColumnModel();
                    tc.ColumnName = p.Name;
                    if (attribute != null)
                    {
                        tc.Description = attribute.Description;
                    }
                    tm.listTCmodel.Add(tc);
                });
            }

            #region 生成说明

            if (dbtype == "sqlserver")
            {
                new Sqlserverdb(dbconnStr);

                string sqlcc = "select   *   from   dbo.sysobjects   where   id   =   object_id(N'CreateColumnDescriptions')   and   OBJECTPROPERTY(id, N'IsProcedure')   =   1";
                string sqltb = "select   *   from   dbo.sysobjects   where   id   =   object_id(N'CreateTableDescriptions')   and   OBJECTPROPERTY(id, N'IsProcedure')   =   1";
                var ishas = Sqlserverdb.ExecuteScalar(sqlcc);
                if (ishas == null || ishas.ToString() == "")
                {
                    #region sql
                    var exsql = @"
                                        SET ANSI_NULLS ON
                                        GO
                                        SET QUOTED_IDENTIFIER ON
                                        GO
                                        -- =============================================
                                        -- Author:		<Author,,Name>
                                        -- Create date: <Create Date,,>
                                        -- Description:	<Description,,>
                                        -- =============================================
                                        CREATE PROCEDURE CreateColumnDescriptions 
	                                        @TableName varchar(200),
	                                        @Column varchar(200),
	                                        @Descriptions  varchar(200),
	                                        @isup int
                                        AS
                                        BEGIN
                                         /*
                                         exec  CreateColumnDescriptions 'ArticleLikes','id','xxx2',0
                                         */
	
	                                        if exists(
					                                         SELECT t.[name] AS 表名,c.[name] AS 字段名,cast(ep.[value] 
						                                        as varchar(100)) AS [字段说明]
						                                        FROM sys.tables AS t
						                                        INNER JOIN sys.columns 
						                                        AS c ON t.object_id = c.object_id
						                                         LEFT JOIN sys.extended_properties AS ep 
						                                        ON ep.major_id = c.object_id AND ep.minor_id = c.column_id WHERE ep.class =1 
						                                        AND t.name=@TableName  and c.name=@Column
		                                          )
		                                        begin
		
			                                        if(@isup=1)
				                                        begin
						                                        EXECUTE sp_updateextendedproperty N'MS_Description', @Descriptions, N'user', N'dbo', N'table', @TableName, N'column', @Column
				                                        end
		                                        end
	                                        else
		                                        begin
						                                        EXECUTE sp_addextendedproperty N'MS_Description', @Descriptions, N'user', N'dbo', N'table', @TableName, N'column', @Column
		                                        end


                                        END
                                        GO

                                    ";
                    #endregion

                    Sqlserverdb.ExecuteNonQuery(exsql);


                }
                ishas = Sqlserverdb.ExecuteScalar(sqltb);
                if (ishas == null || ishas.ToString() == "")
                {
                    #region sql
                    var exsql = @"
                                    SET ANSI_NULLS ON
                                    GO
                                    SET QUOTED_IDENTIFIER ON
                                    GO
                                    -- =============================================
                                    -- Author:		<Author,,Name>
                                    -- Create date: <Create Date,,>
                                    -- Description:	<Description,,>
                                    -- =============================================
                                    CREATE PROCEDURE CreateTableDescriptions 
                                    	@TableName varchar(200),
                                    	@Descriptions  varchar(200),
                                    	@isup int
                                    AS
                                    BEGIN
                                     /*
                                     exec  CreateTableDescriptions 'ArticleLikes','xxx',0
                                     */
                                    	
                                    	if exists(
                                    			SELECT tbs.name 表名,ds.value 描述   FROM sys.extended_properties ds  
                                    						LEFT JOIN sysobjects tbs ON ds.major_id=tbs.id  WHERE  ds.minor_id=0 and  tbs.name=@TableName)
                                    		begin
                                    		
                                    			if(@isup=1)
                                    				begin
                                    						EXEC sp_updateextendedproperty 'MS_Description',@Descriptions,'user',dbo,'table',@TableName,null,null
                                    				end
                                    		end
                                    	else
                                    		begin
                                    						EXECUTE sp_addextendedproperty N'MS_Description',@Descriptions, N'user', N'dbo', N'table', @TableName, NULL, NULL
                                    		end
                                    
                                    
                                    END
                                    GO
                                    ";
                    #endregion

                    Sqlserverdb.ExecuteNonQuery(exsql);
                }

            }

            foreach (var item in listtm)
            {

            }

            #endregion
        }
    }

    public class TableModel
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 表说明
        /// </summary>
        public string Description { get; set; }


        public List<TColumnModel> listTCmodel { get; set; }
    }

    public class TColumnModel
    {
        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 列说明
        /// </summary>
        public string Description { get; set; }

    }
}
