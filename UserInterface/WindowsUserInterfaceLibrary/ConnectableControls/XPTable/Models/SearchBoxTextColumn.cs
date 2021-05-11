/*
 * Copyright © 2005, Mathew Hall
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without modification, 
 * are permitted provided that the following conditions are met:
 *
 *    - Redistributions of source code must retain the above copyright notice, 
 *      this list of conditions and the following disclaimer.
 * 
 *    - Redistributions in binary form must reproduce the above copyright notice, 
 *      this list of conditions and the following disclaimer in the documentation 
 *      and/or other materials provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
 * IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
 * INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT 
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, 
 * OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
 * OF SUCH DAMAGE.
 */


using System;
using System.ComponentModel;
using System.Drawing;

using ConnectableControls.List.Editors;
using ConnectableControls.List.Events;
using ConnectableControls.List.Models.Design;
using ConnectableControls.List.Renderers;
using ConnectableControls.List.Sorting;
using System.Windows.Forms;
using System.Collections.Generic;
using ConnectableControls;
using ConnectableControls.PropertyEditors;



namespace ConnectableControls.List.Models
{
    /// <summary>
    /// Summary description for TextColumn.
    /// </summary>
    /// <MetaDataID>{D819BA32-F3F7-4AB0-8B5B-D00ADF868E8A}</MetaDataID>
	[DesignTimeVisible(false),
	ToolboxItem(false)]
    public class SearchBoxColumn : Column
	{
        Dictionary<string, Control> ControlsAsSource = new Dictionary<string, Control>();

        OOAdvantech.UserInterface.Runtime.OperationCaller _OperationCaller;
        OOAdvantech.UserInterface.Runtime.OperationCaller OperationCaller
        {
            get
            {
                if ((ColumnMetaData as OOAdvantech.UserInterface.SearchBoxColumn).SearchOperation  == null || UserInterfaceObjectConnection == null)
                    return null;

                if (_OperationCaller != null)
                    return _OperationCaller;
                _OperationCaller = new OOAdvantech.UserInterface.Runtime.OperationCaller((ColumnMetaData as OOAdvantech.UserInterface.SearchBoxColumn).SearchOperation , this);
                return _OperationCaller;


            }

        }




        object _OperationCall;
        [Editor(typeof(EditOperationCallMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        public object OperationCall
        {
            get
            {
                if ((ColumnMetaData as OOAdvantech.UserInterface.SearchBoxColumn).SearchOperation == null)
                {
                    OOAdvantech.PersistenceLayer.ObjectStorage storage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(ColumnMetaData);
                    (ColumnMetaData as OOAdvantech.UserInterface.SearchBoxColumn).SearchOperation = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;
                }
                string xml = null;
                UserInterfaceMetaData.MetaDataValue metaDataVaue = null;
                if (OperationCaller == null || _OperationCaller.Operation==null)
                    metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, null);
                else
                    metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, _OperationCaller.Operation.Name);

                metaDataVaue.MetaDataAsObject = (ColumnMetaData as OOAdvantech.UserInterface.SearchBoxColumn).SearchOperation;
                return metaDataVaue;
            }
            set
            {
                _OperationCaller = null;
                return;
            }
        }


        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string[] PropertiesNames
        {
            get
            {
                return new string[2] { "Value", "Text" };
            }
        }
        public override object GetPropertyValue(string propertyName)
        {

            if (propertyName == "Text")
                return Text;
            //if (propertyName == "this.Text")
            //    return Text;

            if (propertyName == "this")
                return this;

            //if (propertyName == "this.Value")
            //    return Value;

            if (propertyName == "Value")
                return Value;
            throw new Exception("There isn't property with name " + propertyName + ".");

        }

        public override bool ContainsProperty(string propertyName)
        {
            if (propertyName == "this")
                return true;
            //if (propertyName == "this.Value")
            //    return true;

            if (propertyName == "Value")
                return true;

            //if (propertyName == "this.Text")
            //    return true;

            if (propertyName == "Text")
                return true;


            return false;
        }


		#region Constructor

        /// <summary>
        /// Creates a new TextColumn with default values
        /// </summary>
        /// <MetaDataID>{E5070A71-A740-40AE-8F57-45230B0852CF}</MetaDataID>
		public SearchBoxColumn() : base()
		{
            Intit();

		}
        /// <MetaDataID>{8B6F24B1-B403-4E49-8C8A-11530C328898}</MetaDataID>
        public SearchBoxColumn(Column copyColumn)
            : base(copyColumn)
        {
            Intit();

        }
        /// <MetaDataID>{51C25287-11FA-4B99-819F-89B8D677B475}</MetaDataID>
        public SearchBoxColumn(OOAdvantech.UserInterface.SearchBoxColumn copyColumn)
            : base(copyColumn)
        {
            Intit();

        }




        /// <MetaDataID>{B80AB81D-C546-46DC-9DFB-0F6B76C6110F}</MetaDataID>
        private void Intit()
        {
            //_Type = ColumnType.SearchBoxColumn;
        }


        /// <summary>
        /// Creates a new TextColumn with the specified header text
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <MetaDataID>{CFC4D73A-BF46-4A16-AB3A-66131F40EE8F}</MetaDataID>
		public SearchBoxColumn(string text) : base(text)
		{
            Intit();

		}


        /// <summary>
        /// Creates a new TextColumn with the specified header text and width
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="width">The column's width</param>
        /// <MetaDataID>{12FFE7D1-3629-4E6D-A761-47DB89F0184C}</MetaDataID>
		public SearchBoxColumn(string text, int width) : base(text, width)
		{
            Intit();
		}


        /// <summary>
        /// Creates a new TextColumn with the specified header text, width and visibility
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="width">The column's width</param>
        /// <param name="visible">Specifies whether the column is visible</param>
        /// <MetaDataID>{F21F99AD-2044-4797-A203-32277CE03945}</MetaDataID>
		public SearchBoxColumn(string text, int width, bool visible) : base(text, width, visible)
		{
            Intit();
		}


        /// <summary>
        /// Creates a new TextColumn with the specified header text and image
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="image">The image displayed on the column's header</param>
        /// <MetaDataID>{30DF95F1-E0E4-4B50-B0EC-68C8A051A896}</MetaDataID>
		public SearchBoxColumn(string text, Image image) : base(text, image)
		{
            Intit();
		}


        /// <summary>
        /// Creates a new TextColumn with the specified header text, image and width
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="image">The image displayed on the column's header</param>
        /// <param name="width">The column's width</param>
        /// <MetaDataID>{7142060D-2E80-4FE8-B68F-4BBB09733946}</MetaDataID>
		public SearchBoxColumn(string text, Image image, int width) : base(text, image, width)
		{
            Intit();
		}


        /// <summary>
        /// Creates a new TextColumn with the specified header text, image, width and visibility
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="image">The image displayed on the column's header</param>
        /// <param name="width">The column's width</param>
        /// <param name="visible">Specifies whether the column is visible</param>
        /// <MetaDataID>{4D253591-E275-40E5-9807-9D28155B0C65}</MetaDataID>
		public SearchBoxColumn(string text, Image image, int width, bool visible) : base(text, image, width, visible)
		{
            Intit();
		}

		#endregion


		#region Methods

        /// <summary>
        /// Gets a string that specifies the name of the Column's default CellRenderer
        /// </summary>
        /// <returns>A string that specifies the name of the Column's default 
        /// CellRenderer</returns>
        /// <MetaDataID>{FF483A1E-6D2B-42D3-A212-DFFBC1FA03C9}</MetaDataID>
		public override string GetDefaultRendererName()
		{
			return "TEXT";
		}


        /// <summary>
        /// Gets the Column's default CellRenderer
        /// </summary>
        /// <returns>The Column's default CellRenderer</returns>
        /// <MetaDataID>{BB080314-8672-4F00-B58D-A99DB850133D}</MetaDataID>
		public override ICellRenderer CreateDefaultRenderer()
		{
			return new SearchBoxCellRenderer();
		}


        /// <summary>
        /// Gets a string that specifies the name of the Column's default CellEditor
        /// </summary>
        /// <returns>A string that specifies the name of the Column's default 
        /// CellEditor</returns>
        /// <MetaDataID>{BB7DB5A7-2F8C-4089-9B97-0562DA0B1409}</MetaDataID>
		public override string GetDefaultEditorName()
		{
            return "SEARCHBOX";
		}


        /// <summary>
        /// Gets the Column's default CellEditor
        /// </summary>
        /// <returns>The Column's default CellEditor</returns>
        /// <MetaDataID>{1094FA96-D6A6-453C-9E08-3F47486FE937}</MetaDataID>
		public override ICellEditor CreateDefaultEditor()
		{
			return new SearchBoxCellEditor();
		}
        //internal override void Save(System.Xml.XmlElement columnElement)
        //{
        //    base.Save(columnElement);
        //    if (!string.IsNullOrEmpty(_OperationCall as string))
        //        columnElement.AppendChild(columnElement.OwnerDocument.CreateElement("OperationCall")).InnerXml = _OperationCall as string;
        //  }
        //internal override void Load(System.Xml.XmlElement columnElement)
        //{
        //    base.Load(columnElement);
        //    foreach (System.Xml.XmlNode operationCallNode in columnElement.ChildNodes)
        //    {
        //        if (operationCallNode.Name == "OperationCall")
        //        {
        //            _OperationCall = operationCallNode.InnerXml;
        //            columnElement.RemoveChild(operationCallNode);
        //            break;
        //        }
        //    }

        //}
		#endregion


		#region Properties

		/// <summary>
		/// Gets the Type of the Comparer used to compare the Column's Cells when 
		/// the Column is sorting
		/// </summary>
		public override Type DefaultComparerType
		{
			get
			{
				return typeof(TextComparer);
			}
		}

		#endregion

        #region IObjectMemberViewControl Members

        /// <MetaDataID>{E32CA00D-21C9-4784-92F5-F1054CC3C40A}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.Classifier GetClassifierFor(ITypeDescriptorContext context)
        {
            //if (context.PropertyDescriptor.Name == "DropDownListTextSource")
            //{
            //    System.Reflection.MethodInfo methodInfo = SearchMethod;
            //    if (methodInfo != null && methodInfo.ReturnType.IsGenericType)
            //    {
            //        System.Type type = methodInfo.ReturnType.GetGenericArguments()[0];
            //        return AssemblyManager.GetClassifier(type.FullName, true);
            //    }
            //    else
            //        return null;
            //}
            return base.GetClassifierFor(context);
        }




        //public override OOAdvantech.MetaDataRepository.Classifier ValueType
        //{
        //    get
        //    {
        //        if (ViewControlObject != null && !string.IsNullOrEmpty(_Path))
        //            return base.ValueType;

        //        OOAdvantech.MetaDataRepository.Operation searchOperation = this.col;
        //        if (searchOperation != null)
        //            return searchOperation.ReturnType.TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] as OOAdvantech.MetaDataRepository.Classifier;
        //        else
        //            return null;
        //    }

        //}

        //OOAdvantech.MetaDataRepository.Operation _SearchOperation;
        //OOAdvantech.MetaDataRepository.Operation SearchOperation
        //{
        //    get
        //    {
        //        if (_SearchOperation != null)
        //            return _SearchOperation;
        //        if ((ColumnMetaData as OOAdvantech.UserInterface.SearchBoxColumn) == null || (ColumnMetaData as OOAdvantech.UserInterface.SearchBoxColumn).SearchOperation==null)
        //            return null;
        //        Operation = (ColumnMetaData as OOAdvantech.UserInterface.SearchBoxColumn).SearchOperation;
                
        //        //string assemblyName = Operation.AssemblyFullName;
        //        string operationName = Operation.OperationPath;
        //        string[] parametersTypes = new string[Operation.ParameterLoaders.Count];
        //        int i = 0;
        //        foreach (OOAdvantech.UserInterface.ParameterLoader parameterLoader in Operation.ParameterLoaders)
        //            parametersTypes[i++] = parameterLoader.ParameterType;
        //        _SearchOperation = OperationCaller.GetOperation(operationName, parametersTypes);
        //        return _SearchOperation;



        //    }
        //}

        //System.Reflection.MethodInfo SearchMethod
        //{
        //    get
        //    {
        //        OOAdvantech.MetaDataRepository.Operation operation = SearchOperation;
        //        if (operation == null)
        //            return null;
        //        return operation.GetExtensionMetaObject(typeof(System.Reflection.MethodInfo)) as System.Reflection.MethodInfo;
        //    }
        //}




        /// <MetaDataID>{B980FF4E-2C53-46E6-9906-77EE0CB818AF}</MetaDataID>
        public override bool CanItAccept(OOAdvantech.MetaDataRepository.MetaObject metaObject, string propertyDescriptor)
        {
            if (propertyDescriptor == "Operation" && metaObject is OOAdvantech.MetaDataRepository.BehavioralFeature)
                return true;
            //else if (metaObject is OOAdvantech.MetaDataRepository.Feature || propertyDescriptor == "DropDownListTextSource")
            //    return true;
            if (metaObject is OOAdvantech.MetaDataRepository.Feature || propertyDescriptor == "DisplayMember")
                return true;


            return false;
        }

  

        #endregion
    }
}
