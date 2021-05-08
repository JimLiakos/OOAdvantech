#region Copyright

//----------------------------------------------------------------------
// VBSctipt grammar implementation for Gold Parser engine.
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
using System.Reflection;

using GoldParser;

#endregion

namespace GoldParser
{
    /// <MetaDataID>{652de442-ed24-4226-8f18-e97f0e311f8a}</MetaDataID>
    internal class TreeParser
	{
        public int LinePosition;
        public int LineNumber;
        public string ErrorMessage;

		private Grammar m_grammar;
		event  ParseActionDelegate m_parseAction; 
		
		/// <summary>
		/// Creates a new instance of <see cref="VBScriptParser"/> class.
		/// </summary>
        public TreeParser(Grammar grammar)
		{
            m_grammar = grammar;
            //"string".l
			//BinaryReader reader = GetResourceReader("TreeBuilder.VBScript.cgt");		
            //BinaryReader reader =TreeParser.GetResourceReader("TreeBuilder.Embedded.cgt");
            //BinaryReader reader = GetResourceReader("TreeBuilder.OQL.cgt");		
            //BinaryReader reader = GetResourceReader("TreeBuilder.CompositeOQL.cgt");		
            //m_grammar = new Grammar(reader);
		}

		/// <summary>
		/// Callback function to monitor parsing events.
		/// </summary>
		public event ParseActionDelegate ParseAction 
		{
			add 
            { 
                 m_parseAction+=value; 
            }
			remove 
            { 
                m_parseAction-= value; 
            }
		}
        public SyntaxNode Parse(string text)
        {
#if DeviceDotNet
            return Parse(System.Text.Encoding.UTF8.GetBytes(text));
#else
            return Parse(System.Text.Encoding.Default.GetBytes(text));
#endif
        }

        public SyntaxNode Parse(byte[] text)
        {
            char[] chrs = new char[text.Length];
            int i = 0;
            foreach (byte chr in text)
            {
                chrs[i] = (char)chr;
                i++;
            }
            string stringForParsing = new string(chrs);
            StringReader reader = new StringReader(stringForParsing);
            return Parse(reader) as NonTerminalNode;
        }

		/// <summary>
		/// Parsers the given string.
		/// </summary>
		/// <param name="value">String to parse.</param>
		/// <returns>
		/// Topmost reduction if the parsing was successful.
		/// Retuns null if the value was not parsed.
		/// </returns>
		public SyntaxNode Parse(TextReader reader)
		{
			int reductionNumber = 0;
			GoldParser parser = new GoldParser(reader, m_grammar);
			parser.TrimReductions = false;
			
			while (true)
			{
				ParseMessage response = parser.Parse();
				switch (response)
				{
					case ParseMessage.LexicalError:
						AddParseAction(parser, response, "Cannot Recognize Token", 
							"",	parser.TokenText, "");
						return null;

					case ParseMessage.SyntaxError:
						StringBuilder expectedTokens = new StringBuilder();
						foreach (Symbol token in parser.GetExpectedTokens())
						{
                            string sepString = "";
                            if (expectedTokens.Length > 0)
                                sepString = ",";
                            expectedTokens.Append(sepString);
							expectedTokens.Append("\'"+token.Name+"\'");

                            
						}
						AddParseAction(parser, response, "Expecting the following tokens", 
							"",	expectedTokens.ToString(), "");


                        ErrorMessage = response.ToString() + " on : " + parser.LineText + " [" + parser.TokenText + "]    \"" + "Expecting the following tokens :" + "  " + expectedTokens + "\"";
                        //ErrorMessage = "Expecting the following tokens :" + expectedTokens.ToString();

                        if (parser.TokenSyntaxNode is TerminalNode)
                        {
                            LinePosition=(parser.TokenSyntaxNode as TerminalNode).LinePosition;
                            LineNumber=(parser.TokenSyntaxNode as TerminalNode).LineNumber;
                            
                        }
                        if (parser.TokenSyntaxNode is NonTerminalNode)
                        {
                            LinePosition = (parser.TokenSyntaxNode as NonTerminalNode).LinePosition;
                            LineNumber = (parser.TokenSyntaxNode as NonTerminalNode).Line;
                        }
                        return null;

                        if (parser.TokenSyntaxNode is NonTerminalNode)
                            (parser.TokenSyntaxNode as NonTerminalNode).ExecuteRepeaters();
                        return (SyntaxNode)parser.TokenSyntaxNode;


					case ParseMessage.Reduction:
                        NonTerminalNode nonTerminal = new NonTerminalNode(parser.ReductionRule,
                            parser.TokenLineNumber,
                            parser.TokenLinePosition,  parser.ParsingExpression);
						nonTerminal.ReductionNumber = ++reductionNumber;
						parser.TokenSyntaxNode = nonTerminal;
						StringBuilder childReductionList = new StringBuilder();
						for (int i = 0; i < parser.ReductionCount; i++)
						{
							SyntaxNode node = parser.GetReductionSyntaxNode(i) as SyntaxNode;
							nonTerminal.Add(node);
							NonTerminalNode childNode = node as NonTerminalNode;
							if (childNode != null)
							{
								childReductionList.Append('#');
								childReductionList.Append(childNode.ReductionNumber);
								childReductionList.Append(' ');
							}
						}
                        //AddParseAction(parser, response, nonTerminal.Rule.ToString(),
                        //    reductionNumber.ToString(),
                        //    childReductionList.ToString(),
                        //    nonTerminal.Rule.Index.ToString());
						break;
     
					case ParseMessage.Accept:	//=== Success!
                        //AddParseAction(parser, response, parser.ReductionRule.ToString(),	
                        //    "", "", "");
                        
                        if (parser.TokenSyntaxNode is NonTerminalNode)
                            (parser.TokenSyntaxNode as NonTerminalNode).ExecuteRepeaters();
                        NonTerminalNode root= new NonTerminalNode((parser.TokenSyntaxNode as NonTerminalNode).Rule, 1, 1,parser.ParsingExpression);
                        root.Add((SyntaxNode)parser.TokenSyntaxNode);
                        root.GetErrorStatements();
						return root;
        
					case ParseMessage.TokenRead:
						TerminalNode terminal = new TerminalNode(
							parser.TokenSymbol,
							parser.TokenText,
							parser.TokenString,
							parser.TokenLineNumber,
							parser.TokenLinePosition,parser.CharPosition);
						parser.TokenSyntaxNode = terminal;
                        //AddParseAction(parser, response, 
                        //    terminal.Symbol.Name, "", 
                        //    terminal.ToString(), 
                        //    terminal.Symbol.Index.ToString());
						break;

					case ParseMessage.InternalError:
						AddParseAction(parser, response, "Error in LR state engine", 
							"",	"", "");
						return null;

					case ParseMessage.NotLoadedError:
						//=== Due to the if-statement above, this case statement should never be true
						AddParseAction(parser, response, "Compiled Grammar Table not loaded", 
							"",	"", "");
						return null;
        
					case ParseMessage.CommentError:
						AddParseAction(parser, response, "Unexpected end of file", 
							"",	"", "");
						return null;
				}
			}		
		}

		private void AddParseAction(GoldParser parser, ParseMessage action, 
			string description, string reductionNo, string value, string tableIndex)
		{
			if (m_parseAction != null)
			{
				m_parseAction(parser, action, description, reductionNo, value, tableIndex);
                //ErrorMessage=parser, action, description, reductionNo, value, tableIndex
			}
		}

	}
}
