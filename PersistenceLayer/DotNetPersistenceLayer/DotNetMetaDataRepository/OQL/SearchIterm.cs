using System;
using System.Collections.Generic;
namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    /// <MetaDataID>{7FE09E4E-0EF1-4403-B68F-798EFA5F7CDF}</MetaDataID>
    /// <summary>Define a condition with result true or false. Contain one or more conditions the search factors. The search term can be true only if all factor conditions are true. </summary>
    [Serializable]
    public class SearchTerm
	{


        public static bool operator ==(SearchTerm leftSearchTerm, SearchTerm rightSearchTerm)
        {
            if (!(leftSearchTerm is SearchTerm) && !(rightSearchTerm is SearchTerm))
                return true;

            if (leftSearchTerm is SearchTerm && rightSearchTerm is SearchTerm)
            {
                return leftSearchTerm.Equals(rightSearchTerm);

            }
            else
                return false;
          
        }
        public override bool Equals(object obj)
        {
            if (!(obj is SearchTerm))
                return false;

            var searchTerm = obj as SearchTerm;

            if (_SearchFactors.Count == searchTerm._SearchFactors.Count)
            {
                foreach (var searchFactor in _SearchFactors)
                {
                    bool exist = false;
                    foreach (var inSearchFactor in searchTerm.SearchFactors)
                    {
                        if (inSearchFactor == searchFactor)
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

        public static bool operator !=(SearchTerm leftSearchTerm, SearchTerm rightSearchTerm)
        {
            return !(leftSearchTerm == rightSearchTerm);
        }
        /// <MetaDataID>{1c1720b1-63da-4347-b5cd-b2e3ac4169e8}</MetaDataID>
        public bool IsNotExpression = false;
        /// <MetaDataID>{982d7221-7621-42cb-960c-d489658188cc}</MetaDataID>
        public override string ToString()
        {
            string tmp = null;
            int searchFactorCount = SearchFactors.Count;
            foreach (SearchFactor searchTerm in SearchFactors)
            {
                if (tmp == null)
                {
                    if (searchFactorCount > 1)
                        tmp = "(";
                }
                else
                    tmp += " AND ";

                tmp += searchTerm.ToString();
            }
            if (searchFactorCount > 1)
                tmp += ")";
            if (tmp == null)
                tmp = "";
            return tmp;

        }
        /// <MetaDataID>{DFD5CD01-E5A1-4863-B1C6-BDFF9AE18364}</MetaDataID>
        public DataNode GetObjectIDDataNodeConstrain(DataNode dataNodeTreeHeader)
        {
            foreach (SearchFactor searchFacor in _SearchFactors)
            {
                DataNode dataNode = searchFacor.GetObjectIDDataNodeConstrain(dataNodeTreeHeader);
                if (dataNode != null)
                    return dataNode;
            }
            return null;
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{35A182C9-13D6-4D7E-9A94-6A3FC2C2FF73}</MetaDataID>
        protected System.Collections.Generic.List<SearchFactor> _SearchFactors=new System.Collections.Generic.List<SearchFactor>();
        /// <MetaDataID>{DE69BD30-D693-45DE-929B-BD6785EFCC0A}</MetaDataID>
        public System.Collections.Generic.List<SearchFactor> SearchFactors
        {
            get
            {
                return _SearchFactors;
            }
        }
        /// <MetaDataID>{13104209-d1e4-4bf8-9db3-221c09ce028f}</MetaDataID>
        public SearchTerm()
        {

        }
        /// <MetaDataID>{DF302611-8FD1-413F-B4FE-B304483B32BC}</MetaDataID>
        public SearchTerm(System.Collections.Generic.List<SearchFactor> searchFactors)
        {
            _SearchFactors = new System.Collections.Generic.List<SearchFactor>(searchFactors);

            foreach (var searchFactor in _SearchFactors)
                searchFactor.OwnerSearchTerm = this;
        }
        /// <MetaDataID>{ae91fcd0-52ca-4dc6-909a-a56ea0c39da5}</MetaDataID>
        bool ConstrainTerm;

        /// <MetaDataID>{61A63579-FA08-4246-BDA7-90F9305C2D47}</MetaDataID>
        protected internal SearchTerm(Parser.ParserNode searchTermParserNode, ObjectsContextQuery oqlStatement, bool constrainTerm)
		{
            ConstrainTerm=constrainTerm;

			 if(searchTermParserNode==null||searchTermParserNode["Search_Factor"]==null)
				 throw new System.Exception("There is Search term without search factors");
			
			foreach(Parser.ParserNode searchFactorParserNode in searchTermParserNode.ChildNodes)
			{
				if(searchFactorParserNode.Name!="Search_Factor")
					continue;
                var searchFactor = CreateSearchFactor(searchFactorParserNode, oqlStatement, constrainTerm);
                _SearchFactors.Add(searchFactor);
                searchFactor.OwnerSearchTerm = this;
			
			}
		}
        /// <MetaDataID>{B32B03EC-8B41-4010-A698-FE2E39D5C1E4}</MetaDataID>
        protected virtual SearchFactor CreateSearchFactor(Parser.ParserNode searchFactorParserNode, ObjectsContextQuery oqlStatement, bool constrainTerm)
		{
			return new SearchFactor(searchFactorParserNode,oqlStatement,constrainTerm);
		}


        /// <MetaDataID>{8d37a1ab-8621-4102-9bac-ad61354704d3}</MetaDataID>
        public System.Collections.Generic.List<Criterion> Criterions
        {
            get
            {
                System.Collections.Generic.List<Criterion> criterions = null; ;
                foreach (SearchFactor searchFactor in SearchFactors)
                {
                    if (criterions == null)
                        criterions = searchFactor.Criterions;
                    else
                        criterions.AddRange(searchFactor.Criterions);
                }
                return criterions;

            }

        }

        #region Filter retrieved data
        ///// <MetaDataID>{a6694225-ba70-421f-b0d9-50e077837a3e}</MetaDataID>
        //internal bool DoesRowPassCondition(System.Data.DataRow row, DataNode ownerDataNode)
        //{
        //    foreach (SearchFactor searchFactor in SearchFactors)
        //    {
        //        if (!searchFactor.DoesRowPassCondition(row, ownerDataNode))
        //            return false;
        //    }
        //    return true;

            
        //}


        ///<summary>
        ///This method checks if the virtual data row qualifies the search term.  
        ///</summary>
        /// <param name="composedRow">
        /// Defines the virtual data row.
        /// </param>
        /// <MetaDataID>{4897e43e-52e5-4fa0-a3a3-bf3d935abe88}</MetaDataID>
        internal bool DoesRowPassCondition(IDataRow[] composedRow, System.Collections.Generic.Dictionary<DataNode, int> dataNodeRowIndices)
        {
            bool result;
            foreach (SearchFactor searchFactor in SearchFactors)
            {
                if (!searchFactor.DoesRowPassCondition(composedRow, dataNodeRowIndices))
                {
                    result = false;
                    if (IsNotExpression)
                        result = !result;
                    return result;
                }
            }
            result= true;
            if (IsNotExpression)
                result = !result;
            return result;


        }
        #endregion

        /// <MetaDataID>{066253c0-0d69-4e43-a603-b9f4e622caa3}</MetaDataID>
        public void AddSearchFactor(SearchFactor searchFactor)
        {



            _SearchFactors.Add(searchFactor);
            searchFactor.OwnerSearchTerm = this;
        }
        /// <MetaDataID>{85cc3b3f-3975-414b-a187-1b0f721d0c8c}</MetaDataID>
        public void RemoveSearchFactor(SearchFactor searchFactor)
        {
            _SearchFactors.Remove(searchFactor);
            searchFactor.OwnerSearchTerm = null;
 
        }
        internal SearchCondition OwnerSearchCondition;
        

        public bool ContainsSearchCondition(SearchCondition searchCondition)
        {
            foreach (var searchFactor in SearchFactors)
            {
                if (searchFactor.ToString()== searchCondition.ToString() ||(searchFactor.SearchCondition!=null&& searchFactor.SearchCondition.ContainsSearchCondition(searchCondition)))
                    return true;
            }

            if (searchCondition.SearchTerms.Count == 1)
            {
                
                bool containsAllSearchFactors = false; 

                foreach (var checkedSearchFactor in searchCondition.Clone().SearchTerms[0].SearchFactors)
                {
                    containsAllSearchFactors = false;
                    foreach (var searchFactor in SearchFactors)
                    {
                        if (searchFactor == checkedSearchFactor)
                        {
                            containsAllSearchFactors = true;
                            break;
                        }
                    }
                    if (!containsAllSearchFactors)
                        break;

                }
                return containsAllSearchFactors;
            }
            return false;
        }

        public bool ContainsCriterion(Criterion criterion)
        {
            foreach (var searchFactor in SearchFactors)
            {
                if (searchFactor.ContainsCriterion(criterion))
                    return true;
            }
            return false;
        }

        internal SearchTerm Clone()
        {
            OOAdvantech.Collections.Generic.List<SearchFactor> searchFactors = new OOAdvantech.Collections.Generic.List<SearchFactor>();
            foreach (var searchFactor in SearchFactors)
                searchFactors.Add(searchFactor.Clone());
            
            SearchTerm searchTerm =new SearchTerm(searchFactors);
            foreach (var searchFatcor in searchFactors)
                searchFatcor.OwnerSearchTerm = searchTerm;
            return searchTerm;
        }

        internal SearchTerm Clone(Dictionary<object, object> clonedObjects)
        {
            
            SearchTerm newSearchTerm = new SearchTerm();
            clonedObjects[this] = newSearchTerm;
            newSearchTerm.ConstrainTerm = ConstrainTerm;
            newSearchTerm.IsNotExpression=IsNotExpression;

            object ownerSearchCondition = null;
            if (clonedObjects.TryGetValue(OwnerSearchCondition, out ownerSearchCondition))
                newSearchTerm.OwnerSearchCondition = ownerSearchCondition as SearchCondition;
            else
                newSearchTerm.OwnerSearchCondition = OwnerSearchCondition.Clone(clonedObjects);



            foreach (var searchFactor in _SearchFactors)
            {
                object createdSearchFactor = null;
                if (clonedObjects.TryGetValue(searchFactor, out createdSearchFactor))
                    newSearchTerm._SearchFactors.Add(createdSearchFactor as SearchFactor);
                else
                    newSearchTerm._SearchFactors.Add(searchFactor.Clone(clonedObjects));
            }

            return newSearchTerm;
            
        }
    }
}
