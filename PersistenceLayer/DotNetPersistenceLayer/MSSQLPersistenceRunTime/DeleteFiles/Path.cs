using System;

namespace OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage
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
				if(ParserNode.Name=="PathAlias")
					return ParserNode["name"][Parser.ParserNode.Field.Value];
				if(ParserNode.Name=="TimePeriodPathAlias")
					return ParserNode["PathAlias"]["name"][Parser.ParserNode.Field.Value];
				return null;
			}
		}
	

		/// <MetaDataID>{2731CD87-02B1-462A-8767-766D0C19303E}</MetaDataID>
		public bool HasTimePeriodConstrain
		{
			get
			{
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
				if(ParserNode.Name=="PathAlias")
				{
					return	ParserNode["Path"]["ClassOrAlias"]["name"][Parser.ParserNode.Field.Value];
				}
				else if(ParserNode.Name=="TimePeriodPathAlias")
				{
					return	ParserNode["PathAlias"]["Path"]["ClassOrAlias"]["name"][Parser.ParserNode.Field.Value];
				}
				else
					return	ParserNode["ClassOrAlias"]["name"][Parser.ParserNode.Field.Value];
			}
		}

		/// <MetaDataID>{01C9F159-6A0A-4C6F-8807-2F0566554CDA}</MetaDataID>
		 public PathHead(Parser.ParserNode parserNode):base(null)
		{
			ParserNode=parserNode;
			if(parserNode==null)
				throw new System.Exception("Parameter parserNode must be not null.");

			if(ParserNode.Name!="PathAlias"&&ParserNode.Name!="Path"&&ParserNode.Name!="TimePeriodPathAlias")
				throw new System.Exception("Parameter parserNode is not path.");

			Parser.ParserNode pathMemberNode =null;
			if(ParserNode.Name=="Path")
				pathMemberNode=ParserNode["PathMember"] as Parser.ParserNode;

			if(ParserNode.Name=="PathAlias")
				pathMemberNode=ParserNode["Path"]["PathMember"]as Parser.ParserNode;

			if(ParserNode.Name=="TimePeriodPathAlias")
				pathMemberNode=ParserNode["PathAlias"]["Path"]["PathMember"]as Parser.ParserNode;

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
					// Error Prone ��� ��� ������� �� parse ������� exception �����������
				}
				else
				{
					System.Globalization.CultureInfo StartDateLocale=null,EndDateLocale=null;
					if(EndDateLocaleParserNode==null)
						EndDateLocaleParserNode=StartDateLocaleParserNode;
					if(StartDateLocaleParserNode==null)
						StartDateLocaleParserNode=EndDateLocaleParserNode;
					StartDateLocale=new System.Globalization.CultureInfo((int)System.Convert.ChangeType(StartDateLocaleParserNode.ChildNodes.GetAt(1).Value,typeof(int)));
					EndDateLocale=new System.Globalization.CultureInfo((int)System.Convert.ChangeType(EndDateLocaleParserNode.ChildNodes.GetAt(1).Value,typeof(int)));
					_TimePeriodStartDate=System.DateTime.Parse(StartDateParserNode.Value,StartDateLocale);
					_TimePeriodEndDate=System.DateTime.Parse(EndDateParserNode.Value,EndDateLocale);
					// Error Prone ��� ��� ������� �� parse ������� exception �����������
				}

			}
		}

	}
}