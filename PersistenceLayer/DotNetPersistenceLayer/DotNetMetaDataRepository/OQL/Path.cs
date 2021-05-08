using System;

namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
	/// <summary>
	/// </summary>
	/// <MetaDataID>{B4668BD1-F50C-4685-999A-8AAF580C28AC}</MetaDataID>
	public class PathHead:Path
	{
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{E758F36F-CD17-4D75-BC61-BE6C8ABF87D8}</MetaDataID>
		private System.DateTime _TimePeriodStartDate;
		/// <MetaDataID>{7A1B7090-3596-4C5B-B04B-6A66624C1888}</MetaDataID>
		public System.DateTime TimePeriodStartDate
		{
			get
			{
				return _TimePeriodStartDate;
			}
		}
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{5182BEEF-A2D3-47C2-83FB-99CBEE1C3E5B}</MetaDataID>
		private System.DateTime _TimePeriodEndDate;
		/// <MetaDataID>{BA80358F-FF90-4AF9-A0C6-2C9ED874EA96}</MetaDataID>
		public System.DateTime TimePeriodEndDate
		{
			get
			{
				return _TimePeriodEndDate;
			}
		}
		/// <MetaDataID>{48A2AF04-4AE4-44CD-A628-AABD51B1A4EA}</MetaDataID>
		public override string AliasName
		{
			get
			{
                string aliasName = null;
                if (ParserNode == null)
                    return null;
				if(ParserNode.Name=="PathAlias")
					aliasName= ParserNode["Name"][Parser.ParserNodeField.Value];
				if(ParserNode.Name=="TimePeriodPathAlias")
					aliasName= ParserNode["PathAlias"]["Name"][Parser.ParserNodeField.Value];
                if (aliasName!=null&&aliasName.Length>2&&aliasName[0] == '[' &&
                    aliasName[aliasName.Length - 1] == ']')
                {
                    return aliasName.Substring(1, aliasName.Length - 2);
                }


                return aliasName;
			}
		}
	

		/// <MetaDataID>{2731CD87-02B1-462A-8767-766D0C19303E}</MetaDataID>
		public bool HasTimePeriodConstrain
		{
			get
			{
                if (ParserNode == null)
                    return false;
				if(ParserNode.Name=="TimePeriodPathAlias")
					return true;
				else
					return false;
			}
		}
		/// <MetaDataID>{BFE5013A-A94B-4BB2-9674-CF605D256949}</MetaDataID>
		public override string Name
		{
			get
			{
                if (ParserNode == null)
                    return base.Name;
                string name = null;
				if(ParserNode.Name=="PathAlias")
				{
					name =	ParserNode["Path"]["ClassOrAlias"]["Name"][Parser.ParserNodeField.Value];
				}
				else if(ParserNode.Name=="TimePeriodPathAlias")
				{
					name =	ParserNode["PathAlias"]["Path"]["ClassOrAlias"]["Name"][Parser.ParserNodeField.Value];
				}
                else if (ParserNode.Name == "Function_Ref")
                {
                    name = (ParserNode["OQL_Function_Name"][0] as Parser.ParserNode).Name;
                }
                else
                    name = ParserNode["ClassOrAlias"]["Name"][Parser.ParserNodeField.Value];

                if (name != null && name.Length > 2 && name[0] == '[' &&
                        name[name.Length - 1] == ']')
                {
                    return name.Substring(1, name.Length - 2);
                }
                return name;
			}
		}

        /// <MetaDataID>{55d7d63e-281d-467c-984a-3f64db7a4a9d}</MetaDataID>
        public PathHead(string name)
            : base(name,null)
        {
        }

		/// <MetaDataID>{01C9F159-6A0A-4C6F-8807-2F0566554CDA}</MetaDataID>
		public PathHead(Parser.ParserNode parserNode):base(null)
		{
			ParserNode=parserNode;
			if(parserNode==null)
				throw new System.Exception("Parameter parserNode must be not null.");

			if(ParserNode.Name!="PathAlias"&&
                ParserNode.Name!="Path"&&
                ParserNode.Name!="TimePeriodPathAlias"&&
                ParserNode.Name != "Function_Ref")
				throw new System.Exception("Parameter parserNode is not path.");

			Parser.ParserNode pathMemberNode =null;
            if (ParserNode.Name == "Path")
            {
                pathMemberNode = ParserNode["PathMember"] as Parser.ParserNode;
                if(pathMemberNode==null)
                    pathMemberNode=parserNode["RecursivePathMember"] as Parser.ParserNode;
                if (pathMemberNode == null)
                    pathMemberNode = parserNode["Function_Ref"] as Parser.ParserNode;

                    
            }

            if (ParserNode.Name == "PathAlias")
            {
                pathMemberNode = ParserNode["Path"]["PathMember"] as Parser.ParserNode;
                if (pathMemberNode == null)
                    pathMemberNode = parserNode["Path"]["RecursivePathMember"] as Parser.ParserNode;
                if (pathMemberNode == null)
                    pathMemberNode = parserNode["Path"]["Function_Ref"] as Parser.ParserNode;


            }

            if (ParserNode.Name == "TimePeriodPathAlias")
            {
                pathMemberNode = ParserNode["PathAlias"]["Path"]["PathMember"] as Parser.ParserNode;
                if (pathMemberNode == null)
                    pathMemberNode = ParserNode["PathAlias"]["Path"]["RecursivePathMember"] as Parser.ParserNode;
            }
            if (ParserNode.Name == "Function_Ref")
            {
                if ((ParserNode["OQL_Function_Name"][0] as Parser.ParserNode).Name.ToLower() == "count")
                {
                    //ParserNode = ParserNode["PathMember"] as Parser.ParserNode;
                    _AggregationPath = true;
                    if (parserNode["Scalar_Expression"] is Parser.ParserNode)
                    {
                        parserNode = parserNode["Scalar_Expression"] as Parser.ParserNode;
                        while (parserNode.Name != "Scalar_Item" && parserNode.ChildNodes.Count == 1)
                            parserNode = parserNode[0] as Parser.ParserNode;

                        if (parserNode.Name != "Scalar_Item")
                            throw new System.Exception("Wrang count expresion.");
                    }

                    //new PathHead(ParserNode)
                    if (parserNode["Path"] != null || parserNode["PathAlias"] != null)
                    {
                        if (parserNode["PathAlias"] != null)
                            AggregateExpressionPath = new PathHead(parserNode["PathAlias"] as Parser.ParserNode);

                        if (parserNode["Path"] != null)
                            AggregateExpressionPath = new PathHead(parserNode["Path"] as Parser.ParserNode);

                    }
                }
            }




			if(pathMemberNode!=null)
				SubPath=new Path(pathMemberNode,this);
			if(HasTimePeriodConstrain)
			{
				Parser.ParserNode TimePeriodParserNode=ParserNode.ChildNodes.GetAt(2);
				Parser.ParserNode StartDateParserNode=TimePeriodParserNode.ChildNodes.GetAt(1).ChildNodes.GetAt(1);
				Parser.ParserNode StartDateLocaleParserNode=null;
				if(TimePeriodParserNode.ChildNodes.GetAt(1).ChildNodes.Count>1)
					StartDateLocaleParserNode=TimePeriodParserNode.ChildNodes.GetAt(1).ChildNodes.GetAt(2);

				Parser.ParserNode EndDateParserNode=TimePeriodParserNode.ChildNodes.GetAt(2).ChildNodes.GetAt(1);
				Parser.ParserNode EndDateLocaleParserNode=null;
				if(TimePeriodParserNode.ChildNodes.GetAt(2).ChildNodes.Count>1)
					EndDateLocaleParserNode=TimePeriodParserNode.ChildNodes.GetAt(2).ChildNodes.GetAt(2);

				
					
				if(EndDateLocaleParserNode==null && StartDateLocaleParserNode==null)
				{
					_TimePeriodStartDate=System.DateTime.Parse(StartDateParserNode.Value);
					_TimePeriodEndDate=System.DateTime.Parse(EndDateParserNode.Value);
					// Error Prone εάν δεν περάσει το parse εγείρει exception αλλαντάλλων
				}
				else
				{
					System.Globalization.CultureInfo StartDateLocale=null,EndDateLocale=null;
					if(EndDateLocaleParserNode==null)
						EndDateLocaleParserNode=StartDateLocaleParserNode;
					if(StartDateLocaleParserNode==null)
						StartDateLocaleParserNode=EndDateLocaleParserNode;

#if DeviceDotNet
                    
                    StartDateLocale = CultureInfoHelper.GetCultureInfo((int)System.Convert.ChangeType(StartDateLocaleParserNode.ChildNodes.GetAt(1).Value, typeof(int), System.Globalization.CultureInfo.CurrentCulture.NumberFormat));// OOAdvantech.cu   new System.Globalization.CultureInfo((int)System.Convert.ChangeType(StartDateLocaleParserNode.ChildNodes.GetAt(1).Value, typeof(int), System.Globalization.CultureInfo.CurrentCulture.NumberFormat));
                    EndDateLocale = CultureInfoHelper.GetCultureInfo((int)System.Convert.ChangeType(EndDateLocaleParserNode.ChildNodes.GetAt(1).Value, typeof(int), System.Globalization.CultureInfo.CurrentCulture.NumberFormat));
                    _TimePeriodStartDate = System.DateTime.Parse(StartDateParserNode.Value, StartDateLocale);
                    _TimePeriodEndDate = System.DateTime.Parse(EndDateParserNode.Value, EndDateLocale);
                    // Error Prone εάν δεν περάσει το parse εγείρει exception αλλαντάλλων

#else
                    StartDateLocale = new System.Globalization.CultureInfo((int)System.Convert.ChangeType(StartDateLocaleParserNode.ChildNodes.GetAt(1).Value,typeof(int),System.Globalization.CultureInfo.CurrentCulture.NumberFormat));
					EndDateLocale=new System.Globalization.CultureInfo((int)System.Convert.ChangeType(EndDateLocaleParserNode.ChildNodes.GetAt(1).Value,typeof(int),System.Globalization.CultureInfo.CurrentCulture.NumberFormat));
					_TimePeriodStartDate=System.DateTime.Parse(StartDateParserNode.Value,StartDateLocale);
					_TimePeriodEndDate=System.DateTime.Parse(EndDateParserNode.Value,EndDateLocale);
					// Error Prone εάν δεν περάσει το parse εγείρει exception αλλαντάλλων
#endif
                }

            }
		}

	}
}

