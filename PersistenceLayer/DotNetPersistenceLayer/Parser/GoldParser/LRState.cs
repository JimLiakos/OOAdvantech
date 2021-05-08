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
    /// State of LR parser.
    /// </summary>
    /// <MetaDataID>{270ce5fe-f71b-4c8d-a45e-4daedef11664}</MetaDataID>
    internal class LRState
	{
        /// <MetaDataID>{064a54f7-997b-499f-95f5-355a1534c183}</MetaDataID>
		private int m_index;
        /// <MetaDataID>{717269b2-4725-4369-9cc6-a0eda6880d91}</MetaDataID>
		private LRStateAction[] m_actions;
        /// <MetaDataID>{176046f7-5d10-491c-bb5a-cb1accbedfbf}</MetaDataID>
		internal LRStateAction[] m_transitionVector;

        /// <summary>
        /// Creates a new instance of the <c>LRState</c> class
        /// </summary>
        /// <param name="index">Index of the LR state in the LR state table.</param>
        /// <param name="actions">List of all available LR actions in this state.</param>
        /// <param name="transitionVector">Transition vector which has symbol index as an index.</param>
        /// <MetaDataID>{83e323f8-d960-4be0-b8d4-d8970b086495}</MetaDataID>
		public LRState(int index, LRStateAction[] actions, LRStateAction[] transitionVector)
		{
			m_index = index;
			m_actions = actions;
			m_transitionVector = transitionVector;
		}

        /// <summary>
        /// Gets index of the LR state in LR state table.
        /// </summary>
        /// <MetaDataID>{dd291304-2004-4b0c-9fd3-9aba5a941e42}</MetaDataID>
		public int Index 
		{
			get { return m_index; }
		}

        /// <summary>
        /// Gets LR state action count.
        /// </summary>
        /// <MetaDataID>{9bb0f817-33e3-48d3-b805-1046c357b3c7}</MetaDataID>
		public int ActionCount 
		{
			get { return m_actions.Length; }
		}

        /// <summary>
        /// Returns state action by its index.
        /// </summary>
        /// <param name="index">State action index.</param>
        /// <returns>LR state action for the given index.</returns>
        /// <MetaDataID>{87d1db73-b0fb-49e5-80f7-0918a523dd60}</MetaDataID>
		public LRStateAction GetAction(int index)
		{
			return m_actions[index];
		}

        /// <summary>
        /// Returns LR state action by symbol index.
        /// </summary>
        /// <param name="symbolIndex">Symbol Index to search for.</param>
        /// <returns>LR state action object.</returns>
        /// <MetaDataID>{5beee273-f8d1-4727-b26e-ab13de01abdb}</MetaDataID>
		public LRStateAction GetActionBySymbolIndex(int symbolIndex)
		{
			return m_transitionVector[symbolIndex];
		}
	}
}
