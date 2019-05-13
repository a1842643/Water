using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WaterCaseTracking.Models.ViewModels.MinuteLog;
using WaterCaseTracking.Dao;

namespace WaterCaseTracking.Service {
    public class MinuteLogService {
        #region 申請單寫入 ApplicationInsert()
        public MinuteLogItemViewModel QuerySearchList(QueryGetoTableViewModel searchInfo) {
            #region 參數宣告				
            MinuteLogItemViewModel searchList = new MinuteLogItemViewModel();
            LoggDao landLogDao = new LoggDao();
            #endregion

            #region 流程																
            searchList = landLogDao.QuerySearchList(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫

            return searchList;
            #endregion
        }
        #endregion
    }
}