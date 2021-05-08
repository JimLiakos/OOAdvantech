namespace OOAdvantech.MetaDataRepository
{
	/// <MetaDataID>{5AED87B6-E45C-4F44-8357-6E669E26FAF6}</MetaDataID>
	public class MetaDataException : System.Exception
	{
		/// <MetaDataID>{C0A5A346-CCA8-4D02-A759-BE801D240ADB}</MetaDataID>
		public MetaObject.MetaDataError MetaDataError;
		

		/// <MetaDataID>{4AEB5C7E-79D8-466B-8073-8965B2107DA1}</MetaDataID>
		public MetaDataException(MetaObject.MetaDataError metaDataError, System.Exception innerException):base(metaDataError.ErrorMessage,innerException)
		{
			MetaDataError=metaDataError;
		}
		/// <MetaDataID>{33ADF5D4-86D9-48C1-9624-7C66CD8CCB8F}</MetaDataID>
		public MetaDataException(MetaObject.MetaDataError metaDataError):base(metaDataError.ErrorMessage)
		{
			MetaDataError=metaDataError;
		}

	}
}
