namespace ConnectableControls
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    /// <MetaDataID>{21e1eb93-3004-45a4-a510-115eeb73ac15}</MetaDataID>
    public partial class PropertyGridEx : PropertyGrid
	{
			
		#region "Protected variables and objects"
		// CustomPropertyCollection assigned to MyBase.SelectedObject
        /// <MetaDataID>{59895713-396e-44cb-9619-0de31bcac2ef}</MetaDataID>
		protected CustomPropertyCollection oCustomPropertyCollection;
        /// <MetaDataID>{4d1269bf-2a38-4225-82ad-72d69d888fcc}</MetaDataID>
		protected bool bShowCustomProperties;
		
		// CustomPropertyCollectionSet assigned to MyBase.SelectedObjects
        /// <MetaDataID>{ebfe9e3d-8b9e-4a61-8a03-50491b0cfce6}</MetaDataID>
		protected CustomPropertyCollectionSet oCustomPropertyCollectionSet;
        /// <MetaDataID>{a602db12-bf28-4169-a895-2a5d1c3c0f3c}</MetaDataID>
		protected bool bShowCustomPropertiesSet;

		// Internal PropertyGrid Controls
        /// <MetaDataID>{a2a1f4eb-7a65-4c57-aa27-6f161d296960}</MetaDataID>
		protected object oPropertyGridView;
        /// <MetaDataID>{78261846-2609-41d2-99b7-8fa0071b74d5}</MetaDataID>
		protected object oHotCommands;
        /// <MetaDataID>{2c865301-9690-4da4-ba3e-74bc06e3967f}</MetaDataID>
		protected object oDocComment;
        /// <MetaDataID>{c68f1297-61f9-447b-8677-015755c9ecbd}</MetaDataID>
		protected ToolStrip oToolStrip;
		
		// Internal PropertyGrid Fields
        /// <MetaDataID>{a9ed7c5f-c2d7-48a6-a901-40e719da6484}</MetaDataID>
		protected Label oDocCommentTitle;
        /// <MetaDataID>{51aed21b-207b-4492-9cc4-3ef2ad46b287}</MetaDataID>
		protected Label oDocCommentDescription;
        /// <MetaDataID>{f9fdecd6-8a78-47c3-af5e-9640a05ca8f6}</MetaDataID>
		protected FieldInfo oPropertyGridEntries;

        // Properties variables
        /// <MetaDataID>{217ebf0d-db45-4720-ab59-2756293b1a59}</MetaDataID>
        protected bool bAutoSizeProperties;
        /// <MetaDataID>{e68ea7c9-e88b-448b-b252-2556045c1c17}</MetaDataID>
        protected bool bDrawFlatToolbar;
        #endregion
		
		#region "Public Functions"
        /// <MetaDataID>{575f6ec9-900f-4cf4-9cfc-166d23005790}</MetaDataID>
		public PropertyGridEx()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();
			
			// Add any initialization after the InitializeComponent() call.
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

			// Initialize collections
            oCustomPropertyCollection = new CustomPropertyCollection();
            oCustomPropertyCollectionSet = new CustomPropertyCollectionSet();
			
			// Attach internal controls
			oPropertyGridView = base.GetType().BaseType.InvokeMember("gridView", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance, null, this, null);
			oHotCommands = base.GetType().BaseType.InvokeMember("hotcommands", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance, null, this, null);
			oToolStrip = (ToolStrip) base.GetType().BaseType.InvokeMember("toolStrip", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance, null, this, null);
			oDocComment = base.GetType().BaseType.InvokeMember("doccomment", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance, null, this, null);
			
			// Attach DocComment internal fields
			if (oDocComment != null)
			{
				oDocCommentTitle = (Label)oDocComment.GetType().InvokeMember("m_labelTitle", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance, null, oDocComment, null);
				oDocCommentDescription = (Label)oDocComment.GetType().InvokeMember("m_labelDesc", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance, null, oDocComment, null);
			}
			
			// Attach PropertyGridView internal fields
			if (oPropertyGridView != null)
			{
				oPropertyGridEntries = oPropertyGridView.GetType().GetField("allGridEntries", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
			}
			
			// Apply Toolstrip style
			if (oToolStrip != null)
			{
				ApplyToolStripRenderMode(bDrawFlatToolbar);
			}
			

        }

        /// <MetaDataID>{8b5ce8d5-aca0-41ec-9c09-63b1d4693584}</MetaDataID>
		public void MoveSplitterTo(int x)
		{
            oPropertyGridView.GetType().InvokeMember("MoveSplitterTo", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance, null, oPropertyGridView, new object[] { x });
        }

        /// <MetaDataID>{67d1c070-6bbe-4382-9158-dc67360065d0}</MetaDataID>
		public override void Refresh()
		{
			if (bShowCustomPropertiesSet)
			{
				base.SelectedObjects =  (object[]) oCustomPropertyCollectionSet.ToArray();
			}
			base.Refresh();
			if (bAutoSizeProperties)
			{
				AutoSizeSplitter(32);
			}
		}

        /// <MetaDataID>{4583a910-6c29-493f-a2f0-17558372489a}</MetaDataID>
		public void SetComment(string title, string description)
		{
            MethodInfo method = oDocComment.GetType().GetMethod("SetComment");
            method.Invoke(oDocComment, new object[] { title, description });
			//oDocComment.SetComment(title, description);
		}
		
		#endregion
		
		#region "Protected Functions"
        /// <MetaDataID>{cccd4cd5-5613-4c5f-866d-a7fb3e5b2a79}</MetaDataID>
		protected override void OnResize(System.EventArgs e)
		{
			base.OnResize(e);
			if (bAutoSizeProperties)
			{
				AutoSizeSplitter(32);
			}
		}

        /// <MetaDataID>{e203c806-d176-4c13-b33d-9903a7aaf13f}</MetaDataID>
		protected void AutoSizeSplitter(int RightMargin)
		{
			
			GridItemCollection oItemCollection =  (System.Windows.Forms.GridItemCollection) oPropertyGridEntries.GetValue(oPropertyGridView);
			if (oItemCollection == null)
			{
				return;
			}
			System.Drawing.Graphics oGraphics = System.Drawing.Graphics.FromHwnd(this.Handle);
			int CurWidth = 0;
			int MaxWidth = 0;
			
			foreach (GridItem oItem in oItemCollection)
			{
				if (oItem.GridItemType == GridItemType.Property)
				{
					CurWidth =  (int) oGraphics.MeasureString(oItem.Label, this.Font).Width + RightMargin;
					if (CurWidth > MaxWidth)
					{
						MaxWidth = CurWidth;
					}
				}
			}
			
			MoveSplitterTo(MaxWidth);
		}
        /// <MetaDataID>{626fb83b-b4cf-4c28-bfb7-34280334e607}</MetaDataID>
		protected void ApplyToolStripRenderMode(bool value)
		{
			if (value)
			{
				oToolStrip.Renderer = new ToolStripSystemRenderer();
			}
			else
			{
                ToolStripProfessionalRenderer renderer = new ToolStripProfessionalRenderer(new CustomColorScheme());
                renderer.RoundedEdges = false;
				oToolStrip.Renderer = renderer;
			}
		}
		#endregion
		
		#region "Properties"

        /// <MetaDataID>{297fa3bc-5123-4307-bc55-d8036c9bf282}</MetaDataID>
        [Category("Behavior"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), DescriptionAttribute("Set the collection of the CustomProperty. Set ShowCustomProperties to True to enable it."), RefreshProperties(RefreshProperties.Repaint)]public CustomPropertyCollection Item
        {
			get
			{
				return oCustomPropertyCollection;
			}
		}

        /// <MetaDataID>{c20ca925-352d-419e-9e6d-2e60b27cb27c}</MetaDataID>
		[Category("Behavior"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), DescriptionAttribute("Set the CustomPropertyCollectionSet. Set ShowCustomPropertiesSet to True to enable it."), RefreshProperties(RefreshProperties.Repaint)]public CustomPropertyCollectionSet ItemSet
        {
			get
			{
				return oCustomPropertyCollectionSet;
			}
		}

        /// <MetaDataID>{7cc627f8-b981-4127-a503-8a948a55283c}</MetaDataID>
        [Category("Behavior"), DefaultValue(false), DescriptionAttribute("Move automatically the splitter to better fit all the properties shown.")]public bool AutoSizeProperties
        {
			get
			{
				return bAutoSizeProperties;
			}
			set
			{
				bAutoSizeProperties = value;
				if (value)
				{
					AutoSizeSplitter(32);
				}
			}
		}

        /// <MetaDataID>{51b70029-fa4c-418d-923b-2edf023eb798}</MetaDataID>
        [Category("Behavior"), DefaultValue(false), DescriptionAttribute("Use the custom properties collection as SelectedObject."), RefreshProperties(RefreshProperties.All)]public bool ShowCustomProperties
        {
			get
			{
				return bShowCustomProperties;
			}
			set
			{
				if (value == true)
				{
					bShowCustomPropertiesSet = false;
					base.SelectedObject = oCustomPropertyCollection;
				}
				bShowCustomProperties = value;
			}
		}

        /// <MetaDataID>{f66610cc-be73-44e5-adb3-824335def53f}</MetaDataID>
        [Category("Behavior"), DefaultValue(false), DescriptionAttribute("Use the custom properties collections as SelectedObjects."), RefreshProperties(RefreshProperties.All)]public bool ShowCustomPropertiesSet
        {
			get
			{
				return bShowCustomPropertiesSet;
			}
			set
			{
				if (value == true)
				{
					bShowCustomProperties = false;
					base.SelectedObjects =  (object[]) oCustomPropertyCollectionSet.ToArray();
				}
				bShowCustomPropertiesSet = value;
			}
		}

        /// <MetaDataID>{c3e5772c-448f-4888-ace9-9be262b6e0f0}</MetaDataID>
		[Category("Appearance"), DefaultValue(false), DescriptionAttribute("Draw a flat toolbar")]public new bool DrawFlatToolbar
        {
			get
			{
				return bDrawFlatToolbar;
			}
			set
			{
				bDrawFlatToolbar = value;
				ApplyToolStripRenderMode(bDrawFlatToolbar);
			}
		}

        /// <MetaDataID>{c6c4b0d2-d9fc-4cf8-be71-cbab0758c2f6}</MetaDataID>
        [Category("Appearance"), DisplayName("Toolstrip"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), DescriptionAttribute("Toolbar object"), Browsable(true)]public ToolStrip ToolStrip
        {
			get
			{
				return oToolStrip;
			}
		}

        /// <MetaDataID>{f267b36b-8cea-4918-abe6-74ad9dafa8cb}</MetaDataID>
		[Category("Appearance"), DisplayName("Help"), DescriptionAttribute("DocComment object. Represent the comments area of the PropertyGrid."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]public Control DocComment
		{
			get
			{
				return  (System.Windows.Forms.Control) oDocComment;
			}
		}

        /// <MetaDataID>{86a7972c-78f0-485c-af3a-3b6ccd68fde1}</MetaDataID>
		[Category("Appearance"), DisplayName("HelpTitle"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), DescriptionAttribute("Help Title Label."), Browsable(true)]public Label DocCommentTitle
		{
			get
			{
				return oDocCommentTitle;
			}
		}

        /// <MetaDataID>{8d9e45ae-5bbc-445f-8f6f-0f6094e79802}</MetaDataID>
		[Category("Appearance"), DisplayName("HelpDescription"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), DescriptionAttribute("Help Description Label."), Browsable(true)]public Label DocCommentDescription
		{
			get
			{
				return oDocCommentDescription;
			}
		}

        /// <MetaDataID>{1a9fffe4-273c-45a9-b289-56b8b664cd8d}</MetaDataID>
		[Category("Appearance"), DisplayName("HelpImageBackground"), DescriptionAttribute("Help Image Background.")]public Image DocCommentImage
		{
			get
			{
				return ((Control)oDocComment).BackgroundImage;
			}
			set
			{
                ((Control)oDocComment).BackgroundImage = value;
			}
		}
		
		#endregion
		
	}
	
}

