#region Copyright

//----------------------------------------------------------------------
// VBSctipt grammar implementation for Gold Parser engine.
// See more details on http://www.devincook.com/goldparser/
// 
// Original code is written in VB by Devin Cook (GOLDParser@DevinCook.com)
//
// This translation is done by Vladimir Morozov (vmoroz@hotmail.com)
// 
// The translation is based on the other engine translations:
// Delphi engine by Alexandre Rai (riccio@gmx.at)
// C# engine by Marcus Klimstra (klimstra@home.nl)
//----------------------------------------------------------------------

#endregion

#region Using directives

using System;
using GoldParser;

#endregion

namespace GoldParser
{
    /// <summary>
    /// Summary description for TerminalNode.
    /// </summary>
    /// <MetaDataID>{2b16476b-8669-4ba8-9bb2-459a5a961ecd}</MetaDataID>
    internal class TerminalNode : SyntaxNode
	{
		private Symbol m_symbol;
		private string m_text;
		private string m_printText;
		private int m_lineNumber;
		private int m_linePosition;
        private int m_StartCharPosition;
        private int m_EndCharPosition;


		public TerminalNode(Symbol symbol, string text, string printText, 
			int lineNumber, int linePosition,int charPosition)
		{
			m_symbol = symbol;
			m_text = text;
			m_printText = printText;
			m_lineNumber = lineNumber;
			m_linePosition = linePosition;
            m_StartCharPosition = charPosition - m_text.Length;
            m_EndCharPosition = charPosition;
		}

		public Symbol Symbol
		{
			get { return m_symbol; }
		}

		public string Text
		{
			get { return m_text; }
		}

		public override string ToString()
		{
			return m_printText;
		}

		public int LineNumber 
		{
			get { return m_lineNumber; }
		}

		public int LinePosition
		{
			get { return m_linePosition; }
		}

        public int StartCharPosition
        {
            get { return m_StartCharPosition; }
        }

        public int EndCharPosition
        {
            get { return m_EndCharPosition; }
        }

	}
}
