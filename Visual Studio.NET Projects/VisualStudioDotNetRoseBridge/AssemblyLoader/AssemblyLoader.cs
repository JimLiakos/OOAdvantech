using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
 

namespace AssemblyLoaderNP
{
	/// <summary>
	/// 
	/// </summary>
	///  
	[GuidAttribute("BAADA6CA-C366-456b-9FBD-F7CCF1CEB542")]
	public interface IAssemblyLoader
	{
		void LoadAssembly(string AssemblyFile);
		long GetNamOfModules();
		object GetModulaAt(long Index);
		
	}
	public interface IType
	{
		long GetFieldsCount();
		object GetFieldAt(long Index );
		long GetPropertiesCount();
		object GetPropertyAt(long Index );
		long GetMethodsCount();
		object GetMethodAt(long Index );
		long GetInterfacesCount();
		object GetInterfaceAt(long Index );
		object WrappedObject
		{
			get;
			set;
		}


	}
	public interface IModule
	{
		long GetTypesCount();
		object GetTypeAt(long Index );
		object WrappedObject
		{
			get;
			set;
		}
		string Name
		{
			get;
		}


	}
	public interface IField
	{
		object WrappedObject
		{
			get;
			set;
		}
		
	}
	public interface IProperty
	{
		object WrappedObject
		{
			get;
			set;
		}
	}
	public interface IMethod
	{
		long GetParametersCount();
		object GetParameterAt(long Index );
		object WrappedObject
		{
			get;
			set;
		}
	}
	public interface IParameter
	{
		object WrappedObject
		{
			get;
			set;
		}
		string GetName();
		object GetType();
	}
	public class  Parameter:IParameter
	{
		protected object mWrappedObject;
		public object WrappedObject
		{
			get
			{
				return mWrappedObject;
			}
			set
			{
				mWrappedObject=value;
			}
		}
		public string GetName()
		{
			return ((System.Reflection.ParameterInfo)WrappedObject).Name;

		}
		public new object GetType()
		{
			IType theType=new Type();
			theType.WrappedObject=((System.Reflection.ParameterInfo)WrappedObject).ParameterType;
			return theType;
		}
	}
		
	public class  Type:IType
	{
		protected object mWrappedObject;
		public object WrappedObject
		{
			get
			{
				return mWrappedObject;
			}
			set
			{
				mWrappedObject=value;
			}
		}
		public long GetFieldsCount()
		{
			return ((System.Type)WrappedObject).GetFields().Length;
		}
		public object GetFieldAt(long Index )
		{
			IField tmpField=new Field();
			tmpField.WrappedObject=((System.Type)WrappedObject).GetFields()[Index];
			return tmpField;
		}
		public long GetPropertiesCount()
		{
			return ((System.Type)WrappedObject).GetProperties().Length;
		}
		public object GetPropertyAt(long Index )
		{
			IProperty tmpProperty=new Property();
			tmpProperty.WrappedObject=((System.Type)WrappedObject).GetProperties()[Index];
			return tmpProperty;
		}
		public long GetMethodsCount()
		{
			System.Reflection.BindingFlags BindingFlag=System.Reflection.BindingFlags.DeclaredOnly|
				System.Reflection.BindingFlags.FlattenHierarchy|
				System.Reflection.BindingFlags.IgnoreCase|
				System.Reflection.BindingFlags.IgnoreReturn|
				System.Reflection.BindingFlags.Instance|
				System.Reflection.BindingFlags.NonPublic|
				System.Reflection.BindingFlags.Public|
				System.Reflection.BindingFlags.Static;
			return ((System.Type)WrappedObject).GetMethods(BindingFlag).Length;
		}
		public object GetMethodAt(long Index )
		{
			System.Reflection.BindingFlags BindingFlag=System.Reflection.BindingFlags.DeclaredOnly|
				System.Reflection.BindingFlags.FlattenHierarchy|
				System.Reflection.BindingFlags.IgnoreCase|
				System.Reflection.BindingFlags.IgnoreReturn|
				System.Reflection.BindingFlags.Instance|
				System.Reflection.BindingFlags.NonPublic|
				System.Reflection.BindingFlags.Public|
				System.Reflection.BindingFlags.Static;
			IMethod tmpMethod=new Method(); 
			tmpMethod.WrappedObject=((System.Type)WrappedObject).GetMethods(BindingFlag)[Index];
			return tmpMethod;
		}
		public long GetInterfacesCount()
		{
			return ((System.Type)WrappedObject).GetInterfaces().Length;
		}
		public object GetInterfaceAt(long Index )
		{

			IType tmpType=new Type(); 
			tmpType.WrappedObject=((System.Type)WrappedObject).GetInterfaces()[Index];
			return tmpType;
		}


		public int ll;
	}
	public class Module : IModule
	{
		protected object mWrappedObject;
		public object WrappedObject
		{
			get
			{
				return mWrappedObject;
			}
			set
			{
	 			mWrappedObject=value;
			}
		}
		public string  Name
		{
			get
			{
				return ((System.Reflection.Module)mWrappedObject).Name;
			}
		}

		public long GetTypesCount()
		{
			System.Reflection.Module CurrModule=(System.Reflection.Module)mWrappedObject;
			return CurrModule.GetTypes().Length;

		}

		public object GetTypeAt(long Index )
		{
			//System.Windows.Forms.MessageBox.Show("iN");
			AssemblyLoaderNP.IType theType=new AssemblyLoaderNP.Type();
			System.Reflection.Module CurrModule=(System.Reflection.Module)mWrappedObject;
			theType.WrappedObject=CurrModule.GetTypes()[Index];
			//CurrModule.GetTypes()[Index]
			

			//System.Windows.Forms.MessageBox.Show("RETURN"+theType.ToString());
					
			return theType;
		}
	}
	public class  Field:IField
	{
		protected object mWrappedObject;
		public object WrappedObject
		{
			get
			{
				return mWrappedObject;
			}
			set
			{
				mWrappedObject=value;
			}
		}
	}
	public class  Method:IMethod
	{
		protected object mWrappedObject;
		public object WrappedObject
		{
			get
			{
				return mWrappedObject;
			}
			set
			{
				mWrappedObject=value;
			}
		}
		public long GetParametersCount()
		{
			return ((System.Reflection.MethodInfo)WrappedObject).GetParameters().Length;
			
		}
		public object GetParameterAt(long Index )
		{
			IParameter tmpParameter =new Parameter();
			tmpParameter.WrappedObject=((System.Reflection.MethodInfo)WrappedObject).GetParameters()[Index];
			return tmpParameter;
		}
	}
	public class  Property:IProperty
	{
		protected object mWrappedObject;
		public object WrappedObject
		{
			get
			{
				return mWrappedObject;
			}
			set
			{
				mWrappedObject=value;
			}
		}
	}
 
	[GuidAttribute("F1C81A78-6046-4cf0-8BB5-C4E2F88BA08B")]
	public class AssemblyLoader : IAssemblyLoader
	{
		protected System.Reflection.Assembly LoadedAssemble;

		public AssemblyLoader()
		{
			
			// 
			// TODO: Add constructor logic here
			//
		}
		public void LoadAssembly(string AssemblyFile)
		{
			try
			{
				LoadedAssemble=System.Reflection.Assembly.LoadFrom(AssemblyFile);
			}
			catch(Exception Error)
			{
				throw Error;
			}
		}
		public long GetNamOfModules()
		{
			Array mArray=LoadedAssemble.GetModules();
			long Length=LoadedAssemble.GetModules().GetLength(0);
			
			System.Reflection.Module mModule=LoadedAssemble.GetModules()[0];
			string tt =mModule.GetTypes()[0].Name;
			return Length;
		}
		public object GetModulaAt(long Index)
		{

			IModule CurrModule=new Module();
			CurrModule.WrappedObject=LoadedAssemble.GetModules()[Index];
			return CurrModule;
		}
	}
}
/*
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace MDIFrameWorkTest
{
	/// <summary>
	/// 
	/// </summary>
	public class StartUp
	{
		public StartUp()
		{
			// 
			// TODO: Add constructor logic here
			//
		}
		[STAThread]
		static void Main() 
		{
			MessageBox.Show("liakos");*/