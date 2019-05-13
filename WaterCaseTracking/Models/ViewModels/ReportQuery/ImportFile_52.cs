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
    class ImportFile_52
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
        private decimal mBuy_Exp_Amt;
        private decimal mSell_Imp_Amt;
        private decimal mExchage_Total_Amt;
        private decimal mTotal_Amt;
        private string mTotal_Percentage;
        private decimal mOB_Save_Amt;
        private decimal mOB_Out_Amt;
        private string mSave_Percentage;
        private string mOut_Percentage;

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

        public decimal Buy_Exp_Amt
        {
            get
            {
                return mBuy_Exp_Amt;
            }

            set
            {
                mBuy_Exp_Amt = value;
            }
        }

        public decimal Sell_Imp_Amt
        {
            get
            {
                return mSell_Imp_Amt;
            }

            set
            {
                mSell_Imp_Amt = value;
            }
        }

        public decimal Exchage_Total_Amt
        {
            get
            {
                return mExchage_Total_Amt;
            }

            set
            {
                mExchage_Total_Amt = value;
            }
        }

        public decimal Total_Amt
        {
            get
            {
                return mTotal_Amt;
            }

            set
            {
                mTotal_Amt = value;
            }
        }

        public string Total_Percentage
        {
            get
            {
                return mTotal_Percentage;
            }

            set
            {
                mTotal_Percentage = value;
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

        public string Save_Percentage
        {
            get
            {
                return mSave_Percentage;
            }

            set
            {
                mSave_Percentage = value;
            }
        }

        public string Out_Percentage
        {
            get
            {
                return mOut_Percentage;
            }

            set
            {
                mOut_Percentage = value;
            }
        }
        /// <summary>
        /// "外匯存款平均餘額"代辦OBU 累計
        /// </summary>
        public decimal OB_Save_Amt_OBU { get;set; }
        /// <summary>
        /// "外幣放款平均餘額"代辦OBU 累計
        /// </summary>
        public decimal OB_Out_Amt_OBU { get; set; }
       public string Save_Percentage_OBU { get; set; }
        public string Out_Percentage_OBU { get; set; }
        public ImportFile_52 getImportFile_52(string YM, string Branch_No)
        {
            ImportFile_52 ImportFile_52 = new ImportFile_52();
            // Dim sql As String = "SELECT  UNT_UNITNAME as UNITNAME,EMP_EXTENDADD,* FROM SALMMPR "
            string sql = @"SELECT Buy_Exp_Amt, Sell_Imp_Amt, Exchage_Total_Amt, Total_Amt, Total_Percentage, OB_Save_Amt, 
                            Save_Percentage, OB_Out_Amt, Out_Percentage  ,OB_Save_Amt_OBU,OB_Out_Amt_OBU     ,[Save_Percentage_OBU]
      ,[Out_Percentage_OBU]
                    FROM ImportFile_52  " +
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
                ImportFile_52.Buy_Exp_Amt = Convert.ToDecimal(dt.Rows[0]["Buy_Exp_Amt"].ToString());
                ImportFile_52.Sell_Imp_Amt = Convert.ToDecimal(dt.Rows[0]["Sell_Imp_Amt"].ToString());
                ImportFile_52.Exchage_Total_Amt = Convert.ToDecimal(dt.Rows[0]["Exchage_Total_Amt"].ToString());
                ImportFile_52.Total_Amt = Convert.ToDecimal(dt.Rows[0]["Total_Amt"].ToString());
                ImportFile_52.Total_Percentage = dt.Rows[0]["Total_Percentage"].ToString();
                ImportFile_52.OB_Save_Amt = Convert.ToDecimal(dt.Rows[0]["OB_Save_Amt"].ToString());
                ImportFile_52.Save_Percentage = dt.Rows[0]["Save_Percentage"].ToString();
                ImportFile_52.OB_Out_Amt = Convert.ToDecimal(dt.Rows[0]["OB_Out_Amt"].ToString());
                ImportFile_52.Out_Percentage = dt.Rows[0]["Out_Percentage"].ToString();
                ImportFile_52.OB_Save_Amt_OBU = Convert.ToDecimal(dt.Rows[0]["OB_Save_Amt_OBU"].ToString());
                ImportFile_52.OB_Out_Amt_OBU = Convert.ToDecimal(dt.Rows[0]["OB_Out_Amt_OBU"].ToString());
                ImportFile_52.Save_Percentage_OBU = dt.Rows[0]["Save_Percentage_OBU"].ToString();
                ImportFile_52.Out_Percentage_OBU= dt.Rows[0]["Out_Percentage_OBU"].ToString();


            }
            else
                ImportFile_52 = null;
            return ImportFile_52;
        }

        public DataTable getImportFile_52byYM(string YM)
        {
            string sql = @"SELECT * FROM ImportFile_52 WHERE YM = @YM";
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Add("@YM", SqlDbType.Char).Value = YM;
            cmd.CommandText = sql;

            clsDB clsDB = new clsDB();
            DataTable dt = clsDB.OpenDataTable(cmd);
            return dt;
        }

        private void insertImportFile_52(string strYM, Common.clsEnum.EditMode enmEditMode)
        {
            Common.clsConvert clsConvert = new Common.clsConvert();
            YM = strYM;
            SqlConnection cn = new SqlConnection();
            string sql = "";
            SqlCommand cmd = new SqlCommand();

            switch (enmEditMode)
            {
                case Common.clsEnum.EditMode.Insert:
                    sql = @"INSERT INTO ImportFile_52 (Branch_No,YM,Buy_Exp_Amt,
                        Sell_Imp_Amt,Exchage_Total_Amt,Total_Amt,Total_Percentage,
                        OB_Out_Amt,Save_Percentage,Out_Percentage,OB_Save_Amt,OB_Save_Amt_OBU,OB_Out_Amt_OBU) 
                         values(
                        @Branch_No,@YM,@Buy_Exp_Amt,
                        @Sell_Imp_Amt,@Exchage_Total_Amt,@Total_Amt,@Total_Percentage,
                        @OB_Out_Amt,@Save_Percentage,@Out_Percentage,@OB_Save_Amt,@OB_Save_Amt_OBU,@OB_Out_Amt_OBU) ";

                    cmd.Parameters.Add("@YM", SqlDbType.VarChar).Value = YM == null ? "" : YM;
                    cmd.Parameters.Add("@Branch_No", SqlDbType.VarChar).Value = Branch_No == null ? "" : Branch_No;
                    cmd.Parameters.Add("@Buy_Exp_Amt", SqlDbType.Decimal).Value = Buy_Exp_Amt;
                    cmd.Parameters.Add("@Sell_Imp_Amt", SqlDbType.Decimal).Value = Sell_Imp_Amt;
                    cmd.Parameters.Add("@Exchage_Total_Amt", SqlDbType.Decimal).Value = Exchage_Total_Amt;
                    cmd.Parameters.Add("@Total_Amt", SqlDbType.Decimal).Value = Total_Amt;
                    cmd.Parameters.Add("@Total_Percentage", SqlDbType.VarChar).Value = Total_Percentage.Trim();
                    cmd.Parameters.Add("@OB_Save_Amt", SqlDbType.Decimal).Value = OB_Save_Amt;
                    cmd.Parameters.Add("@OB_Out_Amt", SqlDbType.Decimal).Value = OB_Out_Amt;
                    cmd.Parameters.Add("@Save_Percentage", SqlDbType.VarChar).Value = "0%";
                    cmd.Parameters.Add("@Out_Percentage", SqlDbType.VarChar).Value = "0%";
                    cmd.Parameters.Add("@OB_Save_Amt_OBU", SqlDbType.Decimal).Value = OB_Save_Amt_OBU;
                    cmd.Parameters.Add("@OB_Out_Amt_OBU", SqlDbType.Decimal).Value = OB_Out_Amt_OBU;
                    break;
                case Common.clsEnum.EditMode.Update:
                    sql = @"UPDATE ImportFile_52 SET 
                        Buy_Exp_Amt=@Buy_Exp_Amt,
                        Sell_Imp_Amt=@Sell_Imp_Amt,
                        Exchage_Total_Amt=@Exchage_Total_Amt,
                        Total_Amt=@Total_Amt,
                        Total_Percentage=@Total_Percentage,
                        OB_Save_Amt=@OB_Save_Amt,OB_Out_Amt=@OB_Out_Amt,
                        OB_Save_Amt_OBU=@OB_Save_Amt_OBU,OB_Out_Amt_OBU=@OB_Out_Amt_OBU
                        WHERE YM=@YM AND Branch_No=@Branch_No";

                    cmd.Parameters.Add("@YM", SqlDbType.VarChar).Value = YM == null ? "" : YM;
                    cmd.Parameters.Add("@Branch_No", SqlDbType.VarChar).Value = Branch_No == null ? "" : Branch_No;
                    cmd.Parameters.Add("@Buy_Exp_Amt", SqlDbType.Decimal).Value = Buy_Exp_Amt;
                    cmd.Parameters.Add("@Sell_Imp_Amt", SqlDbType.Decimal).Value = Sell_Imp_Amt;
                    cmd.Parameters.Add("@Exchage_Total_Amt", SqlDbType.Decimal).Value = Exchage_Total_Amt;
                    cmd.Parameters.Add("@Total_Amt", SqlDbType.Decimal).Value = Total_Amt;
                    cmd.Parameters.Add("@Total_Percentage", SqlDbType.VarChar).Value = Total_Percentage.Trim();
                    cmd.Parameters.Add("@OB_Save_Amt", SqlDbType.Decimal).Value = OB_Save_Amt;
                    cmd.Parameters.Add("@OB_Out_Amt", SqlDbType.Decimal).Value = OB_Out_Amt;
                    cmd.Parameters.Add("@OB_Save_Amt_OBU", SqlDbType.Decimal).Value = OB_Save_Amt_OBU;
                    cmd.Parameters.Add("@OB_Out_Amt_OBU", SqlDbType.Decimal).Value = OB_Out_Amt_OBU;
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
        public Tuple<string, int> parserProperty(string strYM, Dictionary<string, List<Tuple<string, Type, Object>>> parserBody)
        {
            int iCount = 0;
            try
            {
                Type classType = typeof(ImportFile_52);
                DataTable dtChk = getImportFile_52byYM(strYM);
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

                    insertImportFile_52(strYM, IsUpdate == true ? Common.clsEnum.EditMode.Update : Common.clsEnum.EditMode.Insert);
                    iCount += 1;
                }
                return new Tuple<string, int>("轉入ImportFile_52作業完成!，共匯入" + iCount.ToString() + "筆。", iCount);
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
            string strTableName = "ImportFile_52";
            int iCount = 0;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();


            string sql = @"delete from ImportFile_52_old where SUBSTRING(YM,1,3)=@YYY;
                        INSERT INTO ImportFile_52_old 
                        SELECT * FROM ImportFile_52 WHERE  SUBSTRING(YM,1,3)=@YYY
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
            string strTableName = "ImportFile_52";
            int iCount = 0;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();


            string sql = @"delete from ImportFile_52 where SUBSTRING(YM,1,3)=@YYY";

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
