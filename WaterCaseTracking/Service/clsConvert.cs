using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    class clsConvert
    {
        public object getConvertValue_byType(Type objType,string obj)
        {
            object rv = null;
            switch(objType.Name)
            {
                case "String":
                    rv = obj;
                    break;
                case "Decimal":
                    decimal d = 0;
                    decimal.TryParse(obj,out d);
                    rv = d;
                    break;
            }
            return rv;
        }

        /// <summary>
        /// 西元年月轉換成民國年月(201901 to 10801
        /// </summary>
        /// <param name="strYYYYMM"></param>
        /// <returns></returns>
        public string ADYM_TO_ROCYM(string strYYYYMM,clsEnum.YYMM_TYPE YYMM_TYPE)
        {
            int intYYYYMM = 0;
            int.TryParse(strYYYYMM, out intYYYYMM);
            string ROCYM = "";
            if (intYYYYMM > 0) ROCYM = (intYYYYMM - 191100).ToString().PadLeft(5,'0');
            switch (YYMM_TYPE)
            {
                case clsEnum.YYMM_TYPE.yyymm:
                    break;

                case clsEnum.YYMM_TYPE.年月:
                    if (ROCYM.Length >= 5)
                        ROCYM = ROCYM.Remove(ROCYM.Length- 2)+"年"+ ROCYM.Substring(3,2)+"月";
                    break;
            }
            return ROCYM;
        }
        

        public double ConvertToDouble(string str,string FormatString="")
        {
            double rv = 0;
            double.TryParse(str, out rv);
            return rv;
        }

        /// <summary>
        /// Merge two Dictionary
        /// </summary>
        /// <param name="mainDicty"></param>
        /// <param name="subDicty"></param>
        /// <returns></returns>
        public void MergeDicty(ref Dictionary<string, List<Tuple<string, Type, Object>>> mainDicty, Dictionary<string, List<Tuple<string, Type, Object>>> subDicty)
        {
            mainDicty = mainDicty
                        .ToLookup(z => z.Key)
                        .Concat(subDicty.ToLookup(z => z.Key))
                        .GroupBy(z => z.Key, z => z.Select(y => y.Value))
                        .ToDictionary(z => z.Key, z => z.SelectMany(y => y.SelectMany(u => u)).Distinct().ToList());
            
        } 

    }
}
