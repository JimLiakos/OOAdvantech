using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ConnectableControls.Tree.NodeControls
{
    /// <MetaDataID>{B8162D10-D759-4674-B9A3-3A204755C011}</MetaDataID>
	public abstract class BindableControl: NodeControl
	{
		private string _propertyName = "";
		//[DefaultValue("")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string DataPropertyName
		{
			get { return _propertyName; }
			set 
			{
				if (_propertyName == null)
					_propertyName = string.Empty;
				_propertyName = value; 
			}
		}




        /// <MetaDataID>{1BDFEF7A-CB93-4A3A-BB0C-2EA10F2DED42}</MetaDataID>
		public object GetValue(TreeNode node)
		{
            try
            {
                OOAdvantech.AccessorBuilder.FieldPropertyAccessor piFast = GetPropertyInfo(node);

                PropertyInfo pi = null;
                if (piFast != null)
                    pi = piFast.MemberInfo as PropertyInfo;

                if (pi != null && pi.CanRead)
                    return piFast.GetValue(node.Tag);
                else
                    return null;
            }
            catch (System.Exception error)
            {
                throw;
            }
		}

        /// <MetaDataID>{7EFA82F7-E036-433E-AE1A-ACF842B56BDB}</MetaDataID>
		public void SetValue(TreeNode node, object value)
		{
            OOAdvantech.AccessorBuilder.FieldPropertyAccessor piFast = GetPropertyInfo(node);

            PropertyInfo pi = null;
            if (piFast != null)
                pi = piFast.MemberInfo as PropertyInfo;
			if (pi != null && pi.CanWrite)
			{
				try
				{
					pi.SetValue(node.Tag, value, null);
				}
				catch (TargetInvocationException ex)
				{
					if (ex.InnerException != null)
						throw new ArgumentException(ex.InnerException.Message, ex.InnerException);
					else
						throw new ArgumentException(ex.Message);
				}
			}
            
		}

        /// <MetaDataID>{AD6FE289-C82A-4AF8-8047-8F9CE3FA3509}</MetaDataID>
		public Type GetPropertyType(TreeNode node)
		{
            
			if (node.Tag != null && !string.IsNullOrEmpty(DataPropertyName))
			{
				Type type = node.Tag.GetType();
				PropertyInfo pi = type.GetProperty(DataPropertyName);
				if (pi != null)
					return pi.PropertyType;
			}
			return null;
		}

        /// <MetaDataID>{694503CC-CE2C-40F1-810F-C1B24220B065}</MetaDataID>
        private OOAdvantech.AccessorBuilder.FieldPropertyAccessor GetPropertyInfo(TreeNode node)
		{
			if (node.Tag != null && !string.IsNullOrEmpty(DataPropertyName))
			{
				Type type = node.Tag.GetType();
				return OOAdvantech.AccessorBuilder.GetPropertyAccessor(type.GetProperty(DataPropertyName));
			}
			return null;
		}

        /// <MetaDataID>{FD7C95A5-EFA5-4239-971E-1A523AD1917E}</MetaDataID>
		public override string ToString()
		{
			if (string.IsNullOrEmpty(DataPropertyName))
				return GetType().Name;
			else
				return string.Format("{0} ({1})", GetType().Name, DataPropertyName);
		}
	}
}
