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

#endregion

namespace GoldParser
{
    /// <summary>
    /// Action in a LR State. 
    /// </summary>
    /// <MetaDataID>{3902e9b3-3a4f-4969-a32e-70d09cf23f5a}</MetaDataID>
    internal class LRStateAction
	{
        /// <MetaDataID>{cbda14c9-9217-43ff-9f65-827d6ef85e8a}</MetaDataID>
		private int m_index;
        /// <MetaDataID>{ef573044-36cb-4f1a-94c9-ff00bdc69102}</MetaDataID>
		private Symbol m_symbol;
        /// <MetaDataID>{1541808b-f979-4381-ac9a-30929c39a9ee}</MetaDataID>
		private LRAction m_action;
        /// <MetaDataID>{27b3c6ae-be6e-4807-ac4c-7f80edbb5ffe}</MetaDataID>
		internal int m_value;

        /// <summary>
        /// Creats a new instance of the <c>LRStateAction</c> class.
        /// </summary>
        /// <param name="index">Index of the LR state action.</param>
        /// <param name="symbol">Symbol associated with the action.</param>
        /// <param name="action">Action type.</param>
        /// <param name="value">Action value.</param>
        /// <MetaDataID>{b3d10ab6-412b-4019-a26e-0d2010b2d0e9}</MetaDataID>
		public LRStateAction(int index, Symbol symbol, LRAction action, int value)
		{
			m_index = index;
			m_symbol = symbol;
			m_action = action;
			m_value = value;
		}

        /// <summary>
        /// Gets index of the LR state action.
        /// </summary>
        /// <MetaDataID>{4af0b550-6cc0-48d9-b338-ac56822470dd}</MetaDataID>
		public int Index 
		{
			get { return m_index; }
		}

        /// <summary>
        /// Gets symbol associated with the LR state action.
        /// </summary>
        /// <MetaDataID>{2540f4c3-8c74-443c-a4d2-b67ee59276b4}</MetaDataID>
		public Symbol Symbol 
		{
			get { return m_symbol; }
		}

        /// <summary>
        /// Gets action type.
        /// </summary>
        /// <MetaDataID>{816c631a-6e96-4601-baf8-4e1a0632c150}</MetaDataID>
		public LRAction Action 
		{
			get { return m_action; }
		}

        /// <summary>
        /// Gets the action value.
        /// </summary>
        /// <MetaDataID>{929f562d-1e09-4966-8556-a0e1e1feef86}</MetaDataID>
		public int Value 
		{
			get { return m_value; }
		}
	}
}
