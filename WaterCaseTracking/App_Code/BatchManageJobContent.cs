using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking
{
    public class BatchManageJobContent
    {

        public static Dictionary<string, List<Tuple<string, string>>> getJobContent()
        {
            Dictionary<string, List<Tuple<string, string>>> JobContent = new Dictionary<string, List<Tuple<string, string>>>();
            List<Tuple<string, string>> FuncAttributeT = new List<Tuple<string, string>>();
            FuncAttributeT.Add(new Tuple<string, string>("ImportFiles", "匯入檔案"));
            JobContent.Add("T-轉檔", FuncAttributeT);
            List<Tuple<string, string>> FuncAttributeR = new List<Tuple<string, string>>();
            FuncAttributeR.Add(new Tuple<string, string>("01OperatedCardBResult", "營運概況表"));
            FuncAttributeR.Add(new Tuple<string, string>("02AverageBResult", "經營管理紀錄卡月平均"));
            FuncAttributeR.Add(new Tuple<string, string>("03AverageCountBResult", "經營管理紀錄卡累計平均"));
            FuncAttributeR.Add(new Tuple<string, string>("04DepositFXSurplusResult", "存放外匯盈餘概況統計表(當月)"));
            FuncAttributeR.Add(new Tuple<string, string>("05DepositFXSurplusCumResult", "存放外匯盈餘概況統計表(累計)"));
            FuncAttributeR.Add(new Tuple<string, string>("06OperatedCardAllUnitResultA", "全行營運概況表A"));
            FuncAttributeR.Add(new Tuple<string, string>("06OperatedCardAllUnitResultB", "全行營運概況表B"));
            FuncAttributeR.Add(new Tuple<string, string>("07UnitsSAByJob", "各營業單位(存款、放款、盈餘、外匯、逾放、存放款平均利率、用人費率)統計表(依業務分組序)"));
            FuncAttributeR.Add(new Tuple<string, string>("08UnitsSAByArea", "各營業單位(存款、放款、盈餘、外匯、逾放、存放款平均利率、用人費率)統計表(依地區別)"));
            FuncAttributeR.Add(new Tuple<string, string>("09UnitsDeposit3MonthsByJob", "各營業單位總存款最近三月(月平均餘額)增減比較統計表(依業務分組序)"));
            FuncAttributeR.Add(new Tuple<string, string>("10UnitsDemand3MonthsByJob", "各營業單位活期性存款最近三月(月平均餘額)增減比較統計表(依業務分組序)"));
            FuncAttributeR.Add(new Tuple<string, string>("11UnitsSurplus3MonthsByJob", "各營業單位自有資金放款及盈餘最近三月(月平均餘額)增減比較統計表(依業務分組序)"));
            FuncAttributeR.Add(new Tuple<string, string>("12UnitsReceiptByJob", "各營業單位逾期放款、追索債權收回統計表(依業務分組序)"));
            //FuncAttributeR.Add(new Tuple<string, string>("13PerformanceComparison", "業績考核簡表"));
            JobContent.Add("R-報表", FuncAttributeR);
            return JobContent;
        }
    }
}