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
    class ImportFile_15_1
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

        private string mBranch_No;
        private string mYM;
        private Int16 mBuy_Exp_Count;
        private decimal mBuy_Exp_Amt;
        private Int16 mBuy_In_Count;
        private decimal mBuy_In_Amt;
        private Int16 mSell_Imp_Count;
        private decimal mSell_Imp_Amt;
        private Int16 mSell_Out_Count;
        private decimal mSell_Out_Amt;
        private Int16 mExchage_Total_Count;
        private decimal mExchage_Total_Amt;
        private Int16 mTotal_Count;
        private decimal mTotal_Amt;

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

        public Int16 Buy_Exp_Count
        {
            get
            {
                return mBuy_Exp_Count;
            }

            set
            {
                mBuy_Exp_Count = value;
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

        public Int16 Buy_In_Count
        {
            get
            {
                return mBuy_In_Count;
            }

            set
            {
                mBuy_In_Count = value;
            }
        }

        public decimal Buy_In_Amt
        {
            get
            {
                return mBuy_In_Amt;
            }

            set
            {
                mBuy_In_Amt = value;
            }
        }

        public Int16 Sell_Imp_Count
        {
            get
            {
                return mSell_Imp_Count;
            }

            set
            {
                mSell_Imp_Count = value;
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

        public Int16 Sell_Out_Count
        {
            get
            {
                return mSell_Out_Count;
            }

            set
            {
                mSell_Out_Count = value;
            }
        }

        public decimal Sell_Out_Amt
        {
            get
            {
                return mSell_Out_Amt;
            }

            set
            {
                mSell_Out_Amt = value;
            }
        }

        public Int16 Exchage_Total_Count
        {
            get
            {
                return mExchage_Total_Count;
            }

            set
            {
                mExchage_Total_Count = value;
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

        public Int16 Total_Count
        {
            get
            {
                return mTotal_Count;
            }

            set
            {
                mTotal_Count = value;
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
        public ImportFile_15_1 getImportFile_51(string YM, string Branch_No)
        {
            ImportFile_15_1 ImportFile_15_1 = new ImportFile_15_1();
            // Dim sql As String = "SELECT  UNT_UNITNAME as UNITNAME,EMP_EXTENDADD,* FROM SALMMPR "
            string sql = @"SELECT Buy_Exp_Amt, Sell_Imp_Amt, Exchage_Total_Amt, Total_Amt " +
                    "FROM ImportFile_15_1   " +
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
                ImportFile_15_1.Buy_Exp_Amt = Convert.ToDecimal(dt.Rows[0]["Buy_Exp_Amt"].ToString());
                ImportFile_15_1.Sell_Imp_Amt = Convert.ToDecimal(dt.Rows[0]["Sell_Imp_Amt"].ToString());
                ImportFile_15_1.Exchage_Total_Amt = Convert.ToDecimal(dt.Rows[0]["Exchage_Total_Amt"].ToString());
                ImportFile_15_1.Total_Amt = Convert.ToDecimal(dt.Rows[0]["Total_Amt"].ToString());

            }
            else
                ImportFile_15_1 = null;
            return ImportFile_15_1;
        }
        public DataTable getImportFile_51_1byYM(string YM)
        {
            string sql = @"SELECT * FROM ImportFile_15_1 WHERE YM = @YM";
            SqlCommand cmd = new SqlCommand();
            {
                var withBlock = cmd;
                withBlock.Parameters.Add("@YM", SqlDbType.Char).Value = YM;
                withBlock.CommandText = sql;
            }

            clsDB clsDB = new clsDB();
            DataTable dt = clsDB.OpenDataTable(cmd);
            return dt;
        }
        private void insertImportFile_15_1(string strYM, Common.clsEnum.EditMode enmEditMode)
        {
            Common.clsConvert clsConvert = new Common.clsConvert();
            YM = strYM;
            SqlConnection cn = new SqlConnection();
            string sql = "";
            SqlCommand cmd = new SqlCommand();

            switch (enmEditMode)
            {
                case Common.clsEnum.EditMode.Insert:
                    sql = @"INSERT INTO ImportFile_15_1 (Branch_No,YM,Buy_Exp_Count,Buy_Exp_Amt,Buy_In_Count,
                        Buy_In_Amt,Sell_Imp_Count,Sell_Imp_Amt,Sell_Out_Count,Sell_Out_Amt,
                        Exchage_Total_Count,Exchage_Total_Amt,Total_Count,Total_Amt) 
                         values(
                        @Branch_No,@YM,@Buy_Exp_Count,@Buy_Exp_Amt,@Buy_In_Count,
                        @Buy_In_Amt,@Sell_Imp_Count,@Sell_Imp_Amt,@Sell_Out_Count,@Sell_Out_Amt,
                        @Exchage_Total_Count,@Exchage_Total_Amt,@Total_Count,@Total_Amt) ";

                    cmd.Parameters.Add("@YM", SqlDbType.VarChar).Value = YM == null ? "" : YM;
                    cmd.Parameters.Add("@Branch_No", SqlDbType.VarChar).Value = Branch_No == null ? "" : Branch_No;
                    cmd.Parameters.Add("@Buy_Exp_Count", SqlDbType.SmallInt).Value = 0;
                    cmd.Parameters.Add("@Buy_Exp_Amt", SqlDbType.Decimal).Value = Buy_Exp_Amt;
                    cmd.Parameters.Add("@Buy_In_Count", SqlDbType.SmallInt).Value = 0;
                    cmd.Parameters.Add("@Buy_In_Amt", SqlDbType.Decimal).Value = 0;
                    cmd.Parameters.Add("@Sell_Imp_Count", SqlDbType.SmallInt).Value = 0;
                    cmd.Parameters.Add("@Sell_Imp_Amt", SqlDbType.Decimal).Value = Sell_Imp_Amt;
                    cmd.Parameters.Add("@Sell_Out_Count", SqlDbType.SmallInt).Value = 0;
                    cmd.Parameters.Add("@Sell_Out_Amt", SqlDbType.Decimal).Value = 0;
                    cmd.Parameters.Add("@Exchage_Total_Count", SqlDbType.SmallInt).Value = 0;
                    cmd.Parameters.Add("@Exchage_Total_Amt", SqlDbType.Decimal).Value = Exchage_Total_Amt;
                    cmd.Parameters.Add("@Total_Count", SqlDbType.SmallInt).Value = 0;
                    cmd.Parameters.Add("@Total_Amt", SqlDbType.Decimal).Value = Total_Amt;
                    break;
                case Common.clsEnum.EditMode.Update:

                    sql = @"update ImportFile_15_1 set 
                            Buy_Exp_Amt=@Buy_Exp_Amt,
                            Sell_Imp_Amt=@Sell_Imp_Amt,
                            Exchage_Total_Amt=@Exchage_Total_Amt,
                            Total_Amt=@Total_Amt
                            where YM=@YM AND Branch_No=@Branch_No";

                    cmd.Parameters.Add("@YM", SqlDbType.VarChar).Value = YM == null ? "" : YM;
                    cmd.Parameters.Add("@Branch_No", SqlDbType.VarChar).Value = Branch_No == null ? "" : Branch_No;                    
                    cmd.Parameters.Add("@Buy_Exp_Amt", SqlDbType.Decimal).Value = Buy_Exp_Amt;                    
                    cmd.Parameters.Add("@Sell_Imp_Amt", SqlDbType.Decimal).Value = Sell_Imp_Amt;
                    cmd.Parameters.Add("@Exchage_Total_Amt", SqlDbType.Decimal).Value = Exchage_Total_Amt;
                    cmd.Parameters.Add("@Total_Amt", SqlDbType.Decimal).Value = Total_Amt;
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
                DataTable dtChk = getImportFile_51_1byYM(strYM);
                bool IsUpdate = dtChk.Rows.Count > 0;

                foreach (string strBranch_No in parserBody.Keys)
                {
                    Type classType = typeof(ImportFile_15_1);
                    List<Tuple<string, Type, Object>> parserValues = parserBody[strBranch_No];
                    foreach (var UnitData in parserValues)
                    {
                        PropertyInfo classProperty = classType.GetProperty(UnitData.Item1);
                        classProperty.SetValue(this, UnitData.Item3, null);
                    }
                    //若有該筆單位資料為TRUE， 否則為FALSE
                    IsUpdate = dtChk.Select("Branch_No='" + strBranch_No + "'").Length > 0;
                    
                    insertImportFile_15_1(strYM, IsUpdate == true ? Common.clsEnum.EditMode.Update : Common.clsEnum.EditMode.Insert);
                    iCount += 1;
                }
                return new Tuple<string, int>("轉入ImportFile_15_1作業完成!，共匯入" + iCount.ToString() + "筆。", iCount);
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
            string strTableName = "ImportFile_15_1";
            int iCount = 0;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();


            string sql = @"delete from ImportFile_15_1_old where SUBSTRING(YM,1,3)=@YYY;
                        INSERT INTO ImportFile_15_1_old 
                        SELECT * FROM ImportFile_15_1 WHERE  SUBSTRING(YM,1,3)=@YYY
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
            return new Tuple<string, int>(strTableName+ "移轉" + strYYY + "年資料共" + iCount.ToString("#,##0") + "筆。" + Environment.NewLine, iCount);
        }


        /// <summary>
        /// 自動三年歷史資料搬移DB ，113年刪除104年
        /// </summary>
        /// <param name="strYM">要被移轉的年份</param>
        public Tuple<string, int> deleteOldData(string strYYY)
        {
            string strTableName = "ImportFile_15_1";
            int iCount = 0;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();


            string sql = @"delete from ImportFile_15_1 where SUBSTRING(YM,1,3)=@YYY";

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
