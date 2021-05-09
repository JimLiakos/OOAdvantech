using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Microsoft.VisualStudio.Shell.Interop;
using System.Runtime.InteropServices;

namespace VisualStudioEventBridge
{
    public delegate void SolutionOpenedEventHandler();
    public delegate void SolutionClosedEventHandler();
    public delegate void CodeElementAddedEventHandler(EnvDTE.CodeElement element);
    public delegate void CodeElementChangedEventHandler(EnvDTE.CodeElement element, EnvDTE80.vsCMChangeKind change);
    public delegate void CodeElementDeletedEventHandler(object parent, EnvDTE.CodeElement element);
    public delegate void LineChangedEventHandler(EnvDTE.TextPoint StartPoint, EnvDTE.TextPoint EndPoint, int Hint);
    public delegate void AfterCommandExecuteEventHandler(string Guid, int ID, object CustomIn, object CustomOut);

    public delegate void ProjectAddedEventHandler(EnvDTE.Project Project);
    public delegate void ProjectRemovedEventHandler(EnvDTE.Project Project);
    public delegate void ProjectRenamedEventHandler(EnvDTE.Project Project, string OldName);
     
    public delegate void ProjectItemAddedEventHandler(EnvDTE.ProjectItem projectItem);
    public delegate void ProjectItemRemovedEventHandler(EnvDTE.ProjectItem projectItem);
    public delegate void ProjectItemRenamedEventHandler(EnvDTE.ProjectItem ProjectItem, string OldName);

    public delegate void AfterKeyPressEventHandler(string Keypress, EnvDTE.TextSelection Selection, bool InStatementCompletion);
    public delegate void BeforeKeyPressEventHandler(string Keypress, EnvDTE.TextSelection Selection, bool InStatementCompletion, ref bool CancelKeypress);

    /// <MetaDataID>{dba51a07-4756-4542-95e0-9b444553a64f}</MetaDataID>
    public class VisualStudioEvents
    {  
        public static object DTEObject;
        public static event SolutionOpenedEventHandler SolutionOpened;
        public static event SolutionClosedEventHandler SolutionClosed;
        public static event CodeElementAddedEventHandler CodeElementAdded;
        public static event CodeElementChangedEventHandler CodeElementChanged;
        public static event CodeElementDeletedEventHandler CodeElementDeleted;
        public static event LineChangedEventHandler LineChanged;
        public static event AfterCommandExecuteEventHandler AfterCommandExecute;
        public static event ProjectAddedEventHandler ProjectAdded;
        public static event ProjectRemovedEventHandler ProjectRemoved;
        public static event ProjectRenamedEventHandler ProjectRenamed;
        public static event ProjectItemAddedEventHandler ProjectItemAdded;
        public static event ProjectItemRemovedEventHandler ProjectItemRemoved;
        public static event ProjectItemRenamedEventHandler ProjectItemRenamed;

        public static event AfterKeyPressEventHandler AfterKeyPress;
        public static event BeforeKeyPressEventHandler BeforeKeyPress;



        public static void OnSolutionOpened()
        {
            //return;
            try
            {
                if (SolutionOpened != null)
                    SolutionOpened();
            }
            catch (Exception error)
            {
            }
        }
        public static void OnSolutionClosed()
        {
           // return;
            try
            {
                if (SolutionClosed != null)
                    SolutionClosed();

            }
            catch (Exception error)
            {
            }
        }
        public static void OnCodeElementAdded(EnvDTE.CodeElement element)
        { 
            //return;

            try
            {
                


                if (CodeElementAdded != null)
                    CodeElementAdded(element);

            }
            catch (Exception error)
            {
            }
        }
        public static void OnCodeElementChanged(EnvDTE.CodeElement element, EnvDTE80.vsCMChangeKind change)
        {
            //return;
            try
            {
                
                if (CodeElementChanged != null)
                    CodeElementChanged(element, change);

            }
            catch (Exception error)
            {
            }
        }
        public static void OnCodeElementDeleted(object parent, EnvDTE.CodeElement element)
        {
            //return;
            try
            {
                if (CodeElementDeleted != null)
                    CodeElementDeleted(parent, element);

            }
            catch (Exception error)
            {
            }
        }
        public static bool SuspendLineChangedEvent;

        public static void OnLineChanged(EnvDTE.TextPoint StartPoint, EnvDTE.TextPoint EndPoint, int Hint)
        {
            //return;
            try
            {
                if (!SuspendLineChangedEvent)
                {
                    if (LineChanged != null)
                        LineChanged(StartPoint, EndPoint, Hint);
                }

            }
            catch (Exception error)
            {

                
            }
        }
        public static void OnAfterCommandExecute(string Guid, int ID, object CustomIn, object CustomOut)
        {
            //return;
            try
            {
                if (AfterCommandExecute != null)
                    AfterCommandExecute(Guid, ID, CustomIn, CustomOut);

            }
            catch (Exception error)
            {
                
            }
        }
        public static void OnProjectAdded(EnvDTE.Project Project)
        {
           // return;
            try
            {
                if (ProjectAdded != null)
                    ProjectAdded(Project);
            }
            catch (Exception error)
            {
            }

        }
        public static void OnProjectRemoved(EnvDTE.Project Project)
        {
            //return;
            try
            {
                if (ProjectRemoved != null)
                    ProjectRemoved(Project);

            }
            catch (Exception error)
            {
            }
        }
        public static void OnProjectRenamed(EnvDTE.Project Project, string OldName)
        {
            //return;
            try
            {
                if (ProjectRenamed != null)
                    ProjectRenamed(Project, OldName);

            }
            catch (Exception error)
            {
            }
        }

        public static void OnProjectItemAdded(EnvDTE.ProjectItem projectItem)
        {
            //return;
            try
            {
                if (ProjectItemAdded != null)
                    ProjectItemAdded(projectItem);


            }
            catch (Exception error)
            {
            }
        }
        public static void OnProjectItemRemoved(EnvDTE.ProjectItem projectItem)
        {
            //return;
            try
            {
                if (ProjectItemRemoved != null)
                    ProjectItemRemoved(projectItem);

            }
            catch (Exception error)
            {
            }
        }
        public static void OnProjectItemRenamed(EnvDTE.ProjectItem ProjectItem, string OldName)
        {
          //  return;
            try
            {
                if (ProjectItemRenamed != null)
                    ProjectItemRenamed(ProjectItem, OldName);

            }
            catch (Exception error)
            {
            }
        }

        public static void OnAfterKeyPress(string Keypress, EnvDTE.TextSelection Selection, bool InStatementCompletion)
        {
            //return;
            try
            {
                if (AfterKeyPress != null)
                    AfterKeyPress(Keypress, Selection, InStatementCompletion);

            }
            catch (Exception error)
            {
            }
        }
        public static void OnBeforeKeyPress(string Keypress, EnvDTE.TextSelection Selection, bool InStatementCompletion, ref bool CancelKeypress)
        {
            //return;
            try
            {
                if (BeforeKeyPress != null)
                    BeforeKeyPress(Keypress, Selection, InStatementCompletion, ref CancelKeypress);

            }
            catch (Exception error)
            {
            }
        }
    }



   

//        vsServiceProvider = serviceProvider;




    //static public class VisualStudioBridge
    //{
    //    // Microsoft.VisualStudio.OLE.Interop.IServiceProvider 

    //    public static Microsoft.VisualStudio.Shell.Interop.IVsSolution GetIVsSolution(EnvDTE.DTE dte)
    //    {
            
    //        Microsoft.VisualStudio.OLE.Interop.IServiceProvider vsServiceProvider = dte as Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

    //        Microsoft.VisualStudio.Shell.ServiceProvider sp = new Microsoft.VisualStudio.Shell.ServiceProvider((Microsoft.VisualStudio.OLE.Interop.IServiceProvider)dte);

            
    //        Guid serviceGuid = typeof(SVsSolution).GUID;
    //        //Guid interfaceGuid = typeof(IVsSolution).GUID;
    //        //IntPtr ppObj;
    //        //vsServiceProvider.QueryService(ref serviceGuid, ref interfaceGuid, out ppObj);
    //        //IVsSolution solution = Marshal.GetObjectForIUnknown(ppObj) as IVsSolution;
    //        IVsSolution solution = sp.GetService(serviceGuid) as IVsSolution;
    //        return solution;
    //    }
    //}

}


//namespace orfm
//{
//    public class messa
//    {

//    }
//}
