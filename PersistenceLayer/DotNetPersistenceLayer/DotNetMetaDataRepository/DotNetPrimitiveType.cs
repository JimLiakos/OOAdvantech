namespace OOAdvantech.DotNetMetaDataRepository
{
	/// <MetaDataID>{1D234812-3D0F-4E22-8C04-F871AA3FD9AD}</MetaDataID>
	public class PrimitiveType : MetaDataRepository.Classifier
	{

        public PrimitiveType()
        {

        }
		/// <MetaDataID>{15E7C8AF-16E5-4058-BBEB-B9C09C757E50}</MetaDataID>
		public System.Type WrType;
		/// <MetaDataID>{AD94D277-FF54-4A9E-A747-22D303B07EF2}</MetaDataID>
		 public PrimitiveType(System.Type theType)
		{
			WrType=theType;

			 MetaObjectMapper.AddMetaObject(this,WrType.FullName);

		}
	}
}
