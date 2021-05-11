namespace ConnectableControls
{
    using Microsoft.VisualBasic;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Data;
    using System.Xml.Serialization;

    /// <MetaDataID>{2a0043d5-b909-41d8-a3da-c702017bdb12}</MetaDataID>
    [Serializable()]
    [ XmlRootAttribute("CustomProperty")]
    public class CustomProperty
    {				
		#region "Private variables"
		
		// Common properties
        /// <MetaDataID>{d8dbc687-b8ef-46d1-8689-0e4dce147ee3}</MetaDataID>
		protected string sName = "";
        /// <MetaDataID>{488a524b-f96e-41b3-a2a5-fd3145a074a3}</MetaDataID>
		protected object oValue = null;
        /// <MetaDataID>{ee0c13ad-e1b7-4bc9-9f84-826429d37f1b}</MetaDataID>
		protected bool bIsReadOnly = false;
        /// <MetaDataID>{73d00def-8c94-40d7-afae-94b10411dc98}</MetaDataID>
		protected bool bVisible = true;
        /// <MetaDataID>{fc05c9b0-5bb4-4c35-8079-4542095b0750}</MetaDataID>
		protected string sDescription = "";
        /// <MetaDataID>{8c2075c8-1687-42d9-ad91-473d14d339e3}</MetaDataID>
		protected string sCategory = "";
        /// <MetaDataID>{5ff3c105-ef29-4268-9bb2-8ca830881287}</MetaDataID>
		protected bool bIsPassword = false;
        /// <MetaDataID>{c56b1fe3-e1a9-42be-a283-1d4684c2a8a1}</MetaDataID>
		protected bool bIsPercentage = false;
        /// <MetaDataID>{6eea2dd7-a8f7-47f1-a8f0-4ba3d8f421d7}</MetaDataID>
		protected bool bParenthesize = false;
		
		// Filename editor properties
        /// <MetaDataID>{baaef32a-3968-4161-826b-48985669f2f2}</MetaDataID>
		protected string sFilter = null;
        /// <MetaDataID>{f37e0116-d106-4d21-9d3e-3a13e2dd7984}</MetaDataID>
		protected UIFilenameEditor.FileDialogType eDialogType = UIFilenameEditor.FileDialogType.LoadFileDialog;
        /// <MetaDataID>{1bd4668f-36ef-41dc-9d11-cfcd7294eef6}</MetaDataID>
		protected bool bUseFileNameEditor = false;
		
		// Custom choices properties
        /// <MetaDataID>{a6daaff2-6ece-4874-9cfa-fa969ee10341}</MetaDataID>
		protected CustomChoices oChoices = null;
		
		// Browsable properties
        /// <MetaDataID>{cd25a8ef-7eb0-46cc-8d2b-4310fe649b8d}</MetaDataID>
		protected bool bIsBrowsable = false;
        /// <MetaDataID>{b50d6d14-bef0-424b-8455-1461016af38a}</MetaDataID>
		protected BrowsableTypeConverter.LabelStyle eBrowsablePropertyLabel = BrowsableTypeConverter.LabelStyle.lsEllipsis;
		
		// Dynamic properties
        /// <MetaDataID>{8b72c798-576e-40c3-8a9f-9f9d5f932498}</MetaDataID>
		protected bool bRef = false;
        /// <MetaDataID>{e64a74db-3420-4e8d-9d41-2a5081b68efe}</MetaDataID>
		protected object oRef = null;
        /// <MetaDataID>{82e084e9-e8e1-46a0-a068-094b5a29ae5f}</MetaDataID>
		protected string sProp = "";
		
		// Databinding properties
        /// <MetaDataID>{a2afb043-9b1e-4337-bfd0-24ac9d6a79de}</MetaDataID>
		protected object oDatasource = null;
        /// <MetaDataID>{9b34e9d6-102f-4be7-9c58-d2a99b804537}</MetaDataID>
		protected string sDisplayMember = null;
        /// <MetaDataID>{ae2b73c5-9863-41e1-946d-b11c1effdadd}</MetaDataID>
		protected string sValueMember = null;
        /// <MetaDataID>{0954a15f-a06a-404c-bef2-35914ed2748f}</MetaDataID>
		protected object oSelectedValue = null;
        /// <MetaDataID>{8dae8544-b9d0-451e-8c22-0863f23f8c4f}</MetaDataID>
		protected object oSelectedItem = null;
        /// <MetaDataID>{1e5738f0-7b98-4406-ac78-9f232edb6d82}</MetaDataID>
		protected bool bIsDropdownResizable = false;
		
		// 3-dots button event handler
        /// <MetaDataID>{e3470e2d-3736-4d13-b333-73d53632259b}</MetaDataID>
		protected UICustomEventEditor.OnClick MethodDelegate;
		
		// Extended Attributes
        /// <MetaDataID>{76e8971b-7ae8-4e04-bdba-7496951dd6a2}</MetaDataID>
		[NonSerialized()]protected AttributeCollection oCustomAttributes = null;
        /// <MetaDataID>{3358570d-cb88-4d17-a8aa-1a8596295354}</MetaDataID>
		protected object oTag = null;
        /// <MetaDataID>{4b0fdc46-ef76-4bcf-92cf-dfc0da383186}</MetaDataID>
		protected object oDefaultValue = null;
        /// <MetaDataID>{52ee07b2-6291-4c4a-84d1-0a4821a71c89}</MetaDataID>
		protected Type oDefaultType = null;
		
		// Custom Editor and Custom Type Converter
        /// <MetaDataID>{fe63b60d-ca4b-48cd-8bcf-5a1cfd358779}</MetaDataID>
		[NonSerialized()]protected UITypeEditor oCustomEditor = null;
        /// <MetaDataID>{aa8582c5-b278-46b3-af2e-52668fe9ab7b}</MetaDataID>
		[NonSerialized()]protected TypeConverter oCustomTypeConverter = null;
		
		#endregion
		
		#region "Public methods"

        /// <MetaDataID>{61cbbe2a-5b37-4523-8ee0-e7eed5301a86}</MetaDataID>
		public CustomProperty()
		{
			sName = "New Property";
			oValue = new string(' ',0);
		}

        /// <MetaDataID>{c2c080c5-a8ae-470a-9a14-10c20f075e60}</MetaDataID>
		public CustomProperty(string strName, object objValue, bool boolIsReadOnly, string strCategory, string strDescription, bool boolVisible)
		{
			sName = strName;
			oValue = objValue;
			bIsReadOnly = boolIsReadOnly;
			sDescription = strDescription;
			sCategory = strCategory;
			bVisible = boolVisible;
			if (oValue != null)
			{
				oDefaultValue = oValue;
			}
		}

        /// <MetaDataID>{a1dc5043-cb86-4b0a-a161-a5d592e15f22}</MetaDataID>
		public CustomProperty(string strName, ref object objRef, string strProp, bool boolIsReadOnly, string strCategory, string strDescription, bool boolVisible)
		{
			sName = strName;
			bIsReadOnly = boolIsReadOnly;
			sDescription = strDescription;
			sCategory = strCategory;
			bVisible = boolVisible;
			bRef = true;
			oRef = objRef;
			sProp = strProp;
			if (Value != null)
			{
				oDefaultValue = Value;
			}
		}

        /// <MetaDataID>{6d129144-ae39-47c9-ae44-32a92ba5398f}</MetaDataID>
        public void RebuildAttributes()
        {
            if (bUseFileNameEditor)
            {
                BuildAttributes_FilenameEditor();
            }
            else if (oChoices != null)
            {
                BuildAttributes_CustomChoices();
            }
            else if (oDatasource != null)
            {
                BuildAttributes_ListboxEditor();
            }
            else if (bIsBrowsable)
            {
                BuildAttributes_BrowsableProperty();
            }
        }
		
		#endregion
		
		#region "Private methods"

        /// <MetaDataID>{f4f8bd6d-570f-4b14-8750-51796457582e}</MetaDataID>
		private void BuildAttributes_FilenameEditor()
		{
			ArrayList attrs = new ArrayList();
			UIFilenameEditor.FileDialogFilterAttribute FilterAttribute = new UIFilenameEditor.FileDialogFilterAttribute(sFilter);
			UIFilenameEditor.SaveFileAttribute SaveDialogAttribute = new UIFilenameEditor.SaveFileAttribute();
			Attribute[] attrArray;
			attrs.Add(FilterAttribute);
			if (eDialogType == UIFilenameEditor.FileDialogType.SaveFileDialog)
			{
				attrs.Add(SaveDialogAttribute);
			}
			attrArray =  (System.Attribute[]) attrs.ToArray(typeof(Attribute));
			oCustomAttributes = new AttributeCollection(attrArray);
		}

        /// <MetaDataID>{20e42c2c-d93c-45af-a66a-aa62c1adb581}</MetaDataID>
		private void BuildAttributes_CustomChoices()
		{
			if (oChoices != null)
			{
				CustomChoices.CustomChoicesAttributeList list = new CustomChoices.CustomChoicesAttributeList(oChoices.Items);
				ArrayList attrs = new ArrayList();
				Attribute[] attrArray;
				attrs.Add(list);
				attrArray =  (System.Attribute[]) attrs.ToArray(typeof(Attribute));
				oCustomAttributes = new AttributeCollection(attrArray);
			}
		}

        /// <MetaDataID>{b61c1e18-15d2-4054-a37a-80ea78911558}</MetaDataID>
		private void BuildAttributes_ListboxEditor()
		{
			if (oDatasource != null)
			{
				UIListboxEditor.UIListboxDatasource ds = new UIListboxEditor.UIListboxDatasource(ref oDatasource);
				UIListboxEditor.UIListboxValueMember vm = new UIListboxEditor.UIListboxValueMember(sValueMember);
				UIListboxEditor.UIListboxDisplayMember dm = new UIListboxEditor.UIListboxDisplayMember(sDisplayMember);
				UIListboxEditor.UIListboxIsDropDownResizable ddr = null;
				ArrayList attrs = new ArrayList();
				attrs.Add(ds);
				attrs.Add(vm);
				attrs.Add(dm);
				if (bIsDropdownResizable)
				{
					ddr = new UIListboxEditor.UIListboxIsDropDownResizable();
					attrs.Add(ddr);
				}
				Attribute[] attrArray;
				attrArray =  (System.Attribute[]) attrs.ToArray(typeof(Attribute));
				oCustomAttributes = new AttributeCollection(attrArray);
			}
		}

        /// <MetaDataID>{e8e72b53-24c8-4ad3-8dd1-63519a20e9fd}</MetaDataID>
        private void BuildAttributes_BrowsableProperty()
        {
            BrowsableTypeConverter.BrowsableLabelStyleAttribute style = new BrowsableTypeConverter.BrowsableLabelStyleAttribute(eBrowsablePropertyLabel);
            oCustomAttributes = new AttributeCollection(new Attribute[] { style });
        }

        /// <MetaDataID>{57ac7a4b-8f30-4ab3-8eee-c0db034111d7}</MetaDataID>
		private void BuildAttributes_CustomEventProperty()
		{
			UICustomEventEditor.DelegateAttribute attr = new UICustomEventEditor.DelegateAttribute(MethodDelegate);
			oCustomAttributes = new AttributeCollection(new Attribute[] {attr});
		}

        /// <MetaDataID>{8a2c2d4d-c36d-457c-aca5-4b6a63d7b6b1}</MetaDataID>
		private object DataColumn
		{
			get
			{
				DataRow oRow = (System.Data.DataRow) oRef;
				if (oRow.RowState != DataRowState.Deleted)
				{
				if (oDatasource == null)
				{
					return oRow[sProp];
				}
				else
				{
					DataTable oLookupTable = oDatasource as DataTable;
					if (oLookupTable != null)
					{
						return oLookupTable.Select(sValueMember + "=" + oRow[sProp])[0][sDisplayMember];
					}
					else
					{
						Information.Err().Raise(Constants.vbObjectError + 513, null, "Bind of DataRow with a DataSource that is not a DataTable is not possible", null, null);
						return null;
					}
				}
			}
				else
				{
					return null;
				}
			}
			set
			{
				DataRow oRow = (System.Data.DataRow) oRef;
				if (oRow.RowState != DataRowState.Deleted)
				{
				if (oDatasource == null)
				{
					oRow[sProp] = value;
				}
				else
				{
					DataTable oLookupTable = oDatasource as DataTable;
					if (oLookupTable != null)
					{
						if (oLookupTable.Columns[sDisplayMember].DataType.Equals(System.Type.GetType("System.String")))
						{
							
							oRow[sProp] = oLookupTable.Select(oLookupTable.Columns[sDisplayMember].ColumnName + " = \'" + value + "\'")[0][sValueMember];
						}
						else
						{
							oRow[sProp] = oLookupTable.Select(oLookupTable.Columns[sDisplayMember].ColumnName + " = " + value)[0][sValueMember];
						}
					}
					else
					{
						Information.Err().Raise(Constants.vbObjectError + 514, null, "Bind of DataRow with a DataSource that is not a DataTable is impossible", null, null);
					}
				}
			}
		}
		}
		
		#endregion
		
		#region "Public properties"

        /// <MetaDataID>{b3a855fb-d2dc-438d-88b9-01236860a3ab}</MetaDataID>
        [Category("Appearance"), DisplayName("Name")]
        [DescriptionAttribute("Display Name of the CustomProperty.")]
        [ParenthesizePropertyName(true), XmlElementAttribute("Name")]
        public string Name
        {
			get
			{
				return sName;
			}
			set
			{
				sName = value;
			}
		}

        /// <MetaDataID>{8fd7410e-ded1-4dac-a3d9-820c51bdf9ad}</MetaDataID>
        [Category("Appearance")]
        [ DisplayName("ReadOnly")]
        [ DescriptionAttribute("Set read only attribute of the CustomProperty.")]
        [ XmlElementAttribute("ReadOnly")]
        public bool IsReadOnly
        {
			get
			{
				return bIsReadOnly;
			}
			set
			{
				bIsReadOnly = value;
			}
		}

        /// <MetaDataID>{310e98a7-a11e-4e7c-9d14-c27edede37e8}</MetaDataID>
        [Category("Appearance")]
        [ DescriptionAttribute("Set visibility attribute of the CustomProperty.")]
        public bool Visible
        {
			get
			{
				return bVisible;
			}
			set
			{
				bVisible = value;
			}
		}

        /// <MetaDataID>{540c2999-52b8-4e7b-b025-27574472dabd}</MetaDataID>
        [Category("Appearance")]
        [ DescriptionAttribute("Represent the Value of the CustomProperty.")]
        public object Value
        {
			get
			{
				if (bRef)
				{
                    if (oRef.GetType() == typeof(DataRow)  || oRef.GetType().IsSubclassOf(typeof(DataRow)))
						return this.DataColumn;
                    else
                        return Interaction.CallByName(oRef, sProp, CallType.Get, null);
				}
				else
				{
					return oValue;
				}
			}
			set
			{
				if (bRef)
				{
					if (oRef.GetType() == typeof(DataRow)  || oRef.GetType().IsSubclassOf(typeof(DataRow)))
							this.DataColumn = value;
                    else
                        Interaction.CallByName(oRef, sProp, CallType.Set, value);
				}
				else
				{
                    if (value == null)
                        oValue = "";
                    else
                        oValue = value;
				}
			}
		}

        /// <MetaDataID>{8217a7fc-09ef-4f1e-ba76-e5a90b573f9f}</MetaDataID>
        [Category("Appearance")]
        [ DescriptionAttribute("Set description associated with the CustomProperty.")]
        public string Description
        {
			get
			{
				return sDescription;
			}
			set
			{
				sDescription = value;
			}
		}

        /// <MetaDataID>{18ecba67-ba7b-4d1e-bfb9-1f202f87f09a}</MetaDataID>
        [Category("Appearance")]
        [ DescriptionAttribute("Set category associated with the CustomProperty.")]
        public string Category
        {
			get
			{
				return sCategory;
			}
			set
			{
				sCategory = value;
			}
		}

        /// <MetaDataID>{42102934-d992-46fc-8ee9-07b107b93b9c}</MetaDataID>
        [XmlIgnore()]
        public System.Type Type
		{
			get
			{
				if (Value != null)
				{
					return Value.GetType();
				}
				else
				{
					if (oDefaultValue != null)
					{
						return oDefaultValue.GetType();
					}
					else
					{
						return oDefaultType;
					}
				}
            }
		}

        /// <MetaDataID>{e718ff82-8f4d-464b-a341-93ba5b24a297}</MetaDataID>
        [XmlIgnore()]
        public AttributeCollection Attributes
		{
			get
			{
				return oCustomAttributes;
			}
			set
			{
				oCustomAttributes = value;
			}
		}

        /// <MetaDataID>{c6d6b73e-30f6-4709-93ee-aebf66870b92}</MetaDataID>
        [Category("Behavior")]
        [ DescriptionAttribute("Indicates if the property is browsable or not.")]
        [ XmlElementAttribute(IsNullable = false)]
        public bool IsBrowsable
		{
			get
			{
				return bIsBrowsable;
			}
			set
			{
				bIsBrowsable = value;
				if (value == true)
				{
                    BuildAttributes_BrowsableProperty();
                }
			}
		}

        /// <MetaDataID>{8d8c2ace-333d-46a9-ac60-a7b441253068}</MetaDataID>
		[Category("Appearance")]
        [ DisplayName("Parenthesize")]
        [ DescriptionAttribute("Indicates whether the name of the associated property is displayed with parentheses in the Properties window.")][ DefaultValue(false)]
        [ XmlElementAttribute("Parenthesize")]
        public bool Parenthesize
		{
			get
			{
				return bParenthesize;
			}
			set
			{
				bParenthesize = value;
			}
		}

        /// <MetaDataID>{13710104-bccc-4126-996b-1f4e7031b287}</MetaDataID>
        [Category("Behavior")]
        [ DescriptionAttribute("Indicates the style of the label when a property is browsable.")]
        [ XmlElementAttribute(IsNullable = false)]
        public BrowsableTypeConverter.LabelStyle BrowsableLabelStyle
        {
			get
			{
				return eBrowsablePropertyLabel;
			}
			set
			{
				bool Update = false;
				if (value != eBrowsablePropertyLabel)
				{
					Update = true;
				}
				eBrowsablePropertyLabel = value;
				if (Update)
				{
					BrowsableTypeConverter.BrowsableLabelStyleAttribute style = new BrowsableTypeConverter.BrowsableLabelStyleAttribute(value);
					oCustomAttributes = new AttributeCollection(new Attribute[] {style});
				}
			}
		}

        /// <MetaDataID>{3151e8ba-5f83-4b82-a2ce-9e89630df5b6}</MetaDataID>
        [Category("Behavior")]
        [ DescriptionAttribute("Indicates if the property is masked or not.")]
        [ XmlElementAttribute(IsNullable = false)]public bool IsPassword
        {
			get
			{
				return bIsPassword;
			}
			set
			{
				bIsPassword = value;
			}
		}

        /// <MetaDataID>{860d41e9-02a8-4558-adfc-dc1d90b070f1}</MetaDataID>
        [Category("Behavior")]
        [ DescriptionAttribute("Indicates if the property represents a value in percentage.")]
        [ XmlElementAttribute(IsNullable = false)]
        public bool IsPercentage
        {
			get
			{
				return bIsPercentage;
			}
			set
			{
				bIsPercentage = value;
			}
		}

        /// <MetaDataID>{60a68453-83f9-4f57-8046-89eede8b5f81}</MetaDataID>
        [Category("Behavior")]
        [ DescriptionAttribute("Indicates if the property uses a FileNameEditor converter.")]
        [ XmlElementAttribute(IsNullable = false)]
        public bool UseFileNameEditor
        {
			get
			{
				return bUseFileNameEditor;
			}
			set
			{
				bUseFileNameEditor = value;
			}
		}

        /// <MetaDataID>{f19c5195-f693-4d99-a037-c4a2799f8aa0}</MetaDataID>
        [Category("Behavior")]
        [ DescriptionAttribute("Apply a filter to FileNameEditor converter.")]
        [ XmlElementAttribute(IsNullable = false)]
        public string FileNameFilter
        {
			get
			{
				return sFilter;
			}
			set
			{
				bool UpdateAttributes = false;
				if (value != sFilter)
				{
					UpdateAttributes = true;
				}
				sFilter = value;
				if (UpdateAttributes)
				{
					BuildAttributes_FilenameEditor();
				}
			}
		}

        /// <MetaDataID>{7b9264dd-17bc-43db-a009-fb2de1a8f1bb}</MetaDataID>
        [Category("Behavior")]
        [ DescriptionAttribute("DialogType of the FileNameEditor.")]
        [ XmlElementAttribute(IsNullable = false)]
        public UIFilenameEditor.FileDialogType FileNameDialogType
        {
			get
			{
				return eDialogType;
			}
			set
			{
				bool UpdateAttributes = false;
				if (value != eDialogType)
				{
					UpdateAttributes = true;
				}
				eDialogType = value;
				if (UpdateAttributes)
				{
					BuildAttributes_FilenameEditor();
				}
			}
		}

        /// <MetaDataID>{5f5fc2b9-7268-4557-bed0-839e11867a56}</MetaDataID>
        [Category("Behavior")]
        [ DescriptionAttribute("Custom Choices list.")]
        [ XmlIgnore()]
        public CustomChoices Choices
        {
			get
			{
				return oChoices;
			}
			set
			{
				oChoices = value;
				BuildAttributes_CustomChoices();
			}
		}

        /// <MetaDataID>{af008bc6-4a80-4180-8993-50a47e787d2e}</MetaDataID>
        [Category("Databinding")]
        [ XmlIgnore()]
        public object Datasource
        {
			get
			{
				return oDatasource;
			}
			set
			{
				oDatasource = value;
				BuildAttributes_ListboxEditor();
			}
		}

        /// <MetaDataID>{acaa450c-6dd0-42fd-bb35-3f96e6d87c30}</MetaDataID>
        [Category("Databinding")]
        [ XmlElementAttribute(IsNullable = false)]
        public string ValueMember
		{
			get
			{
				return sValueMember;
			}
			set
			{
				sValueMember = value;
				BuildAttributes_ListboxEditor();
			}
		}

        /// <MetaDataID>{b6649c23-8c64-476e-a7ef-4e8de065c8d6}</MetaDataID>
        [Category("Databinding")]
        [ XmlElementAttribute(IsNullable = false)]
        public string DisplayMember
		{
			get
			{
				return sDisplayMember;
			}
			set
			{
				sDisplayMember = value;
				BuildAttributes_ListboxEditor();
			}
		}

        /// <MetaDataID>{700395f5-73c0-4562-823a-80ee591932e4}</MetaDataID>
        [Category("Databinding")]
        [ XmlElementAttribute(IsNullable = false)]
        public object SelectedValue
		{
			get
			{
				return oSelectedValue;
			}
			set
			{
				oSelectedValue = value;
			}
		}

        /// <MetaDataID>{aed99bda-b828-4520-91a5-9a587b15a6e2}</MetaDataID>
        [Category("Databinding")]
        [ XmlElementAttribute(IsNullable = false)]
        public object SelectedItem
		{
			get
			{
				return oSelectedItem;
			}
			set
			{
				oSelectedItem = value;
			}
		}

        /// <MetaDataID>{55b4cf7e-8794-4538-a21f-24d6e4ed3b0d}</MetaDataID>
        [Category("Databinding")]
        [ XmlElementAttribute(IsNullable = false)]
        public bool IsDropdownResizable
		{
			get
			{
				return bIsDropdownResizable;
			}
			set
			{
				bIsDropdownResizable = value;
				BuildAttributes_ListboxEditor();
			}
		}

        /// <MetaDataID>{f9490a74-b341-48c8-9c75-665056641456}</MetaDataID>
        [XmlIgnore()]public UITypeEditor CustomEditor
		{
			get
			{
				return oCustomEditor;
			}
			set
			{
				oCustomEditor = value;
			}
		}

        /// <MetaDataID>{cfcd4045-ebe8-476f-8f00-137e64342325}</MetaDataID>
        [XmlIgnore()]public TypeConverter CustomTypeConverter
		{
			get
			{
				return oCustomTypeConverter;
			}
			set
			{
				oCustomTypeConverter = value;
			}
		}

        /// <MetaDataID>{fe0e6e6a-a2c2-4f79-bd35-f290f5d41dbe}</MetaDataID>
		[XmlIgnore()]public object Tag
		{
			get
			{
				return oTag;
			}
			set
			{
				oTag = value;
			}
		}

        /// <MetaDataID>{0895203b-e15b-4bd4-8898-405c3fdaf93c}</MetaDataID>
		[XmlIgnore()]public object DefaultValue
		{
			get
			{
				return oDefaultValue;
			}
			set
			{
				oDefaultValue = value;
			}
		}

        /// <MetaDataID>{3b75cf02-f938-47ac-90bc-3387705336d8}</MetaDataID>
		[XmlIgnore()]public Type DefaultType
		{
			get
			{
				return oDefaultType;
			}
			set
			{
				oDefaultType = value;
			}
		}

        /// <MetaDataID>{a555cb23-2210-4d66-be72-28f4ec79e5d2}</MetaDataID>
		[XmlIgnore()]public UICustomEventEditor.OnClick OnClick
		{
			get
			{
				return MethodDelegate;
			}
			set
			{
				MethodDelegate = value;
				BuildAttributes_CustomEventProperty();
			}
		}
		
		#endregion
		
		#region "CustomPropertyDescriptor"
		public class CustomPropertyDescriptor : PropertyDescriptor
		{
			
			protected CustomProperty oCustomProperty;
			
			public CustomPropertyDescriptor(CustomProperty myProperty, Attribute[] attrs) : base(myProperty.Name, attrs)
			{
				if (myProperty == null)
				{
					oCustomProperty = null;
				}
				else
				{
					
					oCustomProperty = myProperty;
				}
			}
			
			public override bool CanResetValue(object component)
			{
				if ((oCustomProperty.DefaultValue != null)|| (oCustomProperty.DefaultType != null))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			
			public override System.Type ComponentType
			{
				get
				{
					return this.GetType();
				}
			}
			
			public override object GetValue(object component)
			{
				return oCustomProperty.Value;
			}
			
			public override bool IsReadOnly
			{
				get
				{
					return oCustomProperty.IsReadOnly;
				}
			}
			
			public override System.Type PropertyType
			{
				get
				{
					return oCustomProperty.Type;
				}
			}
			
			public override void ResetValue(object component)
			{
				oCustomProperty.Value = oCustomProperty.DefaultValue;
				this.OnValueChanged(component, EventArgs.Empty);
			}
			
			public override void SetValue(object component, object value)
			{
				oCustomProperty.Value = value;
				this.OnValueChanged(component, EventArgs.Empty);
			}
			
			public override bool ShouldSerializeValue(object component)
			{
				object oValue = oCustomProperty.Value;
				if ((oCustomProperty.DefaultValue != null)&& (oValue != null))
				{
					return ! oValue.Equals(oCustomProperty.DefaultValue);
				}
				else
				{
					return false;
				}
			}
			
			public override string Description
			{
				get
				{
					return oCustomProperty.Description;
				}
			}
			
			public override string Category
			{
				get
				{
					return oCustomProperty.Category;
				}
			}
			
			public override string DisplayName
			{
				get
				{
					return oCustomProperty.Name;
				}
			}
			
			public override bool IsBrowsable
			{
				get
				{
					return oCustomProperty.IsBrowsable;
				}
			}
			
			public object CustomProperty
			{
				get
				{
					return oCustomProperty;
				}
			}
		}
		#endregion		
	}	
}