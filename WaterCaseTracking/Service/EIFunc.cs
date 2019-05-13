using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Common
{
    public class EIFunc
    {
        public static Boolean isEmpty(string lsString)
        {
            lsString = lsString.Replace(" ", "");
            lsString = lsString.Replace("　", "");
            lsString = lsString.Replace("&nbsp;", "");
            lsString = lsString.Replace("\n", "");
            if (lsString ==null || lsString=="")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //     '=======================================================================================================
        //  'DataTime Function

        // '取得民國年今天
        public string getCNow()
        {
            string lsStr;
            lsStr = (DateTime.Now.Year - 1911).ToString() + DateTime.Now.ToString("/MM/dd");
            return lsStr;
             }
   

   // '西元年 to 民國年
   public  string EI_getAD2C(string lsDate)
        {
            string lsStr;
            if(isEmpty(lsDate))
            {
                lsDate = DateTime.Now.ToString("yyyy/MM/dd");
            }
            lsDate = lsDate.Replace("-", "/");
            lsStr = (Convert.ToInt16(lsDate.Substring(0, 4)) - 1911).ToString() + lsDate.Substring(4);

            return lsStr;
        }
   

 //   '民國年 to 西元年
public  string EI_getC2AD(string lsDate)
        {
            string lsStr;
            string[] arrStr;
            if (isEmpty(lsDate))
            {
                lsDate = (DateTime.Now.Year - 1911).ToString() + DateTime.Now.ToString("/MM/dd");
            }
            lsDate = lsDate.Replace("-", "/");
            arrStr = lsDate.Split('/');
            lsStr = (Convert.ToInt16(arrStr[0]) + 1911).ToString() + "/" +arrStr[1] + "/" + arrStr[2];
            return lsStr;
        }
   

  

//    '計算日期(日期, 增減數字, 變更日期類型(Y/M/D))
public  string EI_getCalDate(string lsDate,int liNum,string lsDateType="D")
        {
            string lsStr;
            string[] arrStr;
            string lsYear, lsMonth, lsDay;
            int liYear, liMonth, liMonth1, liDay;
            if (isEmpty(lsDate))
            {
                lsDate = DateTime.Now.ToString("yyyy/MM/dd");
             }
            arrStr = lsDate.Split('/');
            lsYear = arrStr[0];
            lsMonth = arrStr[1];
            lsDay = arrStr[2];



            if ((lsDateType == "Y") || (lsDateType == "y"))

                lsYear = (Convert.ToInt16(lsYear) + liNum).ToString();

            else if ((lsDateType == "M") || (lsDateType == "m"))
            {
                liMonth = Convert.ToInt16(lsMonth) + liNum;
                liYear = Convert.ToInt16(lsYear);
                while (liMonth <= 0)
                {
                    liYear -= 1;
                    liMonth += 12;
                }
                lsYear = liYear.ToString();
                lsMonth = liMonth.ToString();

            }


            else if ((lsDateType == "D") || (lsDateType == "d"))
            {
                liDay = Convert.ToInt16(lsDay) + liNum;
                liMonth = Convert.ToInt16(lsMonth);
                liYear = Convert.ToInt16(lsYear);

                while (liDay <= 0)
                {
                    // '月份減1
                    liMonth = liMonth - 1;
                    if (liMonth == 0)
                    {
                        liYear = liYear - 1;
                        liMonth = 12;
                        liDay = liDay - 31;
                    }
                    else
                        liDay -= DateTime.DaysInMonth(liYear, liMonth);
                  
              }

            lsYear = liYear.ToString();
                lsMonth = liMonth.ToString();
                lsDay = liDay.ToString();

           
            }
            lsStr = lsYear + "/" + lsMonth + "/" +lsDay;

        return lsStr;

        }
        public string EI_getMonth(string lsStatus="0")
        {
            string lsMonth = "";


            lsMonth = DateTime.Now.Month.ToString();

            //  '01~12
            if (lsStatus == "1")
            {
                lsMonth = "0" + lsMonth;
                lsMonth = lsMonth.Substring(lsMonth.Length - 2, 2);
         }

            return lsMonth;



        }

        public static string EI_getAddMonth(int lsAddMonth, string lsStatus)
        {
            string lsMonth = "";

            lsMonth = DateTime.Now.AddMonths(lsAddMonth).Month.ToString();

            // '01~12
            if (lsStatus == "1")
            {
                lsMonth = "0" + lsMonth;
                lsMonth = lsMonth.Substring(lsMonth.Length - 2, 2);
            }

            return lsMonth;
        }


        public string EI_getAddMonth1(int lsAddMonth,Boolean lbStatus=false)
        {
            string lsMonth = "";

            lsMonth = DateTime.Now.AddMonths(lsAddMonth).Month.ToString();

            // '01~12
            if (lbStatus == true)
            {
                lsMonth = "0" + lsMonth;
                lsMonth = lsMonth.Substring(lsMonth.Length - 2, 2);
          }

            return lsMonth;
        }

public  static string EI_getYear(string lsStatus="0")
        {
            string lsYear = "";
            int liYear = DateTime.Now.Year;

            // '0:西元年,1:民國年
            if (lsStatus == "0")
                lsYear = liYear.ToString();
            else if (lsStatus == "1")
                lsYear = (liYear - 1911).ToString();


            return lsYear;
        }

        public static string EI_getAddMonth(int lsAddMonth)
        {
            string lsMonth = "";

            lsMonth = DateTime.Now.AddMonths(lsAddMonth).Month.ToString();

            //// 01~12
            //if (lsStatus == "1")
            //{
            //    lsMonth = "0" + lsMonth;
            //    lsMonth = lsMonth.Substring(lsMonth.Length() - 2, 2);
            //}

            return lsMonth;
        }

        public static  string EI_getAddYear(int lsAddYear,string lsStatus)
        {
            string lsYear = "";
            int liYear = DateTime.Now.AddYears(lsAddYear).Year;

            // '0:西元年,1:民國年
            if (lsStatus == "0")
                lsYear = liYear.ToString();

            else if (lsStatus == "1")
                lsYear = (liYear - 1911).ToString();


            return lsYear;
        }
 
        public static string EI_getFormat3(string YM)
        {
            string S_YM = "";
            if (!YM.Equals("0"))
                S_YM = (Convert.ToInt16(YM.Substring(0, (YM.Length - 2))) - 1).ToString() + "年12月";
            else
                S_YM = "&nbsp;";


            return S_YM;
        }
  
        public static  string EI_getFormat4(string YM)
        {
            string S_YM = "";
            if (!YM.Equals("0"))
                S_YM = (Convert.ToInt16(YM.Substring(0, (YM.Length - 2))) - 2).ToString()+ "年12月";
            else
                S_YM = "&nbsp;";


            return S_YM;
        }
  

  //  '========================================================
 //   '型態轉換
 public static string EI_Format(string lsValue,string lsType="")
        {
           
            try
            {
                if (lsType == "")
                {


                    if ((lsValue == "") || (lsValue == "&nbsp;"))
                        lsValue = "0";
                    else
                        lsValue = string.Format(lsValue, "###,###");


                }
                else if (lsType == "%")
                {
                    lsValue = lsValue;

                }

               
            }
            catch (Exception ex)
            {
                lsValue = "0";
            }

          

            return lsValue;
        }

        public static string EI_Format(decimal Value, string lsType = "")
        {
            string lsValue = Value.ToString();

            try
            {
                if (lsType == "")
                {


                    if ((lsValue == "") || (lsValue == "&nbsp;"))
                        lsValue = "0";
                    else
                        lsValue = Value.ToString("#,0");


                }
                else if (lsType == "%")
                {
                    lsValue = lsValue;

                }


            }
            catch (Exception ex)
            {
                lsValue = "0";
            }



            return lsValue;
        }
        public static string EI_FormatCal(string lsValue1,string lsValue2,string lsCal="-",string lsType="")
        {
            string lsValue="";
            Decimal  ldValue1, ldValue2;
            Decimal ldValue = 0;
            ldValue1 = Convert.ToDecimal(lsValue1);
            ldValue2 = Convert.ToDecimal(lsValue2);
            if (lsCal == "+")
                ldValue = ldValue1 + ldValue2;

            else if (lsCal == "-")
                ldValue = ldValue1 - ldValue2;

            else if (lsCal == "*")
                ldValue = ldValue1 * ldValue2;

            else if (lsCal == "/")
                ldValue = ldValue1 / ldValue2;



            if (lsType == "")
                lsValue = string.Format(Convert.ToString(ldValue), "###,###");

            else if (lsType == "0.00%")
                lsValue = string.Format(Convert.ToString(ldValue), "0.00") + "%";



            return lsValue;
        }

  

  //  '將月份轉換成"MM"二位數
  public string EI_CMonth(int liMonth)
        {
            string lsMonth;
            if (liMonth < 10)
                lsMonth = "0" + liMonth.ToString();
            else
                lsMonth = liMonth.ToString();

            return lsMonth;
        }

  
    }
}