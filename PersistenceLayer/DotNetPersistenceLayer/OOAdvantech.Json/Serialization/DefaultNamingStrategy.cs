namespace OOAdvantech.Json.Serialization
{
    /// <summary>
    /// The default naming strategy. Property names and dictionary keys are unchanged.
    /// </summary>
    /// <MetaDataID>{32e92125-3d7d-4db9-875a-05c1e93606e2}</MetaDataID>
    public class DefaultNamingStrategy : NamingStrategy
    {
        /// <summary>
        /// Resolves the specified property name.
        /// </summary>
        /// <param name="name">The property name to resolve.</param>
        /// <returns>The resolved property name.</returns>
        protected override string ResolvePropertyName(string name)
        {
            return name;
        }
    }
}