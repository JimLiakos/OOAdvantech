using System;
using System.Linq;
using OOAdvantech.Collections.Generic;
namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    using SubDataNodeIdentity = System.Guid;
    using RelationPartIdentity = System.String;
    using DataSourceIdentity = System.Guid;
    using System.Runtime.Serialization;    /// <MetaDataID>{F6E4C6EB-2E9C-46FC-B99A-C04DFD47C011}</MetaDataID>
    public class ObjectsContextQuery : ObjectQuery
    {

#if DEBUG
        /// <MetaDataID>{86353a21-5562-428c-b823-03c4ea3c55ba}</MetaDataID>
        public static QueryResultType LastQueryResult;
#else
        public static QueryResultType LastQueryResult;
#endif
        [IgnoreErrorCheck]
        [RoleBMultiplicityRange(1, 1)]
        [RoleAMultiplicityRange(1)]
        [Association("", Roles.RoleA, "484ed475-348e-4488-a893-9b402bf6a3c9")]
        internal OOAdvantech.Collections.Generic.Dictionary<string, ObjectsContextMetadataDistributionManager> DistributedObjectQueriesManagers = new OOAdvantech.Collections.Generic.Dictionary<string, ObjectsContextMetadataDistributionManager>();



        /// <MetaDataID>{62FDBAD1-8C6B-4107-A9C4-0D990CFC0C0F}</MetaDataID>
        internal OOAdvantech.Collections.Generic.Dictionary<string, DataNode> DataNodeAliases = new OOAdvantech.Collections.Generic.Dictionary<string, DataNode>();
        /// <MetaDataID>{60AC46E1-8EB0-44B9-98A5-47E5B00A2241}</MetaDataID>
        public ObjectsContextQuery(OOAdvantech.Collections.Generic.Dictionary<string, object> parameters)
            : base(Guid.NewGuid())
        {
            Parameters = parameters;
            if (Parameters == null)
                Parameters = new OOAdvantech.Collections.Generic.Dictionary<string, object>();

        }



        ///// <MetaDataID>{88063C84-06E5-431E-8C5C-827DE85C9E38}</MetaDataID>
        /// <MetaDataID>{b2d8e11f-b95b-475b-bb0c-cf8664963d8e}</MetaDataID>
        public PersistenceLayer.ObjectStorage ObjectStorage
        {
            get
            {
                return ObjectsContext as PersistenceLayer.ObjectStorage;
            }
        }


        /// <MetaDataID>{cbc11cfd-fe96-4901-899c-40c773cde0c7}</MetaDataID>
        public OOAdvantech.ObjectsContext ObjectsContext;



        #region OQL Statament translation
        /// <MetaDataID>{D4BB83DC-AC90-47A9-ADC1-E17171ABC037}</MetaDataID>
        public virtual ComparisonTerm CreateComparisonTerm(ComparisonTerm.ComparisonTermType type, Parser.ParserNode comparisonTermParserNode)
        {

            if (type == ComparisonTerm.ComparisonTermType.Object)
                return new ObjectComparisonTerm(comparisonTermParserNode, this);

            if (type == ComparisonTerm.ComparisonTermType.ObjectAttribute)
                return new ObjectAttributeComparisonTerm(comparisonTermParserNode, this);

            if (type == ComparisonTerm.ComparisonTermType.ObjectID)
                return new ObjectIDComparisonTerm(comparisonTermParserNode, this);

            if (type == ComparisonTerm.ComparisonTermType.Literal)
                return new LiteralComparisonTerm(comparisonTermParserNode, this);

            if (type == ComparisonTerm.ComparisonTermType.Parameter)
                return new ParameterComparisonTerm(comparisonTermParserNode, this);
            return null;

        }

        /// <MetaDataID>{B8613980-D076-4D0B-8A01-45E03F9D4EF6}</MetaDataID>
        protected Parser.ParserNode SelectQueryExpression;
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{932A6E0E-E1C0-4AAF-B812-CB599DD6F933}</MetaDataID>
        static private Parser.Parser _OQLParser;
        /// <MetaDataID>{90D87771-67A8-4498-B416-BCC911259B3C}</MetaDataID>
        protected Parser.Parser OQLParser
        {
            get
            {
                if (_OQLParser == null)
                {
                    _OQLParser = new Parser.Parser();
                    //	myParser.SetGrammarPath("G:\\PersistenceLayer\\OQLParser\\OQLParser.gmr");
                    System.Type mType = GetType();
                    string[] Resources = typeof(ObjectsContextQuery).GetMetaData().Assembly.GetManifestResourceNames();
                    //using( System.IO.Stream Grammar = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("DotNetMetaDataRepository.OQL.OQLGrammar"))
                    string resourceName = null;
#if DeviceDotNet
                    resourceName = "OOAdvantech.DotNetMetaDataRepository.OQL.OQL.cgt";
#else
                    resourceName = "OOAdvantech.DotNetMetaDataRepository.OQL.OQL.cgt";
#endif
                    using (System.IO.Stream Grammar = typeof(ObjectsContextQuery).GetMetaData().Assembly.GetManifestResourceStream(resourceName))
                    {
                        byte[] bytes = new byte[Grammar.Length];
                        Grammar.Read(bytes, 0, (int)Grammar.Length);
                        _OQLParser.SetGrammar(bytes, (int)Grammar.Length);
#if !DeviceDotNet
                        Grammar.Close();
#endif
                    }
                }
                return _OQLParser;
            }
        }
        /// <MetaDataID>{51550934-EEAF-47EA-BB95-CFE404EAB34C}</MetaDataID>
        public override OOAdvantech.Collections.Generic.List<DataNode> SelectListItems
        {
            get
            {

                if (SelectQueryExpression == null)
                    throw new System.Exception("There isn't translated oql statement");
                return base.SelectListItems;

            }
        }

        /// <MetaDataID>{f4db2e56-0dd9-4ca7-aac0-d9e9337b70f2}</MetaDataID>
        void RetrieveWhereClauseDataNodes(Parser.ParserNode parserNode)
        {
            System.Collections.Generic.List<PathHead> WhereClausePaths = new System.Collections.Generic.List<PathHead>();
            if (parserNode["Critiria_Expression"] != null)
                GetPathsFromNode(parserNode["Critiria_Expression"] as Parser.ParserNode, WhereClausePaths);

            foreach (PathHead pathHead in WhereClausePaths)
            {
                DataNode parrentDataNode = null;
                if (DataNodeAliases.ContainsKey(pathHead.Name))
                    parrentDataNode = DataNodeAliases[pathHead.Name];
                else
                {
                    if (pathHead.AliasName != null)
                        throw new System.Exception("The " + pathHead.AliasName + " hasn't declared");
                    else
                        throw new System.Exception("The " + pathHead.Name + " hasn't declared");
                }
                DataNode mDataNode = CreateDataNodeFor(pathHead, parrentDataNode);
                mDataNode.ParticipateInWereClause = true;
                mDataNode.RelatedPaths.Add(pathHead);//.ParserNode);
            }
        }

        /// <MetaDataID>{21558B7A-C754-4898-990C-03348BBC1597}</MetaDataID>
        internal DataNode CreateDataNodeFor(Path path, DataNode ParentDataNode)
        {


            DataNode dataNode = null;
            if (path.AggregationPath)
                dataNode = new AggregateExpressionDataNode(this, path);
            else
                dataNode = new DataNode(this, path);



            dataNode.ParentDataNode = ParentDataNode;
            if (path.SubPath != null && dataNode.Type != DataNode.DataNodeType.Count)
            {
                dataNode = CreateDataNodeFor(path.SubPath, dataNode);
            }
            if (path.AggregateExpressionPath != null && dataNode.Type == DataNode.DataNodeType.Count)
            {
                CreateDataNodeFor(path.AggregateExpressionPath, dataNode);
                foreach (DataNode aggregateExpressionDataNode in (dataNode as AggregateExpressionDataNode).AggregateExpressionDataNodes)
                {
                    if (aggregateExpressionDataNode.Alias == null)
                        aggregateExpressionDataNode.Alias = GetValidAlias(aggregateExpressionDataNode.Name);

                    if (!string.IsNullOrEmpty(aggregateExpressionDataNode.Alias))
                        DataNodeAliases[aggregateExpressionDataNode.Alias] = aggregateExpressionDataNode;
                }


            }

            return dataNode;
        }

        /// <MetaDataID>{0FB6EDDC-BC11-48DF-81E4-C599BE2F108B}</MetaDataID>
        virtual protected DataNode CreateDataNodeFor(PathHead path, DataNode aliasDataNode)
        {
            DataNode dataNode = null;
            if (aliasDataNode != null)
            {
                dataNode = aliasDataNode;
                if (path.SubPath != null)
                    dataNode = CreateDataNodeFor(path.SubPath, dataNode);
            }
            else
            {
                dataNode = new DataNode(this, path);
                if (path.SubPath != null)
                    dataNode = CreateDataNodeFor(path.SubPath, dataNode);

            }

            return dataNode;
        }

        /// <MetaDataID>{BF5D88D5-09D4-4660-AE52-672AEDF8866F}</MetaDataID>
        protected void RetrieveWhereClauseDataNodes()
        {

            System.Collections.Generic.List<PathHead> WhereClausePaths = new System.Collections.Generic.List<PathHead>();

            if (SelectQueryExpression["Critiria_Expression"] != null)
                GetPathsFromNode(SelectQueryExpression["Critiria_Expression"] as Parser.ParserNode, WhereClausePaths);

            foreach (PathHead pathHead in WhereClausePaths)
            {

                DataNode ParrentNode = null;
                if (DataNodeAliases.ContainsKey(pathHead.Name))
                    ParrentNode = (DataNode)DataNodeAliases[pathHead.Name];
                else
                {
                    if (pathHead.AliasName != null)
                        throw new System.Exception("The " + pathHead.AliasName + " hasn't declared");
                    else
                        throw new System.Exception("The " + pathHead.Name + " hasn't declared");

                }

                DataNode mDataNode = CreateDataNodeFor(pathHead, ParrentNode);
                mDataNode.ParticipateInWereClause = true;


                mDataNode.RelatedPaths.Add(pathHead);//.ParserNode);

            }
        }

        /// <MetaDataID>{3F00A2B9-05B6-470B-ADC7-D5160194D816}</MetaDataID>
        protected void RetrieveSelectClauseDataNodes()
        {

            System.Collections.Generic.List<PathHead> SelectPaths = new System.Collections.Generic.List<PathHead>();
            GetPathsFromNode(SelectQueryExpression["Select_Clause"] as Parser.ParserNode, SelectPaths);

            foreach (PathHead pathHead in SelectPaths)
            {
                DataNode dataNode = null;
                DataNode ParrentNode = null;
                if (DataNodeAliases.ContainsKey(pathHead.Name))
                {
                    ParrentNode = (DataNode)DataNodeAliases[pathHead.Name];// error prone γιατί όχι μόνο alias
                    dataNode = CreateDataNodeFor(pathHead, ParrentNode);

                }
                else if (pathHead.AggregationPath)
                {
                    dataNode = CreateGroupMemberDataNodeFor(pathHead);
                }
                else
                {
                    if (pathHead.AliasName != null)
                        throw new System.Exception("The " + pathHead.AliasName + " hasn't declared");
                    else
                        throw new System.Exception("The " + pathHead.Name + " hasn't declared");

                }

                dataNode.RelatedPaths.Add(pathHead);//.ParserNode);
                dataNode.ParticipateInSelectClause = true;
                _SelectListItems.Add(dataNode);

            }

        }

        /// <MetaDataID>{c22440bb-7ae4-4a7c-bc5a-513f30433204}</MetaDataID>
        private DataNode CreateGroupMemberDataNodeFor(PathHead pathHead)
        {


            if (DataTrees.Count != 1)
                throw new System.NotSupportedException();

            DataNode groupDataNode = null;
            if (DataTrees[0].Type != DataNode.DataNodeType.Group)
            {
                groupDataNode = new DataNode(this);
                groupDataNode.Type = DataNode.DataNodeType.Group;
                DataTrees[0].ParentDataNode = groupDataNode;
                DataTrees[0] = groupDataNode;
            }
            else
                groupDataNode = DataTrees[0];

            DataNode dataNode = new DataNode(this, pathHead);
            dataNode.ParentDataNode = groupDataNode;
            return dataNode;

        }

        /// <MetaDataID>{6E0635A8-8604-4932-8E2A-9C4CB5E9891A}</MetaDataID>
        public void GetParserSyntaxErrors(Parser.ParserNode ParserNode, ref string ErrorOutput)
        {

            try
            {
                if (ParserNode.Name == "Data_Selection_Syntax_Error")
                {
                    ErrorOutput += "\nOQL Syntax Error in SELECT Clause at line (" + ParserNode.ChildNodes.GetFirst().ChildNodes.GetAt(5).Value + "): " + ParserNode.ParentNode.Value;
                    return;

                }
                if (ParserNode.Name == "Data_Path_Syntax_Error")
                {
                    ErrorOutput += "\nOQL Syntax Error in FROM Clause at line (" + ParserNode.ChildNodes.GetFirst().ChildNodes.GetAt(5).Value + "): " + ParserNode.ParentNode.Value;
                    return;
                }
                if (ParserNode.Name == "Critiria_Exp_Syntax_Error")
                {
                    ErrorOutput += "\nOQL Syntax Error in WHERE Clause at line (" + ParserNode.ChildNodes.GetFirst().ChildNodes.GetAt(5).Value + "): " + ParserNode.ParentNode.ParentNode.Value;
                    return;
                }
                for (short i = 0; i != ParserNode.ChildNodes.Count; i++)
                    GetParserSyntaxErrors(ParserNode.ChildNodes.GetAt(i + 1), ref ErrorOutput);
            }
            catch (Exception error)
            {

                throw;
            }
        }

        /// <MetaDataID>{E4AA8EA4-1E29-49EA-8E0D-2235B43C1CEB}</MetaDataID>
        protected void RetrieveDataTrees()
        {

            System.Collections.Generic.List<PathHead> DataTreePaths = new System.Collections.Generic.List<PathHead>();
            GetPathsFromNode(SelectQueryExpression["From_Clause"] as Parser.ParserNode, DataTreePaths);
            System.Collections.Generic.List<DataNode> DataNodes = new System.Collections.Generic.List<DataNode>();

            foreach (PathHead pathHead in DataTreePaths)
            {
                DataNode dataNode = CreateDataNodeFor(pathHead, null);
                if (dataNode.Alias != null)
                    DataNodeAliases.Add(dataNode.Alias, dataNode);
                DataNodes.Add(dataNode);
                dataNode.Type = DataNode.DataNodeType.Object;

            }

            //build data trees and link alias data trees with the main data trees
            //For example for 
            //FORM Family.Person person,person.Address personAddress ,person.Childrens personChildrens,person.Parents personParents,person.Parents.Address parentsAdress
            //we retrieve five DataNodes "person,personAddress,personChildrens,personParents,parentsAddress
            //and produce one data tree
            //Family
            //  |
            //	|_Person	[person]
            //		|
            //		|___Address		[personAddress]
            //		|  
            //		|___Childrens	[personChildrens]
            //		|
            //		|___Parents		[personParents]
            //				|	
            //				|___Address	[parentsAdress]


            foreach (DataNode dataNode in DataNodes)
            {
                DataNode headerDataNode = dataNode.HeaderDataNode;
                if (DataNodeAliases.ContainsKey(headerDataNode.Name))
                {
                    DataNode aliasDataNode = (DataNode)DataNodeAliases[headerDataNode.Name];
                    //link an alias based DataNode for example the Family.Person person with person.Address personAddress
                    if (headerDataNode.SubDataNodes.Count > 0)
                    {
                        System.Collections.Generic.List<DataNode> subDataNodes = new System.Collections.Generic.List<DataNode>(headerDataNode.SubDataNodes);
                        foreach (DataNode subDataNode in subDataNodes)
                            subDataNode.ParentDataNode = aliasDataNode;
                    }
                }
                else
                    DataTrees.Add(dataNode.HeaderDataNode);
            }
        }

        /// <MetaDataID>{BAA30997-B778-4511-B8F0-E08C7A77DBF5}</MetaDataID>
        static internal void GetPathsFromNode(Parser.ParserNode ParserNode, System.Collections.Generic.List<PathHead> Paths)
        {
            if (ParserNode.Name == "ObjectID")
                return;
            if (ParserNode.Name == "PathAlias" ||
                ParserNode.Name == "Path" ||
                ParserNode.Name == "TimePeriodPathAlias" ||
                ParserNode.Name == "Function_Ref")
                Paths.Add(new PathHead(ParserNode));
            else
            {
                for (short i = 1; i < ParserNode.ChildNodes.Count + 1; i++)
                    GetPathsFromNode(ParserNode.ChildNodes.GetAt(i), Paths);
            }
        }

        /// <MetaDataID>{9d5f6c38-4664-4060-91dd-eac8735eed12}</MetaDataID>
        public bool InParseMode = false;

        /// <MetaDataID>{AE27C493-AA00-4A9A-A5AF-D4E110C03908}</MetaDataID>
        public virtual bool Parse(string OQLExpretion, ref string errors)
        {
            InParseMode = true;

            bool HasSyntaxError = false;
            string CatchesErrorDescription = null;
            Parser.OQLParserResults oQLParserResults = new Parser.OQLParserResults();
            try
            {
                OQLParser.Parse(OQLExpretion,out oQLParserResults);
                int trt = 0;
            }
            catch (System.Exception Error)
            {
                foreach (Parser.SyntaxError syntaxError in oQLParserResults.SyntaxErrors)
                {
                    if (syntaxError.Token == "Select_List")
                    {
                        errors = string.Format("OQL syntax error : {0} at Line {1} and Position {2}", "Error in SELECT list (" + syntaxError.ErrorMessage + ").  ", syntaxError.Line, syntaxError.LinePosition);
                        return true;
                    }
                    errors = string.Format("OQL syntax error : {0} at Line {1} and Position {2}", syntaxError.ErrorMessage + ".  ", syntaxError.Line, syntaxError.LinePosition);
                    return true;
                }
                errors = "Syntax error";
                return true;
            }

            Parser.ParserNode Select_expression = null;
            string ErrorOutput = null;

            try
            {
                try
                {
                    Select_expression = oQLParserResults.theRoot["Start"]["OQLStatament"]["Select_Expression"] as Parser.ParserNode;
                }
                catch
                {
                }
                if (Select_expression != null)
                {
                    GetParserSyntaxErrors(Select_expression, ref ErrorOutput);
                    SelectQueryExpression = Select_expression;
                    RetrieveDataTrees();
                    RetrieveSelectClauseDataNodes();
                    RetrieveWhereClauseDataNodes(SelectQueryExpression);

                    RetrieveOrderByClauseDataNodes();
                    BuildDataNodeTree(ref ErrorOutput);

                    //foreach(DataNode CurrDataNode in DataTrees)
                    //    CurrDataNode.BuildDataNodeTree(default( PersistenceLayer.Storage), ref ErrorOutput);

                    ///TODO SearchCondition
                    if (SelectQueryExpression["Critiria_Expression"] != null)
                        DataTrees[0].AddSearchCondition(new SearchCondition(SelectQueryExpression["Critiria_Expression"]["Search_Condition"] as Parser.ParserNode, this, true));

                    foreach (var dataNode in SelectListItems)
                    {
                        if (SelectQueryExpression["Critiria_Expression"] != null)
                            dataNode.AddSearchCondition(new SearchCondition(SelectQueryExpression["Critiria_Expression"]["Search_Condition"] as Parser.ParserNode, this, true));
                    }
                }
            }
            catch (System.Exception Error)
            {
                CatchesErrorDescription = Error.Message;
            }
            if (CatchesErrorDescription != null)
                ErrorOutput += "\n" + CatchesErrorDescription;

            if (ErrorOutput != null)
            {
                errors += "\n" + ErrorOutput;
                return true;
            }
            else
            {
                if (HasSyntaxError)
                {
                    errors += "\nOQL Syntax Error on: '" + OQLExpretion + "'";
                    return true;
                }
            }
            return false;
        }

        /// <MetaDataID>{679E413F-D02E-4355-A393-A2A33E655A08}</MetaDataID>
        public virtual void Build(string oqlExpretion, OOAdvantech.PersistenceLayer.ObjectStorage objectStorage)
        {
            oqlExpretion = oqlExpretion.Trim();
            if (oqlExpretion.IndexOf("#OQL:") == 0)
                oqlExpretion = oqlExpretion.Substring(5, oqlExpretion.Length - 5);
            if (oqlExpretion.LastIndexOf("#") == oqlExpretion.Length - 1)
                oqlExpretion = oqlExpretion.Substring(0, oqlExpretion.Length - 1);
            Object OQLStatament = null;

            bool hasSyntaxError = false;
            string CatchesErrorDescription = null;
            ObjectsContext = objectStorage;
            Parser.OQLParserResults oQLParserResults = new Parser.OQLParserResults();
            try
            {
                OQLParser.Parse(oqlExpretion, out oQLParserResults);
            }
            catch (System.Exception error)
            {
                foreach (Parser.SyntaxError syntaxError in oQLParserResults.SyntaxErrors)
                {
                    if (syntaxError.Token == "Select_List")
                        throw new System.Exception(string.Format("{0} at Line {1} and Position {2}", "Error in SELECT list (" + syntaxError.ErrorMessage + ").  ", syntaxError.Line, syntaxError.LinePosition));
                    throw new System.Exception(string.Format("{0} at Line {1} and Position {2}", syntaxError.ErrorMessage + ".  ", syntaxError.Line, syntaxError.LinePosition));
                }
                throw new System.Exception("Syntax error");
            }
            int trt = 0;
            Parser.ParserNode Select_expression = null;
            try
            {
                if(oQLParserResults?.theRoot==null||oQLParserResults.theRoot["Start"]==null)
                {

                }
                OQLStatament =oQLParserResults.theRoot["Start"]["OQLStatament"];
                Select_expression = oQLParserResults.theRoot["Start"]["OQLStatament"]["Select_Expression"] as Parser.ParserNode;
            }
            catch (Exception error)
            {

                
            }
            string ErrorOutput = null;
            GetParserSyntaxErrors(Select_expression, ref ErrorOutput);

            try
            {
                SelectQueryExpression = Select_expression;
                RetrieveDataTrees();
                RetrieveSelectClauseDataNodes();
                RetrieveWhereClauseDataNodes(SelectQueryExpression);
                GetSecondaryCollectionFilterDataNodes(DataTrees[0]);
                RetrieveOrderByClauseDataNodes();

                BuildDataNodeTree(ref ErrorOutput);
                //foreach(DataNode CurrDataNode in DataTrees)
                //    CurrDataNode.BuildDataNodeTree(objectStorage.StorageMetaData, ref ErrorOutput);

                if (SelectQueryExpression["Critiria_Expression"] != null)
                {

                    var searchCondition = new SearchCondition(SelectQueryExpression["Critiria_Expression"]["Search_Condition"] as Parser.ParserNode, this, true);
                    foreach (DataNode dataNode in SelectListItems)
                        dataNode.AddSearchCondition(searchCondition);

                    //AssignDataNodesFilter();
                }
                GetSecondaryCollectionsSearchCondition(DataTrees[0]);

                if (SelectQueryExpression["Select_Clause"]["Function_Ref"] != null)
                {
                    DataNode countDataNode = DataTrees[0];
                    if ((SelectQueryExpression["Select_Clause"]["Count"]["Name"] as Parser.ParserNode) != null)
                    {
                        countDataNode = DataNodeAliases[(SelectQueryExpression["Select_Clause"]["Function_Ref"]["Name"] as Parser.ParserNode).Value] as DataNode;
                    }
                    else
                    {
                        while (countDataNode.Type == DataNode.DataNodeType.Namespace)
                            countDataNode = countDataNode.SubDataNodes[0];
                    }

                    if (countDataNode.Type == DataNode.DataNodeType.Object)
                    {
                        //countDataNode.CountSelect = true;
                        //AddSelectListItem(countDataNode);
                    }
                }

            }
            catch (System.Exception Error)
            {
                CatchesErrorDescription = Error.Message;
                if (ErrorOutput != null && ErrorOutput.Trim().Length > 0)
                    ErrorOutput += "\n" + CatchesErrorDescription;
                else ErrorOutput = CatchesErrorDescription;
                throw new System.Exception(ErrorOutput, Error);

            }
            if (ErrorOutput != null)
                throw new System.Exception(ErrorOutput);
            else
                if (hasSyntaxError)
                throw new System.Exception("OQL Syntax Error");

            foreach (DataNode dataNode in DataTrees)
            {
                CreateDataSources(dataNode, null);
                // dataNode.BuildDataSource(null);
            }



            //BuildMembersList();
        }

        /// <MetaDataID>{3002362f-a545-41a7-9339-802b6a2a411e}</MetaDataID>
        private void GetSecondaryCollectionsSearchCondition(DataNode dataNode)
        {
            if (dataNode.Type != DataNode.DataNodeType.Group && dataNode.Path.ParserNode["Critiria_Expression"] != null)
                dataNode.AddSearchCondition(new SearchCondition(dataNode.Path.ParserNode["Critiria_Expression"]["Search_Condition"] as Parser.ParserNode, this, true));


            foreach (DataNode subDataNode in dataNode.SubDataNodes)
                GetSecondaryCollectionsSearchCondition(subDataNode);

        }

        /// <MetaDataID>{64f39ff7-e85d-4510-859c-cb54fb16d670}</MetaDataID>
        private void GetSecondaryCollectionFilterDataNodes(DataNode dataNode)
        {

            if (dataNode.Type != DataNode.DataNodeType.Group && dataNode.Path.ParserNode["Critiria_Expression"] != null)
                RetrieveWhereClauseDataNodes(dataNode.Path.ParserNode);

            foreach (DataNode subDataNode in dataNode.SubDataNodes)
                GetSecondaryCollectionFilterDataNodes(subDataNode);
        }


        /// <MetaDataID>{ea1f9725-bc3e-4aaa-b328-fea97445dfd7}</MetaDataID>
        void GetObjectTypeDataNodes(DataNode dataNode, System.Collections.Generic.List<DataNode> objectTyeDataNodes)
        {
            if (dataNode.Type == DataNode.DataNodeType.Object)
            {
                objectTyeDataNodes.Add(dataNode);
                return;

            }

            foreach (DataNode subDataNode in dataNode.SubDataNodes)
                GetObjectTypeDataNodes(subDataNode, objectTyeDataNodes);
        }

        /// <MetaDataID>{345c7167-62fd-44bf-9f10-ae1293d6be43}</MetaDataID>
        static string LoadMetadataLock = "lock";


        ///// <MetaDataID>{840A2F44-C84D-4329-8B65-399CDD011F1E}</MetaDataID>
        //protected internal override Namespace GetNamespace(string _namespace)
        //{
        //    //TODO θα πρέπει να βρεθεί ένας τρόπος να αντιμετωπίζεται ενιαία τα meta data. 
        //    //Τώρα υπάρχουν δύο περιπτώσεις, η περίπτωση που διαβάζουμε τα meta data από το storage όπου 
        //    //το πρόβλημα είναι ότι αργότερα πρέπει να κάνω access τα fields από τα objects πρέπει DotnetMetadaRepository classes 
        //    //και δεύτερον η περίπτωση που δεν υπάρχουν metadata στην storage και εκεί δεν ξέρω 
        //    //αν το assembly που έχει τα types που θέλει to query έχει φορτωθεί.       


        //        #if !DeviceDotNet
        //        MetaObjectID metaObjectID = new MetaObjectID(_namespace);
        //        MetaDataRepository.Namespace namespaceMetaData = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObject(metaObjectID) as MetaDataRepository.Namespace;
        //        System.Collections.Generic.List<OOAdvantech.DotNetMetaDataRepository.Assembly> assemblies = new System.Collections.Generic.List<OOAdvantech.DotNetMetaDataRepository.Assembly>();
        //        if (namespaceMetaData == null)
        //        {
        //            lock (LoadMetadataLock)
        //            {
        //                foreach (System.Reflection.Assembly dotNetAssembly in System.AppDomain.CurrentDomain.GetAssemblies())
        //                {

        //                    if (dotNetAssembly.GetCustomAttributes(typeof(MetaDataRepository.BuildAssemblyMetadata), false).Length > 0)
        //                    {
        //                        DotNetMetaDataRepository.Assembly assembly = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(dotNetAssembly) as DotNetMetaDataRepository.Assembly;
        //                        if (assembly == null)
        //                            assembly = new DotNetMetaDataRepository.Assembly(dotNetAssembly);
        //                        assemblies.Add(assembly);
        //                        long load = assembly.Residents.Count;
        //                        namespaceMetaData = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObject(metaObjectID) as MetaDataRepository.Namespace;
        //                        if (namespaceMetaData != null)
        //                            return namespaceMetaData;
        //                    }
        //                }
        //            }
        //        }
        //        if (namespaceMetaData == null)
        //        {
        //            foreach (DotNetMetaDataRepository.Assembly assembly in assemblies)
        //            {
        //                foreach (Dependency dependency in assembly.ClientDependencies)
        //                {
        //                    DotNetMetaDataRepository.Assembly referAssembly = dependency.Supplier as DotNetMetaDataRepository.Assembly;
        //                    if (referAssembly.WrAssembly.GetCustomAttributes(typeof(MetaDataRepository.BuildAssemblyMetadata), false).Length > 0)
        //                    {
        //                        long load = referAssembly.Residents.Count;
        //                        namespaceMetaData = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObject(metaObjectID) as MetaDataRepository.Namespace;
        //                        if (namespaceMetaData != null)
        //                            return namespaceMetaData;
        //                    }
        //                }

        //            }
        //            assemblies = new System.Collections.Generic.List<OOAdvantech.DotNetMetaDataRepository.Assembly>();
        //            foreach (DotNetMetaDataRepository.Assembly assembly in assemblies)
        //            {
        //                GetAllAssemblyReferences(assembly, assemblies);
        //            }

        //            foreach (DotNetMetaDataRepository.Assembly referAssembly in assemblies)
        //            {
        //                if (referAssembly.WrAssembly.GetCustomAttributes(typeof(MetaDataRepository.BuildAssemblyMetadata), false).Length > 0)
        //                {
        //                    long load = referAssembly.Residents.Count;
        //                    namespaceMetaData = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObject(metaObjectID) as MetaDataRepository.Namespace;
        //                    if (namespaceMetaData != null)
        //                        return namespaceMetaData;
        //                }
        //            }

        //        }
        //        return namespaceMetaData;
        //    #else
        //        return null;
        //    #endif 

        //}
        //void GetAllAssemblyReferences(DotNetMetaDataRepository.Assembly  assembly, System.Collections.Generic.List<DotNetMetaDataRepository.Assembly > assemblies)
        //{
        //    foreach (Dependency dependency in assembly.ClientDependencies)
        //    {
        //        DotNetMetaDataRepository.Assembly referAssembly = dependency.Supplier as DotNetMetaDataRepository.Assembly;

        //        if (!assemblies.Contains(referAssembly))
        //            assemblies.Add(referAssembly);
        //        GetAllAssemblyReferences(referAssembly, assemblies);
        //    }

        //}


        // /// <MetaDataID>{85C6F8B2-B61E-431F-917F-D7F8B7EE6E7E}</MetaDataID>
        // OOAdvantech.Collections.MemberList Members;
        // /// <MetaDataID>{D27B2297-DC2D-48AB-A3E6-893D7564B661}</MetaDataID>
        // private void BuildMembersList()
        // {
        //     // if (Members == null)
        //     //   Members = new MemberList(mOQLStatement.DataTrees[0] as MetaDataRepository.ObjectQueryLanguage.DataNode);
        //}

        ///// <MetaDataID>{1570CDF6-446C-4FC1-B197-EA32EB51C0C2}</MetaDataID>
        //private void AssignDataNodesFilter()
        //{
        //    foreach (DataNode dataNode in DataTrees)
        //        dataNode.AssignDataNodeFilter(SearchCondition);
        //}


        /// <MetaDataID>{8995124A-C0F4-4F0C-822D-A9D1E6BA55BD}</MetaDataID>
        protected void RetrieveOrderByClauseDataNodes()
        {
            System.Collections.Generic.List<PathHead> OrderByClausePaths = new System.Collections.Generic.List<PathHead>();

            if (SelectQueryExpression["order_by_exp"] != null)
                GetPathsFromNode(SelectQueryExpression["order_by_exp"] as Parser.ParserNode, OrderByClausePaths);

            foreach (PathHead pathHead in OrderByClausePaths)
            {

                DataNode ParrentNode = null;
                if (DataNodeAliases.ContainsKey(pathHead.Name))
                    ParrentNode = (DataNode)DataNodeAliases[pathHead.Name];// error prone γιατί όχι μόνο alias
                else
                {
                    if (pathHead.AliasName != null)
                        throw new System.Exception("The " + pathHead.AliasName + " hasn't declared");
                    else
                        throw new System.Exception("The " + pathHead.Name + " hasn't declared");
                }
                DataNode mDataNode = CreateDataNodeFor(pathHead, ParrentNode);
                mDataNode.RelatedPaths.Add(pathHead);//.ParserNode);
                if ((SelectQueryExpression["order_by_exp"]["ascending_or_descending"] as Parser.ParserNode).Value == "DESC")
                    mDataNode.OrderBy = OrderByType.DESC;
                else
                    mDataNode.OrderBy = OrderByType.ASC;
            }
            int k = 0;
        }


        #endregion

        /// <MetaDataID>{7BD46C75-2F12-4D0D-8A8B-C1E3A0413859}</MetaDataID>
        public void BuildDataNodeTree(ref string errorOutput)
        {
            OOAdvantech.Collections.Generic.List<DataNode> objectTypeDataNodes = new OOAdvantech.Collections.Generic.List<DataNode>();
            foreach (DataNode CurrDataNode in DataTrees)
                GetObjectTypeDataNodes(CurrDataNode, objectTypeDataNodes);



            foreach (DataNode dataNode in objectTypeDataNodes)
            {
                if (dataNode.AssignedMetaObject == null)
                {
                    dataNode.MergeIdenticalDataNodes();
                    dataNode.AssignDataNodeToParserPaths(PathDataNodeMap);

                    System.Type type = null;
                    if (!string.IsNullOrEmpty(dataNode.ClassifierFullName))
                        type = GetType(dataNode.ClassifierFullName, dataNode.ClassifierImplementationUnitName);
                    else
                        type = GetType(dataNode.FullName, "");

                    MetaDataRepository.Classifier classifier = null;
                    if (type != null)
                        classifier = MetaDataRepository.Classifier.GetClassifier(type);

                    if (classifier == null)
                    {
                        errorOutput += "There isn't type '" + dataNode.FullName + "'";
                        return;
                    }

                    dataNode.AssignedMetaObject = classifier;
                }
                dataNode.Validate(ref errorOutput);
                dataNode.MergeIdenticalDataNodes();
                if (errorOutput != null && errorOutput.Length > 0)
                    return;
            }
            OOAdvantech.Collections.Generic.List<DataNode> dataTrees = new OOAdvantech.Collections.Generic.List<DataNode>();
            foreach (DataNode dataNode in DataTrees)
                dataTrees.AddRange(dataNode.RemoveNamespacesDataNodes());
            DataTrees = dataTrees;



        }


        ///// <MetaDataID>{81eeee02-1c73-4a32-adf3-1d9cdda01688}</MetaDataID>
        //void RemoveSearchingData(DataNode dataNode, System.Collections.Generic.Dictionary<string, SearchingData> searchingData)
        //{
        //    searchingData.Add(dataNode.FullName, new SearchingData(dataNode.SearchCondition, dataNode.SearchCriteria));
        //    dataNode.SearchCondition = null;
        //    dataNode.SearchCriteria = null;

        //    foreach (DataNode subDataNode in dataNode.SubDataNodes)
        //        RemoveSearchingData(subDataNode, searchingData);
        //}
        ///// <MetaDataID>{8700a7a8-a310-4794-9fb3-691523aba8bf}</MetaDataID>
        //public static bool NewPrefetchingMechanism = true;
        ///// <MetaDataID>{2d38a61e-56d9-4124-870b-3a37bf463d8b}</MetaDataID>
        //void SetSearchingData(DataNode dataNode, System.Collections.Generic.Dictionary<string, SearchingData> searchingData)
        //{
        //    dataNode.SearchCondition = searchingData[dataNode.FullName].SearchCondition;
        //    dataNode.SearchCriteria = searchingData[dataNode.FullName].SearchCriteria;
        //    foreach (DataNode subDataNode in dataNode.SubDataNodes)
        //        SetSearchingData(subDataNode, searchingData);
        //}



        /// <summary>
        /// This property is true when all DataLoaders metadata are transferred to distribute queries
        /// </summary>
        /// <MetaDataID>{11870326-9cb2-4355-8e11-76c572a0b7de}</MetaDataID>
        bool MetaDataDistributionCompleted
        {
            get
            {
                foreach (ObjectsContextMetadataDistributionManager metaDataDistributionManager in DistributedObjectQueriesManagers.Values)
                {
                    if (metaDataDistributionManager.DistributedObjectQueryUpdated)
                        continue;
                    else
                        return false;
                }
                return true;
            }
        }

        /// <summary>
        /// This method adds, if needed an count aggregation DataNode for the recalculation of average result  
        /// when collects data from destributed queries. 
        /// </summary>
        /// <param name="rootDataNode">
        /// Specifies DataNode considered if has average sub DataNode which needs count dataNode addendum
        /// </param>
        void ExtendDataNodeTreeForAverageDataNodes(DataNode rootDataNode)
        {
            foreach (DataNode subDataNode in rootDataNode.SubDataNodes.ToArray())
            {
                if (subDataNode.Type == DataNode.DataNodeType.Average)
                {
                    if ((rootDataNode.DataSource as StorageDataSource).DataLoadersMetadata.Count > 1)
                    {

                        AggregateExpressionDataNode countDataNode = (from dataNode in rootDataNode.SubDataNodes.OfType<AggregateExpressionDataNode>()
                                                                     where dataNode.Type == DataNode.DataNodeType.Count && (dataNode as AggregateExpressionDataNode).SourceSearchCondition == (subDataNode as AggregateExpressionDataNode).SourceSearchCondition
                                                                     select dataNode).FirstOrDefault();
                        if (countDataNode == null)
                        {
                            countDataNode = new AggregateExpressionDataNode(this);
                            countDataNode.Type = DataNode.DataNodeType.Count;
                            countDataNode.Name = subDataNode.Name + "_AvrgCount";
                            if (rootDataNode is GroupDataNode)
                                countDataNode.AddAggregateExpressionDataNode((rootDataNode as GroupDataNode).GroupedDataNode);
                            countDataNode.ParentDataNode = rootDataNode;
                            countDataNode.SourceSearchCondition = (subDataNode as AggregateExpressionDataNode).SourceSearchCondition;
                        }
                    }
                }
                else
                    ExtendDataNodeTreeForAverageDataNodes(subDataNode);

            }

        }

        /// <summary>
        /// Distribute object query metadata to the query parts.
        /// For each objects context where query retrieves data creates DistributedQuery 
        /// </summary>
        /// <MetaDataID>{e422868c-4bb7-401b-858f-63608ac0ac5b}</MetaDataID>
        public virtual void Distribute()
        {

            //if (QueryResultType != null && QueryResultType.RootDataNode.Type == DataNode.DataNodeType.Group &&
            //    (QueryResultType.RootDataNode.DataSource as StorageDataSource).DataLoadersMetadata.Count > 1)
            //{
            //    foreach (var member in QueryResultType.Members)
            //    {
            //        if (member.SourceDataNode is AggregateExpressionDataNode && member.SourceDataNode.Type == DataNode.DataNodeType.Average)
            //        {
            //            AggregateExpressionDataNode countDataNode = null;
            //            foreach (DataNode subDataNode in QueryResultType.RootDataNode.SubDataNodes)
            //            {
            //                if (subDataNode.Type == DataNode.DataNodeType.Count)
            //                {
            //                    countDataNode = subDataNode as AggregateExpressionDataNode;
            //                    break;
            //                }
            //            }
            //            break;
            //        }
            //    }
            //    if (QueryResultType.Members.Count == 0)
            //    {
            //        if (QueryResultType.ValueDataNode is AggregateExpressionDataNode && QueryResultType.ValueDataNode.Type == DataNode.DataNodeType.Average)
            //        {
            //            AggregateExpressionDataNode countDataNode = null;
            //            foreach (DataNode subDataNode in QueryResultType.RootDataNode.SubDataNodes)
            //            {
            //                if (subDataNode.Type == DataNode.DataNodeType.Count)
            //                {
            //                    countDataNode = subDataNode as AggregateExpressionDataNode;
            //                    break;
            //                }
            //            }
            //        }
            //    }
            //}
            //if (QueryResultType != null)
            //    CheckGroupingTypes(QueryResultType);




            Collections.Generic.Dictionary<string, ObjectsContext> objectsContexts = new OOAdvantech.Collections.Generic.Dictionary<string, ObjectsContext>();
            DataTrees[0].BookAlias();

            ExtendDataNodeTreeForAverageDataNodes(DataTrees[0]); //Adds nessasery count datanodes for average calculation


            (DataTrees[0] as MetaDataRepository.ObjectQueryLanguage.DataNode).GetObjectsContexts(DistributedObjectQueriesManagers);
            foreach (ObjectsContextMetadataDistributionManager metaDataDistributionManager in DistributedObjectQueriesManagers.Values)
                metaDataDistributionManager.MasterQuery = this;
            System.Collections.Generic.List<DataNode> allPrefetchingDataNodes = new System.Collections.Generic.List<DataNode>();

            #region Object query distribution

            while (!MetaDataDistributionCompleted)
            {
                foreach (ObjectsContextMetadataDistributionManager metaDataDistributionManager in new System.Collections.Generic.List<ObjectsContextMetadataDistributionManager>(DistributedObjectQueriesManagers.Values))
                {
                    if (metaDataDistributionManager.DistributedObjectQuery != null && metaDataDistributionManager.PrefetcingDataLoadersMetadata.Count == 0)
                        continue;
                    if (metaDataDistributionManager.MasterQuery == null)
                        metaDataDistributionManager.MasterQuery = this;

                    Collections.Generic.List<DataNodeRelatedDataLoadersMetadata> dataNodesPrefetchingData = metaDataDistributionManager.UpdateDistributedQueryMetadata();

                    #region Dispatch prefetching datanodes DataLoaderMetaData
                    foreach (DataNodeRelatedDataLoadersMetadata dataNodePrefetchingData in dataNodesPrefetchingData)
                        allPrefetchingDataNodes.AddRange(dataNodePrefetchingData.SubDataNodesNewDataLoadersMetadata.Keys);

                    foreach (DataNodeRelatedDataLoadersMetadata dataNodePrefetchingData in dataNodesPrefetchingData)
                    {
                        #region update metadata distribution managers with the new DataLoaderMetadata of prefetching DataNode

                        foreach (DataLoaderMetadata dataLoaderMetadata in dataNodePrefetchingData.DataNodeNewDataLoadersMetadata.Values)
                            UpdateQueryWithNewPrefethingDataLoaderMetadata(dataNodePrefetchingData.DataNode, dataLoaderMetadata);

                        #endregion

                        #region update metadata distribution managers the new DataLoaderMetadata of prefetching DataNode subDataNodes

                        foreach (DataNode subDataNode in dataNodePrefetchingData.SubDataNodesNewDataLoadersMetadata.Keys)
                        {
                            DataNode prefetchingDataDataNode = DataTrees[0].GetDataNode(subDataNode.Identity);
                            if (prefetchingDataDataNode.DataSource == null)
                                prefetchingDataDataNode.DataSource = new StorageDataSource(prefetchingDataDataNode);
                            foreach (DataLoaderMetadata dataLoaderMetadata in dataNodePrefetchingData.SubDataNodesNewDataLoadersMetadata[subDataNode].Values)
                            {
                                (prefetchingDataDataNode.DataSource as StorageDataSource).DataLoadersMetadata[dataLoaderMetadata.ObjectsContextIdentity] = dataLoaderMetadata;
                                if (metaDataDistributionManager.ObjectsContextIdentity != dataLoaderMetadata.ObjectsContextIdentity)
                                    UpdateQueryWithNewPrefethingDataLoaderMetadata(prefetchingDataDataNode, dataLoaderMetadata);
                                if (subDataNode.MembersFetchingObjectActivation && !prefetchingDataDataNode.MembersFetchingObjectActivation)
                                {
                                    prefetchingDataDataNode.MembersFetchingObjectActivation = true;
                                    foreach (DataLoaderMetadata updateDataLoaderMetadata in (prefetchingDataDataNode.DataSource as StorageDataSource).DataLoadersMetadata.Values)
                                    {
                                        if (dataLoaderMetadata.ObjectsContextIdentity != metaDataDistributionManager.ObjectsContextIdentity)
                                            UpdateQueryWithNewPrefethingDataLoaderMetadata(prefetchingDataDataNode, dataLoaderMetadata);
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    #endregion

                }
            }
            #endregion


            #region update master query dataloading meta data

            foreach (DataNode dataNode in allPrefetchingDataNodes)
                dataNode.ParticipateInSelectClause = false;

            System.Collections.Generic.Dictionary<Guid, DataSource> dataSources = null;
            (DataTrees[0] as MetaDataRepository.ObjectQueryLanguage.DataNode).GetDataSources(ref dataSources);


            foreach (ObjectsContextMetadataDistributionManager metaDataDistributionManager in DistributedObjectQueriesManagers.Values)
            {
                DistributedObjectQuery distributedObjectQuery = metaDataDistributionManager.DistributedObjectQuery;
                OOAdvantech.Collections.Generic.Dictionary<Guid, DataLoader> dataLoaders = distributedObjectQuery.DataLoaders;
                foreach (System.Collections.Generic.KeyValuePair<Guid, DataSource> dataSourceEntry in dataSources)
                {
                    DataSource dataSource = dataSourceEntry.Value;
                    if (dataLoaders.ContainsKey(dataSource.Identity) && !dataSource.DataLoaders.ContainsKey((distributedObjectQuery.ObjectsContext as ObjectsContext).Identity))
                        dataSource.DataLoaders.Add((distributedObjectQuery.ObjectsContext as ObjectsContext).Identity, dataLoaders[dataSource.Identity]);
                }
            }
            #endregion




            foreach (DataSource dataSource in dataSources.Values)
            {
                //TODO Αυτή η συνθήκη είναι επίρεπης σε σφάλμα δεν ξεθαρίζει ότι η DataNode δημιουργήθηκε μον για member fetching λόγους
                //if(!dataSource.DataNode.AutoGenaratedForMembersFetching)
                if (dataSource.DataNode.AssignedMetaObject == null)
                {
                    foreach (ObjectsContextMetadataDistributionManager metaDataDistributionManager in DistributedObjectQueriesManagers.Values)
                    {
                        DistributedObjectQuery objectQuery = metaDataDistributionManager.DistributedObjectQuery;
                        objectQuery.UpdateDataLoadingModel(dataSource.DataNode.Identity);
                    }
                }
                else
                {
                    dataSource.UpdateDataLoadingModel();
                }
            }

            // State change from object distribution to data loading
            foreach (DataSource dataSource in dataSources.Values)
                dataSource.PrepareForDataLoading();


            //System.Collections.Generic.Dictionary<Guid, Criterion> criteria = new System.Collections.Generic.Dictionary<Guid, Criterion>();

            //foreach (var criterion in (from searchCondition in DataTrees[0].BranchSearchConditions
            //                           where searchCondition != null
            //                           from criterion in searchCondition.Criterions
            //                           select criterion))
            //{
            //    criterion.Applied = true;
            //    criteria[criterion.Identity] = criterion;
            //}
            //foreach (ObjectsContextMetadataDistributionManager metaDataDistributionManager in DistributedObjectQueries.Values)
            //{
            //    DistributedObjectQuery distributedObjectQuery = metaDataDistributionManager.DistributedObjectQuery;
            //    foreach (var criterionIdentity in distributedObjectQuery.GlobalResolvedCriteria())
            //        criteria[criterionIdentity].Applied = false;
            //}
        }

        ///// <MetaDataID>{c253f8ed-6268-4634-9eae-7098e1a9aa63}</MetaDataID>
        //private void CheckGroupingTypes(QueryResultType queryResultType)
        //{
        //    if (queryResultType.RootDataNode is GroupDataNode &&
        //        queryResultType.RootDataNode.DataSource is StorageDataSource &&
        //        (queryResultType.RootDataNode.DataSource as StorageDataSource).DataLoadersMetadata.Count > 0)
        //    {
        //        QueryResultPart averageMember = null;
        //        QueryResultPart countMember = null;
        //        foreach (var member in queryResultType.Members)
        //        {
        //            if (member.SourceDataNode.Type == DataNode.DataNodeType.Average)
        //                averageMember = member;
        //            if (member.SourceDataNode.Type == DataNode.DataNodeType.Count)
        //                countMember = member;
        //        }

        //        if (averageMember != null && countMember == null)
        //        {
        //            string avrCountName = "AvrgCount";
        //            int i = 1;
        //            while (queryResultType.GetMember(avrCountName) != null)
        //            {
        //                avrCountName = "AvrgCount_" + i.ToString();
        //                i++;
        //            }
        //            AggregateExpressionDataNode countDataNode = new AggregateExpressionDataNode(this);
        //            countDataNode.Type = DataNode.DataNodeType.Count;
        //            countDataNode.Name = avrCountName;
        //            countDataNode.AddAggregateExpressionDataNode((queryResultType.RootDataNode as GroupDataNode).GroupedDataNode);
        //            countDataNode.ParentDataNode = queryResultType.RootDataNode;
        //            countMember = new SinglePart(countDataNode, avrCountName, queryResultType);
        //            queryResultType.AddMember(countMember);
        //        }
        //    }
        //    foreach (var member in queryResultType.Members)
        //    {
        //        if (member is EnumerablePart)
        //            CheckGroupingTypes((member as EnumerablePart).Type);
        //        if (member is CompositePart)
        //            CheckGroupingTypes((member as CompositePart).Type);
        //    }
        //}

        /// <summary>
        /// This method assigns the data to the corresponding ObjectsContextMetadataDistributionManager 
        /// </summary>
        /// <param name="prefetchingDataDataNode">
        /// Defines the DataNode of DataLoaderMetaData on MainQuery
        /// </param>
        /// <param name="dataLoaderMetadata">
        /// Defines the DataLoaderMetaData
        /// </param>
        /// <MetaDataID>{00d229da-d84d-45a0-beab-9c0964771786}</MetaDataID>
        private void UpdateQueryWithNewPrefethingDataLoaderMetadata(DataNode prefetchingDataDataNode, DataLoaderMetadata dataLoaderMetadata)
        {
            if (prefetchingDataDataNode.ObjectQuery != this)
                throw new Exception("Invalid DataNode. DataNode doesn't belong to the StorageObjectQuery");
            if (!DistributedObjectQueriesManagers.ContainsKey(dataLoaderMetadata.ObjectsContextIdentity))
            {
                PersistenceLayer.ObjectStorage objectStorage = PersistenceLayer.ObjectStorage.OpenStorage(dataLoaderMetadata.StorageName, dataLoaderMetadata.StorageLocation, dataLoaderMetadata.StorageType);
                DistributedObjectQueriesManagers[dataLoaderMetadata.ObjectsContextIdentity] = new ObjectsContextMetadataDistributionManager(dataLoaderMetadata.ObjectsContextIdentity, objectStorage);
                DistributedObjectQueriesManagers[dataLoaderMetadata.ObjectsContextIdentity].MasterQuery = this;
            }
            DistributedObjectQueriesManagers[dataLoaderMetadata.ObjectsContextIdentity].AddDataLoaderMetadata(prefetchingDataDataNode, dataLoaderMetadata);
        }

        /// <MetaDataID>{dc2a30fb-e9eb-4d84-aa3b-222748785bbe}</MetaDataID>
        protected internal override void LoadData()
        {
            Distribute();
            var sh = DataTrees[0].BranchSearchConditions.Count;

            OOAdvantech.Collections.Generic.Dictionary<DistributedObjectQuery, QueryResultType> distributedObjectQueriesResults = new OOAdvantech.Collections.Generic.Dictionary<DistributedObjectQuery, QueryResultType>();

            OOAdvantech.Collections.Generic.Dictionary<string, DistributedObjectQuery> distributedObjectQueries = new OOAdvantech.Collections.Generic.Dictionary<string, DistributedObjectQuery>();
            foreach (ObjectsContextMetadataDistributionManager metaDataDistributionManager in DistributedObjectQueriesManagers.Values)
                distributedObjectQueries.Add(metaDataDistributionManager.ObjectsContextIdentity, metaDataDistributionManager.DistributedObjectQuery);

            ObjectQueryDataSet = DataSource.DataObjectsInstantiator.CreateDataSet();
            //System.Collections.Generic.List<SearchCondition> searchConditions = new System.Collections.Generic.List<SearchCondition>();

            bool allQueryResultLoadedLocaly = true;

            #region Load DistributedObjectQuery Data

            foreach (ObjectsContextMetadataDistributionManager metaDataDistributionManager in DistributedObjectQueriesManagers.Values)
            {
                DistributedObjectQuery distributedObjectQuery = metaDataDistributionManager.DistributedObjectQuery;
                distributedObjectQuery.LoadData(distributedObjectQueries);


                #region RowRemove code
                //if (!OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(distributedObjectQuery))
                //{
                //    foreach (DataLoader dataLoader in distributedObjectQuery.DataLoaders.Values)
                //    {
                //        if (dataLoader.DataNode.SearchCondition != null && !searchConditions.Contains(dataLoader.DataNode.SearchCondition) && dataLoader.DataNode.SearchCondition.RemovedRows != null && dataLoader.DataNode.SearchCondition.RemovedRows.Count > 0)
                //        {
                //            searchConditions.Add(dataLoader.DataNode.SearchCondition);
                //            DataNode dataNode = DataTrees[0].GetDataNode(dataLoader.DataNode.Identity);
                //            foreach (System.Data.DataRow row in dataLoader.DataNode.SearchCondition.RemovedRows.Keys)
                //                dataNode.SearchCondition.RemoveRow(row, -1);
                //        }
                //    }
                //}
                #endregion
            }
            #endregion

            #region Loads object relation links
            foreach (ObjectsContextMetadataDistributionManager metaDataDistributionManager in DistributedObjectQueriesManagers.Values)
            {
                DistributedObjectQuery distributedObjectQuery = metaDataDistributionManager.DistributedObjectQuery;
                distributedObjectQuery.LoadObjectRelationLinks(distributedObjectQueries);
            }
            #endregion

            #region Activate passive objects

            foreach (ObjectsContextMetadataDistributionManager metaDataDistributionManager in DistributedObjectQueriesManagers.Values)
            {
                DistributedObjectQuery distributedObjectQuery = metaDataDistributionManager.DistributedObjectQuery;
                distributedObjectQuery.ActivatePassiveObjects();
            }

            #endregion

            bool allQueryDataLoaded = AllQueryDataLoaded;


            foreach (ObjectsContextMetadataDistributionManager metaDataDistributionManager in DistributedObjectQueriesManagers.Values)
            {
                DistributedObjectQuery distributedObjectQuery = metaDataDistributionManager.DistributedObjectQuery;
                distributedObjectQuery.LoadQueryResult(distributedObjectQueries);
                if (distributedObjectQuery.QueryResultLoadedLocaly)
                    distributedObjectQueriesResults[distributedObjectQuery] = distributedObjectQuery.QueryResultType;
                else
                    allQueryResultLoadedLocaly = false;
            }



            System.Collections.Generic.Dictionary<Guid, Criterion> criteria = new System.Collections.Generic.Dictionary<Guid, Criterion>();
            foreach (var criterion in (from searchCondition in DataTrees[0].BranchSearchConditions
                                       where searchCondition != null
                                       from criterion in searchCondition.Criterions
                                       select criterion))
            {
                criterion.Applied = true;
                criteria[criterion.Identity] = criterion;
            }

            foreach (ObjectsContextMetadataDistributionManager metaDataDistributionManager in DistributedObjectQueriesManagers.Values)
            {
                DistributedObjectQuery distributedObjectQuery = metaDataDistributionManager.DistributedObjectQuery;
                foreach (var criterionIdentity in distributedObjectQuery.GlobalResolvedCriteria())
                    criteria[criterionIdentity].Applied = false;
            }

            if (!allQueryResultLoadedLocaly)
            {
                if (QueryResultType != null)
                {
                    #region Gets row Data from distributed queries which can't produce query result locally
                    OOAdvantech.Collections.Generic.Dictionary<Guid, OOAdvantech.Collections.Generic.List<StorageDataLoader>> dataLoaders = new OOAdvantech.Collections.Generic.Dictionary<Guid, OOAdvantech.Collections.Generic.List<StorageDataLoader>>();
                    foreach (ObjectsContextMetadataDistributionManager metaDataDistributionManager in DistributedObjectQueriesManagers.Values)
                    {
                        DistributedObjectQuery distributedObjectQuery = metaDataDistributionManager.DistributedObjectQuery;
                        if (!distributedObjectQueriesResults.ContainsKey(distributedObjectQuery))
                            GetQueryResultDataLoaders(QueryResultType, distributedObjectQuery, DataTrees[0], dataLoaders);
                    }
                    DataTrees[0].GetData(ObjectQueryDataSet, dataLoaders);
                    #endregion


                    #region produce main query query results from the collected data

                    foreach (DataNode dataNode in DataTrees)
                        OrganizeData(dataNode);

                    QueryResultType.DataLoader.LoadData();

                    #endregion

                    //Adds distributed query query results to the main query result 
                    foreach (QueryResultType distributedQueryResult in distributedObjectQueriesResults.Values)
                        QueryResultType.AddDistributedQueryResult(distributedQueryResult);
                }
                else
                    DataTrees[0].GetData(ObjectQueryDataSet, null);
            }
            else
            {
                foreach (QueryResultType distributedQueryResult in distributedObjectQueriesResults.Values)
                    QueryResultType.AddDistributedQueryResult(distributedQueryResult);
            }

            // FilterData();
        }

        private bool AllQueryDataLoaded
        {
            get
            {
                foreach (ObjectsContextMetadataDistributionManager metaDataDistributionManager in DistributedObjectQueriesManagers.Values)
                {
                    DistributedObjectQuery distributedObjectQuery = metaDataDistributionManager.DistributedObjectQuery;
                    if (!distributedObjectQuery.AllQueryDataLoaded)
                        return false;
                }
                return true;
            }
        }


        /// <summary>
        /// Retrieves necessary DataLoaders to loads query result data 
        /// The Root DataLoader load data from objects contex of distributedObjectQuery
        /// </summary>
        /// <param name="queryResultType">
        /// Defines the query result type
        /// </param>
        /// <param name="distributedObjectQuery">
        /// Defines the distribute object query that cann't load querery result locally
        /// </param>
        /// <param name="dataNode">
        /// Defines the root DataNode where mechanism start the data tree traversion to get DataLoaders
        /// </param>
        /// <param name="resultDataLoaders">
        /// Defines the collection where mechanism uses to add  the retrivied dataLoaders
        /// </param>
        /// <MetaDataID>{9a73ad11-0c34-40f3-a894-d2c234e385cf}</MetaDataID>
        private void GetQueryResultDataLoaders(QueryResultType queryResultType,
                                            DistributedObjectQuery distributedObjectQuery,
                                            DataNode dataNode,
                                            OOAdvantech.Collections.Generic.Dictionary<Guid, OOAdvantech.Collections.Generic.List<StorageDataLoader>> resultDataLoaders)
        {

            if (dataNode.Type == DataNode.DataNodeType.Namespace)
                GetQueryResultDataLoaders(queryResultType, distributedObjectQuery, dataNode.SubDataNodes[0], resultDataLoaders);
            else
            {
                if ((dataNode.DataSource as StorageDataSource).HasDataInObjectContext(distributedObjectQuery.ObjectsContextIdentity))
                {
                    DataLoaderMetadata dataLoaderMetadata = (dataNode.DataSource as StorageDataSource).DataLoadersMetadata[distributedObjectQuery.ObjectsContextIdentity];
                    if (!resultDataLoaders.ContainsKey(dataNode.Identity))
                        resultDataLoaders[dataNode.Identity] = new OOAdvantech.Collections.Generic.List<StorageDataLoader>();
                    if (!resultDataLoaders[dataNode.Identity].Contains(dataNode.DataSource.DataLoaders[dataLoaderMetadata.ObjectsContextIdentity] as StorageDataLoader))
                        resultDataLoaders[dataNode.Identity].Add(dataNode.DataSource.DataLoaders[dataLoaderMetadata.ObjectsContextIdentity] as StorageDataLoader);

                    foreach (var subDataNode in dataNode.SubDataNodes)
                    {
                        if (queryResultType.DataLoaderDataNodes.Contains(subDataNode))
                        {
                            if (dataNode.Type == DataNode.DataNodeType.Group && (dataNode as GroupDataNode).GroupedDataNodeRoot == subDataNode)
                            {
                                if (!resultDataLoaders.ContainsKey(subDataNode.Identity))
                                    resultDataLoaders[subDataNode.Identity] = new OOAdvantech.Collections.Generic.List<StorageDataLoader>();
                                if (!resultDataLoaders[subDataNode.Identity].Contains(subDataNode.DataSource.DataLoaders[dataLoaderMetadata.ObjectsContextIdentity] as StorageDataLoader))
                                    resultDataLoaders[subDataNode.Identity].Add(subDataNode.DataSource.DataLoaders[dataLoaderMetadata.ObjectsContextIdentity] as StorageDataLoader);

                                foreach (var storageCell in dataLoaderMetadata.StorageCells)
                                    if (!(storageCell is StorageCellReference))
                                        GetQueryResultDataLoaders(queryResultType, subDataNode, new StorageCellReference.StorageCellReferenceMetaData(storageCell), resultDataLoaders);
                            }
                            else
                            {
                                if (subDataNode.Type == DataNode.DataNodeType.Group)
                                {
                                    DataNode dataSourceDataNode = (subDataNode as GroupDataNode).GroupedDataNodeRoot;
                                    while (dataSourceDataNode.Type == DataNode.DataNodeType.Group)
                                        dataSourceDataNode = (dataSourceDataNode as GroupDataNode).GroupedDataNodeRoot;

                                    if (dataLoaderMetadata.RelatedStorageCells.ContainsKey(dataSourceDataNode.Identity))
                                    {
                                        foreach (var relatedStorageCell in dataLoaderMetadata.RelatedStorageCells[dataSourceDataNode.Identity])
                                        {
                                            foreach (var relationPartStorageCellsRefMetadata in relatedStorageCell.Value)
                                            {
                                                foreach (var storageCellsRefMetadata in relationPartStorageCellsRefMetadata.Value)
                                                {
                                                    if (!resultDataLoaders.ContainsKey(subDataNode.Identity))
                                                        resultDataLoaders[subDataNode.Identity] = new OOAdvantech.Collections.Generic.List<StorageDataLoader>();
                                                    if (!resultDataLoaders[subDataNode.Identity].Contains(subDataNode.DataSource.DataLoaders[dataLoaderMetadata.ObjectsContextIdentity] as StorageDataLoader))
                                                        resultDataLoaders[subDataNode.Identity].Add(subDataNode.DataSource.DataLoaders[dataLoaderMetadata.ObjectsContextIdentity] as StorageDataLoader);

                                                    if (!resultDataLoaders.ContainsKey(dataSourceDataNode.Identity))
                                                        resultDataLoaders[dataSourceDataNode.Identity] = new OOAdvantech.Collections.Generic.List<StorageDataLoader>();
                                                    if (!resultDataLoaders[dataSourceDataNode.Identity].Contains(dataSourceDataNode.DataSource.DataLoaders[storageCellsRefMetadata.StorageIdentity] as StorageDataLoader))
                                                        resultDataLoaders[dataSourceDataNode.Identity].Add(dataSourceDataNode.DataSource.DataLoaders[storageCellsRefMetadata.StorageIdentity] as StorageDataLoader);


                                                    GetQueryResultDataLoaders(queryResultType, dataSourceDataNode, storageCellsRefMetadata, resultDataLoaders);
                                                }
                                            }
                                        }
                                    }
                                    if (dataLoaderMetadata.RelatedMemoryCells.ContainsKey(dataSourceDataNode.Identity))
                                    {
                                        foreach (var memoryCell in dataLoaderMetadata.RelatedMemoryCells[dataSourceDataNode.Identity])
                                        {
                                            if (!resultDataLoaders.ContainsKey(subDataNode.Identity))
                                                resultDataLoaders[subDataNode.Identity] = new OOAdvantech.Collections.Generic.List<StorageDataLoader>();
                                            if (!resultDataLoaders[subDataNode.Identity].Contains(subDataNode.DataSource.DataLoaders[memoryCell.ObjectsContextIdentity] as StorageDataLoader))
                                                resultDataLoaders[subDataNode.Identity].Add(subDataNode.DataSource.DataLoaders[memoryCell.ObjectsContextIdentity] as StorageDataLoader);

                                            if (!resultDataLoaders.ContainsKey(dataSourceDataNode.Identity))
                                                resultDataLoaders[dataSourceDataNode.Identity] = new OOAdvantech.Collections.Generic.List<StorageDataLoader>();
                                            if (!resultDataLoaders[dataSourceDataNode.Identity].Contains(dataSourceDataNode.DataSource.DataLoaders[memoryCell.ObjectsContextIdentity] as StorageDataLoader))
                                                resultDataLoaders[dataSourceDataNode.Identity].Add(dataSourceDataNode.DataSource.DataLoaders[memoryCell.ObjectsContextIdentity] as StorageDataLoader);
                                            GetQueryResultDataLoaders(queryResultType, dataSourceDataNode, memoryCell, resultDataLoaders);
                                        }
                                    }

                                }
                                else
                                {
                                    if (dataLoaderMetadata.RelatedStorageCells.ContainsKey(subDataNode.Identity))
                                    {
                                        #region Get DataLoaders for related storage cells

                                        if (dataLoaderMetadata.RelatedStorageCells.ContainsKey(subDataNode.Identity))
                                        {
                                            foreach (var relatedStorageCell in dataLoaderMetadata.RelatedStorageCells[subDataNode.Identity])
                                            {
                                                foreach (var relationPartStorageCellsRefMetadata in relatedStorageCell.Value)
                                                {
                                                    foreach (var storageCellsRefMetadata in relationPartStorageCellsRefMetadata.Value)
                                                    {
                                                        if (!resultDataLoaders.ContainsKey(subDataNode.Identity))
                                                            resultDataLoaders[subDataNode.Identity] = new OOAdvantech.Collections.Generic.List<StorageDataLoader>();
                                                        if (!resultDataLoaders[subDataNode.Identity].Contains(subDataNode.DataSource.DataLoaders[storageCellsRefMetadata.StorageIdentity] as StorageDataLoader))
                                                            resultDataLoaders[subDataNode.Identity].Add(subDataNode.DataSource.DataLoaders[storageCellsRefMetadata.StorageIdentity] as StorageDataLoader);

                                                        GetQueryResultDataLoaders(queryResultType, subDataNode, storageCellsRefMetadata, resultDataLoaders);
                                                    }
                                                }
                                            }
                                        }

                                        #endregion
                                    }
                                    if (dataLoaderMetadata.RelatedMemoryCells.ContainsKey(subDataNode.Identity))
                                    {
                                        #region Get DataLoaders for related memory cells

                                        foreach (var memoryCell in dataLoaderMetadata.RelatedMemoryCells[subDataNode.Identity])
                                        {
                                            if (!resultDataLoaders.ContainsKey(subDataNode.Identity))
                                                resultDataLoaders[subDataNode.Identity] = new OOAdvantech.Collections.Generic.List<StorageDataLoader>();
                                            if (!resultDataLoaders[subDataNode.Identity].Contains(subDataNode.DataSource.DataLoaders[memoryCell.ObjectsContextIdentity] as StorageDataLoader))
                                                resultDataLoaders[subDataNode.Identity].Add(subDataNode.DataSource.DataLoaders[memoryCell.ObjectsContextIdentity] as StorageDataLoader);
                                            GetQueryResultDataLoaders(queryResultType, subDataNode, memoryCell, resultDataLoaders);
                                        }

                                        #endregion
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (var subDataNode in dataNode.SubDataNodes)
                    {
                        if (queryResultType.DataLoaderDataNodes.Contains(subDataNode))
                            GetQueryResultDataLoaders(queryResultType, distributedObjectQuery, subDataNode, resultDataLoaders);
                    }
                }

            }
        }

        // <MetaDataID>{5a7cb707-4bb8-4bd4-8cf5-ead4b4e382c6}</MetaDataID>
        /// <summary>
        /// Retrieves related DataLoaders which loads data from storageCells which are related to the storage cell of dataNodeStorageCellsRefMetadata parameter
        /// </summary>
        /// <param name="queryResultType">
        /// Defines the query result type
        /// </param>
        /// <param name="dataNode">
        /// Defines DataNode where has DataLoader that Loads data from storage cell of dataNodeStorageCellsRefMetadata parameter
        /// </param>
        /// <param name="dataNodeStorageCellsRefMetadata">
        /// Defines the storage cell where dataNode parameter DataLoader uses to loads data 
        /// </param>
        /// <param name="resultDataLoaders">
        /// Defines the collection where mechanism uses to add  the retrivied dataLoaders
        /// </param>
        private void GetQueryResultDataLoaders(QueryResultType queryResultType,
                                             DataNode dataNode,
                                            StorageCellReference.StorageCellReferenceMetaData dataNodeStorageCellsRefMetadata,
                                            Collections.Generic.Dictionary<Guid, Collections.Generic.List<StorageDataLoader>> resultDataLoaders)
        {

            foreach (var subDataNode in dataNode.SubDataNodes)
            {
                if (queryResultType.DataLoaderDataNodes.Contains(subDataNode))
                {
                    foreach (var dataLoaderMetadata in (dataNode.DataSource as StorageDataSource).DataLoadersMetadata.Values)
                    {

                        if (dataLoaderMetadata.RelatedStorageCells.ContainsKey(subDataNode.Identity) &&
                            dataLoaderMetadata.RelatedStorageCells[subDataNode.Identity].ContainsKey(dataNodeStorageCellsRefMetadata))
                        {
                            foreach (var relationPartStorageCellsRefMetadata in dataLoaderMetadata.RelatedStorageCells[subDataNode.Identity][dataNodeStorageCellsRefMetadata])
                            {
                                foreach (var relatedStorageCellsRefMetadata in relationPartStorageCellsRefMetadata.Value)
                                {
                                    if (!resultDataLoaders.ContainsKey(subDataNode.Identity))
                                        resultDataLoaders[subDataNode.Identity] = new OOAdvantech.Collections.Generic.List<StorageDataLoader>();
                                    if (!resultDataLoaders[subDataNode.Identity].Contains(subDataNode.DataSource.DataLoaders[relatedStorageCellsRefMetadata.StorageIdentity] as StorageDataLoader))
                                        resultDataLoaders[subDataNode.Identity].Add(subDataNode.DataSource.DataLoaders[relatedStorageCellsRefMetadata.StorageIdentity] as StorageDataLoader);
                                    GetQueryResultDataLoaders(queryResultType, subDataNode, relatedStorageCellsRefMetadata, resultDataLoaders);
                                }
                            }
                        }
                    }
                }
            }
        }


        /// <MetaDataID>{81d137d6-5af6-4edb-b8ce-65acc141015e}</MetaDataID>
        private void GetQueryResultDataLoaders(QueryResultType queryResultType,
                                            DataNode dataNode,
                                            MemoryCellReference memoryCellReference,
                                            Collections.Generic.Dictionary<Guid, Collections.Generic.List<StorageDataLoader>> resultDataLoaders)
        {

            foreach (var subDataNode in dataNode.SubDataNodes)
            {
                if (queryResultType.DataLoaderDataNodes.Contains(subDataNode))
                {
                    foreach (var dataLoaderMetadata in (dataNode.DataSource as StorageDataSource).DataLoadersMetadata.Values)
                    {

                        if (dataLoaderMetadata.RelatedMemoryCells.ContainsKey(subDataNode.Identity) &&
                            dataLoaderMetadata.MemoryCell != null &&
                            dataLoaderMetadata.MemoryCell.MemoryCellIdentity == memoryCellReference.MemoryCellIdentity &&
                            dataLoaderMetadata.MemoryCell.ObjectsContextIdentity == memoryCellReference.ObjectsContextIdentity)
                        {
                            foreach (var relatedMemoryCell in dataLoaderMetadata.RelatedMemoryCells[subDataNode.Identity])
                            {
                                if (!resultDataLoaders.ContainsKey(subDataNode.Identity))
                                    resultDataLoaders[subDataNode.Identity] = new OOAdvantech.Collections.Generic.List<StorageDataLoader>();
                                if (!resultDataLoaders[subDataNode.Identity].Contains(subDataNode.DataSource.DataLoaders[relatedMemoryCell.ObjectsContextIdentity] as StorageDataLoader))
                                    resultDataLoaders[subDataNode.Identity].Add(subDataNode.DataSource.DataLoaders[relatedMemoryCell.ObjectsContextIdentity] as StorageDataLoader);
                                GetQueryResultDataLoaders(queryResultType, subDataNode, relatedMemoryCell, resultDataLoaders);
                            }
                        }
                    }
                }
            }
        }

        #region RowRemove code
        ///// <MetaDataID>{19d06ab6-aeaa-4349-8860-8968237c15e8}</MetaDataID>
        //System.Collections.Generic.Dictionary<System.Data.DataRow, byte> RemovedRows = new System.Collections.Generic.Dictionary<System.Data.DataRow, byte>();
        ///// <MetaDataID>{cde305e9-7235-42b5-8814-92e6e8bf9019}</MetaDataID>
        //internal override void CancelRemoveRow(System.Data.DataRow row)
        //{
        //    if (RemovedRows.ContainsKey(row))
        //        RemovedRows.Remove(row);
        //}

        ///// <MetaDataID>{e9df77fa-5b42-47ea-9bee-9df25bf963cd}</MetaDataID>
        //internal override void RemoveRow(System.Data.DataRow row)
        //{
        //    RemovedRows[row] = 0; 
        //}
        ///// <MetaDataID>{04543e86-f650-4ba4-867b-fd5513394a4b}</MetaDataID>
        //public override bool IsRemovedRow(System.Data.DataRow row)
        //{
        //    return RemovedRows.ContainsKey(row);
        //}
        #endregion


        #region Creates data sources for DataNode tree


        /// <summary>
        /// Creates dataSource for value type DataNode
        /// </summary>
        /// <param name="valueTypeDataNode">
        /// Defines value type DataNode
        /// </param>
        /// <param name="referenceDataSource"><
        /// Defines the data source where system uses to find related data for new DataSource.
        /// referenceDataSource DataNode must be parentDataNode of value type dataNode parameter  
        /// /param>
        /// <returns> returns the new datasource </returns>
        /// <MetaDataID>{1a51d2db-6416-45cf-b8f7-097b66e911a6}</MetaDataID>
        public static DataSource CreateValueTypeDataSource(DataNode valueTypeDataNode, DataSource referenceDataSource)
        {
            return new StorageDataSource(valueTypeDataNode, new Dictionary<string, DataLoaderMetadata>((referenceDataSource as StorageDataSource).DataLoadersMetadata));
        }


        /// <summary>
        /// Builds data sources for the dataNode and related data nodes whith it
        /// </summary>
        /// <param name="dataNode">
        /// Method creates datasource for the parameter dataNode 
        /// </param>
        /// <param name="referenceDataSource">
        /// Defines the data source where system uses to find related data for new DataSource.
        /// referenceDataSource DataNode must be parentDataNode or subDataNode of dataNode parameter  
        /// </param>
        /// <MetaDataID>{81777f7c-a988-43b8-8553-018911c472aa}</MetaDataID>
        protected void CreateDataSources(DataNode dataNode, DataSource referenceDataSource)
        {
            if (dataNode.DataSource != null)
                return; // the data source already builded
            if (dataNode.AssignedMetaObject is MetaDataRepository.Attribute)
            {
                if (dataNode.Type == DataNode.DataNodeType.Object && referenceDataSource != null)
                    dataNode.DataSource = CreateValueTypeDataSource(dataNode, referenceDataSource);
                else
                {
                    if (dataNode.HasTimePeriodConstrain)
                        throw new System.Exception("You can,t apply 'TIMEPERIOD' keyword on Class primitive member");
                    return;
                }
            }
            else
            {
                if (dataNode.ObjectIDConstrainStorageCell != null)
                    dataNode.DataSource = CreateDataSourceFor(dataNode);
                else
                {
                    if (referenceDataSource == null)
                    {
                        if (dataNode.Classifier != null)
                        {
                            if (dataNode.Type == DataNode.DataNodeType.Group)
                            {
                                foreach (DataNode subDataNode in dataNode.RealSubDataNodes)
                                    if (subDataNode.Type == DataNode.DataNodeType.Object)
                                        CreateDataSources(subDataNode, null);
                            }
                            dataNode.DataSource = CreateDataSourceFor(dataNode);
                        }
                    }
                    else
                    {
                        if (dataNode.Classifier == null)
                            return;
                        Association association = null;
                        AssociationEnd associationEnd = null;
                        if (dataNode.IsParentDataNode(referenceDataSource.DataNode) && dataNode.AssignedMetaObject is AssociationEnd)
                        {
                            associationEnd = ((AssociationEnd)dataNode.AssignedMetaObject);
                            association = associationEnd.Association;
                        }
                        else if (referenceDataSource.DataNode.AssignedMetaObject is AssociationEnd)
                        {
                            associationEnd = (AssociationEnd)referenceDataSource.DataNode.AssignedMetaObject;
                            associationEnd = associationEnd.GetOtherEnd();
                            association = associationEnd.Association;
                        }
                        if (associationEnd != null)
                        {

                            if ((dataNode.Classifier.ClassHierarchyLinkAssociation == associationEnd.Association) || (dataNode.Name == association.Name && dataNode.Name != associationEnd.Name))
                                dataNode.DataSource = CreateRelationObjectDataSource(dataNode, referenceDataSource, associationEnd, QueryStorageIdentities);
                            else
                            {
                                dataNode.DataSource = CreateRelatedObjectDataSource(dataNode, referenceDataSource, associationEnd, QueryStorageIdentities);
                                if (dataNode.DataSource == null)
                                    return;
                            }
                        }
                        else
                            throw new System.Exception("Error on Data tree");
                    }
                }
            }


            #region Create data soure for all sub data nodes except group data nodes
            System.Collections.Generic.List<DataNode> subDataNodes = new System.Collections.Generic.List<DataNode>();
            do
            {
                foreach (DataNode subDataNode in new System.Collections.Generic.List<DataNode>(dataNode.RealSubDataNodes))
                {
                    if (subDataNodes.Contains(subDataNode))
                        continue;
                    subDataNodes.Add(subDataNode);
                    if (subDataNode.Type == DataNode.DataNodeType.Group)
                    {
                        foreach (DataNode groupedDataNode in subDataNode.SubDataNodes)
                            CreateDataSources(groupedDataNode, dataNode.DataSource);
                        continue;
                    }
                    if (referenceDataSource == null || subDataNode != referenceDataSource.DataNode)
                    {
                        if (dataNode.Classifier == null)
                            CreateDataSources(subDataNode, null);

                        else
                            CreateDataSources(subDataNode, dataNode.DataSource);
                    }
                }
                //foreach (DataNode subDataNode in new System.Collections.Generic.List<DataNode>(subDataNodes))
                //{
                //    if (subDataNode.ParentDataNode == null)
                //        subDataNodes.Remove(subDataNode);
                //}
            }
            while (subDataNodes.Count != dataNode.RealSubDataNodes.Count);
            #endregion

            #region Create data soure for group sub data nodes
            subDataNodes.Clear();
            do
            {
                foreach (DataNode subDataNode in new System.Collections.Generic.List<DataNode>(dataNode.SubDataNodes))
                {
                    if (subDataNodes.Contains(subDataNode))
                        continue;
                    if (subDataNode.Type != DataNode.DataNodeType.Group)
                    {
                        subDataNodes.Add(subDataNode);
                        continue;
                    }
                    else
                    {
                        subDataNodes.Add(subDataNode);
                        subDataNode.DataSource = CreateDataSourceFor(subDataNode);
                    }
                }
            }
            while (subDataNodes.Count != dataNode.SubDataNodes.Count);
            #endregion

            if (dataNode.RealParentDataNode != null && dataNode.RealParentDataNode.Classifier != null)
                CreateDataSources(dataNode.RealParentDataNode, dataNode.DataSource);
            if (dataNode.Type == DataNode.DataNodeType.Group)
                dataNode.DataSource = CreateDataSourceFor(dataNode);
            //    BuildDataSourceFor
        }


        /// <summary>
        /// Builds data sources for related DataNodes of dataNode
        /// </summary>
        /// <param name="dataNode">
        /// Method creates datasource for the parameter dataNode 
        /// </param>
        /// <param name="referenceDataSource">
        /// Defines the data source where system uses to find related data for new DataSource.
        /// referenceDataSource DataNode must be parentDataNode or subDataNode of dataNode parameter  
        /// </param>
        /// <MetaDataID>{8579327a-c375-4c17-af6b-71df9abb194f}</MetaDataID>
        protected void CreateDataSourcesForRelatedDataNode(DataNode dataNode)
        {
            //bool allObjectsInActiveMode = true;
            //if (dataNode.DataSource != null)
            //{
            //    foreach (DataLoaderMetadata dataLoaderMetadata in (dataNode.DataSource as StorageDataSource).DataLoadersMetadata.Values)
            //    {
            //        foreach (StorageCell storageCell in dataLoaderMetadata.StorageCells)
            //        {
            //            if (!storageCell.AllObjectsInActiveMode)
            //            {
            //                allObjectsInActiveMode = false;
            //                break;
            //            }
            //        }
            //    }
            //}
            ////if (!allObjectsInActiveMode)
            ////    dataNode.CreateOnConstructionSubDataNode();

            try
            {
                System.Collections.Generic.List<DataNode> subDataNodes = new System.Collections.Generic.List<DataNode>();
                do
                {
                    foreach (DataNode subDataNode in new System.Collections.Generic.List<DataNode>(dataNode.RealSubDataNodes))
                    {
                        if (subDataNodes.Contains(subDataNode))
                            continue;
                        subDataNodes.Add(subDataNode);
                        if (subDataNode.Type == DataNode.DataNodeType.Group)
                        {
                            foreach (DataNode groupedDataNode in subDataNode.SubDataNodes)
                                CreateDataSources(groupedDataNode, dataNode.DataSource);
                            continue;
                        }
                        if (dataNode.Classifier == null)
                            CreateDataSources(subDataNode, null);
                        else
                            CreateDataSources(subDataNode, dataNode.DataSource);
                    }
                    foreach (DataNode subDataNode in new System.Collections.Generic.List<DataNode>(subDataNodes))
                    {
                        if (subDataNode.ParentDataNode == null)
                            subDataNodes.Remove(subDataNode);
                    }
                }
                while (subDataNodes.Count != dataNode.RealSubDataNodes.Count);

                subDataNodes.Clear();
                do
                {
                    foreach (DataNode subDataNode in new System.Collections.Generic.List<DataNode>(dataNode.SubDataNodes))
                    {
                        if (subDataNodes.Contains(subDataNode))
                            continue;
                        if (subDataNode.Type != DataNode.DataNodeType.Group)
                        {
                            subDataNodes.Add(subDataNode);
                            continue;
                        }
                        else
                        {
                            subDataNodes.Add(subDataNode);
                            subDataNode.DataSource = CreateDataSourceFor(subDataNode);
                        }
                    }
                }
                while (subDataNodes.Count != dataNode.SubDataNodes.Count);
            }
            catch (System.Exception error)
            {
                throw;
            }
            if (dataNode.RealParentDataNode != null && dataNode.RealParentDataNode.Classifier != null)
                CreateDataSources(dataNode.RealParentDataNode, dataNode.DataSource);
            if (dataNode.Type == DataNode.DataNodeType.Group)
                dataNode.DataSource = CreateDataSourceFor(dataNode);
        }


        /// <summary>
        /// Creates DataSource for the DataNode of parameter dataNode
        /// </summary>
        /// <param name="dataNode">
        /// Defines the DataNode of new DataSource 
        /// </param>
        /// <returns>
        /// Return new DataSource
        /// </returns>
        /// <MetaDataID>{ef3477d6-296d-4925-9c6d-2c5678117833}</MetaDataID>
        public DataSource CreateDataSourceFor(DataNode dataNode)
        {
            if (ObjectsContext is MemoryObjectsContext)
            {
                System.Collections.Generic.Dictionary<string, DataLoaderMetadata> dataLoadersMetadata = new System.Collections.Generic.Dictionary<string, DataLoaderMetadata>();
                DataNode dataSourceDataNode = dataNode;
                if (dataNode.Type == DataNode.DataNodeType.Group)
                {

                    GroupDataNode groupDataNode = dataNode as GroupDataNode;
                    {
                        dataSourceDataNode = groupDataNode.GroupedDataNodeRoot;
                        while (dataSourceDataNode.Type == DataNode.DataNodeType.Group)
                            dataSourceDataNode = (dataSourceDataNode as GroupDataNode).GroupedDataNodeRoot;

                        if (dataNode.ParentDataNode != null || dataSourceDataNode.DataSource != null)
                            return new StorageDataSource(dataNode, (dataSourceDataNode.DataSource as StorageDataSource).DataLoadersMetadata);
                    }
                }
                foreach (MemoryCell memoryCell in (ObjectsContext as MemoryObjectsContext).GetMemoryCells(dataSourceDataNode))
                {
                    if (memoryCell is InProcessMemoryCell)
                    {
                        var dataLoaderMetadata = new DataLoaderMetadata(dataNode, memoryCell, ObjectsContext);
                        dataLoadersMetadata[memoryCell.ObjectsContextIdentity] = dataLoaderMetadata;
                        if (!(dataNode.ObjectQuery as ObjectsContextQuery).QueryStorageIdentities.Contains(memoryCell.ObjectsContextIdentity))
                            (dataNode.ObjectQuery as ObjectsContextQuery).QueryStorageIdentities.Add(memoryCell.ObjectsContextIdentity);
                    }
                    if (memoryCell is PartialLoadedMemoryCell)
                    {
                        foreach (StorageCell storageCell in (memoryCell as PartialLoadedMemoryCell).StorageCells)
                        {
                            DataLoaderMetadata dataLoaderMetadata = null;
                            if (!dataLoadersMetadata.TryGetValue(storageCell.StorageIdentity, out dataLoaderMetadata))
                            {
                                var openStorage = PersistenceLayer.ObjectStorage.OpenStorage(storageCell.StorageName, storageCell.StorageLocation, storageCell.StorageType);
                                dataLoaderMetadata = new DataLoaderMetadata(dataNode, memoryCell, openStorage);
                                dataLoadersMetadata[storageCell.StorageIdentity] = dataLoaderMetadata;
                                dataLoaderMetadata.AddStorageCell(storageCell);
                            }
                            else
                                dataLoaderMetadata.AddStorageCell(storageCell);
                            if (!(dataNode.ObjectQuery as ObjectsContextQuery).QueryStorageIdentities.Contains(memoryCell.ObjectsContextIdentity))
                                (dataNode.ObjectQuery as ObjectsContextQuery).QueryStorageIdentities.Add(memoryCell.ObjectsContextIdentity);
                        }
                    }
                    else
                    {
                        var dataLoaderMetadata = new DataLoaderMetadata(dataNode, memoryCell, ObjectsContext);
                        dataLoadersMetadata[memoryCell.ObjectsContextIdentity] = dataLoaderMetadata;
                        if (!(dataNode.ObjectQuery as ObjectsContextQuery).QueryStorageIdentities.Contains(memoryCell.ObjectsContextIdentity))
                            (dataNode.ObjectQuery as ObjectsContextQuery).QueryStorageIdentities.Add(memoryCell.ObjectsContextIdentity);
                    }
                }

                DataSource dataSource = new StorageDataSource(dataSourceDataNode, dataLoadersMetadata);
                if (dataNode.Type == DataNode.DataNodeType.Group)
                    return new StorageDataSource(dataNode, (dataSourceDataNode.DataSource as StorageDataSource).DataLoadersMetadata);
                else
                    return dataSource;

            }
            if (dataNode.ObjectIDConstrainStorageCell != null)
            {
                Collections.Generic.Set<MetaDataRepository.StorageCell> storageCells = new OOAdvantech.Collections.Generic.Set<MetaDataRepository.StorageCell>();
                storageCells.Add(dataNode.ObjectIDConstrainStorageCell);
                return new StorageDataSource(dataNode, GetDataLoadersMetaData(storageCells, dataNode));
            }
            if(dataNode.StorageCellIDsConstrain.Count>0)
            {
                foreach(var storageCellIdentity in dataNode.StorageCellIDsConstrain)
                {
                    var storageCellIdentityParts = storageCellIdentity.Split('/');
                    if(storageCellIdentityParts[0]==ObjectStorage.StorageMetaData.StorageIdentity)
                    {
                        int storageCellSerialNumber = int.Parse(storageCellIdentityParts[1]);
                        var storageCell = ObjectStorage.GetStorageCell(storageCellSerialNumber);
                        Collections.Generic.Set<MetaDataRepository.StorageCell> storageCells = new OOAdvantech.Collections.Generic.Set<MetaDataRepository.StorageCell>();
                        storageCells.Add(storageCell);
                        return new StorageDataSource(dataNode, GetDataLoadersMetaData(storageCells, dataNode));
                    }
                }
            }
            if (dataNode.Type == DataNode.DataNodeType.Group)
            {
                GroupDataNode groupDataNode = dataNode as GroupDataNode;
                if (groupDataNode.GroupedDataNodeRoot.Type == DataNode.DataNodeType.OjectAttribute)
                    return new StorageDataSource(dataNode, (groupDataNode.GroupedDataNodeRoot.ParentDataNode.DataSource as StorageDataSource).DataLoadersMetadata);
                else
                {
                    DataNode dataSourceDataNode = groupDataNode.GroupedDataNodeRoot;
                    while (dataSourceDataNode.Type == DataNode.DataNodeType.Group)
                        dataSourceDataNode = (dataSourceDataNode as GroupDataNode).GroupedDataNodeRoot;
                    if (dataNode.ParentDataNode == null && dataSourceDataNode.DataSource == null)
                        dataSourceDataNode.DataSource = CreateDataSourceFor(dataSourceDataNode);
                    return new StorageDataSource(dataNode, (dataSourceDataNode.DataSource as StorageDataSource).DataLoadersMetadata);
                }
            }
            if (dataNode.HasTimePeriodConstrain)
                return new StorageDataSource(dataNode, GetDataLoadersMetaData(ObjectStorage.GetStorageCells(dataNode.Classifier, dataNode.TimePeriodStartDate, dataNode.TimePeriodEndDate), dataNode));
            else
                return new StorageDataSource(dataNode, GetDataLoadersMetaData(ObjectStorage.GetStorageCells(dataNode.Classifier), dataNode));


        }




        /// <summary>
        /// This method transform the storage cells collection to DataLoadersMetaData Dictionary 
        /// </summary>
        /// <param name="storageCells">
        /// Defines the collection of StorageCells who mwthod transform to DataLoadersMetaData dictionary
        /// </param>
        /// <param name="dataNode">
        /// Defines the DataNode of StorageCells collection
        /// </param>
        /// <returns>
        /// Returns dictionary which corresponds ObjectsContextIdentity with related objects DataLoaderMetadata of objectsContext 
        /// </returns>
        /// <MetaDataID>{4e06913c-2b62-46c6-b013-1399a38678fa}</MetaDataID>
        public static OOAdvantech.Collections.Generic.Dictionary<string, DataLoaderMetadata> GetDataLoadersMetaData(Collections.Generic.Set<MetaDataRepository.StorageCell> storageCells, DataNode dataNode)
        {
            var dataLoadersMetadata = new OOAdvantech.Collections.Generic.Dictionary<string, DataLoaderMetadata>();

            foreach (StorageCell storageCell in storageCells)
            {
                if (!(storageCell.Type is DotNetMetaDataRepository.Class))
                {
                    if (storageCell.Type != null)
                    {
                        Type type = ModulePublisher.ClassRepository.GetType(storageCell.Type.FullName, storageCell.Type.ImplementationUnit.Identity.ToString());

                        if (type == null)
                            throw new TypeLoadException(string.Format("{0},{1}", storageCell.Type.FullName, storageCell.Type.ImplementationUnit.Identity.ToString()));

                        MetaDataRepository.Classifier _class = DotNetMetaDataRepository.Type.GetClassifierObject(type);
                        long count = _class.Features.Count;
                    }
                }
                string storageIdentity = null;// storageCell.StorageIntentity;
                string storageName = null;
                string storageLocation = null;
                string storageType = null;



                storageCell.GetStorageConnectionData(out storageIdentity, out storageName, out storageLocation, out storageType);

                if (!dataLoadersMetadata.ContainsKey(storageIdentity))
                {
                    if (!(dataNode.ObjectQuery as ObjectsContextQuery).QueryStorageIdentities.Contains(storageIdentity))
                        (dataNode.ObjectQuery as ObjectsContextQuery).QueryStorageIdentities.Add(storageIdentity);// = (DataNode.ObjectQuery as OQLStatement).QueryStorageIdentities.Count;

                    dataLoadersMetadata[storageIdentity] = new DataLoaderMetadata(dataNode, (dataNode.ObjectQuery as ObjectsContextQuery).QueryStorageIdentities.IndexOf(storageIdentity), storageIdentity, storageName, storageLocation, storageType);
                }
                dataLoadersMetadata[storageIdentity].AddStorageCell(storageCell);
            }
            return dataLoadersMetadata;
        }



        /// <summary>
        /// Creates a DataSource for relation objects DataNode.
        /// </summary>
        /// <param name="relationObjectsDataNode">
        /// Defines the DataNode for relation objects
        /// </param>
        /// <param name="referenceDatasource">
        /// Defines the DataSource with source objects  
        /// </param>
        /// <param name="associationEnd">
        /// Defines the type of relation between the source objects and related objects
        /// </param>
        /// <param name="queryStorageIdentities">
        /// Defines a collection where method adds the identities of new storages that participate in object query  
        /// </param>
        /// <returns>
        /// return the DataSource of relation objects 
        /// </returns>
        /// <MetaDataID>{ab2e2a2c-31e1-4baf-97e1-1b26d9568977}</MetaDataID>
        public static DataSource CreateRelationObjectDataSource(DataNode relationObjectsDataNode, DataSource referenceDataSource, AssociationEnd associationEnd, System.Collections.Generic.List<string> queryStorageIdentities)
        {
            if ((referenceDataSource as StorageDataSource).DataLoadersMetadata == null)
                throw new System.Exception("Error on Data tree");

            OOAdvantech.Collections.Generic.Dictionary<string, DataLoaderMetadata> relatedObjectsDataLoaderMetadata = GetRelationObjectsDataLoadersMetada(relationObjectsDataNode, referenceDataSource, associationEnd, queryStorageIdentities);
            StorageDataSource storageDataSource = new StorageDataSource(relationObjectsDataNode, relatedObjectsDataLoaderMetadata);
            return storageDataSource;

        }

        /// <summary>
        /// This method uses the referenceDataSource metadata and associationEnd to get DataLoaderMetadata for relatedObjectsDataNode
        /// </summary>
        /// <param name="relationObjectsDataNode">
        /// Defines the DataNode of relation objects
        /// </param>
        /// <param name="referenceDataSource">
        /// Defines the DataSource with source objects 
        /// </param>
        /// <param name="associationEnd">
        /// Defines the type of relation of relation objects
        /// </param>
        /// <param name="queryStorageIdentities">
        /// Defines a collection where method adds the identities of new storages that participate in object query  
        /// </param>
        /// <returns>
        /// Returns dictionary which corresponds ObjectsContextIdentity with relation objects DataLoaderMetadata of objectsContext 
        /// </returns>
        /// <MetaDataID>{46a69137-ff17-4091-88e4-d7a0cf5d6e01}</MetaDataID>
        public static OOAdvantech.Collections.Generic.Dictionary<string, DataLoaderMetadata> GetRelationObjectsDataLoadersMetada(DataNode relationObjectsDataNode, DataSource referenceDataSource, AssociationEnd associationEnd, System.Collections.Generic.List<string> queryStorageIdentities)
        {
            OOAdvantech.Collections.Generic.Dictionary<string, DataLoaderMetadata> relatedObjectsDataLoaderMetadata = new OOAdvantech.Collections.Generic.Dictionary<string, DataLoaderMetadata>();

            System.Collections.Generic.List<string> subDataNodeDataloadersWithParentOutStorageRelations = new System.Collections.Generic.List<string>();
            if (referenceDataSource.DataNode == relationObjectsDataNode.ParentDataNode)
                foreach (System.Collections.Generic.KeyValuePair<string, DataLoaderMetadata> entry in (referenceDataSource as StorageDataSource).DataLoadersMetadata)
                {
                    DataLoaderMetadata sourceDataLoaderMetadata = entry.Value;

                    string storageName = null;
                    string storageLocation = null;
                    string storageType = null;
                    string storageIdentity2 = null;


                    entry.Value.StorageCells[0].GetStorageConnectionData(out storageIdentity2, out storageName, out storageLocation, out storageType);
                    PersistenceLayer.ObjectStorage objectStorage = PersistenceLayer.ObjectStorage.OpenStorage(storageName, storageLocation, storageType);

                    Collections.Generic.Set<MetaDataRepository.RelatedStorageCell> linkedStorageCells = null;
                    if (associationEnd.IsRoleA)
                        linkedStorageCells = objectStorage.GetRelationObjectsStorageCells(associationEnd.Association, new Collections.Generic.Set<MetaDataRepository.StorageCell>(entry.Value.StorageCells), MetaDataRepository.Roles.RoleB);
                    else
                        linkedStorageCells = objectStorage.GetRelationObjectsStorageCells(associationEnd.Association, new Collections.Generic.Set<MetaDataRepository.StorageCell>(entry.Value.StorageCells), MetaDataRepository.Roles.RoleA);

                    sourceDataLoaderMetadata.AddRelatedStorageCell(relationObjectsDataNode, linkedStorageCells);

                    foreach (RelatedStorageCell relatedStorageCell in linkedStorageCells)
                    {
                        StorageCell storageCell = relatedStorageCell.StorageCell;
                        string storageIdentity = null;
                        storageCell.GetStorageConnectionData(out storageIdentity, out storageName, out storageLocation, out storageType);

                        if (entry.Key != storageIdentity)
                        {
                            (referenceDataSource as StorageDataSource).DataLoadersMetadata[entry.Key].HasOutStorageRelationsWithSubDataNode(relationObjectsDataNode);
                            subDataNodeDataloadersWithParentOutStorageRelations.Add(storageIdentity);
                        }
                        if (!relatedObjectsDataLoaderMetadata.ContainsKey(storageIdentity))
                        {
                            if (!queryStorageIdentities.Contains(storageIdentity))
                                queryStorageIdentities.Add(storageIdentity);//] = QueryStorageIdentities.Count;
                            relatedObjectsDataLoaderMetadata[storageIdentity] = new DataLoaderMetadata(relationObjectsDataNode, queryStorageIdentities.IndexOf(storageIdentity), storageIdentity, storageName, storageLocation, storageType);
                        }
                        if (!string.IsNullOrEmpty(relationObjectsDataNode.OfTypeFilterClassifierFullName))
                        {
                            if (storageCell.Type.FullName == relationObjectsDataNode.OfTypeFilterClassifierFullName)
                            {
                                relatedObjectsDataLoaderMetadata[storageIdentity].AddStorageCell(storageCell);
                            }
                            else
                            {
                                foreach (MetaDataRepository.Classifier classifier in storageCell.Type.GetAllGeneralClasifiers())
                                {
                                    if (classifier.FullName == relationObjectsDataNode.OfTypeFilterClassifierFullName)
                                    {
                                        relatedObjectsDataLoaderMetadata[storageIdentity].AddStorageCell(storageCell);
                                        break;
                                    }
                                }
                            }
                        }
                        else
                            relatedObjectsDataLoaderMetadata[storageIdentity].AddStorageCell(storageCell);
                    }
                }
            else
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DataLoaderMetadata> entry in (referenceDataSource as StorageDataSource).DataLoadersMetadata)
                {

                    string storageName = null;
                    string storageLocation = null;
                    string storageType = null;
                    string storageIdentity2 = null;
                    DataLoaderMetadata sourceDataLoaderMetadata = entry.Value;
                    entry.Value.StorageCells[0].GetStorageConnectionData(out storageIdentity2, out storageName, out storageLocation, out storageType);
                    PersistenceLayer.ObjectStorage objectStorage = PersistenceLayer.ObjectStorage.OpenStorage(storageName, storageLocation, storageType);

                    Collections.Generic.Set<MetaDataRepository.RelatedStorageCell> linkedStorageCells = null;
                    if (associationEnd.IsRoleA)
                        linkedStorageCells = objectStorage.GetRelationObjectsStorageCells(associationEnd.Association, new Collections.Generic.Set<MetaDataRepository.StorageCell>(entry.Value.StorageCells), MetaDataRepository.Roles.RoleA);
                    else
                        linkedStorageCells = objectStorage.GetRelationObjectsStorageCells(associationEnd.Association, new Collections.Generic.Set<MetaDataRepository.StorageCell>(entry.Value.StorageCells), MetaDataRepository.Roles.RoleB);

                    sourceDataLoaderMetadata.AddRelatedStorageCell(relationObjectsDataNode, linkedStorageCells);

                    foreach (RelatedStorageCell relatedStorageCell in linkedStorageCells)
                    {
                        StorageCell storageCell = relatedStorageCell.StorageCell;
                        string storageIdentity = null;
                        storageCell.GetStorageConnectionData(out storageIdentity, out storageName, out storageLocation, out storageType);
                        if (entry.Key != storageIdentity)
                        {

                            (referenceDataSource as StorageDataSource).DataLoadersMetadata[entry.Key].HasOutStorageRelationsWithSubDataNode(relationObjectsDataNode);
                            subDataNodeDataloadersWithParentOutStorageRelations.Add(storageIdentity);
                        }
                        if (!relatedObjectsDataLoaderMetadata.ContainsKey(storageIdentity))
                        {
                            if (!queryStorageIdentities.Contains(storageIdentity))
                                queryStorageIdentities.Add(storageIdentity);//] = QueryStorageIdentities.Count;
                        }
                        if (!string.IsNullOrEmpty(relationObjectsDataNode.OfTypeFilterClassifierFullName))
                        {
                            if (storageCell.Type.FullName == relationObjectsDataNode.OfTypeFilterClassifierFullName)
                            {
                                relatedObjectsDataLoaderMetadata[storageIdentity].AddStorageCell(storageCell);
                            }
                            else
                            {
                                foreach (MetaDataRepository.Classifier classifier in storageCell.Type.GetAllGeneralClasifiers())
                                {
                                    if (classifier.FullName == relationObjectsDataNode.OfTypeFilterClassifierFullName)
                                    {
                                        relatedObjectsDataLoaderMetadata[storageIdentity].AddStorageCell(storageCell);
                                        break;
                                    }
                                }
                            }
                        }
                        else
                            relatedObjectsDataLoaderMetadata[storageIdentity].AddStorageCell(storageCell);
                    }
                    if (linkedStorageCells.Count == 0)
                        relatedObjectsDataLoaderMetadata[entry.Value.ObjectsContextIdentity] = new DataLoaderMetadata(relationObjectsDataNode, queryStorageIdentities.IndexOf(entry.Value.ObjectsContextIdentity), entry.Value.ObjectsContextIdentity, entry.Value.StorageName, entry.Value.StorageLocation, entry.Value.StorageType);
                }
            }
            foreach (string storageIdentity in subDataNodeDataloadersWithParentOutStorageRelations)
                relatedObjectsDataLoaderMetadata[storageIdentity].HasOutStorageRelationsWithParent = true;

            return relatedObjectsDataLoaderMetadata;
        }


        /// <summary>
        /// Creates a DataSource for related objects DataNode.
        /// </summary>
        /// <param name="relatedObjectsDataNode">
        /// Defines the DataNode for related objects
        /// </param>
        /// <param name="referenceDatasource">
        /// Defines the DataSource with source objects  
        /// </param>
        /// <param name="associationEnd">
        /// Defines the type of relation between the source objects and related objects
        /// </param>
        /// <param name="queryStorageIdentities">
        /// Defines a collection where method adds the identities of new storages that participate in object query  
        /// </param>
        /// <returns>
        /// return the DataSource of related objects 
        /// </returns>
        /// <MetaDataID>{12bd63e1-4ad2-4b91-b6c5-700c982171cf}</MetaDataID>
        public static DataSource CreateRelatedObjectDataSource(DataNode relatedObjectsDataNode, DataSource referenceDatasource, AssociationEnd associationEnd, System.Collections.Generic.List<string> queryStorageIdentities)
        {
            if ((referenceDatasource as StorageDataSource).DataLoadersMetadata == null)
                throw new System.Exception("Error on Data tree");



            //DataNode pathDataNode = relatedObjectDataNode.ParentDataNode;

            //DataNode subDataNode = null;
            //if (relatedObjectDataNode.IsParentDataNode(referenceDatasource.DataNode))
            //    subDataNode = relatedObjectDataNode;
            //if (referenceDatasource.DataNode.ParentDataNode == relatedObjectDataNode)
            //    subDataNode = referenceDatasource.DataNode;

            OOAdvantech.Collections.Generic.Dictionary<string, DataLoaderMetadata> relatedObjectsDataLoaderMetadata = GetRelatedDataLoadersMetada(relatedObjectsDataNode, referenceDatasource, associationEnd, queryStorageIdentities);
            if (relatedObjectsDataLoaderMetadata.Count == 0)
                return new StorageDataSource(relatedObjectsDataNode);
            StorageDataSource storageDataSource = new StorageDataSource(relatedObjectsDataNode, relatedObjectsDataLoaderMetadata);
            return storageDataSource;
        }


        /// <summary>
        /// This method uses the referenceDataSource metadata and associationEnd to get DataLoaderMetadata for relatedObjectsDataNode
        /// </summary>
        /// <param name="relatedObjectsDataNode">
        /// Defines the DataNode of related objects
        /// </param>
        /// <param name="referenceDataSource">
        /// Defines the DataSource with source objects 
        /// </param>
        /// <param name="associationEnd">
        /// Defines the type of relation between the source objects and related objects
        /// </param>
        /// <param name="queryStorageIdentities">
        /// Defines a collection where method adds the identities of new storages that participate in object query
        /// </param>
        /// <returns>
        /// Returns dictionary which corresponds ObjectsContextIdentity with related objects DataLoaderMetadata of objectsContext 
        /// </returns>
        /// <MetaDataID>{140bf24f-2f0a-4978-b4f8-37958e18c94a}</MetaDataID>
        public static OOAdvantech.Collections.Generic.Dictionary<string, DataLoaderMetadata> GetRelatedDataLoadersMetada(
                                                                                    DataNode relatedObjectsDataNode,
                                                                                    DataSource referenceDataSource,
                                                                                    AssociationEnd associationEnd,
                                                                                    System.Collections.Generic.List<string> queryStorageIdentities)
        {
            DataNode subDataNode = null;
            System.Collections.Generic.List<string> outStorageRelations = new System.Collections.Generic.List<string>();

            if (relatedObjectsDataNode.IsParentDataNode(referenceDataSource.DataNode))
                subDataNode = relatedObjectsDataNode;

            if (referenceDataSource.DataNode.ParentDataNode == relatedObjectsDataNode)
                subDataNode = referenceDataSource.DataNode;

            if (subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd &&
                  (((subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany &&
                  (subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association != subDataNode.ParentDataNode.Classifier.ClassHierarchyLinkAssociation &&
                  (subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association != subDataNode.Classifier.ClassHierarchyLinkAssociation) ||
                  (subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.Specializations.Count > 0))
                subDataNode.ThroughRelationTable = true;


            //if (relatedObjectDataNode.ObjectQuery is StorageObjectQuery && 
            //    (relatedObjectDataNode.ObjectQuery as StorageObjectQuery).ObjectsContext is MemoryObjectsContext && 
            //    relatedObjectDataNode.FullName == "IClient.Orders.OrderDetails.Price")
            //{
            //    relatedObjectDataNode.ThroughRelationTable = true;
            //}


            //relatedObjectsDataLoaderMetadata defines a dictionary which corresponds ObjectsContextIdentity with objectsContext related objects DataLoaderMetadata
            OOAdvantech.Collections.Generic.Dictionary<string, DataLoaderMetadata> relatedObjectsDataLoaderMetadata = new OOAdvantech.Collections.Generic.Dictionary<string, DataLoaderMetadata>();

            foreach (string sourceObjectsContextIdentity in new System.Collections.Generic.List<string>((referenceDataSource as StorageDataSource).DataLoadersMetadata.Keys))
            {
                DataLoaderMetadata sourceDataLoaderMetadata = (referenceDataSource as StorageDataSource).DataLoadersMetadata[sourceObjectsContextIdentity];
                if (sourceObjectsContextIdentity.IndexOf("OnMemory:") == 0)
                {
                    if (sourceDataLoaderMetadata.MemoryCell is OutProcessMemoryCell || sourceDataLoaderMetadata.MemoryCell is InProcessMemoryCell)
                    {
                        MemoryObjectsContext objectsContext = null;
                        if (sourceDataLoaderMetadata.MemoryCell is OutProcessMemoryCell)
                            objectsContext = ((relatedObjectsDataNode.ObjectQuery as ObjectsContextQuery).ObjectsContext as MemoryObjectsContext).GetOutProcessMemoryObjectsCotext((sourceDataLoaderMetadata.MemoryCell as OutProcessMemoryCell).Channeluri, relatedObjectsDataNode.ObjectQuery.QueryIdentity);
                        else
                            objectsContext = (relatedObjectsDataNode.ObjectQuery as ObjectsContextQuery).ObjectsContext as MemoryObjectsContext;


                        List<RelatedMemoryCellData> relatedMemoryCells = objectsContext.GetRelatedMemoryCells(sourceDataLoaderMetadata.MemoryCell, associationEnd.Identity, relatedObjectsDataNode.Identity, ObjectData.GetLazyFetchingMembers(relatedObjectsDataNode.Classifier, relatedObjectsDataNode.SubDataNodes), ref queryStorageIdentities);
                        foreach (var relatedMemoryCell in relatedMemoryCells)
                        {
                            MemoryCell memoryCell = null;
                            ///TODO να γραφτει test case όπου επιστρέφεται OutProcessMemoryCell να αναφέρεται σε MemoryCell με MemoryContext να ανίκει την main query 
                            if (((relatedObjectsDataNode.ObjectQuery as ObjectsContextQuery).ObjectsContext as MemoryObjectsContext).MemoryCells.ContainsKey(relatedMemoryCell.MemoryCell.MemoryCellIdentity))
                                memoryCell = ((relatedObjectsDataNode.ObjectQuery as ObjectsContextQuery).ObjectsContext as MemoryObjectsContext).MemoryCells[relatedMemoryCell.MemoryCell.MemoryCellIdentity];
                            else
                                memoryCell = relatedMemoryCell.MemoryCell;

                            if (sourceDataLoaderMetadata.ObjectsContextIdentity != memoryCell.ObjectsContextIdentity)
                            {
                                subDataNode.ThroughRelationTable = true;
                                if (subDataNode == relatedObjectsDataNode)
                                    (referenceDataSource as StorageDataSource).DataLoadersMetadata[sourceObjectsContextIdentity].HasOutStorageRelationsWithSubDataNode(relatedObjectsDataNode);
                                else
                                    (referenceDataSource as StorageDataSource).DataLoadersMetadata[sourceObjectsContextIdentity].HasOutStorageRelationsWithParent = true;
                                outStorageRelations.Add(memoryCell.ObjectsContextIdentity);
                            }

                            if (!sourceDataLoaderMetadata.RelatedMemoryCells.ContainsKey(relatedObjectsDataNode.Identity))
                                sourceDataLoaderMetadata.RelatedMemoryCells[relatedObjectsDataNode.Identity] = new System.Collections.Generic.List<MemoryCellReference>();
                            sourceDataLoaderMetadata.RelatedMemoryCells[relatedObjectsDataNode.Identity].Add(new MemoryCellReference(memoryCell));

                            if (memoryCell is InProcessMemoryCell)
                            {
                                var dataLoaderMetadata = new DataLoaderMetadata(relatedObjectsDataNode, memoryCell, (relatedObjectsDataNode.ObjectQuery as ObjectsContextQuery).ObjectsContext);
                                relatedObjectsDataLoaderMetadata[memoryCell.ObjectsContextIdentity] = dataLoaderMetadata;
                                if (!(relatedObjectsDataNode.ObjectQuery as ObjectsContextQuery).QueryStorageIdentities.Contains(memoryCell.ObjectsContextIdentity))
                                    (relatedObjectsDataNode.ObjectQuery as ObjectsContextQuery).QueryStorageIdentities.Add(memoryCell.ObjectsContextIdentity);

                            }
                            else if (memoryCell is PartialLoadedMemoryCell)
                            {
                                foreach (StorageCell storageCell in (memoryCell as PartialLoadedMemoryCell).StorageCells)
                                {
                                    DataLoaderMetadata dataLoaderMetadata = null;
                                    if (!relatedObjectsDataLoaderMetadata.TryGetValue(storageCell.StorageIdentity, out dataLoaderMetadata))
                                    {
                                        var openStorage = PersistenceLayer.ObjectStorage.OpenStorage(storageCell.StorageName, storageCell.StorageLocation, storageCell.StorageType);
                                        dataLoaderMetadata = new DataLoaderMetadata(relatedObjectsDataNode, memoryCell, openStorage);
                                        relatedObjectsDataLoaderMetadata[storageCell.StorageIdentity] = dataLoaderMetadata;
                                        dataLoaderMetadata.AddStorageCell(storageCell);
                                    }
                                    else
                                        dataLoaderMetadata.AddStorageCell(storageCell);


                                    if (!(relatedObjectsDataNode.ObjectQuery as ObjectsContextQuery).QueryStorageIdentities.Contains(memoryCell.ObjectsContextIdentity))
                                        (relatedObjectsDataNode.ObjectQuery as ObjectsContextQuery).QueryStorageIdentities.Add(memoryCell.ObjectsContextIdentity);
                                }
                            }
                            else
                            {
                                var dataLoaderMetadata = new DataLoaderMetadata(relatedObjectsDataNode, memoryCell, (relatedObjectsDataNode.ObjectQuery as ObjectsContextQuery).ObjectsContext);

                                relatedObjectsDataLoaderMetadata[memoryCell.ObjectsContextIdentity] = dataLoaderMetadata;
                                if (!(relatedObjectsDataNode.ObjectQuery as ObjectsContextQuery).QueryStorageIdentities.Contains(memoryCell.ObjectsContextIdentity))
                                    (relatedObjectsDataNode.ObjectQuery as ObjectsContextQuery).QueryStorageIdentities.Add(memoryCell.ObjectsContextIdentity);

                            }

                        }
                    }
                    continue;
                }
                if (sourceDataLoaderMetadata.StorageCells.Count == 0)
                {
                    if (!relatedObjectsDataNode.MembersFetchingObjectActivation && !relatedObjectsDataLoaderMetadata.ContainsKey(sourceDataLoaderMetadata.ObjectsContextIdentity))
                        relatedObjectsDataLoaderMetadata[sourceDataLoaderMetadata.ObjectsContextIdentity] = new DataLoaderMetadata(relatedObjectsDataNode.HeaderDataNode.GetDataNode(sourceDataLoaderMetadata.DataNodeIdentity), queryStorageIdentities.IndexOf(sourceDataLoaderMetadata.ObjectsContextIdentity), sourceDataLoaderMetadata.ObjectsContextIdentity, sourceDataLoaderMetadata.StorageName, sourceDataLoaderMetadata.StorageLocation, sourceDataLoaderMetadata.StorageType);

                    continue;
                }
                string storageName = null;
                string storageLocation = null;
                string storageType = null;
                string storageIdentity2 = null;


                sourceDataLoaderMetadata.StorageCells[0].GetStorageConnectionData(out storageIdentity2, out storageName, out storageLocation, out storageType);
                PersistenceLayer.ObjectStorage objectStorage = PersistenceLayer.ObjectStorage.OpenStorage(storageName, storageLocation, storageType);
                Set<RelatedStorageCell> linkedStorageCells = objectStorage.GetLinkedStorageCells(associationEnd, relatedObjectsDataNode.ParentDataNode.ValueTypePath, new OOAdvantech.Collections.Generic.Set<StorageCell>(sourceDataLoaderMetadata.StorageCells), relatedObjectsDataNode.Classifier.Identity.ToString());
                sourceDataLoaderMetadata.AddRelatedStorageCell(relatedObjectsDataNode, linkedStorageCells);
                if (relatedObjectsDataNode.Recursive)
                {
                    var recursiveSteps = relatedObjectsDataNode.RecursiveSteps;
                }
                if (linkedStorageCells.Count > 0)
                {
                    foreach (MetaDataRepository.RelatedStorageCell relatedStorageCell in linkedStorageCells)
                    {
                        if (relatedObjectsDataNode.ParentDataNode == referenceDataSource.DataNode)
                            relatedObjectsDataNode.ThroughRelationTable |= relatedStorageCell.ThrougthRelationTable;
                        if (referenceDataSource.DataNode.ParentDataNode == relatedObjectsDataNode)
                            referenceDataSource.DataNode.ThroughRelationTable |= relatedStorageCell.ThrougthRelationTable;
                        StorageCell storageCell = relatedStorageCell.StorageCell;

                        string storageIdentity = null;
                        storageCell.GetStorageConnectionData(out storageIdentity, out storageName, out storageLocation, out storageType);

                        if (sourceObjectsContextIdentity != storageIdentity)
                        {
                            if (!queryStorageIdentities.Contains(storageIdentity))
                                queryStorageIdentities.Add(storageIdentity);//] = QueryStorageIdentities.Count;
                            if (subDataNode == relatedObjectsDataNode)
                                (referenceDataSource as StorageDataSource).DataLoadersMetadata[sourceObjectsContextIdentity].HasOutStorageRelationsWithSubDataNode(relatedObjectsDataNode);
                            else
                                (referenceDataSource as StorageDataSource).DataLoadersMetadata[sourceObjectsContextIdentity].HasOutStorageRelationsWithParent = true;

                            outStorageRelations.Add(storageIdentity);

                            if (!(referenceDataSource as StorageDataSource).DataLoadersMetadata.ContainsKey(storageIdentity))
                                (referenceDataSource as StorageDataSource).DataLoadersMetadata[storageIdentity] = new DataLoaderMetadata(referenceDataSource.DataNode, queryStorageIdentities.IndexOf(storageIdentity), storageIdentity, storageName, storageLocation, storageType);

                            bool exist = false;
                            StorageCellReference.StorageCellReferenceMetaData rootStorageCellReferenceMetaData = new StorageCellReference.StorageCellReferenceMetaData(relatedStorageCell.RootStorageCell);
                            foreach (MetaDataRepository.StorageCellReference.StorageCellReferenceMetaData storageCellReferenceMetaData in (referenceDataSource as StorageDataSource).DataLoadersMetadata[storageIdentity].StorageCellReferencesMetaData)
                            {
                                if (storageCellReferenceMetaData.SerialNumber == relatedStorageCell.RootStorageCell.SerialNumber &&
                                    storageCellReferenceMetaData.StorageIdentity == relatedStorageCell.RootStorageCell.StorageIdentity)
                                {
                                    rootStorageCellReferenceMetaData = storageCellReferenceMetaData;
                                    exist = true;
                                    break;
                                }
                            }
                            if (!exist)
                            {
                                rootStorageCellReferenceMetaData = new StorageCellReference.StorageCellReferenceMetaData(relatedStorageCell.RootStorageCell);
                                (referenceDataSource as StorageDataSource).DataLoadersMetadata[storageIdentity].StorageCellReferencesMetaData.Add(new StorageCellReference.StorageCellReferenceMetaData(relatedStorageCell.RootStorageCell));
                            }
                            (referenceDataSource as StorageDataSource).DataLoadersMetadata[storageIdentity].AddRelatedStorageCell(relatedObjectsDataNode, relatedStorageCell);
                        }
                        if (!relatedObjectsDataLoaderMetadata.ContainsKey(storageIdentity))
                        {
                            if (!queryStorageIdentities.Contains(storageIdentity))
                                queryStorageIdentities.Add(storageIdentity);//] = QueryStorageIdentities.Count;
                            relatedObjectsDataLoaderMetadata[storageIdentity] = new DataLoaderMetadata(relatedObjectsDataNode, queryStorageIdentities.IndexOf(storageIdentity), storageIdentity, storageName, storageLocation, storageType);
                        }

                        if (!string.IsNullOrEmpty(relatedObjectsDataNode.OfTypeFilterClassifierFullName))
                        {
                            if (storageCell.Type.FullName == relatedObjectsDataNode.OfTypeFilterClassifierFullName)
                            {
                                relatedObjectsDataLoaderMetadata[storageIdentity].AddStorageCell(storageCell);
                            }
                            else
                            {
                                foreach (MetaDataRepository.Classifier classifier in storageCell.Type.GetAllGeneralClasifiers())
                                {
                                    if (classifier.FullName == relatedObjectsDataNode.OfTypeFilterClassifierFullName)
                                    {
                                        relatedObjectsDataLoaderMetadata[storageIdentity].AddStorageCell(storageCell);
                                        break;
                                    }
                                }
                            }
                        }
                        else
                            relatedObjectsDataLoaderMetadata[storageIdentity].AddStorageCell(storageCell);
                    }
                }
                else
                {

                    string storageIdentity = sourceObjectsContextIdentity;
                    if (!relatedObjectsDataLoaderMetadata.ContainsKey(storageIdentity))
                        relatedObjectsDataLoaderMetadata[storageIdentity] = new DataLoaderMetadata(relatedObjectsDataNode, queryStorageIdentities.IndexOf(storageIdentity), storageIdentity, storageName, storageLocation, storageType);
                }
            }

            foreach (string storageIdentity in outStorageRelations)
            {
                if (subDataNode == relatedObjectsDataNode)
                    relatedObjectsDataLoaderMetadata[storageIdentity].HasOutStorageRelationsWithParent = true;
                else
                    relatedObjectsDataLoaderMetadata[storageIdentity].HasOutStorageRelationsWithSubDataNode(referenceDataSource.DataNode);
            }
            return relatedObjectsDataLoaderMetadata;
        }

        #endregion

        /// <MetaDataID>{fc7b5b34-c0da-4e67-a5cd-39b25a7ad51e}</MetaDataID>
        public System.Collections.Generic.List<string> QueryStorageIdentities = new System.Collections.Generic.List<string>();


        //relatedObjectDataNode, dataNode
        //private static OOAdvantech.Collections.Generic.Dictionary<string, DataLoaderMetadata> GetRelatedDataLoadersMetadata(DataNode relatedObjectDataNode,
        //    DataNode dataNode,
        //    AssociationEnd associationEnd,
        //    DataLoaderMetadata sourceDataLoaderMetadata,
        //    System.Collections.Generic.List<string> queryStorageIdentities,
        //    System.Collections.Generic.List<string> outStorageRelations)
        //{

        //    System.Collections.Generic.Dictionary<object, ObjectData> objectsMap = new System.Collections.Generic.Dictionary<object, ObjectData>();
        //    OOAdvantech.Collections.Generic.Dictionary<string, DataLoaderMetadata> relatedDataLoadersMetadata = new Dictionary<string, DataLoaderMetadata>();
        //    List<StorageCell> sourceStorageCells = new List<StorageCell>();
        //    OOAdvantech.Collections.Generic.List<ObjectData> subDataNodeObjects = new OOAdvantech.Collections.Generic.List<ObjectData>();
        //    foreach (var objectData in sourceDataLoaderMetadata.MemoryCell.Objects.Values)
        //    {
        //        System.Collections.Generic.Dictionary<object, ObjectData> relatedObjects = null;
        //        if (!objectData.RelatedObjects.TryGetValue(relatedObjectDataNode.AssignedMetaObject.Identity.ToString(), out relatedObjects))
        //        {
        //            relatedObjects = new System.Collections.Generic.Dictionary<object, ObjectData>();
        //            objectData.RelatedObjects[relatedObjectDataNode.AssignedMetaObject.Identity.ToString()] = relatedObjects;
        //        }

        //        var _object = objectData._Object;
        //        System.Collections.Generic.List<MetaObjectID> lazyFetchingMembersIdentities = ObjectData.GetLazyFetchingMembers(relatedObjectDataNode.Classifier, relatedObjectDataNode.SubDataNodes);


        //        if (relatedObjectDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
        //        {

        //            object memberObject = null;
        //            if (dataNode.Classifier.ClassHierarchyLinkAssociation == associationEnd.Association)
        //            {
        //                if (associationEnd.IsRoleA)
        //                {
        //                    if (dataNode.Classifier is DotNetMetaDataRepository.Interface)
        //                        memberObject = (dataNode.Classifier as DotNetMetaDataRepository.Interface).LinkClassRoleAProperty.GetValue(_object, null);

        //                    if (dataNode.Classifier is DotNetMetaDataRepository.Class)
        //                        memberObject = Member<object>.GetValue((dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleAFastFieldAccessor.GetValue, _object);
        //                }
        //                else
        //                {
        //                    if (dataNode.Classifier is DotNetMetaDataRepository.Interface)
        //                        memberObject = (dataNode.Classifier as DotNetMetaDataRepository.Interface).LinkClassRoleBProperty.GetValue(_object, null);

        //                    if (dataNode.Classifier is DotNetMetaDataRepository.Class)
        //                        memberObject = Member<object>.GetValue((dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleBFastFieldAccessor.GetValue, _object);
        //                }
        //                ObjectData relatedObjectData = null;
        //                if (!objectsMap.TryGetValue(memberObject, out relatedObjectData))
        //                {
        //                    relatedObjectData = new ObjectData(memberObject, objectData.ObjectID, sourceDataLoaderMetadata.QueryStorageID, lazyFetchingMembersIdentities);
        //                    objectsMap[memberObject] = relatedObjectData;
        //                    subDataNodeObjects.Add(relatedObjectData);
        //                }
        //                relatedObjectData.ParentDataNodeRelatedObjects[objectData._Object] = objectData;
        //                relatedObjects[relatedObjectData._Object] = relatedObjectData;


        //            }
        //            else
        //            {
        //                if ((associationEnd as DotNetMetaDataRepository.AssociationEnd).PropertyMember != null)
        //                    memberObject = (associationEnd as DotNetMetaDataRepository.AssociationEnd).FastPropertyAccessor.GetValue(_object);
        //                else
        //                    memberObject = Member<object>.GetValue((associationEnd as DotNetMetaDataRepository.AssociationEnd).FastFieldAccessor.GetValue, _object);
        //                if (memberObject != null)
        //                {
        //                    if (associationEnd.CollectionClassifier != null)
        //                    {

        //                        System.Collections.IEnumerator enumerator = memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]).Invoke(memberObject, new object[0]) as System.Collections.IEnumerator;
        //                        enumerator.Reset();

        //                        while (enumerator.MoveNext())
        //                        {

        //                            object collectionObj = enumerator.Current;
        //                            //ObjectData relatedObjectData = new ObjectData(collectionObj,  objectData.ObjectID, sourceDataLoaderMetadata.QueryStorageID,dataNode );
        //                            ObjectData relatedObjectData = null;
        //                            if (!objectsMap.TryGetValue(collectionObj, out relatedObjectData))
        //                            {
        //                                relatedObjectData = new ObjectData(collectionObj, objectData.ObjectID, sourceDataLoaderMetadata.QueryStorageID, lazyFetchingMembersIdentities);
        //                                objectsMap[collectionObj] = relatedObjectData;
        //                                subDataNodeObjects.Add(relatedObjectData);
        //                            }


        //                            relatedObjects[relatedObjectData._Object] = relatedObjectData;
        //                            relatedObjectData.ParentDataNodeRelatedObjects[objectData._Object] = objectData;
        //                        }

        //                    }
        //                    else
        //                    {

        //                        //ObjectData relatedObjectData = new ObjectData(memberObject, objectData.ObjectID, sourceDataLoaderMetadata.QueryStorageID,dataNode );
        //                        ObjectData relatedObjectData = null;
        //                        if (!objectsMap.TryGetValue(memberObject, out relatedObjectData))
        //                        {
        //                            relatedObjectData = new ObjectData(memberObject, objectData.ObjectID, sourceDataLoaderMetadata.QueryStorageID, lazyFetchingMembersIdentities);
        //                            objectsMap[memberObject] = relatedObjectData;
        //                            subDataNodeObjects.Add(relatedObjectData);
        //                        }

        //                        relatedObjects[relatedObjectData._Object] = relatedObjectData;
        //                        relatedObjectData.ParentDataNodeRelatedObjects[objectData._Object] = objectData;

        //                    }
        //                }
        //            }
        //        }

        //        if (relatedObjectDataNode.AssignedMetaObject is MetaDataRepository.Attribute && relatedObjectDataNode.Type == DataNode.DataNodeType.Object && relatedObjectDataNode.DataSource != null)
        //        {
        //            MetaDataRepository.Attribute attribute = relatedObjectDataNode.AssignedMetaObject as MetaDataRepository.Attribute;
        //            object memberObject = null;

        //            if ((attribute as DotNetMetaDataRepository.Attribute).PropertyMember != null)
        //                memberObject = (attribute as DotNetMetaDataRepository.Attribute).FastPropertyAccessor.GetValue(_object);
        //            else
        //                memberObject = (attribute as DotNetMetaDataRepository.Attribute).FastFieldAccessor.GetValue(_object);
        //            if (memberObject != null)
        //            {
        //                if (memberObject != null)
        //                {
        //                    if (memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]) != null
        //                        && memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]).ReturnType.IsGenericType
        //                        && memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]).ReturnType.GetGenericArguments().Length == 1
        //                        && memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]).ReturnType.GetGenericArguments()[0] == relatedObjectDataNode.Classifier.GetExtensionMetaObject(typeof(System.Type)) as System.Type)
        //                    {
        //                        System.Collections.IEnumerator enumerator = memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]).Invoke(memberObject, new object[0]) as System.Collections.IEnumerator;
        //                        enumerator.Reset();

        //                        while (enumerator.MoveNext())
        //                        {
        //                            object collectionObj = enumerator.Current;
        //                            //ObjectData relatedObjectData = new ObjectData(collectionObj,  objectData.ObjectID, sourceDataLoaderMetadata.QueryStorageID,dataNode );

        //                            ObjectData relatedObjectData = null;
        //                            if (!objectsMap.TryGetValue(collectionObj, out relatedObjectData))
        //                            {
        //                                relatedObjectData = new ObjectData(collectionObj, objectData.ObjectID, sourceDataLoaderMetadata.QueryStorageID, lazyFetchingMembersIdentities);
        //                                objectsMap[collectionObj] = relatedObjectData;
        //                                subDataNodeObjects.Add(relatedObjectData);
        //                            }
        //                            relatedObjectData.ParentDataNodeRelatedObjects[objectData._Object] = objectData;


        //                        }
        //                    }
        //                    else
        //                    {
        //                        //ObjectData relatedObjectData = new ObjectData(memberObject, objectData.ObjectID, sourceDataLoaderMetadata.QueryStorageID,dataNode );
        //                        ObjectData relatedObjectData = null;
        //                        if (!objectsMap.TryGetValue(memberObject, out relatedObjectData))
        //                        {
        //                            relatedObjectData = new ObjectData(memberObject, objectData.ObjectID, sourceDataLoaderMetadata.QueryStorageID, lazyFetchingMembersIdentities);
        //                            objectsMap[memberObject] = relatedObjectData;
        //                            subDataNodeObjects.Add(relatedObjectData);
        //                        }
        //                        relatedObjectData.ParentDataNodeRelatedObjects[objectData._Object] = objectData; ;

        //                    }
        //                }
        //            }
        //        }
        //    }
        //    foreach (var memoryCell in ((relatedObjectDataNode.ObjectQuery as StorageObjectQuery).ObjectsContext as MemoryObjectsContext).GetMemoryCells(subDataNodeObjects, relatedObjectDataNode.Classifier.GetExtensionMetaObject<Type>(), relatedObjectDataNode.Identity))
        //    {
        //        if (!sourceDataLoaderMetadata.RelatedMemoryCells.ContainsKey(relatedObjectDataNode.Identity))
        //            sourceDataLoaderMetadata.RelatedMemoryCells[relatedObjectDataNode.Identity] = new System.Collections.Generic.List<MemoryCellReference>();
        //        sourceDataLoaderMetadata.RelatedMemoryCells[relatedObjectDataNode.Identity].Add(new MemoryCellReference(memoryCell));

        //        if (memoryCell is InProcessMemoryCell)
        //        {
        //            var dataLoaderMetadata = new DataLoaderMetadata(relatedObjectDataNode, memoryCell, (relatedObjectDataNode.ObjectQuery as StorageObjectQuery).ObjectsContext);
        //            relatedDataLoadersMetadata[memoryCell.ObjectsContextIdentity] = dataLoaderMetadata;
        //            if (!(relatedObjectDataNode.ObjectQuery as StorageObjectQuery).QueryStorageIdentities.Contains(memoryCell.ObjectsContextIdentity))
        //                (relatedObjectDataNode.ObjectQuery as StorageObjectQuery).QueryStorageIdentities.Add(memoryCell.ObjectsContextIdentity);

        //        }
        //        else if (memoryCell is PartialLoadedMemoryCell)
        //        {
        //            var storageCell = (memoryCell as PartialLoadedMemoryCell).StorageCells[0];

        //            var openStorage = PersistenceLayer.ObjectStorage.OpenStorage(storageCell.StorageName, storageCell.StorageLocation, storageCell.StorageType);
        //            var dataLoaderMetadata = new DataLoaderMetadata(relatedObjectDataNode, memoryCell, openStorage);
        //            relatedDataLoadersMetadata[dataLoaderMetadata.ObjectsContextIdentity] = dataLoaderMetadata;
        //            if (!(relatedObjectDataNode.ObjectQuery as StorageObjectQuery).QueryStorageIdentities.Contains(dataLoaderMetadata.ObjectsContextIdentity))
        //                (relatedObjectDataNode.ObjectQuery as StorageObjectQuery).QueryStorageIdentities.Add(dataLoaderMetadata.ObjectsContextIdentity);

        //        }
        //        else
        //        {
        //            var dataLoaderMetadata = new DataLoaderMetadata(relatedObjectDataNode, memoryCell, (relatedObjectDataNode.ObjectQuery as StorageObjectQuery).ObjectsContext);
        //            sourceDataLoaderMetadata.HasOutStorageRelationsWithParent = true;
        //            relatedDataLoadersMetadata[dataLoaderMetadata.ObjectsContextIdentity] = dataLoaderMetadata;
        //            if (!(relatedObjectDataNode.ObjectQuery as StorageObjectQuery).QueryStorageIdentities.Contains(dataLoaderMetadata.ObjectsContextIdentity))
        //                (relatedObjectDataNode.ObjectQuery as StorageObjectQuery).QueryStorageIdentities.Add(dataLoaderMetadata.ObjectsContextIdentity);
        //        }

        //    }


        //    foreach (var sourceStorageCell in sourceStorageCells)
        //    {
        //        string storageName = null;
        //        string storageLocation = null;
        //        string storageType = null;
        //        string storageIdentity = null;



        //        sourceStorageCell.GetStorageConnectionData(out storageIdentity, out storageName, out storageLocation, out storageType);
        //        PersistenceLayer.ObjectStorage objectStorage = PersistenceLayer.ObjectStorage.OpenStorage(storageName, storageLocation, storageType);
        //        string sourceObjectsContextIdentity = objectStorage.Identity;
        //        Collections.Generic.Set<MetaDataRepository.RelatedStorageCell> linkedStorageCells = objectStorage.GetLinkedStorageCells(associationEnd, relatedObjectDataNode.ParentDataNode.ValueTypePath, new OOAdvantech.Collections.Generic.Set<StorageCell>(sourceStorageCells));
        //        sourceDataLoaderMetadata.AddRelatedStorageCell(relatedObjectDataNode, linkedStorageCells);
        //        if (relatedObjectDataNode.Recursive)
        //        {
        //            var recursiveSteps = relatedObjectDataNode.RecursiveSteps;
        //        }
        //        if (linkedStorageCells.Count > 0)
        //        {
        //            foreach (MetaDataRepository.RelatedStorageCell relatedStorageCell in linkedStorageCells)
        //            {
        //                if (relatedObjectDataNode.ParentDataNode == dataNode)
        //                    relatedObjectDataNode.ThroughRelationTable |= relatedStorageCell.ThrougthRelationTable;
        //                if (dataNode.ParentDataNode == relatedObjectDataNode)
        //                    dataNode.ThroughRelationTable |= relatedStorageCell.ThrougthRelationTable;
        //                StorageCell storageCell = relatedStorageCell.StorageCell;

        //                storageIdentity = null;
        //                storageCell.GetStorageConnectionData(out storageIdentity, out storageName, out storageLocation, out storageType);

        //                if (sourceObjectsContextIdentity != storageIdentity)
        //                {
        //                    if (!queryStorageIdentities.Contains(storageIdentity))
        //                        queryStorageIdentities.Add(storageIdentity);//] = QueryStorageIdentities.Count;

        //                    if (relatedObjectDataNode == dataNode.ParentDataNode)
        //                    {
        //                        (dataNode.DataSource as StorageDataSource).DataLoadersMetadata[sourceObjectsContextIdentity].HasOutStorageRelationsWithParent = true;
        //                        outStorageRelations.Add(storageIdentity);
        //                    }
        //                    else
        //                    {
        //                        (dataNode.DataSource as StorageDataSource).DataLoadersMetadata[sourceObjectsContextIdentity].HasOutStorageRelationsWithSubDataNode(relatedObjectDataNode);
        //                        outStorageRelations.Add(storageIdentity);
        //                    }
        //                    if (!(dataNode.DataSource as StorageDataSource).DataLoadersMetadata.ContainsKey(storageIdentity))
        //                    {
        //                        (dataNode.DataSource as StorageDataSource).DataLoadersMetadata[storageIdentity] = new DataLoaderMetadata(dataNode, queryStorageIdentities.IndexOf(storageIdentity), storageIdentity, storageName, storageLocation, storageType);
        //                    }
        //                    bool exist = false;
        //                    foreach (MetaDataRepository.StorageCellReference.StorageCellReferenceMetaData storageCellReferenceMetaData in (dataNode.DataSource as StorageDataSource).DataLoadersMetadata[storageIdentity].StorageCellReferencesMetaData)
        //                    {
        //                        if (storageCellReferenceMetaData.SerialNumber == relatedStorageCell.RootStorageCell.SerialNumber &&
        //                            storageCellReferenceMetaData.StorageIdentity == relatedStorageCell.RootStorageCell.StorageIdentity)
        //                        {
        //                            exist = true;
        //                            break;
        //                        }
        //                    }
        //                    if (!exist)
        //                        (dataNode.DataSource as StorageDataSource).DataLoadersMetadata[storageIdentity].StorageCellReferencesMetaData.Add(new StorageCellReference.StorageCellReferenceMetaData(relatedStorageCell.RootStorageCell));
        //                }
        //                if (!relatedDataLoadersMetadata.ContainsKey(storageIdentity))
        //                {
        //                    if (!queryStorageIdentities.Contains(storageIdentity))
        //                        queryStorageIdentities.Add(storageIdentity);//] = QueryStorageIdentities.Count;
        //                    relatedDataLoadersMetadata[storageIdentity] = new DataLoaderMetadata(relatedObjectDataNode, queryStorageIdentities.IndexOf(storageIdentity), storageIdentity, storageName, storageLocation, storageType);
        //                }

        //                if (!string.IsNullOrEmpty(relatedObjectDataNode.OfTypeFilterClassifierFullName))
        //                {
        //                    if (storageCell.Type.FullName == relatedObjectDataNode.OfTypeFilterClassifierFullName)
        //                    {
        //                        relatedDataLoadersMetadata[storageIdentity].AddStorageCell(storageCell);
        //                    }
        //                    else
        //                    {
        //                        foreach (MetaDataRepository.Classifier classifier in storageCell.Type.GetAllGeneralClasifiers())
        //                        {
        //                            if (classifier.FullName == relatedObjectDataNode.OfTypeFilterClassifierFullName)
        //                            {
        //                                relatedDataLoadersMetadata[storageIdentity].AddStorageCell(storageCell);
        //                                break;
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                    relatedDataLoadersMetadata[storageIdentity].AddStorageCell(storageCell);
        //            }
        //        }

        //    }

        //    //foreach (var entry in partialLoadedObjects)
        //    //{
        //    //    var storageInstanceRef = entry.Key;
        //    //    DataLoaderMetadata newSourceDataLoaderMetadata = null;

        //    //    if (!(referenceDataNode.DataSource as StorageDataSource).DataLoadersMetadata.TryGetValue(storageInstanceRef.ObjectStorage.Identity.ToString(), out newSourceDataLoaderMetadata))
        //    //    {
        //    //        if((referenceDataNode.ObjectQuery as StorageObjectQuery).QueryStorageIdentities.Contains(storageInstanceRef.ObjectStorage.Identity.ToString()))
        //    //            (referenceDataNode.ObjectQuery as StorageObjectQuery).QueryStorageIdentities.Add(storageInstanceRef.ObjectStorage.Identity.ToString());
        //    //        newSourceDataLoaderMetadata = new DataLoaderMetadata(referenceDataNode, (referenceDataNode.ObjectQuery as StorageObjectQuery).QueryStorageIdentities.IndexOf(storageInstanceRef.ObjectStorage.Identity.ToString()), storageInstanceRef.ObjectStorage, new InProcessMemoryCell(new List<ObjectData>(), sourceDataLoaderMetadata.MemoryCell.Type, storageInstanceRef.ObjectStorage.Identity.ToString()));
        //    //        (referenceDataNode.DataSource as StorageDataSource).DataLoadersMetadata[storageInstanceRef.ObjectStorage.Identity.ToString()] = newSourceDataLoaderMetadata;
        //    //    }



        //    //    newSourceDataLoaderMetadata.MemoryCell.Objects.Add(entry.Value._Object, entry.Value);
        //    //    sourceDataLoaderMetadata.MemoryCell.Objects.Remove(storageInstanceRef.MemoryInstance);
        //    //    //if (!newSourceDataLoaderMetadata.StorageCells.Contains(storageInstanceRef.StorageInstanceSet))
        //    //    //    newSourceDataLoaderMetadata.AddStorageCell(storageInstanceRef.StorageInstanceSet);

        //    //}
        //    return relatedDataLoadersMetadata;
        //}


    }

    /// <MetaDataID>{9f7bc99f-4ed4-4e0c-9702-dac8febc67f3}</MetaDataID>
    struct SearchingData
    {
        /// <MetaDataID>{337d6fda-32a4-48e3-a057-8d15dc9d048c}</MetaDataID>
        public SearchCondition SearchCondition;
        /// <MetaDataID>{097fa07a-af61-4982-a3da-902d513a7bb8}</MetaDataID>
        public System.Collections.Generic.List<Criterion> SearchCriteria;
        /// <MetaDataID>{a3083181-4d73-458d-9edf-ed5b6043851c}</MetaDataID>
        public SearchingData(SearchCondition searchCondition, System.Collections.Generic.List<Criterion> searchCriteria)
        {
            SearchCondition = searchCondition;
            SearchCriteria = searchCriteria;
        }
    }

    /// <MetaDataID>{c62cb477-fac4-44a8-8fe3-cda42d915df1}</MetaDataID>
    [Serializable]

    public struct DistributedObjectQueryData
    {
        internal DistributedObjectQueryData Clone()
        {
            Dictionary<object, object> clonedObjects = new Dictionary<object, object>();

            DataNode headerDataNode = null;
            if (HeaderDataNode != null)
                headerDataNode = HeaderDataNode.Clone(clonedObjects);
            QueryResultType queryResultMetaData = null;
            if (QueryResultMetaData != null)
                queryResultMetaData = QueryResultMetaData.Clone(clonedObjects);

            System.Collections.Generic.Dictionary<Guid, DataNode> dataNodesDictionary = null;
            if (DataNodesDictionary != null)
            {
                dataNodesDictionary = new System.Collections.Generic.Dictionary<DataSourceIdentity, DataNode>();
                foreach (var entry in DataNodesDictionary)
                    dataNodesDictionary[entry.Key] = entry.Value.Clone(clonedObjects);
            }
            return new DistributedObjectQueryData() { HeaderDataNode = headerDataNode, QueryResultMetaData = queryResultMetaData, DataNodesDictionary = dataNodesDictionary };
        }
        /// <MetaDataID>{5d6e0a70-455b-4d03-9625-7d6df208f4e0}</MetaDataID>
        public DataNode HeaderDataNode;
        /// <MetaDataID>{c17db2df-f168-4874-893a-b86b53cbff35}</MetaDataID>
        public QueryResultType QueryResultMetaData;

        public System.Collections.Generic.Dictionary<Guid, DataNode> DataNodesDictionary;

    }

    /// <summary>
    ///This class manages the data loaders meta data of distributed queries.
    ///The prefetching mechanis produces data loaders meta data that collected from master query through the ObjectsContextMetadataDistributionManager class.
    ///Master query distribute through the ObjectsContextMetadataDistributionManager class, new data loaders metadara to the distribute queries where loads the data.
    /// </summary>
    /// <MetaDataID>{555b1b0a-5f60-4931-bf7f-aa8544418aa6}</MetaDataID>
    class ObjectsContextMetadataDistributionManager
    {


        /// <summary>
        /// Defines the main query which controls the query execution
        /// </summary>
        [IgnoreErrorCheck]
        [Association("", Roles.RoleB, "484ed475-348e-4488-a893-9b402bf6a3c9")]
        public ObjectsContextQuery MasterQuery;


        /// <summary>
        ///Defines the objects context that manages the life cycle of object.
        /// </summary>
        [IgnoreErrorCheck]
        [Association("", Roles.RoleA, "2952634d-6137-4104-8895-3070e14cc36a")]
        public readonly ObjectsContext ObjectsContext;


        /// <summary>
        /// Defines a dictionary where key is the DataSourceIdentity and value is DataLoaderMetadata instance.
        /// PrefetcingDataLoadersMetadata collection has data loaders metada which produced from Prefetcing mechanism
        /// </summary>
        [IgnoreErrorCheck]
        [RoleBMultiplicityRange(1, 1)]
        [Association("", Roles.RoleA, "09c20dc7-e7a0-401e-8fd6-7aa3abdcb797")]
        public OOAdvantech.Collections.Generic.Dictionary<Guid, DataLoaderMetadata> PrefetcingDataLoadersMetadata = new OOAdvantech.Collections.Generic.Dictionary<Guid, DataLoaderMetadata>();

        /// <summary>
        /// Defines a dictionary where key is the DataSourceIdentity and value is DataLoaderMetadata instance
        /// Data Loaders Metadata defines the metadata for data nodes which are claim of query 
        /// </summary>
        [IgnoreErrorCheck]
        [RoleBMultiplicityRange(1, 1)]
        [Association("", Roles.RoleA, "b1cbfe34-41dc-46f0-b985-8e24215bb46b")]
        public OOAdvantech.Collections.Generic.Dictionary<Guid, DataLoaderMetadata> DataLoadersMetadata = new OOAdvantech.Collections.Generic.Dictionary<Guid, DataLoaderMetadata>();


        [IgnoreErrorCheck]
        [RoleBMultiplicityRange(1, 1)]
        [Association("", Roles.RoleA, "188398db-370d-444d-8fbd-e1de44139049")]
        public DistributedObjectQuery DistributedObjectQuery;



        /// <summary>
        /// All DataLoaders meta data are transferred to the DistributedObjectQuery
        /// </summary>
        public bool DistributedObjectQueryUpdated
        {
            get
            {
                return DistributedObjectQuery != null && PrefetcingDataLoadersMetadata.Count == 0;
            }
        }

        /// <summary>
        /// Defines the identity of ObjectsContext
        /// </summary>
        /// <MetaDataID>{31ded5b5-0222-4e4c-a81b-aac7a65807b0}</MetaDataID>
        public readonly string ObjectsContextIdentity;

        /// <summary>
        /// Initialize ObjectsContextMetadataDistributionManager instance
        /// </summary>
        /// <param name="objectsContextIdentity">
        /// Defines the identity of ObjectsContext
        /// </param>
        /// <param name="objectsContext">
        /// Defines the objects context
        /// </param>
        /// <MetaDataID>{7ac2a4ac-d85b-45ae-a004-d21aefa3a723}</MetaDataID>
        public ObjectsContextMetadataDistributionManager(string objectsContextIdentity, OOAdvantech.ObjectsContext objectsContext)
        {
            ObjectsContextIdentity = objectsContextIdentity;
            ObjectsContext = objectsContext;

        }

        /// <summary>
        /// Adds data loaders metada 
        /// </summary>
        /// <param name="dataNode">
        /// Defines the DataNode of DataLoaderMetaData on MasterQuery
        /// </param>
        /// <param name="dataLoaderMetadata">
        /// Defines the DataLoaderMetaData
        /// </param>
        /// <MetaDataID>{8bdf709d-f705-4962-88ef-50423844efbd}</MetaDataID>
        internal void AddDataLoaderMetadata(DataNode dataNode, DataLoaderMetadata dataLoaderMetadata)
        {
            if (dataNode.ObjectQuery != MasterQuery)
                throw new Exception("Invalid DataNode. DataNode doesn't belong to the MasterQuery");
            if (DistributedObjectQuery == null)
            {
                /// In case where there isn't DistributedObjectQuery the ObjectsContextMetadataDistributionManager is at the initialization state
                /// the data loaders meta data added to the DataLoadersMetadata dictionary 
                if (!DataLoadersMetadata.ContainsKey(dataNode.DataSource.Identity))
                    DataLoadersMetadata.Add(dataNode.DataSource.Identity, dataLoaderMetadata);
                else
                {
                    DataLoaderMetadata existingdataLoaderMetadata = DataLoadersMetadata[dataNode.DataSource.Identity];
                    existingdataLoaderMetadata.UpdateWithExtraMetadataInfo(dataLoaderMetadata);
                }
            }
            else
            {
                /// In case where already DistributedObjectQuery initialized and the ObjectsContextMetadataDistributionManager is at the update state
                /// the data loaders meta data added to the PrefetcingDataLoadersMetadata dictionary 
                if (!PrefetcingDataLoadersMetadata.ContainsKey(dataNode.DataSource.Identity))
                    PrefetcingDataLoadersMetadata.Add(dataNode.DataSource.Identity, dataLoaderMetadata);
                else
                {
                    DataLoaderMetadata existingdataLoaderMetadata = PrefetcingDataLoadersMetadata[dataNode.DataSource.Identity];
                    existingdataLoaderMetadata.UpdateWithExtraMetadataInfo(dataLoaderMetadata);
                }
            }
        }


        /// <summary>
        /// Update distributed query with the new data loading metadata for distributed query 
        /// and retrieve the new prefetching data from distributed query.
        /// </summary>
        /// <MetaDataID>{b9471a10-5356-4461-9322-63102be65870}</MetaDataID>
        internal OOAdvantech.Collections.Generic.List<DataNodeRelatedDataLoadersMetadata> UpdateDistributedQueryMetadata()
        {

            DataNode headerDataNode = null;
            headerDataNode = MasterQuery.DataTrees[0];

            Collections.Generic.List<DataNode> selectListItems = MasterQuery.SelectListItems;
            OOAdvantech.Collections.Generic.List<DataNode> marshaledDataTrees = new OOAdvantech.Collections.Generic.List<DataNode>();
            DistributedObjectQueryData distributedObjectQueryData = new DistributedObjectQueryData();
            distributedObjectQueryData.HeaderDataNode = headerDataNode;
            distributedObjectQueryData.QueryResultMetaData = MasterQuery.QueryResultType;

            if (!OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(ObjectsContext))
            {
                #region Copy data node tree when the ObjectsContext is out of process

                distributedObjectQueryData = distributedObjectQueryData.Clone();
                //distributedObjectQueryData = OOAdvantech.AccessorBuilder.Clone<DistributedObjectQueryData>(distributedObjectQueryData);

                //System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                //System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                //binaryFormatter.Serialize(memoryStream, distributedObjectQueryData);
                //memoryStream.Position = 0;
                //distributedObjectQueryData = (DistributedObjectQueryData)binaryFormatter.Deserialize(memoryStream);


                headerDataNode = distributedObjectQueryData.HeaderDataNode;
                selectListItems.Clear();
                foreach (DataNode dataNode in MasterQuery.SelectListItems)
                    selectListItems.Add(headerDataNode.GetDataNode(dataNode.Identity));

                #endregion
            }



            marshaledDataTrees.Add(headerDataNode);

            #region Notify distribute query with meta changes

            if (DistributedObjectQuery == null)
            {
                //Create a DistributedObjectQuery for ObjectContext
                DistributedObjectQuery = (ObjectsContext as IObjectQueryPartialResolver).DistributeObjectQuery(MasterQuery.QueryIdentity, marshaledDataTrees, distributedObjectQueryData.QueryResultMetaData, selectListItems, DataLoadersMetadata, MasterQuery.Parameters, MasterQuery.UsedAliases, MasterQuery.QueryStorageIdentities);
            }
            else
            {
                if (PrefetcingDataLoadersMetadata.Count > 0)
                {
                    #region Update ObjectContext DataLodersMetadata
                    foreach (System.Collections.Generic.KeyValuePair<Guid, DataLoaderMetadata> entry in PrefetcingDataLoadersMetadata)
                    {
                        if (DataLoadersMetadata.ContainsKey(entry.Key))
                            DataLoadersMetadata[entry.Key].UpdateWithExtraMetadataInfo(entry.Value);
                        else
                            DataLoadersMetadata.Add(entry.Key, entry.Value);
                    }
                    PrefetcingDataLoadersMetadata.Clear();
                    #endregion

                    DistributedObjectQuery.NotifyMasterQueryMetaDataChange(marshaledDataTrees, selectListItems, DataLoadersMetadata);
                }

            }

            #endregion


            //
            Collections.Generic.List<DataNodeRelatedDataLoadersMetadata> dataNodesPrefetchingDataRemote = DistributedObjectQuery.RetrieveDataNodesPrefatchingData(ref MasterQuery.QueryStorageIdentities);
            Collections.Generic.List<DataNodeRelatedDataLoadersMetadata> dataNodesPrefetchingData = new OOAdvantech.Collections.Generic.List<DataNodeRelatedDataLoadersMetadata>();


            #region Transform result to refered in master query data nodes

            System.Collections.Generic.Dictionary<Guid, DataNode> prefetchingDataDataNodes = new System.Collections.Generic.Dictionary<Guid, DataNode>();

            foreach (DataNodeRelatedDataLoadersMetadata dataNodePrefetchingData in dataNodesPrefetchingDataRemote)
            {
                foreach (DataNode dataNode in dataNodePrefetchingData.SubDataNodesNewDataLoadersMetadata.Keys)
                    prefetchingDataDataNodes[dataNode.Identity] = dataNode;
            }

            if (!OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(ObjectsContext) && dataNodesPrefetchingDataRemote.Count > 0)
            {

                //prefetchingDataDataNodes = OOAdvantech.AccessorBuilder.Clone<System.Collections.Generic.Dictionary<Guid, DataNode>>(prefetchingDataDataNodes);

                distributedObjectQueryData = new DistributedObjectQueryData() { HeaderDataNode = null, QueryResultMetaData = null, DataNodesDictionary = prefetchingDataDataNodes };
                distributedObjectQueryData = distributedObjectQueryData.Clone();
                prefetchingDataDataNodes = distributedObjectQueryData.DataNodesDictionary;

                int rtt = 0;

                //prefetchingDataDataNodes = OOAdvantech.AccessorBuilder.Clone<System.Collections.Generic.Dictionary<Guid, DataNode>>(prefetchingDataDataNodes);





                //System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bForm = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                //System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                //bForm.Serialize(memoryStream, prefetchingDataDataNodes);
                //memoryStream.Position = 0;
                //prefetchingDataDataNodes = bForm.Deserialize(memoryStream) as System.Collections.Generic.Dictionary<Guid, DataNode>;
            }


            #region Build DataNodePrefetchingData with reference in master query meta data

            foreach (DataNodeRelatedDataLoadersMetadata dataNodePrefetchingDataRemote in dataNodesPrefetchingDataRemote)
            {

                DataNodeRelatedDataLoadersMetadata dataNodePrefetchingData = new DataNodeRelatedDataLoadersMetadata(MasterQuery.DataTrees[0].GetDataNode(dataNodePrefetchingDataRemote.DataNode.Identity));
                dataNodePrefetchingData.DataNodeNewDataLoadersMetadata = dataNodePrefetchingDataRemote.DataNodeNewDataLoadersMetadata;
                DataNode prefetchingDataNode = MasterQuery.DataTrees[0].GetDataNode(dataNodePrefetchingDataRemote.DataNode.Identity);
                foreach (var entry in dataNodePrefetchingDataRemote.SubDataNodesNewDataLoadersMetadata)
                {
                    DataNode prefetchingSubDataNode = prefetchingDataDataNodes[entry.Key.Identity];
                    if (MasterQuery.DataTrees[0].GetDataNode(prefetchingSubDataNode.Identity) == null)
                        prefetchingSubDataNode.ParentDataNode = prefetchingDataNode;
                    else
                    {
                        MasterQuery.DataTrees[0].GetDataNode(prefetchingSubDataNode.Identity).MembersFetchingObjectActivation = prefetchingSubDataNode.MembersFetchingObjectActivation;
                        MasterQuery.DataTrees[0].GetDataNode(prefetchingSubDataNode.Identity).AutoGenaratedForMembersFetching = prefetchingSubDataNode.AutoGenaratedForMembersFetching;
                        MasterQuery.DataTrees[0].GetDataNode(prefetchingSubDataNode.Identity).FilterNotActAsLoadConstraint = prefetchingSubDataNode.FilterNotActAsLoadConstraint;
                    }
                    dataNodePrefetchingData.SubDataNodesNewDataLoadersMetadata[prefetchingSubDataNode] = DataLoaderMetadata.TransformDataLoadersMetadata(MasterQuery as ObjectQuery, entry.Value);
                }

                dataNodesPrefetchingData.Add(dataNodePrefetchingData);
            }

            #endregion

            #endregion

            return dataNodesPrefetchingData;

        }
    }

}
