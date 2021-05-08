using System;
namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
	/// <MetaDataID>{954F3939-79B4-4468-8D56-0A3BAAC147CC}</MetaDataID>
    [Serializable]
    public class ObjectAttributeComparisonTerm : ComparisonTerm
	{

        ObjectAttributeComparisonTerm()
            : base(null)
        {

        }
        internal override ComparisonTerm Clone(System.Collections.Generic.Dictionary<object, object> clonedObjects)
        {
            ObjectAttributeComparisonTerm newObjectAttributeComparisonTerm = new ObjectAttributeComparisonTerm();
            object dataNode = null;
            if (clonedObjects.TryGetValue(DataNode, out dataNode))
                newObjectAttributeComparisonTerm.DataNode = dataNode as DataNode;
            else
                newObjectAttributeComparisonTerm.DataNode = DataNode.Clone(clonedObjects);

            newObjectAttributeComparisonTerm._ValueType = _ValueType;

            return newObjectAttributeComparisonTerm;
        }
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
				_ValueType=attribute.Type.GetExtensionMetaObject<System.Type>();

				
				return _ValueType;
			}
		}
		/// <MetaDataID>{65EEA8ED-E342-4883-A34A-512D8B08589A}</MetaDataID>
		public override string TranslatedExpression
		{
			get
			{
				return null;
			}
		}
        /// <MetaDataID>{D5CC7F5A-8C3F-4394-B6F3-B1D908355628}</MetaDataID>
        static private System.Collections.Generic.Dictionary<object, object> TypeMap = new System.Collections.Generic.Dictionary<object, object>();

		/// <MetaDataID>{CCBBF898-238D-49AA-AA01-1F9A8BF8E589}</MetaDataID>
		void TypeCheck(ComparisonTerm theOtherComparisonTerm)
		{
			if(ValueType!=theOtherComparisonTerm.ValueType)
			{

				if(TypeMap.ContainsKey(theOtherComparisonTerm.ValueType))
				{
					foreach(System.Type type in TypeMap[theOtherComparisonTerm.ValueType] as System.Type[])
					{
						if(type==ValueType)
							return;//CanAssign=true;
					}
				}
				throw new System.Exception("Type mismatch at "+ComparisonTermParserNode.ParentNode.Value); 

			}

		}

		/// <MetaDataID>{8E807BD3-137A-4CA0-9041-1DD41A3CEA58}</MetaDataID>
		public override string GetCompareExpression(Criterion.ComparisonType comparisonType, ComparisonTerm theOtherComparisonTerm)
		{
			
			//TODO Test case for axception.
			if(!(theOtherComparisonTerm is ObjectAttributeComparisonTerm ||theOtherComparisonTerm is LiteralComparisonTerm||theOtherComparisonTerm is ParameterComparisonTerm))
				throw new System.Exception("Comparison error "+ComparisonTermParserNode.ParentNode.Value+" .");

            //TODO Πρέπει να γίνει κατά την διάρκεια του error check του query;
			TypeCheck(theOtherComparisonTerm);

			object wew=ValueType;

			object wew1=theOtherComparisonTerm.ValueType;
			

			string compareExpression=TranslatedExpression;
			if(comparisonType==Criterion.ComparisonType.Equal)
				compareExpression+=" = ";
			if(comparisonType==Criterion.ComparisonType.NotEqual)
				compareExpression+=" <> ";
			if(comparisonType==Criterion.ComparisonType.GreaterThan)
				compareExpression+=" > ";
			if(comparisonType==Criterion.ComparisonType.LessThan)
				compareExpression+=" < ";
			return compareExpression+=theOtherComparisonTerm.TranslatedExpression;
		}
        /// <MetaDataID>{99d9a198-c724-45da-a413-3d1f837b844a}</MetaDataID>
        static ObjectAttributeComparisonTerm()
        {
            TypeMap.Add(typeof(short), new System.Type[7] { typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double) });
            TypeMap.Add(typeof(ushort), new System.Type[7] { typeof(short), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double) });
            TypeMap.Add(typeof(int), new System.Type[7] { typeof(short), typeof(ushort), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double) });
            TypeMap.Add(typeof(uint), new System.Type[7] { typeof(short), typeof(int), typeof(ushort), typeof(long), typeof(ulong), typeof(float), typeof(double) });
            TypeMap.Add(typeof(long), new System.Type[7] { typeof(short), typeof(int), typeof(uint), typeof(ushort), typeof(ulong), typeof(float), typeof(double) });
            TypeMap.Add(typeof(ulong), new System.Type[7] { typeof(short), typeof(int), typeof(uint), typeof(long), typeof(ushort), typeof(float), typeof(double) });
            TypeMap.Add(typeof(float), new System.Type[7] { typeof(short), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(ushort), typeof(double) });
            TypeMap.Add(typeof(double), new System.Type[7] { typeof(short), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(ushort) });
           

        }
		/// <MetaDataID>{C9E5CDC5-0093-4BDB-BADD-1BA7469F7B1F}</MetaDataID>
		internal protected ObjectAttributeComparisonTerm(Parser.ParserNode ComparisonTermParserNode, ObjectsContextQuery oqlStatement):base(ComparisonTermParserNode,oqlStatement)
		{
            DataNode = oqlStatement.PathDataNodeMap[ComparisonTermParserNode["Path"]] as DataNode;
            //if (Route == null)
            //    Route = new System.Collections.Generic.Stack<DataNode>();

		}
        /// <MetaDataID>{cfecb8f5-bc58-4522-adf0-8913bbb793ae}</MetaDataID>
        public ObjectAttributeComparisonTerm(DataNode dataNode, ObjectQuery oqlStatement)
            : base(oqlStatement)
        {
            DataNode = dataNode;
            //if (Route == null)
            //    Route = new System.Collections.Generic.Stack<DataNode>();

        }


        //public ObjectAttributeComparisonTerm(DataNode dataNode, System.Collections.Generic.Stack<DataNode> route, ObjectQuery oqlStatement)
        //    : base(oqlStatement)
        //{
        //    Route = route;
        //    dataNode.ParticipateInWereClause = true;
        //    DataNode = dataNode;
        //}
        // readonly System.Collections.Generic.Stack<DataNode> Route;
        /// <MetaDataID>{4706ce9c-b67c-4b64-9718-f80ff603944e}</MetaDataID>
        public override string ToString()
        {
            return DataNode.FullName;
        }

	}
}
