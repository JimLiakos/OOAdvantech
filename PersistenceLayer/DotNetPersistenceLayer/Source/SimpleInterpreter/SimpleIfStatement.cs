#region Using directives

using System;

#endregion

namespace SimpleInterpreter
{
	/// <summary>
	/// Summary description for SimpleIfStatement.
	/// </summary>
	public class SimpleIfStatement : SimpleStatement
	{
		private SimpleExpression m_ifClause;
		private SimpleStatement m_thenClause;
		private SimpleStatement m_elseClause;

		public SimpleIfStatement(SimpleContext context, SimpleExpression ifClause, 
			SimpleStatement thenClause, SimpleStatement elseClause) : base(context)
		{
			m_ifClause = ifClause;
			m_thenClause = thenClause;
			m_elseClause = elseClause;
		}
	
		public override void Execute()
		{
			if (Convert.ToString(m_ifClause.Value) == true.ToString())
			{
				m_thenClause.Execute();
			}
			else
			{
				if (m_elseClause != null)
				{
					m_elseClause.Execute();
				}
			}
		}
	}
}
