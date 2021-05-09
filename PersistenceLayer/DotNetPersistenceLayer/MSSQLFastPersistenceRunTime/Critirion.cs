using System;

namespace OOAdvantech.MSSQLFastPersistenceRunTime
{
	/// <summary>
	/// 
	/// </summary>
	public class Critirion
	{
		string Alias;
		DotNetMetaDataRepository.Class Class;
		public bool IsTrue(object _object)
		{
			try
			{

				IComparable rightAsValue=null;
				IComparable leftAsValue=null;
				if(RightAsFieldInfo!=null&&LeftAsFieldInfo==null)
				{
					//rightAsValue= RightAsFieldInfo.GetValue(_object) as IComparable;
                    rightAsValue = Member<object>.GetValue(_object, RightAsFieldInfo) as IComparable;
					leftAsValue=LeftAsValue as IComparable;
				}
				
				if(RightAsFieldInfo==null&&LeftAsFieldInfo!=null)
				{
					rightAsValue=RightAsValue as IComparable;
					//leftAsValue=LeftAsFieldInfo.GetValue(_object) as IComparable;
                    leftAsValue = Member<object>.GetValue(_object, LeftAsFieldInfo) as IComparable;
				}
				if(RightAsFieldInfo!=null&&LeftAsFieldInfo!=null)
				{
                    //rightAsValue=RightAsFieldInfo.GetValue(_object) as IComparable;
                    //leftAsValue=LeftAsFieldInfo.GetValue(_object) as IComparable;

                    rightAsValue = Member<object>.GetValue(_object, RightAsFieldInfo) as IComparable;
                    leftAsValue = Member<object>.GetValue(_object, LeftAsFieldInfo) as IComparable;
				}
				
				if(ComparisonOperator=="=")
				{
					if(leftAsValue is System.Guid&&rightAsValue is System.String)
						rightAsValue=new System.Guid(rightAsValue as string);

					if(rightAsValue is System.Guid&& leftAsValue is System.String)
						leftAsValue=new System.Guid(leftAsValue as string);

					if(leftAsValue==null&&rightAsValue==null)
						return true;
					if(leftAsValue==null&&rightAsValue!=null)
						return false;

                        if(leftAsValue!=null&&rightAsValue==null)
						return false;


                    return leftAsValue.CompareTo(System.Convert.ChangeType(rightAsValue,leftAsValue.GetType())) == 0;
				}
				if(leftAsValue==null&&rightAsValue==null)
					return false;
				if(ComparisonOperator==">")
				{
					if(leftAsValue==null)
						return false;

                    return leftAsValue.CompareTo(System.Convert.ChangeType(rightAsValue, leftAsValue.GetType())) > 0;
				}
				if(ComparisonOperator=="<")
				{
					if(leftAsValue==null)
						return true;
                    return leftAsValue.CompareTo(System.Convert.ChangeType(rightAsValue, leftAsValue.GetType())) < 0;
				}
				if(ComparisonOperator=="<>")
				{
					if(leftAsValue==null)
						return true;
                    return leftAsValue.CompareTo(System.Convert.ChangeType(rightAsValue, leftAsValue.GetType())) != 0;
				}
			}
			catch(System.Exception Error)
			{
				throw new System.Exception(Error.Message,Error);
			}
			return false;

		}
		string ComparisonOperator;
		public Critirion(Parser.ParserNode critirion,DotNetMetaDataRepository.Class _class,string alias)
		{
			Alias=alias;
			Class=_class;
			LoadLeftItemInfos(critirion.ChildNodes.GetAt(1));
			ComparisonOperator=critirion.ChildNodes.GetAt(2).Value;
			LoadRightItemInfos(critirion.ChildNodes.GetAt(3));
			
		}
		void LoadLeftItemInfos(Parser.ParserNode comparisonItem)
		{
			if(comparisonItem.ChildNodes.GetAt(1).Name=="Path")
				LeftAsFieldInfo=GetFieldInfoFor(comparisonItem);
			else if(comparisonItem.ChildNodes.GetAt(1).Name=="literal")
			{
				LeftAsValue=GetValueFor(comparisonItem);
			}

		}
		object GetValueFor(Parser.ParserNode comparisonItem)
		{
			Parser.ParserNode Value=comparisonItem.ChildNodes.GetAt(1).ChildNodes.GetAt(1);
			//"float_numeric_literal"
			//"date_literal"
			//"numeric_literal"
			//"__money_literal"
			//"single_quotedstring"
			//"quotedstring"
			//"NULL"


            if (Value.Name == "SingleQuatedStringLiteral")
				return Value.Value.Replace("'","");
            if (Value.Name == "StringLiteral")
				return Value.Value.Replace("\"","");

            if (Value.Name == "NumericLiteral")
				return System.Convert.ToInt64(Value.Value,10);

            if (Value.Name == "RealLiteral")
				return System.Convert.ToDouble(Value.Value,System.Globalization.CultureInfo.CreateSpecificCulture("en"));




			int erte=0;
			return null;

		}
		System.Reflection.FieldInfo GetFieldInfoFor(Parser.ParserNode comparisonItem)
		{
			string AttributeName= null;
			if(comparisonItem.ChildNodes.GetAt(1).ChildNodes.Count>=2)
				AttributeName=comparisonItem.ChildNodes.GetAt(1).ChildNodes.GetAt(2).ChildNodes.GetAt(1).Value;
			else
				AttributeName=comparisonItem.ChildNodes.GetAt(1).ChildNodes.GetAt(1).ChildNodes.GetAt(1).Value;

			AttributeName=AttributeName.Trim();
			foreach(DotNetMetaDataRepository.Attribute attribute in Class.GetAttributes(true))
			{
				if(attribute.Name.Trim()==AttributeName)
					return Class.GetFieldMember(attribute);
			}
			return null;
		}
		void LoadRightItemInfos(Parser.ParserNode comparisonItem)
		{
			if(comparisonItem.ChildNodes.GetAt(1).Name=="Path")
				RightAsFieldInfo=GetFieldInfoFor(comparisonItem);
            else if (comparisonItem.ChildNodes.GetAt(1).Name == "Literal")
			{
				RightAsValue=GetValueFor(comparisonItem);
			}


		}
		System.Reflection.FieldInfo LeftAsFieldInfo;
		System.Reflection.FieldInfo RightAsFieldInfo;
		object RightAsValue;
		object LeftAsValue;

	}
}
