namespace ConnectableControls
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Windows.Forms;

    /// <MetaDataID>{e0daf034-fb7b-4990-b0a7-e786d9adeb24}</MetaDataID>
    [Serializable()]public class CustomChoices : ArrayList
	{
        /// <MetaDataID>{6321c8b0-73c4-4757-9d18-69d2299cc6c7}</MetaDataID>
		public CustomChoices(ArrayList array, bool IsSorted)
		{
			this.AddRange(array);
			if (IsSorted)
			{
				this.Sort();
			}
		}

        /// <MetaDataID>{edd0089c-820e-40de-8a14-563f6d334d02}</MetaDataID>
		public CustomChoices(ArrayList array)
		{
			this.AddRange(array);
		}

        /// <MetaDataID>{27297103-6201-4f2e-b0aa-c99698fe2781}</MetaDataID>
		public CustomChoices(string[] array, bool IsSorted)
		{
			this.AddRange(array);
			if (IsSorted)
			{
				this.Sort();
			}
		}

        /// <MetaDataID>{16a91778-e3fe-4c0a-bb37-f705d49679a3}</MetaDataID>
		public CustomChoices(string[] array)
		{
			this.AddRange(array);
		}

        /// <MetaDataID>{e729764d-db24-4f7b-948d-ea00c8afe923}</MetaDataID>
		public CustomChoices(int[] array, bool IsSorted)
		{
			this.AddRange(array);
			if (IsSorted)
			{
				this.Sort();
			}
		}

        /// <MetaDataID>{ea334155-c55e-49bc-8fff-3a9f8f2b5c68}</MetaDataID>
		public CustomChoices(int[] array)
		{
			this.AddRange(array);
		}

        /// <MetaDataID>{a1caf686-408f-46bf-a6d5-7b1644e24053}</MetaDataID>
		public CustomChoices(double[] array, bool IsSorted)
		{
			this.AddRange(array);
			if (IsSorted)
			{
				this.Sort();
			}
		}

        /// <MetaDataID>{2713381c-cc75-473b-be75-b3aa02b81855}</MetaDataID>
		public CustomChoices(double[] array)
		{
			this.AddRange(array);
		}

        /// <MetaDataID>{cc62a955-fbd6-497e-99fb-2e07cea9d850}</MetaDataID>
		public CustomChoices(object[] array, bool IsSorted)
		{
			this.AddRange(array);
			if (IsSorted)
			{
				this.Sort();
			}
		}

        /// <MetaDataID>{5ec655ab-bb33-4a00-99ad-fd34eadb3f53}</MetaDataID>
		public CustomChoices(object[] array)
		{
			this.AddRange(array);
		}

        /// <MetaDataID>{960c7772-23bb-471b-95c7-c61bd2be4a93}</MetaDataID>
		public ArrayList Items
		{
			get
			{
				return this;
			}
		}
		
		public class CustomChoicesTypeConverter : TypeConverter
		{
			
			private CustomChoicesAttributeList oChoices = null;
			public override bool GetStandardValuesSupported(System.ComponentModel.ITypeDescriptorContext context)
			{
				bool returnValue;
				CustomChoicesAttributeList Choices =  (CustomChoicesAttributeList) context.PropertyDescriptor.Attributes[typeof(CustomChoicesAttributeList)];
				if (oChoices != null)
				{
					return true;
				}
				if (Choices != null)
				{
					oChoices = Choices;
					returnValue = true;
				}
				else
				{
					returnValue = false;
				}
				return returnValue;
			}
			public override bool GetStandardValuesExclusive(System.ComponentModel.ITypeDescriptorContext context)
			{
				bool returnValue;
				CustomChoicesAttributeList Choices =  (CustomChoicesAttributeList) context.PropertyDescriptor.Attributes[typeof(CustomChoicesAttributeList)];
				if (oChoices != null)
				{
					return true;
				}
				if (Choices != null)
				{
					oChoices = Choices;
					returnValue = true;
				}
				else
				{
					returnValue = false;
				}
				return returnValue;
			}
			public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(System.ComponentModel.ITypeDescriptorContext context)
			{
				CustomChoicesAttributeList Choices =  (CustomChoices.CustomChoicesAttributeList) context.PropertyDescriptor.Attributes[typeof(CustomChoicesAttributeList)];
				if (oChoices != null)
				{
					return oChoices.Values;
				}
				return base.GetStandardValues(context);
			}
		}
		
		public class CustomChoicesAttributeList : Attribute
		{
			
			private ArrayList oList = new ArrayList();
			
			public ArrayList Item
			{
				get
				{
					return this.oList;
				}
			}
			
			public TypeConverter.StandardValuesCollection Values
			{
				get
				{
					return new TypeConverter.StandardValuesCollection(this.oList);
				}
			}
			
			public CustomChoicesAttributeList(string[] List)
			{
				oList.AddRange(List);
			}
			
			public CustomChoicesAttributeList(ArrayList List)
			{
				oList.AddRange(List);
			}
			
			public CustomChoicesAttributeList(ListBox.ObjectCollection List)
			{
				oList.AddRange(List);
			}
		}
	}
}
