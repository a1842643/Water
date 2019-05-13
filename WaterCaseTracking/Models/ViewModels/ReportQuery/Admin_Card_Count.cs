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
    class Admin_Card_Count
    {
        /*
        以下欄位，在表一產表時，直接SELECT，本批次不處理
        ========上年度達成額(代入 去年12月資料)======
        drM4C.Temp_G=Temp_E去年12月資料
        drM4C.Temp_L=Temp_K去年12月資料
        drM4C.Temp_R=Temp_Q 去年12月資料
        drM4C.Temp_AZ=Temp_AZ  去年12月資料
        drM4C.Temp_AU=Temp_AU  去年12月資料
        drM4C.Temp_AX=Temp_AX  去年12月資料
        drM4C.Temp_AT=Temp_AT  去年12月資料
        drM4C.Temp_X=Temp_X    去年12月資料
        drM4C.Temp_BB((欄位有改，不用代入舊資料))
        drM4C.Temp_BA((欄位有改，不用代入舊資料))
         */
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

        #region "匯入運算用欄位"
        #region "一、主要業務達成情形(累計平均餘額)"
        /// <summary>
        /// ====以下所屬項目：總存款(不含外幣)====
        /// Temp_E1=存款總額
        /// 資料來源：DD104_NT_A_09_B
        /// Temp_E=總存款(不含外幣) Temp_E:DD104-NT-A-09-B (存款總額+銀行同業存款)+存款調整數
        /// </summary>
        public decimal Temp_E1 { get; set; }
        /// <summary>
        /// Temp_E2=銀行同業存款
        /// 資料來源：DD104_NT_A_09_B
        /// Temp_E=總存款(不含外幣) Temp_E:DD104-NT-A-09-B (存款總額+銀行同業存款)+存款調整數
        /// </summary>
        public decimal Temp_E2 { get; set; }
        /// <summary>
        /// Temp_E3=累計平均存款調整數【從人工上傳報表(業務部-存款調整數.xls)後，每月系統計算(=每月Adrmin_Card.Temp_AR)，
        /// Temp_E3的資料，待INSERT INTO Admin_Card後，再一次取得每月(Admin_Card：Temp_AR<存款調整數>)並計算
        /// 如1月(31天)月平均100、2月(28天)月平均150，則2月累計平均為(100*31+150*28)/(31+28)】
        /// Temp_E=總存款(不含外幣) Temp_E:DD104-NT-A-09-B (存款總額+銀行同業存款)+存款調整數
        /// </summary>
        public decimal Temp_E3 { get; set; }

        /// <summary>
        /// ???
        /// Temp_F1:分配目標數
        /// 總存款(不含外幣) 達成率=本月止達成(Temp_E)／分配目標數
        /// </summary>
        public decimal Temp_F1 { get; set; }
        /// <summary>
        /// ====以下所屬項目：活期性存款====
        /// Temp_Q1=DD104-NT-A-09-B(活期）        
        /// Temp_Q=【DD104-NT-A-09-B(活期）＋ OB240(系統計算)＋ OB344(累計)+存款調整數】
        /// </summary>
        public decimal Temp_Q1 { get; set; }
        /// <summary>
        /// Temp_QOB240=OB344(累計)    OB344一樣是月平均報表，所以要抓農漁會存款合計活期性同業數字，一樣每月算累計平均餘額
        /// Temp_Q=【DD104-NT-A-09-B(活期）＋ OB240(系統計算)＋ OB344(累計)+存款調整數】
        /// </summary>
        public decimal Temp_QOB344 { get; set; }
        /// <summary>
        /// Temp_AU1 基金=DD104-NT-A-09-B(活期存款-基金)
        /// </summary>
        public decimal Temp_AU1 { get; set; }
        /// <summary>
        /// Temp_AX1=支票存款-補償費
        /// 補償金=DD104-NT-A-09-B(支票存款-補償費 + 活期存款-補償費)
        /// </summary>
        public decimal Temp_AX1 { get; set; }
        /// <summary>
        /// Temp_AX2=活期存款-補償費
        /// 補償金=DD104-NT-A-09-B(支票存款-補償費 + 活期存款-補償費)
        /// </summary>
        public decimal Temp_AX2 { get; set; }
        /// <summary>
        /// Temp_X1=DD104-NT-B(P3.放款總額-自有資金)
        /// 自有資金(含外幣)Temp_X=【DD104-NT-B(P3.放款總額-自有資金) + C16外幣放款】
        /// </summary>
        public decimal Temp_X1 { get; set; }
        /// <summary>
        /// Temp_OB429D總計=外幣放款
        /// OB429D總計/1000*匯率，匯率畫面輸入
        /// </summary>
        public decimal Temp_OB429D總計 { get; set; }

        /// <summary>
        /// 外匯  單位:千美元 抓取OB436 總計金額	drM1C.Temp_AK
        /// </summary>
        public decimal Temp_AK1 { get; set; }
        /// <summary>
        /// Temp_AN1=DB201A(本期淨利
        /// 提存前盈餘		DB201A-09(本期淨利+各項提存)	本期淨利=本期淨利（淨損）		drM1C.Temp_AN
        /// </summary>
        public decimal Temp_AN1 { get; set; }
        /// <summary>
        /// Temp_AN1=DB201A(各項提存
        /// 提存前盈餘		DB201A-09(本期淨利+各項提存)	本期淨利=本期淨利（淨損）		drM1C.Temp_AN
        /// </summary>
        public decimal Temp_AN2 { get; set; }

        /// <summary>
        /// Temp_AN1=DD108H(員工人數合計)
        /// 員工人數		DD108H(員工人數合計)	drM1C.Temp_AP
        /// </summary>
        public decimal Temp_AP1 { get; set; }
        /// <summary>
        /// Temp_AI1=逾放比率
        /// 逾放比率	LN705_BRNO_YYMM.txt	資料格式.TXT 逾放比率數值在 列報管比率  DECIMAL(4, 2) Temp_AI
        /// </summary>
        public decimal Temp_AI1 { get; set; }
        #endregion

        #endregion

        #region "property"
        private string mYM;
        private string mBranch_No;
        private decimal mTemp_D;
        private decimal mTemp_E;
        private string mTemp_F;
        private decimal mTemp_G;
        private string mTemp_H;
        private string mTemp_I;
        private decimal mTemp_J;
        private decimal mTemp_K;
        private decimal mTemp_L;
        private decimal mTemp_M;
        private string mTemp_N;
        private string mTemp_O;
        private decimal mTemp_P;
        private decimal mTemp_Q;
        private decimal mTemp_R;
        private string mTemp_S;
        private string mTemp_T;
        private string mTemp_U;
        private decimal mTemp_V;
        private decimal mTemp_W;
        private decimal mTemp_X;
        private decimal mTemp_Y;
        private string mTemp_Z;
        private string mTemp_AA;
        private string mTemp_AB;
        private string mTemp_AC;
        private decimal mTemp_AD;
        private decimal mTemp_AE;
        private decimal mTemp_AF;
        private decimal mTemp_AG;
        private decimal mTemp_AH;
        private string mTemp_AI;
        private decimal mTemp_AJ;
        private decimal mTemp_AK;
        private string mTemp_AL;
        private decimal mTemp_AM;
        private decimal mTemp_AN;
        private string mTemp_AO;
        private decimal mTemp_AP;
        private decimal mTemp_AQ;
        private decimal mTemp_AR;
        private string mTemp_AS;
        private decimal mTemp_AT;
        private decimal mTemp_AU;
        private decimal mTemp_AV;
        private decimal mTemp_AW;
        private decimal mTemp_AX;
        private decimal mTemp_AY;
        private decimal mTemp_AZ;
        private decimal mTemp_BA;
        private decimal mTemp_BB;
        private decimal mTemp_BC;

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
        public string Temp_F
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

        /// <summary>
        /// 上年度達成額:總存款(不含外幣)
        /// </summary>
        public decimal lastY12Temp_G { get; set; }
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

        public string Temp_H
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

        public string Temp_I
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

        /// <summary>
        /// Temp_K:(總存)一般性存款 由每月資料計算
        /// 本月一般性存款(總存款(不含外幣)/一般性存款) ==>【二、主要最近三個月狀況(月平均餘額) =drM1.Temp_Q
        /// 資料來源：每月系統計算(ComputeDD104_NT_A_09_B)
        /// 如1月(31天)月平均100、2月(28天)月平均150，則2月累計平均Temp_K為(100*31+150*28)/(31+28)】
        /// Temp_K=總存款(不含外幣)/一般性存款
        /// </summary>
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
        //上年度達成額:一般性存款(總存) 
        public decimal lastY12Temp_L { get; set; }
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

        public string Temp_N
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

        public string Temp_O
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

        /// <summary>
        /// 上年度達成額:活期性存款
        /// </summary>
        public decimal lastY12Temp_R { get; set; }
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

        public string Temp_S
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

        public string Temp_T
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

        public string Temp_U
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

        public string Temp_AA
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

        public string Temp_AB
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

        public string Temp_AC
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
        /// <summary>
        /// 逾期放款
        /// </summary>
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

        public string Temp_AI
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

        public decimal Temp_AK
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

        public decimal Temp_AM
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

        public decimal Temp_AN
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

        public decimal Temp_AP
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

        public string Temp_AS
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
        /// Temp_AT1=活期性存款 公庫活期【依每月OB240另以檔案計算，
        /// 如1月(31天)月平均100、2月(28天)月平均150，則2月累計平均為(100*31+150*28)/31+28】
        /// 註記:公庫活期
        /// 有些累計平均資料要從月平均資料來計算
        /// 例如一般性存款：
        /// 1月月平均10 天數31天
        /// 2月月平均20 天數28天
        /// 則2月累計平均為　10*31＋20*28 ／ (31+28)
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

        public decimal Temp_AU
        {
            get
            {
                return mTemp_AU;
            }

            set
            {
                mTemp_AU = value;
            }
        }

        public decimal Temp_AV
        {
            get
            {
                return mTemp_AV;
            }

            set
            {
                mTemp_AV = value;
            }
        }

        public decimal Temp_AW
        {
            get
            {
                return mTemp_AW;
            }

            set
            {
                mTemp_AW = value;
            }
        }

        public decimal Temp_AX
        {
            get
            {
                return mTemp_AX;
            }

            set
            {
                mTemp_AX = value;
            }
        }

        public decimal Temp_AY
        {
            get
            {
                return mTemp_AY;
            }

            set
            {
                mTemp_AY = value;
            }
        }
        /// <summary>
        /// (活存)一般性存款 由每月資料計算
        /// </summary>
        public decimal Temp_AZ
        {
            get
            {
                return mTemp_AZ;
            }

            set
            {
                mTemp_AZ = value;
            }
        }

        public decimal Temp_BA
        {
            get
            {
                return mTemp_BA;
            }

            set
            {
                mTemp_BA = value;
            }
        }

        public decimal Temp_BB
        {
            get
            {
                return mTemp_BB;
            }

            set
            {
                mTemp_BB = value;
            }
        }

        public decimal Temp_BC
        {
            get
            {
                return mTemp_BC;
            }

            set
            {
                mTemp_BC = value;
            }

        }
        /// <summary>
        /// 外幣放款
        /// 匯率畫面輸入
        /// </summary>
        public decimal Temp_BD
        {
            get;
            set;
        }
        /// <summary>
        /// 外幣放款=OB429D總計/1000*Temp_BD
        /// </summary>
        public decimal Temp_BE
        {
            get;
            set;
        }

        #endregion
        public Admin_Card_Count getAdmin_Card_Count(string YM, string Branch_No)
        {
            Admin_Card_Count Admin_Card = new Admin_Card_Count();
            // Dim sql As String = "SELECT  UNT_UNITNAME as UNITNAME,EMP_EXTENDADD,* FROM SALMMPR "
            string sql = @"SELECT YM, Temp_D, Temp_E, Temp_F, Temp_G, Temp_H, Temp_I, Temp_J, Temp_K, Temp_L, Temp_M, Temp_N, " +
                            "Temp_O, Temp_P, Temp_Q, Temp_R, Temp_S, Temp_T, Temp_U, Temp_V, Temp_W, Temp_X, Temp_Y, Temp_Z, " +
                            "Temp_AA, Temp_AB, Temp_AC, Temp_AD, Temp_AE, Temp_AF, Temp_AG, Temp_AH, Temp_AI, Temp_AJ, " +
                            "Temp_AK, Temp_AL, Temp_AM, Temp_AN, Temp_AO, Temp_AP, Temp_AQ, Temp_AR, Temp_AS, Temp_AT, " +
                            "Temp_AU, Temp_AV, Temp_AW, Temp_AX, Temp_AY, Temp_AZ, Temp_BA, Temp_BB, Temp_BC " +
                    "FROM Admin_Card_Count " +
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
                Admin_Card.Temp_F = dt.Rows[0]["Temp_F"].ToString();
                Admin_Card.Temp_G = Convert.ToDecimal(dt.Rows[0]["Temp_G"].ToString());
                Admin_Card.Temp_H = dt.Rows[0]["Temp_H"].ToString();
                Admin_Card.Temp_I = dt.Rows[0]["Temp_I"].ToString();
                Admin_Card.Temp_J = Convert.ToDecimal(dt.Rows[0]["Temp_J"].ToString());
                Admin_Card.Temp_K = Convert.ToDecimal(dt.Rows[0]["Temp_K"].ToString());
                Admin_Card.Temp_L = Convert.ToDecimal(dt.Rows[0]["Temp_L"].ToString());
                Admin_Card.Temp_M = Convert.ToDecimal(dt.Rows[0]["Temp_M"].ToString());
                Admin_Card.Temp_N = dt.Rows[0]["Temp_N"].ToString();
                Admin_Card.Temp_O = dt.Rows[0]["Temp_O"].ToString();
                Admin_Card.Temp_P = Convert.ToDecimal(dt.Rows[0]["Temp_P"].ToString());
                Admin_Card.Temp_Q = Convert.ToDecimal(dt.Rows[0]["Temp_Q"].ToString());
                Admin_Card.Temp_R = Convert.ToDecimal(dt.Rows[0]["Temp_R"].ToString());
                Admin_Card.Temp_S = dt.Rows[0]["Temp_S"].ToString();
                Admin_Card.Temp_T = dt.Rows[0]["Temp_T"].ToString();
                Admin_Card.Temp_U = dt.Rows[0]["Temp_U"].ToString();
                Admin_Card.Temp_V = Convert.ToDecimal(dt.Rows[0]["Temp_V"].ToString());
                Admin_Card.Temp_W = Convert.ToDecimal(dt.Rows[0]["Temp_W"].ToString());
                Admin_Card.Temp_X = Convert.ToDecimal(dt.Rows[0]["Temp_X"].ToString());
                Admin_Card.Temp_Y = Convert.ToDecimal(dt.Rows[0]["Temp_Y"].ToString());
                Admin_Card.Temp_Z = dt.Rows[0]["Temp_Z"].ToString();
                Admin_Card.Temp_AA = dt.Rows[0]["Temp_AA"].ToString();
                Admin_Card.Temp_AB = dt.Rows[0]["Temp_AB"].ToString();
                Admin_Card.Temp_AC = dt.Rows[0]["Temp_AC"].ToString();
                Admin_Card.Temp_AD = Convert.ToDecimal(dt.Rows[0]["Temp_AD"].ToString());
                Admin_Card.Temp_AE = Convert.ToDecimal(dt.Rows[0]["Temp_AE"].ToString());
                Admin_Card.Temp_AF = Convert.ToDecimal(dt.Rows[0]["Temp_AF"].ToString());
                Admin_Card.Temp_AG = Convert.ToDecimal(dt.Rows[0]["Temp_AG"].ToString());
                Admin_Card.Temp_AH = Convert.ToDecimal(dt.Rows[0]["Temp_AH"].ToString());
                Admin_Card.Temp_AI = dt.Rows[0]["Temp_AI"].ToString();
                Admin_Card.Temp_AJ = Convert.ToDecimal(dt.Rows[0]["Temp_AJ"].ToString());
                Admin_Card.Temp_AL = dt.Rows[0]["Temp_AL"].ToString();
                Admin_Card.Temp_AM = Convert.ToDecimal(dt.Rows[0]["Temp_AM"].ToString());
                Admin_Card.Temp_AN = Convert.ToDecimal(dt.Rows[0]["Temp_AN"].ToString());
                Admin_Card.Temp_AO = dt.Rows[0]["Temp_AO"].ToString();
                Admin_Card.Temp_AP = Convert.ToDecimal(dt.Rows[0]["Temp_AP"].ToString());
                Admin_Card.Temp_AQ = Convert.ToDecimal(dt.Rows[0]["Temp_AQ"].ToString());
                Admin_Card.Temp_AR = Convert.ToDecimal(dt.Rows[0]["Temp_AR"].ToString());
                Admin_Card.Temp_AS = dt.Rows[0]["Temp_AS"].ToString();
                Admin_Card.Temp_AT = Convert.ToDecimal(dt.Rows[0]["Temp_AT"].ToString());
                Admin_Card.Temp_AU = Convert.ToDecimal(dt.Rows[0]["Temp_AU"].ToString());
                Admin_Card.Temp_AV = Convert.ToDecimal(dt.Rows[0]["Temp_AV"].ToString());
                Admin_Card.Temp_AW = Convert.ToDecimal(dt.Rows[0]["Temp_AW"].ToString());
                Admin_Card.Temp_AK = Convert.ToDecimal(dt.Rows[0]["Temp_AK"].ToString());

                Admin_Card.Temp_AY = Convert.ToDecimal(dt.Rows[0]["Temp_AY"].ToString());
                Admin_Card.Temp_AZ = Convert.ToDecimal(dt.Rows[0]["Temp_AZ"].ToString());
                Admin_Card.Temp_BA = Convert.ToDecimal(dt.Rows[0]["Temp_BA"].ToString());
                Admin_Card.Temp_BB = Convert.ToDecimal(dt.Rows[0]["Temp_BB"].ToString());
                Admin_Card.Temp_BC = Convert.ToDecimal(dt.Rows[0]["Temp_BC"].ToString());

            }
            else
                Admin_Card = null;

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
        public DataTable GetAdmin_Card_Count_byYM(string OneYM, string YM_S, string YM_E, string[] assignYM)
        {
            SqlCommand cmd = new SqlCommand();
            string sql = "SELECT * FROM Admin_Card_Count WHERE 1=1 ";
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
        public void insertAdmin_Card_Count(string YM, Common.clsEnum.EditMode enmEditMode)
        {
            #region "進入Parameters 會進行運算的Property"
            //自有資金(含外幣) Temp_X
            Temp_X = Temp_X1 + Math.Round(Temp_OB429D總計 / 1000 * Temp_BD, 0, MidpointRounding.AwayFromZero);
            Temp_E = (Temp_E1 + Temp_E2 + Temp_E3);
            Temp_Q = Temp_Q1 + Temp_AT + Temp_QOB344 + Temp_E3;
            #endregion
            string sql = "";
            SqlCommand cmd = new SqlCommand();
            SqlConnection cn = new SqlConnection();

            switch (enmEditMode)
            {
                case Common.clsEnum.EditMode.Insert:
                    sql = @"INSERT INTO Admin_Card_Count(
                        YM,Branch_No,Temp_D,Temp_E,Temp_F,Temp_G,Temp_H,Temp_K,Temp_L,
                        Temp_N,Temp_O,Temp_Q,Temp_R,Temp_T,Temp_U,Temp_X,Temp_Y,
                        Temp_AA,Temp_AB,Temp_AD,Temp_AE,Temp_AF,Temp_AG,Temp_AH,Temp_AI,
                        Temp_AJ,Temp_AK,Temp_AL,Temp_AM,Temp_AN,Temp_AO,Temp_AP,Temp_AQ,Temp_AR,Temp_AS,
                        Temp_AT,Temp_AU,Temp_AV,Temp_AW,Temp_AX,Temp_AY,Temp_AZ,Temp_BA,Temp_BB,Temp_BC,
                        Temp_BD,Temp_BE)
                         values(
                        @YM, @Branch_No, @Temp_D, @Temp_E, @Temp_F, @Temp_G, @Temp_H, @Temp_K, @Temp_L,
                        @Temp_N, @Temp_O, @Temp_Q, @Temp_R, @Temp_T, @Temp_U, @Temp_X, @Temp_Y,
                        @Temp_AA, @Temp_AB, @Temp_AD, @Temp_AE, @Temp_AF, @Temp_AG, @Temp_AH, @Temp_AI,
                        @Temp_AJ, @Temp_AK, @Temp_AL, @Temp_AM, @Temp_AN, @Temp_AO, @Temp_AP, @Temp_AQ, @Temp_AR, @Temp_AS,
                        @Temp_AT, @Temp_AU, @Temp_AV, @Temp_AW, @Temp_AX, @Temp_AY, @Temp_AZ, @Temp_BA, @Temp_BB, @Temp_BC, 
                        @Temp_BD,@Temp_BE)";
                    break;
                case Common.clsEnum.EditMode.Update:
                    sql = @"update Admin_Card_Count set 
                            Temp_D=@Temp_D,
                            Temp_E=@Temp_E,
                            Temp_F=@Temp_F,
                            Temp_G=@Temp_G,
                            Temp_H=@Temp_H,
                            Temp_K=@Temp_K,
                            Temp_L=@Temp_L,
                            Temp_N=@Temp_N,
                            Temp_O=@Temp_O,
                            Temp_Q=@Temp_Q,
                            Temp_R=@Temp_R,
                            Temp_T=@Temp_T,
                            Temp_U=@Temp_U,
                            Temp_X=@Temp_X,
                            Temp_Y=@Temp_Y,
                            Temp_AA=@Temp_AA,
                            Temp_AB=@Temp_AB,
                            Temp_AD=@Temp_AD,
                            Temp_AE=@Temp_AE,
                            Temp_AF=@Temp_AF,
                            Temp_AG=@Temp_AG,
                            Temp_AH=@Temp_AH,
                            Temp_AI=@Temp_AI,
                            Temp_AJ=@Temp_AJ,
                            Temp_AK=@Temp_AK,
                            Temp_AL=@Temp_AL,
                            Temp_AM=@Temp_AM,
                            Temp_AN=@Temp_AN,
                            Temp_AO=@Temp_AO,
                            Temp_AP=@Temp_AP,
                            Temp_AQ=@Temp_AQ,
                            Temp_AR=@Temp_AR,
                            Temp_AS=@Temp_AS,
                            Temp_AT=@Temp_AT,
                            Temp_AU=@Temp_AU,
                            Temp_AV=@Temp_AV,
                            Temp_AW=@Temp_AW,
                            Temp_AX=@Temp_AX,
                            Temp_AY=@Temp_AY,
                            Temp_AZ=@Temp_AZ,
                            Temp_BA=@Temp_BA,
                            Temp_BB=@Temp_BB,
                            Temp_BC=@Temp_BC,
                            Temp_BD=@Temp_BD,
                            Temp_BE=@Temp_BE
                            WHERE YM=@YM
                            AND Branch_No=@Branch_No";

                    break;
            }
            cmd.Parameters.Add("@YM", SqlDbType.VarChar).Value = YM;
            cmd.Parameters.Add("@Branch_No", SqlDbType.VarChar).Value = Branch_No;
            cmd.Parameters.Add("@Temp_D", SqlDbType.Decimal).Value = Temp_D;//表3
                                                                            //總存款(不含外幣) Temp_E:DD104-NT-A-09-B (存款總額+銀行同業存款)+存款調整數
            cmd.Parameters.Add("@Temp_E", SqlDbType.Decimal).Value = Temp_E;
            if (Temp_F1 != 0)
                cmd.Parameters.Add("@Temp_F", SqlDbType.VarChar).Value = Math.Round(Temp_E / Temp_F1 * 100, 2, MidpointRounding.AwayFromZero).ToString("#0.00%");//本月止達成／分配目標數=(Temp_E1+ Temp_E2+ Temp_E3)/F1
            else cmd.Parameters.Add("@Temp_F", SqlDbType.VarChar).Value = "0%";
            //自編報表(會計處-平均利率彙整表)
            //cmd.Parameters.Add("@Temp_I", SqlDbType.VarChar).Value = Temp_I;
            //cmd.Parameters.Add("@Temp_J", SqlDbType.Decimal).Value = Temp_J;//表一表三 找不到該欄對應
            //cmd.Parameters.Add("@Temp_M", SqlDbType.Decimal).Value = Temp_M;//表一表三 找不到該欄對應
            //cmd.Parameters.Add("@Temp_Z", SqlDbType.VarChar).Value = Temp_Z;//表一表三 找不到該欄對應
            //表二 自編報表(業務部-目標分配)
            //cmd.Parameters.Add("@Temp_P", SqlDbType.Decimal).Value = Temp_P;
            //表一 本月止達成／分配目標數
            //cmd.Parameters.Add("@Temp_S", SqlDbType.VarChar).Value = Temp_S;
            //自編報表(會計處-平均利率彙整表)
            //cmd.Parameters.Add("@Temp_V", SqlDbType.Decimal).Value = Temp_V;
            //自編報表(業務部-目標分配)
            //cmd.Parameters.Add("@Temp_W", SqlDbType.Decimal).Value = Temp_W;
            //自編報表(會計處-平均利率彙整表)
            //cmd.Parameters.Add("@Temp_AC", SqlDbType.VarChar).Value = Temp_AC;
            cmd.Parameters.Add("@Temp_G", SqlDbType.Decimal).Value = Temp_G;
            cmd.Parameters.Add("@Temp_H", SqlDbType.VarChar).Value = Math.Round((Temp_E - Temp_G) / Temp_G, 2, MidpointRounding.AwayFromZero).ToString("#0.00%");//((Temp_G/ Temp_F)*100).to;
            cmd.Parameters.Add("@Temp_K", SqlDbType.Decimal).Value = Temp_K;//(總存)一般性存款 由每月資料計算
            cmd.Parameters.Add("@Temp_L", SqlDbType.Decimal).Value = Temp_L;
            cmd.Parameters.Add("@Temp_N", SqlDbType.VarChar).Value = Math.Round((Temp_K - Temp_L) / Temp_L, 2, MidpointRounding.AwayFromZero).ToString("#0.00%");

            //一般存款比率=drM1C.Temp_K/drM1C.Temp_E
            cmd.Parameters.Add("@Temp_O", SqlDbType.VarChar).Value = Math.Round(Temp_K / Temp_E, 2, MidpointRounding.AwayFromZero).ToString("#0.00%");

            // Temp_Q=【DD104-NT-A-09-B(活期）＋ OB240(系統計算)＋ OB344(累計)+存款調整數】
            cmd.Parameters.Add("@Temp_Q", SqlDbType.Decimal).Value = Temp_Q;
            cmd.Parameters.Add("@Temp_R", SqlDbType.Decimal).Value = Temp_R;
            cmd.Parameters.Add("@Temp_T", SqlDbType.VarChar).Value = Math.Round((Temp_Q - Temp_R) / Temp_R, 2, MidpointRounding.AwayFromZero).ToString("#0.00%");
            //活存比率=drM1C.Temp_Q/drM1C.Temp_E
            cmd.Parameters.Add("@Temp_U", SqlDbType.VarChar).Value = Math.Round(Temp_Q / Temp_E, 2, MidpointRounding.AwayFromZero).ToString("#0.00%");
            cmd.Parameters.Add("@Temp_X", SqlDbType.Decimal).Value = Temp_X;
            cmd.Parameters.Add("@Temp_Y", SqlDbType.Decimal).Value = Temp_Y;//???表三 放款 ==>自有資金 ==>上年度平均餘額

            cmd.Parameters.Add("@Temp_AA", SqlDbType.VarChar).Value = Temp_AA == null ? "" : Temp_AA;//???表三 放款 ==>自有資金 ==>(4)成長率
                                                                                                     //存放比率=drM1C.Temp_X/drM1C.Temp_E
            cmd.Parameters.Add("@Temp_AB", SqlDbType.VarChar).Value = Math.Round(Temp_X / Temp_E, 2, MidpointRounding.AwayFromZero).ToString("#0.00%");
            cmd.Parameters.Add("@Temp_AD", SqlDbType.Decimal).Value = Temp_AD;//???表三 放款 ==>應收代放款 DD104-FRN-B-D
            cmd.Parameters.Add("@Temp_AE", SqlDbType.Decimal).Value = Temp_X + Temp_AD;//(2)平均餘額Temp_X+應收代放款Temp_AD
            cmd.Parameters.Add("@Temp_AF", SqlDbType.Decimal).Value = Temp_AF;
            cmd.Parameters.Add("@Temp_AG", SqlDbType.Decimal).Value = Temp_AG;
            cmd.Parameters.Add("@Temp_AH", SqlDbType.Decimal).Value = Temp_AH;
            cmd.Parameters.Add("@Temp_AI", SqlDbType.VarChar).Value = Temp_AI == null ? "" : Temp_AI;
            cmd.Parameters.Add("@Temp_AJ", SqlDbType.Decimal).Value = Temp_AJ;
            cmd.Parameters.Add("@Temp_AK", SqlDbType.Decimal).Value = Temp_AK;
            cmd.Parameters.Add("@Temp_AL", SqlDbType.VarChar).Value = Temp_AL == null ? "" : Temp_AL;
            cmd.Parameters.Add("@Temp_AM", SqlDbType.Decimal).Value = Temp_AM;
            //提存前盈餘		DB201A-09(本期淨利+各項提存)
            cmd.Parameters.Add("@Temp_AN", SqlDbType.Decimal).Value = Temp_AN1 + Temp_AN2;
            cmd.Parameters.Add("@Temp_AO", SqlDbType.VarChar).Value = Temp_AO == null ? "" : Temp_AO;
            cmd.Parameters.Add("@Temp_AP", SqlDbType.Decimal).Value = Temp_AP;
            cmd.Parameters.Add("@Temp_AQ", SqlDbType.Decimal).Value = Temp_AQ;
            cmd.Parameters.Add("@Temp_AR", SqlDbType.Decimal).Value = Temp_AR;
            cmd.Parameters.Add("@Temp_AS", SqlDbType.VarChar).Value = Temp_AS == null ? "" : Temp_AS;
            cmd.Parameters.Add("@Temp_AT", SqlDbType.Decimal).Value = Temp_AT;//公庫活期 依每月OB240另以檔案計算
            cmd.Parameters.Add("@Temp_AU", SqlDbType.Decimal).Value = Temp_AU;
            cmd.Parameters.Add("@Temp_AV", SqlDbType.Decimal).Value = Temp_AV;
            cmd.Parameters.Add("@Temp_AW", SqlDbType.Decimal).Value = Temp_AW;

            /// 補償金=DD104-NT-A-09-B(支票存款-補償費 + 活期存款-補償費)
            cmd.Parameters.Add("@Temp_AX", SqlDbType.Decimal).Value = Temp_AX1 + Temp_AX2;
            cmd.Parameters.Add("@Temp_AY", SqlDbType.Decimal).Value = Temp_AY;
            cmd.Parameters.Add("@Temp_AZ", SqlDbType.Decimal).Value = Temp_AZ;//(活存)一般性存款 由每月資料計算
            cmd.Parameters.Add("@Temp_BA", SqlDbType.Decimal).Value = Temp_BA;
            cmd.Parameters.Add("@Temp_BB", SqlDbType.Decimal).Value = Temp_BB;
            cmd.Parameters.Add("@Temp_BC", SqlDbType.Decimal).Value = Temp_BC;
            cmd.Parameters.Add("@Temp_BD", SqlDbType.Decimal).Value = Temp_BD;
            cmd.Parameters.Add("@Temp_BE", SqlDbType.Decimal).Value = Math.Round(Temp_OB429D總計 / 1000 * Temp_BD, 0, MidpointRounding.AwayFromZero);


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
        /// 集合所有資料parser VALUE到Admin_Card_Count
        /// </summary>
        /// <param name="parserBody"></param>
        public Tuple<string, int> parserProperty(string strYM, Dictionary<string, List<Tuple<string, Type, Object>>> parserBody)
        {
            int iCount = 0;
            try
            {
                DataTable dtChk = GetAdmin_Card_Count_byYM(strYM, "", "", new string[] { });
                bool IsUpdate = true;

                foreach (string strBranch_No in parserBody.Keys)
                {
                    Type classType = typeof(Admin_Card_Count);
                    List<Tuple<string, Type, Object>> parserValues = parserBody[strBranch_No];
                    foreach (var UnitData in parserValues)
                    {
                        PropertyInfo classProperty = classType.GetProperty(UnitData.Item1);
                        classProperty.SetValue(this, UnitData.Item3, null);
                    }
                    //若有該筆單位資料為TRUE， 否則為FALSE
                    IsUpdate = dtChk.Select("Branch_No='" + strBranch_No + "'").Length > 0;

                    insertAdmin_Card_Count(strYM, IsUpdate == true ? Common.clsEnum.EditMode.Update : Common.clsEnum.EditMode.Insert);
                    iCount += 1;
                }
                return new Tuple<string, int>("轉入Admin_Card_Count作業完成!，共匯入" + iCount.ToString() + "筆。", iCount);
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
            string strTableName = "Admin_Card_Count";
            int iCount = 0;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();


            string sql = @"delete from Admin_Card_Count_old where SUBSTRING(YM,1,3)=@YYY;
                        INSERT INTO Admin_Card_Count_old 
                        SELECT * FROM Admin_Card_Count WHERE  SUBSTRING(YM,1,3)=@YYY
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
            return new Tuple<string, int>(strTableName+"移轉"+ strYYY + "年資料共" + iCount.ToString("#,##0") + "筆。" + Environment.NewLine, iCount);
        }


        /// <summary>
        /// 自動三年歷史資料搬移DB ，113年刪除104年
        /// </summary>
        /// <param name="strYM">要被移轉的年份</param>
        public Tuple<string, int> deleteOldData(string strYYY)
        {
            string strTableName = "Admin_Card_Count";
            int iCount = 0;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();


            string sql = @"delete from Admin_Card_Count where SUBSTRING(YM,1,3)=@YYY";

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
            return new Tuple<string, int>(strTableName + "刪除"+ strYYY + "年舊資料共" + iCount.ToString("#,##0") + "筆。" + Environment.NewLine, iCount);
        }
    }
}
