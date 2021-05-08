using System.Collections.Generic;

namespace OOAdvantech.Json.Linq.JsonPath
{
    /// <MetaDataID>{7426f4e1-a1ca-4241-938f-0428e4342ebb}</MetaDataID>
    internal class RootFilter : PathFilter
    {
        public static readonly RootFilter Instance = new RootFilter();

        private RootFilter()
        {
        }

        public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch)
        {
            return new[] { root };
        }
    }
}