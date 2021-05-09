// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Apache License, Version 2.0. Please see http://www.apache.org/licenses/LICENSE-2.0.html

using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Presentation;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Uml;
using Microsoft.VisualStudio.Modeling.ExtensionEnablement;
//using Microsoft.VisualStudio.Uml.Classes;
using System.Collections.Generic;

namespace UmlElementLink
{

    /// <MetaDataID>UmlElementLink.UmlElementLinkCommand</MetaDataID>
  abstract partial class UmlElementLinkCommand
  {
      /// <summary>
      /// Tag that identifies our references.
      /// </summary>
      /// <MetaDataID>{36b712aa-85ee-4fa5-8413-797e7cbba919}</MetaDataID>
    private const string ModelLinkReferenceTag = "UmlModelLinkReferenceTag";

    #region Context
    /// <MetaDataID>{16774c7e-11f0-4f2f-a1be-1fd7c6d3b53b}</MetaDataID>
    [Import]
    protected IDiagramContext context { get; set; }

    /// <MetaDataID>{573c966b-e9b0-4577-ba9e-b12d243663e2}</MetaDataID>
    [Import]
    public Microsoft.VisualStudio.Shell.SVsServiceProvider ServiceProvider { get; set; }

    /// <MetaDataID>{554cb2c9-2c94-459d-8ad4-70c016d1ba0b}</MetaDataID>
    [Import]
    public ILinkedUndoContext LinkedUndoContext { get; set; }

    /// <MetaDataID>{b5aded2c-cfa2-41f5-b949-55c69c59a8a8}</MetaDataID>
    internal EnvDTE.DTE Dte
    {
      get
      {
        if (dte == null)
        {
          dte = ServiceProvider.GetService(typeof(EnvDTE.DTE)) as EnvDTE.DTE;
        }
        return dte;
      }
    }
    /// <MetaDataID>{f13b0442-ba29-4bc7-92d8-820d891e09b1}</MetaDataID>
    private EnvDTE.DTE dte = null;


    /// <summary>
    /// IElement of the single currently selected shape, or null.
    /// </summary>
    /// <MetaDataID>{84baf4b8-8eb3-4668-b3c8-8dc8e60c3228}</MetaDataID>
    protected Microsoft.VisualStudio.Uml.Classes.IElement CurrentElement
    {
      get
      {
        var diagram = CurrentDiagram;
        if (diagram == null) return null;
        if (diagram.SelectedShapes.Count() > 1 && !(diagram is ISequenceDiagram)) return null;
        IShape currentShape = diagram.SelectedShapes.FirstOrDefault();
        if (currentShape == null || currentShape is IDiagram) return null;
        return currentShape.GetElement();
      }
    }

    /// <MetaDataID>{b03ed41d-92fa-43fc-966e-4d3d311a4260}</MetaDataID>
    protected IDiagram CurrentDiagram
    {
      get { return context.CurrentDiagram; }
    }

    #endregion

    #region Manage references.
    /// <summary>
    /// Model reference attached to the element. Null if there is none.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    /// <MetaDataID>{f1adf9f8-f82a-4f92-a50b-3d099fc5b24a}</MetaDataID>
    protected static IEnumerable<IReference> GetReference(Microsoft.VisualStudio.Uml.Classes.IElement element)
    {
      return element == null ? null : element.GetReferences(ModelLinkReferenceTag);
    }


    /// <summary>
    /// True if there is a model reference attached to the element.
    /// False if the element is null.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    /// <MetaDataID>{0c8ca625-1ff8-4254-a8e8-36ff963da4f3}</MetaDataID>
    public static bool HasReference(Microsoft.VisualStudio.Uml.Classes.IElement element)
    {
      return element != null && GetReference(element).Count() > 0;
    }

    /// <summary>
    /// Set the reference on an element to point to a UML diagram file.
    /// </summary>
    /// <param name="element">source element</param>
    /// <param name="fullFilePath">UML diagram file path</param>
    /// <MetaDataID>{7d9c3b6d-c1d1-43c6-9321-e03d4b5513a2}</MetaDataID>
    protected static void SetReference(Microsoft.VisualStudio.Uml.Classes.IElement element, string fullFilePath)
    {
      element.AddReference(ModelLinkReferenceTag, fullFilePath, true);
    }
    #endregion

    #region Convert between relative and absolute file paths.
    /// <summary>
    /// File path relative to the current diagram.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    /// <MetaDataID>{b914099e-1469-4574-8a42-27cea58b8617}</MetaDataID>
    protected string RelativeFilePath(string filePath)
    {
      // Current diagram path:
      string localFileName = CurrentDiagram.FileName;
      if (string.IsNullOrEmpty(localFileName)) return filePath;
      return RelativePath(localFileName.ToLowerInvariant(), filePath.ToLowerInvariant());
    }

    /// <summary>
    /// Target path relative to the source.
    /// </summary>
    /// <param name="source">Lowercase local absolute file path</param>
    /// <param name="target">Lowercase local absolute file path</param>
    /// <returns></returns>
    /// <MetaDataID>{0537bf22-2d63-4c77-b094-a9d616ce5071}</MetaDataID>
    protected static string RelativePath(string source, string target)
    {
      // Absolute and local?
      if (target.Length < 3 || target[1] != ':')
      {
        // Already relative; or
        // Network filename: \\machine\...
        return target;
      }
      // Same volume?
      if (source.Substring(0, 3).ToLowerInvariant() != target.Substring(0, 3).ToLowerInvariant())
      {
        // On a different disk  C:\... != D:\...
        return target;
      }
      return RelativePath(source.Split('\\'), target.Split('\\'), 0, 0);
    }

    /// <summary>
    /// The target path relative to the source.
    /// </summary>
    /// <param name="source">Source path as an array of bits</param>
    /// <param name="target">Target path as an array of bits</param>
    /// <param name="sourceIx">Source element before which all items are equal</param>
    /// <param name="targetIx">Target element before which all items are equal</param>
    /// <returns></returns>
    /// <MetaDataID>{7c0f704d-74cb-43b3-a9ff-9f644564bf1e}</MetaDataID>
    protected static string RelativePath(string[] source, string[] target, int sourceIx, int targetIx)
    {
      if (targetIx == target.Length) return target[target.Length - 1];
      if (sourceIx == targetIx && source[sourceIx].Equals(target[targetIx], StringComparison.InvariantCultureIgnoreCase))
        return RelativePath(source, target, sourceIx + 1, targetIx + 1);
      if (sourceIx + 1 < source.Length)
        return "..\\" + RelativePath(source, target, sourceIx + 1, targetIx);
      else
        return string.Join("\\", target, targetIx, target.Length - targetIx);
    }

    /// <summary>
    /// Get an absolute file path from a model reference: {relative path}[#{element id}]
    /// </summary>
    /// <param name="umlModelReference">Path relative to the current file, or an absolute path, or a 
    /// reference in which '#' terminates the path.</param>
    /// <returns></returns>
    /// <MetaDataID>{f62fda80-1eb0-4291-b01f-6da9fdaf8bc6}</MetaDataID>
    protected string AbsoluteFilePath(string umlModelReference)
    {
      string filePath = umlModelReference.Split(new char[] { '|', '#' }, 2)[0];
      if (filePath.IndexOf("://") > 0 || filePath.IndexOf("\\\\")==0)
      {
        return filePath;
      }
      else
      {
        return Path.GetFullPath(Path.Combine(Path.GetDirectoryName(CurrentDiagram.FileName), filePath));
      }
    }
    #endregion

    #region Resolve references

    /// <MetaDataID>{3e63fc75-e011-45a1-8a76-69ef583ff90e}</MetaDataID>
    protected IEnumerable<string> GetReferenceValues(Microsoft.VisualStudio.Uml.Classes.IElement element)
    {
      IEnumerable<IReference> references = GetReference(element);
      if (references == null) return null;
      return references.Select(r => r.Value);
    }


    /// <summary>
    /// Display a form that lets the user choose an item.
    /// </summary>
    /// <param name="items"></param>
    /// <returns>Null if the user closed the dialog</returns>
    /// <MetaDataID>{daacd599-fd09-4b9d-b03a-2fed920b391d}</MetaDataID>
    protected string ChooseOne(IEnumerable<string> items)
    {
      if (items == null || items.Count() == 0) return null;
      if (items.Count() == 1) return items.First();
      using (ChooseReferenceForm form = new ChooseReferenceForm(items))
      {
        if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
          return form.Selection;
        }
        return null;
      }
    }

    #endregion
  }

}
