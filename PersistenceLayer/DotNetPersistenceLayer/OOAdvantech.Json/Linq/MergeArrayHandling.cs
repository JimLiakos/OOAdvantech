namespace OOAdvantech.Json.Linq
{
    /// <summary>
    /// Specifies how JSON arrays are merged together.
    /// </summary>
    /// <MetaDataID>{ede907da-d7a5-4c4d-8ad0-39af61c1b397}</MetaDataID>
    public enum MergeArrayHandling
    {
        /// <summary>Concatenate arrays.</summary>
        Concat = 0,

        /// <summary>Union arrays, skipping items that already exist.</summary>
        Union = 1,

        /// <summary>Replace all array items.</summary>
        Replace = 2,

        /// <summary>Merge array items together, matched by index.</summary>
        Merge = 3
    }
}