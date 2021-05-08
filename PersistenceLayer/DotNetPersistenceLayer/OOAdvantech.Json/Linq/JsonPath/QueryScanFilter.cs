using System;
using System.Collections.Generic;

namespace OOAdvantech.Json.Linq.JsonPath
{
    /// <MetaDataID>{a0b02c89-4c6f-44ba-a534-3ca18ddae7ae}</MetaDataID>
    internal class QueryScanFilter : PathFilter
    {
        public QueryExpression Expression { get; set; }

        public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch)
        {
            foreach (JToken t in current)
            {
                if (t is JContainer c)
                {
                    foreach (JToken d in c.DescendantsAndSelf())
                    {
                        if (Expression.IsMatch(root, d))
                        {
                            yield return d;
                        }
                    }
                }
                else
                {
                    if (Expression.IsMatch(root, t))
                    {
                        yield return t;
                    }
                }
            }
        }
    }
}