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
    class Admin_Card
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

        //Dim tran As SqlTransaction = Nothing
        //Dim cn As SqlConnection = Nothing
        //tran = clsSALMEMS.BeginTransaction
        //cn = tran.Connection
        //clsSALMEMS.Transaction = tran
        //try
        //tran.Commit()
        //catch
        //If tran IsNot Nothing Then tran.Rollback()
        #region "匯入運算用欄位"
        /// <summary>
        /// 存款總額 Temp_D1：memo 總存款	DD104-NT-A-A(存款總額+銀行同業存款)	=Temp_D
        /// </summary>
        public decimal Temp_D1 { get; set; }
        /// <summary>
        /// 一般性-總存款=DD104-NT-A-A(總存款）－ OB340(總計)=Temp_Q
        /// OB340：memo OB340.txt總計	
        /// </summary>
        public decimal Temp_Q_OB340 { get; set; }
        /// <summary>
        /// 231100001活期存款 Temp_E1：memo 活期性存款	DD104-NT-A-A(231100001活期存款) ＋OB240＋OB344	=Temp_E
        /// OB240公庫活期=Temp_AS
        /// </summary>
        public decimal Temp_E1 { get; set; }
        /// <summary>
        /// Temp_E_OB344：memo 活期性存款	DD104-NT-A-A(231100001活期存款) ＋OB240＋OB344	=Temp_E
        /// </summary>
        public decimal Temp_E_OB344 { get; set; }
        /// <summary>
        /// 一般性-活存=DD104-NT-A-A(活期性存款) － OB340
        /// Temp_R1：memo OB340.txt 活期性存款小計
        /// DD104-NT-A-A(活期性存款) － Temp_R_OB340	=Temp_R
        /// </summary>
        public decimal Temp_R1 { get; set; }
        /// <summary>
        /// OB340(公營事業軍政機關金融機構非營利事業團體等存款統計表)
        /// Temp_R_OB340：memo 活期性存款小計
        /// DD104-NT-A-A(活期性存款) － Temp_R_OB340	=Temp_R
        /// </summary>
        public decimal Temp_R_OB340 { get; set; }

        /// <summary>
        /// 230100008支票存款 Temp_F1:memo 支存(含本支)	DD104-NT-A-09-A(支票存款+本行支票)	=Temp_F
        /// </summary>
        public decimal Temp_F1 { get; set; }
        /// <summary>
        /// 230200000本行支票 Temp_F2:memo 支存(含本支)	DD104-NT-A-09-A(支票存款+本行支票)	=Temp_F
        /// </summary>
        public decimal Temp_F2 { get; set; }
        /// <summary>
        /// 支票存款-補償費 Temp_I1 :memo 活存-補償金	DD104-NT-A-09-B(支票存款-補償費 + 活期存款-補償費)	=Temp_I
        /// </summary>
        public decimal Temp_I1 { get; set; }
        /// <summary>
        /// 活期存款-補償費 Temp_I2 :memo 活存-補償金	DD104-NT-A-09-B(支票存款-補償費 + 活期存款-補償費)	=Temp_I
        /// </summary>
        public decimal Temp_I2 { get; set; }
        /// <summary>
        /// 定期性存款 Temp_L1:memo 定期性存款	DD104-NT-A-A(定期性存款)+OB240(定期性)	=Temp_L
        /// </summary>
        public decimal Temp_L1 { get; set; }
        /// <summary>
        /// OB240(定期性) Temp_L_OB240:memo 定期性存款	DD104-NT-A-A(定期性存款)+OB240(定期性)	=Temp_L
        /// </summary>
        public decimal Temp_L_OB240 { get; set; }
        /// <summary>
        /// 232101006　定期存款－一般存單 Temp_M1:memo 定存(一般)	DD104-NT-A-A(一般存單)+調整數	=Temp_M
        /// </summary>
        public decimal Temp_M1 { get; set; }
        /// <summary>
        /// 自有資金放款(含外幣)Temp_S=Temp_S1+外幣放款(Temp_V_OB429C總計/1000*匯率)
        /// Temp_S1：取放款總額DD104-NT-B-09-A .TXT，關鍵字：放款總額（自有資金）
        /// </summary>
        public decimal Temp_S1 { get; set; }
        /// <summary>
        /// Temp_V_OB429C總計
        /// Temp_V外幣放款=Temp_V_OB429C總計/1000*Temp_AU匯率
        /// </summary>
        public decimal Temp_V_OB429C總計 { get; set; }
        /// <summary>
        /// mvc畫面輸入的匯率
        /// </summary>
        public decimal Temp_AU { get; set; }
        /// <summary>
        /// 提存前盈餘(當月)=DB201A-09本期淨利(淨損)+各項提存
        /// Temp_AI1：本期淨利（淨損）　
        /// </summary>
        public decimal Temp_AI1 { get; set; }
        /// <summary>
        /// 提存前盈餘(當月)=DB201A-09本期淨利(淨損)+各項提存
        /// Temp_AI1：各項提存　
        /// </summary>
        public decimal Temp_AI2 { get; set; }
        /// <summary>
        /// Temp_AA1=DB201A-09營業收入
        /// 營業收入Temp_A=	DB201A-09(營業收入+營業外收入)
        /// </summary>
        public decimal Temp_AA1 { get; set; }
        /// <summary>
        /// Temp_AA2=DB201A-09營業外收入
        /// 營業收入Temp_A=	DB201A-09(營業收入+營業外收入)
        /// </summary>
        public decimal Temp_AA2 { get; set; }
        /// <summary>
        /// Temp_AD1=DB201A-09手續費收入
        /// 其他收入	Temp_AD=DB201A-09(手續費收入+其他營業外收入)
        /// </summary>
        public decimal Temp_AD1 { get; set; }
        /// <summary>
        /// Temp_AD2=DB201A-09其他營業外收入
        /// 其他收入	Temp_AD=DB201A-09(手續費收入+其他營業外收入)
        /// </summary>
        public decimal Temp_AD2 { get; set; }

        /*???
         * 營業收入	DB201A(營業收入+營業外收入)	drM1.Temp_AA
            利息收入	DB201A(利息收入)	drM1.Temp_AB
            收入內部損益	DB201A(內部損益)	drM1.Temp_AC
            其他收入	DB201A(手續費收入+其他營業外收入)	drM1.Temp_AD
            營業成本	DB201A(營業收入+營業外收入+本期淨利損)	drM1.Temp_AH
            利息費用	DB201A(利息費用)	drM1.Temp_AE
            成本內部損益	DB201A(內部損益)	drM1.Temp_AF
            提存前盈餘	DB201A(本期淨利損+各項提存)

         */
        #endregion

        #region "data欄位"
        private string mYM;
        private string mBranch_No;
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
        private decimal mTemp_O;
        private decimal mTemp_P;
        private decimal mTemp_Q;
        private decimal mTemp_R;
        private decimal mTemp_S;
        private decimal mTemp_T;
        private decimal mTemp_U;
        private decimal mTemp_V;
        private decimal mTemp_W;
        private decimal mTemp_X;
        private decimal mTemp_Y;
        private string mTemp_Z;
        private decimal mTemp_AA;
        private decimal mTemp_AB;
        private decimal mTemp_AC;
        private decimal mTemp_AD;
        private decimal mTemp_AE;
        private decimal mTemp_AF;
        private decimal mTemp_AG;
        private decimal mTemp_AH;
        private decimal mTemp_AI;
        private decimal mTemp_AJ;
        private string mTemp_AK;
        private string mTemp_AL;
        private string mTemp_AM;
        private string mTemp_AN;
        private string mTemp_AO;
        private string mTemp_AP;
        private decimal mTemp_AQ;
        private decimal mTemp_AR;
        private decimal mTemp_AS;
        private decimal mTemp_AT;
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
                mTemp_L = value;
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

        public decimal Temp_S
        {
            get
            {
                return mTemp_S;
            }

            set
            {
                mTemp_S = value;
            }
        }

        public decimal Temp_T
        {
            get
            {
                return mTemp_T;
            }

            set
            {
                mTemp_T = value;
            }
        }

        public decimal Temp_U
        {
            get
            {
                return mTemp_U;
            }

            set
            {
                mTemp_U = value;
            }
        }

        public decimal Temp_V
        {
            get
            {
                return mTemp_V;
            }

            set
            {
                mTemp_V = value;
            }
        }

        public decimal Temp_W
        {
            get
            {
                return mTemp_W;
            }

            set
            {
                mTemp_W = value;
            }
        }

        public decimal Temp_X
        {
            get
            {
                return mTemp_X;
            }

            set
            {
                mTemp_X = value;
            }
        }

        public decimal Temp_Y
        {
            get
            {
                return mTemp_Y;
            }

            set
            {
                mTemp_Y = value;
            }
        }

        public string Temp_Z
        {
            get
            {
                return mTemp_Z;
            }

            set
            {
                mTemp_Z = value;
            }
        }

        public decimal Temp_AA
        {
            get
            {
                return mTemp_AA;
            }

            set
            {
                mTemp_AA = value;
            }
        }

        public decimal Temp_AB
        {
            get
            {
                return mTemp_AB;
            }

            set
            {
                mTemp_AB = value;
            }
        }

        public decimal Temp_AC
        {
            get
            {
                return mTemp_AC;
            }

            set
            {
                mTemp_AC = value;
            }
        }

        public decimal Temp_AD
        {
            get
            {
                return mTemp_AD;
            }

            set
            {
                mTemp_AD = value;
            }
        }

        public decimal Temp_AE
        {
            get
            {
                return mTemp_AE;
            }

            set
            {
                mTemp_AE = value;
            }
        }

        public decimal Temp_AF
        {
            get
            {
                return mTemp_AF;
            }

            set
            {
                mTemp_AF = value;
            }
        }

        public decimal Temp_AG
        {
            get
            {
                return mTemp_AG;
            }

            set
            {
                mTemp_AG = value;
            }
        }

        public decimal Temp_AH
        {
            get
            {
                return mTemp_AH;
            }

            set
            {
                mTemp_AH = value;
            }
        }

        public decimal Temp_AI
        {
            get
            {
                return mTemp_AI;
            }

            set
            {
                mTemp_AI = value;
            }
        }

        public decimal Temp_AJ
        {
            get
            {
                return mTemp_AJ;
            }

            set
            {
                mTemp_AJ = value;
            }
        }

        public string Temp_AK
        {
            get
            {
                return mTemp_AK;
            }

            set
            {
                mTemp_AK = value;
            }
        }

        public string Temp_AL
        {
            get
            {
                return mTemp_AL;
            }

            set
            {
                mTemp_AL = value;
            }
        }

        public string Temp_AM
        {
            get
            {
                return mTemp_AM;
            }

            set
            {
                mTemp_AM = value;
            }
        }

        public string Temp_AN
        {
            get
            {
                return mTemp_AN;
            }

            set
            {
                mTemp_AN = value;
            }
        }

        public string Temp_AO
        {
            get
            {
                return mTemp_AO;
            }

            set
            {
                mTemp_AO = value;
            }
        }

        public string Temp_AP
        {
            get
            {
                return mTemp_AP;
            }

            set
            {
                mTemp_AP = value;
            }
        }
        /// <summary>
        /// drM1.Temp_AQ=OB240(活期性)+存款調整數=Temp_AS+Temp_AR(存款調整數)
        /// </summary>
        public decimal Temp_AQ
        {
            get
            {
                return mTemp_AQ;
            }

            set
            {
                mTemp_AQ = value;
            }
        }
        /// <summary>
        /// 存款調整數
        /// </summary>
        public decimal Temp_AR
        {
            get
            {
                return mTemp_AR;
            }

            set
            {
                mTemp_AR = value;
            }
        }
        /// <summary>
        /// OB240公庫活期
        /// </summary>
        public decimal Temp_AS
        {
            get
            {
                return mTemp_AS;
            }

            set
            {
                mTemp_AS = value;
            }
        }
        /// <summary>
        /// OB344農漁會存款合計活期性同業
        /// </summary>
        public decimal Temp_AT
        {
            get
            {
                return mTemp_AT;
            }

            set
            {
                mTemp_AT = value;
            }
        }
        #endregion

        public Admin_Card getAdmin_Card(string YM, string Branch_No)
        {
            Admin_Card Admin_Card = new Admin_Card();
            // Dim sql As String = "SELECT  UNT_UNITNAME as UNITNAME,EMP_EXTENDADD,* FROM SALMMPR "
            string sql = @"SELECT Temp_D, Temp_E, Temp_F, Temp_G, Temp_H, Temp_I, Temp_J, " +
                        "Temp_K, Temp_L, Temp_M, Temp_N, Temp_O, Temp_P, Temp_Q, Temp_R, Temp_S, " +
                        "Temp_T, Temp_U, Temp_V, Temp_W, Temp_X, Temp_Y, Temp_Z, " +
                        "Temp_AA, Temp_AB, Temp_AC, Temp_AD, Temp_AE, Temp_AF, Temp_AG, Temp_AH, Temp_AI, " +
                        "Temp_AJ, Temp_AK, Temp_AL, Temp_AM, Temp_AN, Temp_AO, Temp_AP, Temp_AQ " +
                        "FROM Admin_Card " +
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
                Admin_Card.Temp_D = Convert.ToDecimal(dt.Rows[0]["Temp_D"].ToString());
                Admin_Card.Temp_E = Convert.ToDecimal(dt.Rows[0]["Temp_E"].ToString());
                Admin_Card.Temp_F = Convert.ToDecimal(dt.Rows[0]["Temp_F"].ToString());
                Admin_Card.Temp_G = Convert.ToDecimal(dt.Rows[0]["Temp_G"].ToString());
                Admin_Card.Temp_H = Convert.ToDecimal(dt.Rows[0]["Temp_H"].ToString());
                Admin_Card.Temp_I = Convert.ToDecimal(dt.Rows[0]["Temp_I"].ToString());
                Admin_Card.Temp_J = Convert.ToDecimal(dt.Rows[0]["Temp_J"].ToString());
                Admin_Card.Temp_K = Convert.ToDecimal(dt.Rows[0]["Temp_K"].ToString());
                Admin_Card.Temp_L = Convert.ToDecimal(dt.Rows[0]["Temp_L"].ToString());
                Admin_Card.Temp_M = Convert.ToDecimal(dt.Rows[0]["Temp_M"].ToString());
                Admin_Card.Temp_N = Convert.ToDecimal(dt.Rows[0]["Temp_N"].ToString());
                Admin_Card.Temp_O = Convert.ToDecimal(dt.Rows[0]["Temp_O"].ToString());
                Admin_Card.Temp_P = Convert.ToDecimal(dt.Rows[0]["Temp_P"].ToString());
                Admin_Card.Temp_Q = Convert.ToDecimal(dt.Rows[0]["Temp_Q"].ToString());
                Admin_Card.Temp_R = Convert.ToDecimal(dt.Rows[0]["Temp_R"].ToString());
                Admin_Card.Temp_S = Convert.ToDecimal(dt.Rows[0]["Temp_S"].ToString());
                Admin_Card.Temp_T = Convert.ToDecimal(dt.Rows[0]["Temp_T"].ToString());
                Admin_Card.Temp_U = Convert.ToDecimal(dt.Rows[0]["Temp_U"].ToString());
                Admin_Card.Temp_V = Convert.ToDecimal(dt.Rows[0]["Temp_V"].ToString());
                Admin_Card.Temp_W = Convert.ToDecimal(dt.Rows[0]["Temp_W"].ToString());
                Admin_Card.Temp_X = Convert.ToDecimal(dt.Rows[0]["Temp_X"].ToString());
                Admin_Card.Temp_Y = Convert.ToDecimal(dt.Rows[0]["Temp_Y"].ToString());
                Admin_Card.Temp_Z = dt.Rows[0]["Temp_Z"].ToString();
                Admin_Card.Temp_AA = Convert.ToDecimal(dt.Rows[0]["Temp_AA"].ToString());
                Admin_Card.Temp_AB = Convert.ToDecimal(dt.Rows[0]["Temp_AB"].ToString());
                Admin_Card.Temp_AC = Convert.ToDecimal(dt.Rows[0]["Temp_AC"].ToString());
                Admin_Card.Temp_AD = Convert.ToDecimal(dt.Rows[0]["Temp_AD"].ToString());
                Admin_Card.Temp_AE = Convert.ToDecimal(dt.Rows[0]["Temp_AE"].ToString());
                Admin_Card.Temp_AF = Convert.ToDecimal(dt.Rows[0]["Temp_AF"].ToString());
                Admin_Card.Temp_AG = Convert.ToDecimal(dt.Rows[0]["Temp_AG"].ToString());
                Admin_Card.Temp_AH = Convert.ToDecimal(dt.Rows[0]["Temp_AH"].ToString());
                Admin_Card.Temp_AI = Convert.ToDecimal(dt.Rows[0]["Temp_AI"].ToString());
                Admin_Card.Temp_AJ = Convert.ToDecimal(dt.Rows[0]["Temp_AJ"].ToString());
                Admin_Card.Temp_AL = dt.Rows[0]["Temp_AL"].ToString();
                Admin_Card.Temp_AM = dt.Rows[0]["Temp_AM"].ToString();
                Admin_Card.Temp_AN = dt.Rows[0]["Temp_AN"].ToString();
                Admin_Card.Temp_AO = dt.Rows[0]["Temp_AO"].ToString();
                Admin_Card.Temp_AP = dt.Rows[0]["Temp_AP"].ToString();
                Admin_Card.Temp_AQ = Convert.ToDecimal(dt.Rows[0]["Temp_AQ"].ToString());
                Admin_Card.Temp_AK = dt.Rows[0]["Temp_AK"].ToString();

            }
            else
            {
                Admin_Card = null;
            }
            return Admin_Card;

        }


        /// <summary>
        /// DayCount=每一年月的總天數
        /// </summary>
        /// <param name="OneYM"></param>
        /// <param name="YM_S"></param>
        /// <param name="YM_E"></param>
        /// <param name="assignYM"></param>
        /// <returns></returns>
        public DataTable GetAdmin_Card_byYM(string OneYM, string YM_S, string YM_E, string[] assignYM)
        {
            SqlCommand cmd = new SqlCommand();
            string sql = "SELECT DAY(EOMONTH(cast(cast(YM as int)+191100 as varchar)+'01')) AS DayCount,* FROM Admin_Card WHERE 1=1 ";
            if (!string.IsNullOrEmpty(OneYM))
            {
                sql += " and YM =@OneYM";
                cmd.Parameters.Add("@OneYM", SqlDbType.VarChar).Value = OneYM;
            }
            if (!string.IsNullOrEmpty(YM_S) && !string.IsNullOrEmpty(YM_E))
            {
                sql += " and YM BETWEEN @YM_S AND @YM_E";
                cmd.Parameters.Add("@YM_S", SqlDbType.VarChar).Value = YM_S;
                cmd.Parameters.Add("@YM_E", SqlDbType.VarChar).Value = YM_E;
            }
            if (assignYM != null)
            {
                if (assignYM.Length > 0)
                {
                    string sqlSub = " OR YM in (";
                    for (int i = 0; i < assignYM.Length; i++)
                    {
                        sqlSub += "@assignYM" + i.ToString() + ",";
                        cmd.Parameters.Add("@assignYM," + i, SqlDbType.VarChar).Value = assignYM[i];
                    }
                    sqlSub = sqlSub.Substring(0, sqlSub.Length - 1) + ")";

                    sql += sqlSub;
                }
            }
            sql += " order by YM,Branch_No";


            cmd.CommandText = sql;

            clsDB clsDB = new clsDB();
            DataTable dt = clsDB.OpenDataTable(cmd);


            return dt;
        }

        /// <summary>
        /// 
            /*========一、主要業務達成情形(累計平均餘額)======Start
            由每月資料計算，如1月(31天)月平均100、2月(28天)月平均150，則2月累計平均為(100*31+150*28)/(31+28)
            (累計平均)
            1.存款調整數Adrmin_Card_Count.Temp_E3=每月Adrmin_Card.Temp_AR平均
            2.一般性存款(總存) Adrmin_Card_Count.Temp_K1=每月Adrmin_Card_Count.Temp_Q平均
            3.農漁會存款合計活期性同業Adrmin_Card_Count.Temp_QOB344=每月Adrmin_Card.Temp_AT平均
            4.一般性存款(活存)Temp_AZ = 每月Adrmin_Card.Temp_R平均
            5.公庫活期Adrmin_Card_Count.Temp_AT(=OB240(系統計算))=每月Adrmin_Card.Temp_AS平均
            */
        /// </summary>
        /// <param name="OneYM">單一月份</param>
        /// <param name="YM_S">年月起 與YM_E合用</param>
        /// <param name="YM_E">年月迄 與YM_S合用</param>
        /// <param name="assignYM">使用or 年月陣列</param>
        /// <returns></returns>
        public DataTable calAdmin_Card_byYM(string OneYM, string YM_S, string YM_E, string[] assignYM)
        {
            SqlCommand cmd = new SqlCommand();
            string sql = @"
            select Branch_No,
            Round(SUM(DayCount*Temp_AR)/SUM(DayCount),0) as calTemp_AR,
            Round(SUM(DayCount*Temp_Q)/SUM(DayCount),0) as calTemp_Q ,
            Round(SUM(DayCount*Temp_AT)/SUM(DayCount),0) as calTemp_AT ,
            Round(SUM(DayCount*Temp_R)/SUM(DayCount),0) as calTemp_R ,
            Round(SUM(DayCount*Temp_AS)/SUM(DayCount),0) as calTemp_AS 
            FROM (
                SELECT DAY(EOMONTH(cast(cast(YM as int)+191100 as varchar)+'01')) AS DayCount,Branch_No,
                    Temp_AR,Temp_Q,Temp_AT,Temp_R,Temp_AS
                            FROM Admin_Card WHERE 1=1 ";
            if (!string.IsNullOrEmpty(OneYM))
            {
                sql += " and YM =@OneYM";
                cmd.Parameters.Add("@OneYM", SqlDbType.VarChar).Value = OneYM;
            }
            if (!string.IsNullOrEmpty(YM_S) && !string.IsNullOrEmpty(YM_E))
            {
                sql += " and YM BETWEEN @YM_S AND @YM_E";
                cmd.Parameters.Add("@YM_S", SqlDbType.VarChar).Value = YM_S;
                cmd.Parameters.Add("@YM_E", SqlDbType.VarChar).Value = YM_E;
            }
            if (assignYM != null)
            {
                if (assignYM.Length > 0)
                {
                    string sqlSub = " OR YM in (";
                    for (int i = 0; i < assignYM.Length; i++)
                    {
                        sqlSub += "@assignYM" + i.ToString() + ",";
                        cmd.Parameters.Add("@assignYM," + i, SqlDbType.VarChar).Value = assignYM[i];
                    }
                    sqlSub = sqlSub.Substring(0, sqlSub.Length - 1) + ")";

                    sql += sqlSub;
                }
            }
            sql += @" ) AS tmp
                            group by Branch_No
                            order by Branch_No
            ";


            cmd.CommandText = sql;

            clsDB clsDB = new clsDB();
            DataTable dt = clsDB.OpenDataTable(cmd);


            return dt;


        }
        private void insertAdmin_Card(string strYM, Common.clsEnum.EditMode enmEditMode)
        {
            Common.clsConvert clsConvert = new Common.clsConvert();
            YM = strYM;
            #region "進入Parameters 會進行運算的Property"
            Temp_D = Temp_D1 + Temp_AR;
            Temp_E = Temp_E1 + Temp_AS + Temp_E_OB344;
            Temp_Q = (Temp_D1 + Temp_AR) - Temp_Q_OB340;
            Temp_S = Temp_S1 + Math.Round(Temp_V_OB429C總計 / 1000 * Temp_AU, 0, MidpointRounding.AwayFromZero);
            #endregion
            string sql = "";
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            switch (enmEditMode)
            {
                case Common.clsEnum.EditMode.Insert:
                    sql = @"
                        INSERT INTO Admin_Card(YM,Branch_No,Temp_D, Temp_E, Temp_F, Temp_G, Temp_H, Temp_I, Temp_J, 
                        Temp_K, Temp_L, Temp_M, Temp_N, Temp_O, Temp_P, Temp_Q, Temp_R, Temp_S, 
                        Temp_V, Temp_X, Temp_Y, Temp_Z,Temp_AA, Temp_AB, Temp_AC, Temp_AD, 
                        Temp_AE, Temp_AF, Temp_AG, Temp_AH, Temp_AI, Temp_AM, Temp_AN, Temp_AO,
                        Temp_AP, Temp_AQ,Temp_AR,Temp_AS,Temp_AT,Temp_AU,Temp_AV) 
                         values(
                        @YM,@Branch_No,@Temp_D, @Temp_E, @Temp_F, @Temp_G, @Temp_H, @Temp_I, @Temp_J, 
                        @Temp_K, @Temp_L, @Temp_M, @Temp_N, @Temp_O, @Temp_P, @Temp_Q, @Temp_R, @Temp_S, 
                        @Temp_V, @Temp_X, @Temp_Y, @Temp_Z, @Temp_AA, @Temp_AB, @Temp_AC, @Temp_AD,
                        @Temp_AE, @Temp_AF, @Temp_AG, @Temp_AH, @Temp_AI, @Temp_AM, @Temp_AN, @Temp_AO,
                        @Temp_AP, @Temp_AQ,@Temp_AR,@Temp_AS,@Temp_AT,@Temp_AU,@Temp_AV) ";

                    cmd.Parameters.Add("@YM", SqlDbType.Decimal).Value = YM == null ? "" : YM;
                    cmd.Parameters.Add("@Branch_No", SqlDbType.Char).Value = Branch_No == null ? "" : Branch_No;
                    cmd.Parameters.Add("@Temp_D", SqlDbType.Decimal).Value = Temp_D;//總存款(不含外幣)=存款總額+存款調整數=Temp_D1+Temp_AR
                    cmd.Parameters.Add("@Temp_E", SqlDbType.Decimal).Value = Temp_E;// (活期性存款	DD104-NT-A-A(活期) ＋OB240＋OB344)
                    cmd.Parameters.Add("@Temp_F", SqlDbType.Decimal).Value = Temp_F1 + Temp_F2;//支票存款+本行支票
                    cmd.Parameters.Add("@Temp_G", SqlDbType.Decimal).Value = Temp_E1 + Temp_AS + Temp_E_OB344;//活期存款	DD104活期存款+OB240活期+OB344活期性同業	
                    cmd.Parameters.Add("@Temp_H", SqlDbType.Decimal).Value = Temp_H;
                    cmd.Parameters.Add("@Temp_I", SqlDbType.Decimal).Value = Temp_I1 + Temp_I2;//DD104-NT-A-09-A(支票存款-補償費 + 活期存款-補償費)
                    cmd.Parameters.Add("@Temp_J", SqlDbType.Decimal).Value = Temp_J;
                    cmd.Parameters.Add("@Temp_K", SqlDbType.Decimal).Value = Temp_K;
                    cmd.Parameters.Add("@Temp_L", SqlDbType.Decimal).Value = Temp_L1 + Temp_L_OB240;//定期性存款	DD104-NT-A-A(定期性存款)+OB240(定期性)
                    cmd.Parameters.Add("@Temp_M", SqlDbType.Decimal).Value = Temp_M1 + Temp_AR;//DD104-NT-A-A(一般存單)+調整數
                    cmd.Parameters.Add("@Temp_N", SqlDbType.Decimal).Value = Temp_N;
                    cmd.Parameters.Add("@Temp_O", SqlDbType.Decimal).Value = Temp_L - Temp_M - Temp_N;//Temp_L - Temp_M -Temp_N(定期性存款 - 定存(一般) -定存(CD) )
                    cmd.Parameters.Add("@Temp_P", SqlDbType.Decimal).Value = Temp_P;
                    cmd.Parameters.Add("@Temp_Q", SqlDbType.Decimal).Value = Temp_Q;//一般性-總存款 DD104-NT-A-A(總存款）－ OB340總計
                    cmd.Parameters.Add("@Temp_R", SqlDbType.Decimal).Value = Temp_R1 - Temp_R_OB340;//drM1.Temp_R=DD104-NT-A-09-A(活期性存款) － OB340活期
                    cmd.Parameters.Add("@Temp_S", SqlDbType.Decimal).Value = Temp_S;
                    //cmd.Parameters.Add("@Temp_T", SqlDbType.Decimal).Value = Temp_T;
                    //cmd.Parameters.Add("@Temp_U", SqlDbType.Decimal).Value = Temp_U;
                    cmd.Parameters.Add("@Temp_V", SqlDbType.Decimal).Value = Temp_V;
                    //cmd.Parameters.Add("@Temp_W", SqlDbType.Decimal).Value = Temp_W;
                    cmd.Parameters.Add("@Temp_X", SqlDbType.Decimal).Value = Temp_X;
                    cmd.Parameters.Add("@Temp_Y", SqlDbType.Decimal).Value = Math.Round(Temp_Y / 1000, 0, MidpointRounding.AwayFromZero);//列報管金額		DECIMAL(12, 0)
                    cmd.Parameters.Add("@Temp_Z", SqlDbType.VarChar).Value = Temp_Z.Substring(0, 2) + "." + Temp_Z.Substring(2, 2) + "%";//逾放比率 列報管比率		DECIMAL(4, 2)   75
                    cmd.Parameters.Add("@Temp_AA", SqlDbType.Decimal).Value = Temp_AA1 + Temp_AA2;//營業收入	DB201A-09(營業收入+營業外收入)
                    cmd.Parameters.Add("@Temp_AB", SqlDbType.Decimal).Value = Temp_AB;//DB201A-09(利息收入)
                    cmd.Parameters.Add("@Temp_AC", SqlDbType.Decimal).Value = Temp_AC;//DB201A-09(內部損益)
                    cmd.Parameters.Add("@Temp_AD", SqlDbType.Decimal).Value = Temp_AD1 + Temp_AD2;//DB201A-09(手續費收入+其他營業外收入)
                    cmd.Parameters.Add("@Temp_AE", SqlDbType.Decimal).Value = Temp_AE;//DB201A-09(利息費用)
                    cmd.Parameters.Add("@Temp_AF", SqlDbType.Decimal).Value = Temp_AF;//成本內部損益DB201A-09(內部損益)
                    cmd.Parameters.Add("@Temp_AG", SqlDbType.Decimal).Value = Math.Round(Temp_AG / 1000, 0, MidpointRounding.AwayFromZero);//列報金額		DECIMAL(12, 0)//其他費用	=C32-C33-C34	drM1.Temp_AG
                    cmd.Parameters.Add("@Temp_AH", SqlDbType.Decimal).Value = Math.Round(Temp_AH / 1000, 0, MidpointRounding.AwayFromZero);//列管金額		DECIMAL(12, 0)
                    cmd.Parameters.Add("@Temp_AI", SqlDbType.Decimal).Value = Temp_AI1 + Temp_AI2;//提存前盈餘(當月)=DB201A-09本期淨利(淨損)+各項提存
                                                                                                  //cmd.Parameters.Add("@Temp_AJ", SqlDbType.Decimal).Value = Temp_AJ;//特殊性損益 本項 表二註明刪除
                    cmd.Parameters.Add("@Temp_AK", SqlDbType.VarChar).Value = "0%";//INSERT為0，存款平均利率 會計處報表
                    cmd.Parameters.Add("@Temp_AL", SqlDbType.VarChar).Value = "0%";//INSERT為0，放款平均利率 會計處報表
                    cmd.Parameters.Add("@Temp_AM", SqlDbType.VarChar).Value = "0%";//INSERT為0，存放款利差=放款平均利率Temp_AL - 存款平均利率Temp_AK

                    cmd.Parameters.Add("@Temp_AN", SqlDbType.VarChar).Value = (Math.Round(Temp_E / Temp_D / 100, 2, MidpointRounding.AwayFromZero)).ToString("#0.00%");//活存比率	活存/總存(%)	drM1.Temp_AN
                    cmd.Parameters.Add("@Temp_AO", SqlDbType.VarChar).Value = (Math.Round(Temp_Q / Temp_D / 100, 2, MidpointRounding.AwayFromZero)).ToString("#0.00%");//(Temp_M / Temp_D).ToString() + "%";//一般存款比率	一般/總存(%)	drM1.Temp_AO
                    cmd.Parameters.Add("@Temp_AP", SqlDbType.VarChar).Value = (Math.Round(Temp_S / Temp_D / 100, 2, MidpointRounding.AwayFromZero)).ToString("#0.00%");//(Temp_S / Temp_D).ToString() + "%";//存放比率	自有資金放款/總存款(%)	drM1.Temp_AP
                    cmd.Parameters.Add("@Temp_AQ", SqlDbType.Decimal).Value = Temp_AS + Temp_AR;//drM1.Temp_AQ=OB240(活期性)+存款調整數=Temp_AS+Temp_AR(存款調整數)
                    cmd.Parameters.Add("@Temp_AR", SqlDbType.Decimal).Value = Temp_AR;//存款調整數
                    cmd.Parameters.Add("@Temp_AS", SqlDbType.Decimal).Value = Temp_AS;//OB240公庫活期
                    cmd.Parameters.Add("@Temp_AT", SqlDbType.Decimal).Value = Temp_AT;//OB344農漁會存款合計活期性同業
                    cmd.Parameters.Add("@Temp_AU", SqlDbType.Decimal).Value = Temp_AU;//匯率
                    cmd.Parameters.Add("@Temp_AV", SqlDbType.Decimal).Value = Math.Round(Temp_V_OB429C總計 / 1000 * Temp_AU, 0, MidpointRounding.AwayFromZero);//OB429C總計/1000*匯率
                    break;
                case Common.clsEnum.EditMode.Update:
                    sql = @"update Admin_Card set 
                            Temp_D=@Temp_D,Temp_E=@Temp_E,Temp_F=@Temp_F,Temp_G=@Temp_G,Temp_H=@Temp_H,
                            Temp_I=@Temp_I,Temp_J=@Temp_J,Temp_K=@Temp_K,Temp_L=@Temp_L,Temp_M=@Temp_M,
                            Temp_N=@Temp_N,Temp_O=@Temp_O,Temp_P=@Temp_P,Temp_Q=@Temp_Q,Temp_R=@Temp_R,
                            Temp_S=@Temp_S,Temp_V=@Temp_V,Temp_X=@Temp_X,Temp_Y=@Temp_Y,Temp_Z=@Temp_Z,
                            Temp_AA=@Temp_AA,Temp_AB=@Temp_AB,Temp_AC=@Temp_AC,Temp_AD=@Temp_AD,
                            Temp_AE=@Temp_AE,Temp_AF=@Temp_AF,Temp_AG=@Temp_AG,Temp_AH=@Temp_AH,
                            Temp_AI=@Temp_AI,Temp_AN=@Temp_AN,Temp_AO=@Temp_AO,
                            Temp_AP=@Temp_AP,Temp_AQ=@Temp_AQ,Temp_AR=@Temp_AR,Temp_AS=@Temp_AS,
                            Temp_AT=@Temp_AT,Temp_AU=@Temp_AU,Temp_AV=@Temp_AV
                            WHERE YM=@YM AND Branch_No=@Branch_No";

                    cmd.Parameters.Add("@YM", SqlDbType.Decimal).Value = YM == null ? "" : YM;
                    cmd.Parameters.Add("@Branch_No", SqlDbType.Char).Value = Branch_No == null ? "" : Branch_No;
                    cmd.Parameters.Add("@Temp_D", SqlDbType.Decimal).Value = Temp_D;//總存款(不含外幣)=存款總額+存款調整數=Temp_D1+Temp_AR
                    cmd.Parameters.Add("@Temp_E", SqlDbType.Decimal).Value = Temp_E;// (活期性存款	DD104-NT-A-A(活期) ＋OB240＋OB344)
                    cmd.Parameters.Add("@Temp_F", SqlDbType.Decimal).Value = Temp_F1 + Temp_F2;//支票存款+本行支票
                    cmd.Parameters.Add("@Temp_G", SqlDbType.Decimal).Value = Temp_E1 + Temp_AS + Temp_E_OB344;//活期存款	DD104活期存款+OB240活期+OB344活期性同業	
                    cmd.Parameters.Add("@Temp_H", SqlDbType.Decimal).Value = Temp_H;
                    cmd.Parameters.Add("@Temp_I", SqlDbType.Decimal).Value = Temp_I1 + Temp_I2;//DD104-NT-A-09-A(支票存款-補償費 + 活期存款-補償費)
                    cmd.Parameters.Add("@Temp_J", SqlDbType.Decimal).Value = Temp_J;
                    cmd.Parameters.Add("@Temp_K", SqlDbType.Decimal).Value = Temp_K;
                    cmd.Parameters.Add("@Temp_L", SqlDbType.Decimal).Value = Temp_L1 + Temp_L_OB240;//定期性存款	DD104-NT-A-A(定期性存款)+OB240(定期性)
                    cmd.Parameters.Add("@Temp_M", SqlDbType.Decimal).Value = Temp_M1 + Temp_AR;//DD104-NT-A-A(一般存單)+調整數
                    cmd.Parameters.Add("@Temp_N", SqlDbType.Decimal).Value = Temp_N;
                    cmd.Parameters.Add("@Temp_O", SqlDbType.Decimal).Value = Temp_L - Temp_M - Temp_N;//Temp_L - Temp_M -Temp_N(定期性存款 - 定存(一般) -定存(CD) )
                    cmd.Parameters.Add("@Temp_P", SqlDbType.Decimal).Value = Temp_P;
                    cmd.Parameters.Add("@Temp_Q", SqlDbType.Decimal).Value = Temp_Q;//一般性-總存款 DD104-NT-A-A(總存款）－ OB340總計
                    cmd.Parameters.Add("@Temp_R", SqlDbType.Decimal).Value = Temp_R1 - Temp_R_OB340;//drM1.Temp_R=DD104-NT-A-09-A(活期性存款) － OB340活期
                    cmd.Parameters.Add("@Temp_S", SqlDbType.Decimal).Value = Temp_S;
                    //cmd.Parameters.Add("@Temp_T", SqlDbType.Decimal).Value = Temp_T;
                    //cmd.Parameters.Add("@Temp_U", SqlDbType.Decimal).Value = Temp_U;
                    cmd.Parameters.Add("@Temp_V", SqlDbType.Decimal).Value = Temp_V;
                    //cmd.Parameters.Add("@Temp_W", SqlDbType.Decimal).Value = Temp_W;
                    cmd.Parameters.Add("@Temp_X", SqlDbType.Decimal).Value = Temp_X;
                    cmd.Parameters.Add("@Temp_Y", SqlDbType.Decimal).Value = Math.Round(Temp_Y / 1000, 0, MidpointRounding.AwayFromZero);//列報管金額		DECIMAL(12, 0)
                    cmd.Parameters.Add("@Temp_Z", SqlDbType.VarChar).Value = Temp_Z.Substring(0, 2) + "." + Temp_Z.Substring(2, 2) + "%";//逾放比率 列報管比率		DECIMAL(4, 2)   75
                    cmd.Parameters.Add("@Temp_AA", SqlDbType.Decimal).Value = Temp_AA1 + Temp_AA2;//營業收入	DB201A-09(營業收入+營業外收入)
                    cmd.Parameters.Add("@Temp_AB", SqlDbType.Decimal).Value = Temp_AB;//DB201A-09(利息收入)
                    cmd.Parameters.Add("@Temp_AC", SqlDbType.Decimal).Value = Temp_AC;//DB201A-09(內部損益)
                    cmd.Parameters.Add("@Temp_AD", SqlDbType.Decimal).Value = Temp_AD1 + Temp_AD2;//DB201A-09(手續費收入+其他營業外收入)
                    cmd.Parameters.Add("@Temp_AE", SqlDbType.Decimal).Value = Temp_AE;//DB201A-09(利息費用)
                    cmd.Parameters.Add("@Temp_AF", SqlDbType.Decimal).Value = Temp_AF;//成本內部損益DB201A-09(內部損益)
                    cmd.Parameters.Add("@Temp_AG", SqlDbType.Decimal).Value = Math.Round(Temp_AG / 1000, 0, MidpointRounding.AwayFromZero);//列報金額		DECIMAL(12, 0)//其他費用	=C32-C33-C34	drM1.Temp_AG
                    cmd.Parameters.Add("@Temp_AH", SqlDbType.Decimal).Value = Math.Round(Temp_AH / 1000, 0, MidpointRounding.AwayFromZero);//列管金額		DECIMAL(12, 0)
                    cmd.Parameters.Add("@Temp_AI", SqlDbType.Decimal).Value = Temp_AI1 + Temp_AI2;//提存前盈餘(當月)=DB201A-09本期淨利(淨損)+各項提存
                                                                                                  //cmd.Parameters.Add("@Temp_AJ", SqlDbType.Decimal).Value = Temp_AJ;//特殊性損益 本項 表二註明刪除
                    #region Temp_AK Temp_AL外部資料不update
                    //cmd.Parameters.Add("@Temp_AK", SqlDbType.VarChar).Value = Temp_AK==null ? "0%" : Temp_AK;//存款平均利率 會計處報表
                    //cmd.Parameters.Add("@Temp_AL", SqlDbType.VarChar).Value = Temp_AL == null ? "0%" : Temp_AL;//放款平均利率 會計處報表
                    //Temp_AM寫 trigger
                    //cmd.Parameters.Add("@Temp_AM", SqlDbType.VarChar).Value = (clsConvert.percentStringToDecimal(Temp_AL.ToString()) - clsConvert.percentStringToDecimal(Temp_AK.ToString())).ToString() + "%";//存放款利差=放款平均利率 - 存款平均利率
                    #endregion
                    cmd.Parameters.Add("@Temp_AN", SqlDbType.VarChar).Value = (Math.Round(Temp_E / Temp_D / 100, 2, MidpointRounding.AwayFromZero)).ToString("#0.00%");//活存比率	活存/總存(%)	drM1.Temp_AN
                    cmd.Parameters.Add("@Temp_AO", SqlDbType.VarChar).Value = (Math.Round(Temp_Q / Temp_D / 100, 2, MidpointRounding.AwayFromZero)).ToString("#0.00%");//(Temp_M / Temp_D).ToString() + "%";//一般存款比率	一般/總存(%)	drM1.Temp_AO
                    cmd.Parameters.Add("@Temp_AP", SqlDbType.VarChar).Value = (Math.Round(Temp_S / Temp_D / 100, 2, MidpointRounding.AwayFromZero)).ToString("#0.00%");//(Temp_S / Temp_D).ToString() + "%";//存放比率	自有資金放款/總存款(%)	drM1.Temp_AP
                    cmd.Parameters.Add("@Temp_AQ", SqlDbType.Decimal).Value = Temp_AS + Temp_AR;//drM1.Temp_AQ=OB240(活期性)+存款調整數=Temp_AS+Temp_AR(存款調整數)
                    cmd.Parameters.Add("@Temp_AR", SqlDbType.Decimal).Value = Temp_AR;//存款調整數
                    cmd.Parameters.Add("@Temp_AS", SqlDbType.Decimal).Value = Temp_AS;//公庫活期
                    cmd.Parameters.Add("@Temp_AT", SqlDbType.Decimal).Value = Temp_AT;//OB344農漁會存款合計活期性同業
                    cmd.Parameters.Add("@Temp_AU", SqlDbType.Decimal).Value = Temp_AU;//匯率
                    cmd.Parameters.Add("@Temp_AV", SqlDbType.Decimal).Value = Temp_V_OB429C總計 / 1000 * Temp_AU;//OB429C總計/1000*匯率

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

        /// <summary>
        /// 集合所有資料parser VALUE到Admin_Card
        /// </summary>
        /// <param name="parserBody"></param>
        public Tuple<string, int> parserProperty(string strYM, Dictionary<string, List<Tuple<string, Type, Object>>> parserBody)
        {
            int iCount = 0;
            try
            {
                DataTable dtChk = GetAdmin_Card_byYM(strYM, "", "", new string[] { });
                bool IsUpdate = true;

                foreach (string strBranch_No in parserBody.Keys)
                {
                    Type classType = typeof(Admin_Card);
                    List<Tuple<string, Type, Object>> parserValues = parserBody[strBranch_No];
                    foreach (var UnitData in parserValues)
                    {
                        PropertyInfo classProperty = classType.GetProperty(UnitData.Item1);
                        classProperty.SetValue(this, UnitData.Item3, null);
                    }
                    //若有該筆單位資料為TRUE， 否則為FALSE
                    IsUpdate = dtChk.Select("Branch_No='" + strBranch_No + "'").Length > 0;
                 
                    insertAdmin_Card(strYM, IsUpdate==true ? Common.clsEnum.EditMode.Update : Common.clsEnum.EditMode.Insert);
                    iCount += 1;
                }
                return new Tuple<string, int>("轉入Admin_Card作業完成!，共匯入" + iCount.ToString() + "筆。", iCount);
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
            string strTableName = "Admin_Card";
            int iCount = 0;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();


            string sql = @"delete from Admin_Card_old where SUBSTRING(YM,1,3)=@YYY;
                        INSERT INTO Admin_Card_old 
                        SELECT * FROM Admin_Card WHERE  SUBSTRING(YM,1,3)=@YYY
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
                iCount=cmd.ExecuteNonQuery();

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
            return new Tuple<string, int>(strTableName+ "移轉" + strYYY + "年資料共" + iCount.ToString("#,##0")+"筆。" + Environment.NewLine, iCount);
        }


        /// <summary>
        /// 自動三年歷史資料搬移DB ，113年刪除104年
        /// </summary>
        /// <param name="strYM">要被移轉的年份</param>
        public Tuple<string, int> deleteOldData(string strYYY)
        {
            string strTableName = "Admin_Card";
            int iCount = 0;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();


            string sql = @"delete from Admin_Card where SUBSTRING(YM,1,3)=@YYY";

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

