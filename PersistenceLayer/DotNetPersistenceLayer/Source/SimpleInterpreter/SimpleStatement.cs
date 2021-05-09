#region Using directives

using System;

#endregion

namespace SimpleInterpreter
{
	/// <summary>
	/// Summary description for SimpleStatement.
	/// </summary>
	public class SimpleStatement : SimpleSyntaxNode
	{
		public SimpleStatement(SimpleContext context) : base(context)
		{
		}

		public virtual void Execute()
		{
		}
	}
}
