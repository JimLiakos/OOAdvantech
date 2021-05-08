using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

namespace OOAdvantech.Linq.QueryExpressions
{
    /// <MetaDataID>{f34975b6-4740-453e-a84f-9d9c4c987fbc}</MetaDataID>
    class GroupKeyExpressionTreeNode : ExpressionTreeNode
    {

        public GroupKeyExpressionTreeNode(Expression exp, ExpressionTreeNode parent, Translators.ExpressionVisitor expressionTranslator)
            : base(exp, parent, expressionTranslator)
        { 
            if (!(parent is GroupByExpressionTreeNode))
                throw new System.Exception("Wrong expression type");
        }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="derivedDataNode"></param>
        ///// <param name="targetDataNode"></param>
        ///// <returns></returns>
        //internal static DerivedDataNode BuildDerivedDataNodePath(DerivedDataNode derivedDataNode, DataNode targetDataNode)
        //{

        //    if (derivedDataNode.OrgDataNode == DerivedDataNode.GetOrgDataNode(targetDataNode))
        //        return derivedDataNode;

        //    foreach (var dataNode in derivedDataNode.OrgDataNode.SubDataNodes)
        //    {
        //        if (targetDataNode.IsSameOrParentDataNode(dataNode))
        //        {
        //            DerivedDataNode nextDerivedDataNode = null;
        //            foreach (DerivedDataNode subDerivedDataNode in derivedDataNode.SubDataNodes)
        //            {
        //                if (subDerivedDataNode.OrgDataNode == DerivedDataNode.GetOrgDataNode(dataNode))
        //                {
        //                    nextDerivedDataNode = subDerivedDataNode;
        //                    break;
        //                }
        //            }

        //            if (nextDerivedDataNode == null)
        //            {
        //                nextDerivedDataNode = new DerivedDataNode(dataNode);
        //                nextDerivedDataNode.ParentDataNode = derivedDataNode;
        //            }
        //            return BuildDerivedDataNodePath(nextDerivedDataNode, targetDataNode);

        //        }
        //    }
        //    return null;
        //}


        internal protected IDynamicTypeDataRetrieve BridgeEnumerator;
    }
}
