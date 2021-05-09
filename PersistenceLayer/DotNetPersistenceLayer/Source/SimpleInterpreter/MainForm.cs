#region Using directives

using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using GoldParser;

#endregion

namespace SimpleInterpreter
{
	/// <summary>
	/// Summary description for MainForm.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private Grammar m_grammar;
		private SimpleStatement m_program; //'This will point to the tree created by the parser
		private SimpleContext m_context;

		//Private Parser As New GOLDParserEngine.GOLDParser

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtProgram;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnShowVersion;
		private System.Windows.Forms.Button btnParse;
		private System.Windows.Forms.Button btnExecute;
		private System.Windows.Forms.ListBox lstLog;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.txtProgram = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.lstLog = new System.Windows.Forms.ListBox();
			this.btnShowVersion = new System.Windows.Forms.Button();
			this.btnParse = new System.Windows.Forms.Button();
			this.btnExecute = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "\"Simple\" program";
			// 
			// txtProgram
			// 
			this.txtProgram.Location = new System.Drawing.Point(16, 40);
			this.txtProgram.Multiline = true;
			this.txtProgram.Name = "txtProgram";
			this.txtProgram.Size = new System.Drawing.Size(712, 264);
			this.txtProgram.TabIndex = 1;
			this.txtProgram.Text = "";
			this.txtProgram.TextChanged += new System.EventHandler(this.txtProgram_TextChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 312);
			this.label2.Name = "label2";
			this.label2.TabIndex = 2;
			this.label2.Text = "Parse Log";
			// 
			// lstLog
			// 
			this.lstLog.Location = new System.Drawing.Point(16, 344);
			this.lstLog.Name = "lstLog";
			this.lstLog.Size = new System.Drawing.Size(712, 82);
			this.lstLog.TabIndex = 3;
			// 
			// btnShowVersion
			// 
			this.btnShowVersion.Location = new System.Drawing.Point(16, 440);
			this.btnShowVersion.Name = "btnShowVersion";
			this.btnShowVersion.Size = new System.Drawing.Size(120, 23);
			this.btnShowVersion.TabIndex = 4;
			this.btnShowVersion.Text = "Show About Window";
			this.btnShowVersion.Click += new System.EventHandler(this.btnShowVersion_Click);
			// 
			// btnParse
			// 
			this.btnParse.Location = new System.Drawing.Point(480, 440);
			this.btnParse.Name = "btnParse";
			this.btnParse.TabIndex = 5;
			this.btnParse.Text = "Parse";
			this.btnParse.Click += new System.EventHandler(this.btnParse_Click);
			// 
			// btnExecute
			// 
			this.btnExecute.Enabled = false;
			this.btnExecute.Location = new System.Drawing.Point(584, 440);
			this.btnExecute.Name = "btnExecute";
			this.btnExecute.TabIndex = 6;
			this.btnExecute.Text = "Execute";
			this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(744, 470);
			this.Controls.Add(this.btnExecute);
			this.Controls.Add(this.btnParse);
			this.Controls.Add(this.btnShowVersion);
			this.Controls.Add(this.lstLog);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtProgram);
			this.Controls.Add(this.label1);
			this.Name = "MainForm";
			this.Text = "Simple Interprester";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new MainForm());
		}

		private void btnExecute_Click(object sender, System.EventArgs e)
		{
			if (m_program != null)
			{
				m_program.Execute();
			}
		}

		private void btnParse_Click(object sender, System.EventArgs e)
		{
			lstLog.Items.Clear();
			btnExecute.Enabled = DoParse();
		}

		private void btnShowVersion_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show(m_grammar.About);
		}

		private void MainForm_Load(object sender, System.EventArgs e)
		{
			string fileName = Path.GetDirectoryName(Application.ExecutablePath) + @"\..\SimpleGrammar\simple 2.cgt";
			using (Stream stream = File.OpenRead(fileName))
			{
				BinaryReader reader = new BinaryReader(stream);
				m_grammar = new Grammar(reader);
			}
		}

		private void txtProgram_TextChanged(object sender, System.EventArgs e)
		{
			btnExecute.Enabled = false;
		}

		private bool DoParse()
		{
			//This procedure starts the GOLD Parser Engine and handles each of the
			//messages it returns. Each time a reduction is made, a new instance of a
			//"Simple" object is created and stored in the parse tree. The resulting tree
			//will be a pure representation of the Simple language and will be ready to
			//implement.
   			StringReader reader = new StringReader(txtProgram.Text);
			Parser parser = new Parser(reader, m_grammar);
			parser.TrimReductions = true;
			m_context = new SimpleContext(parser);
			while (true)
			{
				switch (parser.Parse())
				{
					case ParseMessage.LexicalError:
						Log("LEXICAL ERROR. Line " + parser.LineNumber + ". Cannot recognize token: " + parser.TokenText);
						return false;
                  
					case ParseMessage.SyntaxError:
						StringBuilder text = new StringBuilder();
						foreach (Symbol tokenSymbol in parser.GetExpectedTokens())
						{
							text.Append(' ');
							text.Append(tokenSymbol.ToString());
						}
						Log("SYNTAX ERROR. Line " + parser.LineNumber + ". Expecting:" + text.ToString());
						return false;
              
					case ParseMessage.Reduction:
						//== Create a new customized object and replace the
						//== CurrentReduction with it. This saves memory and allows
						//== easier interpretation
						parser.TokenSyntaxNode = m_context.GetSimpleObject();
						break;
                
					case ParseMessage.Accept:
						//=== Success!
						m_program = (SimpleStatement) parser.TokenSyntaxNode;                
						Log("-- Program Accepted --");
						return true;
              
					case ParseMessage.TokenRead:
						//=== Make sure that we store token string for needed tokens.
						parser.TokenSyntaxNode = m_context.GetTokenText();
						break;
               
					case ParseMessage.InternalError:
						Log("INTERNAL ERROR! Something is horribly wrong");
						return false;
               
					case ParseMessage.NotLoadedError:
						//=== Due to the if-statement above, this case statement should never be true
						Log("NOT LOADED ERROR! Compiled Grammar Table not loaded");
						return false;
              
					case ParseMessage.CommentError:
						Log("COMMENT ERROR! Unexpected end of file");
						return false;
            
					case ParseMessage.CommentBlockRead:
						//=== Do nothing
						break;
               
					case ParseMessage.CommentLineRead:
						//=== Do nothing
						break;
				}
			}
		}

		private void Log(string text)
		{
			lstLog.Items.Add(text);
		}
	}
}
