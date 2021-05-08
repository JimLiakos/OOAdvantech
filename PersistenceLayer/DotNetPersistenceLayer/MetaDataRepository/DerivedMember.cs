using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.MetaDataRepository
{
    /// <MetaDataID>{1e341ed5-acc5-4df4-b059-e1a447353fac}</MetaDataID>
    public interface IDerivedMemberExpression
    {
        /// <MetaDataID>{70cf18f4-ac26-4704-bc1a-22b0209b9693}</MetaDataID>
        System.Linq.IQueryable QueryableCollection
        {
            get;
        }


    }
    /// <MetaDataID>{8c43a4af-808c-4d45-9b82-0742fdebb306}</MetaDataID>
    public interface IDerivedMemberExpression<TSource, TResult> : IDerivedMemberExpression
    {

        /// <MetaDataID>{fba6e949-4af4-4ba0-80e9-866bf4bab1fc}</MetaDataID>
        IEnumerable<TResult> GetCollection(TSource _object);
        /// <MetaDataID>{025695f5-cfd0-4ac3-8c74-d0e4513ca478}</MetaDataID>
        TResult GetValue(TSource _object);
    }

    /// <MetaDataID>{30a896af-5e3c-49bf-a0b5-a911cd1c2c05}</MetaDataID>
    [AttributeUsage(AttributeTargets.Property)]
    public class DerivedMember : System.Attribute
    {
        Type DerivedMemberExpressionType;
        IDerivedMemberExpression DerivedMemberExpression;

#if !NETCompactFramework
        /// <MetaDataID>{fd44b35b-085a-4a18-9589-b871933ea623}</MetaDataID>
        public System.Linq.Expressions.Expression Expression
        {
            get
            {
                if (DerivedMemberExpression == null)
                    DerivedMemberExpression = System.Activator.CreateInstance(DerivedMemberExpressionType) as IDerivedMemberExpression;
                return DerivedMemberExpression.QueryableCollection.Expression;

            }
        }

        /// <MetaDataID>{0a95f14b-6da0-423f-b82a-7e275732a7d6}</MetaDataID>
        public DerivedMember(Type derivedMemberExpressionType)
        {
            DerivedMemberExpressionType = derivedMemberExpressionType;
            //System.Reflection.PropertyInfo propertyInfo = query.GetProperty("QueryExpression", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            //if(propertyInfo!=null)
            //    Expression = (propertyInfo.GetValue(null, null) as System.Linq.IQueryable).Expression;
        }
#endif
    }
}
