using System;
namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
	/// <MetaDataID>{A21E418D-7C92-485B-BDF9-0E4A4212458F}</MetaDataID>
    [Serializable]
    public class ObjectIDComparisonTerm : ComparisonTerm
	{

        ObjectIDComparisonTerm():base(null)
        {

        }

        internal override ComparisonTerm Clone(System.Collections.Generic.Dictionary<object, object> clonedObjects)
        {
            ObjectIDComparisonTerm newComparisonTerm = new ObjectIDComparisonTerm();
            newComparisonTerm.MultiPartObjectID = new System.Collections.Generic.Dictionary<string, object>(MultiPartObjectID);
            return newComparisonTerm;
        }

        /// <MetaDataID>{afcb7282-8426-49f0-b022-b4950d4335a8}</MetaDataID>
        public System.Collections.Generic.Dictionary<string, object> MultiPartObjectID = new System.Collections.Generic.Dictionary<string, object>();
		/// <MetaDataID>{08D584C4-25BA-460E-A2C4-4083F599CAE3}</MetaDataID>
		public override string  GetCompareExpression(Criterion.ComparisonType comparisonType, ComparisonTerm theOtherComparisonTerm)
		{
			if(theOtherComparisonTerm is ObjectComparisonTerm)
				return theOtherComparisonTerm.GetCompareExpression(comparisonType,this);
			else
				throw new System.Exception("You can't compare ObjectID with other than Object");
		}
		/// <MetaDataID>{0B83166F-128D-4504-8BFE-D9DAAA71DBE7}</MetaDataID>
		internal ObjectIDComparisonTerm(Parser.ParserNode ComparisonTermParserNode, ObjectsContextQuery oqlStatement):base(ComparisonTermParserNode,oqlStatement)
		{
            if (ComparisonTermParserNode["ObjectID"]["Literal"] != null)
            {

                MultiPartObjectID.Add("ObjectID",LiteralComparisonTerm.GetLiteralValue(ComparisonTermParserNode["ObjectID"]["Literal"] as Parser.ParserNode));

            }
            else if (ComparisonTermParserNode["ObjectID"]["ObjectIDFieldList"]["ObjectIDField"] is Parser.ParserNodeCollection)
            {
                foreach (Parser.ParserNode objectIDField in ComparisonTermParserNode["ObjectID"]["ObjectIDFieldList"]["ObjectIDField"] as Parser.ParserNodeCollection)
                {
                    string fieldName = (objectIDField["Name"] as Parser.ParserNode).Value;
                    Parser.ParserNode literal = objectIDField["Literal"] as Parser.ParserNode;
                    if (!MultiPartObjectID.ContainsKey(fieldName))
                        MultiPartObjectID.Add(fieldName, LiteralComparisonTerm.GetLiteralValue(literal));
                    else
                        throw new System.Exception("Syntax error :"+ComparisonTermParserNode.Value);
                }
            }
            else if (ComparisonTermParserNode["ObjectID"]["ObjectIDFieldList"]["ObjectIDField"] is Parser.ParserNode)
            {
                string fieldName = (ComparisonTermParserNode["ObjectID"]["ObjectIDFieldList"]["ObjectIDField"]["Name"] as Parser.ParserNode).Value;
                Parser.ParserNode literal = ComparisonTermParserNode["ObjectID"]["ObjectIDFieldList"]["ObjectIDField"]["Literal"] as Parser.ParserNode;
                object literValue = LiteralComparisonTerm.GetLiteralValue(literal);
                if (ComparisonTermParserNode["ObjectID"]["ObjectIDFieldList"]["ObjectIDField"]["ObjectIDFieldType"] is Parser.ParserNode)
                {
                    string typeFullName = (ComparisonTermParserNode["ObjectID"]["ObjectIDFieldList"]["ObjectIDField"]["ObjectIDFieldType"]["Path"] as Parser.ParserNode).Value;
                    System.Type literalType = ModulePublisher.ClassRepository.GetType(typeFullName,"");
                    if(literalType!=null)
                        literValue = Convert.ChangeType(literValue, literalType);
                }

                MultiPartObjectID.Add(fieldName, literValue);

            }
            else
                throw new System.Exception("Incosistent State");
		
		}
	}
}
