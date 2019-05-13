using ADO;
using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.XWPF.UserModel;
using NPOI.SS.UserModel.Charts;
using NPOI.SS.Util;
using NPOI.HSSF.UserModel;
using WaterCaseTracking.Models.ViewModels.Customreport;
using WaterCaseTracking.Models.ViewModels;
using WaterCaseTracking.Dao;
using ExportViewModel = WaterCaseTracking.Models.ViewModels.CharTable.ExportViewModel;

namespace WaterCaseTracking.Service
{
    public class CharTableService
    {
        static string fileName;
        static string functionName;
        static DateTime nowDateTime = DateTime.Now;
        //ServerIP
        static string ServerIP = ConfigurationManager.AppSettings["ServerIP"];
        static string folderPath = HttpContext.Current.Server.MapPath("~");
        //路徑(~/DataFile/yyyy/MM/dd/)
        static string filePath = ("/DataFile/"
            + nowDateTime.ToString("yyyyMMdd") + "/");
        internal DropDownListViewModel getddlUntil()
        {
            #region 參數宣告				
            UntilDao untilDao = new UntilDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlUntil = untilDao.getddlUntil(); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ddlUntil;
            #endregion
        }
        internal DropDownListViewModel getddlUntil(string Area)
        {
            #region 參數宣告				
            UntilDao untilDao = new UntilDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlUntil = untilDao.GetddlUntil(Area); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ddlUntil;
            #endregion
        }
        internal DropDownListViewModel getddlArea()
        {
            #region 參數宣告				
            UntilDao untilDao = new UntilDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlArea = untilDao.getddlARE(); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ddlArea;
            #endregion
        }
        internal DropDownListViewModel getdllYM()
        {
            #region 參數宣告				
            CustomreportDao CustomreportDao = new CustomreportDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlYM = CustomreportDao.getddlYM(); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ddlYM;
            #endregion
        }
        internal DropDownListViewModel getdllColums()
        {
            #region 參數宣告				
            CustomreportDao CustomreportDao = new CustomreportDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlColumns = CustomreportDao.getddFileColumns(); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ddlColumns;
            #endregion
        }
        internal DropDownListViewModel getdllColums(string Group,string Class)
        {
            #region 參數宣告				
            CustomreportDao CustomreportDao = new CustomreportDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlColumns = CustomreportDao.getddFileColumns( Group,Class); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ddlColumns;
            #endregion
        }
        internal DropDownListViewModel getdllUPColums( string Class)
        {
            #region 參數宣告				
            CustomreportDao CustomreportDao = new CustomreportDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlColumns = CustomreportDao.getddUPFileColumns(Class); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ddlColumns;
            #endregion
        }
        internal DataTable getExportData(ExportViewModel Search)
        {
            #region 參數宣告				
          
            DataTable dt = new DataTable();
            CharTableDao CharTableDao = new CharTableDao();
            #endregion

            #region 流程		
            dt = CharTableDao.getCustomreport(Search.ddlUntil, Search.ddlSYM, Search.ddlEYM, Search.FileColumns);
            return dt;

            #endregion
        }
        #region DaTable轉Excel (起)
        /// <summary>
        /// DaTable轉Excel
        /// </summary>
        /// <param name="isToOds">是否轉Ods</param>
        /// <returns></returns>
        /// 
        internal string getExportChart(string path, int rows)

        {
            try
            {

                FileStream RfileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                //建立讀取資料的FileStream
                XSSFWorkbook wb = new XSSFWorkbook(RfileStream);
                //讀取檔案內的Workbook物件
                ISheet Wsheet = wb.GetSheetAt(1);
                //選擇圖表存放的sheet
                ISheet Rsheet = wb.GetSheetAt(0);
                //選擇資料來源的sheet
                IDrawing drawing = Wsheet.CreateDrawingPatriarch();
                //sheet產生drawing物件
                IClientAnchor clientAnchor = drawing.CreateAnchor(0, 0, 0, 0, 0, 0, 20, 30);
                //設定圖表位置
                IChart chart = drawing.CreateChart(clientAnchor);
                //產生chart物件
                IChartLegend legend = chart.GetOrCreateLegend();
                //還沒研究出這行在做甚麼
                legend.Position = LegendPosition.TopRight;
                ILineChartData<double, double> data = chart.ChartDataFactory.CreateLineChartData<double, double>();
                //產生存放資料的物件(資料型態為double)
                IChartAxis bottomAxis = chart.ChartAxisFactory.CreateCategoryAxis(AxisPosition.Bottom);
                //設定X軸
                IValueAxis leftAxis = chart.ChartAxisFactory.CreateValueAxis(AxisPosition.Left);
                //設定Y軸

                bottomAxis.Crosses = AxisCrosses.AutoZero;
                //設定X軸數值開始為0
                leftAxis.Crosses = AxisCrosses.AutoZero;
                //設定Y軸數值開始為0
                IChartDataSource<double> xs = DataSources.FromNumericCellRange(Rsheet, new CellRangeAddress(0, rows, 0, 0));
                //取得要讀取sheet的資料位置(CellRangeAddress(first_row,end_row, first_column, end_column))
                //x軸資料
                IChartDataSource<double> ys1 = DataSources.FromNumericCellRange(Rsheet, new CellRangeAddress(0, rows, 1, 1));
                //第一條y軸資料


                //第二條y軸資料
                data.AddSeries(xs, ys1);

                //加入到data
                chart.Plot(data, bottomAxis, leftAxis);
                //加入到chart
                FileStream WfileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
                //建立寫入資料的FileStream
                wb.Write(WfileStream);
                //將workbook寫入資料
                RfileStream.Close();
                //關閉FileStream
                WfileStream.Close();
                //關閉FileStream

            }
            catch (Exception ex)
            {
                path = ex.Message;
            }
            return ServerIP + path.Replace(folderPath, "");
        }

        internal string DataTableToExcel(DataTable fromDt, string fromFunctionName)
        {
            IWorkbook wb = new XSSFWorkbook();
            ISheet idpWorkSheet = wb.CreateSheet("Sheet1");//建立sheet
            ISheet idpWorkSheet1 = wb.CreateSheet("Sheet2");//建立sheet
            //設定style
            XSSFCellStyle oStyle = (XSSFCellStyle)wb.CreateCellStyle();

            //設定上下左右的框線
            oStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            oStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            oStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            oStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

            //DataTable轉
            for (int i = 0; i < fromDt.Rows.Count; i++)
            {
                //建立標題列
                if (i == 0)
                {
                    //建立標題列
                    idpWorkSheet.CreateRow(i);
                    for (int j = 0; j < fromDt.Columns.Count; j++)
                    {
                        //設定標題名稱
                        idpWorkSheet.GetRow(i).CreateCell(j).SetCellValue(fromDt.Columns[j].ToString());
                        //設定欄位Style
                        idpWorkSheet.GetRow(i).GetCell(j).CellStyle = oStyle;
                    }
                }
                //建立內容
                int iRow = i + 1;
                idpWorkSheet.CreateRow(iRow);
                for (int j = 0; j < fromDt.Columns.Count; j++)
                {
                    if (j == 0)
                    {
                        idpWorkSheet.GetRow(iRow).CreateCell(j).SetCellValue(fromDt.Rows[i][j].ToString());
                        //設定欄位Style
                        //oStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0");
                        //idpWorkSheet.GetRow(iRow).GetCell(j).CellStyle = oStyle;
                    }
                    else
                    {
                        //設定內容值
                        idpWorkSheet.GetRow(iRow).CreateCell(j).SetCellValue(Convert.ToInt64(fromDt.Rows[i][j]));
                        //設定欄位Style
                        oStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0");
                        idpWorkSheet.GetRow(iRow).GetCell(j).CellStyle = oStyle;
                    }
                }
            }
            //建立路徑
            CreatefilePath();

            //建立檔案(檔名:yyyyMMddHHmmss+functionName)
            fileName = folderPath + filePath + nowDateTime.ToString("yyyyMMddHHmmss") + functionName;
            FileStream fsXlsx = new FileStream(fileName + ".xlsx", FileMode.Create);
            wb.Write(fsXlsx);

            return fileName + ".xlsx";

        }
        #endregion DaTable轉Excel (迄)
        #region 產生路徑 (起)
        private static void CreatefilePath()
        {
            if (!Directory.Exists(folderPath + filePath))
            {
                //新增資料夾
                Directory.CreateDirectory(folderPath + filePath);
            }
        }
        #endregion 產生路徑 (迄)

    }
}