using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.CodeMetaDataRepository
{
    /// <MetaDataID>{2c9d36d2-c167-44bb-b16b-395ffa200e3a}</MetaDataID>
    public class Solution:MetaDataRepository.MetaObject,OOAdvantech.Remoting.IExtMarshalByRefObject
    {
        public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
        {
            return new System.Collections.Generic.List<object>();
        }
        internal EnvDTE.Solution _VSSolution;
            

        /// <MetaDataID>{7355f242-a855-42d7-99c7-cb4ce570f4e8}</MetaDataID>
        internal EnvDTE.Solution VSSolution
        {
            get
            {

                return _VSSolution;
            }
            set
            {
                _VSSolution=value;
            }
        }
        string SolutionFullName;
        /// <MetaDataID>{f8562320-936e-4cd3-ba95-371b124b4d3c}</MetaDataID>
        public Solution(EnvDTE.Solution solution)
        {
            MetaObjectMapper.Clear();
            _VSSolution = solution;
            _VSSolution.DTE.Events.SolutionEvents.AfterClosing += new EnvDTE._dispSolutionEvents_AfterClosingEventHandler(OnAfterClosing);

            SolutionFullName = solution.FullName;
            if (_VSSolution != null)
            {
                _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID( VSSolution.FileName);
                if (System.IO.File.Exists(VSSolution.FileName))
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(VSSolution.FileName);
                    _Name = fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length);
                }
                
                
            }
            MetaObjectMapper.AddTypeMap(VSSolution, this);
        }

        /// <MetaDataID>{df12600a-d878-43b7-a404-9f9f2405e733}</MetaDataID>
        void OnAfterClosing()
        {
            MetaObjectMapper.Clear();
            //VSSolution.DTE.Events.SolutionEvents.AfterClosing -= new EnvDTE._dispSolutionEvents_AfterClosingEventHandler(OnAfterClosing);

            _Projects = null;
        }
        public override string Name
        {
            get
            {
                if (VSSolution != null)
                {
                    if (System.IO.File.Exists(VSSolution.FileName))
                    {
                        System.IO.FileInfo fileInfo = new System.IO.FileInfo(VSSolution.FileName);
                        _Name = fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length);
                    }
                    else
                        _Name = "Solution";

                }
                return base.Name;
            }
            set
            {
                base.Name = value;
            }
        }
        int SolutionProjectNum;
        /// <MetaDataID>{d7ed16b4-669f-48d7-b88a-4d5527b1e756}</MetaDataID>
        Collections.Generic.Set<Project> _Projects;
        /// <MetaDataID>{bd407480-225f-4093-9fa2-eec5a80f69a0}</MetaDataID>
        public Collections.Generic.Set<Project> Projects
        {
            get
            {
                if (VSSolution != null && SolutionFullName == VSSolution.FullName)
                {
                    if (_Projects != null)
                    {
                        if (VSSolution.Projects.Count == SolutionProjectNum)
                            return _Projects;
                    }
                }
                if (VSSolution != null)
                {
                    if(_Projects!=null)
                        _Projects.Clear();
                    SolutionFullName = VSSolution.FullName;
                            SolutionProjectNum = VSSolution.Projects.Count;
                    foreach (EnvDTE.Project vsProject in VSSolution.Projects)
                    {
                        try
                        { 
                            string fileName = vsProject.FileName;
                            if (vsProject.Properties != null)
                            {
                                Project project = MetaObjectMapper.FindMetaObjectFor(vsProject) as Project;

                                if (project == null)
                                    project = new Project(vsProject);
                                if (project.Laguage == ProjectLanguage.CSharp)
                                {

                                    if (_Projects == null)
                                        _Projects = new OOAdvantech.Collections.Generic.Set<Project>();
                                    _Projects.Add(project);
                                }
                            }
                            

                        }
                        catch (Exception error)
                        {
                        }
                     
                    }
                }
                if (_Projects == null)
                    return new OOAdvantech.Collections.Generic.Set<Project>();
                return _Projects;
            }
        }
    }
}
