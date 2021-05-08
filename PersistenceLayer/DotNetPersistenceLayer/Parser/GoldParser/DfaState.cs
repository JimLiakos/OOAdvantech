#region Copyright

//----------------------------------------------------------------------
// Gold Parser engine.
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
using System.Collections;

#endregion

namespace GoldParser
{
    /// <summary>
    /// State in the Deterministic Finite Automata 
    /// which is used by the tokenizer.
    /// </summary>
    /// <MetaDataID>{f12810e1-3621-49b5-8c55-a4f1e73f4726}</MetaDataID>
	internal class DfaState
	{
        /// <MetaDataID>{9960321c-b424-4719-8ed6-7948213d8d1e}</MetaDataID>
		private int m_index;
        /// <MetaDataID>{c3c308d5-37c9-4f65-83e3-fd5f22e1b809}</MetaDataID>
		internal Symbol m_acceptSymbol;
        /// <MetaDataID>{e1055ed3-842b-49c0-a836-53874c339b11}</MetaDataID>
		internal ObjectMap m_transitionVector;

        /// <summary>
        /// Creates a new instance of the <c>DfaState</c> class.
        /// </summary>
        /// <param name="index">Index in the DFA state table.</param>
        /// <param name="acceptSymbol">Symbol to accept.</param>
        /// <param name="transitionVector">Transition vector.</param>
        /// <MetaDataID>{6f2c3293-a642-4c45-8509-3f9e644e5aaa}</MetaDataID>
		public DfaState(int index, Symbol acceptSymbol, ObjectMap transitionVector)
		{
			m_index = index;
			m_acceptSymbol = acceptSymbol;
			m_transitionVector = transitionVector;
		}

        /// <summary>
        /// Gets index of the state in DFA state table.
        /// </summary>
        /// <MetaDataID>{c0aeb110-7d5e-44f8-8c6f-a39bfe302a4a}</MetaDataID>
		public int Index 
		{
			get { return m_index; }
		}

        /// <summary>
        /// Gets the symbol which can be accepted in this DFA state.
        /// </summary>
        /// <MetaDataID>{b657c3cc-8230-4001-856c-33b26850beae}</MetaDataID>
		public Symbol AcceptSymbol 
		{
			get { return m_acceptSymbol; }
		}
	}
}
