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
using System.IO;
using System.Text;
using System.Collections;

#endregion

namespace GoldParser
{
    /// <summary>
    /// Contains grammar tables required for parsing.
    /// </summary>
    /// <MetaDataID>{94fb496f-d3d5-4cd6-bc6e-af0e35c80a4a}</MetaDataID>
    internal class Grammar
	{
		#region Fields and constants

        /// <summary>
        /// Identifies Gold parser grammar file.
        /// </summary>
        /// <MetaDataID>{c918c193-a1ef-47a7-8a80-e3b55b8e557b}</MetaDataID>
		public const string FileHeader = "GOLD Parser Tables/v1.0";

		// Grammar header information
        /// <MetaDataID>{256bd34c-9913-483b-afe8-77d0c28158d3}</MetaDataID>
		private string m_name;                // Name of the grammar
        /// <MetaDataID>{b4fbab16-6efd-4455-b635-4db6432435d1}</MetaDataID>
		private string m_version;             // Version of the grammar
        /// <MetaDataID>{929d108e-9703-4009-8318-fa9f2b09f6ab}</MetaDataID>
		private string m_author;              // Author of the grammar
        /// <MetaDataID>{c19fdac9-28fd-4476-9c98-68fdf807fcc0}</MetaDataID>
		private string m_about;               // Grammar description
        /// <MetaDataID>{7d35a750-0661-49ac-844d-100214164c9a}</MetaDataID>
		private int    m_startSymbolIndex;    // Start symbol index
        /// <MetaDataID>{b618a6c9-b802-43c2-ac78-5cca175c26aa}</MetaDataID>
		private bool   m_caseSensitive;       // Grammar is case sensitive or not

		// Tables read from the binary grammar file
        /// <MetaDataID>{21d86b5d-eaa4-4f93-b97d-858ac22d3363}</MetaDataID>
		private  Symbol[]    m_symbolTable;    // Symbol table
        /// <MetaDataID>{07e750da-168b-4c5a-b848-0f0e340b717d}</MetaDataID>
		private  String[]    m_charSetTable;   // Charset table
        /// <MetaDataID>{b0caa26f-0bb3-4ff7-adea-afdb79714cba}</MetaDataID>
		internal Rule[]      m_ruleTable;      // Rule table
        /// <MetaDataID>{83f0ffbd-2b52-4c44-9d4b-97c1d38f01f5}</MetaDataID>
		internal DfaState[]  m_dfaStateTable;  // DFA state table
        /// <MetaDataID>{03125ef9-d509-4ff9-a01f-86e9e57e847b}</MetaDataID>
		internal LRState[]   m_lrStateTable;   // LR state table

        /// <MetaDataID>{46c4f403-9946-4f84-ba5b-99ae58773bdc}</MetaDataID>
        public System.Collections.Generic.Dictionary<string, object> m_repeatersRuleTable;      // Rule table

		// Initial states
        /// <MetaDataID>{7337e8e6-0d9a-43b8-942c-3a30deba148e}</MetaDataID>
		internal int m_dfaInitialStateIndex;   // DFA initial state index
        /// <MetaDataID>{61f16468-873f-4b7b-936d-0885fa6631e2}</MetaDataID>
		internal DfaState m_dfaInitialState;   // DFA initial state 
        /// <MetaDataID>{48a353d5-3b46-465d-9011-07e3edff86c0}</MetaDataID>
		internal int m_lrInitialState;         // LR initial state

		// Internal state of grammar parser
        /// <MetaDataID>{cd66a776-df78-406d-af47-4a4a3d023f01}</MetaDataID>
		private BinaryReader m_reader;         // Source of the grammar    
        /// <MetaDataID>{45378e62-cb3b-40f8-a4a0-dbfe6a41caf2}</MetaDataID>
		private int m_entryCount;              // Number of entries left

        /// <MetaDataID>{609049f2-ab28-406c-9093-5b4e6205933a}</MetaDataID>
		internal Symbol m_errorSymbol;
        /// <MetaDataID>{312db219-a091-46ed-b3ff-8894723f2532}</MetaDataID>
		internal Symbol m_endSymbol;

		#endregion

		#region Constructors

        /// <summary>
        /// Creates a new instance of <c>Grammar</c> class
        /// </summary>
        /// <param name="reader"></param>
        /// <MetaDataID>{c7185fb8-f098-4e4d-a159-eaaf856feeae}</MetaDataID>
		public Grammar(BinaryReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}

			m_reader = reader;
			Load();
		}

		#endregion

		#region Public members

        /// <summary>
        /// Gets grammar name.
        /// </summary>
        /// <MetaDataID>{594df703-cf08-43a2-b4ff-d3d49aa59634}</MetaDataID>
		public string Name
		{
			get { return m_name; }
		}

        /// <summary>
        /// Gets grammar version.
        /// </summary>
        /// <MetaDataID>{6a21d34a-5255-49fb-8587-ab1386a853c4}</MetaDataID>
		public string Version
		{
			get { return m_version; }
		}

        /// <summary>
        /// Gets grammar author.
        /// </summary>
        /// <MetaDataID>{c7daa868-70db-4600-9a78-8cc5a5c1f5e2}</MetaDataID>
		public string Author
		{
			get { return m_author; }
		}

        /// <summary>
        /// Gets grammar description.
        /// </summary>
        /// <MetaDataID>{0b7165b5-c4e1-4eb1-a64d-1fe7fb0d8062}</MetaDataID>
		public string About
		{
			get { return m_about; }
		}

        /// <summary>
        /// Gets the start symbol for the grammar.
        /// </summary>
        /// <MetaDataID>{9b257ec2-fcfd-49c2-98f3-3bf680a48b21}</MetaDataID>
		public Symbol StartSymbol 
		{
			get { return m_symbolTable[m_startSymbolIndex]; }
		}

        /// <summary>
        /// Gets the value indicating if the grammar is case sensitive.
        /// </summary>
        /// <MetaDataID>{e900a26c-467f-4274-a7d2-378ab74337c4}</MetaDataID>
		public bool CaseSensitive
		{
			get { return m_caseSensitive; }
		}

        /// <summary>
        /// Gets initial DFA state.
        /// </summary>
        /// <MetaDataID>{32d35f84-67c1-4a88-90f1-194950b9ed04}</MetaDataID>
		public DfaState DfaInitialState
		{
			get { return m_dfaInitialState; }
		}

        /// <summary>
        /// Gets initial LR state.
        /// </summary>
        /// <MetaDataID>{87714af6-f1d3-482f-ac68-005421f48a45}</MetaDataID>
		public LRState InitialLRState
		{
			get { return m_lrStateTable[m_lrInitialState]; }
		}

        /// <summary>
        /// Gets a special symbol to designate last token in the input stream.
        /// </summary>
        /// <MetaDataID>{8995c1ba-c497-4380-b2aa-b04c5043a01a}</MetaDataID>
		public Symbol EndSymbol 
		{
			get { return m_endSymbol; }
		}

		#endregion

		#region Private members

        /// <summary>
        /// Loads grammar from the binary reader.
        /// </summary>
        /// <MetaDataID>{5ea12d49-5218-4fcf-bf91-2675b89a9822}</MetaDataID>
		private void Load()
		{
			if (FileHeader != ReadString())
			{
				throw new Exception(SR.GetString(SR.Grammar_WrongFileHeader));
			}
			while (m_reader.PeekChar() != -1)
			{
				RecordType recordType = ReadNextRecord();
				switch (recordType)
				{
					case RecordType.Parameters: 
						ReadHeader();
						break;

					case RecordType.TableCounts: 
						ReadTableCounts();
						break;

					case RecordType.Initial: 
						ReadInitialStates();
						break;

					case RecordType.Symbols: 
						ReadSymbols();
						break;

					case RecordType.CharSets: 
						ReadCharSets();
						break;

					case RecordType.Rules: 
						ReadRules();
						break;

					case RecordType.DfaStates: 
						ReadDfaStates();
						break;

					case RecordType.LRStates: 
						ReadLRStates();
						break;
    
					default:
						throw new Exception(SR.GetString(SR.Grammar_InvalidRecordType));
				}
			}
			m_dfaInitialState = m_dfaStateTable[m_dfaInitialStateIndex];
			OptimizeDfaTransitionVectors();
		}

        /// <summary>
        /// Reads the next record in the binary grammar file.
        /// </summary>
        /// <returns>Read record type.</returns>
        /// <MetaDataID>{fb11e3f5-817c-45de-8945-2da5c97f125e}</MetaDataID>
		private RecordType ReadNextRecord()
		{
			char recordType = (char) ReadByte();
			//Structure below is ready for future expansion
			switch (recordType)
			{
				case 'M': 
					//Read the number of entry's
					m_entryCount = ReadInt16();
					return (RecordType) ReadByteEntry();

				default:
					throw new Exception(SR.GetString(SR.Grammar_InvalidRecordHeader));
			}
		}

        /// <summary>
        /// Reads grammar header information.
        /// </summary>
        /// <MetaDataID>{33d92e80-2a4f-41aa-9b6f-28a0408cb5b3}</MetaDataID>
		private void ReadHeader()
		{
			m_name             = ReadStringEntry();
			m_version          = ReadStringEntry();
			m_author           = ReadStringEntry();
			m_about            = ReadStringEntry(); 
			m_caseSensitive    = ReadBoolEntry(); 
			m_startSymbolIndex = ReadInt16Entry(); 
		}

        /// <summary>
        /// Reads table record counts and initializes tables.
        /// </summary>
        /// <MetaDataID>{1d7b0907-ec98-4044-8dc2-b0b7443381ea}</MetaDataID>
		private void ReadTableCounts()
		{
			// Initialize tables
			m_symbolTable    = new Symbol   [ReadInt16Entry()];
			m_charSetTable   = new String   [ReadInt16Entry()];
			m_ruleTable      = new Rule     [ReadInt16Entry()];
			m_dfaStateTable  = new DfaState [ReadInt16Entry()];
			m_lrStateTable   = new LRState  [ReadInt16Entry()];
            m_repeatersRuleTable = new System.Collections.Generic.Dictionary<string, object>();
		}

        /// <summary>
        /// Read initial DFA and LR states.
        /// </summary>
        /// <MetaDataID>{afe7169f-4e08-446f-8d5e-f541778ce4d5}</MetaDataID>
		private void ReadInitialStates()
		{
			m_dfaInitialStateIndex = ReadInt16Entry();
			m_lrInitialState       = ReadInt16Entry();
		}

        /// <summary>
        /// Read symbol information.
        /// </summary>
        /// <MetaDataID>{13f2396f-6d68-4862-aef2-f7136e4926b6}</MetaDataID>
		private void ReadSymbols()
		{
			int index             = ReadInt16Entry();
			string name           = ReadStringEntry();
			SymbolType symbolType = (SymbolType) ReadInt16Entry();
			
			Symbol symbol = new Symbol(index, name, symbolType);
			switch (symbolType)
			{
				case SymbolType.Error:
					m_errorSymbol = symbol;
					break;

				case SymbolType.End:
					m_endSymbol = symbol;
					break;
			}
			m_symbolTable[index] = symbol;
		}

        /// <summary>
        /// Read char set information.
        /// </summary>
        /// <MetaDataID>{64296d5f-837b-4598-bac5-8929e657a5a6}</MetaDataID>
		private void ReadCharSets()
		{
			m_charSetTable[ReadInt16Entry()] = ReadStringEntry();
		}

        /// <summary>
        /// Read rule information.
        /// </summary>
        /// <MetaDataID>{19419ee8-8ab7-4d4d-a04a-a6b112847c34}</MetaDataID>
		private void ReadRules()
		{
			int index = ReadInt16Entry();
			Symbol nonTerminal = m_symbolTable[ReadInt16Entry()];
			ReadEmptyEntry();
			Symbol[] symbols = new Symbol[m_entryCount];
			for (int i = 0 ; i < symbols.Length; i++)
			{
				symbols[i] = m_symbolTable[ReadInt16Entry()];
			}
			Rule rule = new Rule(index, nonTerminal, symbols);
			m_ruleTable[index] = rule;





            
		}

        /// <summary>
        /// Read DFA state information.
        /// </summary>
        /// <MetaDataID>{09389765-2417-45ec-a9b8-c70e74730b09}</MetaDataID>
		private void ReadDfaStates()
		{
			int index = ReadInt16Entry();
			Symbol acceptSymbol = null;
			bool acceptState = ReadBoolEntry();
			if (acceptState)
			{
				acceptSymbol = m_symbolTable[ReadInt16Entry()];
			}
			else
			{
				ReadInt16Entry();  // Skip the entry.
			}
			ReadEmptyEntry();

			// Read DFA edges
			DfaEdge[] edges = new DfaEdge[m_entryCount / 3];
			for (int i = 0; i < edges.Length; i++)
			{
				edges[i].CharSetIndex = ReadInt16Entry();
				edges[i].TargetIndex  = ReadInt16Entry();
				ReadEmptyEntry();
			}
	
			// Create DFA state and store it in DFA state table
			ObjectMap transitionVector = CreateDfaTransitionVector(edges); 
			DfaState dfaState = new DfaState(index, acceptSymbol, transitionVector);
			m_dfaStateTable[index] = dfaState;
		}

        /// <summary>
        /// Read LR state information.
        /// </summary>
        /// <MetaDataID>{6e8206c9-6fdb-43dd-90dc-1821f131851a}</MetaDataID>
		private void ReadLRStates()
		{
			int index = ReadInt16Entry();
			ReadEmptyEntry();
			LRStateAction[] stateTable = new LRStateAction[m_entryCount / 4]; 
			for (int i = 0; i < stateTable.Length; i++)
			{
				Symbol symbol     = m_symbolTable[ReadInt16Entry()];
				LRAction action = (LRAction) ReadInt16Entry();
				int targetIndex   = ReadInt16Entry();
				ReadEmptyEntry();
				stateTable[i] = new LRStateAction(i, symbol, action, targetIndex);
			}

			// Create the transition vector
			LRStateAction[] transitionVector = new LRStateAction[m_symbolTable.Length]; 
			for (int i = 0; i < transitionVector.Length; i++)
			{
				transitionVector[i] = null;
			}
			for (int i = 0; i < stateTable.Length; i++)
			{
				transitionVector[stateTable[i].Symbol.Index] = stateTable[i];
			}

			LRState lrState = new LRState(index, stateTable, transitionVector);
			m_lrStateTable[index] = lrState;
		}

        /// <summary>
        /// Creates the DFA state transition vector.
        /// </summary>
        /// <param name="edges">Array of automata edges.</param>
        /// <returns>Hashtable with the transition information.</returns>
        /// <MetaDataID>{a37df0d4-6d10-40a7-a5e2-4693dff58ea1}</MetaDataID>
		private ObjectMap CreateDfaTransitionVector(DfaEdge[] edges)
		{
			ObjectMap transitionVector = new ObjectMap(); 
			for (int i = edges.Length; --i >= 0;) 
			{
				string charSet = m_charSetTable[edges[i].CharSetIndex];
				for (int j = 0; j < charSet.Length; j++)
				{
					transitionVector[charSet[j]] = edges[i].TargetIndex;
				}
			}
			return transitionVector;
		}

        /// <summary>
        /// Reads empty entry from the grammar file.
        /// </summary>
        /// <MetaDataID>{94ac8a17-fd7e-42b3-a64f-fa429eae9810}</MetaDataID>
		private void ReadEmptyEntry()
		{
			if (ReadEntryType() != EntryType.Empty)
			{
				throw new Exception(SR.GetString(SR.Grammar_EmptyEntryExpected));
			}
			m_entryCount--;
		}

        /// <summary>
        /// Reads string entry from the grammar file.
        /// </summary>
        /// <returns>String entry content.</returns>
        /// <MetaDataID>{0cd9d0a0-4f0c-4fb5-a714-655197f73b38}</MetaDataID>
		private string ReadStringEntry()
		{
			if (ReadEntryType() != EntryType.String)
			{
				throw new Exception(SR.GetString(SR.Grammar_StringEntryExpected));
			}
			m_entryCount--;
			return ReadString();
		}

        /// <summary>
        /// Reads Int16 entry from the grammar file.
        /// </summary>
        /// <returns>Int16 entry content.</returns>
        /// <MetaDataID>{de3af14c-fda5-4ee5-94b4-0c2ed61442d3}</MetaDataID>
		private int ReadInt16Entry()
		{
			if (ReadEntryType() != EntryType.Integer)
			{
				throw new Exception(SR.GetString(SR.Grammar_IntegerEntryExpected));
			}
			m_entryCount--;
			return ReadInt16();
		}

        /// <summary>
        /// Reads byte entry from the grammar file.
        /// </summary>
        /// <returns>Byte entry content.</returns>
        /// <MetaDataID>{f1edd5d6-994f-4eac-a512-25c30e59253b}</MetaDataID>
		private byte ReadByteEntry()
		{
			if (ReadEntryType() != EntryType.Byte)
			{
				throw new Exception(SR.GetString(SR.Grammar_ByteEntryExpected));
			}
			m_entryCount--;
			return ReadByte();
		}

        /// <summary>
        /// Reads boolean entry from the grammar file.
        /// </summary>
        /// <returns>Boolean entry content.</returns>
        /// <MetaDataID>{cc933993-2239-41a4-b3f9-d942d89e753a}</MetaDataID>
		private bool ReadBoolEntry()
		{
			if (ReadEntryType() != EntryType.Boolean)
			{
				throw new Exception(SR.GetString(SR.Grammar_BooleanEntryExpected));
			}
			m_entryCount--;
			return ReadBool();
		}

        /// <summary>
        /// Reads entry type.
        /// </summary>
        /// <returns>Entry type.</returns>
        /// <MetaDataID>{a0e940f8-a13b-4fc2-a352-31459bc04d6a}</MetaDataID>
		private EntryType ReadEntryType()
		{
			if (m_entryCount == 0)
			{
				throw new Exception(SR.GetString(SR.Grammar_NoEntry));
			}  
			return (EntryType) ReadByte();
		}

        /// <summary>
        /// Reads string from the grammar file.
        /// </summary>
        /// <returns>String value.</returns>
        /// <MetaDataID>{68a5a95a-0e0e-4895-b85b-ee22a9245ce3}</MetaDataID>
		private string ReadString()
		{  
			StringBuilder result = new StringBuilder(); 
			char unicodeChar = (char) ReadInt16();
			while (unicodeChar != (char) 0)
			{
				result.Append(unicodeChar);
				unicodeChar = (char) ReadInt16();
			}
			return result.ToString();
		}

        /// <summary>
        /// Reads two byte integer Int16 from the grammar file.
        /// </summary>
        /// <returns>Int16 value.</returns>
        /// <MetaDataID>{e6db042b-a65c-4b58-b844-14d71f4696cc}</MetaDataID>
		private int ReadInt16()
		{
			return m_reader.ReadUInt16();
		}

        /// <summary>
        /// Reads byte from the grammar file.
        /// </summary>
        /// <returns>Byte value.</returns>
        /// <MetaDataID>{325a14c8-0c39-4d7b-8705-7993ccf8493c}</MetaDataID>
		private byte ReadByte()
		{
			return m_reader.ReadByte();
		}

        /// <summary>
        /// Reads boolean from the grammar file.
        /// </summary>
        /// <returns>Boolean value.</returns>
        /// <MetaDataID>{b60b2921-4252-4580-8d93-43816ed11c0a}</MetaDataID>
		private bool ReadBool()
		{
			return (ReadByte() == 1);
		}

        /// <MetaDataID>{42095981-342c-43e3-ac51-8dd7d0bb7f78}</MetaDataID>
		private void OptimizeDfaTransitionVectors()
		{
			DfaState[] dfaStates = m_dfaStateTable;
			foreach (DfaState state in dfaStates)
			{
				ObjectMap transitions = state.m_transitionVector;
				for (int i = transitions.Count; --i >= 0;)
				{
					int key = transitions.GetKey(i);
					object transition = transitions[key];
					if (transition != null)
					{
						int transitionIndex = (int) transition;
						if (transitionIndex >= 0)
						{
							transitions[key] = dfaStates[transitionIndex];
						}
						else
						{
							transitions[key] = null;
						}
					}
				}
				transitions.ReadOnly = true;
			}
		}

		#endregion

		#region Private type definitions

		/// <summary>
		/// Record type byte in the binary grammar file.
		/// </summary>
		private enum RecordType
		{
			Parameters  = (int) 'P', // 80
			TableCounts = (int) 'T', // 84
			Initial     = (int) 'I', // 73
			Symbols     = (int) 'S', // 83
			CharSets    = (int) 'C', // 67
			Rules       = (int) 'R', // 82
			DfaStates   = (int) 'D', // 68
			LRStates    = (int) 'L', // 76
			Comment     = (int) '!'  // 33
		}

		/// <summary>
		/// Entry type byte in the binary grammar file.
		/// </summary>
		private enum EntryType
		{
			Empty		= (int) 'E', // 69
			Integer		= (int) 'I', // 73
			String		= (int) 'S', // 83
			Boolean		= (int) 'B', // 66
			Byte		= (int) 'b'  // 98
		}

		/// <summary>
		/// Edge between DFA states.
		/// </summary>
		private struct DfaEdge 
		{
			public int CharSetIndex;
			public int TargetIndex;
		}

		#endregion
	}
}
