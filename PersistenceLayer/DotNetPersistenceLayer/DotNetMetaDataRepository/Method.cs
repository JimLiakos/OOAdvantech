namespace OOAdvantech.DotNetMetaDataRepository
{
	/// <MetaDataID>{68A07919-C575-4BC6-99C7-D8CEAA70DBCD}</MetaDataID>
		public class Method : MetaDataRepository.Method
		{
			public override OOAdvantech.MetaDataRepository.Classifier Owner
			{
				get
				{
					if(base.Owner!=null)
						return base.Owner;

					System.Threading.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
					try
					{
						_Owner=DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(WrMethod.DeclaringType) as MetaDataRepository.Classifier;
						if(_Owner!=null)
							return _Owner;
						if(WrMethod.DeclaringType.IsInterface)
						{
							Interface _interface=new Interface(new DotNetMetaDataRepository.Type(WrMethod.DeclaringType));
							_Owner=_interface;
						}
						if(WrMethod.DeclaringType.IsClass)
						{
							Class _class=new Class(new DotNetMetaDataRepository.Type(WrMethod.DeclaringType));
							_Owner=_class;
						}
						if(WrMethod.DeclaringType.IsPrimitive)
						{
							Primitive _primitive=new Primitive(new DotNetMetaDataRepository.Type(WrMethod.DeclaringType));
							_Owner=_primitive;
						}
						if(WrMethod.DeclaringType.IsPrimitive)
						{
							Primitive _primitive=new Primitive(new DotNetMetaDataRepository.Type(WrMethod.DeclaringType));
							_Owner=_primitive;
						}

						return _Owner;
					}
					finally
					{
						ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
					}

				}
			}

			public override OOAdvantech.MetaDataRepository.Namespace Namespace
			{
				get
				{
					if(base.Namespace!=null)
						return base.Namespace;

					System.Threading.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
					try
					{
						_Namespace=DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(WrMethod.DeclaringType) as MetaDataRepository.Namespace;
						if(_Namespace!=null)
							return _Namespace;
						if(WrMethod.DeclaringType.IsInterface)
						{
							Interface _interface=new Interface(new DotNetMetaDataRepository.Type(WrMethod.DeclaringType));
							_Namespace=_interface;

						}
						if(WrMethod.DeclaringType.IsClass)
						{
							Class _class=new Class(new DotNetMetaDataRepository.Type(WrMethod.DeclaringType));
							_Namespace=_class;
						}
						if(WrMethod.DeclaringType.IsPrimitive)
						{
							Primitive _primitive=new Primitive(new DotNetMetaDataRepository.Type(WrMethod.DeclaringType));
							_Namespace=_primitive;
						}
						return _Namespace;
					}
					finally
					{
						ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
					}

				}
			}


			/// <MetaDataID>{BC9B4215-9157-4AC6-B9E1-45F3A6D50B07}</MetaDataID>
			public override MetaDataRepository.MetaObjectID Identity
			{
				get
				{
					System.Threading.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
					MetaDataRepository.MetaObjectID oldIdentity=_Identity;
					try
					{

					
						if(_Identity!=null)
							return _Identity;
					
						object[] Attributes=WrMethod.GetCustomAttributes(typeof(MetaDataRepository.BackwardCompatibilityID),false);
						if(Attributes.Length>0)
							_Identity=new OOAdvantech.MetaDataRepository.MetaObjectID(Attributes[0].ToString());

						if(_Identity==null)
						{
							string StrIdentity=null;
							foreach(System.Reflection.ParameterInfo Parameter in WrMethod.GetParameters())
							{
								if(StrIdentity==null)
									StrIdentity="[";
								else
									StrIdentity+=",";
								StrIdentity+=Parameter.ParameterType.Name;
							}
							if(StrIdentity==null)
								StrIdentity=WrMethod.Name;
							else
								StrIdentity=WrMethod.Name+StrIdentity+"]";
							_Identity=new MetaDataRepository.MetaObjectID("M:"+Namespace.Identity.ToString()+"."+StrIdentity);
							return _Identity;
						}
						else
							_Identity=new MetaDataRepository.MetaObjectID("M:"+Namespace.Identity+_Identity.ToString());
						return _Identity;
					}
					finally
					{
						ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
						if(oldIdentity==null)
							MetaObjectMapper.AddMetaObject(this,WrMethod.DeclaringType.FullName+"."+WrMethod.Name);
					}
				}


			}
			/// <MetaDataID>{22DECF07-2A5A-4220-A611-332FB8C6C1D6}</MetaDataID>
			public override MetaDataRepository.Operation Specification
			{
				get
				{
					ReaderWriterLock.AcquireReaderLock(10000);
					try
					{
						return _Specification;
					}
					finally
					{
						ReaderWriterLock.ReleaseReaderLock();
					}
				}
			}
				/// <MetaDataID>{4576DD23-3F45-4862-8AB3-09D23E7668C8}</MetaDataID>
			public void SetOperation(Operation SpecificationOperation)
			{
				System.Threading.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
				try
				{
					_Specification=SpecificationOperation;
				}
				finally
				{
					ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
				}
			}

			/// <MetaDataID>{AE37DE86-C887-43DD-93AB-648712CD89C2}</MetaDataID>
			 public Method(System.Reflection.MethodInfo Method)
			{
				WrMethod=Method;
				Name=WrMethod.Name;
				
			}

	
			/// <MetaDataID>{8AC4F8D1-5EB3-41BC-ADDA-3C16173184DD}</MetaDataID>
			public System.Reflection.MethodInfo  WrMethod;
		}
}
