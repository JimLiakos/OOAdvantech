using System;
using System.Collections.Generic;

namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{


    ///<summary>
    ///Criterion define self existent condition which acts as is or as part o composite search condition.  
    ///This condition ha two terms and a comparison type like equal, not equal, greater than, like etc. 
    ///Query engine subsystem call the ‘DoesRowPassCondition’ and criterion inform with true or false.
    ///</summary>
    /// <MetaDataID>{728F8A15-8EC4-4229-BDC2-B6D24A6259D8}</MetaDataID>
    [System.Serializable]
    public class Criterion
    {

        /// <MetaDataID>{351c8f06-fc51-41bf-babf-a2f0dfd6df2d}</MetaDataID>
        public object Tag;

        [RoleBMultiplicityRange(1, 1)]
        [Association("SearchCriterion", typeof(SearchFactor), Roles.RoleB, "{73EDD6C7-8D5E-4964-BC25-9DFE5BB30121}")]
        public SearchFactor Owner;

        public enum ComparisonType { Equal = 0, NotEqual, GreaterThan, LessThan, GreaterThanEqual, LessThanEqual, Like, ContainsAny, ContainsAll,TypeIs };
        public enum ComparisonTermsType
        {
            /// <summary>
            /// Compare left term object attribute with the right term also object attribute
            /// </summary>
            ObjectsAttributes = 0,
            /// <summary>
            /// /// Compare left term object attribute with the right term literal
            /// </summary>
            ObjectAttributeWithLiteral,
            /// <summary>
            /// Compare left term object attribute with the right term parameter
            /// </summary>
            ObjectAttributeWithParameter,
            /// <summary>
            /// Compare left term literal with the right term object attribute
            /// </summary>
            LiteralWithObjectAttribute,
            /// <summary>
            /// Compare left term parameter with the right term object attribute
            /// </summary>
            ParameterWithObjectAttribute,
            /// <summary>
            /// Compare left term literal (object Identity) with the right term object 
            /// </summary>
            LiteralWithObject,
            /// <summary>
            /// Compare left term parameter with the right term object 
            /// </summary>
            ParameterWithObject,
            /// <summary>
            /// Compare left term object  with the right term literal  (object Identity)
            /// </summary>
            ObjectWithLiteral,
            /// <summary>
            /// Compare left term object  with the right term parameter 
            /// </summary>
            ObjectWithParameter,

            CollectionContainsAnyAll,
            /// <summary>
            /// Compare left term object  with the right term also object 
            /// </summary>
            Objects
        }

        /// <MetaDataID>{3865de6d-57b2-49cd-8a74-3e5bbe063ccf}</MetaDataID>
        public readonly ComparisonTermsType CriterionType;
        ///<summary>
        ///Defines the left term data node when the source of left term is data node
        ///In case where left term source is literal or parameter   the left term data node is null.
        ///</summary>
        /// <MetaDataID>{74b3c8da-fdb8-4328-911c-68ccab3d6abf}</MetaDataID>
        public readonly DataNode LeftTermDataNode = null;

        public System.Collections.Generic.Stack<DataNode> LeftTermDataNodeRoute
        {
            get
            {
                //if (ComparisonTerms[0] is ObjectAttributeComparisonTerm)
                //    return new System.Collections.Generic.Stack<DataNode>( (ComparisonTerms[0] as ObjectAttributeComparisonTerm).Route);
                //if (ComparisonTerms[0] is ObjectComparisonTerm)
                //    return new System.Collections.Generic.Stack<DataNode>((ComparisonTerms[0] as ObjectComparisonTerm).Route);
                return new System.Collections.Generic.Stack<DataNode>(); 
            }
        }


        public static bool operator ==(Criterion leftCriterion, Criterion rightCriterion)
        {
            if (!(leftCriterion is Criterion) && !(rightCriterion is Criterion))
                return true;

            if (leftCriterion is Criterion && rightCriterion is Criterion)
            {

                if (rightCriterion.ToString() == leftCriterion.ToString())
                    return true;
                return false;

            }
            else
                return false;

        }
        /// <MetaDataID>{3b7f0710-d782-409e-b420-a02c2d0b60d1}</MetaDataID>
        public static bool operator !=(Criterion leftSearchFactor, Criterion rightSearchFactor)
        {
            return !(leftSearchFactor == rightSearchFactor);
        }


        public System.Collections.Generic.Stack<DataNode> RightTermDataNodeRoute
        {
            get
            {
                //if (ComparisonTerms[1] is ObjectAttributeComparisonTerm)
                //    return new System.Collections.Generic.Stack<DataNode>((ComparisonTerms[1] as ObjectAttributeComparisonTerm).Route);
                //if (ComparisonTerms[1] is ObjectComparisonTerm)
                //    return new System.Collections.Generic.Stack<DataNode>((ComparisonTerms[1] as ObjectComparisonTerm).Route);
                return new System.Collections.Generic.Stack<DataNode>();
            }
        }


        ///<summary>
        ///Defines the right term data node when the source of right term is data node
        ///In case where right term source is literal or parameter the right term data node is null.
        ///</summary>
        /// <MetaDataID>{a0701b43-467c-4782-9a01-d61f0197a5f3}</MetaDataID>
        public readonly DataNode RightTermDataNode = null;
        ///<summary>
        ///Literal value is a value which extracted from OQL as string and converted to data node term type. 
        ///If there is not data node as term source the type of literal value is string. 
        ///</summary>
        /// <MetaDataID>{b898d760-829a-4d82-a9e7-edcd83106dd1}</MetaDataID>
        object LiteralValue = null;
        ///<summary>Defines the type of literal value</summary>
        /// <MetaDataID>{48721482-cc55-4c8f-a371-7bf5b02c44ce}</MetaDataID>
        Type LiteralValueType = null;
        /// <MetaDataID>{9b87412c-7ee2-4d17-8d39-26c93501a910}</MetaDataID>
        public ParameterComparisonTerm ParameterComparisonTerm = null;


        /// <exclude>Excluded</exclude>
        [NonSerialized]
        object _ParameterValue;

        /// <MetaDataID>{e991c671-a474-4118-84f2-d1c9dd2309a2}</MetaDataID>
        public object ParameterValue
        {
            get
            {
                if (_ParameterValue == null)
                {
                    if (ParameterComparisonTerm.OQLStatement == null)
                        ParameterComparisonTerm.OQLStatement = OQLStatement;

                    _ParameterValue = ParameterComparisonTerm.ParameterValue;
                }
                return _ParameterValue;
            }
        }

        /// <MetaDataID>{572148eb-49cb-4e3d-85c7-41485c0f2119}</MetaDataID>
        public Type ParameterValueType = null;



        /// <exclude>Excluded</exclude>
        //internal Member<bool> _Applied = new Member<bool>();
        bool? _Applied;

        /// <summary>
        ///Because the persistency system can use many mechanisms save objects state like RDBMS, XML, file stream etc 
        ///the search condition can be resolved at all or some criterions (partially)  
        ///from corresponding data base management mechanisms. 
        ///The criterions where resolved from data base management mechanisms doesn’t applied in query retrieved data in memory.       
        /// </summary>
        /// <MetaDataID>{d21595fc-a097-4078-86bd-f1bf5476c87d}</MetaDataID>
        public bool Applied
        {
            get
            {
                if (!_Applied.HasValue)
                    return false;
                return _Applied.Value;
                //return false;
                //if (_Applied.UnInitialized)
                //{
                //    _Applied.Value = true;
                //    if (LeftTermDataNode != null)
                //    {
                //        DataNode dataNode = LeftTermDataNode;
                //        while(dataNode.Type == DataNode.DataNodeType.OjectAttribute)
                //            dataNode = dataNode.ParentDataNode;

                //        while(dataNode is AggregateExpressionDataNode)
                //            dataNode = dataNode.ParentDataNode;


                //        foreach (var searchCondition in dataNode.SearchConditions)
                //        {
                //            if (searchCondition == null)
                //                continue;
                //            if (searchCondition.Criterions.Contains(this))
                //            {
                //                while (dataNode.ParentDataNode != null && dataNode.ParentDataNode.SearchConditions.Contains(searchCondition))
                //                    dataNode = dataNode.ParentDataNode;
                //            }
                //        }

                //        foreach (System.Collections.Generic.KeyValuePair<string, DataLoader> entry in dataNode.DataSource.DataLoaders)
                //        {
                //            //(OQLStatement as StorageObjectQuery).DistributedObjectQueries[0].DistributedObjectQuery.SearchConditions.cr

                //            if (!(entry.Value is StorageDataLoader))
                //            {
                //                _Applied.Value = false;
                //                break;
                //            }
                //            StorageDataLoader dataLoader = entry.Value as StorageDataLoader;
                //            bool appliedLocaly = false;
                //            foreach (System.Collections.Generic.KeyValuePair<Criterion, System.Collections.Generic.Stack<DataNode>[]> criterionEntry in dataLoader.LocalResolvedCriterions)
                //            {
                //                if (criterionEntry.Key.Identity == Identity)
                //                {
                //                    appliedLocaly = true;
                //                    break;
                //                }
                //            }
                //            foreach (System.Collections.Generic.KeyValuePair<Criterion, System.Collections.Generic.Stack<DataNode>[]> criterionEntry in dataLoader.LocalOnMemoryResolvedCriterions)
                //            {
                //                if (criterionEntry.Key.Identity == Identity)
                //                {
                //                    appliedLocaly = true;
                //                    break;
                //                }
                //            }
                //            if (!appliedLocaly)
                //            {
                //                _Applied.Value = false;
                //                break;
                //            }
                //        }
                //    }
                //    if (RightTermDataNode != null)
                //    {
                //        DataNode dataNode = RightTermDataNode;
                //        foreach (var searchCondition in dataNode.SearchConditions)
                //        {
                //            if (searchCondition.Criterions.Contains(this))
                //            {
                //                while (dataNode.ParentDataNode != null && dataNode.ParentDataNode.SearchConditions.Contains(searchCondition))
                //                    dataNode = dataNode.ParentDataNode;
                //            }
                //        }

                //        foreach (System.Collections.Generic.KeyValuePair<string, DataLoader> entry in dataNode.DataSource.DataLoaders)
                //        {
                //            if (!(entry.Value is StorageDataLoader))
                //            {
                //                _Applied.Value = false;
                //                break;
                //            }
                //            StorageDataLoader dataLoader = entry.Value as StorageDataLoader;
                //            bool appliedLocaly = false;
                //            foreach (System.Collections.Generic.KeyValuePair<Criterion, System.Collections.Generic.Stack<DataNode>[]> criterionEntry in dataLoader.LocalResolvedCriterions)
                //            {
                //                if (criterionEntry.Key.Identity == Identity)
                //                {
                //                    appliedLocaly = true;
                //                    break;
                //                }

                //            }
                //            if (!appliedLocaly)
                //            {
                //                _Applied.Value = false;
                //                break;
                //            }
                //        }
                //    }
                //}
                
            }
            internal set
            {

                if (_Applied.HasValue && _Applied.Value)
                    _Applied = value;
                else if (!_Applied.HasValue)
                    _Applied = value;


                //_Applied.Value = value;

            }
        }

        /// <MetaDataID>{f0b3208c-21bc-4a13-a18f-36f67c153405}</MetaDataID>
        public readonly bool IsNULL = false;
        /// <MetaDataID>{454f272d-3c2b-4a15-8afe-c1ff8f08cec2}</MetaDataID>
        public readonly bool IsNotNULL = false;

        /// <MetaDataID>{ca7f6f06-61af-4bdb-981f-09f0d7f37a49}</MetaDataID>
        public readonly Guid Identity;

        /// <MetaDataID>{69d8ba44-9db7-40cd-8ea8-b8d10225cec3}</MetaDataID>
        public DataNode CollectionContainsDataNode;
        /// <MetaDataID>{7d805788-62ae-4ad3-932a-f59aa9bc0dd7}</MetaDataID>
        public Criterion(ComparisonType comparisonOperator, DataNode collectionContainsDataNode, ComparisonTerm[] comparisonTerms, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.ObjectQuery oqlStatement, bool constrainCriterion, SearchFactor owner)
            : this(comparisonOperator, comparisonTerms, oqlStatement, constrainCriterion, owner)
        {
            CollectionContainsDataNode = collectionContainsDataNode;
            //FilteredDataNode.SearchCriteria.Add(this);

        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <MetaDataID>{cd2bb175-537d-49e2-87fa-31608fa4726e}</MetaDataID>
        public Criterion(ComparisonType comparisonOperator, ComparisonTerm[] comparisonTerms, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.ObjectQuery oqlStatement, bool constrainCriterion, SearchFactor owner)
        {
            ConstrainCriterion = constrainCriterion;
            Identity = Guid.NewGuid();
            ComparisonOperator = comparisonOperator;
            _ComparisonTerms = comparisonTerms.Clone() as ComparisonTerm[];
            //OQLStatement = oqlStatement;
            Owner = owner;
            if (comparisonOperator == ComparisonType.ContainsAny || comparisonOperator == ComparisonType.ContainsAll)
            {
                CriterionType = ComparisonTermsType.CollectionContainsAnyAll;
                ParameterComparisonTerm = (_ComparisonTerms[1] as ParameterComparisonTerm);//.ParameterValue;
                ParameterValueType = (_ComparisonTerms[1] as ParameterComparisonTerm).ValueType;
                LeftTermDataNode = (_ComparisonTerms[0] as ObjectComparisonTerm).DataNode;
                //LeftTermDataNode.SearchCriterions.Add(this);
                return;
            }
            if (_ComparisonTerms[0] is LiteralComparisonTerm)
            {
                if (_ComparisonTerms[1] is ObjectAttributeComparisonTerm)
                    CriterionType = ComparisonTermsType.LiteralWithObjectAttribute;
                else if (_ComparisonTerms[1] is ObjectComparisonTerm)
                    CriterionType = ComparisonTermsType.LiteralWithObject;
                else
                    throw new System.Exception("Invlalid criterion :" + ParserNode.Value.ToString());
            }
            else if (_ComparisonTerms[0] is ParameterComparisonTerm)
            {
                if (_ComparisonTerms[1] is ObjectAttributeComparisonTerm)
                    CriterionType = ComparisonTermsType.ParameterWithObjectAttribute;
                else if (_ComparisonTerms[1] is ObjectComparisonTerm)
                    CriterionType = ComparisonTermsType.ParameterWithObject;
                else
                    throw new System.Exception("Invlalid criterion :" + ParserNode.Value.ToString());
            }
            else if (_ComparisonTerms[0] is ObjectAttributeComparisonTerm)
            {
                if (_ComparisonTerms[1] is ObjectAttributeComparisonTerm)
                    CriterionType = ComparisonTermsType.ObjectsAttributes;
                else if (_ComparisonTerms[1] is LiteralComparisonTerm)
                    CriterionType = ComparisonTermsType.ObjectAttributeWithLiteral;
                else if (_ComparisonTerms[1] is ParameterComparisonTerm)
                    CriterionType = ComparisonTermsType.ObjectAttributeWithParameter;
                else
                    throw new System.Exception("Invlalid criterion :" + ParserNode.Value.ToString());
            }
            else if (_ComparisonTerms[0] is ObjectComparisonTerm)
            {
                if (_ComparisonTerms[1] is ObjectComparisonTerm)
                    CriterionType = ComparisonTermsType.Objects;
                else if (_ComparisonTerms[1] is LiteralComparisonTerm)
                    CriterionType = ComparisonTermsType.LiteralWithObject;
                else if (_ComparisonTerms[1] is ParameterComparisonTerm)
                    CriterionType = ComparisonTermsType.ObjectWithParameter;
                else
                    CriterionType = ComparisonTermsType.ObjectWithLiteral;
            }
            if (_ComparisonTerms[0] is ObjectIDComparisonTerm)
            {
                if (_ComparisonTerms[1] is ObjectComparisonTerm)
                    CriterionType = ComparisonTermsType.LiteralWithObject;
                else
                    throw new System.Exception("Syntax error : " + ParserNode.Value);
            }


            if (_ComparisonTerms[0] is LiteralComparisonTerm)
            {
                LiteralValue = (_ComparisonTerms[0] as LiteralComparisonTerm).Value;
                LiteralValueType = (_ComparisonTerms[0] as LiteralComparisonTerm).ValueType;
            }
            if (_ComparisonTerms[1] is LiteralComparisonTerm)
            {
                LiteralValue = (_ComparisonTerms[1] as LiteralComparisonTerm).Value;
                LiteralValueType = (_ComparisonTerms[1] as LiteralComparisonTerm).ValueType;
            }
            if (_ComparisonTerms[0] is ParameterComparisonTerm)
            {
                ParameterComparisonTerm = (_ComparisonTerms[0] as ParameterComparisonTerm);//.ParameterValue;
                ParameterValueType = (_ComparisonTerms[0] as ParameterComparisonTerm).ValueType;
            }
            //TODO θσ πρέπει να υλοποιηθεί ο παρακάτω κώδικας
            //if (_ComparisonTerms[0] is ObjectIDComparisonTerm)
            //{
            //    LiteralValue = (_ComparisonTerms[0] as ObjectIDComparisonTerm).Value;
            //    LiteralValueType = (_ComparisonTerms[0] as ObjectIDComparisonTerm).ValueType;
            //}
            //if (_ComparisonTerms[1] is ObjectIDComparisonTerm)
            //{
            //    LiteralValue = (_ComparisonTerms[1] as ObjectIDComparisonTerm).Value;
            //    LiteralValueType = (_ComparisonTerms[1] as ObjectIDComparisonTerm).ValueType;
            //}
            if (_ComparisonTerms[1] is ParameterComparisonTerm)
            {
                ParameterComparisonTerm = (_ComparisonTerms[1] as ParameterComparisonTerm);//.ParameterValue;
                ParameterValueType = (_ComparisonTerms[1] as ParameterComparisonTerm).ValueType;
            }
            if (_ComparisonTerms[0] is ObjectComparisonTerm)
                LeftTermDataNode = (_ComparisonTerms[0] as ObjectComparisonTerm).DataNode;

            if (_ComparisonTerms[0] is ObjectAttributeComparisonTerm)
                LeftTermDataNode = (_ComparisonTerms[0] as ObjectAttributeComparisonTerm).DataNode;
            if (_ComparisonTerms[1] is ObjectComparisonTerm)
                RightTermDataNode = (_ComparisonTerms[1] as ObjectComparisonTerm).DataNode;

            if (_ComparisonTerms[1] is ObjectAttributeComparisonTerm)
                RightTermDataNode = (_ComparisonTerms[1] as ObjectAttributeComparisonTerm).DataNode;





            if (LeftTermDataNode != null &&
                //FirstDataNode.Type == DataNode.DataNodeType.Object &&
                _ComparisonTerms[1] is LiteralComparisonTerm &&
                (_ComparisonTerms[1] as LiteralComparisonTerm).Value == null)
            {
                if (ComparisonOperator == ComparisonType.Equal)
                    IsNULL = true;
                else
                    IsNotNULL = true;


            }

            //if (FirstDataNode != null && ConstrainCriterion)
            //    FirstDataNode.ActOnConstrainCriterion = ConstrainCriterion;


            if (RightTermDataNode != null &&
                // SecondDataNode.Type == DataNode.DataNodeType.Object &&
            _ComparisonTerms[0] is LiteralComparisonTerm &&
            (_ComparisonTerms[0] as LiteralComparisonTerm).Value == null)
            {
                if (ComparisonOperator == ComparisonType.Equal)
                    IsNULL = true;
                else
                    IsNotNULL = true;
            }
            //if (SecondDataNode != null && ConstrainCriterion)
            //    SecondDataNode.ActOnConstrainCriterion = ConstrainCriterion;



            //if (RightTermDataNode != null)
            //    RightTermDataNode.SearchCriterions.Add(this);
            //if (LeftTermDataNode != null)
            //    LeftTermDataNode.SearchCriterions.Add(this);



            if (CriterionType == ComparisonTermsType.ObjectWithLiteral ||
                CriterionType == ComparisonTermsType.LiteralWithObject ||
                CriterionType == ComparisonTermsType.ParameterWithObject ||
                CriterionType == ComparisonTermsType.ObjectWithParameter)
            {

                if (ComparisonOperator == ComparisonType.Like)
                {
                    throw new System.Exception("Synax error :" + ParserNode.Value);
                }
                if (ComparisonOperator == ComparisonType.LessThan ||
                    ComparisonOperator == ComparisonType.GreaterThan ||
                    ComparisonOperator == ComparisonType.LessThanEqual ||
                    ComparisonOperator == ComparisonType.GreaterThanEqual)
                {
                    Type leftType = null;
                    Type rightType = null;

                    if ((ComparisonTerms[0] is ObjectComparisonTerm && ComparisonTerms[0].ValueType.GetMetaData().IsValueType) ||
                        (ComparisonTerms[1] is ObjectComparisonTerm && ComparisonTerms[1].ValueType.GetMetaData().IsValueType))
                    {
                        if (ComparisonTerms[0] is ObjectComparisonTerm)
                        {
                            leftType = (ComparisonTerms[0] as ObjectComparisonTerm).ValueType;
                            if (ParameterValueType != null)
                                rightType = ParameterValueType;
                            else
                                rightType = LiteralValueType;
                        }
                        if (ComparisonTerms[0] is ParameterComparisonTerm)
                        {
                            leftType = (ComparisonTerms[0] as ParameterComparisonTerm).ValueType;
                            rightType = (ComparisonTerms[0] as ObjectComparisonTerm).ValueType;

                        }
                        if (leftType == null)
                            throw new System.Exception("Synax error :" + ParserNode.Value);

                        if (ComparisonOperator == ComparisonType.LessThan)
                            OverridenComparisonOperator = leftType.GetMetaData().GetMethod("op_LessThan", new Type[2] { leftType, rightType });
                        if (ComparisonOperator == ComparisonType.GreaterThan)
                            OverridenComparisonOperator = leftType.GetMetaData().GetMethod("op_GreaterThan", new Type[2] { leftType, rightType });

                        if (ComparisonOperator == ComparisonType.LessThanEqual)
                            OverridenComparisonOperator = leftType.GetMetaData().GetMethod("op_LessThanOrEqual", new Type[2] { leftType, rightType });
                        if (ComparisonOperator == ComparisonType.GreaterThanEqual)
                            OverridenComparisonOperator = leftType.GetMetaData().GetMethod("op_GreaterThanOrEqual", new Type[2] { leftType, rightType });

                    }
                    else
                        throw new System.Exception("Synax error :" + ParserNode.Value);
                }

                if (ComparisonOperator == ComparisonType.Equal ||
                    ComparisonOperator == ComparisonType.NotEqual)
                {
                    Type leftType = null;
                    Type rightType = null;

                    if ((ComparisonTerms[0] is ObjectComparisonTerm && ComparisonTerms[0].ValueType.GetMetaData().IsValueType) ||
                        (ComparisonTerms[1] is ObjectComparisonTerm && ComparisonTerms[1].ValueType.GetMetaData().IsValueType))
                    {
                        if (ComparisonTerms[0] is ObjectComparisonTerm)
                        {
                            leftType = (ComparisonTerms[0] as ObjectComparisonTerm).ValueType;
                            if (ParameterValueType != null)
                                rightType = ParameterValueType;
                            else
                                rightType = LiteralValueType;
                        }
                        if (ComparisonTerms[0] is ParameterComparisonTerm)
                        {
                            leftType = (ComparisonTerms[0] as ParameterComparisonTerm).ValueType;
                            rightType = (ComparisonTerms[0] as ObjectComparisonTerm).ValueType;

                        }
                        if (leftType == null)
                            throw new System.Exception("Synax error :" + ParserNode.Value); if (ComparisonOperator == ComparisonType.Equal)
                            OverridenComparisonOperator = leftType.GetMetaData().GetMethod("op_Equality", new Type[2] { leftType, rightType });
                        if (ComparisonOperator == ComparisonType.NotEqual)
                            OverridenComparisonOperator = leftType.GetMetaData().GetMethod("op_Inequality", new Type[2] { leftType, rightType });
                    }
                }
            }
        }


        public Criterion(ComparisonType comparisonOperator, ComparisonTerm[] comparisonTerms, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.ObjectQuery oqlStatement, bool constrainCriterion, SearchFactor owner, Guid identity):
            this(comparisonOperator, comparisonTerms, oqlStatement, constrainCriterion, owner)
        {
            Identity = identity;
        }
        /// <MetaDataID>{5474f4ac-4b98-4cec-85f3-66a5d70a53a6}</MetaDataID>
        public System.Reflection.MethodInfo OverridenComparisonOperator;
        /// <MetaDataID>{CA8FB35E-6D05-40A6-852A-7E0F4D5C6B7A}</MetaDataID>
        protected internal Criterion(Parser.ParserNode criterionParserNode, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.ObjectsContextQuery oqlStatement, bool constrainCriterion, SearchFactor owner)
        {
            Identity = Guid.NewGuid();

            Owner = owner;
            ParserNode = criterionParserNode;
            //OQLStatement = oqlStatement;
            _ComparisonTerms = new ComparisonTerm[2];
            _ComparisonTerms[0] = ComparisonTerm.GetComparisonTermFor(criterionParserNode["Comparison_Item"][0] as Parser.ParserNode, oqlStatement);
            _ComparisonTerms[1] = ComparisonTerm.GetComparisonTermFor(criterionParserNode["Comparison_Item"][1] as Parser.ParserNode, oqlStatement);
            ConstrainCriterion = constrainCriterion;
            if (ParserNode.ChildNodes.GetAt(2).Name == "Comparison_Operator")
            {
                switch (ParserNode.ChildNodes.GetAt(2).Value)
                {
                    case "=":
                        {
                            ComparisonOperator = ComparisonType.Equal;
                            break;
                        }
                    case "<>":
                        {
                            ComparisonOperator = ComparisonType.NotEqual;
                            break;
                        }
                    case ">":
                        {
                            ComparisonOperator = ComparisonType.GreaterThan;
                            break;
                        }
                    case ">=":
                        {
                            ComparisonOperator = ComparisonType.GreaterThanEqual;
                            break;
                        }
                    case "<":
                        {
                            ComparisonOperator = ComparisonType.LessThan;
                            break;
                        }
                    case "<=":
                        {
                            ComparisonOperator = ComparisonType.LessThanEqual;
                            break;
                        }

                    default:
                        throw new System.Exception("Unknown comparison operator : " + ParserNode.ChildNodes.GetAt(2).Value);
                }
            }
            if (ParserNode.ChildNodes.GetAt(2).Name == "Like_Operator")
            {
                ComparisonOperator = ComparisonType.Like;
                if (!(_ComparisonTerms[0] is ObjectAttributeComparisonTerm) && !(_ComparisonTerms[1] is LiteralComparisonTerm))
                    throw new System.Exception("Invalid Like expresion :" + ParserNode.Value);
            }


            if (_ComparisonTerms[0] is LiteralComparisonTerm)
            {
                if (_ComparisonTerms[1] is ObjectAttributeComparisonTerm)
                    CriterionType = ComparisonTermsType.LiteralWithObjectAttribute;
                else if (_ComparisonTerms[1] is ObjectComparisonTerm)
                    CriterionType = ComparisonTermsType.LiteralWithObject;
                else
                    throw new System.Exception("Invlalid criterion :" + ParserNode.Value.ToString());
            }
            else if (_ComparisonTerms[0] is ParameterComparisonTerm)
            {
                if (_ComparisonTerms[1] is ObjectAttributeComparisonTerm)
                    CriterionType = ComparisonTermsType.ParameterWithObjectAttribute;
                else if (_ComparisonTerms[1] is ObjectComparisonTerm)
                    CriterionType = ComparisonTermsType.ParameterWithObject;
                else
                    throw new System.Exception("Invlalid criterion :" + ParserNode.Value.ToString());
            }
            else if (_ComparisonTerms[0] is ObjectAttributeComparisonTerm)
            {
                if (_ComparisonTerms[1] is ObjectAttributeComparisonTerm)
                    CriterionType = ComparisonTermsType.ObjectsAttributes;
                else if (_ComparisonTerms[1] is LiteralComparisonTerm)
                    CriterionType = ComparisonTermsType.ObjectAttributeWithLiteral;
                else if (_ComparisonTerms[1] is ParameterComparisonTerm)
                    CriterionType = ComparisonTermsType.ObjectAttributeWithParameter;
                else
                    throw new System.Exception("Invlalid criterion :" + ParserNode.Value.ToString());
            }
            else if (_ComparisonTerms[0] is ObjectComparisonTerm)
            {
                if (_ComparisonTerms[1] is ObjectComparisonTerm)
                    CriterionType = ComparisonTermsType.Objects;
                else if (_ComparisonTerms[1] is LiteralComparisonTerm)
                    CriterionType = ComparisonTermsType.LiteralWithObject;
                else if (_ComparisonTerms[1] is ParameterComparisonTerm)
                    CriterionType = ComparisonTermsType.ObjectWithParameter;
                else
                    CriterionType = ComparisonTermsType.ObjectWithLiteral;
            }
            if (_ComparisonTerms[0] is ObjectIDComparisonTerm)
            {
                if (_ComparisonTerms[1] is ObjectComparisonTerm)
                    CriterionType = ComparisonTermsType.LiteralWithObject;
                else
                    throw new System.Exception("Syntax error : " + ParserNode.Value);
            }


            if (_ComparisonTerms[0] is LiteralComparisonTerm)
            {
                LiteralValue = (_ComparisonTerms[0] as LiteralComparisonTerm).Value;
                LiteralValueType = (_ComparisonTerms[0] as LiteralComparisonTerm).ValueType;
            }
            if (_ComparisonTerms[1] is LiteralComparisonTerm)
            {
                LiteralValue = (_ComparisonTerms[1] as LiteralComparisonTerm).Value;
                LiteralValueType = (_ComparisonTerms[1] as LiteralComparisonTerm).ValueType;
            }
            if (_ComparisonTerms[0] is ParameterComparisonTerm)
            {
                ParameterComparisonTerm = (_ComparisonTerms[0] as ParameterComparisonTerm);//.ParameterValue;
                ParameterValueType = (_ComparisonTerms[0] as ParameterComparisonTerm).ValueType;
            }
            //TODO θσ πρέπει να υλοποιηθεί ο παρακάτω κώδικας
            //if (_ComparisonTerms[0] is ObjectIDComparisonTerm)
            //{
            //    LiteralValue = (_ComparisonTerms[0] as ObjectIDComparisonTerm).Value;
            //    LiteralValueType = (_ComparisonTerms[0] as ObjectIDComparisonTerm).ValueType;
            //}
            //if (_ComparisonTerms[1] is ObjectIDComparisonTerm)
            //{
            //    LiteralValue = (_ComparisonTerms[1] as ObjectIDComparisonTerm).Value;
            //    LiteralValueType = (_ComparisonTerms[1] as ObjectIDComparisonTerm).ValueType;
            //}
            if (_ComparisonTerms[1] is ParameterComparisonTerm)
            {
                ParameterComparisonTerm = (_ComparisonTerms[1] as ParameterComparisonTerm);//.ParameterValue;
                ParameterValueType = (_ComparisonTerms[1] as ParameterComparisonTerm).ValueType;
            }
            if (_ComparisonTerms[0] is ObjectComparisonTerm)
                LeftTermDataNode = (_ComparisonTerms[0] as ObjectComparisonTerm).DataNode;

            if (_ComparisonTerms[0] is ObjectAttributeComparisonTerm)
                LeftTermDataNode = (_ComparisonTerms[0] as ObjectAttributeComparisonTerm).DataNode;
            if (_ComparisonTerms[1] is ObjectComparisonTerm)
                RightTermDataNode = (_ComparisonTerms[1] as ObjectComparisonTerm).DataNode;

            if (_ComparisonTerms[1] is ObjectAttributeComparisonTerm)
                RightTermDataNode = (_ComparisonTerms[1] as ObjectAttributeComparisonTerm).DataNode;





            if (LeftTermDataNode != null &&
                //FirstDataNode.Type == DataNode.DataNodeType.Object &&
                (_ComparisonTerms[1] is LiteralComparisonTerm &&
                (_ComparisonTerms[1] as LiteralComparisonTerm).Value == null) ||
                                (_ComparisonTerms[1] is ParameterComparisonTerm &&
                (_ComparisonTerms[1] as ParameterComparisonTerm).ParameterValue == null))
            {
                if (ComparisonOperator == ComparisonType.Equal)
                    IsNULL = true;
                else
                    IsNotNULL = true;


            }

            //if (FirstDataNode != null && ConstrainCriterion)
            //    FirstDataNode.ActOnConstrainCriterion = ConstrainCriterion;


            if (RightTermDataNode != null &&
                // SecondDataNode.Type == DataNode.DataNodeType.Object &&
            _ComparisonTerms[0] is LiteralComparisonTerm &&
            (_ComparisonTerms[0] as LiteralComparisonTerm).Value == null)
            {
                if (ComparisonOperator == ComparisonType.Equal)
                    IsNULL = true;
                else
                    IsNotNULL = true;
            }
            //if (SecondDataNode != null && ConstrainCriterion)
            //    SecondDataNode.ActOnConstrainCriterion = ConstrainCriterion;



            //if (RightTermDataNode != null)
            //    RightTermDataNode.SearchCriterions.Add(this);
            //if (LeftTermDataNode != null)
            //    LeftTermDataNode.SearchCriterions.Add(this);



            if (CriterionType == ComparisonTermsType.ObjectWithLiteral ||
                CriterionType == ComparisonTermsType.LiteralWithObject ||
                CriterionType == ComparisonTermsType.ParameterWithObject ||
                CriterionType == ComparisonTermsType.ObjectWithParameter)
            {

                if (ComparisonOperator == ComparisonType.Like ||
                    ComparisonOperator == ComparisonType.LessThan ||
                    ComparisonOperator == ComparisonType.GreaterThan)
                {
                    throw new System.Exception("Synax error :" + ParserNode.Value);
                }
            }












        }

        ///// <MetaDataID>{3B80B1DE-2DAC-4F78-98EA-7662E3627AFA}</MetaDataID>
        //public Criterion(Criterion criterion)
        //{
        //    _ComparisonTerms = new ComparisonTerm[2];
        //    _ComparisonTerms[0] = criterion.ComparisonTerms[0];
        //    _ComparisonTerms[1] = criterion.ComparisonTerms[1];
        //    OQLStatement = criterion.OQLStatement;
        //    ComparisonOperator = criterion.ComparisonOperator;
        //    CriterionType = criterion.CriterionType;
        //    ParameterValue = criterion.ParameterValue;
        //    ParameterValueType = criterion.ParameterValueType;
        //    LiteralValue = criterion.LiteralValue;
        //    LiteralValueType = criterion.LiteralValueType;
        //    FirstDataNode = criterion.FirstDataNode;
        //    SecondDataNode = criterion.SecondDataNode;

        //}
        /// <MetaDataID>{C37F4089-B9D3-49AC-BADF-66CB41E3AFC7}</MetaDataID>
        public SearchCondition SearhConditionHeader
        {
            get
            {
                return Owner.OwnerSearchTerm.OwnerSearchCondition.SearhConditionHeader;
            }
        }

        /// <MetaDataID>{7A8B9B53-0BDD-4648-9893-F9CDF33182B2}</MetaDataID>
        /// <summary>In distributed query the information’s collected from many storages in different machines. 
        /// The information’s concentrate in one machine and criteria applied to filter the information’s. 
        /// For performance reasons it is better when it is possible to applied the criteria as filter, 
        /// locally, in machines where the informations lives. </summary>
        public bool CanAppliedLocaly
        {

            get
            {
                if (!ConstrainCriterion)
                    return false;
                if (ComparisonTerms[0] is ObjectComparisonTerm ||
                    ComparisonTerms[0] is ObjectAttributeComparisonTerm)
                {
                    if (ComparisonTerms[1] is ParameterComparisonTerm
                        || ComparisonTerms[1] is LiteralComparisonTerm
                        || ComparisonTerms[1] is ObjectIDComparisonTerm)
                    {
                        return true;
                    }
                }
                if (ComparisonTerms[1] is ObjectComparisonTerm
                    || ComparisonTerms[1] is ObjectAttributeComparisonTerm)
                {
                    if (ComparisonTerms[0] is ParameterComparisonTerm
                        || ComparisonTerms[0] is LiteralComparisonTerm
                        || ComparisonTerms[0] is ObjectIDComparisonTerm)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        /// <MetaDataID>{C9AC485A-ACEF-4424-97F3-309F6838DDBA}</MetaDataID>
        public ComparisonType ComparisonOperator;
        /// <MetaDataID>{4C5EBC0A-82D0-4ECA-ABB8-0D7F46658B72}</MetaDataID>
        public readonly bool ConstrainCriterion;
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{9099B36B-2136-404B-91D9-D8420740CF55}</MetaDataID>
        protected ComparisonTerm[] _ComparisonTerms;
        /// <MetaDataID>{267ABDAB-867A-4055-9D6B-A8FFE1CB8746}</MetaDataID>
        public ComparisonTerm[] ComparisonTerms
        {

            get
            {
                return _ComparisonTerms.Clone() as ComparisonTerm[];
            }
        }
        /// <MetaDataID>{3A505DE9-816F-4D48-A58B-9998BE4496BD}</MetaDataID>

        protected ObjectQuery OQLStatement
        {
            get
            {
                if (LeftTermDataNode != null)
                    return LeftTermDataNode.ObjectQuery;
                if (RightTermDataNode != null)
                    return RightTermDataNode.ObjectQuery;
                return null;
            }
        }
        /// <MetaDataID>{E0E73F20-09D8-459D-8AC6-5BE4A6F67CF4}</MetaDataID>
        [NonSerialized]
        public Parser.ParserNode ParserNode;



        /// <MetaDataID>{4D05F3E0-8F25-4D76-9183-4E7E78AF18D9}</MetaDataID>
        public bool IsLocalFilterFor(DataNode dataNode)
        {

            if (ComparisonTerms[0] is ObjectComparisonTerm ||
                ComparisonTerms[0] is ObjectAttributeComparisonTerm)
            {
                if (ComparisonTerms[1] is ParameterComparisonTerm
                    || ComparisonTerms[1] is LiteralComparisonTerm
                    || ComparisonTerms[1] is ObjectIDComparisonTerm)
                {
                    if (ComparisonTerms[0] is ObjectComparisonTerm && dataNode == (ComparisonTerms[0] as ObjectComparisonTerm).DataNode)
                        return true;
                    if (ComparisonTerms[0] is ObjectAttributeComparisonTerm && dataNode == (ComparisonTerms[0] as ObjectAttributeComparisonTerm).DataNode.ParentDataNode)
                        return true;
                }
            }

            if (ComparisonTerms[1] is ObjectComparisonTerm
                || ComparisonTerms[1] is ObjectAttributeComparisonTerm)
            {
                if (ComparisonTerms[0] is ParameterComparisonTerm
                    || ComparisonTerms[0] is LiteralComparisonTerm
                    || ComparisonTerms[0] is ObjectIDComparisonTerm)
                {
                    if (ComparisonTerms[1] is ObjectComparisonTerm && dataNode == (ComparisonTerms[1] as ObjectComparisonTerm).DataNode)
                        return true;
                    if (ComparisonTerms[1] is ObjectAttributeComparisonTerm && dataNode == (ComparisonTerms[1] as ObjectAttributeComparisonTerm).DataNode.ParentDataNode)
                        return true;
                }
            }

            DataNode firstDataNode = null;
            DataNode secondDataNode = null;
            if (ComparisonTerms[0] is ObjectComparisonTerm)
                firstDataNode = (ComparisonTerms[0] as ObjectComparisonTerm).DataNode;
            if (ComparisonTerms[0] is ObjectAttributeComparisonTerm)
                firstDataNode = (ComparisonTerms[0] as ObjectAttributeComparisonTerm).DataNode;
            if (ComparisonTerms[1] is ObjectComparisonTerm)
                secondDataNode = (ComparisonTerms[1] as ObjectComparisonTerm).DataNode;
            if (ComparisonTerms[1] is ObjectAttributeComparisonTerm)
                secondDataNode = (ComparisonTerms[1] as ObjectAttributeComparisonTerm).DataNode;
            if (firstDataNode != null &&
                secondDataNode != null &&
                firstDataNode == secondDataNode)
                return true;
            //TODO Να γραφτεί ένα test case θα συγκρινεί ένα OjectAttribute μιας DataNode με ένα άλλο OjectAttribute 
            //της ίδιας DataNode για όλους τους provider.
            if (firstDataNode != null &&
                secondDataNode != null &&
                firstDataNode.Type == DataNode.DataNodeType.OjectAttribute &&
                secondDataNode.Type == DataNode.DataNodeType.OjectAttribute &&
                firstDataNode.ParentDataNode == secondDataNode.ParentDataNode)
                return true;





            return false;

        }



        /// <MetaDataID>{2804085a-91d4-4b9e-a1ca-88b515a03ca7}</MetaDataID>
        public static bool Like(string values, string patern)
        {
            if (patern == null || values == null)
                return false;

            patern = patern.Replace(@"\*", "char(" + ((int)'*').ToString() + ")");
            patern = patern.Replace("*", @"(\w){0,}");
            patern = patern.Replace("char(" + ((int)'*').ToString() + ")", "*");
            patern = patern.Replace("?", @"(\w){1}");
            patern = "^" + patern + @"\z";
            patern = patern.Replace(' ', (char)254);
            values = values.Replace(' ', (char)254);
            return System.Text.RegularExpressions.Regex.Match(values, patern, System.Text.RegularExpressions.RegexOptions.IgnoreCase).Success;
        }
        /// <MetaDataID>{29c182f0-d7aa-4e40-b59b-9281adb7cc9e}</MetaDataID>
        public static bool SQL92Like(string values, string patern)
        {
            if (patern == null || values == null)
                return false;
            patern = patern.Replace(@"\%", "char(" + ((int)'%').ToString() + ")");
            patern = patern.Replace("%", @"(\w){0,}");
            patern = patern.Replace("char(" + ((int)'%').ToString() + ")", "%");
            patern = patern.Replace("?", @"(\w){1}");
            patern = "^" + patern + @"\z";
            patern = patern.Replace(' ', (char)254);
            values = values.Replace(' ', (char)254);
            return System.Text.RegularExpressions.Regex.Match(values, patern, System.Text.RegularExpressions.RegexOptions.IgnoreCase).Success;
        }

        /// <MetaDataID>{0e288fea-d4d5-4ad5-8c18-9c1d6d456c26}</MetaDataID>
        bool ExecuteCondition(object leftValue, object rightValue)
        {
            if (rightValue is DateTime)
                rightValue=((DateTime)rightValue).ToUniversalTime();

            if (leftValue is DateTime)
                leftValue=((DateTime)leftValue).ToUniversalTime();


            if (ComparisonOperator == ComparisonType.Like)
                return Like(leftValue as string, rightValue as string);



            if (leftValue is System.DBNull)
                leftValue = null;
            if (rightValue is System.DBNull)
                rightValue = null;


            if (rightValue == null && leftValue == null && this.ComparisonOperator == ComparisonType.Equal)
                return true;

            if (rightValue == null && leftValue == null && this.ComparisonOperator == ComparisonType.NotEqual)
                return false;


            if (((rightValue == null && leftValue != null) || (rightValue != null && leftValue == null)) && this.ComparisonOperator == ComparisonType.Equal)
                return false;
            if (((rightValue == null && leftValue != null) || (rightValue != null && leftValue == null)) && this.ComparisonOperator == ComparisonType.NotEqual)
                return true;

            if (rightValue is DateTime && leftValue is DateTime && ComparisonOperator == ComparisonType.Equal)
            {

                if ((((DateTime)rightValue).Date == ((DateTime)leftValue).Date) &&
                    (((DateTime)rightValue).Hour == ((DateTime)leftValue).Hour) &&
                    (((DateTime)rightValue).Minute == ((DateTime)leftValue).Minute) &&
                    (((DateTime)rightValue).Second == ((DateTime)leftValue).Second) &&
                    (((DateTime)rightValue).Millisecond == ((DateTime)leftValue).Millisecond))
                {
                    //System.Windows.Forms.MessageBox.Show("Equal "+ ((DateTime)leftValue).ToString() + "." + ((DateTime)leftValue).Millisecond.ToString() + "  " + ((DateTime)rightValue).ToString() + "." + ((DateTime)rightValue).Millisecond.ToString());
                    return true;
                }
                else
                {
                    // System.Windows.Forms.MessageBox.Show(((DateTime)leftValue).ToString() + "." + ((DateTime)leftValue).Millisecond.ToString() + "  " + ((DateTime)rightValue).ToString() + "." + ((DateTime)rightValue).Millisecond.ToString());
                    return false;
                }

            }
            if (rightValue is IComparable && leftValue is IComparable)
            {

                int comparisonResualt = (leftValue as IComparable).CompareTo(Convert.ChangeType(rightValue, leftValue.GetType()));
                if (comparisonResualt == 0 && ComparisonOperator == ComparisonType.Equal)
                    return true;
                if (comparisonResualt != 0 && ComparisonOperator == ComparisonType.NotEqual)
                    return true;

                if (comparisonResualt == 1 && ComparisonOperator == ComparisonType.GreaterThan)
                    return true;
                if (comparisonResualt == -1 && ComparisonOperator == ComparisonType.LessThan)
                    return true;

                if ((comparisonResualt == -1 || comparisonResualt == 0) && ComparisonOperator == ComparisonType.LessThanEqual)
                    return true;
                if ((comparisonResualt == 1 || comparisonResualt == 0) && ComparisonOperator == ComparisonType.GreaterThanEqual)
                    return true;



                return false;
            }
            else
            {
                if (ComparisonOperator == ComparisonType.Equal)
                    return leftValue.Equals(rightValue);
                if (ComparisonOperator == ComparisonType.NotEqual)
                    return !leftValue.Equals(rightValue);
                if (rightValue == null || leftValue == null)
                    return false;

            }

            throw new Exception("The method or operation is not implemented.");

        }


        /// <MetaDataID>{f5b4cf9f-f04f-4a04-ac52-83864735115a}</MetaDataID>
        //internal bool DoesRowPassCondition(System.Data.DataRow row, DataNode ownerDataNode)
        //{
        //    DataNode rowDataNode = ownerDataNode;

        //    object leftValue = null;
        //    object rightValue = null;

        //    //DataNode criterionDataNode=
        //    if (ComparisonTerms[0] is ObjectAttributeComparisonTerm &&
        //        (ComparisonTerms[0] as ObjectAttributeComparisonTerm).DataNode.ParentDataNode == rowDataNode)
        //    {
        //        leftValue = row[(ComparisonTerms[0] as ObjectAttributeComparisonTerm).DataNode.Name];
        //        if (ComparisonTerms[1] is LiteralComparisonTerm)
        //            rightValue = (ComparisonTerms[1] as LiteralComparisonTerm).Value;
        //        if (ComparisonTerms[1] is ParameterComparisonTerm)
        //            rightValue = (ComparisonTerms[1] as ParameterComparisonTerm).ParameterValue;
        //    }
        //    else if(ComparisonTerms[1] is ObjectAttributeComparisonTerm) 
        //        throw new System.Exception("Criterion can't be applied on row");


        //    if (ComparisonTerms[0] is ObjectComparisonTerm &&
        //    (ComparisonTerms[0] as ObjectComparisonTerm).DataNode == rowDataNode)
        //    {
        //        leftValue = row["ObjectID"];
        //        if (ComparisonTerms[1] is LiteralComparisonTerm)
        //            rightValue = (ComparisonTerms[1] as LiteralComparisonTerm).Value;
        //        if (ComparisonTerms[1] is ParameterComparisonTerm)
        //            rightValue = (ComparisonTerms[1] as ParameterComparisonTerm).ParameterValue;
        //        if (leftValue == null && rightValue != null)
        //            return false;
        //        PersistenceLayer.StorageInstanceRef storageInstanceRef = PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(rightValue);
        //        if (storageInstanceRef == null && leftValue != null)
        //            return false;
        //        if (storageInstanceRef != null)
        //        {
        //            #if DeviceDotNet
        //                rightValue = System.Convert.ChangeType(storageInstanceRef.ObjectID, leftValue.GetType(), System.Globalization.CultureInfo.CurrentCulture.NumberFormat);
        //            #else
        //                rightValue = System.Convert.ChangeType(storageInstanceRef.ObjectID, leftValue.GetType());
        //            #endif

        //        }

        //        else
        //            rightValue = null;

        //        //throw new Exception("The functionallity is not implemented.");
        //    }
        //    else if (ComparisonTerms[0] is ObjectComparisonTerm)
        //        throw new System.Exception("Criterion can't be applied on row");


        //    if (ComparisonTerms[1] is ObjectAttributeComparisonTerm &&
        //        (ComparisonTerms[1] as ObjectAttributeComparisonTerm).DataNode.ParentDataNode == rowDataNode)
        //    {
        //        leftValue = row["ObjectID"];
        //        if (ComparisonTerms[0] is LiteralComparisonTerm)
        //            leftValue = (ComparisonTerms[0] as LiteralComparisonTerm).Value;
        //        if (ComparisonTerms[0] is ParameterComparisonTerm)
        //            leftValue = (ComparisonTerms[0] as ParameterComparisonTerm).ParameterValue;
        //    }
        //    else if (ComparisonTerms[1] is ObjectAttributeComparisonTerm)
        //        throw new System.Exception("Criterion can't be applied on row");


        //    if (ComparisonTerms[1] is ObjectComparisonTerm &&
        //    (ComparisonTerms[1] as ObjectComparisonTerm).DataNode == rowDataNode)
        //    {
        //        if (ComparisonTerms[0] is LiteralComparisonTerm)
        //            leftValue = (ComparisonTerms[0] as LiteralComparisonTerm).Value;
        //        if (ComparisonTerms[0] is ParameterComparisonTerm)
        //            leftValue = (ComparisonTerms[0] as ParameterComparisonTerm).ParameterValue;
        //        if (leftValue == null && rightValue != null)
        //            return false;
        //        PersistenceLayer.StorageInstanceRef storageInstanceRef = PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(leftValue);
        //        if (storageInstanceRef == null && rightValue != null)
        //            return false;
        //        if (storageInstanceRef != null)
        //        {
        //            #if DeviceDotNet
        //                rightValue = System.Convert.ChangeType(storageInstanceRef.ObjectID, leftValue.GetType(), System.Globalization.CultureInfo.CurrentCulture.NumberFormat);
        //            #else
        //                leftValue = System.Convert.ChangeType(storageInstanceRef.ObjectID, rightValue.GetType());
        //            #endif
        //        }
        //        else
        //            leftValue = null;
        //        //throw new Exception("The functionallity is not implemented.");
        //    }
        //    else if(ComparisonTerms[1] is ObjectComparisonTerm)
        //        throw new System.Exception("Criterion can't be applied on row");

        //    return ExecuteCondition(leftValue, rightValue);

        //}

        GlobalObjectID? GlobalObjectIDTermValue;
        //GlobalObjectID GlobalObjectIDParameterValue
        //{
        //    get
        //    {
        //        return _GlobalObjectIDParameterValue;
        //    }
        //    set
        //    {
        //        _GlobalObjectIDParameterValue = value;
        //    }
        //}
        /// <MetaDataID>{4223b416-3a1b-4f4a-9a27-48d1f65f5485}</MetaDataID>
        DataNode[] AttributeValuePath;
        ///<summary>
        ///This method checks if the virtual data row qualifies the criterion.  
        ///</summary>
        /// <param name="composedRow">
        /// Defines the virtual data row.
        /// </param>
        /// <MetaDataID>{279fcd21-a290-4eb4-b0b3-3814e8fd682f}</MetaDataID>
        internal bool DoesRowPassCondition(IDataRow[] composedRow, System.Collections.Generic.Dictionary<DataNode, int> dataNodeRowIndices)
        {
            if (IsNULL && composedRow.Length == 0)
                return true;
            if (composedRow.Length == 0)
                return false;

            if (Applied)
                return true;



            switch (CriterionType)
            {
                case ComparisonTermsType.ObjectsAttributes:
                    {

                        object leftValue = null;
                        object rightValue = null;

                        IDataRow row = GetRow(composedRow, dataNodeRowIndices, LeftTermDataNode);
                            //composedRow[DataRetrieveNode.GetDataSourceRowIndex(dataNodeRowIndices, LeftTermDataNode)];//.ParentDataNode.FilteredDataRowIndex];
                        if (row != null)
                            leftValue = row[LeftTermDataNode.Name];
                        if (leftValue is DBNull)
                            leftValue = null;
                        row = GetRow(composedRow, dataNodeRowIndices, RightTermDataNode);
                        //composedRow[DataRetrieveNode.GetDataSourceRowIndex(dataNodeRowIndices, RightTermDataNode)];
                        if (row != null)
                            rightValue = row[RightTermDataNode.Name];
                        if (rightValue is DBNull)
                            rightValue = null;
                        return ExecuteCondition(leftValue, rightValue);
                    }
                case ComparisonTermsType.ObjectAttributeWithLiteral:
                    {
                        object leftValue = null;
                        object rightValue = null;
                        IDataRow row = GetRow(composedRow, dataNodeRowIndices, LeftTermDataNode); ;
                        //composedRow[DataRetrieveNode.GetDataSourceRowIndex(dataNodeRowIndices, LeftTermDataNode)];
                        if (row != null)
                            leftValue = row[LeftTermDataNode.Name];
                        if (leftValue is DBNull)
                            leftValue = null;

                        if (leftValue != null && LiteralValue != null && leftValue.GetType() != LiteralValue.GetType())
                        {
#if !DeviceDotNet
                            rightValue = System.Convert.ChangeType(LiteralValue, leftValue.GetType());
#else
                            rightValue = System.Convert.ChangeType(LiteralValue, leftValue.GetType(), System.Globalization.CultureInfo.CurrentCulture.NumberFormat);
#endif
                        }
                        else
                            rightValue = LiteralValue;
                        return ExecuteCondition(leftValue, rightValue);
                    }
                case ComparisonTermsType.LiteralWithObjectAttribute:
                    {
                        object leftValue = null;
                        object rightValue = null;
                        IDataRow row = GetRow(composedRow, dataNodeRowIndices, RightTermDataNode);
                        //composedRow[DataRetrieveNode.GetDataSourceRowIndex(dataNodeRowIndices, RightTermDataNode)];
                        if (row != null)
                            rightValue = row[RightTermDataNode.Name];
                        if (rightValue is DBNull)
                            rightValue = null;
                        if (rightValue != null && LiteralValue != null && rightValue.GetType() != LiteralValue.GetType())
                        {
#if !DeviceDotNet
                            leftValue = System.Convert.ChangeType(LiteralValue, rightValue.GetType());
#else
                            leftValue = System.Convert.ChangeType(LiteralValue, leftValue.GetType(), System.Globalization.CultureInfo.CurrentCulture.NumberFormat);
#endif
                        }
                        else
                            leftValue = LiteralValue;
                        return ExecuteCondition(leftValue, rightValue);
                    }
                case ComparisonTermsType.ObjectAttributeWithParameter:
                    {
                        object leftValue = null;
                        object rightValue = null;
                        if (AttributeValuePath == null)
                        {
                            System.Collections.Generic.Stack<DataNode> attributeValuePath = new System.Collections.Generic.Stack<DataNode>();
                            DataNode pathDataNode = LeftTermDataNode;
                            //if (pathDataNode is DerivedDataNode)
                            //    pathDataNode = (pathDataNode as DerivedDataNode).OrgDataNode;

                            if (DerivedDataNode.GetOrgDataNode(pathDataNode) is AggregateExpressionDataNode)
                            {
                                attributeValuePath.Push(DerivedDataNode.GetOrgDataNode(pathDataNode));
                                pathDataNode = pathDataNode.ParentDataNode;
                                
                            }
                            else
                            {
                                while (DerivedDataNode.GetOrgDataNode(pathDataNode).Type != DataNode.DataNodeType.Object&&
                                    DerivedDataNode.GetOrgDataNode(pathDataNode).Type != DataNode.DataNodeType.Key)
                                {
                                    attributeValuePath.Push(DerivedDataNode.GetOrgDataNode(pathDataNode));
                                    pathDataNode = pathDataNode.ParentDataNode;
                                }
                            }
                            if(DerivedDataNode.GetOrgDataNode(pathDataNode).Type == DataNode.DataNodeType.Key)
                                attributeValuePath.Push(DerivedDataNode.GetOrgDataNode(pathDataNode).ParentDataNode);
                            else
                                attributeValuePath.Push(pathDataNode);

                            AttributeValuePath = attributeValuePath.ToArray();
                        }
                        IDataRow row = GetRow(composedRow, dataNodeRowIndices, AttributeValuePath[0]);
                       // composedRow[DataRetrieveNode.GetDataSourceRowIndex(dataNodeRowIndices, AttributeValuePath[0])];
                        if (row != null)
                        {
                            int i = 1;
                            if (LeftTermDataNode is AggregateExpressionDataNode)
                                leftValue = row[LeftTermDataNode.Alias];
                            else
                            {
                                leftValue = row[AttributeValuePath[0].DataSource.GetColumnIndex(AttributeValuePath[i])];
                                i++;
                                while (i < AttributeValuePath.Length)
                                {
                                    if (leftValue == null || leftValue is DBNull)
                                        break;
                                    else
                                    {
                                        DataNode dataNode = AttributeValuePath[i++];
                                        if ((dataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FastPropertyAccessor != null)
                                            leftValue = (dataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FastPropertyAccessor.GetValue(leftValue);
                                        else
                                            leftValue = (dataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FastFieldAccessor.GetValue(leftValue);
                                    }
                                }
                            }
                        }
                        if (leftValue is DBNull)
                            leftValue = null;
                        rightValue = ParameterValue;
                        return ExecuteCondition(leftValue, rightValue);
                    }
                case ComparisonTermsType.ParameterWithObjectAttribute:
                    {
                        object leftValue = null;
                        object rightValue = null;
                        if (AttributeValuePath == null)
                        {
                            System.Collections.Generic.Stack<DataNode> attributeValuePath = new System.Collections.Generic.Stack<DataNode>();
                            DataNode pathDataNode = RightTermDataNode;
                            //if (pathDataNode is DerivedDataNode)
                            //    pathDataNode = (pathDataNode as DerivedDataNode).OrgDataNode;


                            while (DerivedDataNode.GetOrgDataNode(pathDataNode).Type != DataNode.DataNodeType.Object &&
                                   DerivedDataNode.GetOrgDataNode(pathDataNode).Type != DataNode.DataNodeType.Key)
                            {
                                attributeValuePath.Push(DerivedDataNode.GetOrgDataNode(pathDataNode));
                                pathDataNode = pathDataNode.ParentDataNode;
                            }

                            if (DerivedDataNode.GetOrgDataNode(pathDataNode).Type == DataNode.DataNodeType.Key)
                                attributeValuePath.Push(DerivedDataNode.GetOrgDataNode(pathDataNode).ParentDataNode);
                            else
                                attributeValuePath.Push(pathDataNode);
                            AttributeValuePath = attributeValuePath.ToArray();
                        }
                        IDataRow row = GetRow(composedRow, dataNodeRowIndices, AttributeValuePath[0]);
                        //composedRow[DataRetrieveNode.GetDataSourceRowIndex(dataNodeRowIndices, AttributeValuePath[0])];

                        if (row != null)
                        {
                            int i = 1;
                            rightValue = row[AttributeValuePath[i++].Name];
                            while (i < AttributeValuePath.Length)
                            {
                                if (rightValue == null || rightValue is DBNull)
                                    break;
                                else
                                {
                                    DataNode dataNode = AttributeValuePath[i++];
                                    if ((dataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FastPropertyAccessor != null)
                                        rightValue = (dataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FastPropertyAccessor.GetValue(rightValue);
                                    else
                                        rightValue = (dataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FastFieldAccessor.GetValue(rightValue);
                                }
                            }
                        }
                        if (rightValue is DBNull)
                            rightValue = null;
                        leftValue = ParameterValue;
                        return ExecuteCondition(leftValue, rightValue);
                    }
                case ComparisonTermsType.ObjectWithParameter:
                case ComparisonTermsType.ParameterWithObject:
                    {
                        object leftValue = null;
                        object rightValue = null;

                        IDataRow row = GetRow(composedRow, dataNodeRowIndices, LeftTermDataNode);
                        DataNode dataNode = LeftTermDataNode;
                        if (LeftTermDataNode == null)
                            dataNode = RightTermDataNode;
                        if (ComparisonOperator == ComparisonType.TypeIs)
                        {
                            if (row != null)
                            {
                                if (LeftTermDataNode != null)
                                    leftValue = row[LeftTermDataNode.DataSource.ObjectIndex];
                                else
                                    leftValue = row[RightTermDataNode.DataSource.ObjectIndex];


                                return (ParameterValue as Type).GetMetaData().IsInstanceOfType(leftValue);
                            }
                            else
                                return false;

                        }

                        if ((OverridenComparisonOperator == null || dataNode.DataSource.ObjectIndex == -1) && !(dataNode.Classifier is MetaDataRepository.Structure))
                        {
                            if (!GlobalObjectIDTermValue.HasValue)
                                GlobalObjectIDTermValue = GetGlobalObjectID(ParameterValue);
                            if (row != null)
                            {
                                if (GlobalObjectIDTermValue.HasValue)
                                {
                                    GlobalObjectID? globalObjectID = StorageDataSource. GetGlobalObjectID(row, GlobalObjectIDTermValue.Value.ObjectIdentityType);
                                    leftValue = globalObjectID.Value;
                                }
                                else
                                {
                                    GlobalObjectID? globalObjectID = StorageDataSource.GetGlobalObjectID(row, ObjectIdentityTypes);
                                    leftValue = globalObjectID.Value;
                                }
                            }
                            if (GlobalObjectIDTermValue.HasValue)
                                rightValue = GlobalObjectIDTermValue.Value;
                        }
                        else
                        {
                            if (row != null)
                                if (LeftTermDataNode != null)
                                    leftValue = row[LeftTermDataNode.DataSource.ObjectIndex];
                                else
                                    leftValue = row[RightTermDataNode.DataSource.ObjectIndex];
                            rightValue = ParameterValue;
                        }
                        if (OverridenComparisonOperator != null)
                            return (bool)OOAdvantech.AccessorBuilder.GetMethodInvoker(OverridenComparisonOperator).Invoke(null, new object[2] { leftValue, rightValue });

                        //Test case που τα objects είναι ιδια αλλά out of proccess
                        return ExecuteCondition(leftValue, rightValue);
                    }
                case ComparisonTermsType.ObjectWithLiteral:
                case ComparisonTermsType.LiteralWithObject:
                    {

                        DataNode dataNode = LeftTermDataNode;
                        if (LeftTermDataNode == null)
                            dataNode = RightTermDataNode;
                        if (!GlobalObjectIDTermValue.HasValue)
                        {
                            ObjectIDComparisonTerm objectIDComparisonTerm = null;
                            if (ComparisonTerms[0] is ObjectIDComparisonTerm)
                                objectIDComparisonTerm = ComparisonTerms[0] as ObjectIDComparisonTerm;
                            else
                                objectIDComparisonTerm = ComparisonTerms[1] as ObjectIDComparisonTerm;

                            ObjectIdentityType comparisonTermObjectIdentityType = null;
                            foreach (var objectIdentityType in dataNode.DataSource.ObjectIdentityTypes)
                            {
                                comparisonTermObjectIdentityType = objectIdentityType;
                                foreach (IIdentityPart part in objectIdentityType.Parts)
                                {
                                    if (!objectIDComparisonTerm.MultiPartObjectID.ContainsKey(part.PartTypeName))
                                    {
                                        comparisonTermObjectIdentityType = null;
                                        break;
                                    }
                                }
                                if (comparisonTermObjectIdentityType != null)
                                {
                                    foreach (var part in comparisonTermObjectIdentityType.Parts)
                                    {
                                        object partValue = objectIDComparisonTerm.MultiPartObjectID[part.PartTypeName];
                                        if (part.Type == typeof(Guid) && partValue is string)
                                        {
                                            try
                                            {
                                                partValue = new Guid(partValue as string);
                                            }
                                            catch (Exception exception)
                                            {
                                            }
                                            objectIDComparisonTerm.MultiPartObjectID[part.PartTypeName] = partValue;
                                        }

                                        if (partValue == null || (partValue.GetType() != part.Type && !partValue.GetType().GetMetaData().IsSubclassOf(part.Type)))
                                        {
                                            comparisonTermObjectIdentityType = null;
                                            break;
                                        }

                                    }
                                }
                            }

                            if (comparisonTermObjectIdentityType != null)
                                GlobalObjectIDTermValue = new GlobalObjectID(new System.Collections.Generic.List<object>(objectIDComparisonTerm.MultiPartObjectID.Values).ToArray(), comparisonTermObjectIdentityType, 0);

                        }


                        System.Collections.Generic.List<ObjectIdentityType> objectIdentityTypes = ObjectIdentityTypes;
                        object leftValue = null;

                        object rightValue = null;
                        if (GlobalObjectIDTermValue.HasValue)
                            rightValue = GlobalObjectIDTermValue.Value;
                        IDataRow row = null;

                        // GetGlobalObjectID(ro
                        if (ComparisonTerms[0] is ObjectIDComparisonTerm)
                        {
                            row = GetRow(composedRow, dataNodeRowIndices, RightTermDataNode);
                            //composedRow[DataRetrieveNode.GetDataSourceRowIndex(dataNodeRowIndices, RightTermDataNode)];
                        }
                        else
                        {
                            row = GetRow(composedRow, dataNodeRowIndices, LeftTermDataNode);
                         //   row = composedRow[DataRetrieveNode.GetDataSourceRowIndex(dataNodeRowIndices, LeftTermDataNode)];
                        }

                        if (IsNotNULL && row == null)
                            return false;
                        if (IsNotNULL && row != null)
                            return true;
                        if (IsNULL && row == null)
                            return true;
                        if (IsNULL && row != null)
                            return false;

                        if (GlobalObjectIDTermValue.HasValue)
                        {
                            GlobalObjectID? globalObjectID = StorageDataSource.GetGlobalObjectID(row, GlobalObjectIDTermValue.Value.ObjectIdentityType);
                            leftValue = globalObjectID.Value;
                        }
                        else
                        {
                            GlobalObjectID? globalObjectID = StorageDataSource.GetGlobalObjectID(row, ObjectIdentityTypes);
                            leftValue = globalObjectID.Value;
                        }



                        return ExecuteCondition(leftValue, rightValue);
                        //foreach (string fieldName in objectIDComparisonTerm.MultiPartObjectID.Keys)
                        //{
                        //    if (row != null)
                        //        leftValue = row[fieldName];

                        //    rightValue = objectIDComparisonTerm.MultiPartObjectID[fieldName];
                        //    if (leftValue is System.Guid && rightValue is string)
                        //    {

                        //        System.Collections.Generic.Dictionary<string, object> multiPartObjectID = new System.Collections.Generic.Dictionary<string, object>(objectIDComparisonTerm.MultiPartObjectID);

                        //        multiPartObjectID[fieldName] = new Guid(rightValue as string);
                        //        rightValue = multiPartObjectID[fieldName];
                        //        objectIDComparisonTerm.MultiPartObjectID = multiPartObjectID;
                        //    }
                        //    if (ComparisonOperator == ComparisonType.Equal && !ExecuteCondition(leftValue, rightValue))
                        //        return false;
                        //    if (ComparisonOperator == ComparisonType.NotEqual && ExecuteCondition(leftValue, rightValue))
                        //        return true;
                        //}
                        //if (ComparisonOperator == ComparisonType.Equal)
                        //    return true;
                        //if (ComparisonOperator == ComparisonType.NotEqual)
                        //    return false;
                        //return false;
                    }

                case ComparisonTermsType.Objects:
                    {
                        throw new Exception("The functionallity is not implemented.");
                    }
                case ComparisonTermsType.CollectionContainsAnyAll:
                    {
                        object leftValue = null;
                        object rightValue = null;
                        IDataRow row = GetRow(composedRow, dataNodeRowIndices, LeftTermDataNode);
                        //composedRow[DataRetrieveNode.GetDataSourceRowIndex(dataNodeRowIndices, LeftTermDataNode)];

                        if (row != null)
                            rightValue = row["Object"];
                        leftValue = ParameterValue;
                        System.Collections.Generic.List<object> valuesCollection = new System.Collections.Generic.List<object>();
                        System.Collections.Generic.List<IDataRow> rows = new System.Collections.Generic.List<IDataRow>();
                        if(GetRow(composedRow, dataNodeRowIndices, LeftTermDataNode)!=null)
                            rows.Add(GetRow(composedRow, dataNodeRowIndices, LeftTermDataNode));

                        //rows.Add(composedRow[DataRetrieveNode.GetDataSourceRowIndex(dataNodeRowIndices, LeftTermDataNode)]);
                        GetCollection(LeftTermDataNode, rows, valuesCollection);
                        if (ComparisonOperator == ComparisonType.ContainsAny)
                        {

                            foreach (object obj in ParameterValue as System.Collections.IEnumerable)
                            {
                                if (CollectionContainsDataNode.Type == DataNode.DataNodeType.Object)
                                {
                                    GlobalObjectID? globalObjectID = GetGlobalObjectID(obj);
                                    if (globalObjectID.HasValue)
                                    {
                                        if (valuesCollection.Contains(globalObjectID.Value))
                                            return true;
                                    }
                                }
                                else
                                {
                                    if (obj != null && valuesCollection.Contains(obj))
                                        return true;
                                }
                                //*********************** old code
                                //PersistenceLayer.ObjectStorage objectStorage = PersistenceLayer.ObjectStorage.GetStorageOfObject(obj);
                                //if (objectStorage != null)
                                //{
                                //    object objectID = PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(obj).TransientObjectID;
                                //    int storageIdentity = (OQLStatement as StorageObjectQuery).QueryStorageIdentities.IndexOf(objectStorage.StorageMetaData.StorageIdentity);
                                //    GlobalObjectID globalObjectID = new GlobalObjectID(objectID, storageIdentity);
                                //    if (valuesCollection.Contains(globalObjectID))
                                //        return true;
                                //}
                                //*************************
                            }
                            return false;
                        }
                        else
                        {
                            foreach (object obj in ParameterValue as System.Collections.IEnumerable)
                            {
                                if (CollectionContainsDataNode.Type == DataNode.DataNodeType.Object)
                                {

                                    PersistenceLayer.ObjectID objectID = null;
                                    string storageIdentity = null;
                                    PersistenceLayer.StorageInstanceRef.GetObjectID(obj, out objectID, out storageIdentity);
                                    if (objectID != null)
                                    {
                                        DataNode dataNode = LeftTermDataNode;
                                        if (LeftTermDataNode == null)
                                            dataNode = RightTermDataNode;

                                        System.Collections.Generic.List<IIdentityPart> parts = new System.Collections.Generic.List<IIdentityPart>();
                                        ///TODO κάπιο cashing πρέπει να γίνει γιατί θα είναι αργό
                                        MetaDataRepository.ObjectIdentityType objectIdentityType = null;
                                        int storageIndex = (OQLStatement as ObjectsContextQuery).QueryStorageIdentities.IndexOf(storageIdentity);
                                        if (dataNode.DataSource.DataLoadedInParentDataSource)
                                        {
                                            foreach (var identityPart in objectID.ObjectIdentityType.Parts)
                                                parts.Add(new IdentityPart(dataNode.Alias + "_" + identityPart.PartTypeName, identityPart.PartTypeName, identityPart.Type));
                                            objectIdentityType = new ObjectIdentityType(parts);
                                        }
                                        else
                                            objectIdentityType = objectID.ObjectIdentityType;
                                        GlobalObjectID globalObjectID = new GlobalObjectID(objectID.ObjectIDPartsValues, objectIdentityType, storageIndex);
                                        if (!valuesCollection.Contains(globalObjectID))
                                            return false;
                                    }
                                    else
                                        return false;
                                }
                                else
                                {

                                    if (!valuesCollection.Contains(obj))
                                        return false;

                                }

                            }

                            return true;


                        }
                    }
                default:
                    throw new Exception("The functionallity is not implemented.");


            }


             




        }

        private IDataRow GetRow(IDataRow[] composedRow, Dictionary<DataNode, int> dataNodeRowIndices, DataNode dataNode)
        {
            int rowIndex = DataRetrieveNode.GetDataSourceRowIndex(dataNodeRowIndices, dataNode);
            if (rowIndex == -1)
                return null;
            return composedRow[rowIndex];
        }

        /// <MetaDataID>{78983280-4c4e-481e-a524-6002cb1f2d76}</MetaDataID>
        System.Collections.Generic.Dictionary<object, GlobalObjectID?> ParameterValuesGlobalObjectIDs = new System.Collections.Generic.Dictionary<object, GlobalObjectID?>();

        /// <MetaDataID>{4bcea1eb-8436-4dd3-b5f9-69f09689db66}</MetaDataID>
        private GlobalObjectID? GetGlobalObjectID(object obj)
        {

            GlobalObjectID? globalObjectID;
            if (obj == null || (obj.GetType().GetMetaData().IsValueType))
                return null;

            if (!ParameterValuesGlobalObjectIDs.TryGetValue(obj, out globalObjectID))
            {
                PersistenceLayer.ObjectID objectID = null;
                string storageIdentity = null;
                if (obj is OOAdvantech.PersistenceLayer.ObjectID)
                {
                    objectID = obj as OOAdvantech.PersistenceLayer.ObjectID;
                    storageIdentity = objectID.StorageIdentity;
                }
                else
                    PersistenceLayer.StorageInstanceRef.GetObjectID(obj, out objectID, out storageIdentity);

                if (objectID != null)
                {
                    DataNode dataNode = LeftTermDataNode;
                    if (LeftTermDataNode == null)
                        dataNode = RightTermDataNode;

                    System.Collections.Generic.List<IIdentityPart> parts = new System.Collections.Generic.List<IIdentityPart>();
                    ///TODO κάπιο cashing πρέπει να γίνει γιατί θα είναι αργό
                    MetaDataRepository.ObjectIdentityType objectIdentityType = null;

                    int storageIndex = 0;
                    if (OQLStatement is DistributedObjectQuery)
                        storageIndex = (OQLStatement as DistributedObjectQuery).QueryStorageIdentities.IndexOf(storageIdentity);
                    else
                        storageIndex = (OQLStatement as ObjectsContextQuery).QueryStorageIdentities.IndexOf(storageIdentity);
                    if (dataNode.DataSource.DataLoadedInParentDataSource)
                    {
                        foreach (var identityPart in objectID.ObjectIdentityType.Parts)
                            parts.Add(new IdentityPart(dataNode.Alias + "_" + identityPart.PartTypeName, identityPart.PartTypeName, identityPart.Type));
                        objectIdentityType = new ObjectIdentityType(parts);
                    }
                    else
                        objectIdentityType = objectID.ObjectIdentityType;
                    globalObjectID = new GlobalObjectID(objectID.ObjectIDPartsValues, objectIdentityType, storageIndex);
                    ParameterValuesGlobalObjectIDs[obj] = globalObjectID;
                    return globalObjectID;
                }
                else
                {
                    ParameterValuesGlobalObjectIDs[obj] = globalObjectID;
                    return globalObjectID;
                }
            }
            return globalObjectID;

        }

        /// <MetaDataID>{53629df2-cd50-4647-b5c4-0c2fc748c519}</MetaDataID>
        System.Collections.Generic.List<ObjectIdentityType> _ObjectIdentityTypes;
        /// <MetaDataID>{d41b36d9-564d-4bda-98a4-ae73fa96d118}</MetaDataID>
        private System.Collections.Generic.List<ObjectIdentityType> ObjectIdentityTypes
        {
            get
            {
                if (_ObjectIdentityTypes == null)
                {
                    DataNode dataNode = LeftTermDataNode;
                    if (dataNode == null)
                        dataNode = RightTermDataNode;
                    System.Collections.Generic.List<IIdentityPart> parts = new System.Collections.Generic.List<IIdentityPart>();
                    if (dataNode.DataSource.DataLoadedInParentDataSource)
                    {
                        _ObjectIdentityTypes = new System.Collections.Generic.List<ObjectIdentityType>();
                        foreach (var objectIdentityType in dataNode.DataSource.ObjectIdentityTypes)
                        {
                            foreach (var identityPart in objectIdentityType.Parts)
                                parts.Add(new IdentityPart(dataNode.Alias + "_" + identityPart.PartTypeName, identityPart.PartTypeName, identityPart.Type));
                            _ObjectIdentityTypes.Add(new ObjectIdentityType(parts));
                        }
                    }
                    else
                        _ObjectIdentityTypes = dataNode.DataSource.ObjectIdentityTypes;
                }
                return _ObjectIdentityTypes;
            }
        }

     

        /// <MetaDataID>{708a0497-4160-4e11-9a67-8b296836080d}</MetaDataID>
        private void GetCollection(DataNode rootDataNode, System.Collections.Generic.ICollection<IDataRow> masterRows, System.Collections.Generic.List<object> values)
        {
            foreach (DataNode dataNode in rootDataNode.SubDataNodes)
            {
                if (CollectionContainsDataNode.IsSameOrParentDataNode(dataNode))
                {
                    System.Collections.Generic.ICollection<IDataRow> rows = rootDataNode.DataSource.GetRelatedRows(masterRows, dataNode);
                    if (dataNode == CollectionContainsDataNode || CollectionContainsDataNode.Type == DataNode.DataNodeType.OjectAttribute && CollectionContainsDataNode.ParentDataNode == dataNode)
                    {
                        if (dataNode == CollectionContainsDataNode)
                        {
                            foreach (IDataRow row in rows)
                            {
                                GlobalObjectID? globalObjectID = StorageDataSource.GetGlobalObjectID(row, ObjectIdentityTypes);
                                if (globalObjectID.HasValue)
                                    values.Add(globalObjectID.Value);
                            }
                            return;
                        }
                        else
                        {
                            int columnIndex = CollectionContainsDataNode.DataSourceColumnIndex;
                            foreach (IDataRow row in rows)
                                values.Add(row[columnIndex]);
                            return;
                        }
                    }
                    else
                    {
                        GetCollection(dataNode, rows, values);
                    }
                }
            }
        }

       
        string toStringCashing;
        /// <MetaDataID>{205631b7-b727-4168-9e4d-3e4fda3706e1}</MetaDataID>
        public override string ToString()
        {
            if (toStringCashing == null)
                toStringCashing = _ComparisonTerms[0].ToString() + " " + ComparisonOperator.ToString() + " " + _ComparisonTerms[1].ToString();

            return toStringCashing;
        }

        internal Criterion Clone()
        {
            var newCriterion = new Criterion(ComparisonOperator, ComparisonTerms, OQLStatement, ConstrainCriterion, null, Identity);
            return newCriterion;
        }

        protected Criterion(Guid identity, ComparisonTermsType criterionType,bool isNotNULL, bool isNULL, DataNode leftTermDataNode, DataNode rightTermDataNode)
        {
            CriterionType = criterionType;
            Identity = identity;
            IsNotNULL = isNotNULL;
            IsNULL = isNULL;
            LeftTermDataNode = leftTermDataNode;
            RightTermDataNode = rightTermDataNode;
        }

     

        internal Criterion Clone(System.Collections.Generic.Dictionary<object, object> clonedObjects)
        {
            DataNode leftTermDataNode = null;
            if (LeftTermDataNode != null)
                leftTermDataNode = LeftTermDataNode.Clone(clonedObjects);

            DataNode rightTermDataNode = null;
            if (RightTermDataNode != null)
                rightTermDataNode = RightTermDataNode.Clone(clonedObjects);
         
            Criterion newCriterion = new Criterion(Identity, CriterionType, IsNotNULL, IsNULL, leftTermDataNode, rightTermDataNode);
            Copy(newCriterion, clonedObjects);


            return newCriterion;
        }

        private void Copy(Criterion newCriterion,System.Collections.Generic.Dictionary<object, object> clonedObjects)
        {
            if(newCriterion._Applied!=null)
                newCriterion._Applied = newCriterion._Applied.Value;
            newCriterion._ComparisonTerms = new ComparisonTerm[_ComparisonTerms.Length];
            int i = 0;
            foreach (ComparisonTerm comparisonTerm in _ComparisonTerms)
                newCriterion._ComparisonTerms[i++] = comparisonTerm.Clone(clonedObjects);

            if (_ObjectIdentityTypes != null)
            {
                newCriterion._ObjectIdentityTypes = new System.Collections.Generic.List<ObjectIdentityType>();
                i = 0;
                foreach (var objectIdentityType in _ObjectIdentityTypes)
                    newCriterion._ObjectIdentityTypes.Add(objectIdentityType.Clone(clonedObjects));
            }

            newCriterion._ParameterValue = _ParameterValue;

            if (AttributeValuePath != null)
            {
                newCriterion.AttributeValuePath = new DataNode[AttributeValuePath.Length];
                i = 0;
                foreach (DataNode dataNode in AttributeValuePath)
                {
                    object createDataNode = null;
                    if (clonedObjects.TryGetValue(dataNode, out createDataNode))
                        newCriterion.AttributeValuePath[i++] = createDataNode as DataNode;
                    else
                        newCriterion.AttributeValuePath[i++] = dataNode.Clone(clonedObjects);
                }
            }

            if(CollectionContainsDataNode!=null)
                newCriterion.CollectionContainsDataNode = CollectionContainsDataNode.Clone(clonedObjects);

            newCriterion.ComparisonOperator = ComparisonOperator;
            newCriterion.GlobalObjectIDTermValue = GlobalObjectIDTermValue;
           
            newCriterion.LiteralValue = LiteralValue;
            newCriterion.LiteralValueType = LiteralValueType;
            newCriterion.OverridenComparisonOperator = OverridenComparisonOperator;

              object createOwner = null;
                if (clonedObjects.TryGetValue(Owner, out createOwner))
                    newCriterion.Owner=createOwner as SearchFactor;
                else
                    newCriterion.Owner = Owner.Clone(clonedObjects);


            if(ParameterComparisonTerm!=null)
                newCriterion.ParameterComparisonTerm = ParameterComparisonTerm.Clone(clonedObjects) as ParameterComparisonTerm;

            if (ParameterValuesGlobalObjectIDs != null)
            {
                foreach (var parameterValueEntry in ParameterValuesGlobalObjectIDs)
                    newCriterion.ParameterValuesGlobalObjectIDs[parameterValueEntry.Key] = new GlobalObjectID(parameterValueEntry.Value.Value.ObjectIDPartValues, parameterValueEntry.Value.Value.ObjectIdentityType, parameterValueEntry.Value.Value.StorageID);
            }
            newCriterion.ParameterValueType = ParameterValueType;
            newCriterion.ParserNode = ParserNode;
            
            newCriterion.Tag = Tag;
            newCriterion.toStringCashing = toStringCashing;
            

        }
    }
    /// <MetaDataID>{fe6c214a-0dde-4ddf-a05c-7eec376d5e25}</MetaDataID>
    [System.Serializable]
    internal struct GlobalObjectID
    {
        public object[] ObjectIDPartValues;
        public int StorageID;
        public MetaDataRepository.ObjectIdentityType ObjectIdentityType;
        public GlobalObjectID(object[] objectIDPartValues, MetaDataRepository.ObjectIdentityType objectIdentityType, int storageID)
        {
            ObjectIdentityType = objectIdentityType;
            ObjectIDPartValues = objectIDPartValues;
            StorageID = storageID;
        }

        public override int GetHashCode()
        {
            int num = -1162279000;
            foreach (object partValue in ObjectIDPartValues)
                num = (-1521134295 * num) + GetHashCode(partValue);
            return num;
            //return base.GetHashCode();
        }

        private int GetHashCode(object partValue)
        {
            if (partValue == null)
                return 0;
            else
                return partValue.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj is GlobalObjectID)
                return this == (GlobalObjectID)obj;
            else
                return false;
        }
        public static bool operator ==(GlobalObjectID left, GlobalObjectID right)
        {
            //left.ObjectID == right.ObjectID &&
            if (left.StorageID == right.StorageID && left.ObjectIdentityType == right.ObjectIdentityType)// && left.UnitMeasure == right.UnitMeasure)
            {
                int i = 0;
                foreach (var leftPartValue in left.ObjectIDPartValues)
                {
                    if (!leftPartValue.Equals(right.ObjectIDPartValues[i++]))
                        return false;
                }
                return true;
            }
            else
                return false;
        }

        public static bool operator !=(GlobalObjectID left, GlobalObjectID right)
        {
            return !(left == right);
        }

        public bool IsEmpty
        {
            get
            {
                return ObjectIDPartValues == null;
            }
        }
    }

}
