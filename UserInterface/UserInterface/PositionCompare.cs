using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.UserInterface
{
    /// <MetaDataID>{F64F652B-9388-4A84-9174-8FF6A6F1DC2B}</MetaDataID>
    class PositionCompare<T> : System.Collections.Generic.IComparer<T>
    {



        /// <MetaDataID>{441B1244-B035-478C-89CD-E13A874C5587}</MetaDataID>
        public int Compare(T x, T y)
        {
            if (x == null && y == null)
                return 0;
            if (x == null && y != null)
                return -1;
            if (x != null && y == null)
                return 1;
            short xPosition = 0;
            short yPosition = 0;
            try
            {
                System.Reflection.MemberInfo positionMember = x.GetType().GetMember("Position")[0];
                if (positionMember is System.Reflection.FieldInfo)
                    xPosition = (short)(positionMember as System.Reflection.FieldInfo).GetValue(x);
                if (positionMember is System.Reflection.PropertyInfo)
                    xPosition = (short)(positionMember as System.Reflection.PropertyInfo).GetValue(x, null);

                positionMember = y.GetType().GetMember("Position")[0];
                if (positionMember is System.Reflection.FieldInfo)
                    yPosition = (short)(positionMember as System.Reflection.FieldInfo).GetValue(y);
                if (positionMember is System.Reflection.PropertyInfo)
                    yPosition = (short)(positionMember as System.Reflection.PropertyInfo).GetValue(y, null);
            }
            catch (System.Exception error)
            {

            }
            return xPosition.CompareTo(yPosition);

        }


    }
}
