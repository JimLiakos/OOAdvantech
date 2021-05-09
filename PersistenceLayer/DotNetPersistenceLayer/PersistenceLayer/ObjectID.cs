namespace OOAdvantech.PersistenceLayer
{
	using System;

	/// <MetaDataID>{6C5A1B95-909E-4990-896B-B36ABEC9A503}</MetaDataID>
	//[MetaDataRepository.BackwardCompatibilityID("{6C5A1B95-909E-4991-896B-B36ABEC9A503}")]
	[Serializable()]
	public class ObjectID
	{
        /// <MetaDataID>{9708f2d8-a7da-456d-ab65-a11047cf7c73}</MetaDataID>
        public static ObjectID Empty = new OOAdvantech.PersistenceLayer.ObjectID(new OOAdvantech.MetaDataRepository.ObjectIdentityType(new System.Collections.Generic.List<OOAdvantech.MetaDataRepository.IIdentityPart>()), new object[0], 0);
        /// <MetaDataID>{a4a07985-511d-4c0b-acac-1b5a1db301ef}</MetaDataID>
        public object[] ObjectIDPartsValues;
    	/// <MetaDataID>{B869009E-6876-495C-8D65-D0027606BE8F}</MetaDataID>
		protected ObjectID()
		{  
		}

        /// <MetaDataID>{20abd432-5aeb-4f13-afc4-66abb29bbb3d}</MetaDataID>
        public override string ToString()
        {
            return ToString(System.Globalization.CultureInfo.CurrentCulture);
        }

        /// <MetaDataID>{c85c6a2a-1b23-4a0b-8fc5-97e4ba8c45e7}</MetaDataID>
        string ObjectIDAsString;
		/// <MetaDataID>{5F03EE3F-4ADB-4E18-BC78-5C6EB848929D}</MetaDataID>
		public virtual string ToString(IFormatProvider provider)
		{

            if (ObjectIDAsString == null)
            {
                ObjectIDAsString = "ObjectID(";

                for (int i = 0; i < ObjectIDPartsValues.Length; i++)
                {
                    if (i > 0)
                        ObjectIDAsString += ",";

                    ObjectIDAsString += ObjectIdentityType.Parts[i].Name + "(" + ObjectIdentityType.Parts[i].Type.FullName + ") = '";
                    if (ObjectIDPartsValues[i] != null)
                    {
                        if (ObjectIDPartsValues[i] is Guid)
                            ObjectIDAsString += ObjectIDPartsValues[i].ToString() + "' ";
                        else
                            ObjectIDAsString += (System.Convert.ChangeType(ObjectIDPartsValues[i], typeof(string), provider) as string) + "' ";
                    }
                    else
                        ObjectIDAsString += "null' ";
                }
                ObjectIDAsString += ")";
            }

            return ObjectIDAsString;// "ObjectID(ObjectID = '" + IntObjID.ToString() + "')";
		}


        /// <summary>
        /// Converts the string representation of a ObjectID to equivalent ObjectID.
        /// A return value indicates whether the conversion succeeded or
        /// failed.
        /// </summary>
        /// <param name="objectIDAsString">
        /// A string representing the ObjectID to convert.
        /// </param>
        /// <param name="objectID">
        /// When this method returns, contains the ObjectID value equivalent
        /// to the ObjectID contained in objectIDAsString, if the conversion succeeded, or null if the
        /// conversion failed. The conversion fails if the objectIDAsString parameter is null, is not
        /// of the correct format, This parameter is passed uninitialized.
        /// </param>
        /// <returns>
        /// true if s was converted successfully; otherwise, false.
        /// </returns>
        /// <MetaDataID>{a8df16ce-9729-4d56-ae7c-6a08324426a8}</MetaDataID>
        public static bool TryParse(string objectIDAsString, out ObjectID objectID)
        {
            return TryParse(objectIDAsString, System.Globalization.CultureInfo.CurrentCulture, out objectID);
        }


        /// <summary>
        /// Converts the string representation of a ObjectID to equivalent ObjectID.
        /// A return value indicates whether the conversion succeeded or
        /// failed.
        /// </summary>
        /// <param name="objectIDAsString">
        /// A string representing the ObjectID to convert.
        /// </param>
        /// <param name="provider">
        /// An System.IFormatProvider object that supplies culture-specific formatting
        /// information about objectIDAsString.
        /// </param>
        /// <param name="objectID">
        /// When this method returns, contains the ObjectID value equivalent
        /// to the ObjectID contained in objectIDAsString, if the conversion succeeded, or null if the
        /// conversion failed. The conversion fails if the objectIDAsString parameter is null, is not
        /// of the correct format, This parameter is passed uninitialized.
        /// </param>
        /// <returns>
        /// true if s was converted successfully; otherwise, false.
        /// </returns>
        /// <MetaDataID>{1775f2a4-1134-436a-9fc6-11344efbe41f}</MetaDataID>
        public static bool TryParse(string objectIDAsString,IFormatProvider provider, out ObjectID objectID )
        {
            objectID = null;
            try
            {
                
                System.Collections.Generic.List<OOAdvantech.MetaDataRepository.IIdentityPart> objectIdentityTypeParts = new System.Collections.Generic.List<OOAdvantech.MetaDataRepository.IIdentityPart>();
                System.Collections.Generic.List<object> partValues = new System.Collections.Generic.List<object>();
                if (objectIDAsString.IndexOf("ObjectID(") == 0)
                {
                    objectIDAsString = objectIDAsString.Substring("ObjectID(".Length);
                    int pos = objectIDAsString.IndexOf("(");
                    if (pos == -1)
                        return false;
                    do
                    {
                        string partName = objectIDAsString.Substring(0, pos);
                        objectIDAsString = objectIDAsString.Substring(pos + 1);
                        pos = objectIDAsString.IndexOf(")");
                        if (pos == -1)
                            return false;
                        string partTypeFullName = objectIDAsString.Substring(0, pos);
                        objectIDAsString = objectIDAsString.Substring(pos + 1);
                        pos = objectIDAsString.IndexOf("= '");
                        if (pos == -1)
                            return false;
                        objectIDAsString = objectIDAsString.Substring(pos + "= '".Length);
                        pos = objectIDAsString.IndexOf("'");
                        if (pos == -1)
                            return false;
                        Type partType = System.Type.GetType(partTypeFullName);
                        objectIdentityTypeParts.Add(new MetaDataRepository.IdentityPart(partName, partName, partType));
                        object partValue = null;
                        if (partType == typeof(Guid))
                            partValue = new Guid(objectIDAsString.Substring(0, pos));
                        else
                            partValue = System.Convert.ChangeType(objectIDAsString.Substring(0, pos), partType,provider);
                        objectIDAsString = objectIDAsString.Substring(pos + 1);
                        partValues.Add(partValue);

                        pos = objectIDAsString.IndexOf(",");
                        if (pos != -1)
                        {
                            objectIDAsString = objectIDAsString.Substring(pos + 1);
                            pos = objectIDAsString.IndexOf("(");
                        }
                        else
                            if (objectIDAsString.Trim() != ")")
                                return false;
                    }
                    while (objectIDAsString.Trim() != ")");
                    objectID = new ObjectID(new MetaDataRepository.ObjectIdentityType(objectIdentityTypeParts), partValues.ToArray(), 0);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception error)
            {
                return false;
            }
            
            
        }
        /// <MetaDataID>{862af45c-3b2a-4aad-94f3-d4a520e815a5}</MetaDataID>
        public static ObjectID Parse(string objectIDAsString)
        {
            return Parse(objectIDAsString, System.Globalization.CultureInfo.CurrentCulture);
        }

        /// <MetaDataID>{27072d95-57b1-4921-8d75-4d5fbec4b6e2}</MetaDataID>
        public static ObjectID Parse(string objectIDAsString, IFormatProvider provider)
        {

            ObjectID objectID = null;
            if (TryParse(objectIDAsString,provider, out objectID))
                return objectID;
            else
                throw new Exception("Input string was not in a correct format.");
            
        }

        /// <MetaDataID>{75719daa-1844-44bd-85ba-437796c8c14e}</MetaDataID>
        static OOAdvantech.MetaDataRepository.ObjectIdentityType GuidIdentityType=new OOAdvantech.MetaDataRepository.ObjectIdentityType(new System.Collections.Generic.List<OOAdvantech.MetaDataRepository.IIdentityPart>{new MetaDataRepository.IdentityPart("ObjectID", "ObjectID", typeof(Guid))});

        /// <MetaDataID>{1E5807A9-A52F-41EA-A130-8DC2AB24C13D}</MetaDataID>
		public ObjectID(Guid IntObjIDValue,int ObjCellIDValue)
		{
			if(IntObjIDValue==System.Guid.Empty)
				IntObjID=System.Guid.NewGuid();
			else
				IntObjID=IntObjIDValue;
            ObjectIDPartsValues = new object[1] { IntObjID };
			ObjCellID=ObjCellIDValue;
            ObjectIdentityType = GuidIdentityType;
		}

        /// <MetaDataID>{e0825471-87b3-40c5-b99d-dd10aa724f2b}</MetaDataID>
        public readonly OOAdvantech.MetaDataRepository.ObjectIdentityType ObjectIdentityType;

        public readonly string StorageIdentity;
        public ObjectID(OOAdvantech.MetaDataRepository.ObjectIdentityType objectIdentityType, object[] objectIDPartsValues, string storageIdentity=null) :this (objectIdentityType, objectIDPartsValues,0)
        {
            StorageIdentity = storageIdentity;
        }



        /// <MetaDataID>{57a6b07c-2d58-4c52-9bdb-744da213622c}</MetaDataID>
        public ObjectID(OOAdvantech.MetaDataRepository.ObjectIdentityType objectIdentityType, object[] objectIDPartsValues, int ObjCellIDValue)
        {
            ObjectIDPartsValues = objectIDPartsValues;
            ObjectIdentityType = objectIdentityType;
            ObjCellID = ObjCellIDValue;
            if (ObjectIdentityType.Parts.Count>0&& ObjectIdentityType.Parts[0].PartTypeName == "ObjectID" && ObjectIdentityType.Parts[0].Type==typeof(Guid))
                IntObjID = (Guid)objectIDPartsValues[0];
        }
        /// <MetaDataID>{b45c060a-9b46-4277-bfaa-c5b382983349}</MetaDataID>
        int HashCode = 0;
        /// <MetaDataID>{d76011f0-f118-476c-a538-b6424cead4e7}</MetaDataID>
        public override int GetHashCode()
        {
            if (HashCode != 0)
                return HashCode;
            int num = -1162279000;
            foreach (object partValue in ObjectIDPartsValues)
                num = (-1521134295 * num) + GetHashCode(partValue);
            HashCode = num;
            return HashCode;
        }

        /// <MetaDataID>{71680ca6-bbe1-46b8-871a-201eb3eacc0d}</MetaDataID>
        private int GetHashCode(object partValue)
        {
            if (partValue == null)
                return 0;
            else
                return partValue.GetHashCode();
        }
		/// <MetaDataID>{7685C941-BE1E-4683-A2FB-A06CC6D65461}</MetaDataID>
		public System.Guid IntObjID;
		/// <MetaDataID>{E2C562AD-6B81-45CB-A73F-33C7BD3F1519}</MetaDataID>
		public int ObjCellID;
		/// <MetaDataID>{4EABECF4-A338-44AB-A7BF-282B3531CF95}</MetaDataID>
        //public override bool Equals(object obj)
        //{
        //    if(obj is ObjectID)
        //    {
        //        ObjectID objectID=obj as ObjectID;
        //        if((objectID.IntObjID==IntObjID))//&&ObjCellID==objectID.ObjCellID)
        //            return true;
        //    }
        //    return false;
        //}
        public override bool Equals(object obj)
        {
            if (obj is ObjectID && ((ObjectID)obj).ObjectIDPartsValues.Length == ObjectIDPartsValues.Length)
            {
                for (int i = 0; i < ObjectIDPartsValues.Length; i++)
                {
                    object leftValue = ObjectIDPartsValues[i];
                    object rightValue = ((ObjectID)obj).ObjectIDPartsValues[i];
                    if (leftValue != null && rightValue == null)
                        return false;

                    if (leftValue == null && rightValue != null)
                        return false;
                    if (leftValue != null && rightValue != null)
                        if (!leftValue.Equals(rightValue))
                            return false;
                }
                return true;

            }
            else
                return false;

        }
		/// <MetaDataID>{77A99809-3851-4DA3-8EB9-002D1D6C5AB8}</MetaDataID>
		public T GetTypedMemberValue<T>(string memberName)
		{
            object memberValue = GetMemberValue(memberName);
            return (T)memberValue;
		}

        /// <MetaDataID>{7a3670ae-79fc-4a6f-9837-ddec9a1ab1b9}</MetaDataID>
        public object GetPartValue(int index)
        {
            return ObjectIDPartsValues[index];
        }
        /// <MetaDataID>{582de3af-e923-4fc3-bf23-451fb055c7f6}</MetaDataID>
        public object GetMemberValue(string memberName)
        {
            //if(memberName=="ObjectID") 
            //    return IntObjID;
            //TODO κατι γρήγορο και σωστό
            if (ObjectIDPartsValues.Length == 1)
                return ObjectIDPartsValues[0];
            throw new System.Exception("The " + memberName + " isn't member of Object ID.");
        }
		/// <MetaDataID>{B32488EF-2718-4930-9C00-9CB0B66675E8}</MetaDataID>
		public void SetMemberValue(string memberName,object value)
		{
			if(memberName=="ObjectID")
				IntObjID=(System.Guid)value;
		}



        
    }
}
