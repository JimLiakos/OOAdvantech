#region Using directives

using System;

#endregion

namespace SimpleInterpreter
{
	/// <summary>
	/// Summary description for SimpleExpression.
	/// </summary>
	public class SimpleBinaryExpression : SimpleExpression
	{
		private SimpleExpression m_leftOperand;
		private string m_operator;
		private SimpleExpression m_rightOperand;

		public SimpleBinaryExpression(SimpleContext context,
			SimpleExpression leftOperand, string op, SimpleExpression rightOperand)
			: base (context)
		{
			m_leftOperand = leftOperand;
			m_operator = op;
			m_rightOperand = rightOperand;
		}

		public override object Value
		{
			get 
			{
				object lValue = m_leftOperand.Value;
				object rValue = m_rightOperand.Value;

				switch (m_operator)
				{
					case "+":
						return Convert.ToDouble(lValue) + Convert.ToDouble(rValue);
						
					case "-":
						return Convert.ToDouble(lValue) - Convert.ToDouble(rValue);
						
					case "&":
						return Convert.ToString(lValue) + Convert.ToString(rValue);
						
					case "*":
						return Convert.ToDouble(lValue) * Convert.ToDouble(rValue);
						
					case "/":
						return Convert.ToDouble(lValue) / Convert.ToDouble(rValue);
						
					case ">":
						return (Convert.ToDouble(lValue) > Convert.ToDouble(rValue)).ToString();
						
					case "<":
						return (Convert.ToDouble(lValue) < Convert.ToDouble(rValue)).ToString();
						
					case "<=":
						return (Convert.ToDouble(lValue) <= Convert.ToDouble(rValue)).ToString();
						
					case ">=":
						return (Convert.ToDouble(lValue) >= Convert.ToDouble(rValue)).ToString();
						
					case "==":
						if (lValue.GetType() == typeof(string) && rValue.GetType() == typeof(string))
						{
							return (String.Compare(lValue as string, rValue as string, true) == 0).ToString(); //== Compare strings
						}
						else
						{
							return (Convert.ToDouble(lValue) == Convert.ToDouble(rValue)).ToString(); //== Compare values
						}
        
					case "<>":
						if (lValue.GetType() == typeof(string) && rValue.GetType() == typeof(string))
						{
							return (String.Compare(lValue as string, rValue as string, true) != 0).ToString(); //== Compare strings
						}
						else
						{
							return (Convert.ToDouble(lValue) != Convert.ToDouble(rValue)).ToString(); //== Compare values
						}
				}
				return null;
			}
		}
	}
}