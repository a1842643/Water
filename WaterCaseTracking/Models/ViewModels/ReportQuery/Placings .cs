using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO
{
    class Placings
    {
        private string mBranch_No;
        private string mBranch_Name;
        private string mList_Year;
        private string mClass;
        private string mTotal;
        private string mPlacings;

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

        public string Branch_Name
        {
            get
            {
                return mBranch_Name;
            }

            set
            {
                mBranch_Name = value;
            }
        }

        public string List_Year
        {
            get
            {
                return mList_Year;
            }

            set
            {
                mList_Year = value;
            }
        }

        public string Class
        {
            get
            {
                return mClass;
            }

            set
            {
                mClass = value;
            }
        }

        public string Total
        {
            get
            {
                return mTotal;
            }

            set
            {
                mTotal = value;
            }
        }

        public string placings
        {
            get
            {
                return mPlacings;
            }

            set
            {
                mPlacings = value;
            }
        }
        public Placings getPlacings(string YM, string Branch_No)
        {
            Placings Placings = new Placings();
            // Dim sql As String = "SELECT  UNT_UNITNAME as UNITNAME,EMP_EXTENDADD,* FROM SALMMPR "
            string sql = @"SELECT Class, Total, Placings " +
                "FROM Placings " +
                          "WHERE  Branch_No = @Branch_No";
            SqlCommand cmd = new SqlCommand();
            {
                var withBlock = cmd;
            
                withBlock.Parameters.Add("@Branch_No", SqlDbType.Char).Value = Branch_No;
                withBlock.CommandText = sql;
            }

            clsDB clsDB = new clsDB();
            DataTable dt = clsDB.OpenDataTable(cmd);
            if (dt.Rows.Count > 0)
            {
                Placings.Class = dt.Rows[0]["Class"].ToString();
                Placings.Total = dt.Rows[0]["Total"].ToString();
                Placings.placings = dt.Rows[0]["Placings"].ToString();


            }
            else
                Placings = null;
            return Placings;
        }
    }
}
