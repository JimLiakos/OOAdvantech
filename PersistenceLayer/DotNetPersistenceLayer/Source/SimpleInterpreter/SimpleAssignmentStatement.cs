#region Using directives

using System;

#endregion

namespace SimpleInterpreter
{
	/// <summary>
	/// Summary description for SimpleAssignmentStatement.
	/// </summary>
	public class SimpleAssignmentStatement : SimpleStatement
	{
		private string m_name;
		private SimpleExpression m_assignValue;

		public SimpleAssignmentStatement(SimpleContext context, 
			string name, SimpleExpression assignValue) : base (context)
		{
			m_name = name;
			m_assignValue = assignValue;
		}
		
		public override void Execute()
		{
			Context.Variables[m_name] = m_assignValue.Value;
		}
	}
}
