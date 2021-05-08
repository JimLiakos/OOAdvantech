namespace Parser
{

    /// <MetaDataID>{60283bf6-d862-4c92-b917-a27efe789a52}</MetaDataID>
    public interface ParserNode : ParserNodeIndexer
    {
        /// <MetaDataID>{a78608b2-cc44-424e-83d1-735e973a0cfd}</MetaDataID>
        bool IsErrorNode
        {
            get;
        }
        /// <MetaDataID>{40afa0e0-6d1c-4ff4-9e67-d51a97789fd6}</MetaDataID>
        bool IsTerminal
        {
            get;
        }

        /// <MetaDataID>{4fb96868-461f-4649-96c1-18550783ee28}</MetaDataID>
        string Value
        {
            get;
        }
        /// <MetaDataID>{36e11440-14c1-4d4c-9ad2-dad2fe13df8f}</MetaDataID>
        string Name
        {
            get;
        }

        /// <MetaDataID>{ffa49c8f-1d8b-4ad7-b65c-a8c75307742a}</MetaDataID>
        int Line
        {
            get;
        }

        /// <MetaDataID>{f1d9ea11-df68-4932-8ff5-a13fca504341}</MetaDataID>
        int LinePosition
        {
            get;
        }
        /// <MetaDataID>{50646aea-01c2-4ee5-8e5f-4fc35fa52474}</MetaDataID>
        ParserNode ParentNode
        {
            get;
        }
        /// <MetaDataID>{cdb4dcd8-54f3-4313-9841-73808bea4c41}</MetaDataID>
        ParserNodeCollection ChildNodes
        {
            get;
        }



    }
    /// <MetaDataID>{a4e4ba0b-ece3-4400-810e-d357e9231043}</MetaDataID>
    public class ParserNodeField
    {
        public string FieldName;
        public static ParserNodeField Name
        {
            get
            {
                ParserNodeField field=new ParserNodeField();
                field.FieldName = "Name";
                return field;
            }
        }
        public static ParserNodeField Value
        {
            get
            {
                ParserNodeField field = new ParserNodeField();
                field.FieldName = "Value";
                return field;
            }
        }

    }
	/// <MetaDataID>{4B4BB6C2-76D5-46A7-B804-253F8721A3B1}</MetaDataID>
	public class ProGrammarParserNode :ParserNode
	{
		/// <MetaDataID>{C8D1332C-9971-44AC-BBBF-370EE26D8308}</MetaDataID>
		public ParserNodeCollection _ChildNodes;
		/// <MetaDataID>{79D16B25-99AC-4AE1-AD38-ACFDD3BE3F09}</MetaDataID>
		public ParserNode _ParentNode;
		/// <MetaDataID>{344629EC-45BA-4941-A47E-7024134FC064}</MetaDataID>
		public string _Value;
		/// <MetaDataID>{4C4C7278-5681-465F-A843-83EE414CBD80}</MetaDataID>
		public string _Name;
		public int _Line;

        ///// <MetaDataID>{4F67CB85-4E9B-42E5-BD6F-33DB0C42A1E0}</MetaDataID>
        //public ProGrammarParserNode(int NodeID, ParserWr.Parser theParser)
        //{


        //    _Name=theParser.GetLabel(NodeID);
        //    _Value=theParser.GetValue(NodeID);
        //    _Line=theParser.GetLine(NodeID);
			
        //    int NumOfChilds=theParser.GetNumChildren(NodeID);
        //    _ChildNodes=new ParserNodeCollection();
        //    for(int i=0;i!=NumOfChilds;i++)
        //    {
        //        int CurrChildNodeID=theParser.GetChild(NodeID,i);
        //        ProGrammarParserNode NewParserNode = new ProGrammarParserNode(CurrChildNodeID, theParser);
        //        _ChildNodes.Add(NewParserNode);
        //        NewParserNode._ParentNode=this;
        //    }
        //}

		/// <MetaDataID>{1E41C589-FD0C-4A08-9AF0-FE49DA667980}</MetaDataID>
		public ParserNodeIndexer this[string nodeName]
		{
			get
			{
				return _ChildNodes[nodeName] as ParserNodeIndexer;
			}
		}
		public ParserNodeIndexer this[int index]
		{
			get
			{
				return _ChildNodes[index] as ParserNodeIndexer;
			}
		}
		public string this[ParserNodeField field]
		{
			get
			{
				if(field.FieldName=="Name")
					return Name;

				if(field.FieldName=="Value")
					return Value;

				throw new System.NotSupportedException("ParserNode doesn't contains member with name "+field.FieldName+".");
			}
		}


        #region ParserNode Members

        public string Value
        {

            get { return _Value; }
        }

        public string Name
        {
            get { return _Name; }
        }

        public int Line
        {
            get { return _Line; }
        }

        public int LinePosition
        {
            get { return 0; }
        }

        public ParserNode ParentNode
        {
            get { return _ParentNode; }
        }

        ParserNodeCollection ParserNode.ChildNodes
        {
            get { return _ChildNodes; }
        }

        #endregion

        #region ParserNode Members

        public bool IsErrorNode
        {
            get { throw new System.Exception("The method or operation is not implemented."); }
        }

        public bool IsTerminal
        {
            get { throw new System.Exception("The method or operation is not implemented."); }
        }

        #endregion
    }
}
