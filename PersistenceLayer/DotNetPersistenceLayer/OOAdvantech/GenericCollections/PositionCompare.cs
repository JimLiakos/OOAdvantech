using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.Collections.Generic
{
    /// <MetaDataID>{F64F652B-9388-4A84-9174-8FF6A6F1DC2B}</MetaDataID>
    class PositionCompare<T> : System.Collections.Generic.IComparer<T>
    {
        static System.Reflection.MemberInfo PositionMember;
        static PositionCompare()
        {
            PositionMember =typeof(T).GetMember("Position")[0];
        }
    

        /// <MetaDataID>{441B1244-B035-478C-89CD-E13A874C5587}</MetaDataID>
        public int Compare(T x, T y)
        {
            if (x == null && y == null)
                return 0;
            if (x == null && y != null)
                return -1;
            if (x != null && y == null)
                return 1;
            long xPosition = 0;
            long yPosition = 0;
            try
            {
                if (PositionMember is System.Reflection.FieldInfo)
                    xPosition = xPosition = (long)System.Convert.ChangeType((PositionMember as System.Reflection.FieldInfo).GetValue(x), typeof(long),System.Globalization.CultureInfo.CurrentCulture.NumberFormat);
                if (PositionMember is System.Reflection.PropertyInfo)
                    xPosition = (long)System.Convert.ChangeType((PositionMember as System.Reflection.PropertyInfo).GetValue(x, null), typeof(long), System.Globalization.CultureInfo.CurrentCulture.NumberFormat);

                if (PositionMember is System.Reflection.FieldInfo)
                    yPosition = xPosition = (long)System.Convert.ChangeType((PositionMember as System.Reflection.FieldInfo).GetValue(y), typeof(long), System.Globalization.CultureInfo.CurrentCulture.NumberFormat);
                if (PositionMember is System.Reflection.PropertyInfo)
                    yPosition = xPosition = (long)System.Convert.ChangeType((PositionMember as System.Reflection.PropertyInfo).GetValue(y, null), typeof(long), System.Globalization.CultureInfo.CurrentCulture.NumberFormat);
            }
            catch (System.Exception error)
            {

            }
            return xPosition.CompareTo(yPosition);

        }


    }
}
