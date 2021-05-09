#region Using directives

using System;

#endregion

namespace SimpleInterpreter
{
	/// <summary>
	/// Summary description for SimpleID.
	/// </summary>
	public class SimpleID : SimpleExpression
	{
		private string m_name;

		public SimpleID(SimpleContext context, string name) : base (context)
		{
			m_name = name;
		}

		public override object Value
		{
			get 
			{
				return Context.Variables[m_name];
			}
		}
	}
}
