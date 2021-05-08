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
using System.Collections;
using GoldParser;
using ParserNodeField = Parser.ParserNodeField;

#endregion

namespace GoldParser
{
	/// <summary>
	/// Summary description for NonTerminalNode.
	/// </summary>
	public class NonTerminalNode : SyntaxNode,global::Parser.ParserNode
	{
        public string Name
        {
            get
            {
                string ruleName = m_rule.Name.Trim();
                ruleName = ruleName.Substring(1);
                ruleName = ruleName.Substring(0, ruleName.Length - 1);

                if (m_rule.Terminal)
                {
                    ruleName=ruleName.Substring(0, ruleName.Length - "_TERMINAL".Length);  
                }

               // ruleName = m_rule.ToString();//.Name.Trim();
                if (Value != null)
                    return ruleName + " = \"" + Value+"\"";
                else
                    return ruleName;

            }
        }
        public bool ShowTerminals
        {
            get
            {
                string ruleName = m_rule.Name.Trim();
                ruleName = ruleName.Substring(1);
                ruleName = ruleName.Substring(0, ruleName.Length - 1);

                if (ruleName.LastIndexOf("SHOWTERMINALS") != -1 && ruleName.LastIndexOf("SHOWTERMINALS") == ruleName.Length - "SHOWTERMINALS".Length)
                    return true;
                else
                    return false;
            }

        }
        public bool Repeater
        {
            get
            {
                return m_rule.Repeater;
            }

        }
        

        int LineNumber = -1;
        int _LinePosition = -1;


        public void ExecuteRepeaters( )
        {
            
            bool repeater=false;
            ArrayList reapterCollection = new ArrayList();
            foreach (SyntaxNode syntaxNode in m_array)
            {
                if (syntaxNode is NonTerminalNode && (syntaxNode as NonTerminalNode).m_rule.Hide)
                {
                    NonTerminalNode hidenNode = syntaxNode as NonTerminalNode;
                    // hidenNode.GetNonHidenNode(reapterCollection);
                }
                else
                {
                    reapterCollection.Add(syntaxNode);
                }


            }
            m_array = new ArrayList(reapterCollection);
            reapterCollection.Clear();
            if (Repeater && m_array.Count>0)
            {
                repeater = true;

                reapterCollection.Add(m_array[0]);
                if (m_array.Count > 2 && m_array[2] != null && (m_array[2] is NonTerminalNode) && (m_array[2] as NonTerminalNode).m_rule.Name == m_rule.Name)
                {
                    //(m_array[2] as NonTerminalNode).ExecuteRepeaters();
                    (m_array[2] as NonTerminalNode).LoadReapterCollection(ref reapterCollection);
                    m_array = reapterCollection;
                }
                else
                {
                    
                    if (m_array.Count > 1 && m_array[1] != null && (m_array[1] is NonTerminalNode) && (m_array[1] as NonTerminalNode).m_rule.Name == m_rule.Name)
                    {
                        
                        (m_array[1] as NonTerminalNode).LoadReapterCollection(ref reapterCollection);
                        m_array = reapterCollection;
                    }

                }
            }

            



                     
            foreach (SyntaxNode syntaxNode in m_array)
            {
                if (syntaxNode is NonTerminalNode)
                    (syntaxNode as NonTerminalNode).ExecuteRepeaters();
            }
            ArrayList m_array_filtered = new ArrayList();
            foreach (SyntaxNode syntaxNode in m_array)
            {
                if (syntaxNode is NonTerminalNode && (syntaxNode as NonTerminalNode).Repeater)
                {
                    foreach (SyntaxNode repeaterSyntaxNode in (syntaxNode as NonTerminalNode).m_array)
                    {
                        if (repeaterSyntaxNode is NonTerminalNode && !(repeaterSyntaxNode as NonTerminalNode).m_rule.Terminal && !(repeaterSyntaxNode as NonTerminalNode).m_rule.Hide)
                            m_array_filtered.Add(repeaterSyntaxNode);
                        else
                        {
                            if ((repeaterSyntaxNode is NonTerminalNode || Repeater)&&!(repeaterSyntaxNode as NonTerminalNode).m_rule.Hide)
                                m_array_filtered.Add(repeaterSyntaxNode);


                            if (m_rule.Terminal)
                            {
                                if (repeaterSyntaxNode is TerminalNode)
                                    _Value += (repeaterSyntaxNode as TerminalNode).Text;
                                else if (repeaterSyntaxNode is NonTerminalNode)
                                {
                                    _Value += (repeaterSyntaxNode as NonTerminalNode).Value;

                                }
                            }
                        }

                    }
                }
                else
                {
                    //if (syntaxNode is NonTerminalNode && (syntaxNode as NonTerminalNode).ShowTerminals)
                    //{
                    //    foreach (SyntaxNode subSyntaxNode in (syntaxNode as NonTerminalNode).m_array)
                    //        m_array_filtered.Add(subSyntaxNode);
                    //}
                    if (syntaxNode is NonTerminalNode && !(syntaxNode as NonTerminalNode).m_rule.Terminal)
                        m_array_filtered.Add(syntaxNode);
                    else
                    {
                        if (syntaxNode is NonTerminalNode ||Repeater)
                            m_array_filtered.Add(syntaxNode);


                        ////  if (m_array.Count == 1 && m_array[0] is TerminalNode)
                        //if (syntaxNode is NonTerminalNode && (syntaxNode as NonTerminalNode).m_rule.Terminal)
                        //    _Value += (syntaxNode as NonTerminalNode).Value;
                        //else 
                        if(m_rule.Terminal)
                        {
                            if(syntaxNode is TerminalNode)
                                _Value += (syntaxNode as TerminalNode).Text;
                            else if (syntaxNode is NonTerminalNode)
                            {
                                _Value += (syntaxNode as NonTerminalNode).Value;

                            }
                        }
                    }

                }
            }
            if (_Value != null)
            {
                byte[] text = new byte[_Value.Length];
                int i = 0;
                foreach (char ch in _Value)
                {
                    text[i] = (byte)ch;
                    i++;

                }


                _Value = System.Text.Encoding.Default.GetString(text);
            }
            m_array = m_array_filtered;


        }

        private void GetNonHidenNode(ArrayList reapterCollection)
        {
            foreach (SyntaxNode syntaxNode in m_array)
            {
                if (syntaxNode is NonTerminalNode && (syntaxNode as NonTerminalNode).m_rule.Hide)
                {
                    NonTerminalNode hidenNode = syntaxNode as NonTerminalNode;
                    hidenNode.GetNonHidenNode(reapterCollection);
                }
                else
                {
                    reapterCollection.Add(syntaxNode);
                }
            }

            
        }
        
        public void LoadReapterCollection(ref ArrayList reapterCollection)
        {
            if (m_array.Count == 0)
                return;

            reapterCollection.Add(m_array[0]);

            if (m_array.Count > 2 && m_array[2] != null && (m_array[2] is NonTerminalNode) && (m_array[2] as NonTerminalNode).m_rule.Name == m_rule.Name)
                (m_array[2] as NonTerminalNode).LoadReapterCollection(ref reapterCollection);
            else
                if (m_array.Count > 1 && m_array[1] != null && (m_array[1] is NonTerminalNode) && (m_array[1] as NonTerminalNode).m_rule.Name == m_rule.Name)
                    (m_array[1] as NonTerminalNode).LoadReapterCollection(ref reapterCollection);



        }

		private int m_reductionNumber;
		private Rule m_rule;
		private ArrayList m_array = new ArrayList();

		public NonTerminalNode(Rule rule,int lineNumber,int linePosition)
		{
			m_rule = rule;
            //LineNumber = lineNumber;
            //LinePosition = linePosition;
		}

		public int ReductionNumber 
		{
			get { return m_reductionNumber; }
			set { m_reductionNumber = value; }
		}

		public int Count 
		{
			get { return m_array.Count; }
		}

		public SyntaxNode this[int index]
		{
			get { return (SyntaxNode) m_array[index]; }
		}
        
		public void Add(SyntaxNode node)
		{
			if (node == null)
			{
				return ; //throw new ArgumentNullException("node");
			}
            if (LineNumber == -1)
            {
                if (node is NonTerminalNode)
                {

                    LineNumber = (node as NonTerminalNode).LineNumber;
                    _LinePosition = (node as NonTerminalNode).LinePosition;
                    (node as NonTerminalNode)._ParentNode= this;
                }
                else if (node is TerminalNode)
                {
                    LineNumber = (node as TerminalNode).LineNumber;
                    _LinePosition = (node as TerminalNode).LinePosition;
                }
            }
            if (node is NonTerminalNode && (node as NonTerminalNode).m_rule.Hide)
                return;
			m_array.Add(node);
		}

		public Rule Rule
		{
			get { return m_rule; }
		}



        #region ParserNode Members
        
        string _Value = null;
        public string Value
        {
            get
            {
                return _Value;
                string temp = _Value;
                foreach (SyntaxNode syntaxNode in m_array)
                {
                    if (syntaxNode is TerminalNode)
                        temp += (syntaxNode as TerminalNode).Text;
                    else
                        temp += (syntaxNode as NonTerminalNode).Value;
                }
                return temp;
            }
        }

        public int Line
        {
            get { return LineNumber;}
        }

        public int LinePosition
        {
            get { return _LinePosition; }
        }

        global::Parser.ParserNode _ParentNode;
        public global::Parser.ParserNode ParentNode
        {
            get { return _ParentNode; }
        }

        global::Parser.ParserNodeCollection _ChildNodes = null;
        public global::Parser.ParserNodeCollection ChildNodes
        {
            get 
            {
                if (_ChildNodes == null)
                {
                    _ChildNodes = new global::Parser.ParserNodeCollection();
                    foreach (SyntaxNode syntaxNode in m_array)
                    {
                        if (syntaxNode is NonTerminalNode)
                            _ChildNodes.Add(syntaxNode as NonTerminalNode);
                    }
                }
                return _ChildNodes;
            }
        }

        #endregion

        #region ParserNodeIndexer Members

        public Parser.ParserNodeIndexer this[string childNodeName]
        {
            get 
            {
                return ChildNodes[childNodeName] as Parser.ParserNodeIndexer;
            }
        }

        Parser.ParserNodeIndexer Parser.ParserNodeIndexer.this[int index]
        {
            get 
            {
                return ChildNodes[index] as Parser.ParserNodeIndexer;
            }
        }

        [System.CLSCompliant(false)]
        public string this[ParserNodeField field]
        {
            get 
            {
                if (field.FieldName == "Name")
                    return Name;

                if (field.FieldName == "Value")
                    return Value;

                throw new System.NotSupportedException("ParserNode doesn't contains member with name " + field.FieldName + ".");
            }
        }

        #endregion
    }
}
