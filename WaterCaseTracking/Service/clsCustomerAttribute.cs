using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{ 
    class clsCustomerAttribute : Attribute
    {
        private string mFieldName;
        private Type mFieldType;

        public string FieldName
        {
            get { return mFieldName; }
            set { mFieldName = value; }
        }
        public Type FieldType
        {
            get { return mFieldType; }
            set { mFieldType = value; }
        }
    }
}
