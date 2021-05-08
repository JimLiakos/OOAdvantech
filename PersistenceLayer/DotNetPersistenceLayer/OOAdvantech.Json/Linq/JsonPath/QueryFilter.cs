using System;
using System.Collections.Generic;

namespace OOAdvantech.Json.Linq.JsonPath
{
    /// <MetaDataID>{9bfc343e-e384-417c-b3ec-f754eb90d21a}</MetaDataID>
    internal class QueryFilter : PathFilter
    {
        public QueryExpression Expression { get; set; }

        public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch)
        {
            foreach (JToken t in current)
            {
                foreach (JToken v in t)
                {
                    if (Expression.IsMatch(root, v))
                    {
                        yield return v;
                    }
                }
            }
        }
    }
}