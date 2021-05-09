using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.PersistenceLayerRunTime
{
    public class ValueTypePath:System.Collections.Generic.Stack<OOAdvantech.MetaDataRepository.MetaObjectID>
    {
        public ValueTypePath(ValueTypePath copyValueTypePath):base(copyValueTypePath)
        {

        }
        public ValueTypePath()
        {
        }
        public override string ToString()
        {
            string returnValue="";
            foreach (MetaDataRepository.MetaObjectID identity in this)
            {
                returnValue += identity.ToString();
            }
            return returnValue;
        }
        public static bool operator ==(ValueTypePath left, ValueTypePath right)
        {


            if (object.ReferenceEquals(left, null) && object.ReferenceEquals(right , null))
                return true;
            if (object.ReferenceEquals(left, null))
                return false;
            if (object.ReferenceEquals(right, null))
                return false;
            return left.ToString() == right.ToString();
         
        }

        public static bool operator !=(ValueTypePath left, ValueTypePath right)
        {
            return !(left == right);
        }



    }
}
