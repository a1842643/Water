using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace ADO
{
    class ImportFile_51
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
        private decimal mOB_Save_Amt;
        private decimal mOB_Out_Amt;

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

        public decimal OB_Save_Amt
        {
            get
            {
                return mOB_Save_Amt;
            }

            set
            {
                mOB_Save_Amt = value;
            }
        }

        public decimal OB_Out_Amt
        {
            get
            {
                return mOB_Out_Amt;
            }

            set
            {
                mOB_Out_Amt = value;
            }
        }
        public decimal OB_Save_Amt_OBU
        {
            get;set;
        }
        public decimal OB_Out_Amt_OBU
        {
            get;set;
        }
        public ImportFile_51 getImportFile_51(string YM, string Branch_No)
        {
            ImportFile_51 ImportFile_51 = new ImportFile_51();
            // Dim sql As String = "SELECT  UNT_UNITNAME as UNITNAME,EMP_EXTENDADD,* FROM SALMMPR "
            string sql = @"SELECT OB_Save_Amt, OB_Out_Amt,OB_Save_Amt, OB_Out_Amt,OB_Save_Amt_OBU,OB_Out_Amt_OBU " +
                    "FROM ImportFile_51   " +
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
                ImportFile_51.OB_Save_Amt = Convert.ToDecimal(dt.Rows[0]["OB_Save_Amt"].ToString());
                ImportFile_51.OB_Out_Amt = Convert.ToDecimal(dt.Rows[0]["OB_Out_Amt"].ToString());
                ImportFile_51.OB_Save_Amt_OBU = Convert.ToDecimal(dt.Rows[0]["OB_Save_Amt_OBU"].ToString());
                ImportFile_51.OB_Out_Amt_OBU = Convert.ToDecimal(dt.Rows[0]["OB_Out_Amt_OBU"].ToString());
            }
            else
            {
                ImportFile_51 = null;
            }
            return ImportFile_51;
        }


        private void insertImportFile_51(string strYM, Common.clsEnum.EditMode enmEditMode)
        {
            Common.clsConvert clsConvert = new Common.clsConvert();
            YM = strYM;
            SqlConnection cn = new SqlConnection();
            string sql = "";
            SqlCommand cmd = new SqlCommand();

            switch (enmEditMode)
            {
                case Common.clsEnum.EditMode.Insert:
                    sql = @"INSERT INTO ImportFile_51 
                        (Branch_No,YM,OB_Save_Amt,OB_Out_Amt,OB_Save_Amt_OBU,OB_Out_Amt_OBU) 
                         values(
                        @Branch_No,@YM,@OB_Save_Amt,@OB_Out_Amt,@OB_Save_Amt_OBU,@OB_Out_Amt_OBU) ";

                    cmd.Parameters.Add("@YM", SqlDbType.VarChar).Value = YM == null ? "" : YM;
                    cmd.Parameters.Add("@Branch_No", SqlDbType.VarChar).Value = Branch_No == null ? "" : Branch_No;
                    cmd.Parameters.Add("@OB_Save_Amt", SqlDbType.Decimal).Value = OB_Save_Amt;
                    cmd.Parameters.Add("@OB_Out_Amt", SqlDbType.Decimal).Value = OB_Out_Amt;
                    cmd.Parameters.Add("@OB_Save_Amt_OBU", SqlDbType.Decimal).Value = Math.Round(OB_Save_Amt_OBU / 1000, 0, MidpointRounding.AwayFromZero);//OB429C(DBU合計/1000) 取整數
                    cmd.Parameters.Add("@OB_Out_Amt_OBU", SqlDbType.Decimal).Value = Math.Round(OB_Out_Amt_OBU / 1000, 0, MidpointRounding.AwayFromZero);//OB429C(OBU合計/1000) 取整數
                    break;
                case Common.clsEnum.EditMode.Update:
                    sql = @"UPDATE ImportFile_51 SET 
                        OB_Save_Amt=@OB_Save_Amt,
                        OB_Out_Amt=@OB_Out_Amt,
                        OB_Save_Amt_OBU=@OB_Save_Amt_OBU,
                        OB_Out_Amt_OBU=@OB_Out_Amt_OBU
                        WHERE YM=@YM AND Branch_No=@Branch_No";

                    cmd.Parameters.Add("@YM", SqlDbType.VarChar).Value = YM == null ? "" : YM;
                    cmd.Parameters.Add("@Branch_No", SqlDbType.VarChar).Value = Branch_No == null ? "" : Branch_No;
                    cmd.Parameters.Add("@OB_Save_Amt", SqlDbType.Decimal).Value = OB_Save_Amt;
                    cmd.Parameters.Add("@OB_Out_Amt", SqlDbType.Decimal).Value = OB_Out_Amt;
                    cmd.Parameters.Add("@OB_Save_Amt_OBU", SqlDbType.Decimal).Value = Math.Round(OB_Save_Amt_OBU / 1000, 0, MidpointRounding.AwayFromZero);//OB429C(DBU合計/1000) 取整數
                    cmd.Parameters.Add("@OB_Out_Amt_OBU", SqlDbType.Decimal).Value = Math.Round(OB_Out_Amt_OBU / 1000, 0, MidpointRounding.AwayFromZero);//OB429C(OBU合計/1000) 取整數
                    break;
            }
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
                cmd.ExecuteNonQuery();

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

        }


        public DataTable getImportFile_51byYM(string YM)
        {
            string sql = @"SELECT * FROM ImportFile_51 WHERE YM = @YM";
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Add("@YM", SqlDbType.Char).Value = YM;
            cmd.CommandText = sql;

            clsDB clsDB = new clsDB();
            DataTable dt = clsDB.OpenDataTable(cmd);
            return dt;
        }
        public Tuple<string, int> parserProperty(string strYM, Dictionary<string, List<Tuple<string, Type, Object>>> parserBody)
        {
            int iCount = 0;
            try
            {
                Type classType = typeof(ImportFile_51);
                DataTable dtChk = getImportFile_51byYM(strYM);
                bool IsUpdate = dtChk.Rows.Count > 0;

                foreach (string strBranch_No in parserBody.Keys)
                {
                    List<Tuple<string, Type, Object>> parserValues = parserBody[strBranch_No];
                    foreach (var UnitData in parserValues)
                    {
                        PropertyInfo classProperty = classType.GetProperty(UnitData.Item1);
                        classProperty.SetValue(this, UnitData.Item3, null);
                    }

                    //若有該筆單位資料為TRUE， 否則為FALSE
                    IsUpdate = dtChk.Select("Branch_No='" + strBranch_No + "'").Length > 0;

                    insertImportFile_51(strYM, IsUpdate == true ? Common.clsEnum.EditMode.Update : Common.clsEnum.EditMode.Insert);
                    iCount += 1;
                }
                return new Tuple<string, int>("轉入ImportFile_51作業完成!，共匯入" + iCount.ToString() + "筆。", iCount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 自動三年歷史資料搬移DB ，113年移轉109
        /// </summary>
        /// <param name="strYM">要被移轉的年份</param>
        public Tuple<string, int> transferOldData(string strYYY)
        {
            string strTableName = "ImportFile_51";
            int iCount = 0;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();


            string sql = @"delete from ImportFile_51_old where SUBSTRING(YM,1,3)=@YYY;
                        INSERT INTO ImportFile_51_old 
                        SELECT * FROM ImportFile_51 WHERE  SUBSTRING(YM,1,3)=@YYY
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
            string strTableName = "ImportFile_51";
            int iCount = 0;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();


            string sql = @"delete from ImportFile_51 where SUBSTRING(YM,1,3)=@YYY";

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
