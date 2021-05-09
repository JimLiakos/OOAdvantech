namespace OOAdvantech.CSharpOQLParser
{
	
	/// <MetaDataID>{AAC14309-31D2-4F80-AD16-A932C56AD3E9}</MetaDataID>
	public class EmbeddedOQLParser
	{
		
		public struct Error
		{
			public Error(string errorMessage,string file,int line)
			{
                 
				ErrorMessage=errorMessage;
				File=file;
				Line=line;
			}

			public string ErrorMessage;
			public string File;
			public int Line;
		}
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{2AA365E6-F261-4A0E-A1AC-4F45A506BC3C}</MetaDataID>
		private Parser.Parser _CompositeOQLParser;
		/// <MetaDataID>{463E40F0-1198-4B81-96D6-E5AAB10F9DEC}</MetaDataID>
		private Parser.Parser CompositeOQLParser
		{
			get
			{
				if(_CompositeOQLParser==null)
				{
					_CompositeOQLParser=new Parser.Parser();
					
					System.Type mType=GetType();
					string[] Resources=System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames();
					//using( System.IO.Stream Grammar = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("OOAdvantech.CSharpOQLParser.Grammars.CompositeOQL.GMR"))
                    using (System.IO.Stream Grammar = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("OOAdvantech.CSharpOQLParser.Grammars.CompositeOQL.cgt"))
					{
						byte[] bytes = new byte[Grammar.Length];
						//NewFile.Write(
						Grammar.Read(bytes,0,(int)Grammar.Length);
						_CompositeOQLParser.SetGrammar(bytes,(int)Grammar.Length);
						Grammar.Close();
					}
				}
				return _CompositeOQLParser;

			}
		}
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{18855E65-C1F8-48F8-A32B-B1D141C810F1}</MetaDataID>
		private Parser.Parser _CSharOQLParser;
		/// <summary/>
		/// <MetaDataID>{863D0025-2709-4800-80FD-7DD5DA00BA3E}</MetaDataID>
		private Parser.Parser CSharOQLParser
		{
			get
			{
				
				if(_CSharOQLParser==null)
				{
					_CSharOQLParser=new Parser.Parser();
					
					System.Type mType=GetType();
					string[] Resources=System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames();
					//using( System.IO.Stream Grammar = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("OOAdvantech.CSharpOQLParser.Grammars.EmbeddedOQL.GMR"))
                    using (System.IO.Stream Grammar = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("OOAdvantech.CSharpOQLParser.Grammars.EmbeddedOQL.cgt"))
					{
						byte[] bytes = new byte[Grammar.Length];
						//NewFile.Write(
						Grammar.Read(bytes,0,(int)Grammar.Length);
						_CSharOQLParser.SetGrammar(bytes,(int)Grammar.Length);
						Grammar.Close();
					}
				}
				return _CSharOQLParser;

			}
		}

		public void CheckProject(EnvDTE.Project project ,ref System.Collections.ArrayList errors)
		{
            //return;
			if(project!=null)
			{
				string OutputDLLName = project.Properties.Item("FullPath").Value as string;
                OutputDLLName += project.ConfigurationManager.ActiveConfiguration.Properties.Item("OutputPath").Value as string;
				OutputDLLName += project.Properties.Item("OutputFileName").Value as string;
				System.Collections.ArrayList codeFiles=new System.Collections.ArrayList();
				foreach(EnvDTE.ProjectItem projectItem in project.ProjectItems)
				{
					EnvDTE.Properties  properties=projectItem.Properties;
					string LocalPath=properties.Item("LocalPath").Value as	string;
					string SubType=properties.Item("SubType").Value as	string;
					int  BuildAction=(int)properties.Item("BuildAction").Value ;
					if(SubType=="Code"&&BuildAction==1)
						codeFiles.Add(LocalPath);

				}
				if(codeFiles.Count>0)
					Parse(codeFiles,OutputDLLName,ref errors);
			}
			
		}

		public bool Parse(System.Collections.ArrayList codeFiles,string aseemblyFile,ref System.Collections.ArrayList errors)
		{
			System.Reflection.Assembly dotNetAssembly=System.Reflection.Assembly.LoadFile(aseemblyFile);
			return ParseAssembly(codeFiles,dotNetAssembly,ref errors);
		}
		

		/// <MetaDataID>{1F54EE81-2DEA-484A-8580-1F42D02704FB}</MetaDataID>
		public bool ParseAssembly(System.Collections.ArrayList codeFiles,System.Reflection.Assembly dotNetAssembly,ref System.Collections.ArrayList errors)
		{
			bool hasError=false;
            return hasError;
			try
			{
				//ModulePublisher.ClassRepository.Initialize();
				Parser.Parser parser = this.CSharOQLParser;
				
				
				DotNetMetaDataRepository.Assembly assembly=DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(dotNetAssembly) as DotNetMetaDataRepository.Assembly;
				if(assembly==null)
					assembly=new DotNetMetaDataRepository.Assembly(dotNetAssembly);

				foreach(string codeFile in codeFiles)
				{
				
					try
					{
						if(!System.IO.File.Exists(codeFile))
							errors.Add(new Error("System can't find file",codeFile,0));
						System.IO.StreamReader reader=System.IO.File.OpenText(codeFile);
						string code=reader.ReadToEnd();
						reader.Close();
                        try
                        {
                            parser.Parse(code);
                        }
                        catch (System.Exception error)
                        {
                        
                            if (parser.theRoot == null)
                                errors.Add(new Error("Unknown error. " +error.Message+" "+ error.StackTrace, codeFile, 1));
                            else
                            {

                                foreach (Parser.ParserNode errorParserNode in parser.theRoot.ChildNodes)
                                    errors.Add(new Error("Error on OQL statement definition. The OQL statement must be enclosed in \"#OQL: ...... #\" adornments.", codeFile, errorParserNode.Line));
                            }
                            hasError = true;
                            continue;
                        }
						Parser.ParserNode parserNode= parser.theRoot["Compilation_Unit"] as Parser.ParserNode;
						foreach(Parser.ParserNode OQLparserNode in parserNode.ChildNodes)
						{
							string compositeOQL=OQLparserNode.Value;
							//System.Diagnostics.Debug.WriteLine(QQLparserNode.Value);
							compositeOQL="\""+compositeOQL+"\"";
							CompositeOQLParser.Parse( compositeOQL);
							Parser.ParserNode compositeParserNode=CompositeOQLParser.theRoot["Composite_Expression"] as Parser.ParserNode;
							string OQLStatament="";
							foreach(Parser.ParserNode OQLPartParserNode in compositeParserNode.ChildNodes)
							{
								OQLStatament+=OQLPartParserNode.Value.Substring(1,OQLPartParserNode.Value.Length-2);
								int rert=0;
							}

                            OQLStatament=OQLStatament.Replace("\\n", "\n");
                            OQLStatament = OQLStatament.Replace("\\r", "\r");
                            OQLStatament = OQLStatament.Replace("\\a", "\a");
                            OQLStatament = OQLStatament.Replace("\\b", "\b");
                            OQLStatament = OQLStatament.Replace("\\f", "\f");
                            OQLStatament = OQLStatament.Replace("\\t", "\t");
                            OQLStatament = OQLStatament.Replace("\\v", "\v");
                            OQLStatament = OQLStatament.Replace("\\'", "\'");
                            OQLStatament = OQLStatament.Replace("\\\"", "\"");
                            OQLStatament = OQLStatament.Replace("\\\\", "\\");

                            OOAdvantech.MetaDataRepository.ObjectQueryLanguage.OQLStatement oqlStatement=new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.OQLStatement(null);
							
							string statamentError=null;
							hasError|=oqlStatement.Parse(OQLStatament,ref statamentError);
							if(statamentError!=null)
								errors.Add(new Error(statamentError, codeFile,OQLparserNode.Line));
					
						}
						int ttrtr=0;
				
					}
					catch(System.Exception error)
					{
						errors.Add(new Error(error.Message+" "+error.StackTrace,null,0));
						hasError=true;
						int werewe=0;
					}
				}
			}
			catch(System.Exception error)
			{
				errors.Add(new Error(error.Message +" "+error.StackTrace,null,0));
				hasError=true;
			}
			return hasError;


		
		}
	}
}
