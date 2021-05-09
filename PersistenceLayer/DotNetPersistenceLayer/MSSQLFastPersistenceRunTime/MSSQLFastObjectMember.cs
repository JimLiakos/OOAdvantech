using System;

namespace OOAdvantech.MSSQLFastPersistenceRunTime
{
	/// <summary>
	/// 
	/// </summary>
	public class ObjectMember : OOAdvantech.PersistenceLayer.Member
	{
		public override System.Type Type
		{
			get
			{
				return null;
			}
		}
		private string _Name;
		public override string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				_Name=value;
			}
		}
		private short _ID;
		public override short ID
		{
			get
			{
				return _ID;
			}
			set
			{
				_ID=value;
			}
		}
		private object _Value;
		public override object Value
		{
			get
			{
				return _Value;
			}
			set
			{
				_Value=value;
			}
		}	
	}
}
