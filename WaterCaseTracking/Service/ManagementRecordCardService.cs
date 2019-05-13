using WaterCaseTracking.Models.ViewModels.ManagementRecordCard;
using WaterCaseTracking.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;

namespace WaterCaseTracking.Service
{
    public class ManagementRecordCardService
    {
        public string ReportList(SearchInfoViewModel SearchInfo)
        {
            #region 宣告
            string HtmlStr = "";
            List<SearchItemViewModel> result = new List<SearchItemViewModel>();
            ManagementRecordCardDao ManagementRecordCardDao = new ManagementRecordCardDao();
            #endregion

            #region 流程
            if (SearchInfo.Report == "Month")
            {
                //月平均 Month
                result = ManagementRecordCardDao.MonthList(SearchInfo).data;
                string BranchName = ManagementRecordCardDao.BranchName(SearchInfo);
                HtmlStr = MonthHtmlData(result, BranchName, SearchInfo);
            }
            else
            {
                //累計平均 Accumulative
                result = ManagementRecordCardDao.AccumulativeList(SearchInfo).data;
                string BranchName = ManagementRecordCardDao.BranchName(SearchInfo);
                HtmlStr = AccumulativeHtmlData(result, BranchName, SearchInfo);
            }
            #endregion

            return HtmlStr;
        }

        #region 月平均報表
        public string MonthHtmlData(List<SearchItemViewModel> result, string BranchName, SearchInfoViewModel SearchInfo)
        {
            string str = "";
            DateTime strdate;
            string YM1,YM2,YM3,YM4,YM5,YM6 = "";
            string YM;
            YM = (int.Parse(SearchInfo.YeartMonth) + 191100).ToString();
            DateTime YeartMonth = DateTime.Parse(YM.Substring(0, 4) + "-" + YM.Substring(4, 2) + "-01");
            YM1 = SearchInfo.YeartMonth.Substring(0, SearchInfo.YeartMonth.Length - 2)+ "年" + YM.Substring( YM.Length - 2,2) + "月";
            YM2 = (int.Parse(SearchInfo.YeartMonth) + 191100).ToString()  + "01";
            strdate = DateTime.ParseExact(YM2, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
            YM2 = (int.Parse(strdate.AddMonths(-1).ToString("yyyyMM")) -191100).ToString(); 
            YM2= YM2.Substring(0, YM2.Length - 2) + "年" + YM2.Substring(YM2.Length - 2, 2) + "月";
            YM3 = (int.Parse(SearchInfo.YeartMonth) + 191100).ToString() + "01";
            strdate = DateTime.ParseExact(YM3, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
            YM3 = (int.Parse(strdate.AddMonths(-2).ToString("yyyyMM")) - 191100).ToString();
            YM3 = YM3.Substring(0, YM3.Length - 2) + "年" + YM3.Substring(YM3.Length - 2, 2) + "月";
            YM4 = (int.Parse(SearchInfo.YeartMonth) + 191100).ToString() + "01";
            strdate = DateTime.ParseExact(YM4, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
            YM4 = (int.Parse(strdate.AddYears(-1).ToString("yyyyMM")) - 191100).ToString();
            YM4 = YM4.Substring(0, YM4.Length - 2) + "年12月";
            YM5 = (int.Parse(SearchInfo.YeartMonth) + 191100).ToString() + "01";
            strdate = DateTime.ParseExact(YM5, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
            YM5 = (int.Parse(strdate.AddYears(-2).ToString("yyyyMM")) - 191100).ToString();
            YM5 = YM5.Substring(0, YM5.Length - 2) + "年12月";
            YM6 = (int.Parse(SearchInfo.YeartMonth) + 191100).ToString() + "01";
            strdate = DateTime.ParseExact(YM6, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
            YM6 = (int.Parse(strdate.AddYears(-3).ToString("yyyyMM")) - 191100).ToString();
            YM6 = YM6.Substring(0, YM6.Length - 2) + "年12月";
            #region 報表開始
            //表頭
            str = "<html><body>" +
                "<table border=0 width=90% align=center cellpadding=0 cellspacing=0>" +
                "<tr align='center'>" +
                "<td colspan=8>" +
                "<font size='5'>" + BranchName + "&nbsp&nbsp&nbsp&nbsp經營管理記錄卡(月平均餘額)</font>" +
                "</td>" +
                "</tr>" +
                "<tr align='center'> " +
                "<td colspan=6>" +
                "<font size='4'>單位代號：" + SearchInfo.Branch_No + "</font>" +
                "</td>" +
                "<td nowrap>單位：千(美)元 </td>" +
                "</tr>" +
                "</table>" +
                "<br>";


            //報表內容
            str += "<table border=1 width=90% align=center cellpadding=0 cellspacing=0>" +
                "<tr>" +
                "<td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td>" +
                "</tr>" +
                "<tr>" +
                "<td colspan=2 align='center'>項目</td>" +
                "<td align='center'>" + YM1 + "</td>" +
                "<td align='center'>" + YM2+ "</td>" +
                "<td align='center'>" + YM3 + "</td>" +
                "<td align='center'>" + YM4 + "</td>" +
                "<td align='center'>" + YM5 + "</td>" +
                "<td align='center'>" + YM6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td rowspan=16 align='center'>存<br>&nbsp;<br>款</td>" +
                "<td nowrap align='left'>總存款</td>" +
                "<td align='right'>" + EIFunc.EI_Format(Convert.ToDecimal( result[0].A1))  + "</td>" +
                "<td align='right'>" + result[0].A2 + "</td>" +
                "<td align='right'>" + result[0].A3 + "</td>" +
                "<td align='right'>" + result[0].A4 + "</td>" +
                "<td align='right'>" + result[0].A5 + "</td>" +
                "<td align='right'>" + result[0].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='right'>一般性-總存款</td>" +
                "<td align='right'>" + result[1].A1 + "</td>" +
                "<td align='right'>" + result[1].A2 + "</td>" +
                "<td align='right'>" + result[1].A3 + "</td>" +
                "<td align='right'>" + result[1].A4 + "</td>" +
                "<td align='right'>" + result[1].A5 + "</td>" +
                "<td align='right'>" + result[1].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='left'>活期性存款 </td>" +
                "<td align='right'>" + result[2].A1 + "</td>" +
                "<td align='right'>" + result[2].A2 + "</td>" +
                "<td align='right'>" + result[2].A3 + "</td>" +
                "<td align='right'>" + result[2].A4 + "</td>" +
                "<td align='right'>" + result[2].A5 + "</td>" +
                "<td align='right'>" + result[2].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='right'>一般性-活存</td>" +
                "<td align='right'>" + result[3].A1 + "</td>" +
                "<td align='right'>" + result[3].A2 + "</td>" +
                "<td align='right'>" + result[3].A3 + "</td>" +
                "<td align='right'>" + result[3].A4 + "</td>" +
                "<td align='right'>" + result[3].A5 + "</td>" +
                "<td align='right'>" + result[3].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='right'>公庫活期 </td>" +
                "<td align='right'>" + result[4].A1 + "</td>" +
                "<td align='right'>" + result[4].A2 + "</td>" +
                "<td align='right'>" + result[4].A3 + "</td>" +
                "<td align='right'>" + result[4].A4 + "</td>" +
                "<td align='right'>" + result[4].A5 + "</td>" +
                "<td align='right'>" + result[4].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='right'>支存(含本支) </td>" +
                "<td align='right'>" + result[5].A1 + "</td>" +
                "<td align='right'>" + result[5].A2 + "</td>" +
                "<td align='right'>" + result[5].A3 + "</td>" +
                "<td align='right'>" + result[5].A4 + "</td>" +
                "<td align='right'>" + result[5].A5 + "</td>" +
                "<td align='right'>" + result[5].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='right'>活期存款 </td>" +
                "<td align='right'>" + result[6].A1 + "</td>" +
                "<td align='right'>" + result[6].A2 + "</td>" +
                "<td align='right'>" + result[6].A3 + "</td>" +
                "<td align='right'>" + result[6].A4 + "</td>" +
                "<td align='right'>" + result[6].A5 + "</td>" +
                "<td align='right'>" + result[6].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='right'>活存-基金</td>" +
                "<td align='right'>" + result[7].A1 + "</td>" +
                "<td align='right'>" + result[7].A2 + "</td>" +
                "<td align='right'>" + result[7].A3 + "</td>" +
                "<td align='right'>" + result[7].A4 + "</td>" +
                "<td align='right'>" + result[7].A5 + "</td>" +
                "<td align='right'>" + result[7].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='right'>活存-補償金</td>" +
                "<td align='right'>" + result[8].A1 + "</td>" +
                "<td align='right'>" + result[8].A2 + "</td>" +
                "<td align='right'>" + result[8].A3 + "</td>" +
                "<td align='right'>" + result[8].A4 + "</td>" +
                "<td align='right'>" + result[8].A5 + "</td>" +
                "<td align='right'>" + result[8].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='right'>活期儲蓄存款</td>" +
                "<td align='right'>" + result[9].A1 + "</td>" +
                "<td align='right'>" + result[9].A2 + "</td>" +
                "<td align='right'>" + result[9].A3 + "</td>" +
                "<td align='right'>" + result[9].A4 + "</td>" +
                "<td align='right'>" + result[9].A5 + "</td>" +
                "<td align='right'>" + result[9].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='right'>行員活儲存款</td>" +
                "<td align='right'>" + result[11].A1 + "</td>" +
                "<td align='right'>" + result[11].A2 + "</td>" +
                "<td align='right'>" + result[11].A3 + "</td>" +
                "<td align='right'>" + result[11].A4 + "</td>" +
                "<td align='right'>" + result[11].A5 + "</td>" +
                "<td align='right'>" + result[11].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='left'>定期性存款 </td>" +
                "<td align='right'>" + result[12].A1 + "</td>" +
                "<td align='right'>" + result[12].A2 + "</td>" +
                "<td align='right'>" + result[12].A3 + "</td>" +
                "<td align='right'>" + result[12].A4 + "</td>" +
                "<td align='right'>" + result[12].A5 + "</td>" +
                "<td align='right'>" + result[12].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='right'>定存(一般)</td>" +
                "<td align='right'>" + result[12].A1 + "</td>" +
                "<td align='right'>" + result[12].A2 + "</td>" +
                "<td align='right'>" + result[12].A3 + "</td>" +
                "<td align='right'>" + result[12].A4 + "</td>" +
                "<td align='right'>" + result[12].A5 + "</td>" +
                "<td align='right'>" + result[12].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='right'>定存(CD)</td>" +
                "<td align='right'>" + result[13].A1 + "</td>" +
                "<td align='right'>" + result[13].A2 + "</td>" +
                "<td align='right'>" + result[13].A3 + "</td>" +
                "<td align='right'>" + result[13].A4 + "</td>" +
                "<td align='right'>" + result[13].A5 + "</td>" +
                "<td align='right'>" + result[13].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='right'>定存(含行員)</td>" +
                "<td align='right'>" + result[14].A1 + "</td>" +
                "<td align='right'>" + result[14].A2 + "</td>" +
                "<td align='right'>" + result[14].A3 + "</td>" +
                "<td align='right'>" + result[14].A4 + "</td>" +
                "<td align='right'>" + result[14].A5 + "</td>" +
                "<td align='right'>" + result[14].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='left'>公庫存款</td>" +
                "<td align='right'>" + result[15].A1 + "</td>" +
                "<td align='right'>" + result[15].A2 + "</td>" +
                "<td align='right'>" + result[15].A3 + "</td>" +
                "<td align='right'>" + result[15].A4 + "</td>" +
                "<td align='right'>" + result[15].A5 + "</td>" +
                "<td align='right'>" + result[15].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td rowspan=4 align='center'>放<br>&nbsp;<br>款</td>" +
                "<td nowrap align='left'>自有資金放款</td>" +
                "<td align='right'>" + result[16].A1 + "</td>" +
                "<td align='right'>" + result[16].A2 + "</td>" +
                "<td align='right'>" + result[16].A3 + "</td>" +
                "<td align='right'>" + result[16].A4 + "</td>" +
                "<td align='right'>" + result[16].A5 + "</td>" +
                "<td align='right'>" + result[16].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='right'>專業放款</td>" +
                "<td align='right'>" + result[17].A1 + "</td>" +
                "<td align='right'>" + result[17].A2 + "</td>" +
                "<td align='right'>" + result[17].A3 + "</td>" +
                "<td align='right'>" + result[17].A4 + "</td>" +
                "<td align='right'>" + result[17].A5 + "</td>" +
                "<td align='right'>" + result[17].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='right'>一般放款</td>" +
                "<td align='right'>" + result[18].A1 + "</td>" +
                "<td align='right'>" + result[18].A2 + "</td>" +
                "<td align='right'>" + result[18].A3 + "</td>" +
                "<td align='right'>" + result[18].A4 + "</td>" +
                "<td align='right'>" + result[18].A5 + "</td>" +
                "<td align='right'>" + result[18].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='left'>應收代放款</td>" +
                "<td align='right'>" + result[19].A1 + "</td>" +
                "<td align='right'>" + result[19].A2 + "</td>" +
                "<td align='right'>" + result[19].A3 + "</td>" +
                "<td align='right'>" + result[19].A4 + "</td>" +
                "<td align='right'>" + result[19].A5 + "</td>" +
                "<td align='right'>" + result[19].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td align='center' colspan=2>保證款項</td>" +
                "<td align='right'>" + result[20].A1 + "</td>" +
                "<td align='right'>" + result[20].A2 + "</td>" +
                "<td align='right'>" + result[20].A3 + "</td>" +
                "<td align='right'>" + result[20].A4 + "</td>" +
                "<td align='right'>" + result[20].A5 + "</td>" +
                "<td align='right'>" + result[20].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td align='center' colspan=2>外匯承作金額</td>" +
                "<td align='right'>" + result[21].A1 + "</td>" +
                "<td align='right'>" + result[21].A2 + "</td>" +
                "<td align='right'>" + result[21].A3 + "</td>" +
                "<td align='right'>" + result[21].A4 + "</td>" +
                "<td align='right'>" + result[21].A5 + "</td>" +
                "<td align='right'>" + result[21].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td rowspan=2 align='center'>逾<br>&nbsp;<br>放</td>" +
                "<td nowrap align='left'>逾放總額</td>" +
                "<td align='right'>" + result[22].A1 + "</td>" +
                "<td align='right'>" + result[22].A2 + "</td>" +
                "<td align='right'>" + result[22].A3 + "</td>" +
                "<td align='right'>" + result[22].A4 + "</td>" +
                "<td align='right'>" + result[22].A5 + "</td>" +
                "<td align='right'>" + result[22].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='left'>逾放比率</td>" +
                "<td align='right'>" + result[23].A1 + "</td>" +
                "<td align='right'>" + result[23].A2 + "</td>" +
                "<td align='right'>" + result[23].A3 + "</td>" +
                "<td align='right'>" + result[23].A4 + "</td>" +
                "<td align='right'>" + result[23].A5 + "</td>" +
                "<td align='right'>" + result[23].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td rowspan=10 align='center'>損&nbsp;<br>益&nbsp;<br>狀&nbsp;<br>況&nbsp;</td>" +
                "<td nowrap align='left'>營業收入</td>" +
                "<td align='right'>" + result[24].A1 + "</td>" +
                "<td align='right'>" + result[24].A2 + "</td>" +
                "<td align='right'>" + result[24].A3 + "</td>" +
                "<td align='right'>" + result[24].A4 + "</td>" +
                "<td align='right'>" + result[24].A5 + "</td>" +
                "<td align='right'>" + result[24].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='right'>利息收入</td>" +
                "<td align='right'>" + result[25].A1 + "</td>" +
                "<td align='right'>" + result[25].A2 + "</td>" +
                "<td align='right'>" + result[25].A3 + "</td>" +
                "<td align='right'>" + result[25].A4 + "</td>" +
                "<td align='right'>" + result[25].A5 + "</td>" +
                "<td align='right'>" + result[25].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='right'>收入內部損益</td>" +
                "<td align='right'>" + result[26].A1 + "</td>" +
                "<td align='right'>" + result[26].A2 + "</td>" +
                "<td align='right'>" + result[26].A3 + "</td>" +
                "<td align='right'>" + result[26].A4 + "</td>" +
                "<td align='right'>" + result[26].A5 + "</td>" +
                "<td align='right'>" + result[26].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='right'>其他收入</td>" +
                "<td align='right'>" + result[27].A1 + "</td>" +
                "<td align='right'>" + result[27].A2 + "</td>" +
                "<td align='right'>" + result[27].A3 + "</td>" +
                "<td align='right'>" + result[27].A4 + "</td>" +
                "<td align='right'>" + result[27].A5 + "</td>" +
                "<td align='right'>" + result[27].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='left'>營業成本</td>" +
                "<td align='right'>" + result[28].A1 + "</td>" +
                "<td align='right'>" + result[28].A2 + "</td>" +
                "<td align='right'>" + result[28].A3 + "</td>" +
                "<td align='right'>" + result[28].A4 + "</td>" +
                "<td align='right'>" + result[28].A5 + "</td>" +
                "<td align='right'>" + result[28].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='right'>利息費用 </td>" +
                "<td align='right'>" + result[29].A1 + "</td>" +
                "<td align='right'>" + result[29].A2 + "</td>" +
                "<td align='right'>" + result[29].A3 + "</td>" +
                "<td align='right'>" + result[29].A4 + "</td>" +
                "<td align='right'>" + result[29].A5 + "</td>" +
                "<td align='right'>" + result[29].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='right'>成本內部損益</td>" +
                "<td align='right'>" + result[30].A1 + "</td>" +
                "<td align='right'>" + result[30].A2 + "</td>" +
                "<td align='right'>" + result[30].A3 + "</td>" +
                "<td align='right'>" + result[30].A4 + "</td>" +
                "<td align='right'>" + result[30].A5 + "</td>" +
                "<td align='right'>" + result[30].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='right'>其他費用</td>" +
                "<td align='right'>" + result[31].A1 + "</td>" +
                "<td align='right'>" + result[31].A2 + "</td>" +
                "<td align='right'>" + result[31].A3 + "</td>" +
                "<td align='right'>" + result[31].A4 + "</td>" +
                "<td align='right'>" + result[31].A5 + "</td>" +
                "<td align='right'>" + result[31].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='left'>提存前盈餘</td>" +
                "<td align='right'>" + result[32].A1 + "</td>" +
                "<td align='right'>" + result[32].A2 + "</td>" +
                "<td align='right'>" + result[32].A3 + "</td>" +
                "<td align='right'>" + result[32].A4 + "</td>" +
                "<td align='right'>" + result[32].A5 + "</td>" +
                "<td align='right'>" + result[32].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='right'>特殊性損益</td>" +
                "<td align='right'>" + result[33].A1 + "</td>" +
                "<td align='right'>" + result[33].A2 + "</td>" +
                "<td align='right'>" + result[33].A3 + "</td>" +
                "<td align='right'>" + result[33].A4 + "</td>" +
                "<td align='right'>" + result[33].A5 + "</td>" +
                "<td align='right'>" + result[33].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td rowspan=6 align='center'>經&nbsp;<br>營&nbsp;<br>指&nbsp;<br>標&nbsp;</td>" +
                "<td nowrap align='left'>存款平均利率</td>" +
                "<td align='right'>" + result[34].A1 + "</td>" +
                "<td align='right'>" + result[34].A2 + "</td>" +
                "<td align='right'>" + result[34].A3 + "</td>" +
                "<td align='right'>" + result[34].A4 + "</td>" +
                "<td align='right'>" + result[34].A5 + "</td>" +
                "<td align='right'>" + result[34].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='left'>放款平均利率</td>" +
                "<td align='right'>" + result[35].A1 + "</td>" +
                "<td align='right'>" + result[35].A2 + "</td>" +
                "<td align='right'>" + result[35].A3 + "</td>" +
                "<td align='right'>" + result[35].A4 + "</td>" +
                "<td align='right'>" + result[35].A5 + "</td>" +
                "<td align='right'>" + result[35].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='left'>放存利差</td>" +
                "<td align='right'>" + result[36].A1 + "</td>" +
                "<td align='right'>" + result[36].A2 + "</td>" +
                "<td align='right'>" + result[36].A3 + "</td>" +
                "<td align='right'>" + result[36].A4 + "</td>" +
                "<td align='right'>" + result[36].A5 + "</td>" +
                "<td align='right'>" + result[36].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='left'>活存比率</td>" +
                "<td align='right'>" + result[37].A1 + "</td>" +
                "<td align='right'>" + result[37].A2 + "</td>" +
                "<td align='right'>" + result[37].A3 + "</td>" +
                "<td align='right'>" + result[37].A4 + "</td>" +
                "<td align='right'>" + result[37].A5 + "</td>" +
                "<td align='right'>" + result[37].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='left'>一般存款比率</td>" +
                "<td align='right'>" + result[38].A1 + "</td>" +
                "<td align='right'>" + result[38].A2 + "</td>" +
                "<td align='right'>" + result[38].A3 + "</td>" +
                "<td align='right'>" + result[38].A4 + "</td>" +
                "<td align='right'>" + result[38].A5 + "</td>" +
                "<td align='right'>" + result[38].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='left'>存放比率</td>" +
                "<td align='right'>" + result[39].A1 + "</td>" +
                "<td align='right'>" + result[39].A2 + "</td>" +
                "<td align='right'>" + result[39].A3 + "</td>" +
                "<td align='right'>" + result[39].A4 + "</td>" +
                "<td align='right'>" + result[39].A5 + "</td>" +
                "<td align='right'>" + result[39].A6 +  "</td>" +
                "</tr>" +
                "</table>";


            #endregion 報表結束

            return str;
        }
        #endregion

        #region 月平均報表
        public string AccumulativeHtmlData(List<SearchItemViewModel> result, string BranchName, SearchInfoViewModel SearchInfo)
        {
            string str = "";
            string YM;
            YM = (int.Parse(SearchInfo.YeartMonth) + 191100).ToString();
            DateTime YeartMonth = DateTime.Parse(YM.Substring(0, 4) + "-" + YM.Substring(4, 2) + "-01");

            #region 報表開始
            //表頭
            str = "<html><body>" +
                "<table border=0 width=90% align=center cellpadding=0 cellspacing=0>" +
                "<tr align='center'>" +
                "<td colspan=8>" +
                "<font size='5'>" + BranchName + "&nbsp&nbsp&nbsp&nbsp經營管理記錄卡(累計平均餘額)</font>" +
                "</td>" +
                "</tr>" +
                "<tr align='center'> " +
                "<td colspan=6>" +
                "<font size='4'>單位代號：" + SearchInfo.Branch_No + "</font>" +
                "</td>" +
                "<td nowrap>單位：千(美)元 </td>" +
                "</tr>" +
                "</table>" +
                "<br>";
            int n = 0;
            string YM1 = (YeartMonth.Year - 1911).ToString() + "年" + YeartMonth.Month.ToString("00") + "月";
            if (YeartMonth.Month < 2) { n = 1; } else { n = 0; }
            string YM2 = (YeartMonth.Year - 1911 - n).ToString() + "年" + YeartMonth.AddMonths(-1).Month.ToString("00") + "月";
            if (YeartMonth.Month < 3) { n = 1; } else { n = 0; }
            string YM3 = (YeartMonth.Year - 1911 - n).ToString() + "年" + YeartMonth.AddMonths(-2).Month.ToString("00") + "月";
            string YM4 = (YeartMonth.Year - 1 - 1911).ToString() + "年12月";
            string YM5 = (YeartMonth.Year - 2 - 1911).ToString() + "年12月";
            string YM6 = (YeartMonth.Year - 3 - 1911).ToString() + "年12月";

            //報表內容
            str += "<table border=1 width=90% align=center cellpadding=0 cellspacing=0>" +
                "<tr>" +
                "<td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td>" +
                "</tr>" +
                "<tr>" +
                "<td colspan=3 align='center'>項目</td>" +
                "<td align='center'>" + YM1 + "</td>" +
                "<td align='center'>" + YM2 + "</td>" +
                "<td align='center'>" + YM3 + "</td>" +
                "<td align='center'>" + YM4 + "</td>" +
                "<td align='center'>" + YM5 + "</td>" +
                "<td align='center'>" + YM6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td rowspan=17 align='center'>" +
                "存&nbsp;" +
                "<br><br>" +
                "款&nbsp;" +
                "</td>" +
                "<td rowspan=10 nowrap align='center'>" +
                "&nbsp;總<br>" +
                "&nbsp;存<br>" +
                "&nbsp;款<br>" +
                "&nbsp;︹<br>" +
                "&nbsp;含<br>" +
                "&nbsp;公<br>" +
                "&nbsp;庫<br>" +
                "&nbsp;︺<br>" +
                "</td>" +
                "<td nowrap align='left'>(1)自定目標</td>" +
                "<td align='right'>" + result[0].A1 + "</td>" +
                "<td align='right'>" + result[0].A2 + "</td>" +
                "<td align='right'>" + result[0].A3 + "</td>" +
                "<td align='right'>" + result[0].A4 + "</td>" +
                "<td align='right'>" + result[0].A5 + "</td>" +
                "<td align='right'>" + result[0].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='left'>(2)平均餘額</td>" +
                "<td align='right'>" + result[1].A1 + "</td>" +
                "<td align='right'>" + result[1].A2 + "</td>" +
                "<td align='right'>" + result[1].A3 + "</td>" +
                "<td align='right'>" + result[1].A4 + "</td>" +
                "<td align='right'>" + result[1].A5 + "</td>" +
                "<td align='right'>" + result[1].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='left'>(3)達成率</td>" +
                "<td align='right'>" + result[2].A1 + "</td>" +
                "<td align='right'>" + result[2].A2 + "</td>" +
                "<td align='right'>" + result[2].A3 + "</td>" +
                "<td align='right'>" + result[2].A4 + "</td>" +
                "<td align='right'>" + result[2].A5 + "</td>" +
                "<td align='right'>" + result[2].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='left'>上年度平均餘額</td>" +
                "<td align='right'>" + result[3].A1 + "</td>" +
                "<td align='right'>" + result[3].A2 + "</td>" +
                "<td align='right'>" + result[3].A3 + "</td>" +
                "<td align='right'>" + result[3].A4 + "</td>" +
                "<td align='right'>" + result[3].A5 + "</td>" +
                "<td align='right'>" + result[3].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='left'>(4)成長率</td>" +
                "<td align='right'>" + result[4].A1 + "</td>" +
                "<td align='right'>" + result[4].A2 + "</td>" +
                "<td align='right'>" + result[4].A3 + "</td>" +
                "<td align='right'>" + result[4].A4 + "</td>" +
                "<td align='right'>" + result[4].A5 + "</td>" +
                "<td align='right'>" + result[4].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='left'>(5)平均利率</td>" +
                "<td align='right'>" + result[5].A1 + "</td>" +
                "<td align='right'>" + result[5].A2 + "</td>" +
                "<td align='right'>" + result[5].A3 + "</td>" +
                "<td align='right'>" + result[5].A4 + "</td>" +
                "<td align='right'>" + result[5].A5 + "</td>" +
                "<td align='right'>" + result[5].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='left'>(1)一般性平均餘額</td>" +
                "<td align='right'>" + result[6].A1 + "</td>" +
                "<td align='right'>" + result[6].A2 + "</td>" +
                "<td align='right'>" + result[6].A3 + "</td>" +
                "<td align='right'>" + result[6].A4 + "</td>" +
                "<td align='right'>" + result[6].A5 + "</td>" +
                "<td align='right'>" + result[6].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='left'>上年度平均餘額</td>" +
                "<td align='right'>" + result[7].A1 + "</td>" +
                "<td align='right'>" + result[7].A2 + "</td>" +
                "<td align='right'>" + result[7].A3 + "</td>" +
                "<td align='right'>" + result[7].A4 + "</td>" +
                "<td align='right'>" + result[7].A5 + "</td>" +
                "<td align='right'>" + result[7].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='left'>(2)一般性成長率</td>" +
                "<td align='right'>" + result[8].A1 + "</td>" +
                "<td align='right'>" + result[8].A2 + "</td>" +
                "<td align='right'>" + result[8].A3 + "</td>" +
                "<td align='right'>" + result[8].A4 + "</td>" +
                "<td align='right'>" + result[8].A5 + "</td>" +
                "<td align='right'>" + result[8].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='left'>(3)一般性存款比率</td>" +
                "<td align='right'>" + result[9].A1 + "</td>" +
                "<td align='right'>" + result[9].A2 + "</td>" +
                "<td align='right'>" + result[9].A3 + "</td>" +
                "<td align='right'>" + result[9].A4 + "</td>" +
                "<td align='right'>" + result[9].A5 + "</td>" +
                "<td align='right'>" + result[9].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td rowspan=6 nowrap align='center'>" +
                "活&nbsp;<br>" +
                "期&nbsp;<br>" +
                "性&nbsp;<br>" +
                "存&nbsp;<br>" +
                "款&nbsp;<br>" +
                "︹&nbsp;<br>" +
                "含&nbsp;<br>" +
                "公&nbsp;<br>" +
                "庫&nbsp;<br>" +
                "︺&nbsp;<br>" +
                "</td>" +
                "<td nowrap align='left'>(1)自定目標</td>" +
                "<td align='right'>" + result[10].A1 + "</td>" +
                "<td align='right'>" + result[10].A2 + "</td>" +
                "<td align='right'>" + result[10].A3 + "</td>" +
                "<td align='right'>" + result[10].A4 + "</td>" +
                "<td align='right'>" + result[10].A5 + "</td>" +
                "<td align='right'>" + result[10].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='left'>(2)平均餘額</td>" +
                "<td align='right'>" + result[11].A1 + "</td>" +
                "<td align='right'>" + result[11].A2 + "</td>" +
                "<td align='right'>" + result[11].A3 + "</td>" +
                "<td align='right'>" + result[11].A4 + "</td>" +
                "<td align='right'>" + result[11].A5 + "</td>" +
                "<td align='right'>" + result[11].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='left'>(3)達成率</td>" +
                "<td align='right'>" + result[12].A1 + "</td>" +
                "<td align='right'>" + result[12].A2 + "</td>" +
                "<td align='right'>" + result[12].A3 + "</td>" +
                "<td align='right'>" + result[12].A4 + "</td>" +
                "<td align='right'>" + result[12].A5 + "</td>" +
                "<td align='right'>" + result[12].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='left'>上年度平均餘額</td>" +
                "<td align='right'>" + result[13].A1 + "</td>" +
                "<td align='right'>" + result[13].A2 + "</td>" +
                "<td align='right'>" + result[13].A3 + "</td>" +
                "<td align='right'>" + result[13].A4 + "</td>" +
                "<td align='right'>" + result[13].A5 + "</td>" +
                "<td align='right'>" + result[13].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='left'>(4)成長率</td>" +
                "<td align='right'>" + result[14].A1 + "</td>" +
                "<td align='right'>" + result[14].A2 + "</td>" +
                "<td align='right'>" + result[14].A3 + "</td>" +
                "<td align='right'>" + result[14].A4 + "</td>" +
                "<td align='right'>" + result[14].A5 + "</td>" +
                "<td align='right'>" + result[14].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='left'>(5)活存比率</td>" +
                "<td align='right'>" + result[15].A1 + "</td>" +
                "<td align='right'>" + result[15].A2 + "</td>" +
                "<td align='right'>" + result[15].A3 + "</td>" +
                "<td align='right'>" + result[15].A4 + "</td>" +
                "<td align='right'>" + result[15].A5 + "</td>" +
                "<td align='right'>" + result[15].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td colspan=2 nowrap align='left'>公庫存款</td>" +
                "<td align='right'>" + result[16].A1 + "</td>" +
                "<td align='right'>" + result[16].A2 + "</td>" +
                "<td align='right'>" + result[16].A3 + "</td>" +
                "<td align='right'>" + result[16].A4 + "</td>" +
                "<td align='right'>" + result[16].A5 + "</td>" +
                "<td align='right'>" + result[16].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td rowspan=9 align='center'>" +
                "放&nbsp;<br>" +
                "&nbsp;<br>" +
                "款&nbsp;<br>" +
                "</td>" +
                "<td rowspan=7 nowrap align='center'>" +
                "自&nbsp;<br>" +
                "有&nbsp;<br>" +
                "資&nbsp;<br>" +
                "金&nbsp;<br>" +
                "</td>" +
                "<td nowrap align='left'>(1)自定目標</td>" +
                "<td align='right'>" + result[17].A1 + "</td>" +
                "<td align='right'>" + result[17].A2 + "</td>" +
                "<td align='right'>" + result[17].A3 + "</td>" +
                "<td align='right'>" + result[17].A4 + "</td>" +
                "<td align='right'>" + result[17].A5 + "</td>" +
                "<td align='right'>" + result[17].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='left'>(2)平均餘額</td>" +
                "<td align='right'>" + result[18].A1 + "</td>" +
                "<td align='right'>" + result[18].A2 + "</td>" +
                "<td align='right'>" + result[18].A3 + "</td>" +
                "<td align='right'>" + result[18].A4 + "</td>" +
                "<td align='right'>" + result[18].A5 + "</td>" +
                "<td align='right'>" + result[18].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap nowrap align='left'>(3)達成率</td>" +
                "<td align='right'>" + result[19].A1 + "</td>" +
                "<td align='right'>" + result[19].A2 + "</td>" +
                "<td align='right'>" + result[19].A3 + "</td>" +
                "<td align='right'>" + result[19].A4 + "</td>" +
                "<td align='right'>" + result[19].A5 + "</td>" +
                "<td align='right'>" + result[19].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='left'>上年度平均餘額</td>" +
                "<td align='right'>" + result[20].A1 + "</td>" +
                "<td align='right'>" + result[20].A2 + "</td>" +
                "<td align='right'>" + result[20].A3 + "</td>" +
                "<td align='right'>" + result[20].A4 + "</td>" +
                "<td align='right'>" + result[20].A5 + "</td>" +
                "<td align='right'>" + result[20].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='left'>(4)成長率</td>" +
                "<td align='right'>" + result[21].A1 + "</td>" +
                "<td align='right'>" + result[21].A2 + "</td>" +
                "<td align='right'>" + result[21].A3 + "</td>" +
                "<td align='right'>" + result[21].A4 + "</td>" +
                "<td align='right'>" + result[21].A5 + "</td>" +
                "<td align='right'>" + result[21].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='left'>(5)存放比率</td>" +
                "<td align='right'>" + result[22].A1 + "</td>" +
                "<td align='right'>" + result[22].A2 + "</td>" +
                "<td align='right'>" + result[22].A3 + "</td>" +
                "<td align='right'>" + result[22].A4 + "</td>" +
                "<td align='right'>" + result[22].A5 + "</td>" +
                "<td align='right'>" + result[22].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='left'>(6)平均利率</td>" +
                "<td align='right'>" + result[23].A1 + "</td>" +
                "<td align='right'>" + result[23].A2 + "</td>" +
                "<td align='right'>" + result[23].A3 + "</td>" +
                "<td align='right'>" + result[23].A4 + "</td>" +
                "<td align='right'>" + result[23].A5 + "</td>" +
                "<td align='right'>" + result[23].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td colspan=2 nowrap align='left'>應收代放款</td>" +
                "<td align='right'>" + result[24].A1 + "</td>" +
                "<td align='right'>" + result[24].A2 + "</td>" +
                "<td align='right'>" + result[24].A3 + "</td>" +
                "<td align='right'>" + result[24].A4 + "</td>" +
                "<td align='right'>" + result[24].A5 + "</td>" +
                "<td align='right'>" + result[24].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td colspan=2 nowrap align='left'>放款總額</td>" +
                "<td align='right'>" + result[25].A1 + "</td>" +
                "<td align='right'>" + result[25].A2 + "</td>" +
                "<td align='right'>" + result[25].A3 + "</td>" +
                "<td align='right'>" + result[25].A4 + "</td>" +
                "<td align='right'>" + result[25].A5 + "</td>" +
                "<td align='right'>" + result[25].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td rowspan=4 align='center'>" +
                "逾&nbsp;<br>" +
                "期&nbsp;<br>" +
                "放&nbsp;<br>" +
                "款&nbsp;<br>" +
                "</td>" +
                "<td colspan=2 nowrap align='left'>逾放總額</td>" +
                "<td align='right'>" + result[26].A1 + "</td>" +
                "<td align='right'>" + result[26].A2 + "</td>" +
                "<td align='right'>" + result[26].A3 + "</td>" +
                "<td align='right'>" + result[26].A4 + "</td>" +
                "<td align='right'>" + result[26].A5 + "</td>" +
                "<td align='right'>" + result[26].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td colspan=2 nowrap align='left'>甲類</td>" +
                "<td align='right'>" + result[27].A1 + "</td>" +
                "<td align='right'>" + result[27].A2 + "</td>" +
                "<td align='right'>" + result[27].A3 + "</td>" +
                "<td align='right'>" + result[27].A4 + "</td>" +
                "<td align='right'>" + result[27].A5 + "</td>" +
                "<td align='right'>" + result[27].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td colspan=2 nowrap align='left'>乙類</td>" +
                "<td align='right'>" + result[28].A1 + "</td>" +
                "<td align='right'>" + result[28].A2 + "</td>" +
                "<td align='right'>" + result[28].A3 + "</td>" +
                "<td align='right'>" + result[28].A4 + "</td>" +
                "<td align='right'>" + result[28].A5 + "</td>" +
                "<td align='right'>" + result[28].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td colspan=2 nowrap align='left'>逾放比率</td>" +
                "<td align='right'>" + result[29].A1 + "</td>" +
                "<td align='right'>" + result[29].A2 + "</td>" +
                "<td align='right'>" + result[29].A3 + "</td>" +
                "<td align='right'>" + result[29].A4 + "</td>" +
                "<td align='right'>" + result[29].A5 + "</td>" +
                "<td align='right'>" + result[29].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td rowspan=3 align='center'>" +
                "外&nbsp;<br>" +
                "匯&nbsp;<br>" +
                "</td>" +
                "<td colspan=2 nowrap align='left'>(1)自定目標 單位:仟美元</td>" +
                "<td align='right'>" + result[30].A1 + "</td>" +
                "<td align='right'>" + result[30].A2 + "</td>" +
                "<td align='right'>" + result[30].A3 + "</td>" +
                "<td align='right'>" + result[30].A4 + "</td>" +
                "<td align='right'>" + result[30].A5 + "</td>" +
                "<td align='right'>" + result[30].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td colspan=2 nowrap align='left'>(2)承作金額 單位:仟美元 </td>" +
                "<td align='right'>" + result[31].A1 + "</td>" +
                "<td align='right'>" + result[31].A2 + "</td>" +
                "<td align='right'>" + result[31].A3 + "</td>" +
                "<td align='right'>" + result[31].A4 + "</td>" +
                "<td align='right'>" + result[31].A5 + "</td>" +
                "<td align='right'>" + result[31].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td colspan=2 nowrap align='left'>(3)達成率</td>" +
                "<td align='right'>" + result[32].A1 + "</td>" +
                "<td align='right'>" + result[32].A2 + "</td>" +
                "<td align='right'>" + result[32].A3 + "</td>" +
                "<td align='right'>" + result[32].A4 + "</td>" +
                "<td align='right'>" + result[32].A5 + "</td>" +
                "<td align='right'>" + result[32].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td rowspan=3 align='center'>" +
                "提&nbsp;<br>" +
                "存&nbsp;<br>" +
                "前&nbsp;<br>" +
                "盈&nbsp;<br>" +
                "餘&nbsp;<br>" +
                "</td>" +
                "<td colspan=2 nowrap align='left'>(1)自訂目標</td>" +
                "<td align='right'>" + result[33].A1 + "</td>" +
                "<td align='right'>" + result[33].A2 + "</td>" +
                "<td align='right'>" + result[33].A3 + "</td>" +
                "<td align='right'>" + result[33].A4 + "</td>" +
                "<td align='right'>" + result[33].A5 + "</td>" +
                "<td align='right'>" + result[33].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap colspan=2 align='left'>(2)金額</td>" +
                "<td align='right'>" + result[34].A1 + "</td>" +
                "<td align='right'>" + result[34].A2 + "</td>" +
                "<td align='right'>" + result[34].A3 + "</td>" +
                "<td align='right'>" + result[34].A4 + "</td>" +
                "<td align='right'>" + result[34].A5 + "</td>" +
                "<td align='right'>" + result[34].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap colspan=2 align='left'>(3)達成率</td>" +
                "<td align='right'>" + result[35].A1 + "</td>" +
                "<td align='right'>" + result[35].A2 + "</td>" +
                "<td align='right'>" + result[35].A3 + "</td>" +
                "<td align='right'>" + result[35].A4 + "</td>" +
                "<td align='right'>" + result[35].A5 + "</td>" +
                "<td align='right'>" + result[35].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td rowspan=3 colspan=2 align='center'>" +
                "財富管理&nbsp;<br>" +
                "業務手續&nbsp;<br>" +
                "費收入&nbsp;<br>" +
                "</td>" +
            #region 需再要調整 自訂目標
                "<td nowrap align='left'>自訂目標</td>" +
                "<td align='right'>" + result[36].A1 + "</td>" +
                "<td align='right'>" + result[37].A2 + "</td>" +
                "<td align='right'>" + result[38].A2 + "</td>" +
                "<td align='right'></td>" +
                "<td align='right'></td>" +
                "<td align='right'></td>" +
                "</tr>" +

                "<tr>" +
                "<td nowrap align='left'>金額</td>" +
                "<td align='right'>" + result[39].A1 + "</td>" +
                "<td align='right'>" + result[40].A2 + "</td>" +
                "<td align='right'>" + result[41].A3 + "</td>" +
                "<td align='right'></td>" +
                "<td align='right'></td>" +
                "<td align='right'></td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap align='left'>達成率</td>" +
                "<td align='right'>" + result[42].A1 + "</td>" +
                "<td align='right'>" + result[43].A2 + "</td>" +
                "<td align='right'>" + result[44].A3 + "</td>" +
                "<td align='right'></td>" +
                "<td align='right'></td>" +
                "<td align='right'></td>" +
                "</tr>" +
            #endregion
                "<tr>" +
                "<td rowspan=5 align='right'>" +
                "經&nbsp;<br>" +
                "營&nbsp;<br>" +
                "指&nbsp;<br>" +
                "標&nbsp;<br>" +
                "</td>" +
                "<td nowrap colspan=2 align='left'>員工人數</td>" +
                "<td align='right'>" + result[45].A1 + "</td>" +
                "<td align='right'>" + result[45].A2 + "</td>" +
                "<td align='right'>" + result[45].A3 + "</td>" +
                "<td align='right'>" + result[45].A4 + "</td>" +
                "<td align='right'>" + result[45].A5 + "</td>" +
                "<td align='right'>" + result[45].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap colspan=2 align='left'>員工營業量</td>" +
                "<td align='right'>" + result[46].A1 + "</td>" +
                "<td align='right'>" + result[46].A2 + "</td>" +
                "<td align='right'>" + result[46].A3 + "</td>" +
                "<td align='right'>" + result[46].A4 + "</td>" +
                "<td align='right'>" + result[46].A5 + "</td>" +
                "<td align='right'>" + result[46].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap colspan=2 align='left'>員工獲利能力</td>" +
                "<td align='right'>" + result[47].A1 + "</td>" +
                "<td align='right'>" + result[47].A2 + "</td>" +
                "<td align='right'>" + result[47].A3 + "</td>" +
                "<td align='right'>" + result[47].A4 + "</td>" +
                "<td align='right'>" + result[47].A5 + "</td>" +
                "<td align='right'>" + result[47].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap colspan=2 align='left'>用人費率</td>" +
                "<td align='right'>" + result[48].A1 + "</td>" +
                "<td align='right'>" + result[48].A2 + "</td>" +
                "<td align='right'>" + result[48].A3 + "</td>" +
                "<td align='right'>" + result[48].A4 + "</td>" +
                "<td align='right'>" + result[48].A5 + "</td>" +
                "<td align='right'>" + result[48].A6 + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td nowrap colspan=2 align='left'>組別</td>" +
                "<td align='right'>" + result[49].A1 + "</td>" +
                "<td align='right'>" + result[49].A2 + "</td>" +
                "<td align='right'>" + result[49].A3 + "</td>" +
                "<td align='right'>" + result[49].A4 + "</td>" +
                "<td align='right'>" + result[49].A5 + "</td>" +
                "<td align='right'>" + result[49].A6 + "</td>" +
                "</tr>" +
                "</table>";
            #endregion 報表結束
            return str;
        }
        #endregion
    }
}