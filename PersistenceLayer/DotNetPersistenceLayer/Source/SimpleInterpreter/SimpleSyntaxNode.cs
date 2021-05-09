#region Using directives

using System;

#endregion

namespace SimpleInterpreter
{
	/// <summary>
	/// Summary description for SimpleSyntaxNode.
	/// </summary>
	public class SimpleSyntaxNode
	{
		private SimpleContext m_context;

		public SimpleSyntaxNode(SimpleContext context)
		{
			m_context = context;
		}

		public SimpleContext Context
		{
			get { return m_context; }
		}
	}
}
