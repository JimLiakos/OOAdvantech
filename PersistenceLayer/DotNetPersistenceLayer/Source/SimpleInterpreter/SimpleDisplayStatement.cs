#region Using directives

using System;
using System.Windows.Forms;
using Microsoft.VisualBasic;

#endregion

namespace SimpleInterpreter
{
	/// <summary>
	/// Summary description for SimpleDisplayStatement.
	/// </summary>
	public class SimpleDisplayStatement : SimpleStatement
	{
		private SimpleExpression m_displayClause;
		private string m_readID;

		public SimpleDisplayStatement(SimpleContext context, 
			SimpleExpression displayClause, string readID) : base (context)
		{
			m_displayClause = displayClause;
			m_readID = readID;
		}

		public override void Execute()
		{
			if (m_readID == null) //No variables accepted
			{
				MessageBox.Show(Convert.ToString(m_displayClause.Value));
			}
			else
			{
				Context.Variables[m_readID] = Interaction.InputBox(Convert.ToString(m_displayClause.Value), "Simple parser", null, 0, 0);
			}
		}
	}
}
