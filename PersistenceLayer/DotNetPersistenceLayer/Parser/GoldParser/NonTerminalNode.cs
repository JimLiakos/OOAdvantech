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
    /// <MetaDataID>{18b25244-d1a2-464f-8935-830b7b6acf87}</MetaDataID>
    internal class NonTerminalNode : SyntaxNode, global::Parser.ParserNode
	{
        public override string ToString()
        {
            string returnStr = Name;
            if (m_rule.Terminal && Value != null)
                returnStr += " = \"" + Value+"\"";
            return returnStr;

        }
        public string Name
        {
            get  
            {
                string ruleName = m_rule.Name.Trim();
                ruleName = ruleName.Substring(1);
                ruleName = ruleName.Substring(0, ruleName.Length - 1);

                if (m_rule.Terminal)
                    ruleName=ruleName.Substring(0, ruleName.Length - "_TERMINAL".Length);

                if (m_rule.Error)
                    ruleName = ruleName.Substring(0, ruleName.Length - "_ERROR".Length);  

                return ruleName;

            }
        }

        /// <MetaDataID>{e4d5028c-1db0-4b6e-af94-ea03831f87f0}</MetaDataID>
        public System.Collections.Generic.List<NonTerminalNode> GetErrorStatements()
        {
            System.Collections.Generic.List<NonTerminalNode> errorStatements = new System.Collections.Generic.List<NonTerminalNode>();
            GetErrorStatements(errorStatements);
            if(errorStatements.Count>0)
            {
                m_array.Clear();
                foreach(var @object in errorStatements)
                    m_array.Add(@object);
            }
            return errorStatements;
        }
        /// <MetaDataID>{bf4175d0-6ed7-4347-9e10-9b884f487e75}</MetaDataID>
        public void GetErrorStatements(System.Collections.Generic.List<NonTerminalNode> errorStatements)
        {
            foreach (SyntaxNode syntaxNode in m_array)
            {
                if (syntaxNode is NonTerminalNode)
                    if ((syntaxNode as NonTerminalNode).Rule.Error)
                        errorStatements.Add(syntaxNode as NonTerminalNode);
                    else
                        (syntaxNode as NonTerminalNode).GetErrorStatements(errorStatements);

            }
        }



        /// <MetaDataID>{54e53936-b44e-42a9-855e-8849d8d7b0de}</MetaDataID>
        int LineNumber = -1;
        /// <MetaDataID>{501a2dac-c638-4530-84d7-135b250da738}</MetaDataID>
        int _LinePosition = -1;


        /// <MetaDataID>{0e2725b1-8907-4829-98e8-2f06233da279}</MetaDataID>
        public void ExecuteRepeaters( )
        {
            
            bool repeater=false;
            string tt = Value;
            System.Collections.Generic.List<object> reapterCollection = new System.Collections.Generic.List<object>();
            foreach (SyntaxNode syntaxNode in m_array)
            {
                if (syntaxNode is NonTerminalNode && (syntaxNode as NonTerminalNode).m_rule.Hidden)
                {
                    NonTerminalNode hidenNode = syntaxNode as NonTerminalNode;
                    // hidenNode.GetNonHidenNode(reapterCollection);
                }
                else
                {
                    reapterCollection.Add(syntaxNode);
                }


            }
            m_array = new System.Collections.Generic.List<object>(reapterCollection);
            reapterCollection.Clear();
            if (m_rule.Repeater && m_array.Count>0)
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
            System.Collections.Generic.List<object> m_array_filtered = new System.Collections.Generic.List<object>();
            foreach (SyntaxNode syntaxNode in m_array)
            {
                if (syntaxNode is NonTerminalNode && (syntaxNode as NonTerminalNode).m_rule.Repeater)
                {
                    foreach (SyntaxNode repeaterSyntaxNode in (syntaxNode as NonTerminalNode).m_array)
                    {
                        if (repeaterSyntaxNode is NonTerminalNode && !(repeaterSyntaxNode as NonTerminalNode).m_rule.Terminal && !(repeaterSyntaxNode as NonTerminalNode).m_rule.Hidden)
                            m_array_filtered.Add(repeaterSyntaxNode);
                        else
                        {
                            if ((repeaterSyntaxNode is NonTerminalNode || m_rule.Repeater) && !(repeaterSyntaxNode as NonTerminalNode).m_rule.Hidden)
                                m_array_filtered.Add(repeaterSyntaxNode);


                           // if (m_rule.Terminal || m_rule.Error)
                            //{
                            //    if (repeaterSyntaxNode is TerminalNode)
                            //    {
                            //        if (_Value == null)
                            //            _LinePosition = (repeaterSyntaxNode as TerminalNode).LinePosition;
                            //        _Value += (repeaterSyntaxNode as TerminalNode).Text;
                            //    }
                            //    else if (repeaterSyntaxNode is NonTerminalNode)
                            //    {
                            //        _Value += (repeaterSyntaxNode as NonTerminalNode).Value;

                            //    }
                            //}
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
                        if (syntaxNode is NonTerminalNode || m_rule.Repeater)
                            m_array_filtered.Add(syntaxNode);


                        ////  if (m_array.Count == 1 && m_array[0] is TerminalNode)
                        //if (syntaxNode is NonTerminalNode && (syntaxNode as NonTerminalNode).m_rule.Terminal)
                        //    _Value += (syntaxNode as NonTerminalNode).Value;
                        //else 
                        //if (m_rule.Terminal || m_rule.Error)
                        //{
                        //    if (syntaxNode is TerminalNode)
                        //    {
                        //        if (_Value == null)
                        //            _LinePosition = (syntaxNode as TerminalNode).LinePosition;

                        //        _Value += (syntaxNode as TerminalNode).Text;
                        //    }
                        //    else if (syntaxNode is NonTerminalNode)
                        //    {
                        //        _Value += (syntaxNode as NonTerminalNode).Value;

                        //    }
                        //}
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

#if DeviceDotNet
                _Value = System.Text.Encoding.UTF8.GetString(text,0,text.Length);
#else
                _Value = System.Text.Encoding.Default.GetString(text,0,text.Length);
#endif
            }
            m_array = m_array_filtered;


        }

        /// <MetaDataID>{6b35b4d3-c1c0-4771-be2a-fc22970b6d67}</MetaDataID>
        private void GetNonHidenNode(System.Collections.Generic.List<object> reapterCollection)
        {
            foreach (SyntaxNode syntaxNode in m_array)
            {
                if (syntaxNode is NonTerminalNode && (syntaxNode as NonTerminalNode).m_rule.Hidden)
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

        /// <MetaDataID>{ab47262c-f548-4620-971d-180028936bbd}</MetaDataID>
        public void LoadReapterCollection(ref System.Collections.Generic.List<object> reapterCollection)
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

        /// <MetaDataID>{ee21b70e-90b5-4e87-a0bb-31e5d45f0932}</MetaDataID>
		private int m_reductionNumber;
        /// <MetaDataID>{610a752c-0ac7-4edb-a596-66ab7e4477b6}</MetaDataID>
		private Rule m_rule;
        /// <MetaDataID>{3b82af0b-8999-4552-9829-f229275fedd2}</MetaDataID>
		private System.Collections.Generic.List<object> m_array = new System.Collections.Generic.List<object>();

        /// <MetaDataID>{adfb2342-52d8-4a16-b09c-dc0af3cd656d}</MetaDataID>
        char[] CharBuffer;
        /// <MetaDataID>{d069c048-ed47-4d9a-adce-10ccfef60d96}</MetaDataID>
		public NonTerminalNode(Rule rule,int lineNumber,int linePosition,char[] charBuffer)
		{
			m_rule = rule;
            CharBuffer = charBuffer;
            //LineNumber = lineNumber;
            //LinePosition = linePosition;
		}

        /// <MetaDataID>{2ec96197-5723-4cd1-b8b0-431c65877be1}</MetaDataID>
		public int ReductionNumber 
		{
			get { return m_reductionNumber; }
			set { m_reductionNumber = value; }
		}

        /// <MetaDataID>{85ad245c-f1a4-42b9-ae9f-621d8ff0358f}</MetaDataID>
		public int Count 
		{
			get { return m_array.Count; }
		}

        /// <MetaDataID>{ca7630ec-b6ba-425f-bb78-db4871f44469}</MetaDataID>
		public SyntaxNode this[int index]
		{
			get { return (SyntaxNode) m_array[index]; }
		}

        /// <MetaDataID>{9617eeee-2668-45f2-ab9f-109823225274}</MetaDataID>
        int _StartCharPosition = -1;
        /// <MetaDataID>{6b3a58d0-990a-4bb6-bfb7-33ca526fd175}</MetaDataID>
        int _EndCharPosition = 0;
        /// <MetaDataID>{75b78f3b-b260-4a3d-8668-169ef841e2ae}</MetaDataID>
        public int StartCharPosition
        {
            get { return _StartCharPosition; }
        }

        /// <MetaDataID>{3f6424f7-08df-46d4-b9bb-1a29d1e9b804}</MetaDataID>
        public int EndCharPosition
        {
            get { return _EndCharPosition; }
        }


        /// <MetaDataID>{2baf6008-9d24-4627-90ea-5f7eb4ae4bf3}</MetaDataID>
		public void Add(SyntaxNode node)
		{
			if (node == null)
			{
				return ; //throw new ArgumentNullException("node");
			}
            if (node is NonTerminalNode && (node as NonTerminalNode).m_rule.Hidden)
                return;
            if (node is NonTerminalNode)
            {
                if (_StartCharPosition == -1)
                    _StartCharPosition = (node as NonTerminalNode).StartCharPosition;
                else if (_StartCharPosition > (node as NonTerminalNode).StartCharPosition)
                    _StartCharPosition = (node as NonTerminalNode).StartCharPosition;

                if(_EndCharPosition<(node as NonTerminalNode).EndCharPosition)
                    _EndCharPosition=(node as NonTerminalNode).EndCharPosition;
            }

            if (node is TerminalNode)
            {
                if (_StartCharPosition == -1)
                    _StartCharPosition = (node as TerminalNode).StartCharPosition;
                else if (_StartCharPosition > (node as TerminalNode).StartCharPosition)
                    _StartCharPosition = (node as TerminalNode).StartCharPosition;

                if (_EndCharPosition < (node as TerminalNode).EndCharPosition)
                    _EndCharPosition = (node as TerminalNode).EndCharPosition;
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
            //string tmp = node.ToString();
			m_array.Add(node);
		}

        /// <MetaDataID>{19b7348c-94cd-4b34-ac30-9ddd9d4826fc}</MetaDataID>
		public Rule Rule
		{
			get { return m_rule; }
		}



        #region ParserNode Members

        /// <MetaDataID>{47e9abe1-0ab5-4af3-a6ad-303a6259f87f}</MetaDataID>
        string _Value = null;
        public string Value
        {
            get
            {
                if (_Value == null)
                {
                    if (StartCharPosition < 0)
                        return null;
                    _Value = new string(CharBuffer, StartCharPosition, EndCharPosition - StartCharPosition);
                    return _Value;
                   
                    string temp = _Value;
                    foreach (SyntaxNode syntaxNode in m_array)
                    {
                        if (syntaxNode is TerminalNode)
                        {
                            if ((syntaxNode as TerminalNode).Text == null || (syntaxNode as TerminalNode).Text.Trim().Length == 0)
                                temp += " " + (syntaxNode as TerminalNode).Text;
                            else
                                temp += (syntaxNode as TerminalNode).Text;


                        }
                        else
                        {
                            if (m_rule.Terminal)
                                temp += " " + (syntaxNode as NonTerminalNode).Value;
                            else
                                temp += (syntaxNode as NonTerminalNode).Value;

                        }
                    }
                    _Value=temp.Trim();
                }
                return _Value;
            }
        }

        public int Line
        {
            get 
            { 
                return LineNumber;
            }
        }

        public int LinePosition
        {
            get 
            {
                if (m_array.Count > 0)
                {
                    if (m_array[0] is NonTerminalNode)
                        return (m_array[0] as NonTerminalNode).LinePosition;

                    if (m_array[0] is TerminalNode)
                        return (m_array[0] as TerminalNode).LinePosition;
                }
                return _LinePosition; 
            }
        }

        /// <MetaDataID>{45f0eb66-7efe-4cf8-8d83-6212882e983a}</MetaDataID>
        global::Parser.ParserNode _ParentNode;
        public global::Parser.ParserNode ParentNode
        {
            get { return _ParentNode; }
        }

        /// <MetaDataID>{b75e9937-1563-4f3c-8c9d-a44ad4f1f309}</MetaDataID>
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

        /// <MetaDataID>{9d9960b7-4a1d-41f2-b234-5a0df5783068}</MetaDataID>
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

        #region ParserNode Members

        public bool IsErrorNode
        {
            get 
            {
                return m_rule.Error;
            }
        }

        public bool IsTerminal
        {
            get 
            {
                return m_rule.Terminal;
                
            }
        }

        #endregion
    }
}
