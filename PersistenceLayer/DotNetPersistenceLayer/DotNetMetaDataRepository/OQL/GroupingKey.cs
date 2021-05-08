using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    /// <MetaDataID>{19474855-f1d1-4125-85e5-d1252e1c6885}</MetaDataID>
    [Serializable]
    public class MultiPartKey
    {
        /// <MetaDataID>{ffdd5455-75b3-44b2-919a-0e4592cc4795}</MetaDataID>
        public object[] KeyPartsValues;
        /// <MetaDataID>{6a3d9de1-793e-4706-9236-c9d7c3bc61df}</MetaDataID>
        public MultiPartKey(int parts)
        {
            KeyPartsValues = new object[parts];
        }

        public MultiPartKey(object[] keyPartsValues)
        {
            KeyPartsValues = keyPartsValues;
        }
        /// <MetaDataID>{2e7016be-14cd-4ce9-9df8-149571aef750}</MetaDataID>
        public override int GetHashCode()
        {
            int num = -1162279000;
            foreach (object partValue in KeyPartsValues)
                num = (-1521134295 * num) + GetHashCode(partValue);
            return num;
        }

        /// <MetaDataID>{43350dff-00e4-4559-9715-bbe7da21bc10}</MetaDataID>
        private int GetHashCode(object partValue)
        {
            if (partValue == null)
                return 0;
            else
                return partValue.GetHashCode();
        }


        /// <MetaDataID>{ae20225d-8ded-4cd3-9187-9c885949fed0}</MetaDataID>
        public override bool Equals(object obj)
        {
            if (obj is MultiPartKey && ((MultiPartKey)obj).KeyPartsValues.Length == KeyPartsValues.Length)
            {
                for (int i = 0; i < KeyPartsValues.Length; i++)
                {
                    object leftValue = KeyPartsValues[i];
                    object rightValue = ((MultiPartKey)obj).KeyPartsValues[i];
                    if (leftValue != null && rightValue == null)
                        return false;

                    if (leftValue == null && rightValue != null)
                        return false;
                    if (leftValue != null && rightValue != null)
                        if (!leftValue.Equals(rightValue))
                            return false;

                }
                return true;

            }
            else
                return false;

        }
    }

    /// <MetaDataID>{6bc20207-d7ee-4cb8-b562-a7f8dd37b8bf}</MetaDataID>
    [Serializable]
    internal class GroupingEntry
    {
        public GroupingEntry(MultiPartKey groupingKey, System.Collections.Generic.List<CompositeRowData> groupedCompositeRows)
        {
            GroupingKey = groupingKey;
            GroupedCompositeRows = groupedCompositeRows;
        }
        public readonly MultiPartKey GroupingKey;
        public readonly System.Collections.Generic.List<CompositeRowData> GroupedCompositeRows;
        


    }
}
