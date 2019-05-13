using WaterCaseTracking.Dao;
using WaterCaseTracking.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Service
{
    public class CommonService
    {
        public static void logging(LoggModel landLogModel)
        {
            #region 參數宣告				
            LoggDao loggDao = new LoggDao();
            #endregion

            #region 流程			
            loggDao.AddLogg(landLogModel); //將參數送入Dao層,組立SQL字串並連接資料庫
            #endregion
        }
    }
}