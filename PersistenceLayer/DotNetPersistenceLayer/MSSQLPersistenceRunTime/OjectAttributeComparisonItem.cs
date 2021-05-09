namespace OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage
{
	/// <MetaDataID>{954F3939-79B4-4468-8D56-0A3BAAC147CC}</MetaDataID>
	public class ObjectAttributeComparisonTerm : ComparisonTerm
	{
		/// <MetaDataID>{346BC117-B1F1-4045-89B9-A68BE71BBE09}</MetaDataID>
		public DataNode DataNode;
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{CCD415F5-745C-47D5-8D67-F3968FAA7790}</MetaDataID>
		private System.Type _ValueType;
		/// <MetaDataID>{5C892D3F-7E47-4E30-9850-9CA0E09606CD}</MetaDataID>
		public override System.Type ValueType
		{
			get
			{
				if(_ValueType!=null)
					return _ValueType;
			
				//DataNode dataNode=OQLStatement.PathDataNodeMap[ComparisonTermParserNode["Path"]] as DataNode;
				MetaDataRepository.Attribute attribute =DataNode.AssignedMetaObject as  MetaDataRepository.Attribute;
				_ValueType=ModulePublisher.ClassRepository.GetType(attribute.Type.FullName,"");

				
				return _ValueType;
			}
		}
		/// <MetaDataID>{65EEA8ED-E342-4883-A34A-512D8B08589A}</MetaDataID>
		public override string SQLExpression
		{
			get
			{
				string _SQLExpression=null;
				//DataNode dataNode=OQLStatement.PathDataNodeMap[ComparisonTermParserNode["Path"]] as DataNode;
				_SQLExpression+=DataNode.ParentDataNode.Alias;
				_SQLExpression+=".";
				_SQLExpression+=DataNode.Name;
				return _SQLExpression;
			}
		}
		/// <MetaDataID>{D5CC7F5A-8C3F-4394-B6F3-B1D908355628}</MetaDataID>
		private System.Collections.Hashtable TypeMap=new System.Collections.Hashtable();

		/// <MetaDataID>{CCBBF898-238D-49AA-AA01-1F9A8BF8E589}</MetaDataID>
		void TypeCheck(ComparisonTerm theOtherComparisonTerm)
		{
			if(ValueType!=theOtherComparisonTerm.ValueType)
			{
				//				System.ComponentModel.TypeConverter typeConverter=null;
				//				try
				//				{
				//					if(ValueType==typeof(string)||theOtherComparisonTerm.ValueType==typeof(string))
				//						throw new System.Exception("Type mismatch at "+ComparisonTermParserNode.ParentNode.Value); 
				//
				//					typeConverter=System.ComponentModel.TypeDescriptor.GetConverter(theOtherComparisonTerm.ValueType);
				//					if(!typeConverter.CanConvertFrom(ValueType))
				//						throw new System.Exception("Type mismatch at "+ComparisonTermParserNode.ParentNode.Value); 
				//
				//				}
				//				catch(System.Exception Error)
				//				{
				//					throw new System.Exception("Type mismatch at "+ComparisonTermParserNode.ParentNode.Value,Error); 
				//				}
				//bool CanAssign=false;
				if(TypeMap.Contains(theOtherComparisonTerm.ValueType))
				{
					foreach(System.Type type in TypeMap[theOtherComparisonTerm.ValueType] as System.Type[])
					{
						if(type==ValueType)
							return;//CanAssign=true;
					}
				}
				throw new System.Exception("Type mismatch at "+ComparisonTermParserNode.ParentNode.Value); 
//				if(!CanAssign)
//					throw new System.Exception("Type mismatch at "+ComparisonTermParserNode.ParentNode.Value); 
				//
				//				if(theOtherComparisonTerm is LiteralComparisonTerm&&theOtherComparisonTerm.ValueType==typeof(System.Int16))
				//				{
				//					if(!(ValueType==typeof(short)||ValueType==typeof(ushort)||ValueType==typeof(int)||ValueType==typeof(uint)
				//						||ValueType==typeof(long)||ValueType==typeof(ulong)||ValueType==typeof(double)||ValueType==typeof(float) ))
				//						throw new System.Exception("Type mismatch at "+ComparisonTermParserNode.ParentNode.Value); 
				//				}
				//				else if(theOtherComparisonTerm is LiteralComparisonTerm&&theOtherComparisonTerm.ValueType==typeof(System.Double))
				//				{
				//					if(!(ValueType==typeof(double)||ValueType==typeof(float) ))
				//						throw new System.Exception("Type mismatch at "+ComparisonTermParserNode.ParentNode.Value); 
				//				}

			}

		}

		/// <MetaDataID>{8E807BD3-137A-4CA0-9041-1DD41A3CEA58}</MetaDataID>
		public override string GetCompareExpression(Criterion.ComparisonType comparisonType, ComparisonTerm theOtherComparisonTerm)
		{
			
			//TODO Test case for axception.
			if(!(theOtherComparisonTerm is ObjectAttributeComparisonTerm ||theOtherComparisonTerm is LiteralComparisonTerm||theOtherComparisonTerm is ParameterComparisonTerm))
				throw new System.Exception("Comparison error "+ComparisonTermParserNode.ParentNode.Value+" .");

			TypeCheck(theOtherComparisonTerm);

			object wew=ValueType;

			object wew1=theOtherComparisonTerm.ValueType;
			

			string compareExpression=SQLExpression;
			if(comparisonType==Criterion.ComparisonType.Equal)
				compareExpression+=" = ";
			if(comparisonType==Criterion.ComparisonType.NotEqual)
				compareExpression+=" <> ";
			if(comparisonType==Criterion.ComparisonType.GreaterThan)
				compareExpression+=" > ";
			if(comparisonType==Criterion.ComparisonType.LessThan)
				compareExpression+=" < ";
			return compareExpression+=theOtherComparisonTerm.SQLExpression;
		}
		/// <MetaDataID>{C9E5CDC5-0093-4BDB-BADD-1BA7469F7B1F}</MetaDataID>
		 internal ObjectAttributeComparisonTerm(Parser.ParserNode ComparisonTermParserNode, OQLStatement oqlStatement):base(ComparisonTermParserNode,oqlStatement)
		{
			TypeMap.Add(typeof(short),new System.Type[7]{typeof(ushort),typeof(int),typeof(uint),typeof(long),typeof(ulong),typeof(float),typeof(double)});
			TypeMap.Add(typeof(ushort),new System.Type[7]{typeof(short),typeof(int),typeof(uint),typeof(long),typeof(ulong),typeof(float),typeof(double)});
			TypeMap.Add(typeof(int),new System.Type[7]{typeof(short),typeof(ushort),typeof(uint),typeof(long),typeof(ulong),typeof(float),typeof(double)});
			TypeMap.Add(typeof(uint),new System.Type[7]{typeof(short),typeof(int),typeof(ushort),typeof(long),typeof(ulong),typeof(float),typeof(double)});
			TypeMap.Add(typeof(long),new System.Type[7]{typeof(short),typeof(int),typeof(uint),typeof(ushort),typeof(ulong),typeof(float),typeof(double)});
			TypeMap.Add(typeof(ulong),new System.Type[7]{typeof(short),typeof(int),typeof(uint),typeof(long),typeof(ushort),typeof(float),typeof(double)});
			TypeMap.Add(typeof(float),new System.Type[7]{typeof(short),typeof(int),typeof(uint),typeof(long),typeof(ulong),typeof(ushort),typeof(double)});
			TypeMap.Add(typeof(double),new System.Type[7]{typeof(short),typeof(int),typeof(uint),typeof(long),typeof(ulong),typeof(float),typeof(ushort)});
			 DataNode=OQLStatement.PathDataNodeMap[ComparisonTermParserNode["Path"]] as DataNode;
		}
	}
}
