using WaterCaseTracking.Models.ViewModels.ParameterSettings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace WaterCaseTracking.Service
{
    public class ParameterSettingsService
    {
        #region 取得設定參數
        public string GetFileJson(string filepath)
        {
            string json = string.Empty;
            using (FileStream fs = new FileStream(filepath, FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("UTF-8")))
                {
                    json = sr.ReadToEnd().ToString();
                }
            }
            return json;
        }
        #endregion

        #region 設定參數
        public string SetFileJson(SearchInfoViewModel data, string filepath)
        {
            string json = string.Empty;
            using (StringWriter sw = new StringWriter())
            {
                using (JsonTextWriter writer = new JsonTextWriter(sw))
                {
                    #region 寫入系統參數檔案
                    writer.WriteStartObject();//建立物件

                    #region 設定Sql參數

                    writer.WritePropertyName("Sql");//物件名稱
                    writer.WriteStartArray();//建立陣列
                    //---------------------------------------------
                    writer.WriteStartObject();//建立物件
                    writer.WritePropertyName(String.Format("Address", data.SqlAddress));//設定屬性名稱
                    writer.WriteValue(data.SqlAddress);//設定值

                    writer.WritePropertyName(String.Format("Account", data.SqlAccount));//設定屬性名稱
                    writer.WriteValue(data.SqlAccount);//設定值

                    writer.WritePropertyName(String.Format("Password", data.SqlPassword));//設定屬性名稱
                    writer.WriteValue(data.SqlPassword);//設定值
                    writer.WriteEndObject();
                    //---------------------------------------------
                    writer.WriteEndArray();

                    #endregion

                    #region 設定Ftp參數

                    writer.WritePropertyName("Ftp");//物件名稱
                    writer.WriteStartArray();//建立陣列
                    //---------------------------------------------
                    writer.WriteStartObject();//建立物件
                    writer.WritePropertyName(String.Format("Address", data.FtpAddress));//設定屬性名稱
                    writer.WriteValue(data.FtpAddress);//設定值

                    writer.WritePropertyName(String.Format("Account", data.FtpAccount));//設定屬性名稱
                    writer.WriteValue(data.FtpAccount);//設定值

                    writer.WritePropertyName(String.Format("Password", data.FtpPassword));//設定屬性名稱
                    writer.WriteValue(data.FtpPassword);//設定值
                    writer.WriteEndObject();
                    //---------------------------------------------
                    writer.WriteEndArray();

                    #endregion

                    #region 設定SSO參數

                    writer.WritePropertyName("SSO");//物件名稱
                    writer.WriteStartArray();//建立陣列
                    //---------------------------------------------
                    writer.WriteStartObject();//建立物件
                    writer.WritePropertyName(String.Format("Address", data.SSOAddress));//設定屬性名稱
                    writer.WriteValue(data.SSOAddress);//設定值

                    writer.WritePropertyName(String.Format("Account", data.SSOAccount));//設定屬性名稱
                    writer.WriteValue(data.SSOAccount);//設定值

                    writer.WritePropertyName(String.Format("Password", data.SSOPassword));//設定屬性名稱
                    writer.WriteValue(data.SSOPassword);//設定值
                    writer.WriteEndObject();
                    //---------------------------------------------
                    writer.WriteEndArray();

                    #endregion

                    #region 設定Mail參數

                    writer.WritePropertyName("Mail");//物件名稱
                    writer.WriteStartArray();//建立陣列
                    //---------------------------------------------
                    writer.WriteStartObject();//建立物件
                    writer.WritePropertyName(String.Format("Address", data.MailAddress));//設定屬性名稱
                    writer.WriteValue(data.MailAddress);//設定值

                    writer.WritePropertyName(String.Format("Account", data.MailAccount));//設定屬性名稱
                    writer.WriteValue(data.MailAccount);//設定值

                    writer.WritePropertyName(String.Format("Password", data.MailPassword));//設定屬性名稱
                    writer.WriteValue(data.MailPassword);//設定值
                    writer.WriteEndObject();
                    //---------------------------------------------
                    writer.WriteEndArray();

                    #endregion

                    writer.WriteEndObject();

                    writer.Flush();
                    writer.Close();
                    sw.Flush();
                    sw.Close();

                    //將文字寫入該檔案
                    File.WriteAllText(filepath, sw.ToString(), Encoding.UTF8);
                    json = "檔案寫入完成";
                    #endregion
                }
            }
            return json;
        }
        #endregion
    }
}