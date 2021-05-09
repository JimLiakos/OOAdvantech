#region Using directives

using System;
using System.Collections;

#endregion

namespace SimpleInterpreter
{
	/// <summary>
	/// Summary description for VariableList.
	/// </summary>
	public class VariableList
	{
		private ArrayList m_variables = new ArrayList();

		public bool Add(string name, object value)
		{
			if (VariableIndex(name) != -1)
			{
				return false;
			}
			m_variables.Add(new Variable(name, value));
			return true;
		}

		public void ClearValues()
		{
			foreach (Variable variable in m_variables)
			{
				variable.Value = null;
			}
		}

		public int Count
		{
			get { return m_variables.Count; }
		}


		public string GetName(int index)
		{
			if (0 <= index && index < Count)
			{
				return (m_variables[index] as Variable).Name;
			}
			return String.Empty;
		}

		public object this[string name]
		{
			get { return this[VariableIndex(name)]; }
			set { this[VariableIndex(name)] = value; }
		}

		public object this[int index]
		{
			get 
			{
				if (0 <= index && index < Count)
				{
					return (m_variables[index] as Variable).Value;
				}
				return null;
			}
			set 
			{
				if (0 <= index && index < Count)
				{
					(m_variables[index] as Variable).Value = value;
				}
			}
		}
	
		private int VariableIndex(string name) 
		{
			for (int i = 0; i < m_variables.Count; i++)
			{
				if (String.Compare((m_variables[i] as Variable).Name, name, true) == 0)
				{
					return i;
				}
			}
			return -1;
		}

		private class Variable
		{
			public string Name;
			public object Value;

			public Variable(string name, object value)
			{
				Name = name;
				Value = value;
			}
		}
	}
}
