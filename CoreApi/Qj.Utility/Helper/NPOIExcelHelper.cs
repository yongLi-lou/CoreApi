using System;
using System.Data;
using System.Web;
using System.IO;
using NPOI;
using NPOI.HPSF;
using NPOI.HSSF;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.POIFS;
using NPOI.Util;
using System.Text;
using NPOI.SS.UserModel;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ICSharpCode.SharpZipLib.Zip;

namespace Qj.Utility.Helper
{
    /// <summary>
    /// NPOI
    /// </summary>
    public class NPIOExcelHelper
    {



        /// <summary>
        /// 
        /// </summary>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        public static DataTable Import(string strFileName)
        {

            int _r = 0;
            int _c = 0;
            try
            {
                DataTable dt = new DataTable();

                //HSSFWorkbook hssfworkbook;
                IWorkbook workbook;
                ISheet sheet;
                using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
                {
                    //hssfworkbook = new HSSFWorkbook(file);
                    workbook = WorkbookFactory.Create(file);//使用接口，自动识别excel2003/2007格式
                    sheet = workbook.GetSheetAt(0);//得到里面第一个sheet
                }
                //using (FileStream stream = new FileStream(@strFileName, FileMode.Open, FileAccess.Read))
                //{
                //    IWorkbook workbook = WorkbookFactory.Create(stream);//使用接口，自动识别excel2003/2007格式
                //    //for (int k = 0; k < 14; k++)
                //    //{
                //    ISheet sheet = workbook.GetSheetAt(0);//得到里面第一个sheet
                //}

                //NPOI.SS.UserModel.ISheet sheet = hssfworkbook.GetSheetAt(0);
                sheet = workbook.GetSheetAt(0);

                //HSSFSheet sheet = hssfworkbook.GetSheetAt(0);
                System.Collections.IEnumerator rows = sheet.GetRowEnumerator();



                NPOI.SS.UserModel.IRow headerRow = sheet.GetRow(0);
                NPOI.SS.UserModel.IRow headerRow1 = sheet.GetRow(1);

                int cellCount = headerRow.LastCellNum;
                int cellCount1 = headerRow1.LastCellNum;
                if (cellCount1 > cellCount)
                {
                    cellCount = cellCount1;
                }

                for (int j = 0; j < cellCount; j++)
                {
                    //NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                    //dt.Columns.Add(cell.ToString());
                    dt.Columns.Add("F" + (j + 1));
                }
                int row1 = sheet.PhysicalNumberOfRows;
                int row2 = sheet.LastRowNum;
                int ROW = row2 + 1;
                if (row1 > row2)
                {
                    ROW = row1;
                }
                for (int i = (sheet.FirstRowNum); i < ROW; i++)
                {
                    NPOI.SS.UserModel.IRow row = sheet.GetRow(i);
                    DataRow dataRow = dt.NewRow();
                    if (row != null)
                    {
                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            _r = i;
                            _c = j;
                            if (row.GetCell(j) != null)
                            {
                                //dataRow[j] = row.GetCell(j).ToString();
                                dataRow[j] = GetCellData(row, j);//主要读取格式或公式
                            }
                        }

                        dt.Rows.Add(dataRow);
                    }
                }
                return dt;
            }
            catch (Exception ee)
            {
                return null;
            }
        }

        /// <summary>
        /// 导出数据到EXCEL
        /// </summary>
        /// <param name="ds" name="fileName">ds 是System.Data.DataSet；fileName是文件名不包含，扩展名</param>
        public static byte[] ExportExcel(DataTable dt)
        {
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            int a = 1;
            //foreach (DataTable dt in ds.Tables)
            //{
            NPOI.SS.UserModel.ISheet sheet = book.CreateSheet("sheet" + a);
            NPOI.SS.UserModel.IRow row = sheet.CreateRow(0);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                row.CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
            }
            NPOI.SS.UserModel.ICell cell;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                NPOI.SS.UserModel.IRow row2 = sheet.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    cell = row2.CreateCell(j);
                    try
                    {
                        if (dt.Columns[i].DataType == typeof(DateTime) && dt.Rows[i][j] != DBNull.Value)
                            cell.SetCellValue(Convert.ToDateTime(dt.Rows[i][j]).ToString("yyyy-MM-dd hh:mm:ss"));
                        else
                            cell.SetCellValue(dt.Rows[i][j].ToString());
                    }
                    catch
                    {
                        cell.SetCellValue(dt.Rows[i][j].ToString());
                    }
                }
            }
            a++;
            //}

            MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Flush();
            ms.Position = 0;
            byte[] data = ms.ToArray();


            return data;
        }

        /// <summary>
        /// 判断列是否存在，并返回字典
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="FirstRowValue">第一行数据值</param>
        /// <param name="resultStr">返回值</param>
        /// <returns></returns>
        public static Dictionary<string, string> JudgeColumns(DataTable dt,string FirstRowValue)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            #region 判断读取列是否存在
            string retColums = "";
            foreach (string item in FirstRowValue.Split(','))
            {
                bool bol = false;
                foreach (DataColumn column in dt.Columns)
                {
                    if (dt.Rows[0][column].ToString() == item)
                    {
                        bol = !bol;
                        dic.Add(item, column.ColumnName);
                        break;
                    }
                }
                if (!bol)
                {
                    retColums += "," + item;
                }
            }

            if (retColums.Length > 0)
            {
               throw new Exception( "未找到以下数据列:" + retColums);
            }
            #endregion
            return dic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strFileName"></param>
        /// <param name="SheetIndex"></param>
        /// <returns></returns>
        public static DataTable ImportBySheetIndex(string strFileName, int SheetIndex)
        {

            int _r = 0;
            int _c = 0;
            try
            {
                DataTable dt = new DataTable();

                //HSSFWorkbook hssfworkbook;
                IWorkbook workbook;
                ISheet sheet;
                using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
                {
                    //hssfworkbook = new HSSFWorkbook(file);
                    workbook = WorkbookFactory.Create(file);//使用接口，自动识别excel2003/2007格式
                }
                //using (FileStream stream = new FileStream(@strFileName, FileMode.Open, FileAccess.Read))
                //{
                //    IWorkbook workbook = WorkbookFactory.Create(stream);//使用接口，自动识别excel2003/2007格式
                //    //for (int k = 0; k < 14; k++)
                //    //{
                //    ISheet sheet = workbook.GetSheetAt(0);//得到里面第一个sheet
                //}

                //NPOI.SS.UserModel.ISheet sheet = hssfworkbook.GetSheetAt(0);
                sheet = workbook.GetSheetAt(SheetIndex);//得到里面第SheetIndex个sheet

                //HSSFSheet sheet = hssfworkbook.GetSheetAt(0);
                System.Collections.IEnumerator rows = sheet.GetRowEnumerator();



                NPOI.SS.UserModel.IRow headerRow = sheet.GetRow(0);
                NPOI.SS.UserModel.IRow headerRow1 = sheet.GetRow(1);

                int cellCount = headerRow.LastCellNum;
                int cellCount1 = headerRow1.LastCellNum;
                if (cellCount1 > cellCount)
                {
                    cellCount = cellCount1;
                }

                for (int j = 0; j < cellCount; j++)
                {
                    //NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                    //dt.Columns.Add(cell.ToString());
                    dt.Columns.Add("F" + (j + 1));
                }
                int row1 = sheet.PhysicalNumberOfRows;
                int row2 = sheet.LastRowNum;
                int ROW = row2 + 1;
                if (row1 > row2)
                {
                    ROW = row1;
                }
                for (int i = (sheet.FirstRowNum); i < ROW; i++)
                {
                    NPOI.SS.UserModel.IRow row = sheet.GetRow(i);
                    DataRow dataRow = dt.NewRow();
                    if (row != null)
                    {
                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            _r = i;
                            _c = j;
                            if (row.GetCell(j) != null)
                            {
                                //dataRow[j] = row.GetCell(j).ToString();
                                dataRow[j] = GetCellData(row, j);//主要读取格式或公式
                            }
                        }

                        dt.Rows.Add(dataRow);
                    }
                }
                return dt;
            }
            catch (Exception ee)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strFileName"></param>
        /// <param name="SheetName"></param>
        /// <returns></returns>
        public static DataTable ImportBySheetName(string strFileName, string SheetName)
        {

            int _r = 0;
            int _c = 0;
            try
            {
                DataTable dt = new DataTable();

                //HSSFWorkbook hssfworkbook;
                IWorkbook workbook;
                ISheet sheet;
                using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
                {
                    //hssfworkbook = new HSSFWorkbook(file);
                    workbook = WorkbookFactory.Create(file);//使用接口，自动识别excel2003/2007格式

                }
                //using (FileStream stream = new FileStream(@strFileName, FileMode.Open, FileAccess.Read))
                //{
                //    IWorkbook workbook = WorkbookFactory.Create(stream);//使用接口，自动识别excel2003/2007格式
                //    //for (int k = 0; k < 14; k++)
                //    //{
                //    ISheet sheet = workbook.GetSheetAt(0);//得到里面第一个sheet
                //}

                //NPOI.SS.UserModel.ISheet sheet = hssfworkbook.GetSheetAt(0);
                sheet = workbook.GetSheet(SheetName);//得到里面sheet

                //HSSFSheet sheet = hssfworkbook.GetSheetAt(0);
                System.Collections.IEnumerator rows = sheet.GetRowEnumerator();



                NPOI.SS.UserModel.IRow headerRow = sheet.GetRow(0);
                NPOI.SS.UserModel.IRow headerRow1 = sheet.GetRow(1);

                int cellCount = headerRow.LastCellNum;
                int cellCount1 = headerRow1.LastCellNum;
                if (cellCount1 > cellCount)
                {
                    cellCount = cellCount1;
                }

                for (int j = 0; j < cellCount; j++)
                {
                    //NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                    //dt.Columns.Add(cell.ToString());
                    dt.Columns.Add("F" + (j + 1));
                }
                int row1 = sheet.PhysicalNumberOfRows;
                int row2 = sheet.LastRowNum;
                int ROW = row2 + 1;
                if (row1 > row2)
                {
                    ROW = row1;
                }
                for (int i = (sheet.FirstRowNum); i < ROW; i++)
                {
                    NPOI.SS.UserModel.IRow row = sheet.GetRow(i);
                    DataRow dataRow = dt.NewRow();
                    if (row != null)
                    {
                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            _r = i;
                            _c = j;
                            if (row.GetCell(j) != null)
                            {
                                //dataRow[j] = row.GetCell(j).ToString();
                                dataRow[j] = GetCellData(row, j);//主要读取格式或公式
                            }
                        }

                        dt.Rows.Add(dataRow);
                    }
                }
                return dt;
            }
            catch (Exception ee)
            {
                return null;
            }
        }




        #region MyRegion

        /// <summary>
        /// 读Excel-得到不同数据类型单元格的数据
        /// </summary>
        /// <param name="datatype">数据类型</param>
        /// <param name="row">数据中的一行</param>
        /// <param name="column">哪列</param>
        /// <returns></returns>
        private static object GetCellData(NpoiDataType datatype, IRow row, int column)
        {
            object obj = row.GetCell(column) ?? null;
            if (datatype == NpoiDataType.Datetime)
            {
                string v = "";
                try
                {
                    v = row.GetCell(column).StringCellValue;
                }
                catch (Exception e1)
                {

                    v = row.GetCell(column).DateCellValue.ToString("yyyy-MM-dd hh:mm:ss");
                }
                if (v != "")
                {
                    try
                    {
                        obj = row.GetCell(column).DateCellValue.ToString("yyyy-MM-dd hh:mm:ss");

                    }
                    catch (Exception e2)
                    {
                        obj = Convert.ToDateTime(v).ToString("yyyy-MM-dd hh:mm:ss");
                    }
                }
                else
                    obj = DBNull.Value;


            }
            if (datatype == NpoiDataType.Numeric)
            {
                obj = DBNull.Value;
                try
                {
                    //if (row.GetCell(column).StringCellValue != "")
                    obj = row.GetCell(column).NumericCellValue;
                }
                catch (Exception e3)
                {
                    //Errors.WriteError(obj.ToString() + "!" + e3.ToString());
                    obj = row.GetCell(column).StringCellValue;
                }

            }
            return obj;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static object GetCellData(IRow row, int column)
        {
            try
            {
                ICell hs = row.GetCell(column);
                NpoiDataType datatype = GetCellDataType(hs); //NpoiDataType.Blank;
                object obj = row.GetCell(column) ?? null;
                if (datatype == NpoiDataType.Datetime)
                {
                    string v = "";
                    try
                    {
                        v = row.GetCell(column).StringCellValue;
                    }
                    catch (Exception e1)
                    {
                        v = row.GetCell(column).DateCellValue.ToString("yyyy-MM-dd hh:mm:ss");
                    }
                    if (v != "")
                    {
                        try
                        {
                            obj = row.GetCell(column).DateCellValue.ToString("yyyy-MM-dd hh:mm:ss");

                        }
                        catch (Exception e2)
                        {
                            obj = Convert.ToDateTime(v).ToString("yyyy-MM-dd hh:mm:ss");
                        }
                    }
                    else
                        obj = DBNull.Value;


                }
                else if (datatype == NpoiDataType.Numeric)
                {
                    obj = DBNull.Value;
                    try
                    {
                        //if (row.GetCell(column).StringCellValue != "")
                        obj = row.GetCell(column).NumericCellValue;
                    }
                    catch (Exception e3)
                    {
                        obj = row.GetCell(column).StringCellValue;
                    }

                }
                else if (datatype == NpoiDataType.String && hs.CellType == CellType.Formula)
                {
                    obj = row.GetCell(column).StringCellValue;
                }
                return obj;
            }
            catch (Exception e22)
            {
                return "";
            }
        }

        #region 获取单元格数据类型
        /// <summary>
        /// 获取单元格数据类型
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        private static NpoiDataType GetCellDataType(ICell hs)
        {
            NpoiDataType dtype;
            DateTime t1;
            string cellvalue = "";

            switch (hs.CellType)
            {
                case CellType.Blank:
                    dtype = NpoiDataType.String;
                    cellvalue = hs.StringCellValue;
                    break;
                case CellType.Boolean:
                    dtype = NpoiDataType.Bool;
                    break;
                case CellType.Numeric:
                    dtype = NpoiDataType.String;
                    try
                    {
                        if (hs.NumericCellValue.ToString().Contains("-") || hs.NumericCellValue.ToString().Contains("/") || hs.ToString().Contains("-") || hs.ToString().Contains("/"))
                        {
                            hs.DateCellValue.ToString();
                            dtype = NpoiDataType.Datetime;
                        }
                    }
                    catch { }
                    cellvalue = hs.NumericCellValue.ToString();
                    break;
                case CellType.String:
                    dtype = NpoiDataType.String;
                    cellvalue = hs.StringCellValue;
                    break;
                case CellType.Error:
                    dtype = NpoiDataType.Error;
                    break;
                case CellType.Formula:
                    {
                        dtype = NpoiDataType.String;
                        try
                        {
                            if (hs.CachedFormulaResultType == CellType.Numeric && hs.NumericCellValue.ToString() != "")
                            {
                                dtype = NpoiDataType.Numeric;
                                cellvalue = hs.NumericCellValue.ToString();
                            }
                            else if (hs.CachedFormulaResultType == CellType.Numeric && hs.DateCellValue.ToString() != "")
                            {
                                dtype = NpoiDataType.Datetime;
                                cellvalue = hs.DateCellValue.ToString();
                            }
                            else if (hs.RichStringCellValue.ToString() != "")
                            {
                                dtype = NpoiDataType.String;
                                cellvalue = hs.RichStringCellValue.ToString();
                            }
                        }
                        catch
                        {
                            try
                            {
                                if (hs.CachedFormulaResultType == CellType.Numeric && hs.NumericCellValue.ToString() != "")
                                {
                                    dtype = NpoiDataType.Numeric;
                                    cellvalue = hs.NumericCellValue.ToString();
                                }
                            }
                            catch
                            {
                                //cellvalue = hs.StringCellValue;
                            }
                        }

                        break;
                    }
                default:
                    dtype = NpoiDataType.Datetime;
                    break;
            }
            //if (cellvalue != "" && DateTime.TryParse(cellvalue, out t1)) dtype = NpoiDataType.Datetime;
            return dtype;
        }
        #endregion
        #endregion


    }


    /// <summary>
    /// 将DataTable数据源转换成实体类
    /// </summary>
    /// <typeparam name="T">实体</typeparam>
    public static class ToModel<T> where T : new()
    {
        /// <summary>
        /// 将DataTable数据源转换成实体类
        /// </summary>
        public static List<T> ConvertToModel(DataTable dt)
        {
            List<T> ts = new List<T>();// 定义集合
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                PropertyInfo[] propertys = t.GetType().GetProperties();// 获得此模型的公共属性
                int a = 0;
                foreach (PropertyInfo pi in propertys)
                {

                    if (dt.Columns.Count-1>= a)
                    {
                        if (!pi.CanWrite) continue;
                        var value = dr[a];
                        if (value != DBNull.Value)
                        {
                            switch (pi.PropertyType.FullName)
                            {
                                case "System.Decimal":
                                    pi.SetValue(t, decimal.Parse(value.ToString()), null);
                                    break;
                                case "System.String":
                                    pi.SetValue(t, value.ToString(), null);
                                    break;
                                case "System.Int32":
                                    pi.SetValue(t, int.Parse(value.ToString()), null);
                                    break;
                                default:
                                    pi.SetValue(t, value, null);
                                    break;
                            }
                        }
                    }
                    a++;
                }
                ts.Add(t);
            }
            return ts;
        }
    }
    #region 枚举(Excel单元格数据类型)
    /// <summary>
    /// 枚举(Excel单元格数据类型)
    /// </summary>
    public enum NpoiDataType
    {
        /// <summary>
        /// 字符串类型-值为1
        /// </summary>
        String,
        /// <summary>
        /// 布尔类型-值为2
        /// </summary>
        Bool,
        /// <summary>
        /// 时间类型-值为3
        /// </summary>
        Datetime,
        /// <summary>
        /// 数字类型-值为4
        /// </summary>
        Numeric,
        /// <summary>
        /// 复杂文本类型-值为5
        /// </summary>
        Richtext,
        /// <summary>
        /// 空白
        /// </summary>
        Blank,
        /// <summary>
        /// 错误
        /// </summary>
        Error
    }
    #endregion
}