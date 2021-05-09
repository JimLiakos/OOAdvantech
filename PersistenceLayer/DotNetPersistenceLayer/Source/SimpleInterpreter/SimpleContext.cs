#region Using directives

using System;
using GoldParser;

#endregion

namespace SimpleInterpreter
{
	/// <summary>
	/// Summary description for SimpleModule.
	/// </summary>
	public class SimpleContext
	{
		private VariableList m_variables = new VariableList();
		private Parser m_parser;

		public SimpleContext(Parser parser)
		{
			m_parser = parser;
		}

		public VariableList Variables 
		{
			get { return m_variables; }
		}
		
		public SimpleSyntaxNode GetSimpleObject()
		{
			switch ((RuleConstants) m_parser.ReductionRule.Index)
			{
				case RuleConstants.Rule_Statements:               // <Statements> ::= <Statement> <Statements>
					return new SimpleStatementList(this, Statement(0), Statement(1));
        
					//=== Since TrimReductions is set to true, none of these reductions will be
					//=== returned. They are performed 'behind the scenes'
					//case RuleStatementsEnd:            // <Statements> ::= <Statement>
					//    return new SimpleStatementList(parser.GetReductionSyntaxNode(0), null);
        
				case RuleConstants.Rule_Statement_Display:         // <Statement> ::= display <Expression>
					return new SimpleDisplayStatement(this, Expression(1), null);
        
				case RuleConstants.Rule_Statement_Display_Read_Id:   // <Statement> ::= display <Expression> read Id
					return new SimpleDisplayStatement(this, Expression(1), Token(3));
        
				case RuleConstants.Rule_Statement_Assign_Id_Eq:          // <Statement> ::= assign Id = <Expression>
					return new SimpleAssignmentStatement(this, Token(1), Expression(3));
                
				case RuleConstants.Rule_Statement_While_Do_End:           // <Statement> ::= while <Expression> do <Statements> end
					return new SimpleWhileStatement(this, Expression(1), Statement(3));
            
				case RuleConstants.Rule_Statement_If_Then_End:          // <Statement> ::= if <Expression> then <Statements> end
					return new SimpleIfStatement(this, Expression(1), Statement(3), null);
        
				case RuleConstants.Rule_Statement_If_Then_Else_End:      // <Statement> ::= if <Expression> then <Statements> else <Statements> end
					return new SimpleIfStatement(this, Expression(1), Statement(3), Statement(5));
                            
				case RuleConstants.Rule_Expression_Gt:
				case RuleConstants.Rule_Expression_Lt:
				case RuleConstants.Rule_Expression_Lteq:
				case RuleConstants.Rule_Expression_Gteq:
				case RuleConstants.Rule_Expression_Eqeq:
				case RuleConstants.Rule_Expression_Ltgt:
				case RuleConstants.Rule_Addexp_Plus:
				case RuleConstants.Rule_Addexp_Minus:
				case RuleConstants.Rule_Addexp_Amp:
				case RuleConstants.Rule_Multexp_Times:
				case RuleConstants.Rule_Multexp_Div:
					return new SimpleBinaryExpression(this, Expression(0), Token(1), Expression(2));
             
				case RuleConstants.Rule_Negateexp_Minus:              // <Negate Exp> ::= '-' <Value>
					return new SimpleNegate(this, Expression(1));

					//=== Since TrimReductions is set to true, none of these reductions will be
					//=== returned. They are performed 'behind the scenes'
					//case RuleConstants.RuleExpEmpty:                 // <Expression> ::= <Add Exp>
					//case RuleConstants.RuleAddExpEmpty:              // <Add Exp> ::= <Mult Exp>
					//case RuleConstants.RuleMultExpEmpty:             // <Mult Exp> ::= <Negate Exp>
					//case RuleConstants.RuleNegateEmpty:              // <Negate Exp> ::= <Value>
        
				case RuleConstants.Rule_Value_Id:                  // <Value> ::= Id
					Variables.Add(Token(0), null);
					return new SimpleID(this, Token(0));
            
				case RuleConstants.Rule_Value_Stringliteral:              // <Value> ::= StringLiteral
					string text = Token(0);
					if (text.Length >= 2)    //Remove single quotes
					{
						text = text.Substring(1, text.Length - 2);
					}
					return new SimpleString(this, text);

				case RuleConstants.Rule_Value_Numberliteral:              // <Value> ::= NumberLiteral
					return new SimpleNumber(this, Convert.ToDouble(Token(0)));
            
				case RuleConstants.Rule_Value_Lparan_Rparan:          // <Value> ::= '(' <Expression> ')'
					return Expression(1);
			}
			return null;
		}

		private SimpleExpression Expression(int index)
		{
			return (SimpleExpression) m_parser.GetReductionSyntaxNode(index);
		}

		private SimpleStatement Statement(int index)
		{
			return (SimpleStatement) m_parser.GetReductionSyntaxNode(index);
		}

		private string Token(int index)
		{
			return (string) m_parser.GetReductionSyntaxNode(index);
		}

		public string GetTokenText()
		{
			// We need text of the following tokens for synatx tree creation.
			switch ((SymbolConstants) m_parser.TokenSymbol.Index)
			{
				case SymbolConstants.Symbol_Minus:          // '-'
				case SymbolConstants.Symbol_Amp:            // &
				case SymbolConstants.Symbol_Times:          // '*'
				case SymbolConstants.Symbol_Div:            // /
				case SymbolConstants.Symbol_Plus:           // '+'
				case SymbolConstants.Symbol_Lt:             // '<'
				case SymbolConstants.Symbol_Lteq:           // '<'=
				case SymbolConstants.Symbol_Ltgt:           // '<''>'
				case SymbolConstants.Symbol_Eqeq:           // ==
				case SymbolConstants.Symbol_Gt:             // '>'
				case SymbolConstants.Symbol_Gteq:           // '>'=
				case SymbolConstants.Symbol_Id:             // Id
				case SymbolConstants.Symbol_Numberliteral:  // NumberLiteral
				case SymbolConstants.Symbol_Stringliteral:  // StringLiteral
					return m_parser.TokenText;
			}
			return null;
		}

		private enum SymbolConstants
		{
			Symbol_Eof           = 0 , // (EOF)
			Symbol_Error         = 1 , // (Error)
			Symbol_Whitespace    = 2 , // (Whitespace)
			Symbol_Commentend    = 3 , // (Comment End)
			Symbol_Commentline   = 4 , // (Comment Line)
			Symbol_Commentstart  = 5 , // (Comment Start)
			Symbol_Minus         = 6 , // '-'
			Symbol_Amp           = 7 , // '&'
			Symbol_Lparan        = 8 , // '('
			Symbol_Rparan        = 9 , // ')'
			Symbol_Times         = 10, // '*'
			Symbol_Div           = 11, // '/'
			Symbol_Plus          = 12, // '+'
			Symbol_Lt            = 13, // '<'
			Symbol_Lteq          = 14, // '<='
			Symbol_Ltgt          = 15, // '<>'
			Symbol_Eq            = 16, // '='
			Symbol_Eqeq          = 17, // '=='
			Symbol_Gt            = 18, // '>'
			Symbol_Gteq          = 19, // '>='
			Symbol_Assign        = 20, // assign
			Symbol_Display       = 21, // display
			Symbol_Do            = 22, // do
			Symbol_Else          = 23, // else
			Symbol_End           = 24, // end
			Symbol_Id            = 25, // Id
			Symbol_If            = 26, // if
			Symbol_Numberliteral = 27, // NumberLiteral
			Symbol_Read          = 28, // read
			Symbol_Stringliteral = 29, // StringLiteral
			Symbol_Then          = 30, // then
			Symbol_While         = 31, // while
			Symbol_Addexp        = 32, // <Add Exp>
			Symbol_Expression    = 33, // <Expression>
			Symbol_Multexp       = 34, // <Mult Exp>
			Symbol_Negateexp     = 35, // <Negate Exp>
			Symbol_Statement     = 36, // <Statement>
			Symbol_Statements    = 37, // <Statements>
			SymbolValue         = 38, // <Value>
		}

		private enum RuleConstants
		{
			Rule_Statements                 = 0 , // <Statements> ::= <Statement> <Statements>
			Rule_Statements2                = 1 , // <Statements> ::= <Statement>
			Rule_Statement_Display          = 2 , // <Statement> ::= display <Expression>
			Rule_Statement_Display_Read_Id  = 3 , // <Statement> ::= display <Expression> read Id
			Rule_Statement_Assign_Id_Eq     = 4 , // <Statement> ::= assign Id '=' <Expression>
			Rule_Statement_While_Do_End     = 5 , // <Statement> ::= while <Expression> do <Statements> end
			Rule_Statement_If_Then_End      = 6 , // <Statement> ::= if <Expression> then <Statements> end
			Rule_Statement_If_Then_Else_End = 7 , // <Statement> ::= if <Expression> then <Statements> else <Statements> end
			Rule_Expression_Gt              = 8 , // <Expression> ::= <Expression> '>' <Add Exp>
			Rule_Expression_Lt              = 9 , // <Expression> ::= <Expression> '<' <Add Exp>
			Rule_Expression_Lteq            = 10, // <Expression> ::= <Expression> '<=' <Add Exp>
			Rule_Expression_Gteq            = 11, // <Expression> ::= <Expression> '>=' <Add Exp>
			Rule_Expression_Eqeq            = 12, // <Expression> ::= <Expression> '==' <Add Exp>
			Rule_Expression_Ltgt            = 13, // <Expression> ::= <Expression> '<>' <Add Exp>
			Rule_Expression                 = 14, // <Expression> ::= <Add Exp>
			Rule_Addexp_Plus                = 15, // <Add Exp> ::= <Add Exp> '+' <Mult Exp>
			Rule_Addexp_Minus               = 16, // <Add Exp> ::= <Add Exp> '-' <Mult Exp>
			Rule_Addexp_Amp                 = 17, // <Add Exp> ::= <Add Exp> '&' <Mult Exp>
			Rule_Addexp                     = 18, // <Add Exp> ::= <Mult Exp>
			Rule_Multexp_Times              = 19, // <Mult Exp> ::= <Mult Exp> '*' <Negate Exp>
			Rule_Multexp_Div                = 20, // <Mult Exp> ::= <Mult Exp> '/' <Negate Exp>
			Rule_Multexp                    = 21, // <Mult Exp> ::= <Negate Exp>
			Rule_Negateexp_Minus            = 22, // <Negate Exp> ::= '-' <Value>
			Rule_Negateexp                  = 23, // <Negate Exp> ::= <Value>
			Rule_Value_Id                   = 24, // <Value> ::= Id
			Rule_Value_Stringliteral        = 25, // <Value> ::= StringLiteral
			Rule_Value_Numberliteral        = 26, // <Value> ::= NumberLiteral
			Rule_Value_Lparan_Rparan        = 27, // <Value> ::= '(' <Expression> ')' 
		}
	}
}