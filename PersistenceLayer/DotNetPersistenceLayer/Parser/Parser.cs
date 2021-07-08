namespace Parser
{
    /// <MetaDataID>{ade451c5-9f46-498d-bfa6-f869d303cb9e}</MetaDataID>
    public class SyntaxError
    {
        public readonly int Line;
        public readonly int LinePosition;
        public readonly string Token;
        public readonly string ErrorMessage;
        public SyntaxError(int line, int linePosition, string token, string errorMessage)
        {
            Line = line;
            LinePosition = linePosition;
            Token = token;
            ErrorMessage = errorMessage;
        }
        public override string ToString()
        {
            return ErrorMessage + "  Line:" + Line.ToString() + "  LinePosition:" + LinePosition.ToString();
        }
          
    }
	/// <MetaDataID>{869340B0-F55F-4C6B-823F-3D1E17E83A0C}</MetaDataID>
	public class Parser
	{
        /// <MetaDataID>{41307037-d97b-4357-af41-70b6add9d1ac}</MetaDataID>
        GoldParser.Grammar GoldGrammar;
		/// <MetaDataID>{14484ECD-5213-4C0C-8AB8-9D89431FC831}</MetaDataID>
		public Parser()
		{
            //theWrParser=new ParserWr.Parser();
           
		}
        /// <MetaDataID>{6459ea76-ca40-4b78-88b1-bd429c43a75b}</MetaDataID>
        public System.Collections.Generic.List<SyntaxError> SyntaxErrors=new System.Collections.Generic.List<SyntaxError>();

        ///// <MetaDataID>{07B205B0-ECBA-4626-B548-4C015CE54493}</MetaDataID>
        //private ParserWr.Parser theWrParser;
		/// <MetaDataID>{F7D3FBF4-BD75-47D3-9333-B75E35092878}</MetaDataID>
		public ParserNode theRoot;
		/// <MetaDataID>{D87FA618-F554-413D-8090-5C44860E866F}</MetaDataID>
		public void Parse(string OQLStatament)
		{
            GoldParser.TreeParser treeParser=new GoldParser.TreeParser(GoldGrammar);
            treeParser.ParseAction += new GoldParser.ParseActionDelegate(OnParseAction);
            theRoot = treeParser.Parse(OQLStatament) as ParserNode;

            if (theRoot == null)
            {
                SyntaxError syntaxError =new SyntaxError(treeParser.LineNumber, treeParser.LinePosition, null, treeParser.ErrorMessage);
                SyntaxErrors.Add(syntaxError);
                throw new System.Exception(syntaxError.ToString());
            }

            System.Collections.Generic.List<GoldParser.NonTerminalNode> errorNodes = (theRoot as GoldParser.NonTerminalNode).GetErrorStatements();
            if (errorNodes.Count > 0)
            { 
                foreach (GoldParser.NonTerminalNode errorNode in errorNodes)
                {
                    SyntaxError syntaxError =new SyntaxError(errorNode.Line, errorNode.LinePosition, errorNode.Name, errorNode.Value);
                    SyntaxErrors.Add(syntaxError);
                } 
                throw new System.Exception("Syntax error");
            }
                
            return;
            



            ////TODO Να εξεταστεί αν υπάρχει πρόβλημα στην προταση WHERE product.Name='Balantain's'
            //theRoot= new ProGrammarParserNode(theWrParser.Parse(OQLStatament),theWrParser);
			
            //if(!theWrParser.IsErrorFree())
            //    throw new System.Exception("Syntax error");
			//IsErrorFree
		}

        /// <MetaDataID>{041d7798-dcdf-4a81-9d68-49ca2951db59}</MetaDataID>
        System.Collections.Generic.List<string> SyntaxErrorsMessage = new System.Collections.Generic.List<string>();

        /// <MetaDataID>{89dabc26-07ec-46b2-ab87-9f5af8d37506}</MetaDataID>
        void OnParseAction(GoldParser.GoldParser parser, GoldParser.ParseMessage action, string description, string reductionNo, string value, string tableIndex)
        {
            if (action == GoldParser.ParseMessage.SyntaxError || action == GoldParser.ParseMessage.InternalError ||
                action == GoldParser.ParseMessage.LexicalError)
            {
                SyntaxErrorsMessage.Add(action.ToString() + " on : " + parser.LineText + " [" + parser.TokenText + "]    \"" + description + "  " + value + "\"");
            }

            
        }
		/// <MetaDataID>{B2BC33B9-ACA5-4542-A07C-C80F7FF14458}</MetaDataID>
		public void SetGrammarPath(string FileName)
		{
            //theWrParser.SetGrammarPath(FileName);
		}

	
		/// <MetaDataID>{5B45A66B-6D54-4795-9458-E3896862DA48}</MetaDataID>
		public void SetGrammar(System.Byte[] Buffer,int Length)
		{

            

            System.IO.BinaryReader grammarReader=new System.IO.BinaryReader(new System.IO.MemoryStream(Buffer));
            GoldGrammar = new GoldParser.Grammar(grammarReader);
            
            //theWrParser.SetGrammar(Buffer,Length);
            

		}
	
	}
}
