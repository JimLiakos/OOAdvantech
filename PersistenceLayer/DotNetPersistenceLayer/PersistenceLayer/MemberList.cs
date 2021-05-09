namespace OOAdvantech.Collections
{
	using System.Collections.Specialized;
	/// <MetaDataID>{356AFBDD-4288-42A6-9B9E-15C1F42BAD5A}</MetaDataID>
	/// <summary></summary>
    [System.Serializable]
	public abstract class MemberList
	{
		/// <summary></summary>
		/// <MetaDataID>{3FF6DF9E-2A7A-4ED3-8D5D-D5610E3F924E}</MetaDataID>
		/// <param name="Index"></param>
		protected abstract Member GetMember(string Index);
		/// <MetaDataID>{A8D6BBD9-A524-4DA1-94F8-6CBCDBF05F6A}</MetaDataID>
		/// <summary></summary>
		public long Count
		{
			get
			{ 
				return Members.Count;
			}
		}
		
		/// <MetaDataID>{8576D64D-AD95-4ECF-A5D8-670E21122711}</MetaDataID>
		/// <summary></summary>
		public class MemberEnumerator
		{
			/// <summary></summary>
			/// <MetaDataID>{CCC8714D-9592-42E3-8267-30D88EBA9E64}</MetaDataID>
			private System.Collections.IDictionaryEnumerator RealEnumerator;
			/// <summary></summary>
			/// <MetaDataID>{294C7525-C677-4913-9B4A-9750A056A0DF}</MetaDataID>
			/// <param name="coll"></param>
			public MemberEnumerator(MemberList coll) 
			{
				RealEnumerator=coll.Members.GetEnumerator();
			}
			/// <summary></summary>
			/// <MetaDataID>{0ED08A18-DF41-4D0A-AA2F-D903878590B6}</MetaDataID>
			public bool MoveNext() 
			{
				return RealEnumerator.MoveNext();
			}
			/// <MetaDataID>{71BD3AAF-8AC1-4875-9FA4-032B332B2B47}</MetaDataID>
			/// <summary></summary>
			public Member Current 
			{
				get 
				{
					((( System.Collections.DictionaryEntry)RealEnumerator.Current ).Value as Member).Name=(( System.Collections.DictionaryEntry)RealEnumerator.Current ).Key as string;
					return (Member)(( System.Collections.DictionaryEntry)RealEnumerator.Current ).Value;
				}
			}
		}
		
		/// <MetaDataID>{8D8D7889-5EF1-499A-B1A7-BB10016873C6}</MetaDataID>
		/// <summary></summary>
		public System.Collections.Generic.Dictionary<string,Member> Members;
	
			
		/// <MetaDataID>{3E8D44B2-890E-4FE7-8692-CFAEACAF91E7}</MetaDataID>
		/// <summary></summary>
		public Member this[string Index ]   // long is a 64-bit integer
		{
			get 
			{
				return GetMember(Index);
			}
		}
	
		
		/// <summary></summary>
		/// <MetaDataID>{49E98EFE-B3FB-405F-B56B-62B11C2F0AC7}</MetaDataID>
		public MemberEnumerator GetEnumerator() 
		{
			return new MemberEnumerator(this);
		}

     


	}




}
