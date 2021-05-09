#region Using directives

using System;

#endregion

namespace SimpleInterpreter
{
	/// <summary>
	/// Summary description for SimpleNumber.
	/// </summary>
	public class SimpleNumber : SimpleExpression
	{
		private double m_value;

		public SimpleNumber(SimpleContext context, double value) : base (context)
		{
			m_value = value;
		}

		public override object Value
		{
			get { return m_value; }
		}
	}
}
