using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Reflection;

#if PORTABLE
using System.PCL.Reflection;
#else
using System.Reflection;
#endif

#if !DeviceDotNet
using System.Reflection.Emit;
#endif
namespace OOAdvantech
{
    /// <MetaDataID>{5d838d94-13a5-441a-bfd7-3484c2f8c912}</MetaDataID>
    public static class AccessorBuilder
    {

        /// <MetaDataID>{05d638ea-8c2a-4e03-b050-f49c7f657c3c}</MetaDataID>
        public static object MethodInvoke(System.Reflection.MethodInfo methodInfo, OOAdvantech.Transactions.Transaction transaction, object obj, ref object[] parameters)
        {
            if (transaction != null)
            {
                using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(transaction))
                {
                    object retValue = methodInfo.Invoke(obj, parameters);
                    stateTransition.Consistent = true;
                    return retValue;
                }
            }
            else
            {
                return methodInfo.Invoke(obj, parameters);
            }
        }
        public class MethodInvokeState
        {
            public InvokeHandle MetodInvokeHandler;
            public OOAdvantech.Transactions.Transaction Transaction;
            public System.AsyncCallback Callback;
        }
        /// <MetaDataID>{6b04fa1d-f304-4ab1-bffe-4a1ec96ebd9c}</MetaDataID>
        public static void MethodInvokeCallback(IAsyncResult result)
        {
            var methodInvokeState = result.AsyncState as MethodInvokeState;
            if (methodInvokeState.Transaction != null)
            {

                using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(methodInvokeState.Transaction))
                {
                    methodInvokeState.Callback(result);
                    stateTransition.Consistent = true;
                }
            }
            else
                methodInvokeState.Callback(result);
        }
        public delegate object InvokeHandle(System.Reflection.MethodInfo methodInfo, OOAdvantech.Transactions.Transaction transaction, object obj, ref object[] parameters);

        internal static FastInvokeHandler GetMethodInvoker(System.Reflection.MemberInfo memberInfo)
        {
            throw new NotImplementedException();
        }


        #region Creates Dynamic Type
#if !DeviceDotNet
        /// <MetaDataID>{8b7796b3-94a1-45fc-bb0a-4bf67e3916c6}</MetaDataID>
        public static void CreateProperty(TypeBuilder typeBuilder, Type propertyType, string propertyName)
        {
            // The last argument of DefineProperty is null, because the
            // property has no parameters. (If you don't specify null, you must
            // specify an array of Type objects. For a parameterless property,
            // use an array with no elements: new Type[] {})
            PropertyBuilder custNamePropBldr = typeBuilder.DefineProperty(propertyName,
                                                             System.Reflection.PropertyAttributes.HasDefault,
                                                             propertyType,
                                                             null);

            // The property set and property get methods require a special
            // set of attributes.


            MethodAttributes getSetAttr = MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.VtableLayoutMask | MethodAttributes.Abstract | MethodAttributes.SpecialName;
            //MethodAttributes.Public | MethodAttributes.SpecialName |
            //    MethodAttributes.HideBySig | MethodAttributes.Abstract;

            // Define the "get" accessor method for CustomerName.
            MethodBuilder custNameGetPropMthdBldr =
                typeBuilder.DefineMethod("get_" + propertyName,
                                           getSetAttr,
                                           propertyType,
                                           Type.EmptyTypes);


            // Define the "set" accessor method for CustomerName.
            MethodBuilder custNameSetPropMthdBldr =
                typeBuilder.DefineMethod("set_" + propertyName,
                                           getSetAttr,
                                           null,
                                           new Type[] { propertyType });



            // Last, we must map the two methods created above to our PropertyBuilder to 
            // their corresponding behaviors, "get" and "set" respectively. 
            custNamePropBldr.SetGetMethod(custNameGetPropMthdBldr);
            custNamePropBldr.SetSetMethod(custNameSetPropMthdBldr);
        }
        /// <MetaDataID>{e23f5412-7e1a-4c73-9c6f-696c5b68310e}</MetaDataID>
        private static AssemblyBuilder assBuilder = null;
        /// <MetaDataID>{49301683-60c6-4d0d-8032-6af531ecd5d5}</MetaDataID>
        private static ModuleBuilder modBuilder = null;
        /// <MetaDataID>{b7d8d231-3ee1-460d-878e-b676a11cd96d}</MetaDataID>
        private static void GenerateAssemblyAndModule()
        {
            if (assBuilder == null)
            {

                AssemblyName draAssemblyName = new AssemblyName();
                draAssemblyName.Name = "DynamicDataRowAdapter";
                AppDomain thisDomain = System.Threading.Thread.GetDomain();
                //TODO:figure out parm list to use for isSynchronized parm = true;
                assBuilder = thisDomain.DefineDynamicAssembly(draAssemblyName, AssemblyBuilderAccess.Run);
                modBuilder = assBuilder.DefineDynamicModule(assBuilder.GetName().Name, false);

            }
        }

        /// <MetaDataID>{4e6051e3-bbe6-460d-8cdc-205f6abdc443}</MetaDataID>
        public static AssemblyBuilder CreateAssembly(string assemblyName)
        {

            AssemblyName draAssemblyName = new AssemblyName();
            draAssemblyName.Name = assemblyName;
            AppDomain thisDomain = System.Threading.Thread.GetDomain();
            AssemblyBuilder assemblyBuilder = thisDomain.DefineDynamicAssembly(draAssemblyName, AssemblyBuilderAccess.Save);
            modBuilder = assemblyBuilder.DefineDynamicModule(assemblyBuilder.GetName().Name, false);
            return assemblyBuilder;

        }
        /// <MetaDataID>{2ac28f33-32a6-4bfc-bf75-fad2320c9934}</MetaDataID>
        static public TypeBuilder GetInterfaceTypeBuilder(string interfaceName)
        {
            if (modBuilder == null)
                GenerateAssemblyAndModule();

            return modBuilder.DefineType(interfaceName, TypeAttributes.Interface | TypeAttributes.Abstract);
        }
        /// <MetaDataID>{f40bb444-0633-486b-83f4-19f67d11b3dc}</MetaDataID>
        static public TypeBuilder GetTypeBuilder(string className, Type parent, TypeAttributes attr)
        {
            if (modBuilder == null)
                GenerateAssemblyAndModule();

            return modBuilder.DefineType(className, attr, parent);
        }
        /// <MetaDataID>{f84b70b3-02ee-4c47-8d99-3ec4eff8288e}</MetaDataID>
        static public TypeBuilder GetTypeBuilder(string className, TypeAttributes attr)
        {
            if (modBuilder == null)
                GenerateAssemblyAndModule();
            return modBuilder.DefineType(className, attr);
        }


#endif
        #endregion


#if PORTABLE
        /// <MetaDataID>{a3e004fd-241c-4e4b-bf47-115d4cdb286d}</MetaDataID>
        public const BindingFlags AllMembers = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;
#else
        public const BindingFlags AllMembers = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;
#endif



        public class FieldPropertyAccessor
        {

            //internal Dictionary<object, object> ImplicitlySetValues;
            public FieldPropertyAccessor()
            {
                
            }
            public FieldPropertyAccessor(System.Reflection.MemberInfo memberInfo, bool isMember, bool initializationRequired, bool isNullable)
            {

                //CreateGetHandler((memberInfo as System.Reflection.FieldInfo).DeclaringType, memberInfo as System.Reflection.FieldInfo);
                //CreateSetHandler((memberInfo as System.Reflection.FieldInfo).DeclaringType, memberInfo as System.Reflection.FieldInfo);
                //GetValue = getHandler;
                //SetValue = setHandler;

                _GetValue = null;
                _SetValue = null;
                MemberInfo = memberInfo;
                IsMember = isMember;
                IsNullable = isNullable;
                InitializationRequired = initializationRequired;

            }
            public readonly System.Reflection.MemberInfo MemberInfo;
            GetHandler _GetValue;
            SetHandler _SetValue;
            /// <MetaDataID>{bc598d33-18df-4a12-bb03-2545e7a2e2a5}</MetaDataID>
            public GetHandler GetValue
            {
                get
                {
                    if (MemberInfo == null)
                        return null;
                    lock (MemberInfo)
                    {
                        if (_GetValue == null && MemberInfo is System.Reflection.FieldInfo)
                            _GetValue = CreateGetHandler(typeof(AccessorBuilder), MemberInfo as System.Reflection.FieldInfo);
                        if (_GetValue == null && MemberInfo is System.Reflection.PropertyInfo)
                            _GetValue = CreateGetHandler(typeof(AccessorBuilder), MemberInfo as System.Reflection.PropertyInfo);

                        return _GetValue;
                    }

                }
            }
            public SetHandler SetValue
            {
                get
                {
                    if (MemberInfo == null)
                        return null;
                    lock (MemberInfo)
                    {
                        if (_SetValue == null && MemberInfo is System.Reflection.FieldInfo)
                            _SetValue = CreateSetHandler(typeof(AccessorBuilder), MemberInfo as System.Reflection.FieldInfo);
                        if (_SetValue == null && MemberInfo is System.Reflection.PropertyInfo)
                            _SetValue = CreateSetHandler(typeof(AccessorBuilder), MemberInfo as System.Reflection.PropertyInfo);

                        return _SetValue;
                    }
                }
            }

            internal readonly bool IsMember;
            internal readonly bool InitializationRequired;
            internal readonly bool IsNullable;
        }
        /// <MetaDataID>{79426cfe-3f7c-4360-a59b-bb1afda4b71c}</MetaDataID>
        static System.Collections.Generic.Dictionary<System.Reflection.FieldInfo, FieldMetadata> FastFieldsAccessors = new Dictionary<FieldInfo, FieldMetadata>();
        /// <MetaDataID>{bee78164-fe88-4cff-82f1-e438ae105a00}</MetaDataID>
        static System.Collections.Generic.Dictionary<System.Reflection.PropertyInfo, FieldPropertyAccessor> FastPropertiesAccessors = new Dictionary<PropertyInfo, FieldPropertyAccessor>();

        /// <MetaDataID>{8c49ce94-5f32-49c5-8343-eeb031151be6}</MetaDataID>
        static System.Collections.Generic.Dictionary<System.Reflection.ConstructorInfo, FastInvokeHandler> FastConstructorInvokers = new Dictionary<ConstructorInfo, FastInvokeHandler>();


        /// <MetaDataID>{6984934e-944d-45fb-8340-0ecff07a2a0f}</MetaDataID>
        static public FieldPropertyAccessor GetFieldAccessor(System.Reflection.FieldInfo fieldInfo)
        {

            FieldPropertyAccessor fastFieldAccessor;
            FieldMetadata fieldMetadata;
            if (!FastFieldsAccessors.TryGetValue(fieldInfo, out fieldMetadata))
            {
                //fastFieldAccessor = new FieldPropertyAccessor(CreateGetHandler(fieldInfo.DeclaringType, fieldInfo), CreateSetHandler(fieldInfo.DeclaringType, fieldInfo), fieldInfo, Member<object>.IsMember(fieldInfo.FieldType), Member<object>.InitializationRequired(fieldInfo.FieldType));
                fastFieldAccessor = new FieldPropertyAccessor(fieldInfo, Member<object>.IsMember(fieldInfo.FieldType), Member<object>.InitializationRequired(fieldInfo.FieldType), Member<object>.IsNullable(fieldInfo.FieldType));
                fieldMetadata = new FieldMetadata(fieldInfo, IsContainByValueField(fieldInfo), fastFieldAccessor);
                FastFieldsAccessors[fieldInfo] = fieldMetadata;
            }
            return fieldMetadata.FieldAccessor;
        }

        /// <MetaDataID>{97d35b38-5a03-4c48-9361-c971f85885fa}</MetaDataID>
        static internal FieldMetadata GetFieldMetadata(FieldInfo fieldInfo)
        {

            FieldPropertyAccessor fastFieldAccessor;
            FieldMetadata fieldMetadata;
            if (!FastFieldsAccessors.TryGetValue(fieldInfo, out fieldMetadata))
            {
                fastFieldAccessor = new FieldPropertyAccessor(fieldInfo, Member<object>.IsMember(fieldInfo.FieldType), Member<object>.InitializationRequired(fieldInfo.FieldType), Member<object>.IsNullable(fieldInfo.FieldType));
                fieldMetadata = new FieldMetadata(fieldInfo, IsContainByValueField(fieldInfo), fastFieldAccessor);
                FastFieldsAccessors[fieldInfo] = fieldMetadata;
            }
            return fieldMetadata;
        }




        /// <MetaDataID>{2e6ddca4-8b7f-4580-b9a2-4637c4737547}</MetaDataID>
        static public FieldPropertyAccessor GetPropertyAccessor(PropertyInfo propertyInfo)
        {
            //TODO to ίδιο με το αποπάνω για ποιό γρήγορα
            lock (FastPropertiesAccessors)
            {
                FieldPropertyAccessor fastPropertyAccessor;
                FastPropertiesAccessors.TryGetValue(propertyInfo, out fastPropertyAccessor);
                if (fastPropertyAccessor == null)
                {
                    //fastPropertyAccessor = new FieldPropertyAccessor(CreateGetHandler(typeof(AccessorBuilder), propertyInfo), CreateSetHandler(typeof(AccessorBuilder), propertyInfo), propertyInfo, Member<object>.IsMember(propertyInfo.PropertyType), Member<object>.InitializationRequired(propertyInfo.PropertyType));
                    fastPropertyAccessor = new FieldPropertyAccessor(propertyInfo, Member<object>.IsMember(propertyInfo.PropertyType), Member<object>.InitializationRequired(propertyInfo.PropertyType), Member<object>.IsNullable(propertyInfo.PropertyType));
                    FastPropertiesAccessors[propertyInfo] = fastPropertyAccessor;
                }
                return fastPropertyAccessor;
            }


            //if (FastPropertiesAccessors.ContainsKey(propertyInfo))
            //    return FastPropertiesAccessors[propertyInfo];
            //else
            //{
            //    FastPropertiesAccessors[propertyInfo] = new FieldPropertyAccessor(CreateGetHandler(typeof(AccessorBuilder), propertyInfo), CreateSetHandler(typeof(AccessorBuilder), propertyInfo),Member<object>.IsMember(propertyInfo.PropertyType));
            //    return FastPropertiesAccessors[propertyInfo];
            //}
        }


        internal class TypeMetadata
        {
            public FieldMetadata[] Fields;
            public FieldPropertyAccessor ExtensionPropertiesFastFieldFieldAccessor;
            public FastInvokeHandler DefaulConstructorFastInvoke;
            public bool MonoStateClass;
            public object BaseTypeMetadata;
            public bool ExtensionMetadataLoaded;

            public FieldPropertyAccessor InitializationRequiredMember;

            internal bool InitializationRequired
            {
                get
                {
                    return InitializationRequiredMember != null;
                }
            }
        }

        internal struct FieldMetadata
        {
            public FieldMetadata(System.Reflection.FieldInfo fieldInfo, bool containByValue, FieldPropertyAccessor fieldAccessor)
                : this(fieldInfo, containByValue)
            {
                _FieldAccessor = fieldAccessor;
            }

            public FieldMetadata(System.Reflection.FieldInfo fieldInfo, bool containByValue)
            {
                FieldInfo = fieldInfo;
                ContainByValue = containByValue;
                //_FieldAccessor = new FieldPropertyAccessor(null,null,null,false,false);
                _FieldAccessor = new FieldPropertyAccessor(null, false, false, false);
                ExtensionMetadata = null;
            }
            public System.Reflection.FieldInfo FieldInfo;
            public bool ContainByValue;
            public MetaDataRepository.MetaObject ExtensionMetadata;

            public override int GetHashCode()
            {

                return FieldInfo.GetHashCode();
            }
            public override bool Equals(object obj)
            {
                if (obj is FieldMetadata)
                    return FieldInfo.Equals(((FieldMetadata)obj).FieldInfo);
                else
                    return false;
            }

            FieldPropertyAccessor _FieldAccessor;
            internal FieldPropertyAccessor FieldAccessor
            {
                get
                {
                    if (_FieldAccessor == null)
                        _FieldAccessor = GetFieldAccessor(FieldInfo);
                    return _FieldAccessor;
                }
            }

            public GetHandler GetValue
            {
                get
                {
                    return FieldAccessor.GetValue;
                }
            }
            public SetHandler SetValue
            {
                get
                {
                    return FieldAccessor.SetValue;
                }
            }

        }
        /// <MetaDataID>{0a2f243e-e65c-4105-9649-b97963ee7846}</MetaDataID>
        static System.Collections.Generic.Dictionary<System.Type, TypeMetadata> Types = new Dictionary<Type, TypeMetadata>(200);

        /// <MetaDataID>{27ca46ca-8b71-4932-9783-8b76de2fa0aa}</MetaDataID>
        static internal bool IsContainByValueField(System.Reflection.FieldInfo fieldInfo)
        {
            if (fieldInfo.FieldType.IsArray)
                return true;
            if (fieldInfo.HasCustomAttribute(typeof(OOAdvantech.Transactions.ContainByValue)) ||
                fieldInfo.FieldType.HasCustomAttribute(typeof(OOAdvantech.Transactions.ContainByValue)))
                return true;

            return false;
        }

        /// <MetaDataID>{f605b8f3-2fca-4a38-903b-34c8ff712586}</MetaDataID>
        internal static void SetTypeMetadata(System.Type type, TypeMetadata typeMetadata)
        {
            lock (Types)
            {
                Types[type] = typeMetadata;
            }

        }

        /// <MetaDataID>{cff02240-621f-4cb0-a833-4ab7195dfe8a}</MetaDataID>
        internal static TypeMetadata LoadTypeMetadata(System.Type type)
        {
            lock (Types)
            {
                //try
                //{
                TypeMetadata typeMetadata;
                if (Types.TryGetValue(type, out typeMetadata))
                    return typeMetadata;
                else
                {
#if DeviceDotNet
                    System.Reflection.FieldInfo[] objectFields = type.GetMetaData().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
#else
                    System.Reflection.FieldInfo[] objectFields = type.GetMetaData().GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
                    objectFields = objectFields.Where(x => !typeof(Delegate).IsAssignableFrom(x.FieldType)).ToArray();
                    
#endif
                    typeMetadata = new TypeMetadata();
                    typeMetadata.Fields = new FieldMetadata[objectFields.Length];
                    for (int i = 0; i != objectFields.Length; i++)
                    {

                        typeMetadata.Fields[i].FieldInfo = objectFields[i];
                        typeMetadata.Fields[i].ContainByValue = IsContainByValueField(objectFields[i]);
                        if (objectFields[i].FieldType == typeof(ObjectStateManagerLink))
                            typeMetadata.ExtensionPropertiesFastFieldFieldAccessor = AccessorBuilder.GetFieldAccessor(objectFields[i]);
                        if (typeMetadata.Fields[i].FieldAccessor.InitializationRequired)
                            typeMetadata.InitializationRequiredMember = typeMetadata.Fields[i].FieldAccessor;


                    }

                    if (type.GetMetaData().BaseType != typeof(object) && type.GetMetaData().BaseType != null)
                        typeMetadata.BaseTypeMetadata = LoadTypeMetadata(type.GetMetaData().BaseType);

                    if (typeMetadata.ExtensionPropertiesFastFieldFieldAccessor == null && type.GetMetaData().BaseType != typeof(object) && type.GetMetaData().BaseType != null)
                        typeMetadata.ExtensionPropertiesFastFieldFieldAccessor = ((TypeMetadata)typeMetadata.BaseTypeMetadata).ExtensionPropertiesFastFieldFieldAccessor;

                    if (typeMetadata.InitializationRequiredMember == null && type.GetMetaData().BaseType != typeof(object) && type.GetMetaData().BaseType != null)
                        typeMetadata.InitializationRequiredMember = ((TypeMetadata)typeMetadata.BaseTypeMetadata).InitializationRequiredMember;


                    if (!type.GetMetaData().IsAbstract && type.GetMetaData().IsClass)
                    {
#if DeviceDotNet
                        System.Reflection.ConstructorInfo constructorInfo = type.GetMetaData().GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly, new System.Type[0]);
#else
                        System.Reflection.ConstructorInfo constructorInfo = type.GetMetaData().GetConstructor(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.DeclaredOnly, new System.Type[0]);
#endif
                        if (constructorInfo != null)
                            typeMetadata.DefaulConstructorFastInvoke = AccessorBuilder.GetConstructorInvoker(constructorInfo);
                    }

                    Type baseType = type.GetMetaData().BaseType;

                    while (baseType != null && baseType != typeof(object) && baseType != typeof(OOAdvantech.Remoting.MonoStateClass))
                        baseType = baseType.GetMetaData().BaseType;
                    if (baseType == typeof(OOAdvantech.Remoting.MonoStateClass))
                        typeMetadata.MonoStateClass = true;
                    else
                        typeMetadata.MonoStateClass = false;
                    Types.Add(type, typeMetadata);
                    return typeMetadata;
                }
                //}
                //catch (System.Exception error)
                //{
                //    if(type==null)
                //        throw new System.Exception("Null type", error);

                //    throw new System.Exception(type.FullName, error);
                //}

            }
        }
        public delegate object GetHandler(object source);
        public delegate object SetHandler(object source, object value);
        public delegate object InstantiateObjectHandler();
        public delegate object FastInvokeHandler(object target, object[] paramters);

        // DynamicMethodCompiler
        //private DynamicMethodCompiler() { }

        /// <MetaDataID>{2a6667f4-dde6-4183-995f-7847a05b5cd1}</MetaDataID>
        public static FastInvokeHandler GetMethodInvoker(System.Reflection.MethodInfo methodInfo)
        {
#if !DeviceDotNet
            DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof(object), new Type[] { typeof(object), typeof(object[]) }, methodInfo.DeclaringType.Module, true);
            ILGenerator il = dynamicMethod.GetILGenerator();
            ParameterInfo[] ps = methodInfo.GetParameters();
            Type[] paramTypes = new Type[ps.Length];
            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (ps[i].ParameterType.IsByRef)
                    paramTypes[i] = ps[i].ParameterType.GetElementType();
                else
                    paramTypes[i] = ps[i].ParameterType;
            }
            LocalBuilder[] locals = new LocalBuilder[paramTypes.Length];

            for (int i = 0; i < paramTypes.Length; i++)
            {
                locals[i] = il.DeclareLocal(paramTypes[i], true);
            }
            for (int i = 0; i < paramTypes.Length; i++)
            {
                il.Emit(OpCodes.Ldarg_1);
                EmitFastInt(il, i);
                il.Emit(OpCodes.Ldelem_Ref);
                EmitCastToReference(il, paramTypes[i]);
                il.Emit(OpCodes.Stloc, locals[i]);
            }
            if (!methodInfo.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
            }
            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (ps[i].ParameterType.IsByRef)
                    il.Emit(OpCodes.Ldloca_S, locals[i]);
                else
                    il.Emit(OpCodes.Ldloc, locals[i]);
            }
            if (methodInfo.IsStatic)
                il.EmitCall(OpCodes.Call, methodInfo, null);
            else
                il.EmitCall(OpCodes.Callvirt, methodInfo, null);
            if (methodInfo.ReturnType == typeof(void))
                il.Emit(OpCodes.Ldnull);
            else
                EmitBoxIfNeeded(il, methodInfo.ReturnType);

            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (ps[i].ParameterType.IsByRef)
                {
                    il.Emit(OpCodes.Ldarg_1);
                    EmitFastInt(il, i);
                    il.Emit(OpCodes.Ldloc, locals[i]);
                    if (locals[i].LocalType.IsValueType)
                        il.Emit(OpCodes.Box, locals[i].LocalType);
                    il.Emit(OpCodes.Stelem_Ref);
                }
            }

            il.Emit(OpCodes.Ret);
            FastInvokeHandler invoder = (FastInvokeHandler)dynamicMethod.CreateDelegate(typeof(FastInvokeHandler));
            return invoder;
#else
            return (FastInvokeHandler)typeof(MethodInvokeImplementor).GetRuntimeMethod("FastInvokeHandlerImplementor", new Type[2] { typeof(object), typeof(object[]) }).CreateDelegate(typeof(FastInvokeHandler), new MethodInvokeImplementor(methodInfo));
#endif



        }

        /// <MetaDataID>{d57ce280-8cec-4107-81f8-ab67e8719ce7}</MetaDataID>
        public static FastInvokeHandler GetConstructorInvoker(ConstructorInfo constInfo)
        {

            FastInvokeHandler fastConstructorInvoker;
            if (FastConstructorInvokers.TryGetValue(constInfo, out fastConstructorInvoker))
                return fastConstructorInvoker;
            else
            {
                fastConstructorInvoker = AccessorBuilder.CreateConstructorInvoker(constInfo);
                FastConstructorInvokers[constInfo] = fastConstructorInvoker;
                return fastConstructorInvoker;

            }


        }

        /// <MetaDataID>{ec076530-8af5-4e45-b181-3e3b90bf6ed2}</MetaDataID>
        public static FastInvokeHandler CreateConstructorInvoker(ConstructorInfo constInfo)
        {
#if !DeviceDotNet
            DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof(object), new Type[] { typeof(object), typeof(object[]) }, constInfo.DeclaringType.Module, true);
            ILGenerator il = dynamicMethod.GetILGenerator();
            ParameterInfo[] ps = constInfo.GetParameters();
            Type[] paramTypes = new Type[ps.Length];
            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (ps[i].ParameterType.IsByRef)
                    paramTypes[i] = ps[i].ParameterType.GetElementType();
                else
                    paramTypes[i] = ps[i].ParameterType;
            }
            LocalBuilder[] locals = new LocalBuilder[paramTypes.Length];

            for (int i = 0; i < paramTypes.Length; i++)
            {
                locals[i] = il.DeclareLocal(paramTypes[i], true);
            }
            for (int i = 0; i < paramTypes.Length; i++)
            {
                il.Emit(OpCodes.Ldarg_1);
                EmitFastInt(il, i);
                il.Emit(OpCodes.Ldelem_Ref);
                EmitCastToReference(il, paramTypes[i]);
                il.Emit(OpCodes.Stloc, locals[i]);
            }

            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (ps[i].ParameterType.IsByRef)
                    il.Emit(OpCodes.Ldloca_S, locals[i]);
                else
                    il.Emit(OpCodes.Ldloc, locals[i]);
            }

            il.Emit(OpCodes.Newobj, constInfo);
            il.Emit(OpCodes.Ret);


            FastInvokeHandler invoder = (FastInvokeHandler)dynamicMethod.CreateDelegate(typeof(FastInvokeHandler));
            return invoder;
#else
            return (FastInvokeHandler)typeof(MethodInvokeImplementor).GetRuntimeMethod("FastInvokeHandlerImplementor", new Type[2] { typeof(object), typeof(object[]) }).CreateDelegate(typeof(FastInvokeHandler), new MethodInvokeImplementor(constInfo));

            //return (FastInvokeHandler)Delegate.CreateDelegate(typeof(FastInvokeHandler), new MethodInvokeImplementor(constInfo), typeof(MethodInvokeImplementor).GetMethod("FastInvokeHandlerImplementor"));
#endif



        }


        // CreateInstantiateObjectDelegate
        /// <MetaDataID>{d68d7d34-596e-48c2-9c33-16d3f347f9a2}</MetaDataID>
        public static InstantiateObjectHandler CreateInstantiateObjectHandler(Type type)
        {
            ConstructorInfo constructorInfo = type.GetMetaData().GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, new Type[0]);
            if (constructorInfo == null)
            {
                throw new System.Exception(string.Format("The type {0} must declare an empty constructor (the constructor may be private, internal, protected, protected internal, or public).", type));
            }
#if !DeviceDotNet

            DynamicMethod dynamicMethod = new DynamicMethod("InstantiateObject", MethodAttributes.Static | MethodAttributes.Public, CallingConventions.Standard, typeof(object), null, type, true);
            ILGenerator generator = dynamicMethod.GetILGenerator();
            generator.Emit(OpCodes.Newobj, constructorInfo);
            generator.Emit(OpCodes.Ret);
            return (InstantiateObjectHandler)dynamicMethod.CreateDelegate(typeof(InstantiateObjectHandler));
#else
            return (InstantiateObjectHandler)typeof(ObjectInstadiator).GetRuntimeMethod("InstantiateObjectHandlerImplementor", new Type[2] { typeof(object), typeof(object[]) }).CreateDelegate(typeof(InstantiateObjectHandler), new ObjectInstadiator(constructorInfo));

            //return (InstantiateObjectHandler)Delegate.CreateDelegate(typeof(InstantiateObjectHandler), new ObjectInstadiator(constructorInfo), typeof(ObjectInstadiator).GetMethod("FastInvokeHandlerImplementor"));
#endif

        }

        // CreateGetDelegate
        /// <MetaDataID>{6efe9c0f-596e-4330-a175-118197d02ed5}</MetaDataID>
        public static GetHandler CreateGetHandler(Type type, System.Reflection.PropertyInfo propertyInfo)
        {
            System.Reflection.MethodInfo getMethodInfo = null;
#if !DeviceDotNet
            getMethodInfo = propertyInfo.GetGetMethod(true);
#else
            getMethodInfo = propertyInfo.GetMethod;
#endif
            if (getMethodInfo == null)
                return null;
#if !DeviceDotNet
            DynamicMethod dynamicGet = CreateGetDynamicMethod(type);
            ILGenerator getGenerator = dynamicGet.GetILGenerator();
            LocalBuilder local = getGenerator.DeclareLocal(propertyInfo.DeclaringType);
            getGenerator.Emit(OpCodes.Ldarg_0);
            if (propertyInfo.DeclaringType.IsValueType)
            {
                //we need to cast obj to fieldInfo declarationType              
                getGenerator.Emit(OpCodes.Unbox_Any, propertyInfo.DeclaringType);
                getGenerator.Emit(OpCodes.Stloc_0);
                getGenerator.Emit(OpCodes.Ldloca_S, local);

            }
            else
            {

                //we need to cast obj to fieldInfo declarationType              
                getGenerator.Emit(OpCodes.Castclass, propertyInfo.DeclaringType);
            }

            if (getMethodInfo.IsStatic)
                getGenerator.EmitCall(OpCodes.Call, getMethodInfo, null);
            else
                getGenerator.EmitCall(OpCodes.Callvirt, getMethodInfo, null);
            BoxIfNeeded(getMethodInfo.ReturnType, getGenerator);
            getGenerator.Emit(OpCodes.Ret);

            return (GetHandler)dynamicGet.CreateDelegate(typeof(GetHandler));
#else
            return (GetHandler)typeof(FieldPropertyGetImplementor).GetRuntimeMethod("GetHandlerImplementor", new Type[1] { typeof(object) }).CreateDelegate(typeof(GetHandler), new FieldPropertyGetImplementor(propertyInfo));
#endif

        }

        // CreateGetDelegate
        /// <MetaDataID>{3817fb57-3b52-4375-a73d-c48511b1438a}</MetaDataID>
        public static GetHandler CreateGetHandler(Type type, FieldInfo fieldInfo)
        {
#if !DeviceDotNet
            DynamicMethod dynamicGet = CreateGetDynamicMethod(type);
            ILGenerator getGenerator = dynamicGet.GetILGenerator();

            getGenerator.Emit(OpCodes.Ldarg_0);
            if (fieldInfo.DeclaringType.IsValueType)
            {
                //we need to cast obj to fieldInfo declarationType              
                getGenerator.Emit(OpCodes.Unbox_Any, fieldInfo.DeclaringType);
            }
            else
            {
                //we need to cast obj to fieldInfo declarationType              
                getGenerator.Emit(OpCodes.Castclass, fieldInfo.DeclaringType);
            }
            getGenerator.Emit(OpCodes.Ldfld, fieldInfo);
            BoxIfNeeded(fieldInfo.FieldType, getGenerator);
            getGenerator.Emit(OpCodes.Ret);

            return (GetHandler)dynamicGet.CreateDelegate(typeof(GetHandler));
#else
            return (GetHandler)typeof(FieldPropertyGetImplementor).GetRuntimeMethod("GetHandlerImplementor", new Type[1] { typeof(object) }).CreateDelegate(typeof(GetHandler), new FieldPropertyGetImplementor(fieldInfo));

#endif

        }

        // CreateSetDelegate
        /// <MetaDataID>{8d1a1ee4-a687-48c0-ba96-c1894178ace4}</MetaDataID>
        public static SetHandler CreateSetHandler(Type type, PropertyInfo propertyInfo)
        {
            System.Reflection.MethodInfo setMethodInfo = null;
#if !DeviceDotNet
            setMethodInfo = propertyInfo.GetSetMethod(true);
#else
            setMethodInfo = propertyInfo.SetMethod;
#endif

            if (setMethodInfo == null)
                return null;
#if !DeviceDotNet
            DynamicMethod dynamicSet = CreateSetDynamicMethod(type);
            ILGenerator setGenerator = dynamicSet.GetILGenerator();
            LocalBuilder local = setGenerator.DeclareLocal(propertyInfo.DeclaringType);
            setGenerator.Emit(OpCodes.Ldarg_0);
            if (propertyInfo.DeclaringType.IsValueType)
            {
                //we need to cast obj to fieldInfo declarationType              
                setGenerator.Emit(OpCodes.Unbox_Any, propertyInfo.DeclaringType);
                setGenerator.Emit(OpCodes.Stloc_0);
                setGenerator.Emit(OpCodes.Ldloca_S, local);

            }
            else
            {
                //we need to cast obj to fieldInfo declarationType              
                setGenerator.Emit(OpCodes.Castclass, propertyInfo.DeclaringType);
            }
            setGenerator.Emit(OpCodes.Ldarg_1);
            UnboxIfNeeded(setMethodInfo.GetParameters()[0].ParameterType, setGenerator);
            setGenerator.Emit(OpCodes.Call, setMethodInfo);
            setGenerator.Emit(OpCodes.Ldloc_0);
            BoxIfNeeded(propertyInfo.DeclaringType, setGenerator);
            setGenerator.Emit(OpCodes.Ret);

            return (SetHandler)dynamicSet.CreateDelegate(typeof(SetHandler));
#else
            return (SetHandler)typeof(FieldPropertySetImplementor).GetRuntimeMethod("SetHandlerImplementor", new Type[2] { typeof(object), typeof(object) }).CreateDelegate(typeof(SetHandler), new FieldPropertySetImplementor(propertyInfo));
#endif

        }

        // CreateSetDelegate
        /// <MetaDataID>{9c9c1acb-26dc-4079-a61c-fcb7254e32ef}</MetaDataID>
        public static SetHandler CreateSetHandler(Type type, FieldInfo fieldInfo)
        {
#if !DeviceDotNet
            DynamicMethod dynamicSet = CreateSetDynamicMethod(type);
            ILGenerator setGenerator = dynamicSet.GetILGenerator();

            //dynamicSet.DefineParameter(1,ParameterAttributes.In|ParameterAttributes.Out,"obj");

            if (fieldInfo.DeclaringType.IsValueType)
            {
                setGenerator.DeclareLocal(fieldInfo.DeclaringType);
                setGenerator.Emit(OpCodes.Nop);
            }

            setGenerator.Emit(OpCodes.Ldarg_0);

            if (fieldInfo.DeclaringType.IsValueType)
            {
                //we need to cast obj to fieldInfo declarationType              
                setGenerator.Emit(OpCodes.Unbox_Any, fieldInfo.DeclaringType);
                setGenerator.Emit(OpCodes.Stloc_0);
                setGenerator.Emit(OpCodes.Ldloca_S, 0);
            }
            else
            {
                //we need to cast obj to fieldInfo declarationType              
                setGenerator.Emit(OpCodes.Castclass, fieldInfo.DeclaringType);
            }
            setGenerator.Emit(OpCodes.Ldarg_1);
            UnboxIfNeeded(fieldInfo.FieldType, setGenerator);
            setGenerator.Emit(OpCodes.Stfld, fieldInfo);


            if (fieldInfo.DeclaringType.IsValueType)
            {
                setGenerator.Emit(OpCodes.Ldloc_0);
                setGenerator.Emit(OpCodes.Box, fieldInfo.DeclaringType);
            }
            else
            {
                setGenerator.Emit(OpCodes.Ldarg_0);
            }


            setGenerator.Emit(OpCodes.Ret);

            return (SetHandler)dynamicSet.CreateDelegate(typeof(SetHandler));
#else
           
            return (SetHandler)typeof(FieldPropertySetImplementor).GetRuntimeMethod("SetHandlerImplementor", new Type[2] { typeof(object), typeof(object) }).CreateDelegate(typeof(SetHandler), new FieldPropertySetImplementor(fieldInfo));
#endif
        }

#if !DeviceDotNet
        // CreateGetDynamicMethod
        /// <MetaDataID>{0413dd16-2c08-405c-a1e6-5fc10790165d}</MetaDataID>
        private static DynamicMethod CreateGetDynamicMethod(Type type)
        {
            return new DynamicMethod("DynamicGet", typeof(object), new Type[] { typeof(object) }, type, true);
        }
#endif

        /// <MetaDataID>{670a55a9-02cf-4d5e-b166-aa34921cb1bc}</MetaDataID>
        static System.Type LastInstanceCreationType;
        /// <MetaDataID>{eb285c9a-569c-49d9-9641-5804fbd9696f}</MetaDataID>
        static TypeMetadata LastInstanceCreationTypeMetadata;

        /// <MetaDataID>{84011cb6-2bc1-4375-b789-838cb953d6a5}</MetaDataID>
        public static object CreateInstance(Type type)
        {

#if !DeviceDotNet
            Type baseType = type.BaseType;
            while (baseType != typeof(object) && baseType.FullName != "OOAdvantech.Remoting.MonoStateClass")
                baseType = baseType.BaseType;
            if (baseType.FullName == "OOAdvantech.Remoting.MonoStateClass")
            {
                object monoStateInsance = baseType.GetMethod("GetInstance",new Type[1]{ typeof(Type) }).Invoke(null, new object[1] { type });
                if (monoStateInsance != null)
                    return monoStateInsance;

            }
#endif

            FastInvokeHandler constructorFastInvoke = null;
            lock (FastConstructorInvokers)
            {
                if (type == LastInstanceCreationType)
                    constructorFastInvoke = LastInstanceCreationTypeMetadata.DefaulConstructorFastInvoke;
            }
            if (constructorFastInvoke != null)
                return constructorFastInvoke(null, new object[0]);

            TypeMetadata typeMedata = LoadTypeMetadata(type);
            if (typeMedata.DefaulConstructorFastInvoke == null)
                throw new System.Exception("There isn't default constructor for '" + type.FullName + "'.");

            lock (FastConstructorInvokers)
            {
                LastInstanceCreationTypeMetadata = typeMedata;
                LastInstanceCreationType = type;
            }

            return typeMedata.DefaulConstructorFastInvoke(null, new object[0]);
        }
        /// <MetaDataID>{0508b16c-7ccc-4440-a62f-e576b3257f69}</MetaDataID>
        public static object CreateInstance(Type type, Type[] paramsTypes, params object[] ctorParams)
        {
            try
            {

                Type baseType = type.GetMetaData().BaseType;
                while (baseType != typeof(object) && baseType.FullName != "OOAdvantech.Remoting.MonoStateClass")
                    baseType = baseType.GetMetaData().BaseType;
                if (baseType.FullName == "OOAdvantech.Remoting.MonoStateClass")
                {
                    object monoStateInsance = baseType.GetMetaData().GetMethod("GetInstance").Invoke(null, new object[1] { type });
                    if (monoStateInsance != null)
                        return monoStateInsance;

                }

                //TypeMetadata typeMedata = LoadTypeMetadata(type);

                //if (typeMedata.MonoStateClass)
                //{
                //    Type baseType = type.BaseType;
                //    while (baseType != typeof(object) && baseType != typeof(OOAdvantech.Remoting.MonoStateClass))
                //        baseType = baseType.BaseType;

                //    object monoStateInsance = baseType.GetMethod("GetInstance").Invoke(null, new object[1] { type });
                //    if (monoStateInsance != null)
                //        return monoStateInsance;

                //}
                if (ctorParams.Length > 0)
                {
                    object[] Params = new object[ctorParams.Length];
                    int i = 0;
                    for (i = 0; i != ctorParams.Length; i++)
                        Params[i] = ctorParams[i];
                    Type[] ParamTypes = paramsTypes;
                    if (ParamTypes == null)
                        throw new System.Exception("Missing types of parameters");
                    try
                    {
                        System.Reflection.ConstructorInfo constructorInfo = type.GetMetaData().GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, ParamTypes);
                        if (constructorInfo == null)
                            throw new System.Exception("Missing constructor with this parameters types");
                        FastInvokeHandler fastConstructorInvoker;
                        if (FastConstructorInvokers.TryGetValue(constructorInfo, out fastConstructorInvoker))
                            return fastConstructorInvoker(null, Params);
                        else
                        {
                            fastConstructorInvoker = AccessorBuilder.CreateConstructorInvoker(constructorInfo);
                            FastConstructorInvokers[constructorInfo] = fastConstructorInvoker;
                            return fastConstructorInvoker(null, Params);

                        }
                    }
                    catch (System.Reflection.AmbiguousMatchException Error)
                    {
                        throw new System.Exception("The call is ambiguous");
                    }
                }
                else
                {
                    throw new System.Exception("There aren't contractor paramters");
                }
                //else
                //{


                //    //System.Reflection.ConstructorInfo constructorInfo = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly, null, new System.Type[0], null);
                //    if (typeMedata.DefaulConstructorFastInvoke == null)
                //        throw new System.Exception("There isn't default constructor for '" + type.FullName + "'.");
                //    return typeMedata.DefaulConstructorFastInvoke(null, new object[0]);

                //    //FastInvokeHandler fastConstructorInvoker;
                //    //if (FastConstructorInvokers.TryGetValue(constructorInfo, out fastConstructorInvoker))
                //    //    return fastConstructorInvoker(null, new object[0]);
                //    //else
                //    //{
                //    //    fastConstructorInvoker = AccessorBuilder.CreateConstructorInvoker(constructorInfo);
                //    //    FastConstructorInvokers[constructorInfo] = fastConstructorInvoker;
                //    //    return fastConstructorInvoker(null, new object[0]);

                //    //}

                //}
            }
            catch (System.Exception Error)
            {
#if !DeviceDotNet
                //Error prone γεμισει με message το log file τοτε παράγει exception
                if (!System.Diagnostics.EventLog.SourceExists("PersistencySystem", "."))
                    System.Diagnostics.EventLog.CreateEventSource("PersistencySystem", "OOAdvance");
                System.Diagnostics.EventLog myLog = new System.Diagnostics.EventLog();
                myLog.Source = "PersistencySystem";
                System.Diagnostics.Debug.WriteLine(
                    Error.Message + Error.StackTrace);
                myLog.WriteEntry(Error.Message + Error.StackTrace, System.Diagnostics.EventLogEntryType.Error);
#endif
                throw new System.Exception("ModulePublisher can't create instance of '" + type.FullName + "' because :" + Error.Message + ".", Error);
            }
        }

#if !DeviceDotNet
        // CreateSetDynamicMethod
        /// <MetaDataID>{77237068-2c88-459c-be39-ecb3ed412f19}</MetaDataID>
        private static DynamicMethod CreateSetDynamicMethod(Type type)
        {

            return new DynamicMethod("DynamicSet", typeof(object), new Type[] { typeof(object), typeof(object) }, type, true);
        }

        // BoxIfNeeded
        /// <MetaDataID>{e496ca07-6183-4259-9446-fe00689732dd}</MetaDataID>
        private static void BoxIfNeeded(Type type, ILGenerator generator)
        {
            if (type.IsValueType)
            {
                generator.Emit(OpCodes.Box, type);
            }
        }

        // UnboxIfNeeded
        /// <MetaDataID>{79d53b15-c917-4945-82fa-75bae3aabeaf}</MetaDataID>
        private static void UnboxIfNeeded(Type type, ILGenerator generator)
        {
            if (type.IsValueType)
            {
                generator.Emit(OpCodes.Unbox_Any, type);
            }
        }

        /// <MetaDataID>{c23713f1-2c83-4ca4-b7fa-9984b787c855}</MetaDataID>
        private static void EmitCastToReference(ILGenerator il, System.Type type)
        {
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Unbox_Any, type);
            }
            else
            {
                il.Emit(OpCodes.Castclass, type);
            }
        }

        /// <MetaDataID>{4a1758dd-a456-4403-913b-25cda4cebe65}</MetaDataID>
        private static void EmitBoxIfNeeded(ILGenerator il, System.Type type)
        {
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Box, type);
            }
        }

        /// <MetaDataID>{180dc9bd-0e6b-4300-b18f-6c83d1e2f96a}</MetaDataID>
        private static void EmitFastInt(ILGenerator il, int value)
        {
            switch (value)
            {
                case -1:
                    il.Emit(OpCodes.Ldc_I4_M1);
                    return;
                case 0:
                    il.Emit(OpCodes.Ldc_I4_0);
                    return;
                case 1:
                    il.Emit(OpCodes.Ldc_I4_1);
                    return;
                case 2:
                    il.Emit(OpCodes.Ldc_I4_2);
                    return;
                case 3:
                    il.Emit(OpCodes.Ldc_I4_3);
                    return;
                case 4:
                    il.Emit(OpCodes.Ldc_I4_4);
                    return;
                case 5:
                    il.Emit(OpCodes.Ldc_I4_5);
                    return;
                case 6:
                    il.Emit(OpCodes.Ldc_I4_6);
                    return;
                case 7:
                    il.Emit(OpCodes.Ldc_I4_7);
                    return;
                case 8:
                    il.Emit(OpCodes.Ldc_I4_8);
                    return;
            }

            if (value > -129 && value < 128)
            {
                il.Emit(OpCodes.Ldc_I4_S, (SByte)value);
            }
            else
            {
                il.Emit(OpCodes.Ldc_I4, value);
            }
        }
#endif

        class TypeDefaultValue<T>
        {
            public static object DefaultValue
            {
                get
                {
                    return default(T);
                }
            }
        }

        /// <MetaDataID>{f3ba49a1-b7a4-44b1-bb2e-58af5e7f19db}</MetaDataID>
        static System.Collections.Generic.Dictionary<Type, object> TypeDefaultValues = new Dictionary<Type, object>();
        /// <MetaDataID>{6636c346-b12d-483d-95bb-b6770affe58c}</MetaDataID>
        public static object GetDefaultValue(System.Type type)
        {
            object value = null;
            if (!TypeDefaultValues.TryGetValue(type, out value))
            {
                try
                {
                    value = typeof(TypeDefaultValue<>).MakeGenericType(type).GetMetaData().GetProperty("DefaultValue").GetValue(null, null);
                    TypeDefaultValues[type] = value;
                }
                catch (System.Exception error)
                {

                }
            }
            return value;
        }

        internal static T Clone<T>(T cloneObject)
        {
#if DeviceDotNet

            MemoryStream ms = new MemoryStream();

            System.Runtime.Serialization.DataContractSerializer dataContractSerialize = new System.Runtime.Serialization.DataContractSerializer(typeof(T));
            dataContractSerialize.WriteObject(ms, cloneObject);
            ms.Position = 0;
            cloneObject = (T)dataContractSerialize.ReadObject(ms);
            return cloneObject;
#else

            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            // System.Collections.Generic.Dictionary<string, SearchingData> searchingData = new System.Collections.Generic.Dictionary<string, SearchingData>();
            binaryFormatter.Serialize(memoryStream, cloneObject);
            memoryStream.Position = 0;
            cloneObject = (T)binaryFormatter.Deserialize(memoryStream);
            return cloneObject;
#endif
        }


    }
}
