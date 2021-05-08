using System;

namespace OOAdvantech.Json.Linq
{
    /// <summary>
    /// Specifies how null value properties are merged.
    /// </summary>
    /// <MetaDataID>{019b7fc2-3a88-4c2f-a573-da48a2e9d4b9}</MetaDataID>
    [Flags]
    public enum MergeNullValueHandling
    {
        /// <summary>
        /// The content's null value properties will be ignored during merging.
        /// </summary>
        Ignore = 0,

        /// <summary>
        /// The content's null value properties will be merged.
        /// </summary>
        Merge = 1
    }
}
