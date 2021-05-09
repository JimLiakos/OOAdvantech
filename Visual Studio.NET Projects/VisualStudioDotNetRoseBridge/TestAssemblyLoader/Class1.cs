using System;
using System.Reflection;

namespace TestAssemblyLoader
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class Class1
	{
		

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			CrystalDecisions.CrystalReports.Engine.DrawingObject tt;
			Class1 mClass=new Class1();
			Type myType=mClass.GetType();
			myType.GetDefaultMembers();
//			These BindingFlags control enumeration for a great many classes in the System, System.Reflection, and 
	/*		System.Reflection.BindingFlags lop=System.Reflection.BindingFlags.CreateInstance|
						System.Reflection.BindingFlags.DeclaredOnly|
						System.Reflection.BindingFlags.Default;
*/
			BindingFlags lot=BindingFlags.DeclaredOnly|
							BindingFlags.FlattenHierarchy|
							BindingFlags.IgnoreCase|
							BindingFlags.IgnoreReturn|
							BindingFlags.Instance|
							BindingFlags.NonPublic|
							BindingFlags.Public|
							BindingFlags.Static;


			long ttk=myType.GetMethods(lot).Length;
			for(long i=0; i!=ttk;i++)
			{
				string llm=myType.GetMethods(lot)[i].Name;
				int lok1=0;
			}
			
			
			AssemblyLoaderNP.IAssemblyLoader mAssemblyLoader= new AssemblyLoaderNP.AssemblyLoader();
			//mAssemblyLoader.LoadAssembly("X:\\source\\Visual Studio.NET Projects\\VisualStudioDotNetRoseBridge\\AssemblyLoader\\bin\\Debug\\AssemblyLoader.dll");
			
			mAssemblyLoader.LoadAssembly("X:\\source\\Visual Studio.NET Projects\\VisualStudioDotNetRoseBridge\\TestAssemblyLoader\\bin\\Debug\\TestAssemblyLoader.exe"/*"X:\\source\\Visual Studio.NET Projects\\VisualStudioDotNetRoseBridge\\TestAssemblyLoader\\bin\\Release\\TestAssemblyLoader.exe"*/);
			long Count=mAssemblyLoader.GetNamOfModules();
			for(long i=0;i!=Count;i++)
			{
				AssemblyLoaderNP.IModule theModule=(AssemblyLoaderNP.IModule)mAssemblyLoader.GetModulaAt(i);
				System.Reflection.Module CurrModule=(System.Reflection.Module)theModule.WrappedObject;
				AssemblyLoaderNP.IType theType=(AssemblyLoaderNP.IType)theModule.GetTypeAt(0);
				System.Type mType=(System.Type)theType.WrappedObject;
				string Name=mType.Name;
				string Sn="pp";

				//string Sn=CurrModule.ScopeName;
				//theModule

			}
			//
			// TODO: Add code to start application here
			//
		}

		protected void test1()
		{
		
		}

		private void test2()
		{
		
		}

		internal void test3()
		{
		
		}

		protected internal void test4()
		{
		
		}
		public void test5()
		{
		
		}

	}

}
