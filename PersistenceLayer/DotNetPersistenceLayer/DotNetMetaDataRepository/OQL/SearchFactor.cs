using System;
namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    /// <MetaDataID>{73BC1058-17AB-432B-8E33-66C8BCB91E2D}</MetaDataID>
    /// <summary>Define a condition with result true or false. Condition can be a simple comparison expression or a more complex condition, a composite search condition. </summary>
    [Serializable]
    public class SearchFactor
    {

        /// <MetaDataID>{66f0e54f-f613-48da-8af3-4eb0e0388f94}</MetaDataID>
        public System.Collections.Generic.List<DataNode> DataNodes
        {
            get
            {
                System.Collections.Generic.List<DataNode> dataNodes = new System.Collections.Generic.List<DataNode>();
                foreach (Criterion criterion in Criterions)
                {
                    if (criterion.LeftTermDataNode != null)
                        dataNodes.Add(criterion.LeftTermDataNode);
                    if (criterion.RightTermDataNode != null)
                        dataNodes.Add(criterion.RightTermDataNode);
                }
                return dataNodes;
            }
        }

        /// <MetaDataID>{e201f35c-2476-47ba-91d3-01bd3121788c}</MetaDataID>
        public static bool operator ==(SearchFactor leftSearchFactor, SearchFactor rightSearchFactor)
        {
            if (!(leftSearchFactor is SearchFactor) && !(rightSearchFactor is SearchFactor))
                return true;

            if (leftSearchFactor is SearchFactor && rightSearchFactor is SearchFactor)
            {

                if(leftSearchFactor.Criterion!=null&&rightSearchFactor.Criterion!=null&&rightSearchFactor.Criterion.ToString()==leftSearchFactor.Criterion.ToString())
                    return true;
                if (leftSearchFactor.SearchCondition != null && rightSearchFactor.SearchCondition != null && rightSearchFactor.SearchCondition == leftSearchFactor.SearchCondition)
                    return true;
                return false;
    
            }
            else
                return false;

        }
        /// <MetaDataID>{3b7f0710-d782-409e-b420-a02c2d0b60d1}</MetaDataID>
        public static bool operator !=(SearchFactor leftSearchFactor, SearchFactor rightSearchFactor)
        {
            return !(leftSearchFactor == rightSearchFactor);
        }
        /// <MetaDataID>{e8487d07-1d91-4ac7-a9ed-188aa0ba50b3}</MetaDataID>
        SearchTerm _OwnerSearchTerm;
        /// <MetaDataID>{714563a5-bf5c-42be-a676-baaa98110088}</MetaDataID>
        public SearchTerm OwnerSearchTerm
        {
            get
            {
                return _OwnerSearchTerm;
            }
            internal set
            {
                _OwnerSearchTerm = value;
            }
        }
        /// <MetaDataID>{7de35bbf-2112-4c5e-bc1b-ba19a1cb398f}</MetaDataID>
        public bool IsNotExpression = false;
        /// <MetaDataID>{cf3950d6-1189-4699-99fa-08e7cbd5b121}</MetaDataID>
        public override string ToString()
        {
            string tmp = null;
            if (SearchCondition != null)
            {
                int searchTermCount = SearchCondition.SearchTerms.Count;
                foreach (SearchTerm searchTerm in SearchCondition.SearchTerms)
                {
                    if (tmp == null)
                    {
                        if (searchTermCount > 1)
                            tmp = "(";
                    }
                    else
                        tmp += " OR ";
                    tmp += searchTerm.ToString();
                }
                if (searchTermCount > 1)
                    tmp += ")";
                if (tmp == null)
                    tmp = "";
                return tmp;
            }
            else
                return Criterion.ToString();
            
        }
        /// <exclude>Excluded</exclude>
        SearchCondition _SearchCondition;
        [RoleBMultiplicityRange(0)]
        [RoleAMultiplicityRange(0, 1)]
        [Association("", typeof(SearchCondition), Roles.RoleA, "{18224D96-7D74-4B8A-BEEF-AD9E532517B6}")]
        [IgnoreErrorCheck]
        public SearchCondition SearchCondition
        {
            get
            {
                return _SearchCondition;
            }
            set
            {
                if (_SearchCondition != null)
                    _SearchCondition.OwnerSearchFactor = null;
 
                _SearchCondition = value;
                if (_SearchCondition != null)
                    _SearchCondition.OwnerSearchFactor = this;
 
            }
        }


        [RoleAMultiplicityRange(0, 1)]
        [Association("SearchCriterion", Roles.RoleA, "{73EDD6C7-8D5E-4964-BC25-9DFE5BB30121}")]
        public Criterion Criterion;
        /// <MetaDataID>{c435213a-2170-4ab2-986f-e404f9402a33}</MetaDataID>
        public System.Collections.Generic.List<Criterion> Criterions
        {
            get
            {
                if (SearchCondition != null)
                    return SearchCondition.Criterions;
                else
                {
                    System.Collections.Generic.List<Criterion> criterions = new System.Collections.Generic.List<Criterion>();
                    criterions.Add(Criterion);
                    return criterions;
                }
            }
        }
        /// <MetaDataID>{6F39C24C-8744-487F-B00D-E28D2B6BF0FE}</MetaDataID>
        public DataNode GetObjectIDDataNodeConstrain(DataNode dataNodeTreeHeader)
        {
            if (SearchCondition != null)
                return (SearchCondition as SearchCondition).GetObjectIDDataNodeConstrain(dataNodeTreeHeader);
            else
            {
                if (Criterion.ComparisonTerms[0] is MetaDataRepository.ObjectQueryLanguage.ObjectIDComparisonTerm && Criterion.ComparisonTerms[1] is ObjectComparisonTerm)
                {
                    ObjectComparisonTerm objectComparisonTerm = Criterion.ComparisonTerms[1] as ObjectComparisonTerm;
                    MetaDataRepository.ObjectQueryLanguage.ObjectIDComparisonTerm objectIDComparisonTerm = Criterion.ComparisonTerms[0] as MetaDataRepository.ObjectQueryLanguage.ObjectIDComparisonTerm;


                    (objectComparisonTerm.DataNode as DataNode).ObjectIDConstrainStorageCell = objectComparisonTerm.GetStorageCellFromObjectID(objectIDComparisonTerm.ComparisonTermParserNode["ObjectID"] as Parser.ParserNode);

                    DataNode constrainDataNode = objectComparisonTerm.DataNode as DataNode;
                    if (constrainDataNode.HeaderDataNode == dataNodeTreeHeader)
                        return constrainDataNode;
                    else
                        return null;

                }
                else if (Criterion.ComparisonTerms[1] is MetaDataRepository.ObjectQueryLanguage.ObjectIDComparisonTerm && Criterion.ComparisonTerms[0] is ObjectComparisonTerm)
                {
                    ObjectComparisonTerm objectComparisonTerm = Criterion.ComparisonTerms[0] as ObjectComparisonTerm;
                    MetaDataRepository.ObjectQueryLanguage.ObjectIDComparisonTerm objectIDComparisonTerm = Criterion.ComparisonTerms[1] as MetaDataRepository.ObjectQueryLanguage.ObjectIDComparisonTerm;

                    (objectComparisonTerm.DataNode as DataNode).ObjectIDConstrainStorageCell = objectComparisonTerm.GetStorageCellFromObjectID(objectIDComparisonTerm.ComparisonTermParserNode["ObjectID"] as Parser.ParserNode);

                    DataNode constrainDataNode = objectComparisonTerm.DataNode as DataNode;
                    if (constrainDataNode.HeaderDataNode == dataNodeTreeHeader)
                        return constrainDataNode;
                    else
                        return null;

                }
                else if (Criterion.ComparisonTerms[0] is ParameterComparisonTerm && Criterion.ComparisonTerms[1] is ObjectComparisonTerm)
                {
                    ObjectComparisonTerm objectComparisonTerm = Criterion.ComparisonTerms[1] as ObjectComparisonTerm;
                    ParameterComparisonTerm parameterComparisonTerm = Criterion.ComparisonTerms[0] as ParameterComparisonTerm;


                    (objectComparisonTerm.DataNode as DataNode).ObjectIDConstrainStorageCell = objectComparisonTerm.GetStorageCellFromParameterValue(parameterComparisonTerm.ParameterValue);

                    DataNode constrainDataNode = objectComparisonTerm.DataNode as DataNode;
                    if (constrainDataNode.HeaderDataNode == dataNodeTreeHeader)
                        return constrainDataNode;
                    else
                        return null;


                }
                else if (Criterion.ComparisonTerms[1] is ParameterComparisonTerm && Criterion.ComparisonTerms[0] is ObjectComparisonTerm)
                {
                    ObjectComparisonTerm objectComparisonTerm = Criterion.ComparisonTerms[0] as ObjectComparisonTerm;
                    ParameterComparisonTerm parameterComparisonTerm = Criterion.ComparisonTerms[1] as ParameterComparisonTerm;


                    (objectComparisonTerm.DataNode as DataNode).ObjectIDConstrainStorageCell = objectComparisonTerm.GetStorageCellFromParameterValue(parameterComparisonTerm.ParameterValue);

                    DataNode constrainDataNode = objectComparisonTerm.DataNode as DataNode;
                    if (constrainDataNode.HeaderDataNode == dataNodeTreeHeader)
                        return constrainDataNode;
                    else
                        return null;
                }

            }
            return null;

        }

        /// <MetaDataID>{3E78CCA0-FD91-471E-A83E-4BC1A2A9C0F0}</MetaDataID>
        public SearchFactor(SearchCondition searchCondition)
        {
            SearchCondition = searchCondition;
            if (SearchCondition != null)
                SearchCondition.OwnerSearchFactor=this;
        }
        /// <MetaDataID>{5035a7b1-200c-4bc5-a7f4-19f405c2b13e}</MetaDataID>
        public SearchFactor()
        {

        }


        /// <MetaDataID>{ECC8B1C6-0A9E-4CC0-A104-0D34804DD639}</MetaDataID>
        public SearchFactor(SearchFactor searchFactor)
        {
            Criterion = searchFactor.Criterion;

        }
        /// <MetaDataID>{27a50041-b666-453a-90f2-8e6c082b7e7c}</MetaDataID>
        bool ConstrainFactor;

        /// <MetaDataID>{F69531C9-B4A5-4A5B-A5CF-A3737716D356}</MetaDataID>
        internal protected SearchFactor(Parser.ParserNode searchFactorParserNode, ObjectsContextQuery oqlStatement, bool constrainFactor)
        {
            Parser.ParserNode searchConditionParserNode = searchFactorParserNode["Search_Condition"] as Parser.ParserNode;
            if (searchConditionParserNode != null)
                SearchCondition = CreateSearchCondition(searchConditionParserNode, oqlStatement, constrainFactor);

            Parser.ParserNode criterionParserNode = searchFactorParserNode["Criterion"] as Parser.ParserNode;
            if (criterionParserNode != null)
                Criterion = CreateCriterion(criterionParserNode, oqlStatement, constrainFactor);

            ConstrainFactor = constrainFactor;
            if (SearchCondition != null)
                SearchCondition.OwnerSearchFactor = this;

        }
        /// <MetaDataID>{1610BEA0-2E92-48E3-91DE-A01AE5E09C9D}</MetaDataID>
        protected virtual SearchCondition CreateSearchCondition(Parser.ParserNode searchConditionParserNode, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.ObjectsContextQuery oqlStatement, bool constrainTerm)
        {
            return new SearchCondition(searchConditionParserNode, oqlStatement, constrainTerm);

        }

        /// <MetaDataID>{47357683-52C4-4267-8FD8-EBE71AA4B2CD}</MetaDataID>
        protected virtual Criterion CreateCriterion(Parser.ParserNode criterionParserNode, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.ObjectsContextQuery oqlStatement, bool constrainCriterion)
        {
            return new Criterion(criterionParserNode, oqlStatement, constrainCriterion,this);
        }

        ///// <MetaDataID>{E7CD71E7-FFA0-4806-8CF3-DCFFD3659647}</MetaDataID>
        //public Criterion Criterion;
        ///// <MetaDataID>{39A19DCB-5419-4D34-A722-87739F883877}</MetaDataID>
        //public SearchCondition SearchCondition;

        #region Filter retrieved data
        ///// <MetaDataID>{ed33d27e-262b-432f-b546-b9d7b048bbc5}</MetaDataID>
        //internal bool DoesRowPassCondition(System.Data.DataRow row, DataNode ownerDataNode)
        //{
        //    if (SearchCondition != null)
        //        return SearchCondition.DoesRowPassCondition(row, ownerDataNode);
        //    else
        //        return Criterion.DoesRowPassCondition(row, ownerDataNode);
        //}


        ///<summary>
        ///This method checks if the virtual data row qualifies the search factor.  
        ///</summary>
        /// <param name="composedRow">
        /// Defines the virtual data row.
        /// </param>
        /// <MetaDataID>{6d96fcd3-f57e-4c29-9d95-39ae29fafd5a}</MetaDataID>
        internal bool DoesRowPassCondition(IDataRow[] composedRow, System.Collections.Generic.Dictionary<DataNode, int> dataNodeRowIndices)
        {
            bool result;
            if (SearchCondition != null)
                result = SearchCondition.DoesRowPassCondition(composedRow, dataNodeRowIndices);
            else
            {
                //if (!Criterion.SearhConditionHeader.CanBeApplied(Criterion))
                //    result = true;
                //else
                    result = Criterion.DoesRowPassCondition(composedRow, dataNodeRowIndices);
            }
            if (IsNotExpression)
                result = !result;
            return result;
        }
        #endregion

        /// <MetaDataID>{b09583c0-d7f9-4393-995c-9721950545e4}</MetaDataID>
        internal bool HaveAllCommonFactor(System.Collections.Generic.List<Criterion> criterions)
        {
            if (criterions.Count == Criterions.Count)
            {
                foreach (Criterion criterion in criterions)
                {
                    if (!Criterions.Contains(criterion))
                        return false;
                }
                return true;
            }
            if (criterions.Count > Criterions.Count)
                return false;

            if (SearchCondition != null)
                return SearchCondition.ActsAsConstrainCriterion(criterions);
            else
                return false;
        }

        /// <MetaDataID>{f1315941-4027-4eea-bdf5-881daab66807}</MetaDataID>
        internal bool HasAndRelation(Criterion criterion, Criterion outGroupCriterion)
        {
            if (SearchCondition == null)
                return false;
            else
            {
                foreach (SearchTerm searchTerm in SearchCondition.SearchTerms)
                {
                    if (searchTerm.ContainsCriterion(criterion) && searchTerm.ContainsCriterion(outGroupCriterion))
                    {
                        foreach (SearchFactor searchFactor in searchTerm.SearchFactors)
                        {
                            if (searchFactor.Criterions.Contains(outGroupCriterion) && searchFactor.Criterions.Contains(criterion)
                                        && searchFactor.HasAndRelation(criterion, outGroupCriterion))
                                return false;
                            if ((searchFactor.Criterions.Contains(outGroupCriterion) && !searchFactor.Criterions.Contains(criterion) ||
                                !searchFactor.Criterions.Contains(outGroupCriterion) && searchFactor.Criterions.Contains(criterion)))
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;

            }
            
        }

        /// <MetaDataID>{a5fec3e6-4c46-4578-8869-64f0ee1f550a}</MetaDataID>
        public bool ContainsCriterion(Criterion criterion)
        {
            if (SearchCondition != null)
                return SearchCondition.ContainsCriterion(criterion);
            else
                return criterion == Criterion;
        }

        /// <MetaDataID>{cd27f11c-ade4-40e8-848e-90ac283cdd6e}</MetaDataID>
        internal SearchFactor Clone()
        {
            if (SearchCondition != null)
                return new SearchFactor(SearchCondition.Clone());
            else
            {
                SearchFactor searchFactor = new SearchFactor();
                searchFactor.Criterion = Criterion.Clone();
                searchFactor.Criterion.Owner = searchFactor;
                return searchFactor;
            }

            
        }

        internal SearchFactor Clone(System.Collections.Generic.Dictionary<object, object> clonedObjects)
        {

            SearchFactor newSearchFactor = new SearchFactor();
            clonedObjects[this] = newSearchFactor;
            if (SearchCondition != null)
            {
                object newSearchCondition = null;
                if (clonedObjects.TryGetValue(SearchCondition, out newSearchCondition))
                    newSearchFactor._SearchCondition = newSearchCondition as SearchCondition;
                else
                    newSearchFactor._SearchCondition = SearchCondition.Clone(clonedObjects);
            }
            else
            {
                object newCriterion = null;
                if (clonedObjects.TryGetValue(Criterion, out newCriterion))
                    newSearchFactor.Criterion = newCriterion as Criterion;
                else
                    newSearchFactor.Criterion = Criterion.Clone(clonedObjects);
 
            }
            newSearchFactor.ConstrainFactor = ConstrainFactor;
            newSearchFactor.IsNotExpression = IsNotExpression;

            object newSearchTerm = null;
            if (clonedObjects.TryGetValue(_OwnerSearchTerm, out newSearchTerm))
                newSearchFactor._OwnerSearchTerm = newSearchTerm as SearchTerm;
            else
                newSearchFactor._OwnerSearchTerm = _OwnerSearchTerm.Clone(clonedObjects);


            return newSearchFactor;
        }
    }
}
