namespace Parser
{


    /// <MetaDataID>{e4787f36-3957-4775-ba98-32335a154242}</MetaDataID>
	public interface ParserNodeIndexer
	{


        /// <MetaDataID>{fb48cb2a-5b6c-4372-9845-4d7381f66a9d}</MetaDataID>
		ParserNodeIndexer this[string childNodeName]
		{
			get;
			
		}
        /// <MetaDataID>{c7b961d1-3464-4a44-b816-94fde1ec058a}</MetaDataID>
		ParserNodeIndexer this[int index]
		{
			get;
		}
        /// <MetaDataID>{60ea2201-04c3-4951-9f4a-f7ff62a5f5ec}</MetaDataID>
		string this[ParserNodeField field]
		{
			get;
		}


	}
	/// <MetaDataID>{3AD64CF7-6122-4B47-AC85-A417BC0E0378}</MetaDataID>
	public class ParserNodeCollection :ParserNodeIndexer
	{
		public System.Collections.IEnumerator GetEnumerator()
		{
			
			return Nodes.GetEnumerator();
		}

		/// <MetaDataID>{DFD60B73-AE3E-4A33-ABF0-D6F3676C549F}</MetaDataID>
		public ParserNodeCollection()
		{
			Nodes=new System.Collections.Generic.List<object>();
			NamedNodes=new System.Collections.Generic.Dictionary<string, object>();
			SameType=false;
		}
		bool SameType=false;
		public ParserNodeCollection(bool sameType)
		{
			Nodes=new System.Collections.Generic.List<object>();
			
			SameType=sameType;
			if(!SameType)
				NamedNodes=new System.Collections.Generic.Dictionary<string, object>();
		}

	
		/// <MetaDataID>{6FB9DA14-0DE9-4A18-B543-C6EAB649CFB1}</MetaDataID>
		private System.Collections.Generic.List<object> Nodes;
		System.Collections.Generic.Dictionary<string,object> NamedNodes;
		
		/// <MetaDataID>{76E89983-9146-4B63-957B-AC8109D5FED3}</MetaDataID>
		public int Count
		{
			get
			{
				return Nodes.Count;
			}
		}
		/// <MetaDataID>{0AEE4F20-D79F-42F8-B5C4-E2E892E40EC6}</MetaDataID>
		public ParserNode GetFirst()
		{
			return (ParserNode)Nodes[0];
		}

		/// <MetaDataID>{C8EDFE35-13D3-4C77-AA14-21F3F0F59F07}</MetaDataID>
		public ParserNode GetAt(int Index)
		{
			return (ParserNode)Nodes[Index-1];
		}
		/// <MetaDataID>{EB583E06-27C2-4425-B974-9B1F207334EE}</MetaDataID>
		public void Add(ParserNode NewParserNode)
		{
			Nodes.Add(NewParserNode);
			if(!SameType)
			{
				if(NamedNodes.ContainsKey(NewParserNode.Name))
				{
					object namedObject=NamedNodes[NewParserNode.Name];
					ParserNodeCollection parserNodes=namedObject as ParserNodeCollection;
					ParserNode parserNode=namedObject as ParserNode;
					if(parserNodes==null)
					{
						parserNodes=new ParserNodeCollection(true);
						parserNodes.Add(parserNode);
						parserNodes.Add(NewParserNode);
						NamedNodes[NewParserNode.Name]=parserNodes;
					}
					else
						parserNodes.Add(NewParserNode);
				}
				else
					NamedNodes.Add(NewParserNode.Name,NewParserNode);
			}
		}
		public ParserNodeIndexer this[string nodeName]
		{
			get
            {
                object value = null;
                if (NamedNodes.TryGetValue(nodeName, out value))
                    return value as ParserNodeIndexer;
                return null;
            }
		}
		public ParserNodeIndexer this[int index]
		{
			get
			{
				return Nodes[index] as ParserNodeIndexer;
			}
		}
		public string this[ParserNodeField field]
		{
			get
			{
				throw new System.NotSupportedException("Not supported from ParserNode collection");
			}
		}



	}
}
