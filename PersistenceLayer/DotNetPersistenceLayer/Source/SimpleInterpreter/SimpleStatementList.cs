#region Using directives

using System;

#endregion

namespace SimpleInterpreter
{
	/// <summary>
	/// Summary description for SimpleStatementList.
	/// </summary>
	public class SimpleStatementList : SimpleStatement
	{
		private SimpleStatement m_currentStatement;
		private SimpleStatement m_nextStatement;

		public SimpleStatementList(SimpleContext context, 
			SimpleStatement currentStatement, SimpleStatement nextStatement) : base(context)
		{
			m_currentStatement = currentStatement;
			m_nextStatement = nextStatement;
		}

		public override void Execute()
		{
			m_currentStatement.Execute();
			if (m_nextStatement != null)
			{
				m_nextStatement.Execute();
			}
		}
	}
}
