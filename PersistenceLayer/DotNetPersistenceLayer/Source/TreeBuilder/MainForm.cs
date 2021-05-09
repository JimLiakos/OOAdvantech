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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Text;
using GoldParser;
using System.Reflection;
using System.Globalization;
using OOAdvantech.PersistenceLayer;

#endregion

namespace TreeBuilder
{

    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    /// <MetaDataID>{9d29782a-9a52-43a6-8949-9075f5d1616c}</MetaDataID>
    public class MainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.ToolBar toolBar1;
        private System.Windows.Forms.TabPage tpSource;
        private System.Windows.Forms.TabPage tpParseActions;
        private System.Windows.Forms.TabPage tpParseTree;
        private System.Windows.Forms.ListView lvParseActions;
        private System.Windows.Forms.ColumnHeader chActions;
        private System.Windows.Forms.ColumnHeader chLine;
        private System.Windows.Forms.ColumnHeader chDescription;
        private System.Windows.Forms.ColumnHeader chNo;
        private System.Windows.Forms.ColumnHeader chValue;
        private System.Windows.Forms.ColumnHeader chTableIndex;
        private System.Windows.Forms.TreeView tvParseTree;
        private System.Windows.Forms.ToolBarButton tbOpen;
        private System.Windows.Forms.ToolBarButton tbSave;
        private System.Windows.Forms.ToolBarButton tbSeparator1;
        private System.Windows.Forms.ToolBarButton tbParse;
        private System.Windows.Forms.ImageList ilIcons;
        private System.Windows.Forms.TextBox tbSource;
        private System.Windows.Forms.TabControl tcPages;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.ComponentModel.IContainer components;

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
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
            this.toolBar1 = new System.Windows.Forms.ToolBar();
            this.tbOpen = new System.Windows.Forms.ToolBarButton();
            this.tbSave = new System.Windows.Forms.ToolBarButton();
            this.tbSeparator1 = new System.Windows.Forms.ToolBarButton();
            this.tbParse = new System.Windows.Forms.ToolBarButton();
            this.ilIcons = new System.Windows.Forms.ImageList(this.components);
            this.tcPages = new System.Windows.Forms.TabControl();
            this.tpSource = new System.Windows.Forms.TabPage();
            this.tbSource = new System.Windows.Forms.TextBox();
            this.tpParseActions = new System.Windows.Forms.TabPage();
            this.lvParseActions = new System.Windows.Forms.ListView();
            this.chActions = new System.Windows.Forms.ColumnHeader();
            this.chLine = new System.Windows.Forms.ColumnHeader();
            this.chDescription = new System.Windows.Forms.ColumnHeader();
            this.chNo = new System.Windows.Forms.ColumnHeader();
            this.chValue = new System.Windows.Forms.ColumnHeader();
            this.chTableIndex = new System.Windows.Forms.ColumnHeader();
            this.tpParseTree = new System.Windows.Forms.TabPage();
            this.tvParseTree = new System.Windows.Forms.TreeView();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.tcPages.SuspendLayout();
            this.tpSource.SuspendLayout();
            this.tpParseActions.SuspendLayout();
            this.tpParseTree.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolBar1
            // 
            this.toolBar1.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																						this.tbOpen,
																						this.tbSave,
																						this.tbSeparator1,
																						this.tbParse});
            this.toolBar1.DropDownArrows = true;
            this.toolBar1.ImageList = this.ilIcons;
            this.toolBar1.Location = new System.Drawing.Point(0, 0);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.ShowToolTips = true;
            this.toolBar1.Size = new System.Drawing.Size(688, 28);
            this.toolBar1.TabIndex = 2;
            this.toolBar1.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
            this.toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
            // 
            // tbOpen
            // 
            this.tbOpen.ImageIndex = 4;
            this.tbOpen.Text = "Open";
            this.tbOpen.ToolTipText = "Open Source File";
            // 
            // tbSave
            // 
            this.tbSave.ImageIndex = 5;
            this.tbSave.Text = "Save";
            this.tbSave.ToolTipText = "Save Source File";
            // 
            // tbSeparator1
            // 
            this.tbSeparator1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // tbParse
            // 
            this.tbParse.ImageIndex = 6;
            this.tbParse.Text = "Parse";
            this.tbParse.ToolTipText = "Parse the Source Text";
            // 
            // ilIcons
            // 
            this.ilIcons.ImageSize = new System.Drawing.Size(16, 16);
            this.ilIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilIcons.ImageStream")));
            this.ilIcons.TransparentColor = System.Drawing.Color.Magenta;
            // 
            // tcPages
            // 
            this.tcPages.Controls.Add(this.tpSource);
            this.tcPages.Controls.Add(this.tpParseActions);
            this.tcPages.Controls.Add(this.tpParseTree);
            this.tcPages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcPages.Location = new System.Drawing.Point(0, 28);
            this.tcPages.Name = "tcPages";
            this.tcPages.SelectedIndex = 0;
            this.tcPages.Size = new System.Drawing.Size(688, 554);
            this.tcPages.TabIndex = 3;
            // 
            // tpSource
            // 
            this.tpSource.Controls.Add(this.tbSource);
            this.tpSource.Location = new System.Drawing.Point(4, 22);
            this.tpSource.Name = "tpSource";
            this.tpSource.Size = new System.Drawing.Size(680, 528);
            this.tpSource.TabIndex = 0;
            this.tpSource.Text = "Source";
            // 
            // tbSource
            // 
            this.tbSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbSource.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.tbSource.Location = new System.Drawing.Point(0, 0);
            this.tbSource.Multiline = true;
            this.tbSource.Name = "tbSource";
            this.tbSource.Size = new System.Drawing.Size(680, 528);
            this.tbSource.TabIndex = 0;
            this.tbSource.Text = "Put your source text here";
            // 
            // tpParseActions
            // 
            this.tpParseActions.Controls.Add(this.lvParseActions);
            this.tpParseActions.Location = new System.Drawing.Point(4, 22);
            this.tpParseActions.Name = "tpParseActions";
            this.tpParseActions.Size = new System.Drawing.Size(680, 514);
            this.tpParseActions.TabIndex = 1;
            this.tpParseActions.Text = "Parse Actions";
            // 
            // lvParseActions
            // 
            this.lvParseActions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							 this.chActions,
																							 this.chLine,
																							 this.chDescription,
																							 this.chNo,
																							 this.chValue,
																							 this.chTableIndex});
            this.lvParseActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvParseActions.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.lvParseActions.FullRowSelect = true;
            this.lvParseActions.GridLines = true;
            this.lvParseActions.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvParseActions.HideSelection = false;
            this.lvParseActions.Location = new System.Drawing.Point(0, 0);
            this.lvParseActions.MultiSelect = false;
            this.lvParseActions.Name = "lvParseActions";
            this.lvParseActions.Size = new System.Drawing.Size(680, 514);
            this.lvParseActions.SmallImageList = this.ilIcons;
            this.lvParseActions.TabIndex = 0;
            this.lvParseActions.View = System.Windows.Forms.View.Details;
            // 
            // chActions
            // 
            this.chActions.Text = "Action";
            this.chActions.Width = 123;
            // 
            // chLine
            // 
            this.chLine.Text = "Line";
            this.chLine.Width = 40;
            // 
            // chDescription
            // 
            this.chDescription.Text = "Description";
            this.chDescription.Width = 256;
            // 
            // chNo
            // 
            this.chNo.Text = "#";
            this.chNo.Width = 25;
            // 
            // chValue
            // 
            this.chValue.Text = "Value";
            this.chValue.Width = 81;
            // 
            // chTableIndex
            // 
            this.chTableIndex.Text = "Table Index";
            this.chTableIndex.Width = 117;
            // 
            // tpParseTree
            // 
            this.tpParseTree.Controls.Add(this.tvParseTree);
            this.tpParseTree.Location = new System.Drawing.Point(4, 22);
            this.tpParseTree.Name = "tpParseTree";
            this.tpParseTree.Size = new System.Drawing.Size(680, 514);
            this.tpParseTree.TabIndex = 2;
            this.tpParseTree.Text = "ParseTree";
            // 
            // tvParseTree
            // 
            this.tvParseTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvParseTree.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.tvParseTree.ImageList = this.ilIcons;
            this.tvParseTree.Location = new System.Drawing.Point(0, 0);
            this.tvParseTree.Name = "tvParseTree";
            this.tvParseTree.Size = new System.Drawing.Size(680, 514);
            this.tvParseTree.TabIndex = 0;
            // 
            // ParseTest
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(688, 582);
            this.Controls.Add(this.tcPages);
            this.Controls.Add(this.toolBar1);
            this.Name = "ParseTest";
            this.Text = "Parse Test";
            this.tcPages.ResumeLayout(false);
            this.tpSource.ResumeLayout(false);
            this.tpParseActions.ResumeLayout(false);
            this.tpParseTree.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            //ObjectStorage objectStorage = null;
            //objectStorage = ObjectStorage.OpenStorage("Abstractions",
            //                                            @"c:\Abstractions.xml",
            //                                            "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
            //string objectQuery = "#OQL: SELECT order " +
            //                     " FROM AbstractionsAndPersistency.IOrder order where order.Name Like 'aK_9*'#";

            //StructureSet objectSet = objectStorage.Execute(objectQuery);
            //foreach (StructureSet instanceSet in objectSet)
            //{
            //    object obj = instanceSet["order"];


            //}
 


            bool asa = Like("", "Car*nd *mver");
            asa = SQL92Like("s%666amsgsgs", @"s\%%ams%gs"); 
            int mal =254;
            char sd = (char)mal;
             
            Application.Run(new MainForm());
        }
        public static bool Like(string values, string patern)
        {
            if (patern == null || values==null)
                return false;
            patern = patern.Replace(@"\*", "char(" + ((int)'*').ToString() + ")");
            patern = patern.Replace("*", @"(\w){0,}");
            patern = patern.Replace("char(" + ((int)'*').ToString() + ")", "*");
            patern = patern.Replace("?", @"(\w){1}");
            patern = "^" + patern + @"\z";
            patern = patern.Replace(' ', (char)254);
            values = values.Replace(' ', (char)254);
            return System.Text.RegularExpressions.Regex.Match(values, patern,System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace).Success;
        }
        public static bool SQL92Like(string values, string patern)
        {
            patern = patern.Replace(@"\%", "char(" + ((int)'%').ToString() + ")");
            patern = patern.Replace("%", @"(\w){0,}");
            patern = patern.Replace("char(" + ((int)'%').ToString() + ")", "%");
            patern = patern.Replace("?", @"(\w){1}");
            patern = "^" + patern + @"\z";
            patern = patern.Replace(' ', (char)254);
            values = values.Replace(' ', (char)254);
            return System.Text.RegularExpressions.Regex.Match(values, patern).Success;
        }


        private void toolBar1_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
        {

            
            if (e.Button == tbOpen)
            {
                OpenFile();
            }
            else if (e.Button == tbSave)
            {
                SaveFile();
            }
            else if (e.Button == tbParse)
            {
                ParseText();
            }
        }

        private void ClearData()
        {
            lvParseActions.Items.Clear();
            tvParseTree.Nodes.Clear();
        }

        private void OpenFile()
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                ClearData();
                FileStream reader = new FileStream(openFileDialog.FileName, System.IO.FileMode.Open);
                byte[] buffer = new byte[reader.Length];
                reader.Read(buffer, 0, (int)reader.Length);
                tbSource.Text = System.Text.Encoding.Default.GetString(buffer);
                reader.Close();
            }
        }

        private void SaveFile()
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(saveFileDialog.FileName);
                writer.Write(tbSource.Text);
                writer.Close();
            }
        }

        private BinaryReader GetResourceReader(string resourceName)
        {

            Assembly assembly = this.GetType().Assembly;
            Stream stream = assembly.GetManifestResourceStream(resourceName);
            return new BinaryReader(stream);
        }

        //private struct LigatureInfo
        //{
        //    internal LikeOperator.CharKind Kind;
        //    internal char CharBeforeExpansion;
        //}





        //public static bool LikeString(string Source, string Pattern)
        //{

        //    CompareInfo compareInfo;
        //    CompareOptions ordinal;
        //    char ch;
        //    int num;
        //    int length;
        //    LigatureInfo[] inputLigatureInfo = null;
        //    int num3;
        //    int num4;
        //    LigatureInfo[] infoArray2 = null;
        //    bool flag7;
        //    if (Pattern == null)
        //    {
        //        length = 0;
        //    }
        //    else
        //    {
        //        length = Pattern.Length;
        //    }
        //    if (Source == null)
        //    {
        //        num4 = 0;
        //    }
        //    else
        //    {
        //        num4 = Source.Length;
        //    }
        //    //if (CompareOption == CompareMethod.Binary)
        //    //{
        //    //    ordinal = CompareOptions.Ordinal;
        //    //    compareInfo = null;
        //    //}
        //    //else
        //    {
        //        compareInfo = Utils.GetCultureInfo().CompareInfo;
        //        ordinal = CompareOptions.IgnoreWidth | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreCase;
        //        byte[] localeSpecificLigatureTable = new byte[(LigatureExpansions.Length - 1) + 1];
        //        flag7 = false;
        //        ExpandString(ref Source, ref num4, ref infoArray2, localeSpecificLigatureTable, compareInfo, ordinal, ref flag7, false);
        //        flag7 = false;
        //        ExpandString(ref Pattern, ref length, ref inputLigatureInfo, localeSpecificLigatureTable, compareInfo, ordinal, ref flag7, false);
        //    }
        //    while ((num < length) && (num3 < num4))
        //    {
        //        ch = Pattern[num];
        //        switch (ch)
        //        {
        //            case '?':
        //            case 0xff1f:
        //                SkipToEndOfExpandedChar(infoArray2, num4, ref num3);
        //                break;

        //            case '#':
        //            case 0xff03:
        //                if (!char.IsDigit(Source[num3]))
        //                {
        //                    return false;
        //                }
        //                break;

        //            case '[':
        //            case 0xff3b:
        //                {
        //                    bool flag2;
        //                    bool flag3;
        //                    bool flag4;
        //                    flag7 = false;
        //                    MatchRange(Source, num4, ref num3, infoArray2, Pattern, length, ref num, inputLigatureInfo, ref flag3, ref flag2, ref flag4, compareInfo, ordinal, ref flag7, null, false);
        //                    if (flag4)
        //                    {
        //                        throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Pattern" }));
        //                    }
        //                    if (flag2)
        //                    {
        //                        return false;
        //                    }
        //                    if (!flag3)
        //                    {
        //                        break;
        //                    }
        //                    num++;
        //                    continue;
        //                }
        //            case '*':
        //            case 0xff0a:
        //                bool flag5;
        //                bool flag6;
        //                MatchAsterisk(Source, num4, num3, infoArray2, Pattern, length, num, inputLigatureInfo, ref flag5, ref flag6, compareInfo, ordinal);
        //                if (flag6)
        //                {
        //                    throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Pattern" }));
        //                }
        //                return !flag5;

        //            default:
        //                if (CompareChars(Source, num4, num3, ref num3, infoArray2, Pattern, length, num, ref num, inputLigatureInfo, compareInfo, ordinal, false, false) != 0)
        //                {
        //                    return false;
        //                }
        //                break;
        //        }
        //        num++;
        //        num3++;
        //    }
        //    while (num < length)
        //    {
        //        ch = Pattern[num];
        //        if ((ch == '*') || (ch == 0xff0a))
        //        {
        //            num++;
        //        }
        //        else
        //        {
        //            if (((num + 1) >= length) || (((ch != '[') || (Pattern[num + 1] != ']')) && ((ch != 0xff3b) || (Pattern[num + 1] != 0xff3d))))
        //            {
        //                break;
        //            }
        //            num += 2;
        //        }
        //    }
        //    return ((num >= length) && (num3 >= num4));
        //}






        private void ParseText()
        {
            ClearData();
            //if (m_grammar == null)
            //{

            //    //BinaryReader reader = GetResourceReader("TreeBuilder.VBScript.cgt");		
            //    BinaryReader reader = GetResourceReader("TreeBuilder.EmbeddedOQL.cgt");
           // BinaryReader reader = GetResourceReader("TreeBuilder.SQLite.cgt");
            
            //BinaryReader reader = GetResourceReader("TreeBuilder.OQL.cgt");
            //  // BinaryReader reader = GetResourceReader("TreeBuilder.CompositeOQL.cgt");		
            //   m_grammar = new Grammar(reader);

            //}
            //BinaryReader reader = GetResourceReader("TreeBuilder.EmbeddedOQL.cgt");
            // BinaryReader reader = GetResourceReader("TreeBuilder.CompositeOQL.cgt");		
             BinaryReader reader = GetResourceReader("TreeBuilder.Generics.cgt");
            byte[] grammar = new byte[reader.BaseStream.Length];
            reader.Read(grammar, 0, (int)reader.BaseStream.Length);


            Parser.Parser parser = new Parser.Parser();
            parser.SetGrammar(grammar, grammar.Length);

            //parser.ParseAction = new ParseActionDelegate(AddParseAction);

            byte[] text = System.Text.Encoding.Default.GetBytes(tbSource.Text);
            //FileStream tr = System.IO.File.Open(@"C:\temp\mitsos.txt", FileMode.OpenOrCreate);
            //tr.Write(text, 0, text.Length);
            //tr.Close();

            parser.Parse(tbSource.Text);

            if (parser.theRoot != null)
            {
                BuildParseTree(parser.theRoot);
                tcPages.SelectedIndex = 2;
            }
            else
            {
                tcPages.SelectedIndex = 1;
            }
        }

        //private void AddParseAction(GoldParser.GoldParser parser, ParseMessage action, string description, 
        //    string reductionNo, string value, string tableIndex)
        //{
        //    Font font = lvParseActions.Font;
        //    Color foreColor = lvParseActions.ForeColor;
        //    Color backColor = Color.White;
        //    IconType iconType = IconType.Error;
        //    string actionName = "Error";
        //    switch (action) 
        //    {
        //        case ParseMessage.TokenRead:
        //            actionName = "Token Read";
        //            iconType = IconType.Token;
        //            break;

        //        case ParseMessage.Reduction:
        //            actionName = "Reduction";
        //            iconType = IconType.Reduction;
        //            foreColor = Color.FromArgb(0x60, 0x30, 0x18);
        //            backColor = Color.White;
        //            break;

        //        case ParseMessage.Accept:
        //            actionName = "Accept";
        //            iconType = IconType.Accept;
        //            foreColor = Color.FromArgb(0x00, 0x60, 0x00);
        //            backColor = Color.White;
        //            break;

        //        case ParseMessage.CommentError:
        //            actionName = "Comment Error";
        //            goto default;

        //        case ParseMessage.InternalError:
        //            actionName = "Internal Error";
        //            goto default;

        //        case ParseMessage.LexicalError:
        //            actionName = "Tokenizer Error";
        //            goto default;

        //        case ParseMessage.NotLoadedError:
        //            actionName = "Not Loaded Error";
        //            goto default;

        //        case ParseMessage.SyntaxError:
        //            actionName = "Syntax Error";
        //            goto default;


        //        default:
        //            iconType = IconType.Error;
        //            foreColor = Color.FromArgb(0x40, 0x00, 0x00);
        //            backColor = Color.White;
        //            break;					
        //    }

        //    ListViewItem item = new ListViewItem(new string[] 
        //        {actionName, parser.LineNumber.ToString(), description,
        //        reductionNo, value, tableIndex}, Convert.ToInt32(iconType), foreColor, backColor, font);
        //    lvParseActions.Items.Add(item);
        //}

        private void BuildParseTree(Parser.ParserNode nonTerminal)
        {
            tvParseTree.Nodes.Clear();
            TreeNode node = null;
            if (nonTerminal.Value == null)
            {
                node = new TreeNode("ParserTree", 7, 7);
            }
            else
            {
                node = new TreeNode("ParserTree", 7, 7);

            }

            tvParseTree.Nodes.Add(node);



            for (int i = 0; i < nonTerminal.ChildNodes.Count; i++)
            {
                Parser.ParserNode childNode = nonTerminal.ChildNodes.GetAt(i + 1);
                BuildParseTree(childNode, node);
            }
            node.Expand();
        }

        private void BuildParseTree(Parser.ParserNode syntaxNode, TreeNode parentNode)
        {

            if (syntaxNode != null)
            {
                TreeNode node = null;
                if (!syntaxNode.IsTerminal)
                {
                    if (syntaxNode.IsErrorNode)
                    {
                        node = new TreeNode(syntaxNode.Name,
                        Convert.ToInt32(IconType.Error),
                        Convert.ToInt32(IconType.Error));

                    }
                    else
                    {
                        node = new TreeNode(syntaxNode.Name,
                            Convert.ToInt32(IconType.Reduction),
                            Convert.ToInt32(IconType.Reduction));
                    }
                }
                else
                {
                    if (syntaxNode.IsErrorNode)
                    {
                        node = new TreeNode(syntaxNode.Name + " = " + syntaxNode.Value,
                            Convert.ToInt32(IconType.Error),
                            Convert.ToInt32(IconType.Error));
                    }
                    else
                    {

                        node = new TreeNode(syntaxNode.Name + " = " + syntaxNode.Value,
                            Convert.ToInt32(IconType.Reduction),
                            Convert.ToInt32(IconType.Reduction));
                    }
                }

                parentNode.Nodes.Add(node);


                TreeNode propertiesNode = new TreeNode("Properties",
                            Convert.ToInt32(IconType.Reduction),
                            Convert.ToInt32(IconType.Reduction));
                node.Nodes.Add(propertiesNode);

                TreeNode valueNode = new TreeNode("Value = " + syntaxNode.Value);
                propertiesNode.Nodes.Add(valueNode);
                TreeNode lineNode = new TreeNode("LineNumber = " + syntaxNode.Line);
                propertiesNode.Nodes.Add(lineNode);
                TreeNode LinePosition = new TreeNode("LinePosition = " + syntaxNode.LinePosition);
                propertiesNode.Nodes.Add(LinePosition);

                for (int i = 0; i < syntaxNode.ChildNodes.Count; i++)
                {
                    Parser.ParserNode childNode = syntaxNode.ChildNodes.GetAt(i + 1);
                    BuildParseTree(childNode, node);
                }
            }
            else
            {
                TreeNode node = new TreeNode(syntaxNode.ToString(),
                    Convert.ToInt32(IconType.Token),
                    Convert.ToInt32(IconType.Token));
                parentNode.Nodes.Add(node);
            }
        }

        private enum IconType
        {
            Token = 0,
            Reduction = 1,
            Accept = 2,
            Error = 3
        }
    }
}
