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
using ConnectableControls.PropertyEditors;

using ConnectableControls.List.Editors;
using ConnectableControls.List.Models.Design;
using ConnectableControls.List.Renderers;
using ConnectableControls.List.Sorting;
using ConnectableControls;
using OOAdvantech.UserInterface.Runtime;
using ConnectableControls.List.Events;


namespace ConnectableControls.List.Models
{
    /// <summary>
    /// Represents a Column whose Cells are displayed as a ComboBox
    /// </summary>
    /// <MetaDataID>{7544AACE-5985-4AFA-BED7-94D7A26D906D}</MetaDataID>
    [DesignTimeVisible(false),
    ToolboxItem(false)]
    public class ComboBoxColumn : DropDownColumn
    {
        #region Constructor

        /// <summary>
        /// Creates a new ComboBoxColumn with default values
        /// </summary>
        /// <MetaDataID>{C50CB237-E891-4FF4-9763-262DF147E36A}</MetaDataID>
        public ComboBoxColumn()
            : base()
        {
            //_Type = ColumnType.ComboBoxColumn;

        }
        /// <MetaDataID>{10B55590-B18C-48A8-BB7F-FC0BCBC6426A}</MetaDataID>
        public ComboBoxColumn(Column copyColumn)
            : base(copyColumn)
        {
        }
        /// <MetaDataID>{e0fb327f-f126-4301-bb86-7b3b739029d1}</MetaDataID>
        public ComboBoxColumn(OOAdvantech.UserInterface.ComboBoxColumn copyColumn)
            : base(copyColumn)
        {
        }



        //OOAdvantech.MetaDataRepository.Operation _SearchOperation;
        //OOAdvantech.MetaDataRepository.Operation SearchOperation
        //{
        //    get
        //    {
        //        if (_SearchOperation != null)
        //            return _SearchOperation;
        //        if ((ColumnMetaData as OOAdvantech.UserInterface.SearchBoxColumn) == null || (ColumnMetaData as OOAdvantech.UserInterface.SearchBoxColumn).SearchOperation == null)
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


        /// <MetaDataID>{63b7c9ad-3b42-48ed-b4dc-297aa76b686a}</MetaDataID>
        OOAdvantech.UserInterface.OperationCall Operation;

        /// <MetaDataID>{3afcb7d4-6b9a-4d3f-a44e-d6e4519b594d}</MetaDataID>
        object _OperationCall;
        /// <MetaDataID>{a9bc9cd0-97e1-420a-84a4-be0ebf897a61}</MetaDataID>
        [Editor(typeof(EditOperationCallMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        public object OperationCall
        {
            get
            {
                if ((ColumnMetaData as OOAdvantech.UserInterface.ComboBoxColumn).SearchOperation == null)
                {
                    OOAdvantech.PersistenceLayer.ObjectStorage storage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(ColumnMetaData);
                    (ColumnMetaData as OOAdvantech.UserInterface.ComboBoxColumn).SearchOperation = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;
                }
                string xml = null;
                UserInterfaceMetaData.MetaDataValue metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, null);
                metaDataVaue.MetaDataAsObject = (ColumnMetaData as OOAdvantech.UserInterface.ComboBoxColumn).SearchOperation;
                return metaDataVaue;
            }
            set
            {
                // _SearchOperation = null;
                return;
            }
        }


        /// <exclude>Excluded</exclude>
        private bool _AutoInsert;
        [Category("Object Model Connection")]
        public bool AutoInsert
        {
            get
            {
               

                if (this.ColumnMetaData is OOAdvantech.UserInterface.ComboBoxColumn)
                    _AutoInsert=(this.ColumnMetaData as OOAdvantech.UserInterface.ComboBoxColumn).AutoInsert;

                return _AutoInsert;
                
            }

            set
            {

                if (_AutoInsert != value)
                {
                    _AutoInsert = value;
                    if (this.ColumnMetaData is OOAdvantech.UserInterface.ComboBoxColumn)
                        (this.ColumnMetaData as OOAdvantech.UserInterface.ComboBoxColumn).AutoInsert = value; ;
                }
            }
        }
        /// <MetaDataID>{b41966ca-82aa-4813-8638-188c6274c616}</MetaDataID>
        public override string[] PropertiesNames
        {
            get
            {
                System.Collections.Generic.List<string> propertiesNames = new System.Collections.Generic.List<string>(base.PropertiesNames);
                propertiesNames.Add("Text");
                return propertiesNames.ToArray();
            }
        }
        /// <MetaDataID>{968620c2-87b2-46a7-8f4c-fe380942ed43}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.Classifier GetPropertyType(string propertyName)
        {
            if (propertyName == "Text")
                return OOAdvantech.MetaDataRepository.Classifier.GetClassifier(typeof(string));

            return base.GetPropertyType(propertyName);
        }
        /// <MetaDataID>{218350d0-3f53-48c9-abca-a4bb7c703fe0}</MetaDataID>
        public override bool CanItAccept(OOAdvantech.MetaDataRepository.MetaObject metaObject, string propertyDescriptor)
        {
      
            if (metaObject is OOAdvantech.MetaDataRepository.Attribute)
                return true;
            else if (propertyDescriptor == "Operation" && metaObject is OOAdvantech.MetaDataRepository.Operation)
                return true;
            else if (propertyDescriptor == "OperationCall" && metaObject is OOAdvantech.UserInterface.OperationCall)
            {
                if (new OperationCaller(metaObject as OOAdvantech.UserInterface.OperationCall, this).Operation != null)
                    return true;
                else
                    return false;
            }
            else if (propertyDescriptor == "InsertOperationCall" && metaObject is OOAdvantech.UserInterface.OperationCall)
            {
                if (new OperationCaller(metaObject as OOAdvantech.UserInterface.OperationCall, this).Operation != null)
                    return true;
                else
                    return false;
            }
            else
                return false;

        
        }
        /// <exclude>Excluded</exclude>
        object _InsertOperationCall;
        /// <MetaDataID>{5c02c9eb-5c03-4e96-afbb-f3a8ae991750}</MetaDataID>
        [Editor(typeof(EditOperationCallMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        public object InsertOperationCall
        {
            get
            {
                if ((ColumnMetaData as OOAdvantech.UserInterface.ComboBoxColumn).InsertOperation == null)
                {
                    OOAdvantech.PersistenceLayer.ObjectStorage storage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(ColumnMetaData);
                    (ColumnMetaData as OOAdvantech.UserInterface.ComboBoxColumn).InsertOperation = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;
                }
                string xml = null;
                UserInterfaceMetaData.MetaDataValue metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, null);
                metaDataVaue.MetaDataAsObject = (ColumnMetaData as OOAdvantech.UserInterface.ComboBoxColumn).InsertOperation;
                return metaDataVaue;
            }
            set
            {
                // _SearchOperation = null;
                return;
            }
        }






        /// <summary>
        /// Creates a new ComboBoxColumn with the specified header text
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <MetaDataID>{882766EF-B0FA-4D1A-AEB3-9A0F51F875BC}</MetaDataID>
        public ComboBoxColumn(string text)
            : base(text)
        {
            //_Type = ColumnType.ComboBoxColumn;

        }


        /// <summary>
        /// Creates a new ComboBoxColumn with the specified header text and width
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="width">The column's width</param>
        /// <MetaDataID>{EBB712B0-73EF-4B1C-B15F-94802703ADA4}</MetaDataID>
        public ComboBoxColumn(string text, int width)
            : base(text, width)
        {
            //_Type = ColumnType.ComboBoxColumn;

        }


        /// <summary>
        /// Creates a new ComboBoxColumn with the specified header text, width and visibility
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="width">The column's width</param>
        /// <param name="visible">Specifies whether the column is visible</param>
        /// <MetaDataID>{15A32D2D-6C3A-405A-A2FB-2C1AC47BADB8}</MetaDataID>
        public ComboBoxColumn(string text, int width, bool visible)
            : base(text, width, visible)
        {
            //_Type = ColumnType.ComboBoxColumn;

        }


        /// <summary>
        /// Creates a new ComboBoxColumn with the specified header text and image
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="image">The image displayed on the column's header</param>
        /// <MetaDataID>{9453AECC-4374-485C-B4E4-8C8CF4E0EF47}</MetaDataID>
        public ComboBoxColumn(string text, Image image)
            : base(text, image)
        {
            //_Type = ColumnType.ComboBoxColumn;

        }


        /// <summary>
        /// Creates a new ComboBoxColumn with the specified header text, image and width
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="image">The image displayed on the column's header</param>
        /// <param name="width">The column's width</param>
        /// <MetaDataID>{DE151F67-7775-4F3F-8D59-C77E736696FA}</MetaDataID>
        public ComboBoxColumn(string text, Image image, int width)
            : base(text, image, width)
        {
            //_Type = ColumnType.ComboBoxColumn;

        }


        /// <summary>
        /// Creates a new ComboBoxColumn with the specified header text, image, width and visibility
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="image">The image displayed on the column's header</param>
        /// <param name="width">The column's width</param>
        /// <param name="visible">Specifies whether the column is visible</param>
        /// <MetaDataID>{A8004C3F-F1F2-433E-AEA3-E69DD043D0FF}</MetaDataID>
        public ComboBoxColumn(string text, Image image, int width, bool visible)
            : base(text, image, width, visible)
        {
            //_Type = ColumnType.ComboBoxColumn;

        }

        #endregion


        #region Methods

        /// <summary>
        /// Gets a string that specifies the name of the Column's default CellRenderer
        /// </summary>
        /// <returns>A string that specifies the name of the Column's default 
        /// CellRenderer</returns>
        /// <MetaDataID>{AC0BAF68-1276-487C-A0DE-5F1846ADD3C1}</MetaDataID>
        public override string GetDefaultRendererName()
        {
            return "COMBOBOX";
        }


        /// <summary>
        /// Gets the Column's default CellRenderer
        /// </summary>
        /// <returns>The Column's default CellRenderer</returns>
        /// <MetaDataID>{46D5B5C2-E1F0-4297-99DA-C601313A206F}</MetaDataID>
        public override ICellRenderer CreateDefaultRenderer()
        {
            return new ComboBoxCellRenderer();
        }


        /// <summary>
        /// Gets a string that specifies the name of the Column's default CellEditor
        /// </summary>
        /// <returns>A string that specifies the name of the Column's default 
        /// CellEditor</returns>
        /// <MetaDataID>{111345B5-83E1-4885-B63E-DF9F58B0BC33}</MetaDataID>
        public override string GetDefaultEditorName()
        {
            return "COMBOBOX";
        }


        /// <summary>
        /// Gets the Column's default CellEditor
        /// </summary>
        /// <returns>The Column's default CellEditor</returns>
        /// <MetaDataID>{84B9F78C-59E9-4A88-BED6-F0DAE8F267B1}</MetaDataID>
        public override ICellEditor CreateDefaultEditor()
        {
            //return new ComboBoxCellEditor();
            Editor = new ComboBoxCellEditor(); ;
            return Editor;

        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets the Type of the Comparer used to compare the Column's Cells when 
        /// the Column is sorting
        /// </summary>
        /// <MetaDataID>{db4134c1-03ba-4620-bd33-25d930c8fe4f}</MetaDataID>
        public override Type DefaultComparerType
        {
            get
            {
                return typeof(TextComparer);
            }
        }

        #endregion
    }
}
