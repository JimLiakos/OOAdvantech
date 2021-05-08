namespace OOAdvantech.MetaDataRepository
{
	/// <MetaDataID>{AA4957A4-E537-4370-A50B-F70ABD94ED52}</MetaDataID>
	public class DeleteCommand
	{
		/// <MetaDataID>{1E8502B3-4EF4-463B-9A01-F1F3EA90B812}</MetaDataID>
		public MetaObject CandidateForDeleteObject;
		/// <MetaDataID>{777C4B47-036C-4E35-B26B-661B415059A9}</MetaDataID>
		public System.Collections.IList UpdateCollection;

		/// <MetaDataID>{56B4E14F-FF11-4F2E-A167-3303A9123749}</MetaDataID>
		public DeleteCommand(MetaObject theMetaObject,System.Collections.IList theUpdateCollection)
		{
			 CandidateForDeleteObject=theMetaObject;
			 UpdateCollection=theUpdateCollection;
		}
	}
}
