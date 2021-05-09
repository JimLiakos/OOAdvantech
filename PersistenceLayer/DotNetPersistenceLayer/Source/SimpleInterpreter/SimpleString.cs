#region Using directives

using System;

#endregion

namespace SimpleInterpreter
{
	/// <summary>
	/// Summary description for SimpleString.
	/// </summary>
	public class SimpleString : SimpleExpression
	{
		private string m_value;

		public SimpleString(SimpleContext context, string text) : base(context)
		{	
			m_value = text;
		}

		public override object Value
		{
			get { return m_value; }
		}
	}
}
