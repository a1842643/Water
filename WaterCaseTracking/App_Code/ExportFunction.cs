// Modified By      YYYY-MM-DD
// Jay             2019-01-25 - Creation

using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.SS.Converter;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.XWPF.UserModel;
using OdsReadWrite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

namespace WaterCaseTracking
{
    public class ExportFunction
    {
        static string fileName;
        static string functionName;
        static DataTable dt;
        static DateTime nowDateTime = DateTime.Now;
        //ServerIP
        static string ServerIP = ConfigurationManager.AppSettings["ServerIP"];
        static string folderPath = HttpContext.Current.Server.MapPath("~");
        //路徑(~/DataFile/yyyy/MM/dd/)
        static string filePath = ("/DataFile/"
            + nowDateTime.ToString("yyyyMMdd") + "/");
        /// <summary>
        /// 匯出DataTable
        /// </summary>
        /// <param name="fromDt">要轉報表的DataTable</param>
        /// <param name="fileExtension">副檔名</param>
        /// <param name="fromFunctionName">功能名稱</param>
        /// <returns></returns>
        public static string ExportDataTableTo(DataTable fromDt, string fileExtension, string fromFunctionName)
        {
            dt = fromDt;
            functionName = fromFunctionName;
            switch (fileExtension)
            {
                case "xlsx":
                    return DataTableToExcel(false);
                case "docx":
                    return DataTableToWord(false);
                case "ODF":
                    return DataTableToODF();
                case "PDF":
                    return DataTableToExcel(true);
                default:
                    return "error";
            }
        }

        #region DaTable轉Excel (起)
        /// <summary>
        /// DaTable轉Excel
        /// </summary>
        /// <param name="isToOds">是否轉Ods</param>
        /// <returns></returns>
        private static string DataTableToExcel(bool isToOds)
        {
            IWorkbook wb = new XSSFWorkbook();
            ISheet idpWorkSheet = wb.CreateSheet("Sheet1");//建立sheet

            //設定style
            XSSFCellStyle oStyle = (XSSFCellStyle)wb.CreateCellStyle();

            //設定上下左右的框線
            oStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            oStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            oStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            oStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            oStyle.DataFormat = NPOI.HSSF.UserModel.HSSFDataFormat.GetBuiltinFormat("@");
            oStyle.WrapText = true;

            //DataTable轉
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //建立標題列
                if (i == 0)
                {
                    //建立標題列
                    idpWorkSheet.CreateRow(i);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        //設定標題名稱
                        idpWorkSheet.GetRow(i).CreateCell(j).SetCellValue(dt.Columns[j].ToString());
                        //設定欄位Style
                        idpWorkSheet.GetRow(i).GetCell(j).CellStyle = oStyle;
                        idpWorkSheet.GetRow(i).GetCell(j).CellStyle.WrapText = true;
                    }
                }
                //建立內容
                int iRow = i + 1;
                idpWorkSheet.CreateRow(iRow);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    //設定內容值
                    idpWorkSheet.GetRow(iRow).CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
                    //設定欄位Style
                    idpWorkSheet.GetRow(iRow).GetCell(j).CellStyle = oStyle;
                }
            }
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                //自動調整欄寬
                idpWorkSheet.AutoSizeColumn(j);
            }
            //建立路徑
            CreatefilePath();

            //建立檔案(檔名:yyyyMMddHHmmss+functionName)
            fileName = folderPath + filePath + nowDateTime.ToString("yyyyMMddHHmmss") + functionName;
            FileStream fsXlsx = new FileStream(fileName + ".xlsx", FileMode.Create);
            wb.Write(fsXlsx);
            if (isToOds)
            {
                ExcelToPDF(wb, fileName + ".pdf");
                return ServerIP + fileName.Replace(folderPath, "") + ".pdf";
            }
            else
            {
                return ServerIP + fileName.Replace(folderPath, "") + ".xlsx";
            }
        }
        #endregion DaTable轉Excel (迄)

        #region DaTable轉Word (起)
        /// <summary>
        /// DaTable轉Word
        /// </summary>
        /// <param name="isToOdt">是否轉Odt</param>
        /// <returns></returns>
        private static string DataTableToWord(bool isToOdt)
        {
            //創建文檔
            XWPFDocument m_Docx = new XWPFDocument();

            CT_SectPr m_SectPr = new CT_SectPr();

            //頁面設置A4橫向
            m_SectPr.pgSz.w = (ulong)16838;
            m_SectPr.pgSz.h = (ulong)11906;
            m_Docx.Document.body.sectPr = m_SectPr;

            //建Table
            XWPFTable table = m_Docx.CreateTable(dt.Rows.Count + 1, dt.Columns.Count);

            //DataTable轉
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //建立標題列
                if (i == 0)
                {
                    //建立標題列
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        //設定標題名稱
                        table.GetRow(i).GetCell(j).SetText(dt.Columns[j].ToString());
                    }
                }
                //建立內容
                int iRow = i + 1;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    //設定內容值
                    table.GetRow(iRow).GetCell(j).SetText(dt.Rows[i][j].ToString());
                }
            }
            //建立路徑
            CreatefilePath();

            //建立檔案(檔名:yyyyMMddHHmmss+functionName)
            fileName = folderPath + filePath + nowDateTime.ToString("yyyyMMddHHmmss") + functionName;
            FileStream fsXlsx = new FileStream(fileName + ".docx", FileMode.Create);
            m_Docx.Write(fsXlsx);
            if (isToOdt)
            {
                WordToODT(fileName + ".docx", fileName + ".odt");
                return ServerIP + fileName.Replace(folderPath, "") + ".odt";
            }
            else
            {
                return ServerIP + fileName.Replace(folderPath, "") + ".docx";
            }
        }
        #endregion DaTable轉Word (迄)

        #region DaTable轉ODF (起)
        /// <summary>
        /// DaTable轉ODF
        /// </summary>
        /// <returns></returns>
        private static string DataTableToODF()
        {
            //建立路徑
            CreatefilePath();

            //建立檔案(檔名:yyyyMMddHHmmss+functionName)
            fileName = folderPath + filePath + nowDateTime.ToString("yyyyMMddHHmmss") + functionName;

            MemoryStream msOds = new OdsReaderWriter().OdsReport(dt);
            FileStream fsOds = new FileStream(fileName + ".ods", FileMode.Create);
            msOds.Position = 0;
            msOds.WriteTo(fsOds);

            return ServerIP + fileName.Replace(folderPath, "") + ".ods";

        }
        #endregion DaTable轉ODF (迄)

        #region DaTable轉PDF (起)
        /// <summary>
        /// Excel轉PDF
        /// </summary>
        /// <returns></returns>
        private static void ExcelToPDF(IWorkbook workbook, string TargetPath)
        {
            //建立路徑
            CreatefilePath();

            ExcelToHtmlConverter excelToHtmlConverter = new ExcelToHtmlConverter();

            // 设置输出参数
            excelToHtmlConverter.OutputColumnHeaders = true;
            excelToHtmlConverter.OutputHiddenColumns = false;
            excelToHtmlConverter.OutputHiddenRows = false;
            excelToHtmlConverter.OutputLeadingSpacesAsNonBreaking = true;
            excelToHtmlConverter.OutputRowNumbers = true;
            excelToHtmlConverter.UseDivsToSpan = true;

            // 处理的Excel文件
            excelToHtmlConverter.ProcessWorkbook(workbook);
            //轉出來的HTML
            string xlsHtml = excelToHtmlConverter.Document.OuterXml;

            //移除沒用的部分
            if(xlsHtml.IndexOf("<style type=\"text/css\">") != -1)
                xlsHtml = xlsHtml.Remove(xlsHtml.IndexOf("<style type=\"text/css\">") + 24, xlsHtml.IndexOf("</style>") - xlsHtml.IndexOf("<style type=\"text/css\">") - 24 );

            TextReader tr = new StringReader(xlsHtml);

            iTextSharp.text.Document doc = new iTextSharp.text.Document(PageSize.A4, 25, 25, 25, 25);//             '直式 'L, R, T, B 橫式:PageSize.A4.Rotate()
            HTMLWorker hw = new HTMLWorker(doc);
            PdfWriter.GetInstance(doc, new FileStream(TargetPath, System.IO.FileMode.Create));

            FontFactory.Register(Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\..\\Fonts\\mingliu.ttc");//      '新細明體

            StyleSheet Style = new StyleSheet();
            Style.LoadTagStyle("body", "face", "SIMHEI");
            Style.LoadTagStyle("body", "encoding", "Identity-H");
            Style.LoadTagStyle("body", "leading", "10,0");

            Style.LoadTagStyle("td", "face", "mingliu");
            Style.LoadTagStyle("td", "encoding", "Identity-H");
            Style.LoadTagStyle("td", "size", "10pt");
            Style.LoadTagStyle("td", "leading", "10,0");

            //'Style.LoadStyle("TableWithBorders", "style", "outset")
            //'Style.LoadStyle("TableWithBorders", "border", "1")
            //'Style.LoadStyle("CellBorderOnLeft", "border", "1")
            //'Style.LoadStyle("CellBorderOnLeft", "style", "solid")
            //'Style.LoadStyle("CellBorderOnLeft", "valign", "top")
            //'Style.LoadStyle("CellBorderOnLeft", "align", "left")
            //'Style.LoadStyle("CellBorderOn", "width", "1")
            //'Style.LoadStyle("CellBorderOn", "valign", "top")
            //'Style.LoadStyle("CellBorderOn", "align", "center")
            doc.Open();

            int i = 0;
            List<IElement> htmlElement = HTMLWorker.ParseToList(tr, Style); //'讀取字串的型式TextReader　
            for (i = 0; i <= (htmlElement.Count - 1); i++)
                doc.Add(htmlElement[i]);

            //    'doc.NewPage()   '換頁
            //'tr = New StringReader(msg1)
            //'htmlElement = HTMLWorker.ParseToList(tr, Style)
            //'For i = 0 To (htmlElement.Count - 1)
            //'    doc.Add(htmlElement(i))
            //'Next
            doc.Close();
            tr.Close();



        }
        #endregion DaTable轉ODF (迄)

        #region Excel轉ODS (起)
        /// <summary>
        /// Excel轉ODS
        /// </summary>
        /// <param name="FromPath"></param>
        /// <param name="TargetPath"></param>
        private static void ExcelToODS(string FromPath, string TargetPath)
        {
            var ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook book = ExcelApp.Workbooks.Open(FromPath);
            try
            {
                book.SaveAs(TargetPath, Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenDocumentSpreadsheet);
                ExcelApp.Visible = false;
                book.Close();
                ExcelApp.Quit();
            }
            catch (Exception ex)
            {
                book.Close();
                ExcelApp.Quit();
                throw ex;
            }
        }
        #endregion Excel轉ODS (迄)

        #region Word轉ODT (起)
        /// <summary>
        /// Word轉ODT
        /// </summary>
        /// <param name="FromPath"></param>
        /// <param name="TargetPath"></param>
        private static void WordToODT(string FromPath, string TargetPath)
        {
            var WordApp = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document doc = WordApp.Documents.Open(FromPath);
            try
            {
                doc.SaveAs2000(TargetPath, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatOpenDocumentText);
                WordApp.Visible = false;
                WordApp.Quit();

            }
            catch (Exception ex)
            {
                WordApp.Quit();
                throw ex;
            }
        }
        #endregion Word轉ODT (迄)

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

        #region 取得副檔名 (起)
        public static string getFileContentType(string fileExtension)
        {
            switch (fileExtension)
            {
                case "xlsx":
                    return "application/xlsx";
                case "docx":
                    return "application/docx";
                case "ods":
                    return "application/vnd.oasis.opendocument.spreadsheet";
                case "odt":
                    return "application/vnd.oasis.opendocument.text";
                default:
                    return "error";
            }
        }
        #endregion 取得副檔名 (迄)
    }
}