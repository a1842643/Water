using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO
{
    class Until
    {

        private string mBranch_No;
        private string mName;
        private DateTime mCreate_Date;
        private string mAddress;
        private string mTelephone;
        private string mAREA;
        private string mFax;
        private string mDormName;
        private string mManager;
        private DateTime mJoin_Date;
        private string mVice_Manager_1;
        private string mVice_Manager_2;
        private string mAss_GroupName;
        private string mAreacenterName;
        private string mGen_JurisName;
        private string mRemark;

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

        public string Name
        {
            get
            {
                return mName;
            }

            set
            {
                mName = value;
            }
        }

        public DateTime Create_Date
        {
            get
            {
                return mCreate_Date;
            }

            set
            {
                mCreate_Date = value;
            }
        }

        public string Address
        {
            get
            {
                return mAddress;
            }

            set
            {
                mAddress = value;
            }
        }

        public string Telephone
        {
            get
            {
                return mTelephone;
            }

            set
            {
                mTelephone = value;
            }
        }

        public string AREA
        {
            get
            {
                return mAREA;
            }

            set
            {
                mAREA = value;
            }
        }

        public string Fax
        {
            get
            {
                return mFax;
            }

            set
            {
                mFax = value;
            }
        }

        public string DormName
        {
            get
            {
                return mDormName;
            }

            set
            {
                mDormName = value;
            }
        }

        public string Manager
        {
            get
            {
                return mManager;
            }

            set
            {
                mManager = value;
            }
        }

        public DateTime Join_Date
        {
            get
            {
                return mJoin_Date;
            }

            set
            {
                mJoin_Date = value;
            }
        }

        public string Vice_Manager_1
        {
            get
            {
                return mVice_Manager_1;
            }

            set
            {
                mVice_Manager_1 = value;
            }
        }

        public string Vice_Manager_2
        {
            get
            {
                return mVice_Manager_2;
            }

            set
            {
                mVice_Manager_2 = value;
            }
        }

        public string Ass_GroupName
        {
            get
            {
                return mAss_GroupName;
            }

            set
            {
                mAss_GroupName = value;
            }
        }

        public string AreacenterName
        {
            get
            {
                return mAreacenterName;
            }

            set
            {
                mAreacenterName = value;
            }
        }

        public string Gen_JurisName
        {
            get
            {
                return mGen_JurisName;
            }

            set
            {
                mGen_JurisName = value;
            }
        }

        public string Remark
        {
            get
            {
                return mRemark;
            }

            set
            {
                mRemark = value;
            }
        }
        public static DataTable GetAllsUntil()
        {
            List<Until> untils = new List<Until>();
            Until Until = new Until();
            string sql = @"SELECT * FROM Until order by Ass_GroupName,Branch_No"; 
            SqlCommand cmd = new SqlCommand();
            {
                var withBlock = cmd;

                withBlock.CommandText = sql;
            }

            clsDB clsDB = new clsDB();
            DataTable dt = clsDB.OpenDataTable(cmd);


            return dt;


        }

        public static DataTable GetUntillist()
        {
            List<Until> untils = new List<Until>();
            Until Until = new Until();
            string sql = @"SELECT Branch_No FROM Until " +
             " order by Branch_No ";
            SqlCommand cmd = new SqlCommand();
            {
                var withBlock = cmd;
              
                withBlock.CommandText = sql;
            }

            clsDB clsDB = new clsDB();
            DataTable dt = clsDB.OpenDataTable(cmd);
           
        
                return dt;


        }
        public static string getBranch_Name(string Branch_No)
        {
            string Name="";
            string sql = @"SELECT Name FROM Until " +
            "WHERE  Branch_No = @Branch_No";
            SqlCommand cmd = new SqlCommand();
            {
                var withBlock = cmd;
                withBlock.Parameters.Add("@Branch_No", SqlDbType.Char).Value = Branch_No;
                withBlock.CommandText = sql;
            }

            clsDB clsDB = new clsDB();
            DataTable dt = clsDB.OpenDataTable(cmd);
            if (dt.Rows.Count>0)
            Name = dt.Rows[0][0].ToString();
            return Name;
        }
        public static Until getChief(string Branch_No)
        {
            Until Until = new Until();

            string Name = "";
            string sql = @"SELECT Create_Date, Join_Date, Manager, Vice_Manager_1, Vice_Manager_2 from Until  " +
            "WHERE  Branch_No = @Branch_No";
            SqlCommand cmd = new SqlCommand();
            {
                var withBlock = cmd;
                withBlock.Parameters.Add("@Branch_No", SqlDbType.Char).Value = Branch_No;
                withBlock.CommandText = sql;
            }

            clsDB clsDB = new clsDB();
            DataTable dt = clsDB.OpenDataTable(cmd);
            if (dt.Rows.Count>0)
            {
                if (dt.Rows[0]["Create_Date"]!=DBNull.Value)
                Until.Create_Date = Convert.ToDateTime(dt.Rows[0]["Create_Date"]);
                if (dt.Rows[0]["Join_Date"] != DBNull.Value)
                    Until.Join_Date = Convert.ToDateTime(dt.Rows[0]["Join_Date"]);
                Until.Manager = dt.Rows[0]["Manager"].ToString();
                Until.Vice_Manager_1 = dt.Rows[0]["Vice_Manager_1"].ToString();
                Until.Vice_Manager_2 = dt.Rows[0]["Vice_Manager_2"].ToString();
            }
            Name = dt.Rows[0][0].ToString();
            return Until;
        }
    }
}
