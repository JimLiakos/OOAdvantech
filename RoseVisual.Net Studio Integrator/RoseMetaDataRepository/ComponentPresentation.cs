using System;
using System.Collections.Generic;
using System.Text;

namespace RoseMetaDataRepository
{
    /// <MetaDataID>{d65a3b17-41da-4fc4-ae84-8009e3d7ffc9}</MetaDataID>
    class ComponentPresentation : OOAdvantech.UserInterface.Runtime.PresentationObject<Component>
    {
        static MsdevManager.MsdevMonitorThread MsdevMonitor = null;
        public ComponentPresentation(Component component)
            : base(component)
        {
            if (MsdevMonitor == null)
            {
                MsdevMonitor = new MsdevManager.MsdevMonitorThread(true);
                MsdevMonitor.Changed += new MsdevManager.MsdevMonitorThread.MonitorMsdevHandler(MsdevMonitor_Changed);
                MsdevMonitor.Start();
            }

        }


        void MsdevMonitor_Changed()
        {
            _Solution = null;
            _Project = null;
            if (ObjectChangeState != null)
                ObjectChangeState(this, null);
        }
        ~ComponentPresentation()
        {

        }
 
        public override void FormClosed()
        {
            MsdevMonitor.Changed -= new MsdevManager.MsdevMonitorThread.MonitorMsdevHandler(MsdevMonitor_Changed);
            base.FormClosed();

        }
        public System.Collections.Generic.List<EnvDTE.Solution> Solutions
        {
            get
            {
                try
                {
                    return MsdevManager.Msdev.GetOpenSolutions();
                }
                catch(System.Exception error)
                {
                    throw;
                }

            }

        }
        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;

        EnvDTE.Solution _Solution;
        public EnvDTE.Solution Solution
        {
            get
            {
                if (_Solution == null&&!string.IsNullOrEmpty(SolutionFileName))
                {
                    foreach (EnvDTE.Solution solution in Solutions)
                    {
                        try
                        {
                            if (SolutionFileName == solution.FileName)
                                _Solution = solution;
                        }
                        catch (Exception error)
                        {
                        }
                    }
                }
                return _Solution;
            }
            set
            {

                if (_Solution != value)
                {
                    _Solution = value;
                    if (_Solution != null)
                        RealObject.Solution = _Solution.FileName;
                    else
                        RealObject.Solution = "";

                    _Project = null;
                    RealObject.Project = "";

                    if (ObjectChangeState != null)
                        ObjectChangeState(this, null);


                }

            }
        } 

        public void OpenSolution()
        {
            if (System.IO.File.Exists(SolutionFileName) && Solution == null)
            {
                if (System.IO.File.Exists(SolutionFileName) && Solution == null)
                {
                    EnvDTE.DTE DTE = MsdevManager.Msdev.GetIDEInstance(SolutionFileName);
                    if (ObjectChangeState != null)
                        ObjectChangeState(this, null);
                }

            }

        }

        EnvDTE.Project _Project;
        public EnvDTE.Project Project
        {
            get
            {
                if (_Project==null&& Solution != null && !string.IsNullOrEmpty(ProjectFileName))
                {
                    foreach (EnvDTE.Project project in Projects)
                    {
                        try
                        {
                            if (ProjectFileName == project.FileName)
                                _Project = project;
                        }
                        catch (Exception error)
                        {
                        }
                    }
                }

                return _Project;
            }
            set
            {
                if (_Project != value)
                {
                    _Project = value;
                    try
                    {
                        if (_Project != null)
                            RealObject.Project = _Project.FileName;
                        if (ObjectChangeState != null)
                            ObjectChangeState(this, "ProjectFileName");
                    }
                    catch (System.Exception error)
                    {

                    }

                }

            }
        }

        public string SolutionFileName
        {
            get
            {
                return RealObject.Solution;
            }
            set
            {

            }
        }
        public string ProjectFileName
        {
            get
            {
                return RealObject.Project;
            }
            set
            {

            }
        }
        public System.Collections.Generic.List<EnvDTE.Project> Projects
        {
            get
            {
                try
                {

                    System.Collections.Generic.List<EnvDTE.Project> _Projects = new List<EnvDTE.Project>();

                    if (_Solution != null)
                    {
                        foreach (EnvDTE.Project project in _Solution.Projects)
                            _Projects.Add(project);
                    }
                    return _Projects;
                }
                catch (Exception err0r)
                {
                    throw;
                }

            }
        }



    }
}
