#region Using directives

using System;

#endregion

namespace SimpleInterpreter
{
	/// <summary>
	/// Summary description for SimpleExpression.
	/// </summary>
	public class SimpleExpression : SimpleSyntaxNode
	{
		public SimpleExpression(SimpleContext context) : base(context)
		{
		}

		public virtual object Value 
		{
			get { return null; }
		}
	}
}
