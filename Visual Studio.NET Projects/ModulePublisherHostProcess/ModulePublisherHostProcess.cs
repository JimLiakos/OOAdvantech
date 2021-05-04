using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace ModulePublisherHostProcess
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class ModulePublisherHostProcess 
	{
	
		[STAThread]
		static void Main(string[] args)
		//static void Main() 
		{
			try
			{
				//System.Windows.Forms.MessageBox.Show(args[0],"ModulePublisherHostProcess");
				if(args.Length>1)
					ModulePublisher.ClassRepository.Publish(args[0],args[1]);
				else
					ModulePublisher.ClassRepository.Publish(args[0],"");

			}
			catch(System.Exception E)
			{

			}
		}
	}
}
