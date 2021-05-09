#region Using directives

using System;

#endregion

namespace SimpleInterpreter
{
	/// <summary>
	/// Summary description for SimpleWhileStatement.
	/// </summary>
	public class SimpleWhileStatement : SimpleStatement
	{
		private SimpleExpression m_whileClause;
		private SimpleStatement m_doClause;
		
		public SimpleWhileStatement(SimpleContext context, SimpleExpression whileClause, 
			SimpleStatement doClause) : base(context)
		{
			m_whileClause = whileClause;
			m_doClause = doClause;
		}

		public override void Execute()
		{
			while ((string) m_whileClause.Value == true.ToString())
			{
				m_doClause.Execute();
			}
		}
	}
}
