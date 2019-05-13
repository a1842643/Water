using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml;
using Ionic.Zip;
using System.Collections.Generic;

namespace OdsReadWrite
{
    internal sealed class OdsReaderWriter
    {
        private static string tmpods = "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SAzGtI4ICR6NQmn15HwaOc7iCWZXUXTroJB59yA9i906qaCyCmG2Ur2HtiCRgUCNCUzKhHSDHLpOS8UzlvNROcGh7eLHYL3Tg6I8YPArjs/Y3ogMpuVe4L2w7lyD33yVaHruY3p108Xx3yOUYJwy7k/quzt5/+f+Ls//GeKvtHZEbEDOo2f6kOe08h9VR69QSwcItPdo0gUBAACDAwAAUEsDBBQAAAgAADB3h074lc9/DAMAAAwDAAAIAAAAbWV0YS54bWw8P3htbCB2ZXJzaW9uPSIxLjAiIGVuY29kaW5nPSJVVEYtOCI/Pgo8b2ZmaWNlOmRvY3VtZW50LW1ldGEgeG1sbnM6b2ZmaWNlPSJ1cm46b2FzaXM6bmFtZXM6dGM6b3BlbmRvY3VtZW50OnhtbG5zOm9mZmljZToxLjAiIHhtbG5zOnhsaW5rPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5L3hsaW5rIiB4bWxuczpkYz0iaHR0cDovL3B1cmwub3JnL2RjL2VsZW1lbnRzLzEuMS8iIHhtbG5zOm1ldGE9InVybjpvYXNpczpuYW1lczp0YzpvcGVuZG9jdW1lbnQ6eG1sbnM6bWV0YToxLjAiIHhtbG5zOm9vbz0iaHR0cDovL29wZW5vZmZpY2Uub3JnLzIwMDQvb2ZmaWNlIiB4bWxuczpncmRkbD0iaHR0cDovL3d3dy53My5vcmcvMjAwMy9nL2RhdGEtdmlldyMiIG9mZmljZTp2ZXJzaW9uPSIxLjIiPjxvZmZpY2U6bWV0YT48bWV0YTpnZW5lcmF0b3I+T3Blbk9mZmljZS80LjEuNiRXaW4zMiBPcGVuT2ZmaWNlLm9yZ19wcm9qZWN0LzQxNm0xJEJ1aWxkLTk3OTA8L21ldGE6Z2VuZXJhdG9yPjxtZXRhOmNyZWF0aW9uLWRhdGU+MjAxOS0wNC0wN1QyMjo1NzoxMy40ODwvbWV0YTpjcmVhdGlvbi1kYXRlPjxtZXRhOmVkaXRpbmctZHVyYXRpb24+UDBEPC9tZXRhOmVkaXRpbmctZHVyYXRpb24+PG1ldGE6ZWRpdGluZy1jeWNsZXM+MTwvbWV0YTplZGl0aW5nLWN5Y2xlcz48bWV0YTpkb2N1bWVudC1zdGF0aXN0aWMgbWV0YTp0YWJsZS1jb3VudD0iMSIgbWV0YTpjZWxsLWNvdW50PSIwIiBtZXRhOm9iamVjdC1jb3VudD0iMCIvPjwvb2ZmaWNlOm1ldGE+PC9vZmZpY2U6ZG9jdW1lbnQtbWV0YT5QSwMEFAAICAgAMHeHTgAAAAAAAAAAAAAAABUAAABNRVRBLUlORi9tYW5pZmVzdC54bWytlMFuwyAMhu99iojrFNh6mlDTHibtCboHYMRJkcAgMFX79iOV0mZbOy1Vb9iy/++3Qaw2B2erPcRkPDbshT+zClD71mDfsI/te/3KNuvFyik0HSSS46EqfZjOYcNyROlVMkmicpAkaekDYOt1doAkv9fLE+kcTQws2XpRXXidsVCX/ni8VDtojarpGKBhKgRrtKLSLfbY8pMFPiXzFCKoNu0A6Bbyku6ytXVQtGuYYGKWlesqbx470+d4spiWQmkNFkroo9A5xsFh2c1M1s+xU8ZBhWfD9RT4P08z4capHkTA/rr6dpfdJypjk6DxyIfqeRSCA4lhM1chZUq6b3V/6yYgKm8/PV6YjhbukZ1edWy7p5uEMcdL1UOtOyA1Gl+JX1/B+gtQSwcISUgspiYBAABFBAAAUEsBAhQAFAAACAAAMHeHToVsOYouAAAALgAAAAgAAAAAAAAAAAAAAAAAAAAAAG1pbWV0eXBlUEsBAhQAFAAICAgAMHeHTgAAAAACAAAAAAAAACcAAAAAAAAAAAAAAAAAVAAAAENvbmZpZ3VyYXRpb25zMi9hY2NlbGVyYXRvci9jdXJyZW50LnhtbFBLAQIUABQAAAgAADB3h04AAAAAAAAAAAAAAAAfAAAAAAAAAAAAAAAAAKsAAABDb25maWd1cmF0aW9uczIvaW1hZ2VzL0JpdG1hcHMvUEsBAhQAFAAACAAAMHeHTgAAAAAAAAAAAAAAABgAAAAAAAAAAAAAAAAA6AAAAENvbmZpZ3VyYXRpb25zMi9mbG9hdGVyL1BLAQIUABQAAAgAADB3h04AAAAAAAAAAAAAAAAcAAAAAAAAAAAAAAAAAB4BAABDb25maWd1cmF0aW9uczIvcHJvZ3Jlc3NiYXIvUEsBAhQAFAAACAAAMHeHTgAAAAAAAAAAAAAAABgAAAAAAAAAAAAAAAAAWAEAAENvbmZpZ3VyYXRpb25zMi9tZW51YmFyL1BLAQIUABQAAAgAADB3h04AAAAAAAAAAAAAAAAaAAAAAAAAAAAAAAAAAI4BAABDb25maWd1cmF0aW9uczIvcG9wdXBtZW51L1BLAQIUABQAAAgAADB3h04AAAAAAAAAAAAAAAAaAAAAAAAAAAAAAAAAAMYBAABDb25maWd1cmF0aW9uczIvc3RhdHVzYmFyL1BLAQIUABQAAAgAADB3h04AAAAAAAAAAAAAAAAYAAAAAAAAAAAAAAAAAP4BAABDb25maWd1cmF0aW9uczIvdG9vbGJhci9QSwECFAAUAAAIAAAwd4dOAAAAAAAAAAAAAAAAGgAAAAAAAAAAAAAAAAA0AgAAQ29uZmlndXJhdGlvbnMyL3Rvb2xwYW5lbC9QSwECFAAUAAAIAAAwd4dOSqNxV/gCAAD4AgAAGAAAAAAAAAAAAAAAAABsAgAAVGh1bWJuYWlscy90aHVtYm5haWwucG5nUEsBAi0AFAAAAAgAU7iHTizWVlyzAwAASw0AAAsAJAAAAAAAAAAAAAAAmgUAAGNvbnRlbnQueG1sCgAgAAAAAAABABgAjkSu8VLt1AGORK7xUu3UAY5ErvFS7dQBUEsBAhQAFAAICAgAMHeHTjAKGoy3AwAA4BsAAAwAAAAAAAAAAAAAAAAAmgkAAHNldHRpbmdzLnhtbFBLAQIUABQACAgIADB3h066k1EFtQYAAI0eAAAKAAAAAAAAAAAAAAAAAIsNAABzdHlsZXMueG1sUEsBAhQAFAAICAgAMHeHTrT3aNIFAQAAgwMAAAwAAAAAAAAAAAAAAAAAeBQAAG1hbmlmZXN0LnJkZlBLAQIUABQAAAgAADB3h074lc9/DAMAAAwDAAAIAAAAAAAAAAAAAAAAALcVAABtZXRhLnhtbFBLAQIUABQACAgIADB3h05JSCymJgEAAEUEAAAVAAAAAAAAAAAAAAAAAOkYAABNRVRBLUlORi9tYW5pZmVzdC54bWxQSwUGAAAAABEAEQCUBAAAUhoAAAAA";
        // Namespaces. We need this to initialize XmlNamespaceManager so that we can search XmlDocument.
        private static string[,] namespaces = new string[,] 
        {
            {"table", "urn:oasis:names:tc:opendocument:xmlns:table:1.0"},
            {"office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0"},
            {"style", "urn:oasis:names:tc:opendocument:xmlns:style:1.0"},
            {"text", "urn:oasis:names:tc:opendocument:xmlns:text:1.0"},            
            {"draw", "urn:oasis:names:tc:opendocument:xmlns:drawing:1.0"},
            {"fo", "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0"},
            {"dc", "http://purl.org/dc/elements/1.1/"},
            {"meta", "urn:oasis:names:tc:opendocument:xmlns:meta:1.0"},
            {"number", "urn:oasis:names:tc:opendocument:xmlns:datastyle:1.0"},
            {"presentation", "urn:oasis:names:tc:opendocument:xmlns:presentation:1.0"},
            {"svg", "urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0"},
            {"chart", "urn:oasis:names:tc:opendocument:xmlns:chart:1.0"},
            {"dr3d", "urn:oasis:names:tc:opendocument:xmlns:dr3d:1.0"},
            {"math", "http://www.w3.org/1998/Math/MathML"},
            {"form", "urn:oasis:names:tc:opendocument:xmlns:form:1.0"},
            {"script", "urn:oasis:names:tc:opendocument:xmlns:script:1.0"},
            {"ooo", "http://openoffice.org/2004/office"},
            {"ooow", "http://openoffice.org/2004/writer"},
            {"oooc", "http://openoffice.org/2004/calc"},
            {"dom", "http://www.w3.org/2001/xml-events"},
            {"xforms", "http://www.w3.org/2002/xforms"},
            {"xsd", "http://www.w3.org/2001/XMLSchema"},
            {"xsi", "http://www.w3.org/2001/XMLSchema-instance"},
            {"rpt", "http://openoffice.org/2005/report"},
            {"of", "urn:oasis:names:tc:opendocument:xmlns:of:1.2"},
            {"rdfa", "http://docs.oasis-open.org/opendocument/meta/rdfa#"},
            {"config", "urn:oasis:names:tc:opendocument:xmlns:config:1.0"}
        };

        // Read zip stream (.ods file is zip file).
        private ZipFile GetZipFile(Stream stream)
        {
            return ZipFile.Read(stream);
        }

        // Read zip file (.ods file is zip file).
        private ZipFile GetZipFile(string inputFilePath)
        {
             
            return ZipFile.Read(inputFilePath);
        }

        private XmlDocument GetContentXmlFile(ZipFile zipFile)
        {
            // Get file(in zip archive) that contains data ("content.xml").
            ZipEntry contentZipEntry = zipFile["content.xml"];

            // Extract that file to MemoryStream.
            Stream contentStream = new MemoryStream();
            contentZipEntry.Extract(contentStream);
            contentStream.Seek(0, SeekOrigin.Begin);

            // Create XmlDocument from MemoryStream (MemoryStream contains content.xml).
            XmlDocument contentXml = new XmlDocument();
            contentXml.Load(contentStream);

            return contentXml;
        }

        private XmlNamespaceManager InitializeXmlNamespaceManager(XmlDocument xmlDocument)
        {
            XmlNamespaceManager nmsManager = new XmlNamespaceManager(xmlDocument.NameTable);

            for (int i = 0; i < namespaces.GetLength(0); i++)
                nmsManager.AddNamespace(namespaces[i, 0], namespaces[i, 1]);

            return nmsManager;
        }

        /// <summary>
        /// Read .ods file and store it in DataSet.
        /// </summary>
        /// <param name="inputFilePath">Path to the .ods file.</param>
        /// <returns>DataSet that represents .ods file.</returns>
        public DataSet ReadOdsFile(string inputFilePath)
        {
            ZipFile odsZipFile = this.GetZipFile(inputFilePath);

            // Get content.xml file
            XmlDocument contentXml = this.GetContentXmlFile(odsZipFile);

            // Initialize XmlNamespaceManager
            XmlNamespaceManager nmsManager = this.InitializeXmlNamespaceManager(contentXml);

            DataSet odsFile = new DataSet(Path.GetFileName(inputFilePath));

            foreach (XmlNode tableNode in this.GetTableNodes(contentXml, nmsManager))
                odsFile.Tables.Add(this.GetSheet(tableNode, nmsManager));

            return odsFile;
        }

        // In ODF sheet is stored in table:table node
        private XmlNodeList GetTableNodes(XmlDocument contentXmlDocument, XmlNamespaceManager nmsManager)
        {
            return contentXmlDocument.SelectNodes("/office:document-content/office:body/office:spreadsheet/table:table", nmsManager);
        }

        private DataTable GetSheet(XmlNode tableNode, XmlNamespaceManager nmsManager)
        {
            DataTable sheet = new DataTable(tableNode.Attributes["table:name"].Value);

            XmlNodeList rowNodes = tableNode.SelectNodes("table:table-row", nmsManager);

            int rowIndex = 0;
            foreach (XmlNode rowNode in rowNodes)
                this.GetRow(rowNode, sheet, nmsManager, ref rowIndex);

            return sheet;
        }

        private void GetRow(XmlNode rowNode, DataTable sheet, XmlNamespaceManager nmsManager, ref int rowIndex)
        {
            XmlAttribute rowsRepeated = rowNode.Attributes["table:number-rows-repeated"];
            if (rowsRepeated == null || Convert.ToInt32(rowsRepeated.Value, CultureInfo.InvariantCulture) == 1)
            {
                while (sheet.Rows.Count < rowIndex)
                    sheet.Rows.Add(sheet.NewRow());

                DataRow row = sheet.NewRow();

                XmlNodeList cellNodes = rowNode.SelectNodes("table:table-cell", nmsManager);

                int cellIndex = 0;
                foreach (XmlNode cellNode in cellNodes)
                    this.GetCell(cellNode, row, nmsManager, ref cellIndex);

                sheet.Rows.Add(row);

                rowIndex++;
            }
            else
            {
                rowIndex += Convert.ToInt32(rowsRepeated.Value, CultureInfo.InvariantCulture);
            }

            // sheet must have at least one cell
            if (sheet.Rows.Count == 0)
            {
                sheet.Rows.Add(sheet.NewRow());
                sheet.Columns.Add();
            }
        }

        private void GetCell(XmlNode cellNode, DataRow row, XmlNamespaceManager nmsManager, ref int cellIndex)
        {
            XmlAttribute cellRepeated = cellNode.Attributes["table:number-columns-repeated"];

            if (cellRepeated == null)
            {
                DataTable sheet = row.Table;

                while (sheet.Columns.Count <= cellIndex)
                    sheet.Columns.Add();

                row[cellIndex] = this.ReadCellValue(cellNode);

                cellIndex++;
            }
            else
            {
                cellIndex += Convert.ToInt32(cellRepeated.Value, CultureInfo.InvariantCulture);
            }
        }

        private string ReadCellValue(XmlNode cell)
        {
            XmlAttribute cellVal = cell.Attributes["office:value"];

            if (cellVal == null)
                return String.IsNullOrEmpty(cell.InnerText) ? null : cell.InnerText;
            else
                return cellVal.Value;
        }

        /// <summary>
        /// Writes DataSet as .ods file.
        /// </summary>
        /// <param name="odsFile">DataSet that represent .ods file.</param>
        /// <param name="outputFilePath">The name of the file to save to.</param>
        public MemoryStream OdsReport(List<string[]> LstData, string sourcePath)
        {
            using (ZipFile templateFile = this.GetZipFile(sourcePath))
            {
                XmlDocument contentXml = this.GetContentXmlFile(templateFile);
                XmlNamespaceManager nmsManager = this.InitializeXmlNamespaceManager(contentXml);

                XmlNodeList tableNodes = this.GetTableNodes(contentXml, nmsManager);
                XmlNode sheetsRootNode = this.GetSheetsRootNodeAndRemoveChildrens(contentXml, nmsManager);

               
                this.SaveContentXml(templateFile, contentXml);
                var ms = new MemoryStream();
                ms.Seek(0, SeekOrigin.Begin);
                templateFile.Save(ms);
                templateFile.Dispose();
                return ms;
            }
        
        }
        public MemoryStream OdsReport(List<string[]> LstData)
        {
            using (ZipFile templateFile = this.GetZipFile(new MemoryStream(Convert.FromBase64String(tmpods))))
            {
                XmlDocument contentXml = this.GetContentXmlFile(templateFile);
                XmlNamespaceManager nmsManager = this.InitializeXmlNamespaceManager(contentXml);

                XmlNodeList tableNodes = this.GetTableNodes(contentXml, nmsManager);
                XmlNode sheetsRootNode = this.GetSheetsRootNodeAndRemoveChildrens(contentXml, nmsManager);

               // this.SaveSheet(LstData, sheetsRootNode, tableNodes.Item(0), nmsManager);

                this.SaveContentXml(templateFile, contentXml);
                var ms = new MemoryStream();
                ms.Seek(0, SeekOrigin.Begin);
                templateFile.Save(ms);
                templateFile.Dispose();
                return ms;
            }

        }
        public MemoryStream OdsReport(DataTable LstData)
        {
            using (ZipFile templateFile = this.GetZipFile(new MemoryStream(Convert.FromBase64String(tmpods))))
            {
                XmlDocument contentXml = this.GetContentXmlFile(templateFile);
                XmlNamespaceManager nmsManager = this.InitializeXmlNamespaceManager(contentXml);

                XmlNodeList tableNodes = this.GetTableNodes(contentXml, nmsManager);
                XmlNode sheetsRootNode = this.GetSheetsRootNodeAndRemoveChildrens(contentXml, nmsManager);

                this.SaveSheet(LstData, sheetsRootNode, tableNodes.Item(0), nmsManager);

                this.SaveContentXml(templateFile, contentXml);
                var ms = new MemoryStream();
                ms.Seek(0, SeekOrigin.Begin);
                templateFile.Save(ms);
                templateFile.Dispose();
                return ms;
            }

        }
        private XmlNode GetSheetsRootNodeAndRemoveChildrens(XmlDocument contentXml, XmlNamespaceManager nmsManager)
        {
            XmlNodeList tableNodes = this.GetTableNodes(contentXml, nmsManager);

            XmlNode sheetsRootNode = tableNodes.Item(0).ParentNode;
            // remove sheets from template file
            //foreach (XmlNode tableNode in tableNodes)
            //    sheetsRootNode.RemoveChild(tableNode);

            return sheetsRootNode;
        }

        private void SaveSheet(DataTable dt, XmlNode sheetsRootNode, XmlNode tempSheet, XmlNamespaceManager nmsManager)
        {
            XmlDocument ownerDocument = sheetsRootNode.OwnerDocument;

            XmlNode sheetNode = tempSheet;

            this.SaveRows(dt, sheetNode, ownerDocument, nmsManager);

            //sheetsRootNode.AppendChild(sheetNode);
        }

        private void SaveColumnDefinition(DataTable sheet, XmlNode sheetNode, XmlDocument ownerDocument)
        {
            XmlNode columnDefinition = ownerDocument.CreateElement("table:table-column", this.GetNamespaceUri("table"));

            XmlAttribute columnsCount = ownerDocument.CreateAttribute("table:number-columns-repeated", this.GetNamespaceUri("table"));
            columnsCount.Value = sheet.Columns.Count.ToString(CultureInfo.InvariantCulture);
            columnDefinition.Attributes.Append(columnsCount);

            sheetNode.AppendChild(columnDefinition);
        }

        private void SaveRows(DataTable dt, XmlNode sheetNode, XmlDocument ownerDocument, XmlNamespaceManager nmsManager)
        {
            XmlNodeList rowNodes = sheetNode.SelectNodes("table:table-row", nmsManager);
            string[] aryHeader = new string[dt.Columns.Count];
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                aryHeader[i] = dt.Columns[i].ColumnName;
            }
            XmlNode rowNode = ownerDocument.CreateElement("table:table-row", this.GetNamespaceUri("table"));
            sheetNode.AppendChild(rowNode);
            this.SaveCell(aryHeader, rowNode, ownerDocument, nmsManager);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                rowNode = ownerDocument.CreateElement("table:table-row", this.GetNamespaceUri("table"));
                sheetNode.AppendChild(rowNode);
                this.SaveCell(dt.Rows[i].ItemArray, rowNode, ownerDocument, nmsManager);



            }
        }

        private void SaveCell(object[] row, XmlNode rowNode, XmlDocument ownerDocument, XmlNamespaceManager nmsManager)
        {
            XmlNodeList cellNodes = rowNode.SelectNodes("table:table-cell", nmsManager);
            object[] cells = row;

            for (int i = 0; i < cells.Length; i++)
            {
                XmlElement cellNode;
                cellNode = ownerDocument.CreateElement("table:table-cell", this.GetNamespaceUri("table"));
                rowNode.AppendChild(cellNode);


                XmlAttribute valueType2 = ownerDocument.CreateAttribute("table:style-name", this.GetNamespaceUri("table"));
                valueType2.Value = "ce2";
                cellNode.Attributes.Append(valueType2);
                if (row[i] != null)
                {
                    string ovalue = row[i] == null ? "" : row[i].ToString();
                    float fvalue=0;
                    XmlAttribute valueType = ownerDocument.CreateAttribute("office:value-type", this.GetNamespaceUri("office"));
                    if (float.TryParse(ovalue, out fvalue))
                    {
                        valueType.Value = "float";
                        cellNode.Attributes.Append(valueType);
                       
                    }
                    else
                    {
                        valueType.Value = "string";
                        cellNode.Attributes.Append(valueType);
                    }
                    XmlAttribute valueType3 = ownerDocument.CreateAttribute("office:value", this.GetNamespaceUri("office"));
                    valueType3.Value = ovalue;
                    cellNode.Attributes.Append(valueType3);
                    XmlElement cellValue = ownerDocument.CreateElement("text:p", this.GetNamespaceUri("text"));
                    cellValue.InnerText = ovalue;
                    cellNode.AppendChild(cellValue);
                }
                //<table:table-cell table:number-columns-repeated="16366"/>
                //if (i == cells.Length-1)
                //{
                 
                //     XmlElement cellNodeLast = ownerDocument.CreateElement("table:table-cell", this.GetNamespaceUri("table"));
                //     XmlAttribute valueTypeLast = ownerDocument.CreateAttribute("table:number-columns-repeated", this.GetNamespaceUri("table"));
                //     valueTypeLast.Value = "16366";
                //     cellNodeLast.Attributes.Append(valueTypeLast); 
                //     rowNode.AppendChild(cellNodeLast);
                //}



                
                
                   


            }
        }

        private void SaveContentXml(ZipFile templateFile, XmlDocument contentXml)
        {
            templateFile.RemoveEntry("content.xml");

            MemoryStream memStream = new MemoryStream();
            contentXml.Save(memStream);
            memStream.Seek(0, SeekOrigin.Begin);

            templateFile.AddEntry("content.xml", memStream);
        }

        private string GetNamespaceUri(string prefix)
        {
            for (int i = 0; i < namespaces.GetLength(0); i++)
            {
                if (namespaces[i, 0] == prefix)
                    return namespaces[i, 1];
            }

            throw new InvalidOperationException("Can't find that namespace URI");
        }
    }
}
