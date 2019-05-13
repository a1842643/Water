using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO
{
    class ImportFile_54
    {
        private SqlTransaction oTran;
        public SqlTransaction Transaction
        {
            get
            {
                return oTran;
            }
            set
            {
                oTran = value;
            }
        }
        public SqlTransaction BeginTransaction()
        {
            SqlConnection cn;
            cn = clsDB.DBConn();
            if (cn.State == ConnectionState.Closed)
                cn.Open();
            oTran = cn.BeginTransaction();
            return oTran;
        }
        private string mYM;
        private string mBranch_No;
        private decimal mTemp_C;
        private decimal mTemp_D;
        private decimal mTemp_E;
        private decimal mTemp_F;
        private decimal mTemp_G;
        private decimal mTemp_H;
        private decimal mTemp_I;
        private decimal mTemp_J;
        private decimal mTemp_K;
        private decimal mTemp_L;
        private decimal mTemp_M;
        private decimal mTemp_N;
        private decimal mTemp_P;
        private decimal mTemp_O;
        private decimal mTemp_Q;
        private decimal mTemp_R;

        public string YM
        {
            get
            {
                return mYM;
            }

            set
            {
                mYM = value;
            }
        }

        public string Branch_No
        {
            get
            {
                return mBranch_No;
            }

            set
            {
                mBranch_No = value;
            }
        }

        public decimal Temp_C
        {
            get
            {
                return mTemp_C;
            }

            set
            {
                mTemp_C = value;
            }
        }

        public decimal Temp_D
        {
            get
            {
                return mTemp_D;
            }

            set
            {
                mTemp_D = value;
            }
        }

        public decimal Temp_E
        {
            get
            {
                return mTemp_E;
            }

            set
            {
                mTemp_E = value;
            }
        }

        public decimal Temp_F
        {
            get
            {
                return mTemp_F;
            }

            set
            {
                mTemp_F = value;
            }
        }
        public decimal Temp_G
        {
            get
            {
                return mTemp_G;
            }

            set
            {
                mTemp_G = value;
            }
        }
        public decimal Temp_H
        {
            get
            {
                return mTemp_H;
            }

            set
            {
                mTemp_H = value;
            }
        }
     

        public decimal Temp_I
        {
            get
            {
                return mTemp_I;
            }

            set
            {
                mTemp_I = value;
            }
        }

        public decimal Temp_J
        {
            get
            {
                return mTemp_J;
            }

            set
            {
                mTemp_J = value;
            }
        }
        public decimal Temp_K
        {
            get
            {
                return mTemp_K;
            }

            set
            {
                mTemp_K = value;
            }
        }
        public decimal Temp_L
        {
            get
            {
                return mTemp_L;
            }

            set
            {
                mTemp_L= value;
            }
        }
        public decimal Temp_M
        {
            get
            {
                return mTemp_M;
            }

            set
            {
                mTemp_M = value;
            }
        }
        public decimal Temp_N
        {
            get
            {
                return mTemp_N;
            }

            set
            {
                mTemp_N = value;
            }
        }
        public decimal Temp_O
        {
            get
            {
                return mTemp_O;
            }

            set
            {
                mTemp_O = value;
            }
        }
        public decimal Temp_P
        {
            get
            {
                return mTemp_P;
            }

            set
            {
                mTemp_P = value;
            }
        }
        public decimal Temp_Q
        {
            get
            {
                return mTemp_Q;
            }

            set
            {
                mTemp_Q = value;
            }
        }

        public decimal Temp_R
        {
            get
            {
                return mTemp_R;
            }

            set
            {
                mTemp_R = value;
            }
        }
              public ImportFile_54 getImportFile_54(string YM, string Branch_No)
        {
            ImportFile_54 ImportFile_54 = new ImportFile_54();
            // Dim sql As String = "SELECT  UNT_UNITNAME as UNITNAME,EMP_EXTENDADD,* FROM SALMMPR "
            string sql = @"SELECT Temp_C, Temp_D, Temp_E, Temp_F,Temp_G,Temp_H, Temp_I, Temp_J,Temp_K,Temp_L,Temp_M,Temp_O,Temp_P Temp_Q, Temp_R " +
                "FROM ImportFile_54 " +
                          "WHERE YM = @YM and Branch_No = @Branch_No";
            SqlCommand cmd = new SqlCommand();
            {
                var withBlock = cmd;
                withBlock.Parameters.Add("@YM", SqlDbType.Char).Value = YM;
                withBlock.Parameters.Add("@Branch_No", SqlDbType.Char).Value = Branch_No;
                withBlock.CommandText = sql;
            }

            clsDB clsDB = new clsDB();
            DataTable dt = clsDB.OpenDataTable(cmd);
            if (dt.Rows.Count > 0)
            {
                ImportFile_54.Temp_C = Convert.ToDecimal(dt.Rows[0]["Temp_C"].ToString());
                ImportFile_54.Temp_D = Convert.ToDecimal(dt.Rows[0]["Temp_D"].ToString());
                ImportFile_54.Temp_E = Convert.ToDecimal(dt.Rows[0]["Temp_E"].ToString());
                ImportFile_54.Temp_F = Convert.ToDecimal(dt.Rows[0]["Temp_F"].ToString());
                ImportFile_54.Temp_G = Convert.ToDecimal(dt.Rows[0]["Temp_G"].ToString());
                ImportFile_54.Temp_H = Convert.ToDecimal(dt.Rows[0]["Temp_H"].ToString());
                ImportFile_54.Temp_J = Convert.ToDecimal(dt.Rows[0]["Temp_J"].ToString());
                ImportFile_54.Temp_Q = Convert.ToDecimal(dt.Rows[0]["Temp_Q"].ToString());
                ImportFile_54.Temp_I = Convert.ToDecimal(dt.Rows[0]["Temp_I"].ToString());
               
            }
            return ImportFile_54;
        }


        /// <summary>
        /// 自動三年歷史資料搬移DB ，113年移轉109
        /// </summary>
        /// <param name="strYM">要被移轉的年份</param>
        public Tuple<string, int> transferOldData(string strYYY)
        {
            string strTableName = "ImportFile_54";
            int iCount = 0;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();


            string sql = @"delete from ImportFile_54_old where SUBSTRING(YM,1,3)=@YYY;
                        INSERT INTO ImportFile_54_old 
                        SELECT * FROM ImportFile_54 WHERE  SUBSTRING(YM,1,3)=@YYY
            ";

            cmd.Parameters.Add("@YYY", SqlDbType.VarChar).Value = strYYY == null ? "" : strYYY;

            cmd.CommandText = sql;


            clsDB clsDB = new clsDB();
            try
            {
                if (oTran != null)
                {
                    cn = oTran.Connection;
                    cmd.Transaction = oTran;
                }
                else cn = clsDB.DBConn();
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = sql;
                iCount = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (oTran == null)
                {
                    if (cn != null)
                        cn.Dispose();
                }
            }
            return new Tuple<string, int>(strTableName + "移轉" + strYYY + "年資料共" + iCount.ToString("#,##0") + "筆。" + Environment.NewLine, iCount);
        }



        /// <summary>
        /// 自動三年歷史資料搬移DB ，113年刪除104年
        /// </summary>
        /// <param name="strYM">要被移轉的年份</param>
        public Tuple<string, int> deleteOldData(string strYYY)
        {
            string strTableName = "ImportFile_54";
            int iCount = 0;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();


            string sql = @"delete from ImportFile_54 where SUBSTRING(YM,1,3)=@YYY";

            cmd.Parameters.Add("@YYY", SqlDbType.VarChar).Value = strYYY == null ? "" : strYYY;

            cmd.CommandText = sql;


            clsDB clsDB = new clsDB();
            try
            {
                if (oTran != null)
                {
                    cn = oTran.Connection;
                    cmd.Transaction = oTran;
                }
                else cn = clsDB.DBConn();
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = sql;
                iCount = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (oTran == null)
                {
                    if (cn != null)
                        cn.Dispose();
                }
            }
            return new Tuple<string, int>(strTableName + "刪除" + strYYY + "年舊資料共" + iCount.ToString("#,##0") + "筆。" + Environment.NewLine, iCount);
        }

    }
}
