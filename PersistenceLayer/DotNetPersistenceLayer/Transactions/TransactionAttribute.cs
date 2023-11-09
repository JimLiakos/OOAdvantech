using System;
using OOAdvantech;
using System.Reflection;
using System.Linq;
#if DeviceDotNet 
using System.PCL.Reflection;
#else
using System.Reflection;
#endif

namespace OOAdvantech.Transactions
{
    /// <summary>Indicates that a class can be participating in transaction context.</summary>
    /// <MetaDataID>{DD1786DB-E67C-4A8C-8A96-228A1630D512}</MetaDataID>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class TransactionalAttribute : Attribute
    {
        /// <MetaDataID>{CDD3F6D8-75F7-4F26-8B00-5652C1EFFD18}</MetaDataID>
        public TransactionalAttribute()
        {

        }
    }

    ///<summary>
    /// Sometimes we want from the transaction system to act on reference field like a value type field (contain by value). 
    /// The system has to control the state of object of field automatically. 
    /// In case where the ContainByValue attribute used from field, the field behaves as type value field. 
    /// In case where the ContainByValue attribute used from interface or class, 
    /// all fields where the fields types is a (class or interface) with ContainByValue attribute behave as type value fields.  
    /// In both cases the type of object of field must be implement the System.ICloneable or  OOAdvantech.Transactions.TransactionalObject interface.
    ///</summary>
	/// <MetaDataID>{B9EEFD8D-C6FD-4859-BA31-227EB554AD0F}</MetaDataID>
	[AttributeUsage(AttributeTargets.Field|AttributeTargets.Class|AttributeTargets.Interface)]
    public class ContainByValue : Attribute
    {
        /// <MetaDataID>{CDD3F6D8-75F7-4F26-8B00-5652C1EFFD18}</MetaDataID>
        public ContainByValue()
        {

        }
    }

    /// <summary>
    /// Defines the way where system lock an object.
    /// </summary>
    /// <MetaDataID>{94e1ac74-e97e-4881-83ce-b99308fa5569}</MetaDataID>
    public enum LockOptions
    {
        /// <summary>
        /// In case where used for full object enlistment, 
        /// the system can enlists object partially to different transaction 
        /// only for fields which are marked as transactional shared.
        /// In case where this option used to mark a transactional member, 
        /// the object is always unlocked when enlisted partial for the marked transactional member.
        /// Except than the case where object has been enlisted full and exclusively. 
        /// </summary>
        Shared,
        /// <summary>
        /// In case where used for full object enlistment, 
        /// the object is locked for all type of enlistment to different transaction.
        /// In case where this option used to mark a transactional member, 
        /// in partial enlistment with this member, 
        /// the object is locked for full object enlistment in different transaction or 
        /// partial enlistment with the same transactional member.      
        /// </summary>
        Exclusive
    }

    /// <summary>
    /// Indicate that the class member participate with a special way.
    /// </summary>
    /// <MetaDataID>{c7c82960-37bd-46ce-9cfc-904c3c748cbf}</MetaDataID>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class TransactionalMemberAttribute : Attribute
    {

        /// <MetaDataID>{a1796d10-913d-4ebb-8239-11ecaea826eb}</MetaDataID>
        internal string ImplentationField;
        /// <MetaDataID>{99a53cd5-2cc8-4edb-82b8-7248dc9164f0}</MetaDataID>
        public LockOptions MemberTransactionLockOption;
        /// <MetaDataID>{50177bb8-5c0b-4339-a7a3-c8d73cd88976}</MetaDataID>
        public TransactionalMemberAttribute()
        {
        }
        /// <MetaDataID>{357081bb-5ebf-47d0-93dc-07838768b25d}</MetaDataID>
        System.Reflection.MemberInfo _Member;
        /// <MetaDataID>{36cdba32-45d5-433a-a040-3d9516fc6fe3}</MetaDataID>
        public System.Reflection.MemberInfo Member
        {
            get
            {

                return _Member;
            }
        }
        /// <summary>
        /// Initialize a new TransactionalMemberAttribute instance
        /// </summary>
        /// <param name="implentationField">
        /// Defines the private storage field which hold the value of property.
        /// </param>
        /// <MetaDataID>{669f14ed-be0d-4eba-a995-b702560a5646}</MetaDataID>
        public TransactionalMemberAttribute(string implentationField)
        {
            ImplentationField = implentationField;
        }

        /// <summary>
        /// Initialize a new TransactionalMemberAttribute instance
        /// </summary>
        /// <param name="memberTransactionLockOption">
        /// Defines the lock behavior of transaction system in case of partial enlistment 
        /// </param>
        /// <MetaDataID>{769a3165-8737-44d2-8582-16b94cba37a8}</MetaDataID>
        public TransactionalMemberAttribute(LockOptions memberTransactionLockOption)
        {
            MemberTransactionLockOption = memberTransactionLockOption;

        }

        /// <summary>
        /// Initialize a new TransactionalMemberAttribute instance
        /// </summary>
        /// <param name="memberTransactionLockOption">
        /// Defines the lock behavior of transaction system in case of partial enlistment 
        /// </param>
        /// <param name="implentationField">
        /// Defines the private storage field which hold the value of property.
        /// </param>
        /// <MetaDataID>{f9cfd55e-b8a5-4de9-9bdd-cf0a89f0ae0d}</MetaDataID>
        public TransactionalMemberAttribute(LockOptions memberTransactionLockOption, string implentationField)
        {
            ImplentationField = implentationField;
            MemberTransactionLockOption = memberTransactionLockOption;
        }
        /// <summary>
        /// Defines the private storage field which hold the value of property.
        /// </summary>
        /// <MetaDataID>{14cb8866-f3f3-4129-9ae6-75be24af226e}</MetaDataID>
        internal System.Reflection.FieldInfo FieldInfo;

        /// <summary>
        /// Used for fast field value access.
        /// </summary>
        /// <MetaDataID>{d8e1fb46-8d79-47ad-bbb4-ef9b5a546b97}</MetaDataID>
        internal AccessorBuilder.FieldMetadata FieldMetadata;

        /// <summary></summary>
        /// <param name="type"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        /// <MetaDataID>{be4d8620-c3d7-4bc1-8c69-7da8b4af6080}</MetaDataID>
        System.Reflection.FieldInfo InitForm(System.Type type, System.Reflection.MemberInfo member)
        {
            if (FieldInfo != null)
                return FieldInfo;
            else
            {
                if (member is System.Reflection.FieldInfo)
                {
                    FieldInfo = member as System.Reflection.FieldInfo;
                    FieldMetadata = AccessorBuilder.GetFieldMetadata(FieldInfo);
                }
                else
                {
                    FieldInfo = GetFieldInfo(type, ImplentationField);
                    if (FieldInfo ==null)
                        throw new OOAdvantech.Transactions.TransactionException("System can't find implementation field with name '"+ImplentationField+"' for property '"+member.Name+"' of type '"+ member.DeclaringType.FullName+"'.");
                    FieldMetadata = AccessorBuilder.GetFieldMetadata(FieldInfo);
                }
            }
            _Member = member;
            return FieldInfo;
        }


        /// <MetaDataID>{a5bf9ea2-4a4d-48ea-bc43-4d56d80045c3}</MetaDataID>
        private System.Reflection.FieldInfo GetFieldInfo(System.Type type, string implentationField)
        {

            System.Reflection.FieldInfo fieldInfo = type.GetMetaData().GetField(implentationField, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            if (fieldInfo != null)
                return fieldInfo;
            if (type.GetMetaData().BaseType != typeof(object))
                return GetFieldInfo(type.GetMetaData().BaseType, implentationField);
            return null;

        }

        /// <MetaDataID>{d6f687ed-b9d0-4e3e-bc60-f67b6d104d8e}</MetaDataID>
        static System.Collections.Generic.Dictionary<Type, System.Collections.Generic.Dictionary<System.Reflection.MemberInfo, TransactionalMemberAttribute>> TransactionalMembers = new System.Collections.Generic.Dictionary<Type, System.Collections.Generic.Dictionary<System.Reflection.MemberInfo, TransactionalMemberAttribute>>();

        /// <MetaDataID>{58fb70c9-5b4e-4469-917e-22df3f9db616}</MetaDataID>
        internal static TransactionalMemberAttribute GetTransactionalMember(System.Reflection.MemberInfo memberInfo, System.Type objectType)
        {
            if (objectType == null)
            {
#if DEBUG
                System.Diagnostics.Debug.Assert(false, "parameter objectType must be not null ");
#endif
                throw new ArgumentNullException("parameter objectType must be not null ");
            }

            if (memberInfo is System.Reflection.FieldInfo)
            {
                TransactionalMemberAttribute transactionalMember = memberInfo.GetCustomAttributes(typeof(TransactionalMemberAttribute), true).FirstOrDefault() as TransactionalMemberAttribute;
                transactionalMember.InitForm(objectType, memberInfo);
                return transactionalMember;
            }
            else
            {
                TransactionalMemberAttribute transactionalMember = null;
                System.Collections.Generic.Dictionary<System.Reflection.MemberInfo, TransactionalMemberAttribute> transactionalMembers = null;
                lock (TransactionalMembers)
                {
                    if (TransactionalMembers.TryGetValue(objectType, out transactionalMembers))
                        if (transactionalMembers.TryGetValue(memberInfo, out transactionalMember))
                            return transactionalMember;
                }


                if (transactionalMembers == null)
                {
                    transactionalMembers = new System.Collections.Generic.Dictionary<System.Reflection.MemberInfo, TransactionalMemberAttribute>();
                    lock (TransactionalMembers)
                        TransactionalMembers[objectType] = transactionalMembers;
                }

                transactionalMember = new TransactionalMemberAttribute(memberInfo.GetCustomAttributes(typeof(TransactionalMemberAttribute), true).FirstOrDefault() as TransactionalMemberAttribute);
                transactionalMember.InitForm(objectType, memberInfo);
                lock (TransactionalMembers)
                    TransactionalMembers[objectType][memberInfo] = transactionalMember;
                return transactionalMember;

            }

        }

        /// <MetaDataID>{c2229ffc-6937-4e39-8d18-c4ee5b2173cd}</MetaDataID>
        private TransactionalMemberAttribute(TransactionalMemberAttribute transactionalMemberAttribute)
        {
            ImplentationField = transactionalMemberAttribute.ImplentationField;
            this.MemberTransactionLockOption = transactionalMemberAttribute.MemberTransactionLockOption;
        }
    }
}
