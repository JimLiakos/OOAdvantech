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
    /// Maps integer values used for transition vectors to objects.
    /// </summary>
    /// <MetaDataID>{8ccf1a2e-c7b0-4fe7-9d1d-5c0d5fa61c16}</MetaDataID>
    internal class ObjectMap
	{
		#region Fields

        /// <MetaDataID>{9ffea90b-ae0b-40dc-bdf7-fc9b24d0c6b4}</MetaDataID>
		private bool m_readonly;
        /// <MetaDataID>{3312ef3a-3859-490f-94e1-689b482615c4}</MetaDataID>
		private MapProvider m_mapProvider;

        /// <MetaDataID>{6f3ad313-dcec-459a-b468-81e1b08251c0}</MetaDataID>
		private const int MAXINDEX = 255;
        /// <MetaDataID>{3406e2de-8d48-4b83-a875-15fd84eee6c5}</MetaDataID>
		private const int GROWTH = 32;
        /// <MetaDataID>{327229b6-0f9c-42fb-be40-4a77cb1c7e9f}</MetaDataID>
		private const int MINSIZE = 32;
        /// <MetaDataID>{2ebf81cf-2183-4c96-b8e9-f845f6cd86c0}</MetaDataID>
		private const int MAXARRAYCOUNT = 12;

        /// <MetaDataID>{85b027ba-7558-4dfe-a85a-9ec3af6add63}</MetaDataID>
		private const int INVALIDKEY = Int32.MaxValue;

		#endregion

		#region Constructors

        /// <summary>
        /// Creates new instance of <see cref="ObjectMap"/> class.
        /// </summary>
        /// <MetaDataID>{1817a467-157a-45be-8373-eb54d20b1328}</MetaDataID>
		public ObjectMap()
		{
			m_mapProvider = new SortedMapProvider(MINSIZE);
		}

		#endregion

		#region Public members.

        /// <summary>
        /// Gets number of entries in the map.
        /// </summary>
        /// <MetaDataID>{1735bd59-0605-4451-acea-5c23ee82f879}</MetaDataID>
		public int Count
		{
			get	{ return m_mapProvider.m_count; }
		}

        /// <summary>
        /// Gets or sets read only flag.
        /// </summary>
        /// <MetaDataID>{df31c39c-4151-42b7-bddd-5f79385bb14c}</MetaDataID>
		public bool ReadOnly 
		{
			get { return m_readonly; }
			set 
			{ 
				if (m_readonly != value)
				{
					SetMapProvider(value);
					m_readonly = value; 
				}
			}
		}

        /// <summary>
        /// Gets or sets value by key.
        /// </summary>
        /// <MetaDataID>{b0052ccb-c2b5-47c8-92a0-f663b118c217}</MetaDataID>
		public object this[int key]
		{
			get { return m_mapProvider[key]; }
			set { m_mapProvider.Add(key, value); }
		}

        /// <summary>
        /// Returns key by index.
        /// </summary>
        /// <param name="index">Zero based index of the requested key.</param>
        /// <returns>Returns key for the given index.</returns>
        /// <MetaDataID>{d0c23668-94c7-4642-8fa4-4c7d65c828d4}</MetaDataID>
		public int GetKey(int index)
		{
			return m_mapProvider.GetEntry(index).Key;
		}

        /// <summary>
        /// Removes entry by its key.
        /// </summary>
        /// <param name="key"></param>
        /// <MetaDataID>{eea4aa5f-f8fa-4b94-b89d-ce8526fc1789}</MetaDataID>
		public void Remove(int key)
		{
			m_mapProvider.Remove(key);
		}

        /// <summary>
        /// Adds a new key and value pair. 
        /// If key exists then value is applied to existing key.
        /// </summary>
        /// <param name="key">New key to add.</param>
        /// <param name="value">Value for the key.</param>
        /// <MetaDataID>{a4e9571e-38d6-4b53-a58f-f2794a41c214}</MetaDataID>
		public void Add(int key, object value)
		{
			m_mapProvider.Add(key, value);
		}

		#endregion

		#region Private members

        /// <MetaDataID>{83fa737b-4237-4230-90fc-93c10e59e340}</MetaDataID>
		private void SetMapProvider(bool readOnly)
		{
			int count = m_mapProvider.m_count;
			MapProvider provider = m_mapProvider;
			if (readOnly)
			{
				SortedMapProvider pr = m_mapProvider as SortedMapProvider;
				if (pr.m_lastKey <= MAXINDEX)
				{
					provider = new IndexMapProvider();
				}
				else if (count <= MAXARRAYCOUNT)
				{
					provider = new ArrayMapProvider(m_mapProvider.m_count);
				}
			}
			else
			{
				if (! (provider is SortedMapProvider))
				{
					provider = new SortedMapProvider(m_mapProvider.m_count);
				}
			}
			if (provider != m_mapProvider)
			{
				for (int i = 0; i < count; i++)
				{
					Entry entry = m_mapProvider.GetEntry(i);
					provider.Add(entry.Key, entry.Value);
				}
				m_mapProvider = provider;
			}
		}

		#endregion

		#region Entry struct definition

		private struct Entry
		{
			internal int Key;
			internal object Value;

			internal Entry(int key, object value)
			{
				Key = key;
				Value = value;
			}
		}

		#endregion

		private abstract class MapProvider 
		{
			internal int m_count;        // Entry count in the collection.

			internal abstract object this[int key]
			{
				get;
			}

			internal abstract Entry GetEntry(int index);

			internal abstract void Add(int key, object value);

			internal virtual void Remove(int key)
			{
				throw new InvalidOperationException();
			}
		}

		private class SortedMapProvider : MapProvider
		{
			internal Entry[] m_entries; // Array of entries.

			internal int m_lastKey;      // Bigest key number.

			internal SortedMapProvider(int capacity)
			{
				m_entries = new Entry[capacity];
			}

			internal override object this[int key]
			{
				get 
				{
					int minIndex = 0;
					int maxIndex = m_count - 1;
					if (maxIndex >= 0 && key <= m_lastKey)
					{
						do
						{
							int midIndex = (maxIndex + minIndex) / 2;
							if (key <= m_entries[midIndex].Key)
							{
								maxIndex = midIndex;
							}
							else
							{
								minIndex = midIndex + 1;
							}
						} while (minIndex < maxIndex);
						if (key == m_entries[minIndex].Key)
						{
							return m_entries[minIndex].Value;
						}
					}
					return null;
				}
			}

			internal override Entry GetEntry(int index)
			{
				return m_entries[index];
			}

			internal override void Add(int key, object value)
			{
				bool found;
				int index = FindInsertIndex(key, out found);
				if (found)
				{
					m_entries[index].Value = value;
					return;
				}
				if (m_count >= m_entries.Length)
				{
					Entry[] entries = new Entry[m_entries.Length + GROWTH];
					Array.Copy(m_entries, 0, entries, 0, m_entries.Length);
					m_entries = entries;
				}
				if (index < m_count)
				{
					Array.Copy(m_entries, index, m_entries, index + 1, m_count - index);
				}
				else
				{
					m_lastKey = key;
				}
				m_entries[index].Key = key;
				m_entries[index].Value = value;
				m_count++;
			}

			internal override void Remove(int key)
			{
				int index = FindIndex(key);
				if (index >= 0)
				{
					int tailSize = (m_count - 1) - index;
					if (tailSize > 0)
					{
						Array.Copy(m_entries, index + 1, m_entries, index, tailSize);
					}
					else if (m_count > 1)
					{
						m_lastKey = m_entries[m_count - 2].Key;
					}
					else
					{
						m_lastKey = INVALIDKEY;
					}
					m_count--;
					m_entries[m_count].Key = INVALIDKEY;
					m_entries[m_count].Value = null;
				}
			}

			private int FindIndex(int key)
			{
				int minIndex = 0;
				if (m_count > 0 && key <= m_lastKey)
				{
					int maxIndex = m_count - 1;
					do
					{
						int midIndex = (maxIndex + minIndex) / 2;
						if (key <= m_entries[midIndex].Key)
						{
							maxIndex = midIndex;
						}
						else
						{
							minIndex = midIndex + 1;
						}
					} while (minIndex < maxIndex);
					if (key == m_entries[minIndex].Key)
					{
						return minIndex;
					}
				}
				return -1;
			}

			private int FindInsertIndex(int key, out bool found)
			{
				int minIndex = 0;
				if (m_count > 0 && key <= m_lastKey)
				{
					int maxIndex = m_count - 1;
					do
					{
						int midIndex = (maxIndex + minIndex) / 2;
						if (key <= m_entries[midIndex].Key)
						{
							maxIndex = midIndex;
						}
						else
						{
							minIndex = midIndex + 1;
						}
					} while (minIndex < maxIndex);
					found = (key == m_entries[minIndex].Key);
					return minIndex;
				}
				found = false;
				return m_count;
			}
		}

		private class IndexMapProvider : MapProvider
		{
			private object[] m_array; // Array of entries.			

			internal IndexMapProvider()
			{
				m_array = new object[MAXINDEX + 1];
				for (int i = m_array.Length; --i >= 0; )
				{
					m_array[i] = Unassigned.Value;
				}
			}

			internal override object this[int key]
			{
				get 
				{ 
					if (key >= m_array.Length || key < 0)
					{
						return null;
					}
					return m_array[key]; 
				}
			}

			internal override Entry GetEntry(int index)
			{
				int idx = -1;
				for (int i = 0; i < m_array.Length; i++)
				{
					object value = m_array[i];
					if (value != Unassigned.Value)
					{
						idx++;
					}
					if (idx == index)
					{
						return new Entry(i, value);
					}
				}
				return new Entry();
			}

			internal override void Add(int key, object value)
			{
				m_array[key] = value;
				m_count++;
			}
		}
		
		private class ArrayMapProvider : MapProvider
		{
			private Entry[] m_entries; // Array of entries.			

			internal ArrayMapProvider(int capacity)
			{
				m_entries = new Entry[capacity];
			}

			internal override object this[int key]
			{
				get 
				{ 
					for (int i = m_count; --i >= 0;)
					{
						Entry entry = m_entries[i];
						int entryKey = entry.Key;
						if (entryKey < key)
						{
							continue;
						}
						else if (entryKey == key)
						{
							return entry.Value;
						}
						else if (entryKey > key)
						{
							return null;
						}
					}
					return null; 
				}
			}

			internal override Entry GetEntry(int index)
			{
				return m_entries[index];
			}

			internal override void Add(int key, object value)
			{
				m_entries[m_count].Key = key;
				m_entries[m_count].Value = value;
				m_count++;
			}
		}

		private class Unassigned
		{
			internal static Unassigned Value = new Unassigned();
		}
	}
}
