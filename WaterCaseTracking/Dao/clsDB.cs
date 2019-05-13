using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace ADO
{

    class clsDB
    {
        private SqlConnection g_conn;

        public string connString
        {
            get { return g_conn.ConnectionString; }
        }
        public clsDB(Common.clsEnum.DB_TYPE enmDB_TYPE = Common.clsEnum.DB_TYPE.LandBank)
        {
            string sConStr = "";
            sConStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            g_conn = new SqlConnection(sConStr);
        }

        /// <summary>
        /// 取得各資料庫conn
        /// </summary>
        /// <param name="sDB_ID"></param>
        /// <returns></returns>
        public static SqlConnection DBConn(string sDB_ID = "LandBank")
        {
            string sConStr = "";
            sConStr = ConfigurationManager.ConnectionStrings[sDB_ID].ConnectionString;

            SqlConnection oCon = new SqlConnection(sConStr);
            return oCon;
        }
         
        public DataTable OpenDataTable(string sSql)
        {
            return OpenDataTable(new SqlCommand(sSql));
        }

        // 取得DataTable
        public DataTable OpenDataTable(SqlCommand cmm)
        {
            ConnOpen();
            cmm.Connection = g_conn;
            cmm.CommandTimeout = 60;
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmm);

            try
            {
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmm != null)
                    cmm.Dispose();
                ConnClose();
            }
            return dt;
        } 
        public void ConnClose()
        {
            if (g_conn.State == ConnectionState.Open)
                g_conn.Close();
        }
        public void ConnOpen()
        {
            if (g_conn.State == ConnectionState.Closed)
                g_conn.Open();
        }
    }
}
