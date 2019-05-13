// Modified By      YYYY-MM-DD
// Jay             2019-01-16 - Creation

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace WaterCaseTracking
{
    public class LoadExcel
    {
        /// <summary>
        /// 讀取Xlsx轉成DataTable
        /// </summary>
        /// <returns></returns>
        public static DataTable LoadXlsx(HttpPostedFileBase file)
        {
            //儲存Xls的DataTable
            DataTable dt = new DataTable();

            XSSFWorkbook workbook;
            //讀取專案內中的sample.xls 的excel 檔案
            using (file.InputStream)
            {
                workbook = new XSSFWorkbook(file.InputStream);
            }

            //讀取Sheet1 工作表
            var sheet = workbook.GetSheetAt(0);


            for (int row = 0; row <= sheet.LastRowNum; row++)
            {
                if (sheet.GetRow(row) != null) //null is when the row only contains empty cells 
                {
                    if (row == 0)//標題列
                    {
                        foreach (var c in sheet.GetRow(row).Cells)
                        {
                            dt.Columns.Add(HttpUtility.UrlEncode(c.StringCellValue));
                        }
                    }
                    else
                    {
                        DataRow r = dt.NewRow();
                        foreach (var c in sheet.GetRow(row).Cells)
                        {
                            //如果是數字型 就要取 NumericCellValue  這屬性的值
                            if (c.CellType == CellType.Numeric)
                                if (HSSFDateUtil.IsCellDateFormatted(c))//日期類型
                                {
                                    r[c.ColumnIndex] = c.DateCellValue.ToString("yyyy/MM/dd");
                                }
                                else
                                {
                                    r[c.ColumnIndex] = (Decimal)c.NumericCellValue;
                                }
                            //如果是字串型 就要取 StringCellValue  這屬性的值
                            else if(c.CellType == CellType.String)
                                r[c.ColumnIndex] = c.StringCellValue;

                        }
                        dt.Rows.Add(r);
                    }
                }
            }
            //回傳DataTable
            return dt;
        }
    }
}