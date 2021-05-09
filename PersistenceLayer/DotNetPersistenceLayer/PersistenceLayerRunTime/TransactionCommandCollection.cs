namespace OOAdvantech.PersistenceLayerRunTime.Commands
{
	/// <MetaDataID>{E3E50364-CBFB-468C-B364-A86390EE7DFF}</MetaDataID>
	/// <summary></summary>
	public class CommandCollection
	{
		/// <MetaDataID>{62E8AD0F-65E3-412E-B963-F85AD8329370}</MetaDataID>
		public void AddCollection(CommandCollection collection)
		{
			Commands=new System.Collections.Hashtable(collection.Commands);
		}
		/// <MetaDataID>{0FA00DB8-C3B4-428F-9A7D-F532D0010D53}</MetaDataID>
		public void Clear()
		{
			Commands.Clear();

		}

		/// <MetaDataID>{B3B03DA0-53A8-4E3A-A1C9-805299EEA127}</MetaDataID>
		/// <summary></summary>
		private System.Collections.Hashtable Commands=new System.Collections.Hashtable();

		/// <MetaDataID>{6DE82549-626B-4166-A751-CC789D14EBAF}</MetaDataID>
		public TransactionCommandEnumerator GetEnumerator() 
		{
			return new TransactionCommandEnumerator(Commands.GetEnumerator());
		}
		/// <MetaDataID>{76E5DC4F-A155-4DAE-B175-7E0EEE2EC4D2}</MetaDataID>
		public class TransactionCommandEnumerator 
		{
			/// <MetaDataID>{F89B45E3-7698-4618-A03D-2B318E795A89}</MetaDataID>
			private int nIndex;
			/// <MetaDataID>{BD8020A3-F115-494C-8DE3-D17A70CD57B2}</MetaDataID>
			System.Collections.IEnumerator Enumerator;
			/// <MetaDataID>{6E9DD082-6A50-4735-90D3-F888033E0E9C}</MetaDataID>
			public TransactionCommandEnumerator(System.Collections.IEnumerator  enumerator) 
			{
				Enumerator=enumerator;
				//collection = coll;
			//	nIndex = -1;
			}

			/// <MetaDataID>{39970C2C-BC4E-4978-B1FB-0C1C88E9854E}</MetaDataID>
			public bool MoveNext() 
			{
				return Enumerator.MoveNext();
				//nIndex++;
				//return(nIndex < collection.Count);
			}
			/// <MetaDataID>{476CF7C8-2C9F-4DDD-B979-766C9448576D}</MetaDataID>
			public Command Current 
			{
				get 
				{
					System.Collections.DictionaryEntry entry=(System.Collections.DictionaryEntry)Enumerator.Current;
					return entry.Value as Command;
				}
			}
		}


		/// <summary></summary>
		/// <MetaDataID>{61868BDB-0F73-450A-BC40-1DB6659FFA85}</MetaDataID>
		/// <param name="command"></param>
		public void Add(Command command)
		{
			if(command==null)
				return;
			if(!Commands.Contains(command)) //Performance
				Commands.Add(command,command);
		}
		/// <MetaDataID>{8090FB2B-D07D-456D-8BEE-A9CA53F4CE81}</MetaDataID>
		/// <summary></summary>
		public int Count
		{
			get
			{
				return Commands.Count;
			}
		}
		/// <summary></summary>
		/// <MetaDataID>{80E87F05-80F3-4820-ABC2-C797107DA762}</MetaDataID>
		/// <param name="command"></param>
		public void Delete(Command command)
		{
			Commands.Remove(command);
			
		}
	
	}
}
