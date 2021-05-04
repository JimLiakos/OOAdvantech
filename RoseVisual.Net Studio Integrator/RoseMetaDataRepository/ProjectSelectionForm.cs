using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RoseMetaDataRepository
{
    /// <MetaDataID>{509692fa-5146-47b1-a9a6-4e42f5d08abb}</MetaDataID>
    internal partial class ProjectSelectionForm : Form
    {

        //class ProjectItem
        //{
        //    public readonly EnvDTE.Project Project;
        //    public ProjectItem(EnvDTE.Project project)
        //    {
        //        Project= project;
        //    }
        //    public override string ToString()
        //    {
        //        return Project.Name;
        //    }
        //}
        //class SolutionItem
        //{
        //    public readonly EnvDTE.DTE DTE;
        //    public SolutionItem(EnvDTE.DTE dte)
        //    {
        //        DTE = dte;
        //    }
        //    public override string ToString()
        //    {
        //        try
        //        {
        //            if (this.DTE.Solution != null)
        //            {
        //                System.IO.FileInfo fileInfo = new System.IO.FileInfo(this.DTE.Solution.FullName);
        //                return fileInfo.Name;
        //            }
        //            else
        //                return "";
        //        }
        //        catch (System.Exception error)
        //        {
        //            return "";
        //        }
        //    }
        //}
        //MsdevManager.MsdevMonitorThread MsdevMonitor;
        public ProjectSelectionForm()
        {
            InitializeComponent();
            //MsdevMonitor = new MsdevManager.MsdevMonitorThread(this, false);
            //MsdevMonitor.Changed += new MsdevManager.MsdevMonitorThread.MonitorMsdevHandler(MsdevMonitor_Changed);
            //MsdevMonitor.Start();
            //foreach (System.Collections.DictionaryEntry entry in MsdevManager.Msdev.GetIDEInstances(true))
            //{

            //    Solutions.Items.Add(new SolutionItem(entry.Value as EnvDTE.DTE));
            //}
            //if (Solutions.Items.Count > 0)
            //    Solutions.SelectedIndex = 0;

        }

        private void SolutionFileLabel_Click(object sender, EventArgs e)
        {

        }

        //void MsdevMonitor_Changed()
        //{
            
        //}

        //private void Solutions_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    SolutionFile.Text= (Solutions.Items[Solutions.SelectedIndex] as SolutionItem).DTE.Solution.FullName;
        //    SolutionFile.SelectionStart = SolutionFile.Text.Length - 1;
        //    Projects.Items.Clear();

        //    foreach (EnvDTE.Project project in (Solutions.Items[Solutions.SelectedIndex] as SolutionItem).DTE.Solution.Projects)
        //    {
        //        //Projects.Items.Add(


        //    }
        //}
    }
}