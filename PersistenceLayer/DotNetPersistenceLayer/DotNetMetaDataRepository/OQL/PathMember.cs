namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
	/// <MetaDataID>{E958F1F7-B3DC-4273-8B6D-2C36B7F98925}</MetaDataID>
	public class Path
	{
		/// <MetaDataID>{81CE6D58-4AB0-44DE-AC33-C5FCD2D41ACE}</MetaDataID>
		internal Parser.ParserNode ParserNode;
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{9E216E49-4D0C-4AFB-8F1A-78E5BE376EE0}</MetaDataID>
		private string _Name;
		/// <MetaDataID>{5EF4EBBE-06E7-464C-A768-91CAD201F82C}</MetaDataID>
		public virtual string Name
		{
			get
			{
                if (_Name == null)
                {
                    if (ParserNode["Name"] == null)
                        return null;
                    _Name = ParserNode["Name"][Parser.ParserNodeField.Value];
                    if (_Name != null && _Name.Length > 2 && _Name[0] == '[' &&
                            _Name[_Name.Length - 1] == ']')
                    {
                        _Name= _Name.Substring(1, _Name.Length - 2);
                    }

                }
				return _Name;

			}
		}
		/// <MetaDataID>{C4D41097-A4F4-46D0-ADE4-5F2692D0176A}</MetaDataID>
		public virtual string AliasName
		{
			get
			{
				if(Parent!=null&&SubPath==null)
				{
					Path parent=Parent;
					while(!(parent is PathHead))
						parent=parent.Parent;
					return parent.AliasName;
				}
				return null;
			}
		}
		/// <MetaDataID>{A9123E35-B399-43A9-958D-A3DA09C3D8A3}</MetaDataID>
		public Path Parent;
		/// <MetaDataID>{4FBCD297-3D05-4E51-ADC9-3B182D957D45}</MetaDataID>
		public Path SubPath;


        /// <MetaDataID>{9ceb9498-0b0e-41dc-b94f-91bb8375f774}</MetaDataID>
        public Path AggregateExpressionPath;

        /// <MetaDataID>{5326E01B-747B-4F54-86C4-80F25E7DA37A}</MetaDataID>
		public Path(Path parent)
		{
			Parent=parent;

		}
        /// <MetaDataID>{5ca77bd0-b507-4aaf-970b-28e005375d07}</MetaDataID>
       protected bool  _AggregationPath = false;
       /// <MetaDataID>{af0362f4-7ded-4f62-a408-fdb5f0b527a8}</MetaDataID>
       public bool AggregationPath
       {
           get
           {
               return _AggregationPath;
           }
       }
		/// <MetaDataID>{A5E58564-AD46-4D34-901B-BEF1E6AA0F0C}</MetaDataID>
		public Path(Parser.ParserNode parserNode,Path parent)
		{
			Parent=parent;
			ParserNode=parserNode;
            

			if(parserNode==null)
				throw new System.Exception("Parameter parserNode must be not null.");

            if (parserNode.Name == "RecursivePathMember")
            {
                _Name = parserNode["RecursiveSpecs"]["Name"][Parser.ParserNodeField.Value];
                if (parserNode["RecursiveSpecs"]["NumericLiteral"] != null)
                {
#if !DeviceDotNet
                    int.TryParse(parserNode["RecursiveSpecs"]["NumericLiteral"][Parser.ParserNodeField.Value], out RecursiveSteps);
#else
                    int.TryParse(parserNode["RecursiveSpecs"]["NumericLiteral"][Parser.ParserNodeField.Value], out RecursiveSteps);
                    //parserNode["RecursiveSpecs"]["NumericLiteral"][Parser.ParserNodeField.Value].TryConvert(out RecursiveSteps);
#endif
                }
                Recursive = true;
            }
            else if (ParserNode.Name == "Function_Ref")
            {
                if ((ParserNode["OQL_Function_Name"][0] as Parser.ParserNode).Name.ToLower()== "count")
                {
                    //ParserNode = ParserNode["PathMember"] as Parser.ParserNode;
                    _AggregationPath = true;
                    if (ParserNode["Scalar_Expression"] != null)
                    {

                        ParserNode = ParserNode["Scalar_Expression"] as Parser.ParserNode;
                        while (ParserNode.Name != "Scalar_Item" && ParserNode.ChildNodes.Count == 1)
                            ParserNode = ParserNode[0] as Parser.ParserNode;

                        if (ParserNode.Name != "Scalar_Item")
                            throw new System.Exception("Wrang count expresion.");
                    }


                    //new PathHead(ParserNode)
                    if (ParserNode["Path"] != null || ParserNode["PathAlias"] != null)
                    {
                        if (ParserNode["PathAlias"] != null)
                            AggregateExpressionPath = new PathHead(ParserNode["PathAlias"] as Parser.ParserNode);

                        if (ParserNode["Path"] != null)
                            AggregateExpressionPath = new PathHead(ParserNode["Path"] as Parser.ParserNode);

                    }
                }
            }
            else if (ParserNode.Name != "PathMember")
                throw new System.Exception("Parameter parserNode is not PathMember.");

			 Parser.ParserNode pathMemberNode =ParserNode["PathMember"] as Parser.ParserNode;
			 if(pathMemberNode!=null)
				 SubPath=new Path(pathMemberNode,this);

		}
        /// <MetaDataID>{7f031633-f42c-425d-80a4-db6c93a8436f}</MetaDataID>
        public readonly int RecursiveSteps = 0;
        /// <MetaDataID>{e855ac37-2e49-45fd-b7e3-bf106a61428b}</MetaDataID>
        public readonly bool Recursive = false;
        /// <MetaDataID>{119b164c-4bde-4b98-a3a8-3859d57c0dda}</MetaDataID>
        public Path(string name, Path parent)
        {
            name = name.Trim();
            name = name.Replace(" ", "");
           // int recursiveSteps =0;
            if (name.IndexOf("Recursive(") == 0)
            {
                Recursive = true;
                name=name.Remove(0, "Recursive(".Length);
                name = name.Replace(")", "");
                int nPos = name.IndexOf(',');
                if (nPos != -1)
                    RecursiveSteps = int.Parse(name.Substring(nPos + 1));
                name = name.Substring(0, nPos);
            }
            Parent = parent;
            _Name = name;
        }
        /// <MetaDataID>{c9ae5a60-47b0-4a51-8e60-c93e0c7ea23f}</MetaDataID>
        public void AddSubPath(string subPathFullName)
        {
            if (string.IsNullOrEmpty(subPathFullName))
                return;
            int nPos = subPathFullName.IndexOf(".");
            if (nPos == -1)
                SubPath = new Path(subPathFullName,this);
            else
            {
                string name = subPathFullName.Substring(0, nPos);
                SubPath = new Path(name, this);
                SubPath.AddSubPath(subPathFullName.Substring(nPos + 1));
            }

        }

        /// <MetaDataID>{962dacbd-97d3-4dc9-9c8a-4ae319fc6601}</MetaDataID>
        internal static Path CreatePath(string pathFullName)
        {
            if (string.IsNullOrEmpty(pathFullName))
                return null;
            int nPos = pathFullName.IndexOf(".");
            if (nPos == -1)
                return new PathHead(pathFullName);
            else
            {
                string name = pathFullName.Substring(0, nPos);
                Path path = new PathHead(name);
                path.AddSubPath(pathFullName.Substring(nPos + 1));
                return path;

            }

            
        }
    }
}
