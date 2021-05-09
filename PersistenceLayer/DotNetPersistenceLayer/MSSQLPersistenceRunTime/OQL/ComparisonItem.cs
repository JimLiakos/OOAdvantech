namespace OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage
{
	/// <MetaDataID>{E23112BF-B90B-4B55-8F25-2DD5D4E5ECCC}</MetaDataID>
	/// <summary>Define an abstract class for comparison terms. Firstly has a static method (GetComparisonTermFor) which allows it to operate as factory class. Secondly has abstract method (GetCompareExpression) which returns the comparison expression. The expression is relative to the implementation class. </summary>
	public abstract class ComparisonTerm
	{
		/// <MetaDataID>{8C5B1172-FDDC-434B-B9DE-D7AA7C0AC929}</MetaDataID>
		/// <summary>Define the type of value of comparison term (string, long, double, integer  etc) </summary>
		public virtual System.Type ValueType
		{
			get
			{
				return typeof(object);
			}
		}
		/// <MetaDataID>{7B9374C9-859A-4D81-B0C6-845982F1F4EB}</MetaDataID>
		/// <summary>Return the SQL expression for the comparison term pair. The token (comparison term pair) means the comparison term object which implements the function and the function parameter theOtherComparisonTerm </summary>
		/// <param name="comparisonType">Define the type of comparison (Equal, Not Equal, Greater Than, Less Than etc.) </param>
		/// <param name="theOtherComparisonTerm">Define the second term of comparison. </param>
		public abstract string GetCompareExpression(Criterion.ComparisonType comparisonType, ComparisonTerm theOtherComparisonTerm);
	
		public enum ComparisonTermType{ObjectAttribute=0,Object,Parameter,Literal,ObjectID};


		

		/// <MetaDataID>{4ADFEEF7-7219-4A85-870D-E5E6E648CA7E}</MetaDataID>
		/// <summary>Return a specialized ComparisonTerm for the parser node. For instance if the parser node is parameter return an Parameter ComparisonTerm. </summary>
		/// <param name="comparisonTermParserNode">Define the parser node which contains the information for comparison term. The type, the value etc. </param>
		/// <param name="oqlStatement">Define the OQL statement which contain this comparison term </param>
		internal static ComparisonTerm GetComparisonTermFor(Parser.ParserNode comparisonTermParserNode, OQLStatement oqlStatement)
		{
			ComparisonTermType type=GetType(comparisonTermParserNode,oqlStatement);

			if(type==ComparisonTermType.Object)
				return new ObjectComparisonTerm(comparisonTermParserNode,oqlStatement);

			if(type==ComparisonTermType.ObjectAttribute)
				return new ObjectAttributeComparisonTerm(comparisonTermParserNode,oqlStatement);

			if(type==ComparisonTermType.ObjectID)
				return new ObjectIDComparisonTerm(comparisonTermParserNode,oqlStatement);

			if(type==ComparisonTermType.Literal)
				return new LiteralComparisonTerm(comparisonTermParserNode,oqlStatement);

			if(type==ComparisonTermType.Parameter)
				return new ParameterComparisonTerm(comparisonTermParserNode,oqlStatement);
			return null;

		}
		/// <MetaDataID>{BA308EB8-0C99-4898-B4AF-780B6F458E70}</MetaDataID>
		/// <summary>Extract the type information from the parser node. For example literal, parameter, object ID etc. </summary>
		/// <param name="comparisonTerm">Define the parser node which contains the information for comparison term. The type. the value etc. </param>
		/// <param name="oqlStatement">Define the OQL statement which contain this comparison term </param>
		private static ComparisonTermType GetType(Parser.ParserNode comparisonTerm, OQLStatement oqlStatement)
		{
			string TypeName=comparisonTerm.ChildNodes.GetFirst().Name;
			if(TypeName=="ObjectID")
				return ComparisonTermType.ObjectID;
			
			if(TypeName=="literal")
				return ComparisonTermType.Literal;
			if(TypeName=="parameter")
				return ComparisonTermType.Parameter;
			if(TypeName=="Path")
			{
				DataNode PathCorespondingObjectCollection=(DataNode)oqlStatement.PathDataNodeMap[comparisonTerm.ChildNodes.GetFirst()];
				if(PathCorespondingObjectCollection!=null)
				{
					if(typeof(RDBMSMetaDataRepository.Attribute).IsInstanceOfType(PathCorespondingObjectCollection.AssignedMetaObject))
						return ComparisonTermType.ObjectAttribute;
					if(typeof(RDBMSMetaDataRepository.Class).IsInstanceOfType(PathCorespondingObjectCollection.AssignedMetaObject))
						return ComparisonTermType.Object;
					if(typeof(RDBMSMetaDataRepository.AssociationEnd).IsInstanceOfType(PathCorespondingObjectCollection.AssignedMetaObject))
					{
						if(((RDBMSMetaDataRepository.AssociationEnd)PathCorespondingObjectCollection.AssignedMetaObject).Specification!=null)
							return ComparisonTermType.Object;
					}
				}
			}
			throw new System.Exception("Critirion term with unknown type");
		}
	

		/// <MetaDataID>{A795182F-99D2-4F39-A2E5-E4A0235B6D99}</MetaDataID>
		/// <summary>Define the OQL statement which contain this comparison term </summary>
		internal OQLStatement OQLStatement;
		/// <MetaDataID>{62296F1B-1C2B-45BE-A613-5281E27D05E9}</MetaDataID>
		/// <param name="comparisonTermParserNode">Define the parser node which contains the information for comparison term. The type. the value etc. </param>
		/// <param name="oqlStatement">Define the OQL statement which contain this comparison term </param>
		/// <summary>Define the object constructor </summary>
		 internal ComparisonTerm(Parser.ParserNode comparisonTermParserNode, OQLStatement oqlStatement)
		{
			ComparisonTermParserNode=comparisonTermParserNode;
			OQLStatement=oqlStatement;
		}
		/// <MetaDataID>{B2F4D99D-318A-43AC-8266-55FB44EF8413}</MetaDataID>
		public virtual string SQLExpression
		{
			get
			{
				return null;
			}
		}
		/// <MetaDataID>{981F9E8C-A672-4BB1-826D-4E4023D3C7F7}</MetaDataID>
		/// <summary>Define the parser node which contains the information for comparison term. The type. the value etc. </summary>
		public Parser.ParserNode ComparisonTermParserNode;
	}
}
