using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.Linq
{
    public class LINQObjectQuery : OOAdvantech.MetaDataRepository.ObjectQueryLanguage.ObjectQuery, ILINQObjectQuery
    {
        public override bool IsRemovedRow(System.Data.DataRow row)
        {   
            return false;
        }
        public OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode RootDataNode;
        public LINQObjectQuery(System.Linq.Expressions.Expression expression, Translators.ExpressionVisitor translator)
        { 

            translator.LINQObjectQuery = this;
            translator.Translate(expression, null);
            RootDataNode = translator.RootPaths[0];
        }

        #region ILINQObjectQuery Members
        internal System.Type _EnumerableType;
        public Type EnumerableType
        {
            get
            {
                return _EnumerableType;
            }
            set
            {
                _EnumerableType = value;
            }
        }

        #endregion
    }
}
