namespace OOAdvantech.DotNetMetaDataRepository
{
	/// <MetaDataID>{751FF8D0-262C-4C13-BAE8-DAC4F2EC2DFA}</MetaDataID>
	public class Operation : MetaDataRepository.Operation
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

		/// <MetaDataID>{2B8D4A5C-013E-44ED-AAD5-43A0630A163A}</MetaDataID>
		public override MetaDataRepository.MetaObjectID Identity
		{
			get
			{
//				if(_Namespace==null)
//					throw new System.Exception("There is not namespace");
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

						_Identity=new MetaDataRepository.MetaObjectID("O:"+Namespace.Identity.ToString()+"."+StrIdentity);
						return _Identity;
					}
					else
						_Identity=new MetaDataRepository.MetaObjectID("O:"+Namespace.Identity+_Identity.ToString());
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
	/// <MetaDataID>{FCC5C06C-45B5-4331-964F-6383637678B2}</MetaDataID>
		public Operation (System.Reflection.MethodInfo Method)
		{
			WrMethod=Method;
			if(Method.Name=="CanInsertAfter")
			{
				int ere=0;
			}
			DotNetMetaDataRepository.MetaObjectMapper.AddTypeMap(WrMethod,this);


			Name=WrMethod.Name;
			ReturnType=(MetaDataRepository.Classifier)DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(WrMethod.ReturnType);
			if(ReturnType==null)
			{
				if(WrMethod.ReturnType.IsClass)
				{
					Type mType=new Type(WrMethod.ReturnType);
					ReturnType=new Class(mType);
				}
				if(WrMethod.ReturnType.IsInterface)
				{
					Type mType=new Type(WrMethod.ReturnType);
					ReturnType=new Interface(mType);
				}
				// Error Prone είναι λάθος όταν πρόκεται για int,long,enum κλπ.
			}

			
		}

		/// <MetaDataID>{9C906243-8508-4D3C-B65C-28DA64214AE5}</MetaDataID>
		public System.Reflection.MethodInfo WrMethod;
	}
}
