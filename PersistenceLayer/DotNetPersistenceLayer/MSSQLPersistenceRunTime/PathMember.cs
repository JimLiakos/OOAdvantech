namespace OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage
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
				if(_Name==null)
					_Name= ParserNode["name"][Parser.ParserNode.Field.Value];
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

		public Path(Path parent)
		{
			Parent=parent;

		}
		/// <MetaDataID>{A5E58564-AD46-4D34-901B-BEF1E6AA0F0C}</MetaDataID>
		public Path(Parser.ParserNode parserNode,Path parent)
		{
			Parent=parent;
			ParserNode=parserNode;
			if(parserNode==null)
				throw new System.Exception("Parameter parserNode must be not null.");

			if(ParserNode.Name!="PathMember")
				throw new System.Exception("Parameter parserNode is not PathMember.");

			 Parser.ParserNode pathMemberNode =ParserNode["PathMember"] as Parser.ParserNode;
			 if(pathMemberNode!=null)
				 SubPath=new Path(pathMemberNode,this);


		
		}
	}
}
