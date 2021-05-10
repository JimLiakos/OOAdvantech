using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using OOAdvantech.UserInterface.Runtime;
using System.Reflection.Emit;
using System.ComponentModel;
using System.Collections;
using OOAdvantech.Transactions;

namespace OOAdvantech.UserInterface
{
    /// <MetaDataID>{862fcdfa-f6f0-49db-8e31-283a52dcbb3a}</MetaDataID>
    public class CodeInjection
    {

        public const string assemblyName = "CodeInjectionAssembly";

        public const string className = "TempClassInjection";


        ///// <summary>
        ///// Create a instance of our external type
        ///// </summary>
        ///// <param name="target">External type instance</param>
        ///// <param name="interfaceType">Decorate interface methods with attributes</param>
        ///// <returns>Intercepted type</returns>
        //public static object CreateProxy(object target)
        //{
        //    Type proxyType = EmiProxyType(target.GetType());

        //    DynamicUIProxy proxy = Activator.CreateInstance(proxyType, new object[] { target, target.GetType() }) as DynamicUIProxy;
        //    proxy.Target = target;

        //    return proxy;

        //}
        public static object CreateProxy(Type type, DisplayedValue target, UserInterfaceObjectConnection userInterfaceObjectConnection)
        {
            
            
            
            Type proxyType = EmiProxyType(type);
            if (target == null)
            {
                DynamicUIProxy proxy = Activator.CreateInstance(proxyType, new object[] { null, type }) as DynamicUIProxy;
                proxy.TargetType = type;
                return proxy;
            }
            else
            {
                DynamicUIProxy proxy = Activator.CreateInstance(proxyType, new object[] { target.Value, type }) as DynamicUIProxy;
                proxy.Target = target.Value;
                proxy.DisplayedValue = target;
                proxy.TargetType = type;
                proxy.UserInterfaceObjectConnection = userInterfaceObjectConnection;
                return proxy;
            }

        }

        private static TypeBuilder typeBuilder;

        private static FieldBuilder target, iface;



        static ModuleBuilder modBuilder;
        /// <summary>
        /// Generate proxy type emiting IL code.
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        public static Type EmiProxyType(Type targetType)
        {
            if (targetType == null)
                return null;
            if (targetType == typeof(object))
                return targetType;
            if (modBuilder == null)
            {
                AssemblyBuilder myAssemblyBuilder;
                // Get the current application domain for the current thread.
                AppDomain myCurrentDomain = System.Threading.Thread.GetDomain();
                AssemblyName myAssemblyName = new AssemblyName();
                myAssemblyName.Name = assemblyName;

                //Only save the custom-type dll while debugging
#if SaveDLL && DEBUG
				myAssemblyBuilder = myCurrentDomain.DefineDynamicAssembly(myAssemblyName,AssemblyBuilderAccess.RunAndSave);
			ModuleBuilder modBuilder = myAssemblyBuilder.DefineDynamicModule(className,"Test.dll");
#else
                myAssemblyBuilder = myCurrentDomain.DefineDynamicAssembly(myAssemblyName, AssemblyBuilderAccess.Run);
                modBuilder = myAssemblyBuilder.DefineDynamicModule(className);
#endif
            }

            Type type = modBuilder.GetType("DynamicUIProxies." + targetType.Namespace+ "." + targetType.Name);


            if (type == null)
            {
                typeBuilder = modBuilder.DefineType(
                    "DynamicUIProxies." + targetType.Namespace+ "." + targetType.Name,
                    TypeAttributes.Class | TypeAttributes.Public, typeof(DynamicUIProxy));

                target = typeBuilder.DefineField("target", targetType, FieldAttributes.Private);

                iface = typeBuilder.DefineField("iface", typeof(Type), FieldAttributes.Private);

                
                
                EmitConstructor(typeBuilder, target, iface);


                List<PropertyInfo> properties = new List<PropertyInfo>();
                GetProperties(targetType, properties);
                foreach (var p in properties)
                {
                    EmitProxyProperty(p, typeBuilder);

                }

                List<MethodInfo> methods = new List<MethodInfo>();
                GetMethods(targetType, methods);
                foreach (MethodInfo m in methods)
                {
                    foreach (var p in properties)
                    {
                        if (p.GetGetMethod() == m)
                            continue;

                        if (p.GetSetMethod() == m)
                            continue;
                    }
                    EmitProxyMethod(m, typeBuilder);
                }

                type = typeBuilder.CreateType();
                
                TypeDescriptor.AddAttributes(targetType, new TypeConverterAttribute(typeof(DynamicProcyConverter<>).MakeGenericType(targetType)));

            }


#if SaveDLL && DEBUG
			myAssemblyBuilder.Save("Test.dll");
#endif

            return type;
        }



        public static Type EmiProxyInterface(Type targetType)
        {
            if (targetType == null)
                return null;
            if (targetType == typeof(object))
                return targetType;
            if (modBuilder == null)
            {
                AssemblyBuilder myAssemblyBuilder;
                // Get the current application domain for the current thread.
                AppDomain myCurrentDomain = System.Threading.Thread.GetDomain();
                AssemblyName myAssemblyName = new AssemblyName();
                myAssemblyName.Name = assemblyName;

                //Only save the custom-type dll while debugging
#if SaveDLL && DEBUG
				myAssemblyBuilder = myCurrentDomain.DefineDynamicAssembly(myAssemblyName,AssemblyBuilderAccess.RunAndSave);
			ModuleBuilder modBuilder = myAssemblyBuilder.DefineDynamicModule(className,"Test.dll");
#else
                myAssemblyBuilder = myCurrentDomain.DefineDynamicAssembly(myAssemblyName, AssemblyBuilderAccess.Run);
                modBuilder = myAssemblyBuilder.DefineDynamicModule(className);
#endif
            }

            Type type = modBuilder.GetType("DynamicUIProxies." + targetType.Namespace + ".I" + targetType.Name);


            if (type == null)
            {
                typeBuilder = modBuilder.DefineType("DynamicUIProxies." + targetType.Namespace + ".I" + targetType.Name,
                                                    TypeAttributes.Interface| TypeAttributes.Public|TypeAttributes.Abstract);

             



                List<PropertyInfo> properties = new List<PropertyInfo>();
                GetProperties(targetType, properties);
                foreach (var p in properties)
                {
                    EmitProxyInterfaceProperty(p, typeBuilder);

                }

                List<MethodInfo> methods = new List<MethodInfo>();
                GetMethods(targetType, methods);
                foreach (MethodInfo m in methods)
                {
                    foreach (var p in properties)
                    {
                        if (p.GetGetMethod() == m)
                            continue;

                        if (p.GetSetMethod() == m)
                            continue;
                    }
                    EmitProxyInterfaceMethod(m, typeBuilder);
                }

                type = typeBuilder.CreateType();

                TypeDescriptor.AddAttributes(targetType, new TypeConverterAttribute(typeof(DynamicProcyConverter<>).MakeGenericType(targetType)));

            }


#if SaveDLL && DEBUG
			myAssemblyBuilder.Save("Test.dll");
#endif

            return type;
        }

        private static void GetMethods(Type targetType, List<MethodInfo> methods)
        {
            if (targetType == null || targetType == typeof(object))
                return;

            foreach (MethodInfo method in targetType.GetMethods())
            {
                if (method.DeclaringType == typeof(System.Object))
                    continue;


                bool exist = false;
                foreach (var otherTypeMethod in methods)
                {
                    if (otherTypeMethod.ToString() == method.ToString())
                        exist = true;
                }
                if (!exist)
                    methods.Add(method);
            }
            GetMethods(targetType.BaseType, methods);
        }

        private static void GetProperties(Type targetType, List<PropertyInfo> properties)
        {
            if (targetType == null)
                return;

            foreach (PropertyInfo property in targetType.GetProperties())
            {
                bool exist = false;
                foreach (var innerProprty in properties)
                {
                    if (innerProprty.ToString() == property.ToString())
                        exist = true;
                }
                if (!exist)
                    properties.Add(property);
            }
            GetProperties(targetType.BaseType, properties);

        }





        //	The methods require a special set of attributes.
        private const MethodAttributes METHOD_ATTRIBUTES =
            MethodAttributes.Private
            | MethodAttributes.HideBySig
            | MethodAttributes.NewSlot
            | MethodAttributes.Virtual
            | MethodAttributes.Final;
        //	The property get property and set methods require a special set of attributes.
        private const MethodAttributes getSetAttributes =
            METHOD_ATTRIBUTES
            | MethodAttributes.SpecialName;



        /// <summary>Implements a simple forward to the underlying adapted.</summary>
        /// <param name="propertyInfo"></param>
        /// <param name="ilGenerator"></param>
        /// <param name="typeBuilder"></param>
        protected static void EmitGetMethod(
            PropertyInfo propertyInfo,
            ILGenerator ilGenerator,
            TypeBuilder typeBuilder)
        {
            MethodInfo method = propertyInfo.GetGetMethod();

            EmitMethod(method, ilGenerator, typeBuilder);
        }

        /// <summary>Implements a simple forward to the underlying adapted.</summary>
        /// <param name="propertyInfo"></param>
        /// <param name="ilGenerator"></param>
        /// <param name="typeBuilder"></param>
        protected static void EmitSetMethod(
            PropertyInfo propertyInfo,
            ILGenerator ilGenerator,
            TypeBuilder typeBuilder)
        {
            MethodInfo method = propertyInfo.GetSetMethod();

            EmitMethod(method, ilGenerator, typeBuilder);
        }


        protected static MethodBuilder EmitProxyMethod(
         MethodInfo methodInfo,
         TypeBuilder typeBuilder)
        {
            Type[] paramTypes = Helper.GetParameterTypes(methodInfo);

            MethodBuilder mb = typeBuilder.DefineMethod(methodInfo.Name,
                MethodAttributes.Public,
                methodInfo.ReturnType,
                paramTypes);

            ILGenerator ilGenerator = mb.GetILGenerator();
            EmitMethod(methodInfo, ilGenerator, typeBuilder);
            return mb;

        }

        protected static MethodBuilder EmitProxyInterfaceMethod(
           MethodInfo methodInfo,
           TypeBuilder typeBuilder)
        {
            Type[] paramTypes = Helper.GetParameterTypes(methodInfo);

            MethodBuilder mb = typeBuilder.DefineMethod(methodInfo.Name,
                MethodAttributes.Public | MethodAttributes.Abstract| MethodAttributes.Virtual,
                methodInfo.ReturnType,
                paramTypes);

            //ILGenerator ilGenerator = mb.GetILGenerator();
            //EmitMethod(methodInfo, ilGenerator, typeBuilder);
            return mb;

        }

        /// <summary>Implements a simple forward to the underlying adapted.</summary>
        /// <param name="methodInfo"></param>
        /// <param name="ilGenerator"></param>
        /// <param name="typeBuilder"></param>
        protected static void EmitMethod(
            MethodInfo methodInfo,
            ILGenerator ilGenerator,
            TypeBuilder typeBuilder)
        {


            //	Put the dimension of the parameter array on the stack
            ilGenerator.Emit(OpCodes.Ldc_I4, methodInfo.GetParameters().Length);
            //	Create an object array
            ilGenerator.Emit(OpCodes.Newarr, typeof(object));

            LocalBuilder arrayLocal = EmitMethodParameterArray(methodInfo, ilGenerator);

            //	Put this on the stack
            ilGenerator.Emit(OpCodes.Ldarg_0);
            //	Call GetCurrentMethod on MethodBase
            ilGenerator.Emit(OpCodes.Call, typeof(MethodBase).GetMethod("GetCurrentMethod"));
            //	Push the array on the stack
            ilGenerator.Emit(OpCodes.Ldloc, arrayLocal);
            //	Call the protected CallInterceptor method
            ilGenerator.Emit(
                OpCodes.Call,
                typeof(DynamicUIProxy).GetMethod("CallInterceptor", BindingFlags.Instance | BindingFlags.Public));
            if (methodInfo.ReturnType == typeof(void))
            {	//	Remove the null returned value
                ilGenerator.Emit(OpCodes.Pop);
            }
            else if (methodInfo.ReturnType.IsValueType)
            {	//	Unbox the value
                ilGenerator.Emit(OpCodes.Unbox_Any, methodInfo.ReturnType);

            }
            GetOutValues(methodInfo, ilGenerator, arrayLocal);

            //	Return
            ilGenerator.Emit(OpCodes.Ret);
        }


        private static void GetOutValues(
        MethodInfo methodInfo,
        ILGenerator ilGenerator,
        LocalBuilder arrayLocal)
        {
            //	Gather back values of out and ref parameters
            for (int i = 0; i != methodInfo.GetParameters().Length; ++i)
            {
                ParameterInfo parameter = methodInfo.GetParameters()[i];

                if (parameter.IsOut || parameter.ParameterType.IsByRef)
                {
                    //	Put this on the stack
                    ilGenerator.Emit(OpCodes.Ldarg_0);
                    //	Push the argument on the stack
                    ilGenerator.Emit(OpCodes.Ldarg, i + 1);
                    //	Push the array on the stack
                    ilGenerator.Emit(OpCodes.Ldloc, arrayLocal);
                    //	Push the array index on the stack
                    ilGenerator.Emit(OpCodes.Ldc_I4, i);
                    //	Push the array element value on the stack
                    ilGenerator.Emit(OpCodes.Ldelem, typeof(object));

                    //	Call StoreInReference
                    MethodInfo genericMethod = typeof(DynamicUIProxy).GetMethod(
                        "StoreInReference",
                        BindingFlags.Instance | BindingFlags.Public);
                    MethodInfo method =
                        genericMethod.MakeGenericMethod(parameter.ParameterType.GetElementType());

                    ilGenerator.Emit(OpCodes.Call, method);
                }
            }
        }

        private static LocalBuilder EmitMethodParameterArray(
            MethodInfo methodInfo,
            ILGenerator ilGenerator)
        {
            //	Declare a local variTypeBuilderLibe for the array
            LocalBuilder arrayLocal = ilGenerator.DeclareLocal(typeof(object[]));

            //	Stores the array in the local variTypeBuilderLibe
            ilGenerator.Emit(OpCodes.Stloc, arrayLocal);
            //	Populate the parameter array
            for (int i = 0; i != methodInfo.GetParameters().Length; ++i)
            {
                ParameterInfo parameter = methodInfo.GetParameters()[i];

                //	Push the array on the stack
                ilGenerator.Emit(OpCodes.Ldloc, arrayLocal);
                //	Push the array index on the stack
                ilGenerator.Emit(OpCodes.Ldc_I4, i);
                if (parameter.IsOut)
                {	//	Push null on the stack (an out param shouldn't be used)
                    ilGenerator.Emit(OpCodes.Ldnull);
                }
                else
                {	//	Put this on the stack
                    ilGenerator.Emit(OpCodes.Ldarg_0);
                    //	Push the argument of the current index on the stack
                    //	The index 0 is 'this', so we need to offset by 1
                    ilGenerator.Emit(OpCodes.Ldarg, i + 1);
                    if (parameter.ParameterType.IsByRef)
                    {	//	Call GetReferenceValue
                        MethodInfo genericMethod = typeof(DynamicUIProxy).GetMethod(
                            "GetReferenceValue",
                            BindingFlags.Instance | BindingFlags.Public);
                        MethodInfo method =
                            genericMethod.MakeGenericMethod(parameter.ParameterType.GetElementType());

                        ilGenerator.Emit(OpCodes.Call, method);
                    }
                    else
                    {	//	Call GetValue
                        MethodInfo genericMethod = typeof(DynamicUIProxy).GetMethod(
                            "GetValue",
                            BindingFlags.Instance | BindingFlags.Public);
                        MethodInfo method =
                            genericMethod.MakeGenericMethod(parameter.ParameterType);

                        ilGenerator.Emit(OpCodes.Call, method);
                    }
                }
                //	Assign the parameter value to the array
                ilGenerator.Emit(OpCodes.Stelem, typeof(object));
            }

            return arrayLocal;
        }


        /// <MetaDataID>{999a5c7a-2874-4519-96f1-687c8642201a}</MetaDataID>
        private static void EmitProxyInterfaceProperty(PropertyInfo p, TypeBuilder typeBuilder)
        {
            string name = p.Name;
            Type type = p.PropertyType;
            PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(name, PropertyAttributes.None, type, null);

            MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.HideBySig;

            MethodInfo getterMethod = p.GetGetMethod();
            if (getterMethod != null)
            {
                MethodBuilder getter = EmitProxyInterfaceMethod(getterMethod, typeBuilder);
                propertyBuilder.SetGetMethod(getter);
            }

            MethodInfo setterMethod = p.GetSetMethod();
            if (setterMethod != null)
            {

                MethodBuilder setter = EmitProxyInterfaceMethod(setterMethod, typeBuilder);
                propertyBuilder.SetSetMethod(setter);
            }
        }


        private static void EmitProxyProperty(PropertyInfo p, TypeBuilder typeBuilder)
        {
            string name = p.Name;
            Type type = p.PropertyType;
            PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(name, PropertyAttributes.None, type, null);

            MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.HideBySig;

            MethodInfo getterMethod = p.GetGetMethod();
            if (getterMethod != null)
            {
                MethodBuilder getter = EmitProxyMethod(getterMethod, typeBuilder);
                propertyBuilder.SetGetMethod(getter);
            }

            MethodInfo setterMethod = p.GetSetMethod();
            if (setterMethod != null)
            {

                MethodBuilder setter = EmitProxyMethod(setterMethod, typeBuilder);
                propertyBuilder.SetSetMethod(setter);
            }
        }


        /// <summary>
        /// Generate the contructor of our proxy type
        /// </summary>
        /// <param name="typeBuilder">TypeBuilder needed to generate proxy type using IL code</param>
        /// <param name="target">Proxy type target</param>
        /// <param name="iface">Proxy type interface </param>
        private static void EmitConstructor(TypeBuilder typeBuilder, FieldBuilder target, FieldBuilder iface)
        {


            Type objType = typeof(DynamicUIProxy);
            ConstructorInfo objCtor = objType.GetConstructor(new Type[0]);

            ConstructorBuilder pointCtor = typeBuilder.DefineConstructor(
                MethodAttributes.Public,
                CallingConventions.Standard,
                new Type[] { typeof(object), typeof(Type) });
            ILGenerator ctorIL = pointCtor.GetILGenerator();


            ctorIL.Emit(OpCodes.Ldarg_0);


            ctorIL.Emit(OpCodes.Call, objCtor);

            ctorIL.Emit(OpCodes.Ldarg_0);
            ctorIL.Emit(OpCodes.Ldarg_1);
            ctorIL.Emit(OpCodes.Stfld, target);


            ctorIL.Emit(OpCodes.Ldarg_0);
            ctorIL.Emit(OpCodes.Ldarg_2);
            ctorIL.Emit(OpCodes.Stfld, iface);

            ctorIL.Emit(OpCodes.Ret);
        }
    }
    /// <MetaDataID>{496741b7-2c31-4e23-b464-2356cb0adf1f}</MetaDataID>
    public class DynamicUIProxy  : MarshalByRefObject, IPathDataDisplayer, System.ComponentModel.INotifyPropertyChanged
    {
        
        public DynamicUIProxy()
        {

        }
        static List<Type> ExcludedTypes = new List<Type>();
        public static void ExcludeType(System.Type type)
        {
            if (!ExcludedTypes.Contains(type))
                ExcludedTypes.Add(type);

        }

        static List<Assembly> ExcludedAssemblyTypes = new List<Assembly>();
        public static void ExcludeAssemblyTypes(Assembly asembly)
        {
            if (!ExcludedAssemblyTypes.Contains(asembly))
                ExcludedAssemblyTypes.Add(asembly);

        }
       
        class MethodData
        {
            public MethodInfo TargetMethod;
            public PropertyInfo TargetProperty;
            public bool PropertySetter;
            public bool PropertyGetter;
        }

        public override string ToString()
        {
            if (Target == null)
                return "";
            return Target.ToString();
        }
        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }
       
        public object Target;
        public Type TargetType;

        public object CallInterceptor(MethodBase proxyMethod, params object[] parameters)
        {
            if (Target == null)
            {
                if ((proxyMethod as MethodInfo).ReturnType != typeof(void) && (proxyMethod as MethodInfo).ReturnType != typeof(object))
                    return CodeInjection.CreateProxy((proxyMethod as MethodInfo).ReturnType, null, null);
                else
                    return null;
            }

            MethodData methodData = GetMethodData(proxyMethod);




            if (ObjectChangeStateManager == null)
                ObjectChangeStateManager = new Runtime.ObjectChangeStateManager(this);

            object ret=null;
            if (methodData.PropertyGetter)
                ret = GetPropertyValue(methodData.TargetProperty);
            else if (methodData.PropertySetter)
                SetPropertyValue(methodData.TargetProperty, parameters[0]);
            else
            {
                MethodInfo method = methodData.TargetMethod;
                ret = method.Invoke(Target, parameters);
            }
            return ret;
        }
        Dictionary<MethodBase, MethodData> MethodsData=new Dictionary<MethodBase,MethodData>(); 
        private MethodData GetMethodData(MethodBase proxyMethod)
        {
            if (MethodsData == null)
                MethodsData = new Dictionary<MethodBase, MethodData>();

            MethodData methodData =null;
            if (!MethodsData.TryGetValue(proxyMethod, out  methodData))
            {
                foreach (MethodInfo method in Target.GetType().GetMethods())
                {
                    if (method.ToString() == proxyMethod.ToString())
                    {
                        if (method.Name.IndexOf("get_") == 0)
                        {
                            PropertyInfo property = Target.GetType().GetProperty(method.Name.Substring(4));
                            if (property.GetGetMethod() == method)
                            {
                                methodData = new MethodData();
                                methodData.TargetMethod = method;
                                methodData.TargetProperty = property;
                                methodData.PropertyGetter = true;
                                MethodsData[proxyMethod] = methodData;
                                return methodData;
                            }
                        }
                        else if (method.Name.IndexOf("set_") == 0)
                        {
                            PropertyInfo property = Target.GetType().GetProperty(method.Name.Substring(4));
                            if (property.GetSetMethod() == method)
                            {
                                methodData = new MethodData();
                                methodData.TargetMethod = method;
                                methodData.TargetProperty = property;
                                methodData.PropertySetter = true;
                                MethodsData[proxyMethod] = methodData;
                                return methodData;
                            }
                        }

                        if (!MethodsData.TryGetValue(proxyMethod, out  methodData))
                        {
                            methodData = new MethodData();
                            methodData.TargetMethod = method;
                            MethodsData[proxyMethod] = methodData;
                            return methodData;
                        }

                    }
                }
                return methodData;
            }
            else return methodData;
        }

        

        private MethodInfo GetMethod(Type type, MethodBase proxyMethod)
        {
            MethodInfo m = type.GetMethod(proxyMethod.Name, Helper.GetParameterTypes(proxyMethod as MethodInfo));
            if (m == null && type.BaseType != null)
                m = GetMethod(type.BaseType, proxyMethod);
            return m;
        }

        /// <summary>Dereference a reference.</summary>
        /// <remarks>Used by emitted code, simpler than emitting.</remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="reference"></param>
        /// <returns></returns>
        public object GetReferenceValue<T>(ref T reference)
        {
            return reference;
        }

        /// <summary>Boxes a value.</summary>
        /// <remarks>Used by emitted code, simpler than emitting.</remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="reference"></param>
        /// <returns></returns>
        public object GetValue<T>(T reference)
        {
            return reference;
        }

        /// <summary>Stores a value in a reference.</summary>
        /// <remarks>Used by emitted code, simpler than emitting.</remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="reference"></param>
        /// <param name="value"></param>
        public void StoreInReference<T>(ref T reference, object value)
        {
            reference = (T)value;
        }

        public DisplayedValue DisplayedValue { get; set; }


        /// <MetaDataID>{8ff3f80e-abcc-4689-bc4c-4fceb08c8468}</MetaDataID>
        private void SetPropertyValue(System.Reflection.MemberInfo memberInfo, object value)
        {
            if (Target == null)
                return;
            UserInterfaceObjectConnection.SetValue(DisplayedValue.Value, value,MetaDataRepository.Classifier.GetClassifier(  Target.GetType()), memberInfo.Name);
        }

        ObjectChangeStateManager ObjectChangeStateManager = null;
        /// <MetaDataID>{907c35d5-0f0a-48a7-a30c-52d82ac0119e}</MetaDataID>
        private object GetPropertyValue(System.Reflection.MemberInfo memberInfo)
        {
            if (Target == null)
            {
                if (memberInfo is System.Reflection.PropertyInfo)
                    return OOAdvantech.AccessorBuilder.GetDefaultValue((memberInfo as System.Reflection.PropertyInfo).PropertyType);
                if (memberInfo is System.Reflection.FieldInfo)
                    return OOAdvantech.AccessorBuilder.GetDefaultValue((memberInfo as System.Reflection.FieldInfo).FieldType);
            }

            
            //DisplayedValue displayedValue = DisplayedValue.UserInterfaceSession.GetDisplayedValue(DisplayedValue.Value, Target.GetType(), memberInfo.Name, ObjectChangeStateManager);

            DisplayedValue displayedValue = UserInterfaceObjectConnection.GetDisplayedValue(DisplayedValue.Value, MetaDataRepository.Classifier.GetClassifier(Target.GetType()), memberInfo.Name, ObjectChangeStateManager);
            return GetWPFCompatibleValue(memberInfo, displayedValue);
        }

        private object GetWPFCompatibleValue(System.Reflection.MemberInfo memberInfo, DisplayedValue displayedValue)
        {
            if (memberInfo is System.Reflection.PropertyInfo && ((memberInfo as System.Reflection.PropertyInfo).PropertyType.IsSubclassOf(typeof(string)) ||
                (memberInfo as System.Reflection.PropertyInfo).PropertyType == typeof(string)))
                return displayedValue.Value;

            if (memberInfo is System.Reflection.PropertyInfo && ((memberInfo as System.Reflection.PropertyInfo).PropertyType.IsSubclassOf(typeof(string)) ||
              (memberInfo as System.Reflection.PropertyInfo).PropertyType.IsValueType))
                return displayedValue.Value;

            if (memberInfo is System.Reflection.FieldInfo && ((memberInfo as System.Reflection.FieldInfo).FieldType.IsSubclassOf(typeof(string)) ||
                (memberInfo as System.Reflection.FieldInfo).FieldType == typeof(string)))
                return displayedValue.Value;
            if (displayedValue.Members.ContainsKey("Items"))
            {
                Transaction transaction = UserInterfaceObjectConnection.ScoopTransaction;
                using (SystemStateTransition cancelScoopTransition = new SystemStateTransition(TransactionOption.Suppress))
                {

                    if (transaction != null)
                    {
                        using (SystemStateTransition stateTransition = new SystemStateTransition(transaction))
                        {
                            Type collectionType = (memberInfo as System.Reflection.PropertyInfo).PropertyType;
                            Type elemetnType = FindIEnumerable(collectionType);
                            if (elemetnType != null)
                            {
                                elemetnType = elemetnType.GetGenericArguments()[0];
                                if (!displayedValue.Members.ContainsKey("ObservableCollection"))
                                    AddObservableCollection(memberInfo, displayedValue, elemetnType);
                                stateTransition.Consistent = true;
                                return displayedValue.Members["ObservableCollection"][0].Value; ;
                            }
                            stateTransition.Consistent = true;
                        }
                    }
                    else
                    {
                        Type collectionType = (memberInfo as System.Reflection.PropertyInfo).PropertyType;
                        Type elemetnType = FindIEnumerable(collectionType);
                        if (elemetnType != null)
                        {
                            elemetnType = elemetnType.GetGenericArguments()[0];
                            if (!displayedValue.Members.ContainsKey("ObservableCollection"))
                                AddObservableCollection(memberInfo, displayedValue, elemetnType);
                            return displayedValue.Members["ObservableCollection"][0].Value; ;
                        }

                    }
                }
            }
            Type memperType = null;
            memperType = (memberInfo as System.Reflection.PropertyInfo).PropertyType;
            object retValue = displayedValue;
            if (!DynamicUIProxy.TypeExcluded(memperType))
                retValue = displayedValue.GetDynamicUIProxy(_UserInterfaceObjectConnection, memperType);

            if (retValue is DisplayedValue)
                retValue = (retValue as DisplayedValue).Value;
            if (retValue is UIProxy)
                retValue = (retValue as UIProxy).GetTransparentProxy();
            return retValue;
        }

        internal static bool TypeExcluded(Type memperType)
        {
            if (ExcludedAssemblyTypes.Contains(memperType.Assembly))
                return true;
            else if (ExcludedTypes.Contains(memperType))
                return true;
            else
                return false;

        }

        private void AddObservableCollection(System.Reflection.MemberInfo memberInfo, DisplayedValue displayedValue, Type elemetnType)
        {
            IList list = null;
            foreach (var item in displayedValue.Members["Items"].ValuesCollection)
            {
                object proxyItem = null;
                if (item.Value == null || item.Value is System.String || item.Value is Enum || item.Value.GetType().IsValueType)
                    proxyItem = item.Value;
                else
                    proxyItem = item.GetDynamicUIProxy(UserInterfaceObjectConnection, elemetnType);
                if (list == null)
                    list = Activator.CreateInstance(typeof(System.Collections.ObjectModel.ObservableCollection<>).MakeGenericType(proxyItem.GetType())) as IList;
                list.Add(proxyItem);
            }
            if (list == null)
            {
                list = Activator.CreateInstance(typeof(System.Collections.ObjectModel.ObservableCollection<>).MakeGenericType(elemetnType)) as IList;
            }
            displayedValue.Members["ObservableCollection"] = new Member("ObservableCollection", displayedValue, memberInfo.DeclaringType, (memberInfo as System.Reflection.PropertyInfo).PropertyType);
            DisplayedValue displayedObservableCollection = null;
            if (!displayedValue.UserInterfaceSession.TryGetDisplayedValue(list, out displayedObservableCollection))
                displayedObservableCollection = new Runtime.DisplayedValue(list, displayedValue.UserInterfaceSession);
            displayedValue.Members["ObservableCollection"][0] = displayedObservableCollection;
        }

        /// <MetaDataID>{f1fc7682-b45a-4d5e-a8be-98b6d799d926}</MetaDataID>
        internal static System.Type GetElementType(System.Type seqType)
        {
            System.Type type = FindIEnumerable(seqType);
            if (type == null)
            {
                return seqType;
            }
            return type.GetGenericArguments()[0];
        }

        /// <MetaDataID>{47d2ecba-28f1-4892-8c53-42586130c343}</MetaDataID>
        internal static System.Type FindIEnumerable(System.Type seqType)
        {
            if ((seqType != null) && (seqType != typeof(string)))
            {
                if (seqType.IsArray)
                {
                    return typeof(IEnumerable<>).MakeGenericType(new[] { seqType.GetElementType() });
                }
                if (seqType.IsGenericType)
                {
                    foreach (System.Type type in seqType.GetGenericArguments())
                    {
                        System.Type type2 = typeof(IEnumerable<>).MakeGenericType(new[] { type });
                        if (type2.IsAssignableFrom(seqType))
                        {
                            return type2;
                        }
                    }
                }
                System.Type[] interfaces = seqType.GetInterfaces();
                if ((interfaces != null) && (interfaces.Length > 0))
                {
                    foreach (System.Type type3 in interfaces)
                    {
                        System.Type type4 = FindIEnumerable(type3);
                        if (type4 != null)
                        {
                            return type4;
                        }
                    }
                }
                if ((seqType.BaseType != null) && (seqType.BaseType != typeof(object)))
                {
                    return FindIEnumerable(seqType.BaseType);
                }
            }
            return null;
        }



        public event PropertyChangedEventHandler PropertyChanged;



        #region IPathDataDisplayer Members

        public object Path
        {
            get
            {
                return null;
            }
            set
            {

            }
        }

        public void LoadControlValues()
        {

        }

        public void SaveControlValues()
        {

        }

        UserInterfaceObjectConnection _UserInterfaceObjectConnection;
        public UserInterfaceObjectConnection UserInterfaceObjectConnection
        {
            get
            {
                return _UserInterfaceObjectConnection;
            }
             set
            {
                _UserInterfaceObjectConnection = value;
            }
        }

        public Collections.Generic.List<string> Paths
        {
            get
            {
                return new Collections.Generic.List<string>();
            }
        }

        public bool HasLockRequest
        {
            get
            {
                return true;
            }
        }

        public void DisplayedValueChanged(object sender, MemberChangeEventArg change)
        {
            if (change.Member.Name=="Items"&& change.Member.Owner.Members.ContainsKey("ObservableCollection"))
            {
                if (change.Type == ChangeType.ItemsAdded)
                {
                    Type elemetnType = FindIEnumerable(change.Member.Owner.Value.GetType());
                    elemetnType= elemetnType.GetGenericArguments()[0];
                    (change.Member.Owner.Members["ObservableCollection"][0].Value as IList).Insert(change.Index, change.Value.GetDynamicUIProxy(UserInterfaceObjectConnection, elemetnType)); 
                }
                if (change.Type == ChangeType.ItemsRemoved)
                {
                    Type elemetnType = FindIEnumerable(change.Member.Owner.Value.GetType());
                    elemetnType = elemetnType.GetGenericArguments()[0];
                    (change.Member.Owner.Members["ObservableCollection"][0].Value as IList).RemoveAt(change.Index);
                }

            }
            else
            {
                ///if(change
                if (PropertyChanged != null)
                {
                    if (change.Member != null)
                        PropertyChanged(this, new PropertyChangedEventArgs(change.Member.Name));

                }
            }

        }

        public void LockStateChange(object sender)
        {

        }

        #endregion

    }

    /// <MetaDataID>{6ac6b7b9-52bd-4487-ab66-3f24919c067f}</MetaDataID>
    public class Helper
    {
        public static Type[] GetParameterTypes(MethodInfo method)
        {
            if (method == null)
                return null;
            ParameterInfo[] pIColl = method.GetParameters();

            Type[] t = new Type[pIColl.Length];


            int i = 0;
            foreach (ParameterInfo pI in pIColl)
            {
                t[i] = pI.ParameterType;
                i++;
            }

            return t;
        }


        public static MethodInfo GetMethodFromType(Type type, MethodBase methodBase)
        {
            MethodInfo method = type.GetMethod(methodBase.Name);

            return method;
        }



    }

    /// <MetaDataID>{fb66c137-26ba-4046-804d-9d86dc2a75c3}</MetaDataID>
    public class DynamicProcyConverter<T> : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(DynamicUIProxy) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context,
                System.Globalization.CultureInfo culture, object value)
        {
            
            if (value is DynamicUIProxy && (value as DynamicUIProxy).TargetType == typeof(T))
                return (value as DynamicUIProxy).Target;
            return value;// new StringList((string)value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(DynamicUIProxy) || base.CanConvertFrom(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
                System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            return null;// value == null ? null : string.Join(", ", (StringList)value);
        }
    }
}
