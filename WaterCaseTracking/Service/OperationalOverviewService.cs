using ADO;
using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using WaterCaseTracking.Models.ViewModels.OperationalOverview;

namespace WaterCaseTracking.Service
{
    public class OperationalOverviewService
    {
        public static string lsBNo = "";
        public static string lsMonth1 = "";
        public static string lsMonth2 = "";
        public static string lsMonth3 = "";
        public static string lsMonth4 = "";
        public static Boolean lbMonth1 = false;
        public static Boolean lbMonth2 = false;
        public static Boolean lbMonth3 = false;
        public static Boolean lbMonth4 = false;
        public static Boolean lbMonth1C = false;
        public static Boolean lbMonth2C = false;
        public static Boolean lbMonth3C = false;
        public static Boolean lbMonth4C = false;
        public static Boolean lbT15_1 = false;
        public static Boolean lbT51 = false;
        public static Boolean lbT52 = false;
        public static Boolean lbT53 = false;
        public static Boolean lbT54 = false;
        public static Boolean lbChief = false;

        public static string lsTempYM, lsTempYM1, lsYear, lsMonth;

        public static string[] arrList;

        public static string curPath = "";
        public static string mainPath = "";


        public string OperatedCardB(SearchListViewModel Search)//
        {
            string htmlstr = "";
            string strYM = Search.Yeart.ToString();
            if (Search.Month.ToString().Length < 2)
            {
                strYM += "0" + Search.Month.ToString();
            }
            else
            {
                strYM += Search.Month.ToString();
            }

            //try
            //{
                string M1, M2, M3, M4 = "";
                string[] Mstr = { "", "", "", "" };
                M1 = strYM;
                try
                {
                    string YYYYMM = (int.Parse(strYM) + 191100).ToString();
                    DateTime basicDate = DateTime.Parse(YYYYMM.Substring(0, 4) + "-" + YYYYMM.Substring(4, 2) + "-01");
                    for (int i = 0; i < 4; i++)
                    {
                        Mstr[i] = (basicDate.AddMonths(-i).Year - 1911).ToString().PadLeft(3, '0') + basicDate.AddMonths(-i).Month.ToString().PadLeft(2, '0');
                    }
                    //M1 = (basicDate.AddMonths(-0).Year - 1911).ToString().PadLeft(3, '0') + basicDate.AddMonths(-0).Month.ToString().PadLeft(2, '0');
                    //M2 = (basicDate.AddMonths(-1).Year - 1911).ToString().PadLeft(3, '0') + basicDate.AddMonths(-1).Month.ToString().PadLeft(2, '0');
                    //M3 = (basicDate.AddMonths(-2).Year - 1911).ToString().PadLeft(3, '0') + basicDate.AddMonths(-2).Month.ToString().PadLeft(2, '0');
                    //M4 = (basicDate.AddMonths(-3).Year - 1911).ToString().PadLeft(3, '0') + basicDate.AddMonths(-3).Month.ToString().PadLeft(2, '0');
                    //M1 = "10704";
                    //M2 = "10703";
                    //M3 = "10702";
                    //M4 = "10701";

                }
                catch (Exception ex)
                {
                    throw ex;
                }

                for (int i = 0; i < 4; i++)
                {
                    if (string.IsNullOrEmpty(Mstr[i]) == false)
                        switch (i)
                        {
                            case 0:
                                lsMonth1 = Mstr[i];
                                break;
                            case 1:
                                lsMonth2 = Mstr[i];
                                break;
                            case 2:
                                lsMonth3 = Mstr[i];
                                break;
                            case 3:
                                lsMonth4 = Mstr[i];
                                break;
                        }
                    else
                        switch (i)
                        {
                            case 0:
                                lsMonth1 = "0";
                                break;
                            case 1:
                                lsMonth2 = "0";
                                break;
                            case 2:
                                lsMonth3 = "0";
                                break;
                            case 3:
                                lsMonth4 = "0";
                                break;
                        }
                }
                
                //// 'Month1
                //if (string.IsNullOrEmpty(M1) == false)
                //    lsMonth1 = M1;
                //else
                //    lsMonth1 = "0";

                //// 'Month2

                //if (string.IsNullOrEmpty(M2) == false)
                //    lsMonth2 = M2;
                //else
                //    lsMonth2 = "0";

                ////'Month3
                //if (string.IsNullOrEmpty(M3) == false)
                //    lsMonth1 = M3;
                //else
                //    lsMonth3 = "0";

                //// 'Month4
                //if (string.IsNullOrEmpty(M4) == false)
                //    lsMonth4 = M4;
                //else
                //    lsMonth4 = "0";

                //DataTable dt = new DataTable();
                //dt = Until.GetUntillist();

                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    //Console.WriteLine(dt.Rows[i]["Branch_No"].ToString());
                //    if (Search.Until == dt.Rows[i]["Branch_No"].ToString())
                //    {
                //        htmlstr += OperatedCardBPDF(dt.Rows[i]["Branch_No"].ToString(), M1, M2, M3, M4);
                //        break;
                //    }
                //}
                //htmlstr = OperatedCardBPDF(Search.Until.ToString(), M1, M2, M3, M4);
                htmlstr = OperatedCardBPDF(Search.Until.ToString(), Mstr[0], Mstr[1], Mstr[2], Mstr[3]);
                return htmlstr;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }
        public static string OperatedCardBPDF(string lsBNo, string lsM1 = "", string lsM2 = "", string lsM3 = "", string lsM4 = "")
        {
            string lsTempYM1, lsTempYM2, lsTempYM3, lsTempYM4, lsTempYM5, lsTempYM6;
            string lsTempYM, lsYear, lsMonth;
            lsMonth1 = lsM1;
            string lsM1Year = lsMonth1.Substring(0, lsMonth1.Length - 2);
            string lsM1Month = lsMonth1.Replace(lsM1Year, "");
            string labMonth1 = lsM1Year + "年" + lsM1Month + "月";
            string labMonth1_1 = lsM1Year + "年" + lsM1Month + "月";
            string labbMonth1_2 = lsM1Year + "年" + lsM1Month + "月";
            string labMonth1_3 = lsM1Year + "年" + lsM1Month + "月";
            string labMonth1_4 = (Convert.ToInt16(lsM1Year) - 1) + "年 " + lsM1Month + "月";
            string labMonth1_5 = lsM1Year + "年" + lsM1Month + "月";


            //  'chief====================================================================================================
            string labChief_Setup_Date = "";//           '成立日期
            string labChief_Join_Date = "";//            '到任日期
            string labChief_Manager = "";             //'經理
            string labChief_Vice_Manager_1 = "";    //  '副理1
            string labChief_Vice_Manager_2 = "";     //  '副理2

            Until until = Until.getChief(lsBNo);
            try
            {
                //Console.WriteLine(until.Create_Date);
                if (until.Create_Date.Year > 1911)
                    labChief_Setup_Date = Convert.ToString(Convert.ToInt16(until.Create_Date.Year) - 1911) + "/" + until.Create_Date.ToString("MM/dd");
                if (until.Join_Date.Year > 1911)
                    labChief_Join_Date = Convert.ToString(Convert.ToInt16(until.Join_Date.Year) - 1911) + "/" + until.Join_Date.ToString("MM/dd");
            }

            catch (Exception ex)
            {
                //Console.WriteLine(ex.StackTrace);
                //Console.WriteLine("lsBNo=" + lsBNo + " labChief_Setup_Date=" + labChief_Setup_Date + " labChief_Join_Date=" + labChief_Join_Date + "/r/n");
                throw ex;
            }
            labChief_Manager = until.Manager;
            labChief_Vice_Manager_1 = until.Vice_Manager_1;
            labChief_Vice_Manager_2 = until.Vice_Manager_2;

            //  'Rank=====================================================================================================
            int liCY;
            if (EIFunc.EI_getAddMonth(-1) == "12")
                liCY = Convert.ToInt16(EIFunc.EI_getAddYear(-1, "1"));

            liCY = Convert.ToInt16(EIFunc.EI_getYear("1"));


            //'Rank1====================================================================================================
            string labR1_Year = "";
            string labR1_Class = "";
            string labR1_Total = "";
            string labR1_Placings = "";

            //  'Rank1====================================================================================================
            labR1_Year = "";
            labR1_Class = "";
            labR1_Total = "";
            labR1_Placings = "";
            ADO.Placings drRank1 = new Placings();
            labR1_Year = liCY.ToString();
            drRank1 = drRank1.getPlacings(labR1_Year, lsBNo);
            //    lsQry = "SELECT Class, Total, Placings " + _
            //        "FROM Placings " + _
            //        "WHERE Branch_No = @Bran_No and List_Year = @YY "
            //ds.SelectCommand = lsQry
            //ds.SelectParameters.Clear()
            //ds.SelectParameters.Add("Bran_No", lsBNo)

            //ds.SelectParameters.Add("YY", labR1_Year)

            //drRank1 = CType(ds.Select(DataSourceSelectArguments.Empty), SqlDataReader)

            if (drRank1 != null)
            {

                labR1_Class = drRank1.Class;
                labR1_Total = drRank1.Total;
                labR1_Placings = drRank1.placings;
            }


            //    'Rank2====================================================================================================
            string labR2_Year = "";
            string labR2_Class = "";
            string labR2_Total = "";
            string labR2_Placings = "";
            ADO.Placings drRank2 = new Placings();
            labR2_Year = (liCY - 1).ToString();
            drRank2.getPlacings(labR2_Year, lsBNo);

            if (drRank2 != null)
            {
                labR2_Class = drRank2.Class;
                labR2_Total = drRank2.Total;
                labR2_Placings = drRank2.placings;
            }


            // 'Rank3====================================================================================================
            string labR3_Year = "";
            string labR3_Class = "";
            string labR3_Total = "";
            string labR3_Placings = "";

            labR3_Year = (liCY - 2).ToString();
            ADO.Placings drRank3 = new Placings();
            drRank3 = drRank3.getPlacings(labR3_Year, lsBNo);


            if (drRank3 != null)
            {

                labR3_Class = drRank3.Class;
                labR3_Total = drRank3.Total;
                labR3_Placings = drRank3.placings;

            }
            //  'Rank4====================================================================================================
            string labR4_Year = "";
            string labR4_Class = "";
            string labR4_Total = "";
            string labR4_Placings = "";
            ADO.Placings drRank4 = new Placings();
            labR4_Year = (liCY - 3).ToString();
            drRank4 = drRank4.getPlacings(labR4_Year, lsBNo);


            if (drRank4 != null)
            {

                labR4_Class = drRank4.Class;
                labR4_Total = drRank4.Total;
                labR4_Placings = drRank4.placings;
            }
            //   'Rank5====================================================================================================
            string labR5_Year = "";
            string labR5_Class = "";
            string labR5_Total = "";
            string labR5_Placings = "";
            labR5_Year = (liCY - 4).ToString();
            ADO.Placings drRank5 = new Placings();

            if (drRank5 != null)
            {
                labR5_Class = drRank5.Class;
                labR5_Total = drRank5.Total;
                labR5_Placings = drRank5.placings;
            }

            //    'M1=======================================================================================================
            string labM1_D = "0";
            string labM1_E = "0";
            string labM1_F = "0";
            string labM1_G = "0";
            string labM1_H = "0"; ;
            string labM1_I = "0";
            string labM1_J = "0";
            string labM1_K = "0";
            string labM1_L = "0";
            string labM1_M = "0";
            string labM1_N = "0";
            string labM1_O = "0";
            string labM1_P = "0";
            string labM1_Q = "0";
            string labM1_R = "0";
            string labM1_S = "0";
            string labM1_T = "0";
            string labM1_U = "0";
            string labM1_V = "0";
            string labM1_W = "0";
            string labM1_X = "0";
            string labM1_Y = "0";
            string labM1_Z = "0.00%";//    '%
            string labM1_AA = "0";
            string labM1_AB = "0";
            string labM1_AC = "0";
            string labM1_AD = "0";
            string labM1_AE = "0";
            string labM1_AF = "0";
            string labM1_AG = "0";
            string labM1_AH = "0";
            string labM1_AI = "0";
            string labM1_AJ = "0";
            string labM1_AK = "0.00%";//    '%
            string labM1_AL = "0.00%";//    '%
            string labM1_AM = "0.00%";//   '%
            string labM1_AN = "0.00%";//   '%
            string labM1_AO = "0.00%";//    '%
            string labM1_AP = "0.00%"; //'%
            string labM1_AQ = "0";
            Double ld_M1_S = 0, ld_M1_U = 0, ld_M1_T = 0, ld_M1_SUT = 0;
            string labM1sut = "";
            ADO.Admin_Card drM1 = new Admin_Card();
            drM1 = drM1.getAdmin_Card(lsM1, lsBNo);
            if (drM1 != null)
            {
                labM1_D = EIFunc.EI_Format(drM1.Temp_D);
                labM1_Q = EIFunc.EI_Format(drM1.Temp_Q);
                labM1_E = EIFunc.EI_Format(drM1.Temp_E);
                labM1_R = EIFunc.EI_Format(drM1.Temp_R);
                labM1_H = EIFunc.EI_Format(drM1.Temp_H);
                labM1_I = EIFunc.EI_Format(drM1.Temp_I);
                labM1_AQ = EIFunc.EI_Format(drM1.Temp_AQ);
                labM1_L = EIFunc.EI_Format(drM1.Temp_L);
                labM1_P = EIFunc.EI_Format(drM1.Temp_P);
                ld_M1_S = Convert.ToDouble(drM1.Temp_S);
                labM1_S = EIFunc.EI_Format(Convert.ToDecimal(ld_M1_S));
                ld_M1_U = Convert.ToDouble(drM1.Temp_U);
                labM1_U = EIFunc.EI_Format(Convert.ToDecimal(ld_M1_U));
                ld_M1_T = Convert.ToDouble(drM1.Temp_T);
                labM1_T = EIFunc.EI_Format(Convert.ToDecimal(ld_M1_T));
                ld_M1_SUT = ld_M1_S - ld_M1_U - ld_M1_T;
                labM1sut = EIFunc.EI_Format(Convert.ToDecimal(ld_M1_SUT));
                labM1_Y = EIFunc.EI_Format(drM1.Temp_Y);
                labM1_X = EIFunc.EI_Format(drM1.Temp_X);
                labM1_AI = EIFunc.EI_Format(drM1.Temp_AI);
                labM1_AN = drM1.Temp_AN; // '%
                labM1_AO = drM1.Temp_AO;//   '%
                labM1_AP = drM1.Temp_AP;//  '%
                labM1_AK = drM1.Temp_AK;//   '%
                labM1_Z = drM1.Temp_Z;//     '%
                labM1_AL = drM1.Temp_AL;//   '%
                labM1_AM = drM1.Temp_AM;//   '%
            }
            //  'M1C======================================================================================================
            string labM1C_E = "0";
            string labM1C_F = "0.00%";
            string labM1C_G = "0";
            string labS_EG = "0";
            string labM1C_H = "0.00%";
            string labM1C_K = "0";
            string labM1C_L = "0";
            string labS_KL = "0";
            string labM1C_N = "0.00%";
            string labM1C_Q = "0";
            string labM1C_S = "0.00%";
            string labM1C_R = "0";
            string labS_QR = "0";
            string labM1C_T = "0.00%";
            string labM1C_AZ = "0";
            string labM1C_AU = "0";
            string labM1C_AX = "0";
            string labM1C_AT = "0";
            string labM1C_X = "0";
            string labM1C_Z = "0.00%";
            string labM1C_BB = "0";
            string labM1C_BA = "0";
            string labM1C_AF = "0";
            string labM1C_AK = "0";
            string labM1C_AL = "0.00%";
            string labM1C_AN = "0";
            string labM1C_AO = "0.00%";
            string labM1C_AP = "0";
            string labM1C_U = "0.00%";
            string labM1C_O = "0.00%";
            string labM1C_AB = "0.00%";
            string labM1C_AI = "0.00%";
            string ls_M1C_AC = "0";
            string ls_M1C_I = "0";
            string labS_aci = "0.00%";
            string labM1C_AG = "0";
            string labM1C_AH = "0";
            Double ld_M1C_AZ = 0, ld_M1C_AU = 0, ld_M1C_AX = 0, ld_M1C_AT = 0, ld_M1C_X = 0;
            Double ld_M1C_BB = 0, ld_M1C_BA = 0, ld_M1C_AF = 0, ld_M1C_AP = 0;
            string ls_M1C_U = "", ls_M1C_O = "", ls_M1C_AB = "", ls_M1C_AI = "", ls_M1C_ACI = "";

            Double ld_M4C_AZ, ld_M4C_AU, ld_M4C_AX, ld_M4C_AT, ld_M4C_X, ld_M4C_BB, ld_M4C_BA, ld_M4C_AF, ld_M4C_AP = 0;
            ld_M4C_AZ = 0;
            ld_M4C_AU = 0;
            ld_M4C_AX = 0;
            ld_M4C_AT = 0;
            ld_M4C_X = 0;
            ld_M4C_BB = 0;
            ld_M4C_BA = 0;
            ld_M4C_AF = 0;
            string ls_M4C_U = "", ls_M4C_O = "", ls_M4C_AB = "", ls_M4C_AI = "";
            string ls_M4C_AC, ls_M4C_I, ls_M4C_ACI = "";
            string labM4C_AZ = "0";
            string labM4C_AU = "0";
            string labM4C_AX = "0";
            string labM4C_AT = "0";
            string labM4C_X = "0";
            string labM4C_BB = "0";
            string labM4C_BA = "0";
            string labM4C_AF = "0";
            string labM4C_AP = "0";
            string labM4C_AK = "0";
            string labM4C_AN = "0";
            string labM4C_U = "0";
            string labM4C_O = "0";
            string labM4C_AB = "0";
            string labM4C_AI = "0";
            string labS_aci_4mc = "0.00%";

            string labS_azGap = "", labS_auGap = "", labS_axGap = "", labS_atGap = "", labS_xGap = "", labS_bbGap = "", labS_baGap = "";
            string labS_azGapP = "", labS_auGapP = "", labS_axGapP = "", labS_atGapP = "", labS_xGapP = "", labS_bbGapP = "", labS_baGapP = "";
            Double ld_M1C_XBBBA, ld_M4C_XBBBA;
            ld_M4C_XBBBA = 0;
            string labS_xbbba = "0";
            string labS_xbbba_M4C = "0";
            string labS_xbbba_gap = "0";
            string labS_xbbba_gapP = "0.00%";
            string labS_afGap = "0";
            string labS_apGap = "0";
            string labS_uGap = "0.00%";
            string labS_oGap = "0.00%";
            string labS_abGap = "0.00%";
            string labS_aiGap = "0.00%";
            string labS_aci_Gap = "0.00%";
            string msg = "";
            string lsDT1 = "";// 'YM

            ADO.Admin_Card_Count drM1C = new Admin_Card_Count();
            drM1C = drM1C.getAdmin_Card_Count(lsM1, lsBNo);






            if (drM1C != null)
            {
                labM1C_E = EIFunc.EI_Format(drM1C.Temp_E);
                labM1C_F = drM1C.Temp_F;//    '%
                labM1C_G = EIFunc.EI_Format(drM1C.Temp_G);
                labS_EG = EIFunc.EI_FormatCal(drM1C.Temp_E.ToString(), drM1C.Temp_G.ToString(), "-");
                labS_EG = EIFunc.EI_Format(Convert.ToDecimal(labS_EG));
                labM1C_H = drM1C.Temp_H;//    '%

                labM1C_K = EIFunc.EI_Format(drM1C.Temp_K);
                labM1C_L = EIFunc.EI_Format(drM1C.Temp_L);
                labS_KL = EIFunc.EI_Format( Convert.ToDecimal(EIFunc.EI_FormatCal(drM1C.Temp_K.ToString(), drM1C.Temp_L.ToString(), "-")));
                labM1C_N = drM1C.Temp_N;//    '%

                labM1C_Q = EIFunc.EI_Format(drM1C.Temp_Q);
                labM1C_S = drM1C.Temp_S;// ");//    '%
                labM1C_R = EIFunc.EI_Format(drM1C.Temp_R);
                labS_QR = EIFunc.EI_Format( Convert.ToDecimal(EIFunc.EI_FormatCal(drM1C.Temp_Q.ToString(), drM1C.Temp_R.ToString(), "-")));
                labM1C_T = drM1C.Temp_T;//    '%
                ld_M1C_AZ =  Convert.ToDouble(drM1C.Temp_AZ);
                labM1C_AZ = EIFunc.EI_Format(Convert.ToDecimal(ld_M1C_AZ));
                ld_M1C_AU = Convert.ToDouble(drM1C.Temp_AU);
                labM1C_AU = EIFunc.EI_Format(Convert.ToDecimal(ld_M1C_AU));
                ld_M1C_AX = Convert.ToDouble(drM1C.Temp_AX);
                labM1C_AX = EIFunc.EI_Format(Convert.ToDecimal(ld_M1C_AX));
                ld_M1C_AT = Convert.ToDouble(drM1C.Temp_AT);
                labM1C_AT = EIFunc.EI_Format(Convert.ToDecimal(ld_M1C_AT));
                ld_M1C_X = Convert.ToDouble(drM1C.Temp_X);
                labM1C_X = EIFunc.EI_Format(Convert.ToDecimal(ld_M1C_X));

                labM1C_Z = drM1C.Temp_Z;//            '%
                ld_M1C_BB = Convert.ToDouble(drM1C.Temp_BB);
                labM1C_BB = EIFunc.EI_Format(Convert.ToDecimal(ld_M1C_BB));
                ld_M1C_BA = Convert.ToDouble(drM1C.Temp_BA);
                labM1C_BA = EIFunc.EI_Format(Convert.ToDecimal(ld_M1C_BA));
                ld_M1C_AF = Convert.ToDouble(drM1C.Temp_AF);
                labM1C_AF = EIFunc.EI_Format(Convert.ToDecimal(ld_M1C_AF));
                labM1C_AK = EIFunc.EI_Format(drM1C.Temp_AK);
                labM1C_AL = drM1C.Temp_AL;//       '%
                labM1C_AN = EIFunc.EI_Format(drM1C.Temp_AN);
                labM1C_AO = drM1C.Temp_AO;//         '%
                ld_M1C_AP = Convert.ToDouble(drM1C.Temp_AP);
                labM1C_AP = EIFunc.EI_Format(Convert.ToDecimal(ld_M1C_AP));

                ls_M1C_U = drM1C.Temp_U;
                if (ls_M1C_U == null)
                    ls_M1C_U = "";
                labM1C_U = ls_M1C_U;                     //      '%
                ls_M1C_O = drM1C.Temp_O;
                labM1C_O = ls_M1C_O;//'%
                ls_M1C_AB = drM1C.Temp_AB;
                labM1C_AB = ls_M1C_AB;  //                        '%
                ls_M1C_AI = drM1C.Temp_AI;
                labM1C_AI = ls_M1C_AI;        //                '%

                ls_M1C_AC = drM1C.Temp_AC.Replace("%", "");
                if (ls_M1C_AC.Equals("") || ls_M1C_AC.Equals("-"))
                    ls_M1C_AC = "0";

                ls_M1C_I = drM1C.Temp_I.Replace("%", "");
                if (ls_M1C_I.Equals("") || ls_M1C_I.Equals("-"))
                    ls_M1C_I = "0";

                try
                {
                    ls_M1C_ACI = (Convert.ToDecimal(ls_M1C_AC) - Convert.ToDecimal(ls_M1C_I)).ToString();
                    labS_aci = ls_M1C_ACI + "%";
                    labM1C_AG = EIFunc.EI_Format(drM1C.Temp_AG);
                    labM1C_AH = EIFunc.EI_Format(drM1C.Temp_AH);
                }
                catch (Exception ex)
                {

                    //Console.WriteLine("Branch_No" + lsBNo + " lsBNo=" + lsBNo + " ls_M1C_AC=" + ls_M1C_AC + " ls_M1C_I=" + ls_M1C_I + " ls_M1C_ACI=" + ls_M1C_ACI);
                    //Console.WriteLine(ex.StackTrace);
                    throw ex;
                }
            }
            try

            {

                //   '============================================
                //   'M4C
                lsDT1 = (Convert.ToInt16(lsM1.Substring(0, lsM1.Length - 2)) - 1).ToString() + "12";
                ADO.Admin_Card_Count drM4C = new Admin_Card_Count();
                drM4C = drM4C.getAdmin_Card_Count(lsDT1, lsBNo);
                //            lsQry = "SELECT Temp_AZ, Temp_AU, Temp_AX, Temp_AT, Temp_X, Temp_BB, Temp_BA, Temp_AF, Temp_AP, Temp_AK, Temp_AN, " + _
                //                "Temp_U, Temp_O, Temp_AB, Temp_AI, Temp_AC, Temp_I " + _
                //        "FROM Admin_Card_Count " + _
                //        "WHERE YM = @YM and Branch_No = @Branch_No"
                //ds.SelectCommand = lsQry
                //ds.SelectParameters.Clear()
                //ds.SelectParameters.Add("YM", lsDT1)
                //ds.SelectParameters.Add("Branch_No", lsBNo)

                //'Response.Write(lsQry + lsDT1 + lsBNo + "<br>")
                //drM4C = CType(ds.Select(DataSourceSelectArguments.Empty), SqlDataReader)
                if (drM4C != null)
                {

                    ld_M4C_AZ = Convert.ToDouble(drM4C.Temp_AZ);
                    labM4C_AZ = EIFunc.EI_Format(Convert.ToDecimal(ld_M4C_AZ));
                    ld_M4C_AU = Convert.ToDouble(drM4C.Temp_AU);
                    labM4C_AU = EIFunc.EI_Format(Convert.ToDecimal(ld_M4C_AU));
                    ld_M4C_AX = Convert.ToDouble(drM4C.Temp_AX);
                    labM4C_AX = EIFunc.EI_Format(Convert.ToDecimal(ld_M4C_AX));
                    ld_M4C_AT = Convert.ToDouble(drM4C.Temp_AT);
                    labM4C_AT = EIFunc.EI_Format(Convert.ToDecimal(ld_M4C_AT));
                    ld_M4C_X = Convert.ToDouble(drM4C.Temp_X);
                    labM4C_X = EIFunc.EI_Format(Convert.ToDecimal(ld_M4C_X));
                    ld_M4C_BB = Convert.ToDouble(drM4C.Temp_BB);
                    labM4C_BB = EIFunc.EI_Format(Convert.ToDecimal(ld_M4C_BB));
                    ld_M4C_BA = Convert.ToDouble(drM4C.Temp_BA);

                    labM4C_BA = EIFunc.EI_Format(Convert.ToDecimal(ld_M4C_BA));
                    ld_M4C_AF = Convert.ToDouble(drM4C.Temp_AF);
                    labM4C_AF = EIFunc.EI_Format(Convert.ToDecimal(ld_M4C_AF));
                    ld_M4C_AP = Convert.ToDouble(drM4C.Temp_AP);
                    labM4C_AP = EIFunc.EI_Format(Convert.ToDecimal(ld_M4C_AP));

                    labM4C_AK = EIFunc.EI_Format(drM4C.Temp_AK);
                    labM4C_AN = EIFunc.EI_Format(drM4C.Temp_AN);

                    ls_M4C_U = drM4C.Temp_U;
                    labM4C_U = ls_M4C_U;
                    ls_M4C_O = drM4C.Temp_O;
                    labM4C_O = ls_M4C_O;
                    ls_M4C_AB = drM4C.Temp_AB;
                    labM4C_AB = ls_M4C_AB;
                    ls_M4C_AI = drM4C.Temp_AI;
                    labM4C_AI = ls_M4C_AI;

                    ls_M4C_AC = drM4C.Temp_AC.ToString().Replace("%", "");
                    ls_M4C_I = drM4C.Temp_I.ToString().Replace("%", "");
                    if (ls_M4C_AC.Equals("") || ls_M4C_AC.Equals("-"))
                        ls_M4C_AC = "0";

                    if (ls_M4C_I.Equals("") || ls_M4C_I.Equals("-"))

                        ls_M4C_I = "0";
                    ls_M4C_ACI = (Convert.ToDecimal(ls_M4C_AC) - Convert.ToDecimal(ls_M4C_I)).ToString();
                    labS_aci_4mc = Convert.ToDecimal(ls_M4C_ACI) + "%";

                }
            }
            catch (Exception ex)
            {

                //Console.WriteLine("Branch_No" + lsBNo + " lsBNo=" + lsBNo + " ls_M1C_AC=" + ls_M1C_AC + " ls_M1C_I=" + ls_M1C_I + " ls_M1C_ACI=" + ls_M1C_ACI);
                //Console.WriteLine(ex.StackTrace);
                throw ex;
            }
            try
            {
                labS_azGap = EIFunc.EI_Format(Convert.ToDecimal((ld_M1C_AZ - ld_M4C_AZ)));
                if (ld_M1C_AZ > 0)
                    labS_azGapP = (((ld_M1C_AZ - ld_M4C_AZ) / ld_M4C_AZ) * 100).ToString("0.00") + "%";
                else
                    labS_azGapP = "--";


                labS_auGap = EIFunc.EI_Format(Convert.ToDecimal(ld_M1C_AU - ld_M4C_AU));
                if (ld_M4C_AU > 0)
                    labS_auGapP = (((ld_M1C_AU - ld_M4C_AU) / ld_M4C_AU) * 100).ToString("0.00") + "%";
                else
                    labS_auGapP = "--";


                labS_axGap = EIFunc.EI_Format((ld_M1C_AX - ld_M4C_AX).ToString());
                if (ld_M4C_AX > 0)

                    labS_axGapP = (((ld_M1C_AX - ld_M4C_AX) / ld_M4C_AX) * 100).ToString("0.00") + "%";
                else
                    labS_axGapP = "--";


                labS_atGap = EIFunc.EI_Format((ld_M1C_AT - ld_M4C_AT).ToString());
                if (ld_M4C_AT > 0)
                    labS_atGapP = (((ld_M1C_AT - ld_M4C_AT) / ld_M4C_AT) * 100).ToString("0.00") + "%";
                else
                    labS_atGapP = "--";


                labS_xGap = EIFunc.EI_Format((ld_M1C_X - ld_M4C_X).ToString());
                if (ld_M4C_X > 0)
                    labS_xGapP = (((ld_M1C_X - ld_M4C_X) / ld_M4C_X) * 100).ToString("0.00") + "%";
                else
                    labS_xGapP = "--";

                labS_bbGap = EIFunc.EI_Format((ld_M1C_BB - ld_M4C_BB).ToString());
                if (ld_M4C_BB > 0)
                    labS_bbGapP = (((ld_M1C_BB - ld_M4C_BB) / ld_M4C_BB) * 100).ToString("0.00") + "%";
                else
                    labS_bbGapP = "--";


                labS_baGap = EIFunc.EI_Format((ld_M1C_BA - ld_M4C_BA).ToString());
                if (ld_M4C_BA > 0)
                    labS_baGapP = (((ld_M1C_BA - ld_M4C_BA) / ld_M4C_BA) * 100).ToString("0.00").ToString() + "%";
                else
                    labS_baGapP = "--";


                ld_M1C_XBBBA = ld_M1C_X - ld_M1C_BB - ld_M1C_BA;
                labS_xbbba = EIFunc.EI_Format(ld_M1C_XBBBA.ToString());
                ld_M4C_XBBBA = ld_M4C_X - ld_M4C_BB - ld_M4C_BA;
                labS_xbbba_M4C = EIFunc.EI_Format(ld_M4C_XBBBA.ToString());
                labS_xbbba_gap = EIFunc.EI_Format((ld_M1C_XBBBA - ld_M4C_XBBBA).ToString());

                if (ld_M4C_XBBBA > 0)
                    labS_xbbba_gapP = (((ld_M1C_XBBBA - ld_M4C_XBBBA) / ld_M4C_XBBBA) * 100).ToString("0.00") + "%";
                else
                    labS_xbbba_gapP = "--";

                labS_afGap = EIFunc.EI_Format((ld_M1C_AF - ld_M4C_AF).ToString());
                labS_apGap = EIFunc.EI_Format((ld_M1C_AP - ld_M4C_AP).ToString());

                ls_M1C_U = ls_M1C_U.Replace("%", "");

                ls_M4C_U = ls_M4C_U.Replace("%", "");
                if (ls_M1C_U.Equals("") || ls_M1C_U.Equals("-"))
                    ls_M1C_U = "0";

                if (ls_M4C_U.Equals("") || ls_M4C_U.Equals("-"))
                    ls_M4C_U = "0";

                labS_uGap = (Convert.ToDecimal(ls_M1C_U) - Convert.ToDecimal(ls_M4C_U)).ToString("0.00") + "%";


                ls_M1C_O = ls_M1C_O.Replace("%", "");
                ls_M4C_O = ls_M4C_O.Replace("%", "");
                if (ls_M1C_O.Equals("") || ls_M1C_O.Equals("-"))
                    ls_M1C_O = "0";

                if (ls_M4C_O.Equals("") || ls_M4C_O.Equals("-"))
                    ls_M4C_O = "0";


                labS_oGap = (Convert.ToDecimal(ls_M1C_O) - Convert.ToDecimal(ls_M4C_O)).ToString("0.00") + "%";


                ls_M1C_AB = ls_M1C_AB.Replace("%", "");
                ls_M4C_AB = ls_M4C_AB.Replace("%", "");
                if (ls_M1C_AB.Equals("") || ls_M1C_AB.Equals("-"))
                    ls_M1C_AB = "0";

                if (ls_M4C_AB.Equals("") || ls_M4C_AB.Equals("-"))
                    ls_M4C_AB = "0";

                labS_abGap = (Convert.ToDecimal(ls_M1C_AB) - Convert.ToDecimal(ls_M4C_AB)).ToString("0.00") + "%";


                ls_M1C_AI = ls_M1C_AI.Replace("%", "");
                ls_M4C_AI = ls_M4C_AI.Replace("%", "");
                if (ls_M1C_AI.Equals("") || ls_M1C_AI.Equals("-"))
                    ls_M1C_AI = "0";

                if (ls_M4C_AI.Equals("") || ls_M4C_AI.Equals("-"))
                    ls_M4C_AI = "0";

                labS_aiGap = (Convert.ToDecimal(ls_M1C_AI) - Convert.ToDecimal(ls_M4C_AI)).ToString("0.00") + "%";
                if (string.IsNullOrEmpty(ls_M4C_ACI))
                    ls_M4C_ACI = "0";
                if (string.IsNullOrEmpty(ls_M1C_ACI))
                    ls_M1C_ACI = "0";
                labS_aci_Gap = (Convert.ToDecimal(ls_M1C_ACI) - Convert.ToDecimal(ls_M4C_ACI)).ToString("0.00") + "%";

            }
            catch (Exception ex)
            {
                //Console.WriteLine("YM=" + lsDT1);
                //Console.WriteLine("Branch_No=" + lsBNo);
                //Console.WriteLine(ex.ToString());

                throw ex;

            }

            //        'M1_T15_1=================================================================================================
            string labM1T15_1_Buy_Exp_Amt = "0";
            string labM1T15_1_Sell_Imp_Amt = "0";
            string labM1T15_1_Exchage_Total_Amt = "0";
            string labM1T15_1_Total_Amt = "0";

            ADO.ImportFile_15_1 drM1_T15_1 = new ImportFile_15_1();
            drM1_T15_1 = drM1_T15_1.getImportFile_51(lsM1, lsBNo);
            if (drM1_T15_1 != null)
            {
                labM1T15_1_Buy_Exp_Amt = EIFunc.EI_Format(drM1_T15_1.Buy_Exp_Amt);
                labM1T15_1_Sell_Imp_Amt = EIFunc.EI_Format(drM1_T15_1.Sell_Imp_Amt);
                labM1T15_1_Exchage_Total_Amt = EIFunc.EI_Format(drM1_T15_1.Exchage_Total_Amt);
                labM1T15_1_Total_Amt = EIFunc.EI_Format(drM1_T15_1.Total_Amt);

            }






            // 'M1_T51===================================================================================================
            string labM1T51_OB_Save_Amt = "0";
            string labM1T51_OB_Out_Amt = "0";
            string labM1T51_OB_Save_Amt_OBU = "0";
            string labM1T51_OB_Out_Amt_OBU = "0";
            ADO.ImportFile_51 drM1_T51 = new ImportFile_51();
            drM1_T51.getImportFile_51(lsM1, lsBNo);


            if (drM1_T51 != null)
            {


                labM1T51_OB_Save_Amt = EIFunc.EI_Format(drM1_T51.OB_Save_Amt);
                labM1T51_OB_Out_Amt = EIFunc.EI_Format(drM1_T51.OB_Out_Amt);
                labM1T51_OB_Save_Amt_OBU = EIFunc.EI_Format(drM1_T51.OB_Save_Amt_OBU);
                labM1T51_OB_Out_Amt_OBU = EIFunc.EI_Format(drM1_T51.OB_Out_Amt_OBU);
            }
            //     'M1_T52===================================================================================================
            string labM1T52_Buy_Exp_Amt = "0";
            string labM1T52_Sell_Imp_Amt = "0";
            string labM1T52_Exchage_Total_Amt = "0";
            string labM1T52_Total_Amt = "0";
            string labM1T52_Total_Percentage = "0";
            string labM1T52_OB_Save_Amt = "0";
            string labM1T52_Save_Percentage = "0";
            string labM1T52_OB_Out_Amt = "0";
            string labM1T52_Out_Percentage = "0";
            string labM1T52_OB_Save_Amt_OBU = "0";
            string labM1T52_Save_Percentage_OBU = "0";
            string labM1T52_OB_Out_Amt_OBU = "0";
            string labM1T52_Out_Percentage_OBU = "0";
            ADO.ImportFile_52 drM1_T52 = new ImportFile_52();
            drM1_T52.getImportFile_52(lsM1, lsBNo);




            if (drM1_T52 != null)
            {


                labM1T52_Buy_Exp_Amt = EIFunc.EI_Format(drM1_T52.Buy_Exp_Amt);
                labM1T52_Sell_Imp_Amt = EIFunc.EI_Format(drM1_T52.Sell_Imp_Amt);
                labM1T52_Exchage_Total_Amt = EIFunc.EI_Format(drM1_T52.Exchage_Total_Amt);
                labM1T52_Total_Amt = EIFunc.EI_Format(drM1_T52.Total_Amt);
                labM1T52_Total_Percentage = drM1_T52.Total_Percentage;
                labM1T52_OB_Save_Amt = EIFunc.EI_Format(drM1_T52.OB_Save_Amt);
                labM1T52_Save_Percentage = drM1_T52.Save_Percentage;
                labM1T52_OB_Out_Amt = EIFunc.EI_Format(drM1_T52.OB_Out_Amt);
                labM1T52_Out_Percentage = drM1_T52.Out_Percentage;
                labM1T52_OB_Save_Amt_OBU = EIFunc.EI_Format(drM1_T52.OB_Save_Amt_OBU);
                labM1T52_Save_Percentage_OBU = drM1_T52.Save_Percentage_OBU;
                labM1T52_OB_Out_Amt_OBU = EIFunc.EI_Format(drM1_T52.OB_Out_Amt_OBU);
                labM1T52_Out_Percentage_OBU = drM1_T52.Out_Percentage_OBU;

            }
            //              'M1_T53===================================================================================================
            string labM1T53_D = "0";
            string labM1T53_G = "0";
            string labM1T53_K = "0";
            string labM1T53_N = "0";
            string labM1T53_Q = "0.00%";
            string labM1T53_E = "0";
            string labM1T53_H = "0";
            string labM1T53_L = "0";
            string labM1T53_O = "0";
            string labM1T53_R = "0.00%";
            string labM1T53_C = "0";
            string labM1T53_F = "0";
            string labM1T53_I = "0";
            string labM1T53_J = "0.00%";
            string labM1T53_M = "0";
            string labM1T53_P = "0";
            string labM1T53_S = "0.00%";
            ADO.ImportFile_53 drM1_T53 = new ImportFile_53();
            drM1_T53 = drM1_T53.getImportFile_53(lsM1, lsBNo);




            if (drM1_T53 != null)
            {


                // '財富管理業務手續費收入
                labM1T53_D = EIFunc.EI_Format(drM1_T53.Temp_D);
                labM1T53_G = EIFunc.EI_Format(drM1_T53.Temp_G);
                labM1T53_K = EIFunc.EI_Format(drM1_T53.Temp_K);
                labM1T53_N = EIFunc.EI_Format(drM1_T53.Temp_N);
                labM1T53_Q = drM1_T53.Temp_Q;// '%
                labM1T53_E = EIFunc.EI_Format(drM1_T53.Temp_E);
                labM1T53_H = EIFunc.EI_Format(drM1_T53.Temp_H);
                labM1T53_L = EIFunc.EI_Format(drM1_T53.Temp_L);
                labM1T53_O = EIFunc.EI_Format(drM1_T53.Temp_O);
                labM1T53_R = drM1_T53.Temp_R;// '%
                labM1T53_C = EIFunc.EI_Format(drM1_T53.Temp_C);
                labM1T53_F = EIFunc.EI_Format(drM1_T53.Temp_F);
                labM1T53_I = EIFunc.EI_Format(drM1_T53.Temp_I);
                labM1T53_J = drM1_T53.Temp_J;//  '%
                labM1T53_M = EIFunc.EI_Format(drM1_T53.Temp_M);
                labM1T53_P = EIFunc.EI_Format(drM1_T53.Temp_P);
                labM1T53_S = drM1_T53.Temp_S;// '%

            }


            //    'M1_T54===================================================================================================
            Double ldTemp_C, ldTemp_D, ldTemp_E, ldTemp_F, ldTemp_G, ldTemp_H ,ldTemp_I, ldTemp_J,ldTemp_K,ldTemp_L, ldTemp_M, ldTemp_N, ldTemp_O, ldTemp_P, ldTemp_Q, ldTemp_R;
            Double ldTotal_1, ldTotal_2;
            string labM1T54_C = "0";
            string labM1T54_D = "0";
            string labM1T54_E = "0";
            string labM1T54_F = "0";
            string labM1T54_G = "0";
            string labM1T54_H = "0";
            string labM1T54_I = "0";
            string labM1T54_J = "0";
            string labM1T54_K = "0";
            string labM1T54_L = "0";
            string labM1T54_M = "0";
            string labM1T54_N = "0";
            string labM1T54_O = "0";
            string labM1T54_P = "0";
            string labM1T54_Q = "0";
            string labM1T54_R = "0";
            string labM1T54_Total1 = "0";
            string labM1T54_Total2 = "0";
            ADO.ImportFile_54 drM1_T54 = new ImportFile_54();
            drM1_T54 = drM1_T54.getImportFile_54(lsM1, lsBNo);


            if (drM1_T54 != null)
            {

                ldTemp_C = Convert.ToDouble(drM1_T54.Temp_C);
                ldTemp_D = Convert.ToDouble(drM1_T54.Temp_D);
                ldTemp_E = Convert.ToDouble(drM1_T54.Temp_E);
                ldTemp_F = Convert.ToDouble(drM1_T54.Temp_F);
                ldTemp_G = Convert.ToDouble(drM1_T54.Temp_G);
                ldTemp_H = Convert.ToDouble(drM1_T54.Temp_H);
                ldTemp_I = Convert.ToDouble(drM1_T54.Temp_I);
                ldTemp_J = Convert.ToDouble(drM1_T54.Temp_J);
                ldTemp_K = Convert.ToDouble(drM1_T54.Temp_K);
                ldTemp_L = Convert.ToDouble(drM1_T54.Temp_L);
                ldTemp_M = Convert.ToDouble(drM1_T54.Temp_M);
                ldTemp_N= Convert.ToDouble(drM1_T54.Temp_N);
                ldTemp_O = Convert.ToDouble(drM1_T54.Temp_O);
                ldTemp_P = Convert.ToDouble(drM1_T54.Temp_P);
                ldTemp_Q = Convert.ToDouble(drM1_T54.Temp_Q);
                ldTemp_R = Convert.ToDouble(drM1_T54.Temp_R);

                //'信託業務
                labM1T54_C = EIFunc.EI_Format(ldTemp_C.ToString());
                labM1T54_E = EIFunc.EI_Format(ldTemp_E.ToString());
                labM1T54_G= EIFunc.EI_Format(ldTemp_G.ToString());
                labM1T54_I = EIFunc.EI_Format(ldTemp_I.ToString());
                labM1T54_K = EIFunc.EI_Format(ldTemp_K.ToString());
                labM1T54_M = EIFunc.EI_Format(ldTemp_M.ToString());
                labM1T54_O = EIFunc.EI_Format(ldTemp_O.ToString());
                labM1T54_Q = EIFunc.EI_Format(ldTemp_Q.ToString());
                ldTotal_1 = ldTemp_C + ldTemp_E + ldTemp_I + ldTemp_Q+ ldTemp_G + ldTemp_K+ ldTemp_M+ ldTemp_O;
                labM1T54_Total1 = EIFunc.EI_Format(ldTotal_1.ToString());

                labM1T54_D = EIFunc.EI_Format(ldTemp_D.ToString());
                labM1T54_F = EIFunc.EI_Format(ldTemp_F.ToString());
                labM1T54_H = EIFunc.EI_Format(ldTemp_H.ToString());
                labM1T54_L = EIFunc.EI_Format(ldTemp_L.ToString());
                labM1T54_N = EIFunc.EI_Format(ldTemp_N.ToString());
                labM1T54_J = EIFunc.EI_Format(ldTemp_J.ToString());
                labM1T54_P = EIFunc.EI_Format(ldTemp_P.ToString());
                labM1T54_R = EIFunc.EI_Format(ldTemp_R.ToString());
                ldTotal_2 = ldTemp_D + ldTemp_F + ldTemp_J + ldTemp_R+ ldTemp_H+ ldTemp_L+ ldTemp_N+ ldTemp_P;
                labM1T54_Total2 = EIFunc.EI_Format(ldTotal_2.ToString());


            }

            // 'Month2
            string lsM2Year = "";
            string lsM2Month = "";
            //  'M2
            string labM2_D = "", labM2_E = "", labM2_F, labM2_G, labM2_H = "", labM2_I = "", labM2_J, labM2_K, labM2_L = "", labM2_M = "", labM2_N = "";
            string labM2_O = "", labM2_P = "", labM2_Q = "", labM2_R = "", labM2_S = "", labM2_T = "", labM2_U = "", labM2_V = "", labM2_W = "", labM2_X = "", labM2_Y = "";
            string labM2_Z = "", labM2_AA = "", labM2_AB = "", labM2_AC, labM2_AD = "", labM2_AE, labM2_AF, labM2_AG, labM2_AH = "", labM2_AI = "";
            string labM2_AJ = "", labM2_AK = "", labM2_AL = "", labM2_AM = "", labM2_AN = "", labM2_AO = "", labM2_AP = "", labM2_AQ = "", labM2sut = "";
            Decimal ld_M2_S = 0, ld_M2_U = 0, ld_M2_T = 0, ld_M2_SUT = 0;
            //   'M2C
            string labM2C_AG = "", labM2C_AH = "";

            string labMonth2_1 = "";
            if ((lsM2 != null) || (lsM2 == "0"))
            {
                lsMonth2 = lsM2;
                lsM2Year = lsMonth2.Substring(0, lsMonth2.Length - 2);
                lsM2Month = lsMonth2.Replace(lsM2Year, "");
                labMonth2_1 = lsM2Year + "年 " + lsM2Month + "月";
                // 'msg = getM2Value(lsMonth2, lsBNo, msg)
                //'msg = getM2CValue(lsMonth2, lsBNo, msg)
                // 'M2=======================================================================================================
                ADO.Admin_Card drM2 = new Admin_Card();
                drM2 = drM2.getAdmin_Card(lsM2, lsBNo);



                if (drM2 != null)
                {

                    labM2_D = EIFunc.EI_Format(drM2.Temp_D);
                    labM2_Q = EIFunc.EI_Format(drM2.Temp_Q);
                    labM2_E = EIFunc.EI_Format(drM2.Temp_E);
                    labM2_R = EIFunc.EI_Format(drM2.Temp_R);
                    labM2_H = EIFunc.EI_Format(drM2.Temp_H);
                    labM2_I = EIFunc.EI_Format(drM2.Temp_I);
                    labM2_AQ = EIFunc.EI_Format(drM2.Temp_AQ);
                    labM2_L = EIFunc.EI_Format(drM2.Temp_L);
                    labM2_P = EIFunc.EI_Format(drM2.Temp_P);


                    ld_M2_S = Convert.ToDecimal(drM2.Temp_S);
                    labM2_S = EIFunc.EI_Format(ld_M2_S);
                    ld_M2_U = drM2.Temp_U;
                    labM2_U = EIFunc.EI_Format(ld_M2_U);
                    ld_M2_T = drM2.Temp_T;
                    labM2_T = EIFunc.EI_Format(ld_M2_T);
                    ld_M2_SUT = ld_M2_S - ld_M2_U - ld_M2_T;
                    labM2sut = EIFunc.EI_Format(ld_M2_SUT);


                    labM2_Y = EIFunc.EI_Format(drM2.Temp_Y);
                    labM2_X = EIFunc.EI_Format(drM2.Temp_X);
                    labM2_AI = EIFunc.EI_Format(drM2.Temp_AI);
                    //  'labM2_AI = Format(CType(drM2.Item("Temp_AI").ToString(), Decimal), "###,###")


                    labM2_AN = drM2.Temp_AN;//   '%
                    labM2_AO = drM2.Temp_AO;//  '%
                    labM2_AP = drM2.Temp_AP;//   '%
                    labM2_Z = drM2.Temp_Z;//    '%
                    labM2_AK = drM2.Temp_AK;//  '%
                    labM2_AL = drM2.Temp_AL;//  '%
                    labM2_AM = drM2.Temp_AM;// ").ToString()   '%



                }
                else
                {
                    labM2_D = "0";
                    labM2_E = "0";
                    labM2_F = "0";
                    labM2_G = "0";
                    labM2_H = "0";
                    labM2_I = "0";
                    labM2_J = "0";
                    labM2_K = "0";
                    labM2_L = "0";
                    labM2_M = "0";
                    labM2_N = "0";
                    labM2_O = "0";
                    labM2_P = "0";
                    labM2_Q = "0";
                    labM2_R = "0";
                    labM2_S = "0";
                    labM2_T = "0";
                    labM2_U = "0";
                    labM2_V = "0";
                    labM2_W = "0";
                    labM2_X = "0";
                    labM2_Y = "0";
                    labM2_Z = "0.00%";//  '%
                    labM2_AA = "0";
                    labM2_AB = "0";
                    labM2_AC = "0";
                    labM2_AD = "0";
                    labM2_AE = "0";
                    labM2_AF = "0";
                    labM2_AG = "0";
                    labM2_AH = "0";
                    labM2_AI = "0";
                    labM2_AJ = "0";
                    labM2_AK = "0.00%";//    '%
                    labM2_AL = "0.00%";//   '%
                    labM2_AM = "0.00%";  //  '%
                    labM2_AN = "0.00%";   //'%
                    labM2_AO = "0.00%"; //  '%
                    labM2_AP = "0.00%";  //  '%
                    labM2_AQ = "0";
                    labM2sut = "0";
                }


                //   'M2C======================================================================================================
                ADO.Admin_Card_Count drM2C = new Admin_Card_Count();
                drM2C = drM2C.getAdmin_Card_Count(lsM2, lsBNo);


                if (drM2C != null)
                {

                    labM2C_AG = EIFunc.EI_Format(drM2C.Temp_AG);
                    labM2C_AH = EIFunc.EI_Format(drM2C.Temp_AH);


                }
                else
                {
                    labM2C_AG = "0";
                    labM2C_AH = "0";
                }
            }
            //    'Month3
            string lsM3Year = "";
            string lsM3Month = "";
            //'M3
            string labM3_D = "", labM3_E = "", labM3_F = "", labM3_G = "", labM3_H = "", labM3_I = "", labM3_J, labM3_K = "", labM3_L = "", labM3_M, labM3_N;
            string labM3_O = "", labM3_P = "", labM3_Q = "", labM3_R = "", labM3_S = "", labM3_T = "", labM3_U = "", labM3_V, labM3_W = "", labM3_X = "", labM3_Y = "";
            string labM3_Z = "", labM3_AA, labM3_AB, labM3_AC, labM3_AD = "", labM3_AE, labM3_AF, labM3_AG = "", labM3_AH = "", labM3_AI = "";
            string labM3_AJ = "", labM3_AK = "", labM3_AL = "", labM3_AM = "", labM3_AN = "", labM3_AO = "", labM3_AP = "", labM3_AQ = "", labM3sut = "";
            Decimal ld_M3_S = 0, ld_M3_U = 0, ld_M3_T = 0, ld_M3_SUT = 0;
            // 'M3C
            string labM3C_AG = "", labM3C_AH = "";

            string labMonth3_1 = "";
            if ((lsM3 != null) || (lsM3 == "0"))
            {
                lsMonth3 = lsM3;
                lsM3Year = lsMonth3.Substring(0, lsMonth3.Length - 2);
                lsM3Month = lsMonth3.Replace(lsM3Year, "");
                labMonth3_1 = lsM3Year + "年 " + lsM3Month + "月";
                // 'msg = getM3Value(lsMonth3, lsBNo, msg)
                // 'msg = getM3CValue(lsMonth3, lsBNo, msg)
                //   'M3=======================================================================================================
                ADO.Admin_Card drM3 = new Admin_Card();
                drM3 = drM3.getAdmin_Card(lsM3, lsBNo);


                if (drM3 != null)
                {


                    labM3_D = EIFunc.EI_Format(drM3.Temp_D);
                    labM3_Q = EIFunc.EI_Format(drM3.Temp_Q);
                    labM3_E = EIFunc.EI_Format(drM3.Temp_E);
                    labM3_R = EIFunc.EI_Format(drM3.Temp_R);
                    labM3_H = EIFunc.EI_Format(drM3.Temp_H);
                    labM3_I = EIFunc.EI_Format(drM3.Temp_I);
                    labM3_AQ = EIFunc.EI_Format(drM3.Temp_AQ);
                    labM3_L = EIFunc.EI_Format(drM3.Temp_L);
                    labM3_P = EIFunc.EI_Format(drM3.Temp_P);


                    ld_M3_S = Convert.ToDecimal(drM3.Temp_S);
                    labM3_S = EIFunc.EI_Format(ld_M3_S);
                    ld_M3_U = Convert.ToDecimal(drM3.Temp_U);
                    labM3_U = EIFunc.EI_Format(ld_M3_U);
                    ld_M3_T = Convert.ToDecimal(drM3.Temp_T);
                    labM3_T = EIFunc.EI_Format(ld_M3_T);
                    ld_M3_SUT = ld_M3_S - ld_M3_U - ld_M3_T;
                    labM3sut = EIFunc.EI_Format(ld_M3_SUT);


                    labM3_Y = EIFunc.EI_Format(drM3.Temp_Y);
                    labM3_X = EIFunc.EI_Format(drM3.Temp_X);
                    labM3_AI = EIFunc.EI_Format(drM3.Temp_AI);
                    //   'labM3_AI = Format(CType(drM3.Item("Temp_AI").ToString(), Decimal), "###,###")


                    labM3_AN = drM3.Temp_AN;//   '%
                    labM3_AO = drM3.Temp_AO; // '%
                    labM3_AP = drM3.Temp_AP;//   '%
                    labM3_Z = drM3.Temp_Z;//     '%
                    labM3_AK = drM3.Temp_AK;//   '%
                    labM3_AL = drM3.Temp_AL;// '%
                    labM3_AM = drM3.Temp_AM;//  '%


                }
                else
                {
                    labM3_D = "0";
                    labM3_E = "0";
                    labM3_F = "0";
                    labM3_G = "0";
                    labM3_H = "0";
                    labM3_I = "0";
                    labM3_J = "0";
                    labM3_K = "0";
                    labM3_L = "0";
                    labM3_M = "0";
                    labM3_N = "0";
                    labM3_O = "0";
                    labM3_P = "0";
                    labM3_Q = "0";
                    labM3_R = "0";
                    labM3_S = "0";
                    labM3_T = "0";
                    labM3_U = "0";
                    labM3_V = "0";
                    labM3_W = "0";
                    labM3_X = "0";
                    labM3_Y = "0";
                    labM3_Z = "0.00%";//  '%
                    labM3_AA = "0";
                    labM3_AB = "0";
                    labM3_AC = "0";
                    labM3_AD = "0";
                    labM3_AE = "0";
                    labM3_AF = "0";
                    labM3_AG = "0";
                    labM3_AH = "0";
                    labM3_AI = "0";
                    labM3_AJ = "0";
                    labM3_AK = "0.00%";//   '%
                    labM3_AL = "0.00%";//   '%
                    labM3_AM = "0.00%";   //'%
                    labM3_AN = "0.00%"; //  '%
                    labM3_AO = "0.00%"; //  '%
                    labM3_AP = "0.00%";  // '%
                    labM3_AQ = "0";
                    labM3sut = "0";
                }

                // 'M3C======================================================================================================
                ADO.Admin_Card_Count drM3C = new Admin_Card_Count();
                drM3C = drM3C.getAdmin_Card_Count(lsM3, lsBNo);



                if (drM3C != null)
                {

                    labM3C_AG = EIFunc.EI_Format(drM3C.Temp_AG);
                    labM3C_AH = EIFunc.EI_Format(drM3C.Temp_AH);




                }
                else
                {
                    labM3C_AG = "0";
                    labM3C_AH = "0";
                }
            }

            // 'Month4
            string lsM4Year = "";
            string lsM4Month = "";
            //'M4
            string labM4_D = "", labM4_E = "", labM4_F = "", labM4_G = "", labM4_H = "", labM4_I = "", labM4_J = "", labM4_K = "", labM4_L = "", labM4_M = "", labM4_N = "";
            string labM4_O = "", labM4_P = "", labM4_Q = "", labM4_R = "", labM4_S = "", labM4_T = "", labM4_U = "", labM4_V = "", labM4_W = "", labM4_X = "", labM4_Y = "";
            string labM4_Z = "", labM4_AA = "", labM4_AB = "", labM4_AC = "", labM4_AD = "", labM4_AE = "", labM4_AF = "", labM4_AG = "", labM4_AH = "", labM4_AI = "";
            string labM4_AJ = "", labM4_AK = "", labM4_AL = "", labM4_AM = "", labM4_AN = "", labM4_AO = "", labM4_AP = "", labM4_AQ = "", labM4sut = "";
            Decimal ld_M4_S = 0, ld_M4_U = 0, ld_M4_T = 0, ld_M4_SUT = 0;
            //  'M4CM
            string labM4CM_AG, labM4CM_AH;
            string labMonth4_1 = "";

            if ((lsM4 != null) || (lsM4 == "0"))
            {
                lsMonth4 = lsM4;
                lsM4Year = lsMonth4.Substring(0, lsMonth4.Length - 2);
                lsM4Month = lsMonth4.Replace(lsM4Year, "");
                labMonth4_1 = lsM4Year + "年 " + lsM4Month + "月";
                //  'msg = getM4Value(lsMonth4, lsBNo, msg)
                //  'msg = getM4CValue(lsMonth4, lsBNo, msg)
                // 'M4=======================================================================================================
                ADO.Admin_Card drM4 = new Admin_Card();
                drM4 = drM4.getAdmin_Card(lsM4, lsBNo);




                if (drM4 != null)
                {



                    labM4_D = EIFunc.EI_Format(drM4.Temp_D);
                    labM4_Q = EIFunc.EI_Format(drM4.Temp_Q);
                    labM4_E = EIFunc.EI_Format(drM4.Temp_E);
                    labM4_R = EIFunc.EI_Format(drM4.Temp_R);
                    labM4_H = EIFunc.EI_Format(drM4.Temp_H);
                    labM4_I = EIFunc.EI_Format(drM4.Temp_I);
                    labM4_AQ = EIFunc.EI_Format(drM4.Temp_AQ);
                    labM4_L = EIFunc.EI_Format(drM4.Temp_L);
                    labM4_P = EIFunc.EI_Format(drM4.Temp_P);


                    ld_M4_S = drM4.Temp_S;
                    labM4_S = EIFunc.EI_Format(ld_M4_S);
                    ld_M4_U = drM4.Temp_U;
                    labM4_U = EIFunc.EI_Format(ld_M4_U);
                    ld_M4_T = drM4.Temp_T;
                    labM4_T = EIFunc.EI_Format(ld_M4_T);
                    ld_M4_SUT = ld_M4_S - ld_M4_U - ld_M4_T;
                    labM4sut = EIFunc.EI_Format(ld_M4_SUT);


                    labM4_Y = EIFunc.EI_Format(drM4.Temp_Y);
                    labM4_X = EIFunc.EI_Format(drM4.Temp_X);
                    labM4_AI = EIFunc.EI_Format(drM4.Temp_AI);
                    // 'labM4_AI = Format(CType(drM4.Item("Temp_AI").ToString(), Decimal), "###,###")


                    labM4_AN = drM4.Temp_AN; // '%
                    labM4_AO = drM4.Temp_AO;//  '%
                    labM4_AP = drM4.Temp_AP;//   '%
                    labM4_Z = drM4.Temp_Z;//    '%
                    labM4_AK = drM4.Temp_AK;//   '%
                    labM4_AL = drM4.Temp_AL;//   '%
                    labM4_AM = drM4.Temp_AM;//  '%



                }
                else
                {
                    labM4_D = "0";
                    labM4_E = "0";
                    labM4_F = "0";
                    labM4_G = "0";
                    labM4_H = "0";
                    labM4_I = "0";
                    labM4_J = "0";
                    labM4_K = "0";
                    labM4_L = "0";
                    labM4_M = "0";
                    labM4_N = "0";
                    labM4_O = "0";
                    labM4_P = "0";
                    labM4_Q = "0";
                    labM4_R = "0";
                    labM4_S = "0";
                    labM4_T = "0";
                    labM4_U = "0";
                    labM4_V = "0";
                    labM4_W = "0";
                    labM4_X = "0";
                    labM4_Y = "0";
                    labM4_Z = "0.00%";//  '%
                    labM4_AA = "0";
                    labM4_AB = "0";
                    labM4_AC = "0";
                    labM4_AD = "0";
                    labM4_AE = "0";
                    labM4_AF = "0";
                    labM4_AG = "0";
                    labM4_AH = "0";
                    labM4_AI = "0";
                    labM4_AJ = "0";
                    labM4_AK = "0.00%";//    '%
                    labM4_AL = "0.00%";//   '%
                    labM4_AM = "0.00%";   //'%
                    labM4_AN = "0.00%"; //  '%
                    labM4_AO = "0.00%"; //  '%
                    labM4_AP = "0.00%"; //  '%
                    labM4_AQ = "0";
                    labM4sut = "0";
                }
            }
            ADO.Admin_Card_Count drM4CM = new Admin_Card_Count();
            drM4CM = drM4CM.getAdmin_Card_Count(lsM4, lsBNo);
            if (drM4CM != null)
            {

                labM4CM_AG = EIFunc.EI_Format(drM4CM.Temp_AG);
                labM4CM_AH = EIFunc.EI_Format(drM4CM.Temp_AH);

            }
            else
            {
                labM4CM_AG = "0";
                labM4CM_AH = "0";
            }
            try
            {
                string fontchinese, font10, font12, font14, font16;
                font10 = "<font size='10'>";
                font12 = "<font size='12'>";
                font14 = "<font size='14'>";
                font16 = "<font size='16'>";

                msg = "<html><body>";
                msg += "<table border=0 width=90% align=center cellpadding=0 cellspacing=0><tr>";
                msg += "<td colspan=2>" + ADO.Until.getBranch_Name(lsBNo) + " 營運概況表</td>";
                lsTempYM = lsM1.Substring(0, lsM1.Length - 2);
                lsYear = lsTempYM;
                lsMonth = lsM1.Replace(lsYear, "");
                msg += "<td nowrap>" + lsYear + "年" + lsMonth + "月</td>";
                msg += "<td nowrap>單位代號：" + lsBNo + "</td>";
                msg += "<td nowrap>單位：仟元</td>";
                msg += "</tr>";

                // '===========================================================================================
                //  '一、主要業務達成情形(累計平均餘額
                msg += "<tr>";
                msg += "<td colspan=5>一、主要業務達成情形(累計平均餘額)</td>";
                msg += "</tr>";
                msg += "<tr><td colspan=5>";

                msg += "<table width=90% border=1 align=center cellpadding=1 cellspacing=0 >";
                msg += "<tr>";
                msg += "<td colspan=6 rowspan=2 align=center nowrap>項目</td>";//    'width=110 
                msg += "<td colspan=4 rowspan=2 align=center >本月止達成</td>";//   'width=95
                msg += "<td colspan=3 rowspan=2 align=center >達成率</td>";//     ' width=95
                msg += "<td colspan=4 rowspan=2 align=center >上年度達成額</td>";//  'width=95
                msg += "<td colspan=8 align=center >較上年度增減</td>";
                msg += "<td colspan=4 rowspan=17 align=center valign=top width=100>";//      'width=95
                msg += "<table border=1 align=center cellpadding=2 cellspacing=0 width=100% >";//
                msg += "<tr>";
                msg += "<td align=center>成立日期</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td align=center>" + labChief_Setup_Date + "</td>";// 'yyyy/MM/dd
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td align=center>經理</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td align=center>" + labChief_Manager + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td align=center>到任日期</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td align=center>" + labChief_Join_Date + "</td>";//  'yyyy/MM/dd
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td align=center>副理</td>";
                msg += "</tr>";
                msg += "<tr>";
                if (labChief_Vice_Manager_1 == "")
                    msg += "<td align=center> &nbsp;</td>";
                else
                    msg += "<td align=center>" + labChief_Vice_Manager_1 + "</td>";

                msg += "</tr>";
                msg += "<tr>";
                msg += "<td align=center>副理</td>";
                msg += "</tr>";
                msg += "<tr>";
                if (labChief_Vice_Manager_2 == "")
                    msg += "<td align=center> &nbsp;</td>";
                else
                    msg += "<td align=center>" + labChief_Vice_Manager_2 + " &nbsp;</td>";

                msg += "</tr>";
                msg += "<tr>";
                msg += "<td align=center>副理</td>";
                msg += "</tr>";
                msg += "<tr>";

                msg += "</tr>";
                msg += "</table>";
                msg += "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td colspan=4 align=center >金額</td>"; //'w//idth=95;
                msg += "<td colspan=4 align=center >比率</td>";// 'width=95;
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td rowspan=7 align=center>存<br />&nbsp;<br />款</td>";//    'bgcolor=#FFF3C4
                msg += "<td colspan=5 nowrap>總存款(不含外幣)</td>";
                msg += "<td colspan=4 align=right >" + labM1C_E + "</td>";
                msg += "<td colspan=3 align=right >" + labM1C_F + "</td>";//       '%
                msg += "<td colspan=4 align=right >" + labM1C_G + "</td>";
                msg += "<td colspan=4 align=right >" + labS_EG + "</td>";
                msg += "<td colspan=4 align=right >" + labM1C_H + "</td>";//       '%
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td colspan=5 align=right >一般性存款</td>";
                msg += "<td colspan=4 align=right >" + labM1C_K + "</td>";
                msg += "<td colspan=3 align=right >&nbsp;</td>";
                msg += "<td colspan=4 align=right >" + labM1C_L + "</td>";
                msg += "<td colspan=4 align=right >" + labS_KL + "</td>";
                msg += "<td colspan=4 align=right >" + labM1C_N + "</td>";//   '%
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td colspan=5 >活期性存款</td>";
                msg += "<td colspan=4 align=right >" + labM1C_Q + "</td>";
                msg += "<td colspan=3 align=right >" + labM1C_S + "</td>";//   '%
                msg += "<td colspan=4 align=right >" + labM1C_R + "</td>";
                msg += "<td colspan=4 align=right >" + labS_QR + "</td>";
                msg += "<td colspan=4 align=right >" + labM1C_T + "</td>";//   '%
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td colspan=5 align=right >一般性存款</td>";
                msg += "<td colspan=4 align=right>" + labM1C_AZ + "</td>";
                msg += "<td colspan=3 align=right>&nbsp;</td>";
                msg += "<td colspan=4 align=right>" + labM4C_AZ + "</td>";
                msg += "<td colspan=4 align=right>" + labS_azGap + "</td>";
                msg += "<td colspan=4 align=right>" + labS_azGapP + "</td>";
                msg += "</tr>";

                msg += "<tr>";
                msg += "<td colspan=5 align=right >基金</td>";
                msg += "<td colspan=4 align=right>" + labM1C_AU + "</td>";
                msg += "<td colspan=3 align=right>&nbsp;</td>";
                msg += "<td colspan=4 align=right>" + labM4C_AU + "</td>";
                msg += "<td colspan=4 align=right>" + labS_auGap + "</td>";
                msg += "<td colspan=4 align=right>" +labS_auGapP + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td colspan=5 align=right >補償金</td>";
                msg += "<td colspan=4 align=right>" + EIFunc.EI_Format( Convert.ToDecimal(labM1C_AX)) + "</td>";
                msg += "<td colspan=3 align=right>&nbsp;</td>";
                msg += "<td colspan=4 align=right>" + EIFunc.EI_Format(Convert.ToDecimal( labM4C_AX)) + "</td>";
                msg += "<td colspan=4 align=right>" + EIFunc.EI_Format(Convert.ToDecimal( labS_axGap ))+ "</td>";
                msg += "<td colspan=4 align=right>" +  labS_axGapP + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td colspan=5 align=right >公庫活期</td>";
                msg += "<td colspan=4 align=right>" + EIFunc.EI_Format(Convert.ToDecimal(labM1C_AT)) + "</td>";
                msg += "<td colspan=3 align=right>&nbsp;</td>";
                msg += "<td colspan=4 align=right>" + EIFunc.EI_Format(Convert.ToDecimal(labM4C_AT)) + "</td>";
                msg += "<td colspan=4 align=right>" + EIFunc.EI_Format(Convert.ToDecimal(labS_atGap)) + "</td>";
                msg += "<td colspan=4 align=right>" +labS_atGapP+ "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td rowspan=5 align=center>放<br />&nbsp;<br /><br />&nbsp;<br />款</td>";//   'bgcolor=#FFF3C4
                msg += "<td colspan=5 nowrap>自有資金(含外幣)</td>";
                msg += "<td colspan=4 align=right >" + EIFunc.EI_Format(Convert.ToDecimal(labM1C_X ))+ "</td>";
                msg += "<td colspan=3 align=right >" +labM1C_Z + "</td>";
                msg += "<td colspan=4 align=right >" + EIFunc.EI_Format(Convert.ToDecimal(labM4C_X)) + "</td>";
                msg += "<td colspan=4 align=right >" + EIFunc.EI_Format(Convert.ToDecimal(labS_xGap)) + "</td>";
                msg += "<td colspan=4 align=right >" +  labS_xGapP + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td colspan=5 align=right >專業放款</td>";
                msg += "<td colspan=4 align=right>" + EIFunc.EI_Format(Convert.ToDecimal(labM1C_BB)) + "</td>";
                msg += "<td colspan=3 align=right>&nbsp;</td>";
                msg += "<td colspan=4 align=right>" + EIFunc.EI_Format(Convert.ToDecimal(labM4C_BB)) + "</td>";
                msg += "<td colspan=4 align=right>" + EIFunc.EI_Format(Convert.ToDecimal(labS_bbGap)) + "</td>";
                msg += "<td colspan=4 align=right>" +  labS_bbGapP + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td colspan=5 align=right >個金放款</td>";
                msg += "<td colspan=4 align=right>" + EIFunc.EI_Format(Convert.ToDecimal( labM1C_BA)) + "</td>";
                msg += "<td colspan=3 align=right>&nbsp;</td>";
                msg += "<td colspan=4 align=right>" + EIFunc.EI_Format(Convert.ToDecimal(labM4C_BA)) + "</td>";
                msg += "<td colspan=4 align=right>" + EIFunc.EI_Format(Convert.ToDecimal(labS_baGap)) + "</td>";
                msg += "<td colspan=4 align=right>" +  labS_baGapP + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td colspan=5 >外幣放款</td>";
                msg += "<td colspan=4 align=right >" + EIFunc.EI_Format(Convert.ToDecimal(labS_xbbba)) + "</td>";
                msg += "<td colspan=3 align=right >&nbsp;</td>";
                msg += "<td colspan=4 align=right >" + EIFunc.EI_Format(Convert.ToDecimal(labS_xbbba_M4C)) + "</td>";
                msg += "<td colspan=4 align=right >" + EIFunc.EI_Format(Convert.ToDecimal(labS_xbbba_gap)) + "</td>";
                msg += "<td colspan=4 align=right >" + labS_xbbba_gapP + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td colspan=5 >逾期放款</td>";
                msg += "<td colspan=4 align=right >" + EIFunc.EI_Format(Convert.ToDecimal(labM1C_AF)) + "</td>";
                msg += "<td colspan=3 align=right >&nbsp;</td>";
                msg += "<td colspan=4 align=right >" + EIFunc.EI_Format(Convert.ToDecimal(labM4C_AF)) + "</td>";
                msg += "<td colspan=4 align=right >" + EIFunc.EI_Format(Convert.ToDecimal(labS_afGap)) + "</td>";
                msg += "<td colspan=4 align=right >&nbsp;</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td align=center colspan=6 >外匯 單位:仟美元</td>";
                msg += "<td colspan=4 align=right >" + labM1C_AK + "</td>";
                msg += "<td colspan=3 align=right >" + labM1C_AL + "</td>";
                msg += "<td colspan=4 align=right >" + labM4C_AK + "</td>";
                msg += "<td colspan=4 align=right >&nbsp;</td>";
                msg += "<td colspan=4 align=right >&nbsp;</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td colspan=6 align=center>提存前盈餘</td>";
                msg += "<td align=right colspan=4 >" + labM1C_AN + "</td>";
                msg += "<td align=right colspan=3 >" + labM1C_AO + "</td>";
                msg += "<td align=right colspan=4 >" + labM4C_AN + "</td>";
                msg += "<td align=right colspan=4 >&nbsp;</td>";
                msg += "<td align=right colspan=4 >&nbsp;</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td colspan=6 align=center>員工人數</td>";
                msg += "<td colspan=4 align=right >" + labM1C_AP + "</td>";
                msg += "<td colspan=3 align=right >&nbsp;</td>";
                msg += "<td colspan=4 align=right>" + labM4C_AP + "</td>";
                msg += "<td colspan=4 align=right >" + labS_apGap + "</td>";
                msg += "<td colspan=4 align=right >&nbsp;</td>";
                msg += "</tr>";
                msg += "</table>";
                msg += "</td></tr>";

                msg += "<tr><td colspan=5>";
                msg += "<table width=90% border=1 align=center cellpadding=2 cellspacing=0 >";
                msg += "<tr height=15>";
                msg += "<td colspan=5 align=center valign=middle >項目</td>";
                msg += "<td colspan=3 align=center valign=middle >本月比率</td>";
                msg += "<td colspan=3 align=center valign=middle >上年度比率</td>";
                msg += "<td colspan=3 align=center valign=middle >增減比率</td>";
                msg += "<td colspan=4 align=center valign=middle >年度別</td>";
                msg += "<td colspan=3 align=center valign=middle >組別</td>";
                msg += "<td colspan=3 align=center valign=middle >同組分行數</td>";
                msg += "<td colspan=3 align=center valign=middle >考核名次</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td rowspan=5 align=center valign=middle>經<br />營<br />指<br />標</td>";
                msg += "<td colspan=4 align=right>活存比率</td>";
                msg += "<td colspan=3 align=right>" + labM1C_U + " </td>";
                msg += "<td colspan=3 align=right>" + labM4C_U + " </td>";
                msg += "<td colspan=3 align=right>" + labS_uGap + " </td>";
                msg += "<td rowspan=5 align=center valign=middle>組<br />別<br />暨<br />考<br />核<br />名<br />次</td>";
                msg += "<td colspan=3 align=right>" + labR1_Year + " </td>";
                msg += "<td colspan=3 align=right>" + labR1_Class + " </td>";
                msg += "<td colspan=3 align=right>" + labR1_Total + " </td>";
                msg += "<td colspan=3 align=right>" + labR1_Placings + " </td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td colspan=4 align=right>一般存款比率</td>";
                msg += "<td colspan=3 align=right>" + labM1C_O + " </td>";
                msg += "<td colspan=3 align=right>" + labM4C_O + " </td>";
                msg += "<td colspan=3 align=right>" + labS_oGap + " </td>";
                msg += "<td colspan=3 align=right>" + labR2_Year + " </td>";
                msg += "<td colspan=3 align=right>" + labR2_Class + " </td>";
                msg += "<td colspan=3 align=right>" + labR2_Total + " </td>";
                msg += "<td colspan=3 align=right>" + labR2_Placings + " </td>";
                msg += "</tr>";
                msg += "<td colspan=4 align=right>存放比率</td>";
                msg += "<td colspan=3 align=right>" + labM1C_AB + " </td>";
                msg += "<td colspan=3 align=right>" + labM4C_AB + " </td>";
                msg += "<td colspan=3 align=right>" + labS_abGap + " </td>";
                msg += "<td colspan=3 align=right>" + labR3_Year + " </td>";
                msg += "<td colspan=3 align=right>" + labR3_Class + " </td>";
                msg += "<td colspan=3 align=right>" + labR3_Total + " </td>";
                msg += "<td colspan=3 align=right>" + labR3_Placings + " </td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td colspan=4 align=right>逾放比率</td>";
                msg += "<td colspan=3 align=right>" + labM1C_AI + " </td>";
                msg += "<td colspan=3 align=right>" + labM4C_AI + " </td>";
                msg += "<td colspan=3 align=right>" + labS_aiGap + " </td>";
                msg += "<td colspan=3 align=right>" + labR4_Year + " </td>";
                msg += "<td colspan=3 align=right>" + labR4_Class + " </td>";
                msg += "<td colspan=3 align=right>" + labR4_Total + " </td>";
                msg += "<td colspan=3 align=right>" + labR4_Placings + " </td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td colspan=4 align=right>存放款利差</td>";
                msg += "<td colspan=3 align=right>" + labS_aci + " </td>";
                msg += "<td colspan=3 align=right>" + labS_aci_4mc + " </td>";
                msg += "<td colspan=3 align=right>" + labS_aci_Gap + " </td>";
                msg += "<td colspan=3 align=right>" + labR5_Year + " </td>";
                msg += "<td colspan=3 align=right>" + labR5_Class + " </td>";
                msg += "<td colspan=3 align=right>" + labR5_Total + " </td>";
                msg += "<td colspan=3 align=right>" + labR5_Placings + " </td>";
                msg += "</tr>";
                msg += "</table>";
                msg += "</td></tr>";

                // '' ===========================================================================================
                //  ''二、主要最近三個月狀況(月平均餘額)
                msg += "<tr><td colspan=5>";
                msg += "<table width=90% border=0 align=center cellpadding=0 cellspacing=0 >";
                msg += "<tr>";
                msg += "<td width=400>二、主要最近三個月狀況(月平均餘額)</td>";
                msg += "<td width=300 align=right >單位：仟元</td>";
                msg += "</tr>";
                msg += "</table>";
                msg += "<table width=90% border=1 align=center cellpadding=1 cellspacing=0 >";
                msg += "<tr>";
                msg += "<td colspan=8 align=center >項目</td>";
                msg += "<td colspan=6 align=center >" + labMonth1_1 + "</td>";
                msg += "<td colspan=6 align=center >" + labMonth2_1 + "</td>";
                msg += "<td colspan=6 align=center >" + labMonth3_1 + "</td>";
                msg += "<td colspan=6 align=center >" + labMonth4_1 + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td rowspan=9 align=center>存款</td>";
                msg += "<td colspan=7 nowrap >總存款(不含外幣)</td>";
                msg += "<td colspan=6 align=right >" + labM1_D + "</td>";
                msg += "<td colspan=6 align=right >" + labM2_D + "</td>";
                msg += "<td colspan=6 align=right >" + labM3_D + "</td>";
                msg += "<td colspan=6 align=right >" + labM4_D + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td colspan=7 align=right>一般性存款</td>";
                msg += "<td colspan=6 align=right>" + labM1_Q + "</td>";
                msg += "<td colspan=6 align=right>" + labM2_Q + "</td>";
                msg += "<td colspan=6 align=right>" + labM3_Q + "</td>";
                msg += "<td colspan=6 align=right>" + labM4_Q + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td colspan=7 >活期性存款</td>";
                msg += "<td colspan=6 align=right >" + labM1_E + "</td>";
                msg += "<td colspan=6 align=right >" + labM2_E + "</td>";
                msg += "<td colspan=6 align=right >" + labM3_E + "</td>";
                msg += "<td colspan=6 align=right >" + labM4_E + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td colspan=7 align=right>一般性存款</td>";
                msg += "<td colspan=6 align=right>" + labM1_R + "</td>";
                msg += "<td colspan=6 align=right>" + labM2_R + "</td>";
                msg += "<td colspan=6 align=right>" + labM3_R + "</td>";
                msg += "<td colspan=6 align=right>" + labM4_R + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td colspan=7 align=right>基金</td>";
                msg += "<td colspan=6 align=right>" + labM1_H + "</td>";
                msg += "<td colspan=6 align=right>" + labM2_H + "</td>";
                msg += "<td colspan=6 align=right>" + labM3_H + "</td>";
                msg += "<td colspan=6 align=right>" + labM4_H + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td colspan=7 align=right>補償金</td>";
                msg += "<td colspan=6 align=right>" + labM1_I + "</td>";
                msg += "<td colspan=6 align=right>" + labM2_I + "</td>";
                msg += "<td colspan=6 align=right>" + labM3_I + "</td>";
                msg += "<td colspan=6 align=right>" + labM4_I + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td colspan=7 align=right>公庫活期</td>";
                msg += "<td colspan=6 align=right>" + labM1_AQ + "</td>";
                msg += "<td colspan=6 align=right>" + labM2_AQ + "</td>";
                msg += "<td colspan=6 align=right>" + labM3_AQ + "</td>";
                msg += "<td colspan=6 align=right>" + labM4_AQ + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td colspan=7 >定期性存款</td>";
                msg += "<td colspan=6 align=right >" + labM1_L + "</td>";
                msg += "<td colspan=6 align=right >" + labM2_L + "</td>";
                msg += "<td colspan=6 align=right >" + labM3_L + "</td>";
                msg += "<td colspan=6 align=right >" + labM4_L + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td colspan=7 >公庫存款</td>";
                msg += "<td colspan=6 align=right >" + labM1_P + "</td>";
                msg += "<td colspan=6 align=right >" + labM2_P + "</td>";
                msg += "<td colspan=6 align=right >" + labM3_P + "</td>";
                msg += "<td colspan=6 align=right >" + labM4_P + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td rowspan=7 align=center >放款</td>";
                msg += "<td colspan=7 nowrap>自有資金放款(含外幣)</td>";
                msg += "<td colspan=6 align=right >" + labM1_S + "</td>";
                msg += "<td colspan=6 align=right >" + labM2_S + "</td>";
                msg += "<td colspan=6 align=right >" + labM3_S + "</td>";
                msg += "<td colspan=6 align=right >" + labM4_S + "</td>";
                msg += "</tr>";
                msg += "<tr>"; ;
                msg += "<td colspan=7 align=right>專業放款</td>";
                msg += "<td colspan=6 align=right>" + labM1_U + "</td>";
                msg += "<td colspan=6 align=right>" + labM2_U + "</td>";
                msg += "<td colspan=6 align=right>" + labM3_U + "</td>";
                msg += "<td colspan=6 align=right>" + labM4_U + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td colspan=7 align=right>個金放款</td>";
                msg += "<td colspan=6 align=right>" + labM1_T + "</td>";
                msg += "<td colspan=6 align=right>" + labM2_T + "</td>";
                msg += "<td colspan=6 align=right>" + labM3_T + "</td>";
                msg += "<td colspan=6 align=right>" + labM4_T + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td colspan=7 >外幣放款</td>";
                msg += "<td colspan=6 align=right >" + labM1sut + "</td>";
                msg += "<td colspan=6 align=right >" + labM2sut + "</td>";
                msg += "<td colspan=6 align=right >" + labM3sut + "</td>";
                msg += "<td colspan=6 align=right >" + labM4sut + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td colspan=7 >逾期放款</td>";
                msg += "<td colspan=6 align=right >" + labM1_Y + "</td>";
                msg += "<td colspan=6 align=right >" + labM2_Y + "</td>";
                msg += "<td colspan=6 align=right >" + labM3_Y + "</td>";
                msg += "<td colspan=6 align=right >" + labM4_Y + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td colspan=7 align=right>甲類</td>";
                msg += "<td colspan=6 align=right>" + labM1C_AG + "</td>";
                msg += "<td colspan=6 align=right>" + labM2C_AG + "</td>";
                msg += "<td colspan=6 align=right>" + labM3C_AG + "</td>";
                msg += "<td colspan=6 align=right>" + labM4CM_AG + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td colspan=7 align=right>乙類</td>";
                msg += "<td colspan=6 align=right>" + labM1C_AH + "</td>";
                msg += "<td colspan=6 align=right>" + labM2C_AH + "</td>";
                msg += "<td colspan=6 align=right>" + labM3C_AH + "</td>";
                msg += "<td colspan=6 align=right>" + labM4CM_AH + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td colspan=8 align=center >外匯(當月) 單位:仟美元</td>";
                msg += "<td colspan=6 align=right >" + labM1_X + "</td>";
                msg += "<td colspan=6 align=right >" + labM2_X + "</td>";
                msg += "<td colspan=6 align=right >" + labM3_X + "</td>";
                msg += "<td colspan=6 align=right >" + labM4_X + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td colspan=8 align=center >提存前盈餘(當月)</td>";
                msg += "<td colspan=6 align=right >" + labM1_AI + "</td>";
                msg += "<td colspan=6 align=right >" + labM2_AI + "</td>";
                msg += "<td colspan=6 align=right >" + labM3_AI + "</td>";
                msg += "<td colspan=6 align=right >" + labM4_AI + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td rowspan=7 align=center>經營指標</td>";
                msg += "<td colspan=7 align=right>活存比率</td>";
                msg += "<td colspan=6 align=right>" + labM1_AN + "</td>";
                msg += "<td colspan=6 align=right>" + labM2_AN + "</td>";
                msg += "<td colspan=6 align=right>" + labM3_AN + "</td>";
                msg += "<td colspan=6 align=right>" + labM4_AN + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td colspan=7 align=right>一般存款比率</td>";
                msg += "<td colspan=6 align=right>" + labM1_AO + "</td>";
                msg += "<td colspan=6 align=right>" + labM2_AO + "</td>";
                msg += "<td colspan=6 align=right>" + labM3_AO + "</td>";
                msg += "<td colspan=6 align=right>" + labM4_AO + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td colspan=7 align=right>存放比率</td>";
                msg += "<td colspan=6 align=right>" + labM1_AP + "</td>";
                msg += "<td colspan=6 align=right>" + labM2_AP + "</td>";
                msg += "<td colspan=6 align=right>" + labM3_AP + "</td>";
                msg += "<td colspan=6 align=right>" + labM4_AP + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td colspan=7 align=right>逾放比率</td>";
                msg += "<td colspan=6 align=right>" + labM1_Z + "</td>";
                msg += "<td colspan=6 align=right>" + labM2_Z + "</td>";
                msg += "<td colspan=6 align=right>" + labM3_Z + "</td>";
                msg += "<td colspan=6 align=right>" + labM4_Z + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td colspan=7 align=right>存款平均利率</td>";
                msg += "<td colspan=6 align=right>" + labM1_AK + "</td>";
                msg += "<td colspan=6 align=right>" + labM2_AK + "</td>";
                msg += "<td colspan=6 align=right>" + labM3_AK + "</td>";
                msg += "<td colspan=6 align=right>" + labM4_AK + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td colspan=7 align=right>放款平均利率</td>";
                msg += "<td colspan=6 align=right>" + labM1_AL + "</td>";
                msg += "<td colspan=6 align=right>" + labM2_AL + "</td>";
                msg += "<td colspan=6 align=right>" + labM3_AL + "</td>";
                msg += "<td colspan=6 align=right>" + labM4_AL + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td colspan=7 align=right>存放款利差</td>";
                msg += "<td colspan=6 align=right>" + labM1_AM + "</td>";
                msg += "<td colspan=6 align=right>" + labM2_AM + "</td>";
                msg += "<td colspan=6 align=right>" + labM3_AM + "</td>";
                msg += "<td colspan=6 align=right>" + labM4_AM + "</td>";
                msg += "</tr>";
                msg += "</table>";
                msg += "</td></tr>";
                // 'msg += "</table></body></html>"

                // '' ===========================================================================================
                //  ''三、外匯業務概況
                //  'msg1 = "<html><body>"

                msg += "<tr><td colspan=5>";
                msg += "<table width=90% border=1 align=center cellpadding=1 cellspacing=0 >";
                msg += "<tr>";
                msg += "<td colspan=3 align=center>" + ADO.Until.getBranch_Name(lsBNo) + "&nbsp;&nbsp;&nbsp;" + lsYear + "年" + lsMonth + "月外匯業務概況</td>";
                msg += "<td align=center >單位:         仟美元</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td align=center >&nbsp;</td>";
                msg += "<td align=center >本月</td>";
                msg += "<td align=center >累計</td>";
                msg += "<td align=center >達成率</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td align=center >&nbsp;</td>";
                msg += "<td align=center >(1)</td>";
                msg += "<td align=center >(2)</td>";
                msg += "<td align=center >(3)</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td align=center>出口外匯業務承作量</td>";
                msg += "<td align=right>" + labM1T15_1_Buy_Exp_Amt + "</td>";
                msg += "<td align=right>" + labM1T52_Buy_Exp_Amt + "</td>";
                msg += "<td align=right>&nbsp;</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td align=center>進口業務業務承作量</td>";
                msg += "<td align=right>" + labM1T15_1_Sell_Imp_Amt + "</td>";
                msg += "<td align=right>" + labM1T52_Sell_Imp_Amt + "</td>";
                msg += "<td align=right>&nbsp;</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td align=center>匯兌業務業務承作量</td>";
                msg += "<td align=right>" + labM1T15_1_Exchage_Total_Amt + "</td>";
                msg += "<td align=right>" + labM1T52_Exchage_Total_Amt + "</td>";
                msg += "<td align=right>&nbsp;</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td align=center>合計</td>";
                msg += "<td align=right>" + labM1T15_1_Total_Amt + "</td>";
                msg += "<td align=right>" + labM1T52_Total_Amt + "</td>";
                msg += "<td align=right>" + labM1T52_Total_Percentage + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td align=center>外匯存款平均餘額</td>";
                msg += "<td align=right>" + labM1T51_OB_Save_Amt_OBU + "</td>";
                msg += "<td align=right>" + labM1T52_OB_Save_Amt_OBU + "</td>";
                msg += "<td align=right>" + labM1T52_Save_Percentage_OBU + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td align=center>外幣放款平均餘額</td>";
                msg += "<td align=right>" + labM1T51_OB_Out_Amt_OBU + "</td>";
                msg += "<td align=right>" + labM1T52_OB_Out_Amt_OBU + "</td>";
                msg += "<td align=right>" + labM1T52_Out_Percentage_OBU + "</td>";
                msg += "</tr>";
                msg += "</table>";

                //    '' ===========================================================================================
                //      ''四、財富管理業務手續費收入
                msg += "<table width=90% border=0 align=center cellpadding=0 cellspacing=0 >";
                msg += "<tr><td colspan=2> &nbsp;</td></tr>";
                msg += "<tr>";
                msg += "<td width=400> &nbsp;</td>";
                msg += "<td width=300 align=right >單位：新臺幣仟元</td>";
                msg += "</tr>";
                msg += "</table>";
                msg += "</td></tr>";
                msg += "<tr><td colspan=5>";
                msg += "<table width=90% border=1 align=center cellpadding=1 cellspacing=0 >";
                msg += "<tr>";
                msg += "<td width=490 colspan=5 align=center>" + ADO.Until.getBranch_Name(lsBNo) + "&nbsp;&nbsp;&nbsp;" + lsYear + "年" + lsMonth + "月財富管理業務手續費收入</td>";
                msg += "<td width=70 align=center>" + labMonth1_4 + "</td>";
                msg += "<td width=140 colspan=2 align=center>與去年同期比較</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td width=210 align=left>項目</td>";
                msg += "<td width=70 align=center>本年度目標</td>";
                msg += "<td width=70 align=center>本月承作</td>";
                msg += "<td width=70 align=center>累計承作</td>";
                msg += "<td width=70 align=center>累計達成率</td>";
                msg += "<td width=70 align=center>累計承作</td>";
                msg += "<td width=70 align=center>金額</td>";
                msg += "<td width=70 align=center>比率</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td width=210 align=left>受託投資有價證券業務</td>";
                msg += "<td width=70 align=center>--</td>";
                msg += "<td width=70 align=right>" + labM1T53_D + "</td>";
                msg += "<td width=70 align=right>" + labM1T53_G + "</td>";
                msg += "<td width=70 align=center>--</td>";
                msg += "<td width=70 align=right>" + labM1T53_K + "</td>";
                msg += "<td width=70 align=right>" + labM1T53_N + "</td>";
                msg += "<td width=70 align=right>" + labM1T53_Q + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td width=210 align=left>銀行保險業務</td>";
                msg += "<td width=70 align=center>--</td>";
                msg += "<td width=70 align=right>" + labM1T53_E + "</td>";
                msg += "<td width=70 align=right>" + labM1T53_H + "</td>";
                msg += "<td width=70 align=center>--</td>";
                msg += "<td width=70 align=right>" + labM1T53_L + "</td>";
                msg += "<td width=70 align=right>" + labM1T53_O + "</td>";
                msg += "<td width=70 align=right>" + labM1T53_R + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td width=210 align=left>合計</td>";
                msg += "<td width=70 align=right>" + labM1T53_C + "</td>";
                msg += "<td width=70 align=right>" + labM1T53_F + "</td>";
                msg += "<td width=70 align=right>" + labM1T53_I + "</td>";
                msg += "<td width=70 align=right>" + labM1T53_J + "</td>";
                msg += "<td width=70 align=right>" + labM1T53_M + "</td>";
                msg += "<td width=70 align=right>" + labM1T53_P + "</td>";
                msg += "<td width=70 align=right>" + labM1T53_S + "</td>";
                msg += "</tr>";
                msg += "</table>";
                msg += "</td></tr>";

                //  '' ===========================================================================================
                //    ''五、信託業務 手續費收入
                msg += "<tr><td colspan=5>";
                msg += "<table width=90% border=0 align=center cellpadding=0 cellspacing=0 >";
                msg += "<tr><td colspan=2> &nbsp;</td></tr>";
                msg += "<tr>";
                msg += "<td width=400 align=center>&nbsp;</td>";
                msg += "<td width=300 align=right >單位:新臺幣元</td>";
                msg += "</tr>";
                msg += "</table>";

                msg += "<table width=90% border=1 align=center cellpadding=1 cellspacing=0>";
                msg += "<tr>";
                msg += "<td width=550 colspan=3 align=center>";
                msg += Until.getBranch_Name(lsBNo) + "&nbsp;&nbsp;&nbsp;" + lsYear + "年" + lsMonth + "月信託業務&nbsp;手續費收入(以聯行貼補息方式撥付)</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td width=234 align=right>期間</td>";
                msg += "<td width=233 align=center rowspan=2 valign=middle>本月</td>";
                msg += "<td width=233 align=center rowspan=2 valign=middle>本年度累計</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td width=234 align=left>項目</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td width=234 align=left>受託投資有價證券業務</td>";
                msg += "<td width=233 align=right>" + labM1T54_C + "</td>";
                msg += "<td width=233 align=right>" + labM1T54_D + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td width=234 align=left>不動產信託業務</td>";
                msg += "<td width=233 align=right>" + labM1T54_E + "</td>";
                msg += "<td width=233 align=right>" + labM1T54_F + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td width=234 align=left>租賃權信託業務</td>";
                msg += "<td width=233 align=right>" + labM1T54_G + "</td>";
                msg += "<td width=233 align=right>" + labM1T54_H + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td width=234 align=left>簽證業務</td>";
                msg += "<td width=233 align=right>" + labM1T54_I + "</td>";
                msg += "<td width=233 align=right>" + labM1T54_J + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td width=234 align=left>地上權信託業務</td>";
                msg += "<td width=233 align=right>" + labM1T54_K + "</td>";
                msg += "<td width=233 align=right>" + labM1T54_L + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td width=234 align=left>公司債發行受託人</td>";
                msg += "<td width=233 align=right>" + labM1T54_M + "</td>";
                msg += "<td width=233 align=right>" + labM1T54_N + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td width=234 align=left>外國債及結構債券</td>";
                msg += "<td width=233 align=right>" + labM1T54_M + "</td>";
                msg += "<td width=233 align=right>" + labM1T54_N + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td width=234 align=left>財產信託業務</td>";
                msg += "<td width=233 align=right>" + labM1T54_Q + "</td>";
                msg += "<td width=233 align=right>" + labM1T54_R + "</td>";
                msg += "</tr>";
                msg += "<tr>";
                msg += "<td width=234 align=left>合計數</td>";
                msg += "<td width=233 align=right>" + labM1T54_Total1 + "</td>";
                msg += "<td width=233 align=right>" + labM1T54_Total2 + "</td>";
                msg += "</tr>";
                msg += "</table>";
                msg += "</td></tr>";

                //  '' ===========================================================================================
                //  ''五、信託業務 手續費收入
                msg += "<tr><td colspan=5 align=right>製表日：" + DateTime.Now.ToShortDateString() + "</td>";
                msg += "</tr></table>";
                msg += "</body></html>";
                return msg;
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                throw ex;
            }
        }


        #region 回傳HTML語法
        public string OperatedCardBResult(SearchListViewModel Search)
        {
            string msg = "";

            msg = OperatedCardB(Search);

            return msg;
        }
        #endregion
    }
}