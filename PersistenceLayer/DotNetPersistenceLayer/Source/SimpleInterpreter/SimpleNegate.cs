#region Using directives

using System;

#endregion

namespace SimpleInterpreter
{
	/// <summary>
	/// Summary description for SimpleNegate.
	/// </summary>
	public class SimpleNegate : SimpleExpression
	{
		private SimpleExpression m_expr;

		public SimpleNegate(SimpleContext context, SimpleExpression expression) 
			: base(context)
		{
			m_expr = expression;
		}

		public override object Value
		{
			get	{return -Convert.ToDouble(m_expr.Value); }
		}

	}
}
