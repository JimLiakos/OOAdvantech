using System;
using System.Collections.Generic;
using System.Linq;
namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{

    /// <summary>
    /// Search condition keeps the criteria. 
    /// The query engine use the search condition to filter the retrieved data.
    /// </summary>
    /// <remarks>
    ///Because the persistency system can use many mechanisms save objects state like RDBMS, XML, file stream etc 
    ///the search condition can be resolved at all or some criterions (partially)  
    ///from corresponding data base management mechanisms. 
    ///The criterions where resolved from data base management mechanisms doesn’t applied in query retrieved data in memory.       
    /// </remarks>
    /// <MetaDataID>{50014F55-97F4-4AA3-A67F-9D59356AFE85}</MetaDataID>
    [Serializable]
    public class SearchCondition
    {

        /// <MetaDataID>{75a09893-d8e2-44a9-aa1e-4c31128ab257}</MetaDataID>
        public static SearchCondition JoinSearchConditions(SearchCondition leftSearchCondition, SearchCondition rightearchCondition)
        {

            if (leftSearchCondition == null && rightearchCondition == null)
                return null;
            if (leftSearchCondition != null && rightearchCondition == null)
                return leftSearchCondition;

            if (leftSearchCondition == null && rightearchCondition != null)
                return rightearchCondition;

            if (leftSearchCondition.ContainsSearchCondition(rightearchCondition))
                return leftSearchCondition;
            if (rightearchCondition.ContainsSearchCondition(leftSearchCondition))
                return rightearchCondition;
            leftSearchCondition = leftSearchCondition.Clone();
            rightearchCondition = rightearchCondition.Clone();
            List<SearchFactor> searchFactors = new List<SearchFactor>();
            searchFactors.Add(new SearchFactor(leftSearchCondition));
            searchFactors.Add(new SearchFactor(rightearchCondition));
            SearchTerm searchTerm = new SearchTerm(searchFactors);
            var newSearchCondition = new SearchCondition(new List<SearchTerm>() { searchTerm }, leftSearchCondition.ObjectQuery);
            return newSearchCondition;
        }

        /// <MetaDataID>{e65de435-2093-4ae8-ab03-2b72a2f8dc68}</MetaDataID>
        public SearchCondition Clone()
        {
            List<SearchTerm> searchTerms = new List<SearchTerm>();
            foreach (var searchTerm in SearchTerms)
                searchTerms.Add(searchTerm.Clone());
            SearchCondition searchCondition = new SearchCondition(searchTerms, ObjectQuery);
            foreach (var newSearchTerm in searchCondition.SearchTerms)
                newSearchTerm.OwnerSearchCondition = searchCondition;
            return searchCondition;
        }

        /// <MetaDataID>{95c5c2bc-7004-44fd-8383-883d40146d38}</MetaDataID>
        public override bool Equals(object obj)
        {
            if (!(obj is SearchCondition))
                return false;

            var searchCondition = obj as SearchCondition;

            if (_SearchTerms.Count == searchCondition._SearchTerms.Count)
            {
                foreach (var searchTerm in _SearchTerms)
                {
                    bool exist = false;
                    foreach (var inSearchTerm in searchCondition.SearchTerms)
                    {
                        if (inSearchTerm == searchTerm)
                        {
                            exist = true;
                            break;
                        }
                    }
                    if (!exist)
                        return false;
                }
                return true;
            }
            else
                return false;
        }
        /// <MetaDataID>{cbff8828-45f2-46b3-94da-20a510c48ef3}</MetaDataID>
        public static bool operator ==(SearchCondition leftSearchCondition, SearchCondition rightSearchCondition)
        {
            if (!(leftSearchCondition is SearchCondition) && !(rightSearchCondition is SearchCondition))
                return true;

            if (leftSearchCondition is SearchCondition && rightSearchCondition is SearchCondition)
                return leftSearchCondition.Equals(rightSearchCondition);
            else
                return false;


        }

        /// <MetaDataID>{903323f5-af5b-401a-991f-d9440991d864}</MetaDataID>
        public static bool operator !=(SearchCondition leftSearchCondition, SearchCondition rightSearchCondition)
        {
            return !(leftSearchCondition == rightSearchCondition);
        }
        /// <MetaDataID>{4414d8ec-8d6d-472f-9256-2b9e4c1136e8}</MetaDataID>
        public override string ToString()
        {
            string tmp = null;
            int searchTermCount = SearchTerms.Count;
            foreach (SearchTerm searchTerm in SearchTerms)
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
        /// <MetaDataID>{25a5e685-0c98-4bc3-b937-47c79643244b}</MetaDataID>
        public bool HasAndRelation(Criterion criterion, Criterion outGroupCriterion)
        {
            foreach (SearchTerm searchTerm in SearchTerms)
            {
                if (searchTerm.ContainsCriterion(outGroupCriterion) && searchTerm.ContainsCriterion(criterion))
                {
                    foreach (SearchFactor searchFactor in searchTerm.SearchFactors)
                    {
                        if ((searchFactor.Criterions.Contains(outGroupCriterion) && !searchFactor.Criterions.Contains(criterion) ||
                            !searchFactor.Criterions.Contains(outGroupCriterion) && searchFactor.Criterions.Contains(criterion)))
                        {
                            return true;

                        }
                        if (searchFactor.Criterions.Contains(outGroupCriterion) && searchFactor.Criterions.Contains(criterion)
                            && !searchFactor.HasAndRelation(criterion, outGroupCriterion))
                            return false;
                        if (searchFactor.Criterions.Contains(outGroupCriterion) && searchFactor.Criterions.Contains(criterion)
                                && searchFactor.HasAndRelation(criterion, outGroupCriterion))
                            return true;
                    }

                }
                else
                    return false;
            }
            return true;

        }
        /// <MetaDataID>{b3ca0595-b962-4f45-88e0-632bd18459fb}</MetaDataID>
        public bool ActsAsConstrainCriterion(System.Collections.Generic.List<Criterion> criterions)
        {

            foreach (Criterion criterion in criterions)
            {
                foreach (Criterion outGroupCriterion in Criterions)
                {
                    if (criterions.Contains(outGroupCriterion))
                        continue;

                    foreach (SearchTerm searchTerm in SearchTerms)
                    {
                        if (searchTerm.ContainsCriterion(outGroupCriterion) && searchTerm.ContainsCriterion(criterion))
                        {
                            foreach (SearchFactor searchFactor in searchTerm.SearchFactors)
                            {
                                if ((searchFactor.Criterions.Contains(outGroupCriterion) && !searchFactor.Criterions.Contains(criterion) ||
                                    !searchFactor.Criterions.Contains(outGroupCriterion) && searchFactor.Criterions.Contains(criterion)))
                                {
                                    goto NextCriterion;

                                }
                                if (searchFactor.Criterions.Contains(outGroupCriterion) && searchFactor.Criterions.Contains(criterion)
                                    && !searchFactor.HasAndRelation(criterion, outGroupCriterion))
                                    return false;

                            }

                        }
                        else
                            return false;
                    }

                NextCriterion:
                    continue;
                }
            }
            return true;


            bool allConstrainCriterion = true;
            foreach (Criterion criterion in criterions)
            {
                if (!criterion.ConstrainCriterion)
                {
                    allConstrainCriterion = false;
                    break;
                }
            }
            if (allConstrainCriterion)
                return true;

            if (criterions.Count == Criterions.Count)
            {
                foreach (Criterion criterion in criterions)
                {
                    if (!criterions.Contains(criterion))
                        return false;
                }
                return true;
            }
            if (criterions.Count > Criterions.Count)
                return false;
            foreach (SearchTerm searchTerm in SearchTerms)
            {
                foreach (SearchFactor searchFactor in searchTerm.SearchFactors)
                {
                    if (searchFactor.HaveAllCommonFactor(criterions) && SearchTerms.Count == 1)
                        return true;
                }
            }
            return false;
        }

        /// <MetaDataID>{699e8dbe-0003-46f9-9aa1-145496a062dc}</MetaDataID>
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
        /// <MetaDataID>{7a6cdfe6-d99d-4432-b8ce-4d5c08558432}</MetaDataID>
        public System.Collections.Generic.List<Criterion> Criterions
        {
            get
            {
                System.Collections.Generic.List<Criterion> criterions = null; ;
                foreach (SearchTerm searchTerm in SearchTerms)
                {
                    if (criterions == null)
                        criterions = searchTerm.Criterions;
                    else
                        criterions.AddRange(searchTerm.Criterions);
                }
                return criterions;
            }
        }


        /// <MetaDataID>{385C69CA-7964-402B-AE19-9EB40E379405}</MetaDataID>
        public DataNode GetObjectIDDataNodeConstrain(DataNode dataNodeTreeHeader)
        {
            if (_SearchTerms.Count != 1)
                return null;
            else
                return (_SearchTerms[0] as SearchTerm).GetObjectIDDataNodeConstrain(dataNodeTreeHeader);
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{DFF26D6D-6463-4127-9052-B4EEAC99BB29}</MetaDataID>
        protected OOAdvantech.Collections.Generic.List<SearchTerm> _SearchTerms = new OOAdvantech.Collections.Generic.List<SearchTerm>();
        /// <MetaDataID>{C4E01113-3AA4-4AA5-8BD0-F5381A5DC177}</MetaDataID>
        public OOAdvantech.Collections.Generic.List<SearchTerm> SearchTerms
        {
            get
            {
                return _SearchTerms;
            }
        }
        /// <MetaDataID>{2832fc73-769a-472a-964f-ee5b093cf3fc}</MetaDataID>
        [NonSerialized]
        ObjectQuery ObjectQuery;
        /// <MetaDataID>{2A3971EE-F30F-45BC-8EEA-80A7A468C944}</MetaDataID>
        public SearchCondition(System.Collections.Generic.List<SearchTerm> searchTerms, ObjectQuery oqlStatement)
        {
            ObjectQuery = oqlStatement;
            if (searchTerms.Count == 1)
            {
                bool simplify = true;
                System.Collections.Generic.List<SearchFactor> searchFactors = new System.Collections.Generic.List<SearchFactor>();
                if (searchTerms[0].SearchFactors.Count == 0)
                    simplify = false;
                foreach (var searchFactor in searchTerms[0].SearchFactors)
                {
                    if (searchFactor.SearchCondition == null)
                    {
                        simplify = false;
                        break;

                    }
                    if (searchFactor.SearchCondition != null && searchFactor.SearchCondition.SearchTerms.Count > 1)
                    {
                        simplify = false;
                        break;
                    }
                    else if (searchFactor.SearchCondition != null && searchFactor.SearchCondition.SearchTerms.Count == 1)
                    {
                        foreach (var inSearchFactor in searchFactor.SearchCondition.SearchTerms[0].SearchFactors)
                        {
                            if (!searchFactors.Contains(inSearchFactor))
                                searchFactors.Add(inSearchFactor);
                        }
                    }
                }
                if (simplify)
                {
                    //searchFactorForRemove = new System.Collections.Generic.List<SearchFactor>();
                    List<int> doublicateSearchFactors = new List<int>();
                    int i = 0;
                    foreach (var searchFactor in new System.Collections.Generic.List<SearchFactor>(searchFactors))
                    {
                        int k = 0;
                        foreach (var inSearchFactor in new System.Collections.Generic.List<SearchFactor>(searchFactors))
                        {

                            if (inSearchFactor != searchFactor && inSearchFactor.SearchCondition != null &&
                                searchFactor.SearchCondition == inSearchFactor.SearchCondition &&
                                !doublicateSearchFactors.Contains(k) && !doublicateSearchFactors.Contains(i))
                                doublicateSearchFactors.Add(k);
                            else if (inSearchFactor != searchFactor && inSearchFactor.Criterion != null &&
                                searchFactor.Criterion == inSearchFactor.Criterion &&
                                (!doublicateSearchFactors.Contains(k) && !doublicateSearchFactors.Contains(i)))
                                doublicateSearchFactors.Add(k);
                            else if (inSearchFactor == searchFactor && k != i && !doublicateSearchFactors.Contains(k) && !doublicateSearchFactors.Contains(i))
                                doublicateSearchFactors.Add(k);
                            k++;
                        }
                        i++;
                    }

                    foreach (var searchFactorIndex in doublicateSearchFactors)
                        searchFactors.RemoveAt(searchFactorIndex);





                    SearchTerm searchTerm = new SearchTerm(searchFactors);
                    _SearchTerms = new OOAdvantech.Collections.Generic.List<SearchTerm>() { searchTerm };
                }
                else
                    _SearchTerms = new OOAdvantech.Collections.Generic.List<SearchTerm>(searchTerms);

            }
            else
                _SearchTerms = new OOAdvantech.Collections.Generic.List<SearchTerm>(searchTerms);
            // OQLStatement = oqlStatement;
            foreach (var searchTerm in _SearchTerms)
                searchTerm.OwnerSearchCondition = this;






        }
        /// <MetaDataID>{f7f5de81-d28f-48d9-9766-9d6c23281b43}</MetaDataID>
        bool SearchConditionApplied = false;
        /// <MetaDataID>{52e8237c-88ee-4eb2-a6f1-d6e941b8b933}</MetaDataID>
        //[NonSerialized]
        //public ObjectQuery OQLStatement;
        /// <MetaDataID>{6d01c513-879e-4f5d-af59-c6a74fa15da5}</MetaDataID>
        bool ConstrainCondition;
        /// <MetaDataID>{C7EC3A49-9400-443D-928A-232320658F0F}</MetaDataID>
        protected internal SearchCondition(Parser.ParserNode searchConditionParserNode, ObjectsContextQuery oqlStatement, bool constrainCondition)
        {
            ObjectQuery = oqlStatement;

            if (searchConditionParserNode == null || searchConditionParserNode["Search_Term"] == null)
                throw new System.Exception("There is Search Condition without search terms");


            if (_SearchTerms.Count > 1)
                constrainCondition = false;
            ConstrainCondition = constrainCondition;
            foreach (Parser.ParserNode searchTermsParserNode in searchConditionParserNode.ChildNodes)
            {
                if (searchTermsParserNode.Name != "Search_Term")
                    continue;

                var searchTerm = CreateSearchTerm(searchTermsParserNode, oqlStatement, constrainCondition);
                searchTerm.OwnerSearchCondition = this;
                _SearchTerms.Add(searchTerm);

            }
        }
        /// <MetaDataID>{2D78B9F1-8806-44F2-9983-D8F11A05D837}</MetaDataID>
        protected virtual SearchTerm CreateSearchTerm(Parser.ParserNode searchTermsParserNode, ObjectsContextQuery oqlStatement, bool constrainTerm)
        {
            return new SearchTerm(searchTermsParserNode, oqlStatement, constrainTerm);
        }

        #region Filter retrieved data

        #region RowRemove code
        ///// <MetaDataID>{db2ad267-3015-4977-ba98-a580deeaff6f}</MetaDataID>
        //System.Collections.Generic.Dictionary<DataRetrieveNode, int> RowRemoveIndicies = new System.Collections.Generic.Dictionary<DataRetrieveNode, int>();

        #endregion


        /// <summary>
        /// Check the filter condition.
        /// If the filterCondition resolved from native dataBase system returns true otherwise returns false
        /// </summary>
          /// <param name="dataRetrievePath">
        /// Defines the query result data retrieve path
        /// </param>
        /// <returns>
        /// Returns true when filterCondition resolved from native dataBase system otherwise returns false
        /// </returns>
        /// <MetaDataID>{638f4e78-1627-4f9b-bbd9-e475d79800f5}</MetaDataID>
        internal bool FilterConditionApplied(List<DataNode> dataRetrievePathDataNodes)
        {
            int numOfDataNodes = 0;
            bool hasCriterionsToApply = false;
            foreach (Criterion criterion in Criterions)
            {
                if (/*!GlobalCriterions.Contains(criterion) &&*/ !criterion.Applied)
                {
                    hasCriterionsToApply = true;
                    break;
                }
            }
            if (hasCriterionsToApply)
            {
                foreach (var criterion in Criterions)
                    criterion.Applied = false;
                return false;
            }
            foreach (var dataNode in DataNodes)
            {
                DataNode orgDataNode = DerivedDataNode.GetOrgDataNode(dataNode);
                while (orgDataNode.Type != DataNode.DataNodeType.Object && orgDataNode.Type != DataNode.DataNodeType.Group)
                    orgDataNode = orgDataNode.ParentDataNode;

                var exist = (from dataRetrievePathDataNode in dataRetrievePathDataNodes
                             where dataRetrievePathDataNode == orgDataNode
                             select dataRetrievePathDataNode).Count() != 0;
                if (exist)
                    numOfDataNodes++;
            }
            if (numOfDataNodes > 1)
            {
                foreach (var criterion in Criterions)
                    criterion.Applied = false;

                return false;
            }
            return true;
        }

        ///<summary>
        /// Build the data retrieve path for data node and subdatanodes to retrieve data and search condition apply criterions.
        ///</summary>
        ///<param name="dataNode">
        /// This parameter defines the next data node of data retrieve path.
        ///</param>
        ///<param name="dataRetrieveDataPath">
        ///This parameter defines the built data retrieve path.
        ///</param>
        ///<param name="dataNodeRowIndices">
        ///This parameter defines a dictionary with data nodes indices on composite row.
        ///</param>
        ///<param name="unAssignedNodes">
        ///This parameter defines a collection with data nodes, t
        ///hat not yet assigned and will be assigned in data retrieve path tail.  
        ///</param>
        /// <MetaDataID>{07d31d7e-7c87-4c65-8187-b0679e7885ef}</MetaDataID>
        internal void BuildRetrieveDataPath(DataRetrieveNode dataRetrieveNode, System.Collections.Generic.Dictionary<DataNode, int> dataNodeRowIndices, System.Collections.Generic.LinkedList<DataRetrieveNode> dataRetrieveDataPath, System.Collections.Generic.List<DataRetrieveNode> unAssignedNodes, bool ignoreCriterionSate)
        {
            System.Collections.Generic.List<DataRetrieveNode> subNodes = new System.Collections.Generic.List<DataRetrieveNode>();
            if ((DerivedDataNode.GetOrgDataNode(dataRetrieveNode.DataNode).Type == DataNode.DataNodeType.Object || DerivedDataNode.GetOrgDataNode(dataRetrieveNode.DataNode).Type == DataNode.DataNodeType.Group) && DataNodeParticipateAsBranch(dataRetrieveNode.DataNode))
            {
                #region Checks for branch search criterions where doesn't applied from data base manage mechanism. If there aren't returns
                bool allAreLocalCriterion = true;
                bool hasCriterionsToApply = false;

                foreach (Criterion criterion in dataRetrieveNode.DataNode.BranchSearchCriterions)
                {

                    Synchronization(criterion);
                    if (ContainsCriterion(criterion) &&
                        !GlobalCriterionsContains(criterion) &&
                        !criterion.Applied)
                    {
                        hasCriterionsToApply = true;
                        break;
                    }
                }
                if (!ignoreCriterionSate)
                {
                    if (!hasCriterionsToApply && unAssignedNodes.Count == 0)
                        return;
                }
                #endregion

                //dataNode.FilteredDataRowIndex = dataRetrieveDataPath.Count;

                if (DerivedDataNode.GetOrgDataNode(dataRetrieveNode.DataNode).DataSource != null && DerivedDataNode.GetOrgDataNode(dataRetrieveNode.DataNode).DataSource.DataTable != null)
                {
                    DerivedDataNode.GetOrgDataNode(dataRetrieveNode.DataNode).DataSource.DataTable.FilteredTable = true;
                    var exist = (from pathDataRetrieveNode in dataRetrieveDataPath.ToArray()
                                 where pathDataRetrieveNode.DataNode == dataRetrieveNode.DataNode
                                 select pathDataRetrieveNode).Count() != 0;


                    if (!exist)
                    {
                        dataNodeRowIndices[dataRetrieveNode.DataNode] = dataRetrieveDataPath.Count;
                        dataRetrieveNode.OnlyForDataFilter = true;
                        dataRetrieveDataPath.AddLast(dataRetrieveNode);//new DataRetrieveNode(dataRetrieveNode, DataRetrieveNode.GetDataRetrieveNode(dataRetrieveNode.RealParentDataNode, dataRetrieveDataPath), dataNodeRowIndices));
                    }
                }
                #region RowRemove code
                //if (dataRetrieveNode.DataNode.DataSource != null)
                //    RowRemoveIndicies[dataRetrieveDataPath.Last.Value] = dataRetrieveNode.DataNode.DataSource.GetRowRemoveIndex(this);
                //else
                //    RowRemoveIndicies[dataRetrieveDataPath.Last.Value] = -1;
                #endregion

                #region retrieves all sub data nodes which participate in search condition as branch

                foreach (DataNode subDataNode in dataRetrieveNode.DataNode.SubDataNodes)
                {
                    if (DataNodeParticipateAsBranch(subDataNode) &&
                        (DerivedDataNode.GetOrgDataNode(subDataNode).Type == DataNode.DataNodeType.Object || DerivedDataNode.GetOrgDataNode(subDataNode).Type == DataNode.DataNodeType.Group))
                        subNodes.Add(new DataRetrieveNode(subDataNode, DataRetrieveNode.GetDataRetrieveNode(dataRetrieveNode.DataNode, dataRetrieveDataPath), dataNodeRowIndices, dataRetrieveDataPath));

                    if (DerivedDataNode.GetOrgDataNode(subDataNode).Type == DataNode.DataNodeType.Key)
                    {
                        foreach (DataNode groupKeyDataNode in subDataNode.SubDataNodes)
                        {
                            if (DataNodeParticipateAsBranch(groupKeyDataNode))
                                subNodes.Add(new DataRetrieveNode(groupKeyDataNode, DataRetrieveNode.GetDataRetrieveNode(groupKeyDataNode.RealParentDataNode, dataRetrieveDataPath), dataNodeRowIndices, dataRetrieveDataPath));
                        }
                    }
                }
                #endregion

                if (subNodes.Count > 0)
                {
                    #region Continues recursively with first subdatanodes the others are market as unassinged and will be added at the end.
                    DataRetrieveNode subDataNode = subNodes[0] as DataRetrieveNode;
                    subNodes.RemoveAt(0);
                    if (subNodes.Count > 0)
                        unAssignedNodes.AddRange(subNodes);
                    //CreateFilterPath(subDataNode, dataRetrieveDataPath, unAssignedNodes);
                    BuildRetrieveDataPath(subDataNode, dataNodeRowIndices, dataRetrieveDataPath, unAssignedNodes, ignoreCriterionSate);
                    #endregion
                }
                else if (unAssignedNodes.Count > 0)
                {
                    #region Continues recursively with first unassigned datanode the others will be added at the end.
                    DataRetrieveNode subDataNode = unAssignedNodes[0] as DataRetrieveNode;
                    unAssignedNodes.RemoveAt(0);
                    //CreateFilterPath(subDataNode, dataRetrieveDataPath, unAssignedNodes);
                    BuildRetrieveDataPath(subDataNode, dataNodeRowIndices, dataRetrieveDataPath, unAssignedNodes, ignoreCriterionSate);
                    #endregion
                }
            }
            else if (unAssignedNodes.Count > 0)
            {
                #region Continues recursively with first unassigned datanode the others will be added at the end.
                DataRetrieveNode subDataNode = unAssignedNodes[0] as DataRetrieveNode;
                unAssignedNodes.RemoveAt(0);
                //CreateFilterPath(subDataNode, dataRetrieveDataPath, unAssignedNodes);
                BuildRetrieveDataPath(subDataNode, dataNodeRowIndices, dataRetrieveDataPath, unAssignedNodes, ignoreCriterionSate);
                #endregion
            }
        }

        /// <MetaDataID>{a81fcde6-9264-4285-9d4b-dd05d7f7f8d2}</MetaDataID>
        private void Synchronization(Criterion criterion)
        {
            foreach (var synCriterion in Criterions)
            {
                if (synCriterion == criterion)
                {
                    synCriterion.Applied = criterion.Applied;
                    return;
                }
            }
            
        }

        /// <MetaDataID>{6172152e-9355-410b-9f59-2d02eb914fed}</MetaDataID>
        private bool GlobalCriterionsContains(Criterion criterion)
        {
            //foreach (var globalCriterion in GlobalCriterions)
            //{
            //    if (globalCriterion == criterion)
            //        return true;
            //}
            return false;
            
        }

        /// <MetaDataID>{17d152dd-e26e-48ad-ab53-86f00f0fe38b}</MetaDataID>
        private bool DataNodeParticipateAsBranch(DataNode dataNode)
        {

            foreach (DataNode searchConditionDataNode in DataNodes)
            {
                if (searchConditionDataNode.IsSameOrParentDataNode(dataNode))
                    return true;
            }
            return false;

        }

        /// <MetaDataID>{05AA01B0-0D29-49DA-B46B-D14978C9AFE2}</MetaDataID>
        //private void CreateFilterPath(DataNode dataNode, System.Collections.Generic.LinkedList<DataNode> filterDataPath, System.Collections.Generic.List<DataNode> unAssignedNodes)
        //{
        //    if (dataNode.Type == DataNode.DataNodeType.Namespace)
        //    {
        //        CreateFilterPath(dataNode.SubDataNodes[0], filterDataPath, unAssignedNodes);
        //        return;
        //    }

        //    System.Collections.Generic.List<DataNode> subNodes = new System.Collections.Generic.List<DataNode>();
        //    if ((dataNode.Type == DataNode.DataNodeType.Object || dataNode.Type == DataNode.DataNodeType.Group) && dataNode.BranchParticipateInWereClause)
        //    {
        //        #region Checks for branch search criterions where doesn't aplied from data base manage mechanism. If there aren't returns
        //        bool allAreLocalCriterion = true;
        //        foreach (Criterion criterion in dataNode.BranchSearchCriteria)
        //        {

        //            if (Criterions.Contains(criterion) && !criterion.Applied)
        //            {
        //                allAreLocalCriterion = false;
        //                break;
        //            }
        //        }

        //        if (allAreLocalCriterion && unAssignedNodes.Count == 0)
        //            return;
        //        #endregion

        //        dataNode.FilteredDataRowIndex = filterDataPath.Count;
        //        if (dataNode.DataSource != null)
        //            (dataNode.DataSource.DataTable as DataLoader.DataTable).FilteredTable = true;
        //        filterDataPath.AddLast(dataNode);

        //        #region retrieves all sub data nodes which participate in search condition as branch

        //        foreach (DataNode subDataNode in dataNode.SubDataNodes)
        //        {
        //            if (subDataNode.BranchParticipateInWereClause && subDataNode.Type == DataNode.DataNodeType.Object)
        //                subNodes.Add(subDataNode);
        //        }
        //        #endregion

        //        if (subNodes.Count > 0)
        //        {
        //            #region Continues recursively with first subdatanodes the others are market as unassinged and will be added at the end.
        //            DataNode subDataNode = subNodes[0] as DataNode;
        //            subNodes.RemoveAt(0);
        //            if (subNodes.Count > 0)
        //                unAssignedNodes.AddRange(subNodes);
        //            CreateFilterPath(subDataNode, filterDataPath, unAssignedNodes);
        //            #endregion
        //        }
        //        else if (unAssignedNodes.Count > 0)
        //        {
        //            #region Continues recursively with first unassigned datanode the others will be added at the end.
        //            DataNode subDataNode = unAssignedNodes[0] as DataNode;
        //            unAssignedNodes.RemoveAt(0);
        //            CreateFilterPath(subDataNode, filterDataPath, unAssignedNodes);
        //            #endregion
        //        }
        //    }
        //}
        #region  RowRemove code
        ///// <MetaDataID>{19d06ab6-aeaa-4349-8860-8968237c15e8}</MetaDataID>
        //[NonSerialized]
        //internal System.Collections.Generic.Dictionary<System.Data.DataRow, byte> RemovedRows = new System.Collections.Generic.Dictionary<System.Data.DataRow, byte>();


        
        ///// <MetaDataID>{cde305e9-7235-42b5-8814-92e6e8bf9019}</MetaDataID>
        //internal void CancelRemoveRow(System.Data.DataRow row, int rowRemoveIndex)
        //{
        //    if (RemovedRows == null)
        //        RemovedRows = new System.Collections.Generic.Dictionary<System.Data.DataRow, byte>();
        //    if (rowRemoveIndex == -1)
        //        RemovedRows.Remove(row);
        //    else
        //        row[rowRemoveIndex] = false;

        //}
        

        ///// <MetaDataID>{e9df77fa-5b42-47ea-9bee-9df25bf963cd}</MetaDataID>
        //internal void RemoveRow(System.Data.DataRow row, int rowRemoveIndex)
        //{

        //    if (RemovedRows == null)
        //        RemovedRows = new System.Collections.Generic.Dictionary<System.Data.DataRow, byte>();
        //    if (rowRemoveIndex == -1)
        //        RemovedRows[row] = 0;
        //    else
        //        row[rowRemoveIndex] = true;

        //}
        #endregion



        ///// <MetaDataID>{04543e86-f650-4ba4-867b-fd5513394a4b}</MetaDataID>
        //public bool IsRemovedRow(System.Data.DataRow row, int rowRemoveIndex)
        //{
        //    bool result = false;
        //    if (RemovedRows != null)

        //        result = RemovedRows.ContainsKey(row);

        //    if (!result && rowRemoveIndex != -1)
        //    {
        //        object value = row[rowRemoveIndex];
        //        if (value is bool)
        //            result = (bool)value;
        //    }

        //    return result;
        //}

        #region RowRemove code
        ///// <MetaDataID>{16b84089-3ace-4d62-9ffc-d2af73e00529}</MetaDataID>
        //System.Collections.Generic.Dictionary<DataSource, int> RowRemoveIndices = new System.Collections.Generic.Dictionary<DataSource, int>();
       


        ///// <MetaDataID>{7846fb4a-476c-49fc-9adc-748c3f941bc6}</MetaDataID>
        //public bool IsRemovedRow(System.Data.DataRow row, DataSource ownerDataSource)
        //{
        //    int rowRemoveIndex = -1;
            
        //    #region RowRemove code
        //    //if (!RowRemoveIndices.TryGetValue(ownerDataSource, out rowRemoveIndex))
        //    //{
        //    //    rowRemoveIndex = ownerDataSource.GetRowRemoveIndex(this);
        //    //    RowRemoveIndices[ownerDataSource] = rowRemoveIndex;
        //    //}
        //    #endregion

        //    bool result = false;
        //    if (RemovedRows != null && rowRemoveIndex == -1)
        //        result = RemovedRows.ContainsKey(row);
        //    if (!result && rowRemoveIndex != -1)
        //    {
        //        object value = row[rowRemoveIndex];
        //        if (value is bool)
        //            result = (bool)value;
        //    }

        //    return result;
        //}
        #endregion

        /// <MetaDataID>{b9513017-2762-4941-b9ce-2727e7fabd0a}</MetaDataID>
        System.Collections.Generic.LinkedList<DataRetrieveNode> RetrieveDataPath = new System.Collections.Generic.LinkedList<DataRetrieveNode>();

       // /// <MetaDataID>{2e1885dc-a2b4-413a-886c-dee6db6d2a41}</MetaDataID>
        //internal System.Collections.Generic.List<Criterion> GlobalCriterions = new System.Collections.Generic.List<Criterion>();

        #region  RowRemove code
        ///// <MetaDataID>{9a00d516-e281-41b2-85fd-2600f97e307e}</MetaDataID>
        //internal bool CanBeApplied(Criterion criterion)
        //{
        //    DataNode termDataNode = null;
        //    if (criterion.LeftTermDataNode != null)
        //        termDataNode = criterion.LeftTermDataNode;
        //    if (criterion.RightTermDataNode != null)
        //        termDataNode = criterion.RightTermDataNode;
        //    if (termDataNode != null && termDataNode.ObjectQuery is DistributedObjectQuery)
        //    {
        //        return true;
        //        if (!GlobalCriterions.Contains(criterion))
        //            return true;
        //        else
        //            return false;
        //    }
        //    else
        //        return true;

        //}
        
        /////<summary>
        ///// This method filter object query data. 
        ///// </summary>
        ///// <MetaDataID>{3eb9311c-3bb5-4011-80ea-eb72653f133b}</MetaDataID>
        //internal void FilterData(DataNode rootDataNode)
        //{
        //    if(rootDataNode.ObjectQuery.QueryResultType!=null)
        //        return;
        //    if (SearchConditionApplied)
        //        return;

        //    throw new NotImplementedException();
            
        //    //bool searchConditionApplied = true;
        //    //foreach (Criterion criterion in Criterions)
        //    //{
        //    //    if (!criterion.Applied)
        //    //    {
        //    //        searchConditionApplied = false;
        //    //        break;
        //    //    }
        //    //}
        //    //if (searchConditionApplied)
        //    //    return;

        //    //foreach (Criterion criterion in Criterions)
        //    //    if (criterion.SearhConditionHeader != this)
        //    //    {

        //    //    }

        //    //if (rootDataNode.ObjectQuery is DistributedObjectQuery)
        //    //{
        //    //    string storageIdentity = ((rootDataNode.ObjectQuery as DistributedObjectQuery).ObjectsContext as PersistenceLayer.ObjectStorage).StorageMetaData.StorageIdentity;
        //    //    if (rootDataNode.DataSource.HasInObjectContextData)
        //    //    {
        //    //        StorageDataLoader rootDataLoder = rootDataNode.DataSource.DataLoaders[storageIdentity] as StorageDataLoader;
        //    //        if (rootDataLoder.RetrievesData)
        //    //        {

        //    //            foreach (var criterion in Criterions)
        //    //            {
        //    //                bool local = true;
        //    //                if (criterion.LeftTermDataNode != null)
        //    //                {
        //    //                    DataNode leftDataNode = DerivedDataNode.GetOrgDataNode(criterion.LeftTermDataNode);
        //    //                    while (leftDataNode.Type == DataNode.DataNodeType.OjectAttribute)
        //    //                        leftDataNode = leftDataNode.ParentDataNode;

        //    //                    if (leftDataNode.DataSource != null &&
        //    //                        leftDataNode.DataSource.HasInObjectContextData)
        //    //                    {
        //    //                        if (!(leftDataNode.DataSource.DataLoaders[storageIdentity] as StorageDataLoader).ExistOnlyLocalRoute(rootDataNode))
        //    //                            local = false;
        //    //                    }
        //    //                    else
        //    //                        local = false;

        //    //                }
        //    //                if (criterion.RightTermDataNode != null)
        //    //                {

        //    //                    DataNode rightDataNode = DerivedDataNode.GetOrgDataNode(criterion.RightTermDataNode);
        //    //                    while (rightDataNode.Type == DataNode.DataNodeType.OjectAttribute)
        //    //                        rightDataNode = rightDataNode.ParentDataNode;

        //    //                    if (rightDataNode.DataSource != null &&
        //    //                      rightDataNode.DataSource.HasInObjectContextData)
        //    //                    {
        //    //                        if (!(criterion.RightTermDataNode.DataSource.DataLoaders[storageIdentity] as StorageDataLoader).ExistOnlyLocalRoute(rootDataNode))
        //    //                            local = false;
        //    //                    }
        //    //                    else
        //    //                        local = false;
        //    //                }
        //    //                if (!local)
        //    //                    GlobalCriterions.Add(criterion);
        //    //            }

        //    //        ReCalculateGlobalCriterions:
        //    //            foreach (Criterion globalCriterion in GlobalCriterions)
        //    //            {
        //    //                foreach (var criterion in Criterions)
        //    //                {
        //    //                    if (!GlobalCriterions.Contains(criterion))
        //    //                    {
        //    //                        if (!HasAndRelation(criterion, globalCriterion))
        //    //                        {
        //    //                            GlobalCriterions.Add(criterion);
        //    //                            goto ReCalculateGlobalCriterions;
        //    //                        }
        //    //                    }
        //    //                }
        //    //            }
        //    //            if (GlobalCriterions.Count == Criterions.Count)
        //    //                return;
        //    //        }
        //    //        else
        //    //            return;
        //    //    }
        //    //    else
        //    //        return;
        //    //}



        //    //System.DateTime start = System.DateTime.Now;
        //    //System.Collections.Generic.List<DataRetrieveNode> unAssignedNodes = new System.Collections.Generic.List<DataRetrieveNode>();

        //    //System.Collections.Generic.Dictionary<DataNode, int> dataNodeRowIndices = new System.Collections.Generic.Dictionary<DataNode, int>();
        //    //BuildRetrieveDataPath(new DataRetrieveNode(rootDataNode, null, dataNodeRowIndices, RetrieveDataPath), dataNodeRowIndices, RetrieveDataPath, unAssignedNodes, false);
        //    //if (RetrieveDataPath.Count == 0)
        //    //    return;
        //    //System.Data.DataRow[] composedRow = new System.Data.DataRow[RetrieveDataPath.Count];
        //    //if (RetrieveDataPath.First.Value.DataNode.DataSource.DataLoaders.Count > 0)
        //    //{
        //    //    foreach (System.Data.DataRow row in RetrieveDataPath.First.Value.DataNode.DataSource.DataTable.Rows)
        //    //    {
        //    //        composedRow[RetrieveDataPath.First.Value.DataRowIndex] = row;
        //    //        //RemoveRow(row, RowRemoveIndicies[RetrieveDataPath.First.Value]); #RowRemove code
        //    //        RemoveRow(row, -1);
        //    //        FilterData(composedRow, RetrieveDataPath.First.Next);
        //    //        for (int i = 0; i < RetrieveDataPath.Count; i++)
        //    //            composedRow[i] = null;

        //    //    }
        //    //}
        //    //foreach (var criterion in Criterions)
        //    //{
        //    //    if (!GlobalCriterions.Contains(criterion))
        //    //        criterion.Applied = true;
        //    //}
        //    //SearchConditionApplied = true;
            
        //}
        #endregion



        #region  RowRemove code
        /////<summary>
        ///// The FilterData is recursive call method, executed recursively thought the end of path.
        ///// Produce a virtual datarow with all current data rows of datatree data nodes
        /////</summary>
        ///// <MetaDataID>{4e683488-b269-462a-9417-62b3e49351b5}</MetaDataID>
        //private void FilterData(System.Data.DataRow[] composedRow, System.Collections.Generic.LinkedListNode<DataRetrieveNode> linkedListNode)
        //{
        //    try
        //    {
        //        if (linkedListNode == null)
        //        {
        //            //System recursively has reached to the end of path and the virtual data row has been created.
        //            //Now system perform the search condition to virtual data row
        //            //if pass the criterions all data rows of virtual data row marked as filter pass.
        //            if (DoesRowPassCondition(composedRow, RetrieveDataPath.First.Value.DataNodeRowIndices))
        //            {
        //                foreach (var dataRetrieveNode in RetrieveDataPath)
        //                //foreach (System.Data.DataRow row in composedRow)
        //                {
        //                    System.Data.DataRow row = composedRow[dataRetrieveNode.DataRowIndex];
        //                    if (row != null)
        //                        CancelRemoveRow(row, -1);
        //                        //CancelRemoveRow(row, RowRemoveIndicies[dataRetrieveNode]);#region RowRemove code
                                
        //                }
        //            }
        //        }
        //        else
        //        {
        //            System.Collections.Generic.ICollection<System.Data.DataRow> rows = null;
        //            if (composedRow[linkedListNode.Value.MasterDataNode.DataRowIndex] != null)
        //                rows = linkedListNode.Value.MasterDataNode.DataNode.DataSource.GetRelatedRows(composedRow[linkedListNode.Value.MasterDataNode.DataRowIndex], linkedListNode.Value.DataNode);
        //            if (rows == null || rows.Count == 0)
        //            {
        //                //TODO να γραφτεί test σενάριο για αυτήν την περίπτωση.
        //                composedRow[linkedListNode.Value.DataRowIndex] = null;
        //                FilterData(composedRow, linkedListNode.Next);
        //                return;
        //            }
        //            else
        //            {
        //                foreach (System.Data.DataRow row in rows)
        //                {
        //                    composedRow[linkedListNode.Value.DataRowIndex] = row;

        //                    // RemoveRow(row, RowRemoveIndicies[linkedListNode.Value]); #region RowRemove code
        //                    RemoveRow(row, -1);
        //                    FilterData(composedRow, linkedListNode.Next);
        //                }
        //            }
        //        }
        //    }
        //    catch (System.Exception error)
        //    {
        //        throw error;
        //    }


        //}

#endregion

        ///<summary>
        /// The FilterData is recursive call method, executed recursively thought the end of path.
        /// Produce a virtual datarow with all current data rows of datatree data nodes
        ///</summary>
        /// <MetaDataID>{4e683488-b269-462a-9417-62b3e49351b5}</MetaDataID>
        //private void FilterData(System.Data.DataRow[] composedRow, System.Collections.Generic.LinkedListNode<DataNode> linkedListNode)
        //{
        //    try
        //    {
        //        if (linkedListNode == null)
        //        {
        //            //System recursively has reached to the end of path and the virtual data row has been created.
        //            //Now system perform the search condition to virtual data row
        //            //if pass the criterions all data rows of virtual data row marked as filter pass.
        //            if (DoesRowPassCondition(composedRow))
        //            {
        //                foreach (System.Data.DataRow row in composedRow)
        //                {
        //                    if (row != null)
        //                        CancelRemoveRow(row);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            System.Collections.Generic.ICollection<System.Data.DataRow> rows = linkedListNode.Value.RealParentDataNode.DataSource.GetRelatedRows(composedRow[linkedListNode.Value.RealParentDataNode.FilteredDataRowIndex], linkedListNode.Value);
        //            if (rows == null || rows.Count == 0)
        //            {
        //                //TODO να γραφτεί test σενάριο για αυτήν την περίπτωση.
        //                composedRow[linkedListNode.Value.FilteredDataRowIndex] = null;
        //                FilterData(composedRow, linkedListNode.Next);
        //                return;
        //            }
        //            else
        //            {
        //                foreach (System.Data.DataRow row in rows)
        //                {
        //                    composedRow[linkedListNode.Value.FilteredDataRowIndex] = row;
        //                    RemoveRow(row);
        //                    FilterData(composedRow, linkedListNode.Next);
        //                }
        //            }
        //        }
        //    }
        //    catch (System.Exception error)
        //    {
        //        throw error;
        //    }


        //}

        /// <MetaDataID>{c3eb2589-8770-48d7-a826-9887d89542fc}</MetaDataID>
        //internal bool DoesRowPassCondition(System.Data.DataRow row, DataNode ownerDataNode)
        //{
        //    foreach (SearchTerm searchTerm in SearchTerms)
        //    {
        //        if (searchTerm.DoesRowPassCondition(row, ownerDataNode))
        //            return true;
        //    }
        //    return false;

        //}
        ///<summary>
        ///This method checks if the virtual data row qualifies the search condition.  
        ///</summary>
        /// <param name="composedRow">
        /// Defines the virtual data row.
        /// </param>
        /// <MetaDataID>{5b0cb048-5efc-4b65-95b5-f8ca5aeb57eb}</MetaDataID>
        internal bool DoesRowPassCondition(IDataRow[] composedRow, System.Collections.Generic.Dictionary<DataNode, int> dataNodeRowIndices)
        {
            foreach (SearchTerm searchTerm in SearchTerms)
            {
                if (searchTerm.DoesRowPassCondition(composedRow, dataNodeRowIndices))
                    return true;
            }
            return false;
        }

        #endregion

        


        /// <MetaDataID>{b88d7e1e-5384-4090-890a-6288af4dd811}</MetaDataID>
        internal bool ContainsSearchCondition(SearchCondition searchCondition)
        {
            if (SearchTerms.Count == 1)
            {
                if (this == searchCondition)
                    return true;

                return SearchTerms[0].ContainsSearchCondition(searchCondition);
            }
            return false;
        }
        /// <MetaDataID>{a5360674-8918-4d44-9bf3-d7f3465f764f}</MetaDataID>
        internal bool ContainsCriterion(Criterion criterion)
        {
            foreach (var searchTerm in SearchTerms)
            {
                if (searchTerm.ContainsCriterion(criterion))
                    return true;
            }
            return false;
        }

        /// <MetaDataID>{829b0c88-1119-4b4b-9868-0cbb1d1b4d8f}</MetaDataID>
        internal SearchFactor OwnerSearchFactor;

        /// <MetaDataID>{09c2fc3c-3c05-4db4-8282-5e912fa363b3}</MetaDataID>
        public SearchCondition SearhConditionHeader
        {
            get
            {
                if (OwnerSearchFactor != null)
                    return OwnerSearchFactor.OwnerSearchTerm.OwnerSearchCondition.SearhConditionHeader;
                else
                    return this;
            }
        }
        /// <MetaDataID>{6ac35b85-4664-46fa-8549-82e5476c970a}</MetaDataID>
        SearchCondition()
        {

        }
        /// <MetaDataID>{dc9d961b-058a-469a-b6ec-b12d846bb09a}</MetaDataID>
        internal SearchCondition Clone(Dictionary<object, object> clonedObjects)
        {
            if (clonedObjects.ContainsKey(this))
                return clonedObjects[this] as SearchCondition;
            SearchCondition newSearchCondition = new SearchCondition();
            clonedObjects[this] = newSearchCondition;
            newSearchCondition.ConstrainCondition = ConstrainCondition;
            if (OwnerSearchFactor != null)
            {
                object ownerSearchFactor = null;
                if (clonedObjects.TryGetValue(OwnerSearchFactor, out ownerSearchFactor))
                    newSearchCondition.OwnerSearchFactor = ownerSearchFactor as SearchFactor;
                else
                    newSearchCondition.OwnerSearchFactor = OwnerSearchFactor.Clone(clonedObjects);
            }
            newSearchCondition.SearchConditionApplied = SearchConditionApplied;

            foreach (var searchTerm in _SearchTerms)
            {
                object newSearchTerm = null;
                if (clonedObjects.TryGetValue(searchTerm, out newSearchTerm))
                    newSearchCondition._SearchTerms.Add(newSearchTerm as SearchTerm);
                else
                    newSearchCondition._SearchTerms.Add(searchTerm.Clone(clonedObjects));


                //newSearchCondition._SearchTerms.Add(
            }
            return newSearchCondition;
        }
    }
}
