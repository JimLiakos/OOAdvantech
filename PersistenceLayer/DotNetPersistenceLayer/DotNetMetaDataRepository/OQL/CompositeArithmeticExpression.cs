using System;
namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
}
namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    /// <MetaDataID>{b21094d6-f688-4a4f-852d-66a04159bdfc}</MetaDataID>
    /// <summary>
    /// This class describes arithmetic expression with two parts and one operator
    /// </summary>
    [Serializable]
    public class CompositeArithmeticExpression : OOAdvantech.MetaDataRepository.ObjectQueryLanguage.ArithmeticExpression
    {




        internal override ArithmeticExpression Clone(System.Collections.Generic.Dictionary<object, object> clonedObjects)
        {
            if (clonedObjects.ContainsKey(this))
                return clonedObjects[this] as ArithmeticExpression;
            var newCompositeArithmeticExpression = new CompositeArithmeticExpression();
            clonedObjects[this] = newCompositeArithmeticExpression;



            
            newCompositeArithmeticExpression._ResultType = _ResultType;
            newCompositeArithmeticExpression.Operator = Operator;
            newCompositeArithmeticExpression.Right = Right.Clone(clonedObjects);

            newCompositeArithmeticExpression.Left = Left.Clone(clonedObjects);



            return newCompositeArithmeticExpression;
        }



        /// <MetaDataID>{f2d90d03-b4cc-450d-b670-6a789a928192}</MetaDataID>
        public CompositeArithmeticExpression()
        {

        }

        /// <MetaDataID>{268fcb79-0c87-4380-84b2-cc4066133041}</MetaDataID>
        public CompositeArithmeticExpression(System.Type type)
        {
            _ResultType = type;

        }
        /// <MetaDataID>{6da032d1-8859-4e77-ac45-8ba1bb2ff0c0}</MetaDataID>
        ICalculatedValue Calculation;

        /// <MetaDataID>{030d3ab1-6f0e-45a9-afaf-4ac7ddf18e3a}</MetaDataID>
        internal override object CalculateValue(IDataRow[] compositeRow, System.Collections.Generic.Dictionary<DataNode, int> dataNodeRowIndices)
        {
            System.Type type = ResultType;
            if (Calculation == null)
            {
                switch (Operator)
                {
                    case ArithmeticOperator.Add:
                        {
                            Calculation = Add.GetTypedCalculator(type);
                            break;
                        }
                    case ArithmeticOperator.Subtract:
                        {
                            Calculation = Subtract.GetTypedCalculator(type);
                            break;
                        }
                    case ArithmeticOperator.Multiply:
                        {
                            Calculation = Multiply.GetTypedCalculator(type);
                            break;
                        }
                    case ArithmeticOperator.Divide:
                        {
                            Calculation = Divide.GetTypedCalculator(type);
                            break;
                        }
                }
            }
            return Calculation.Calculate(Left.CalculateValue(compositeRow, dataNodeRowIndices), Right.CalculateValue(compositeRow, dataNodeRowIndices));

        }

        /// <exclude>Excluded</exclude>
        System.Type _ResultType;
        /// <MetaDataID>{63dfb8c3-b9a9-4f47-8e9c-a770f6fcf64c}</MetaDataID>
        public override System.Type ResultType
        {
            get
            {
                System.Type leftType = null;
                System.Type rightType = null;

                if (_ResultType == null)
                    _ResultType = Left.ResultType;
                return _ResultType;

            }
        }


        /// <exclude>Excluded</exclude>
        System.Collections.Generic.List<DataNode> _ArithmeticExpressionDataNodes;

        /// <MetaDataID>{47965cef-cd50-4dc7-8d72-d9ca745506ce}</MetaDataID>
        public override System.Collections.Generic.List<DataNode> ArithmeticExpressionDataNodes
        {
            get
            {
                if (_ArithmeticExpressionDataNodes == null)
                {
                    _ArithmeticExpressionDataNodes = new System.Collections.Generic.List<DataNode>();
                    if (Left is CompositeArithmeticExpression)
                        _ArithmeticExpressionDataNodes.AddRange((Left as CompositeArithmeticExpression).ArithmeticExpressionDataNodes);
                    if (Right is CompositeArithmeticExpression)
                        _ArithmeticExpressionDataNodes.AddRange((Left as CompositeArithmeticExpression).ArithmeticExpressionDataNodes);
                    if (Left is ScalarFromData)
                        _ArithmeticExpressionDataNodes.Add((Left as ScalarFromData).DataNode);
                    if (Right is ScalarFromData)
                        _ArithmeticExpressionDataNodes.Add((Right as ScalarFromData).DataNode);

                }
                return _ArithmeticExpressionDataNodes;
            }
        }
        /// <MetaDataID>{9c3c2097-eca7-4c11-bf51-9f46695f5292}</MetaDataID>
        public ArithmeticOperator Operator;
        [RoleBMultiplicityRange(0)]
        [RoleAMultiplicityRange(1, 1)]
        [Association("", typeof(ArithmeticExpression), Roles.RoleA, "e1de1c67-0d06-484f-93ed-10a75317fd23")]
        [IgnoreErrorCheck]
        public ArithmeticExpression Right;
        [RoleBMultiplicityRange(0)]
        [RoleAMultiplicityRange(1, 1)]
        [Association("", typeof(ArithmeticExpression), Roles.RoleA, "e3feeb77-cd2a-4941-b2b1-3c260cf790bb")]
        [IgnoreErrorCheck]
        public ArithmeticExpression Left;
    }
    /// <MetaDataID>{691d7c2e-8837-4333-ab3d-e4a34b7e43d4}</MetaDataID>
    /// <summary>
    /// Defines the operators which can used
    /// </summary>
    public enum ArithmeticOperator
    {
        // Summary:
        //     A node that represents arithmetic addition without overflow checking.
        Add = 0,
        // Summary:
        //     A node that represents arithmetic subtraction without overflow checking.
        Subtract = 1,
        //
        // Summary:
        //     A node that represents arithmetic multiplication without overflow checking.
        Multiply = 2,
        //
        // Summary:
        //     A node that represents arithmetic division.
        Divide = 3

    }

    /// <MetaDataID>{187b811f-8048-4481-b183-c6be46692845}</MetaDataID>
    public interface ICalculation
    {
        /// <MetaDataID>{81cf4685-0732-4132-93c9-745fb96c702e}</MetaDataID>
        object Calculate(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.ArithmeticExpression left, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.ArithmeticExpression right, IDataRow[] compositeRow);
    }
    /// <MetaDataID>{4d5aa853-3c95-4ea0-a3f0-b79fe98a9385}</MetaDataID>
    interface ICalculatedValue
    {
        /// <MetaDataID>{b77062ef-fea1-4836-8555-51f24bf20131}</MetaDataID>
        void Calculate(object value);
        /// <MetaDataID>{191614db-0802-4c17-9614-4c654752f0e0}</MetaDataID>
        object Calculate(object left, object right);
        /// <MetaDataID>{4486ebaf-d8eb-4e87-a4c5-838561cb3b37}</MetaDataID>
        object Value
        {
            get;
        }
    }
    /// <MetaDataID>{1e3167e9-ab62-4bf1-ac51-92fee1b99ed4}</MetaDataID>
    class CalculatedValue<T>:ICalculatedValue
    {
        /// <MetaDataID>{14cd8b30-7d88-4aaa-8bfa-40fef17d523f}</MetaDataID>
        public CalculatedValue(CalculateHandle<T> calculate)
        {
            Calculate = calculate;
        }
        /// <MetaDataID>{08069702-fb2e-41d7-a21d-bef38ab72985}</MetaDataID>
        CalculateHandle<T> Calculate;


        /// <MetaDataID>{cd8b8d95-8e48-463a-bd44-6fa2ed81e9c9}</MetaDataID>
        T _Value=default(T);

        /// <MetaDataID>{26c2124a-b841-4019-b07c-df006e10dbf6}</MetaDataID>
        void ICalculatedValue.Calculate(object value)
        {
            _Value=Calculate(_Value,value);

        }
        /// <MetaDataID>{79a96fc7-985f-469b-8289-41b58245d856}</MetaDataID>
        object ICalculatedValue.Calculate(object left, object right)
        {
            
            return Calculate(left, right);

        }

        /// <MetaDataID>{d584d369-27ec-4679-9ec1-e6ea9199fe30}</MetaDataID>
        public object Value
        {
            get { return _Value; }
        }

        
    }
    public delegate T CalculateHandle<T>(object left, object right);
    /// <MetaDataID>{e1da805b-7947-46ea-9d57-bff0640fa9d8}</MetaDataID>
    class Add : ICalculation
    {
        /// <MetaDataID>{33e01da4-93d7-40a7-9a33-a0c7288cb9a4}</MetaDataID>
        public object Calculate(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.ArithmeticExpression left, ArithmeticExpression right, IDataRow[] compositeRow)
        {
            return 0;
            
        }

        /// <MetaDataID>{26045ace-9d21-46e6-a99e-3d7babd673f5}</MetaDataID>
        public static ICalculatedValue GetTypedCalculator(Type type)
        {
            if (type == typeof(short))
                return new CalculatedValue<short>( new CalculateHandle<short>(Calculate_Short));
            if (type == typeof(int))
                return new CalculatedValue<int>(new CalculateHandle<int>(Calculate_Int));
            if (type == typeof(long))
                return new CalculatedValue<long>(new CalculateHandle<long>(Calculate_Long));
            if (type == typeof(double))
                return new CalculatedValue<double>(new CalculateHandle<double>(Calculate_Double));
            if (type == typeof(float))
                return new CalculatedValue<float>(new CalculateHandle<float>(Calculate_Float));
            if (type == typeof(decimal))
                return new CalculatedValue<decimal>(new CalculateHandle<decimal>(Calculate_Decimal));
            throw new System.Exception("System doesn't support this type");


        }
        /// <MetaDataID>{66e63e96-c25b-43bd-98cc-925f79e895cf}</MetaDataID>
        static short Calculate_Short(short left, object right)
        {
            return (short)(left + System.Convert.ToInt16( right));
        }
        /// <MetaDataID>{534a91e9-14d3-4c6c-84dc-cd148d7c745f}</MetaDataID>
        static int Calculate_Int(int left, object right)
        {
            return ((int)left) + System.Convert.ToInt32(right);
        }
        /// <MetaDataID>{6799cd56-cf5a-4556-a0e2-6706ac27a25a}</MetaDataID>
        static long Calculate_Long(long left, object right)
        {
            return ((long)left) + System.Convert.ToInt64(right);
        }
        /// <MetaDataID>{ad990be5-73fe-4814-8f76-11741191e0e9}</MetaDataID>
        static double Calculate_Double(double left, object right)
        {
            
            return ((double)left) +System.Convert.ToDouble(right);
        }
        /// <MetaDataID>{9708dbf4-8bb1-42ce-b7cb-68284326332c}</MetaDataID>
        static float Calculate_Float(float left, object right)
        {
            return ((float)left) + System.Convert.ToSingle( right);
        }
        /// <MetaDataID>{4d02c423-82eb-4191-9969-0d77225e84cd}</MetaDataID>
        static decimal Calculate_Decimal(decimal left, object right)
        {
            return ((decimal)left) +  System.Convert.ToDecimal(right);
        }




        /// <MetaDataID>{f98dfe3b-a55a-43aa-bf13-5a38889ad76d}</MetaDataID>
        static short Calculate_Short(object left, object right)
        {
            return (short) (System.Convert.ToInt16(left) + System.Convert.ToInt16(right));
        }
        /// <MetaDataID>{5f94fcff-7ee4-4eed-93de-c01d7b298c74}</MetaDataID>
        static int Calculate_Int(object left, object right)
        {
            return System.Convert.ToInt32(left) + System.Convert.ToInt32(right);
        }
        /// <MetaDataID>{2e0b2c37-1df7-466e-921f-4f8280e92c6d}</MetaDataID>
        static long Calculate_Long(object left, object right)
        {
            return System.Convert.ToInt64(left) + System.Convert.ToInt64(right);
        }
        /// <MetaDataID>{f834405c-a96f-4427-a770-64f9235e4165}</MetaDataID>
        static double Calculate_Double(object left, object right)
        {
            return System.Convert.ToDouble(left) + System.Convert.ToDouble(right);
        }
        /// <MetaDataID>{999e4483-d67d-4d61-bcf0-4fca3a4ef8c0}</MetaDataID>
        static float Calculate_Float(object left, object right)
        {
            return System.Convert.ToSingle(left) + System.Convert.ToSingle(right);
        }
        /// <MetaDataID>{7b27ee9e-a567-4c5e-8ed3-3b2d5995d169}</MetaDataID>
        static decimal Calculate_Decimal(object left, object right)
        {
            return System.Convert.ToDecimal(left) + System.Convert.ToDecimal(right);
        }





    }
    /// <MetaDataID>{2a7981b6-d968-4b19-ae48-d06f1959fc74}</MetaDataID>
    class Subtract : ICalculation
    {
        /// <MetaDataID>{57b74e65-7061-4fcb-8643-1a974c74ab25}</MetaDataID>
        public object Calculate(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.ArithmeticExpression left, ArithmeticExpression right, IDataRow[] compositeRow)
        {
            throw new NotImplementedException();
        }
        /// <MetaDataID>{87324226-97e7-4c69-a91f-31b41ec7a7f2}</MetaDataID>
        public static ICalculatedValue GetTypedCalculator(Type type)
        {
            if (type == typeof(short))
                return new CalculatedValue<short>(new CalculateHandle<short>(Calculate_Short));
            if (type == typeof(int))
                return new CalculatedValue<int>(new CalculateHandle<int>(Calculate_Int));
            if (type == typeof(long))
                return new CalculatedValue<long>(new CalculateHandle<long>(Calculate_Long));
            if (type == typeof(double))
                return new CalculatedValue<double>(new CalculateHandle<double>(Calculate_Double));
            if (type == typeof(float))
                return new CalculatedValue<float>(new CalculateHandle<float>(Calculate_Float));
            if (type == typeof(decimal))
                return new CalculatedValue<decimal>(new CalculateHandle<decimal>(Calculate_Decimal));
            throw new System.Exception("System doesn't support this type");


        }
        /// <MetaDataID>{910fa2dd-a9b0-4185-bb91-2ee31acc0ee5}</MetaDataID>
        static short Calculate_Short(short left, object right)
        {
            return (short)(left - Convert.ToInt16(right));
        }
        /// <MetaDataID>{e686627c-1614-450b-8850-4bd758bbf44b}</MetaDataID>
        static int Calculate_Int(int left, object right)
        {
            return ((int)left) - Convert.ToInt32(right);
        }
        /// <MetaDataID>{30e2d121-ab8f-47e4-8388-df1f5209c37b}</MetaDataID>
        static long Calculate_Long(long left, object right)
        {
            return ((long)left) - Convert.ToInt64(right);
        }
        /// <MetaDataID>{3d685fba-8e4f-4b86-8652-07c57c5cd4b4}</MetaDataID>
        static double Calculate_Double(double left, object right)
        {
            return ((double)left) - Convert.ToDouble(right);
        }
        /// <MetaDataID>{32423bc6-eac8-4ff5-960a-94911181bfe4}</MetaDataID>
        static float Calculate_Float(float left, object right)
        {
            return ((float)left) - Convert.ToSingle(right);
        }
        /// <MetaDataID>{8ac916d3-52ef-4ecb-bba4-a24d739fe39c}</MetaDataID>
        static decimal Calculate_Decimal(decimal left, object right)
        {
            return ((decimal)left) - Convert.ToDecimal (right);
        }


        /// <MetaDataID>{93db29ce-6701-4b10-b365-6a9604aa8ccf}</MetaDataID>
        static short Calculate_Short(object left, object right)
        {
            return (short)(System.Convert.ToInt16(left) - System.Convert.ToInt16(right));
        }
        /// <MetaDataID>{a6a33508-369b-43c4-a9bf-a0d8cf7746e8}</MetaDataID>
        static int Calculate_Int(object left, object right)
        {
            return System.Convert.ToInt32(left) - System.Convert.ToInt32(right);
        }
        /// <MetaDataID>{e3c498e7-d408-4438-9a96-8e895f987890}</MetaDataID>
        static long Calculate_Long(object left, object right)
        {
            return System.Convert.ToInt64(left) - System.Convert.ToInt64(right);
        }
        /// <MetaDataID>{cabc5b1f-d796-407e-ab21-213faff8db46}</MetaDataID>
        static double Calculate_Double(object left, object right)
        {
            return System.Convert.ToDouble(left) - System.Convert.ToDouble(right);
        }
        /// <MetaDataID>{528a7231-9485-4f74-8e30-48d5ae4da222}</MetaDataID>
        static float Calculate_Float(object left, object right)
        {
            return System.Convert.ToSingle(left) - System.Convert.ToSingle(right);
        }
        /// <MetaDataID>{fbe8954e-6f78-452f-9cb2-7c0e402a56f7}</MetaDataID>
        static decimal Calculate_Decimal(object left, object right)
        {
            return System.Convert.ToDecimal(left) - System.Convert.ToDecimal(right);
        }

    }

    /// <MetaDataID>{96150c82-b741-4576-b22b-f55d81981f35}</MetaDataID>
    class Multiply : ICalculation
    {
        /// <MetaDataID>{7c600414-63e8-4967-a48a-36a44b4f0ce9}</MetaDataID>
        public object Calculate(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.ArithmeticExpression left, ArithmeticExpression right, IDataRow[] compositeRow)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{56f5e0ba-88b6-46ee-9b2e-4eb009f4da37}</MetaDataID>
        public static ICalculatedValue GetTypedCalculator(Type type)
        {
            if (type == typeof(short))
                return new CalculatedValue<short>(new CalculateHandle<short>(Calculate_Short));
            if (type == typeof(int))
                return new CalculatedValue<int>(new CalculateHandle<int>(Calculate_Int));
            if (type == typeof(long))
                return new CalculatedValue<long>(new CalculateHandle<long>(Calculate_Long));
            if (type == typeof(double))
                return new CalculatedValue<double>(new CalculateHandle<double>(Calculate_Double));
            if (type == typeof(float))
                return new CalculatedValue<float>(new CalculateHandle<float>(Calculate_Float));
            if (type == typeof(decimal))
                return new CalculatedValue<decimal>(new CalculateHandle<decimal>(Calculate_Decimal));
            throw new System.Exception("System doesn't support this type");


        }
        /// <MetaDataID>{1be46e13-057e-45bc-8d20-6c7e163bb0d4}</MetaDataID>
        static short Calculate_Short(short left, object right)
        {
            return (short)(left * Convert.ToInt16(right));
        }
        /// <MetaDataID>{2d427a55-2cbc-4739-afe5-8b16134cedec}</MetaDataID>
        static int Calculate_Int(int left, object right)
        {
            return ((int)left) * Convert.ToInt32(right);
        }
        /// <MetaDataID>{8ffd770a-98b3-4d5f-a497-36649c962c75}</MetaDataID>
        static long Calculate_Long(long left, object right)
        {
            return ((long)left) * Convert.ToInt64(right);
        }
        /// <MetaDataID>{bcff6549-605c-4b79-9407-826295296ad1}</MetaDataID>
        static double Calculate_Double(double left, object right)
        {
            return ((double)left) * Convert.ToDouble(right);
        }
        /// <MetaDataID>{55adf8f0-d6f7-4009-ad2d-8f7044465e93}</MetaDataID>
        static float Calculate_Float(float left, object right)
        {
            return ((float)left) * Convert.ToSingle(right);
        }
        /// <MetaDataID>{23ef3eaf-cc4a-4ab3-aaa7-65775e13513b}</MetaDataID>
        static decimal Calculate_Decimal(decimal left, object right)
        {
            return ((decimal)left) * Convert.ToDecimal(right);
        }


        /// <MetaDataID>{903f7fd5-c44a-4ab5-ae79-aa35103cacc2}</MetaDataID>
        static short Calculate_Short(object left, object right)
        {
            return (short)(System.Convert.ToInt16(left) * System.Convert.ToInt16(right));
        }
        /// <MetaDataID>{0028f25c-99b4-4856-a48a-6dd95c96efaa}</MetaDataID>
        static int Calculate_Int(object left, object right)
        {
            return System.Convert.ToInt32(left) * System.Convert.ToInt32(right);
        }
        /// <MetaDataID>{630c9ad7-50f3-4df9-a956-b4684afd928f}</MetaDataID>
        static long Calculate_Long(object left, object right)
        {
            return System.Convert.ToInt64(left) * System.Convert.ToInt64(right);
        }
        /// <MetaDataID>{3f6a4e68-84db-4c4c-a701-e1f27dc146f1}</MetaDataID>
        static double Calculate_Double(object left, object right)
        {
            return System.Convert.ToDouble(left) * System.Convert.ToDouble(right);
        }
        /// <MetaDataID>{0483b52b-3c5e-4d1f-bb00-78d90b6de953}</MetaDataID>
        static float Calculate_Float(object left, object right)
        {
            return System.Convert.ToSingle(left) * System.Convert.ToSingle(right);
        }
        /// <MetaDataID>{45105f52-6dda-44fa-a883-9c91691de8cb}</MetaDataID>
        static decimal Calculate_Decimal(object left, object right)
        {
            return System.Convert.ToDecimal(left) * System.Convert.ToDecimal(right);
        }


    }

    /// <MetaDataID>{c716acb0-2b29-4dd9-9382-4738258ce5be}</MetaDataID>
    class Divide : ICalculation
    {
        /// <MetaDataID>{652eb24b-bb65-41ef-a6a1-e6ae2b9c4d24}</MetaDataID>
        public object Calculate(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.ArithmeticExpression left, ArithmeticExpression right, IDataRow[] compositeRow)
        {
            throw new NotImplementedException();
        }
        /// <MetaDataID>{248b8643-60ed-42af-a430-ea27c3e89be5}</MetaDataID>
        public static ICalculatedValue GetTypedCalculator(Type type)
        {
            if (type == typeof(short))
                return new CalculatedValue<short>(new CalculateHandle<short>(Calculate_Short));
            if (type == typeof(int))
                return new CalculatedValue<int>(new CalculateHandle<int>(Calculate_Int));
            if (type == typeof(long))
                return new CalculatedValue<long>(new CalculateHandle<long>(Calculate_Long));
            if (type == typeof(double))
                return new CalculatedValue<double>(new CalculateHandle<double>(Calculate_Double));
            if (type == typeof(float))
                return new CalculatedValue<float>(new CalculateHandle<float>(Calculate_Float));
            if (type == typeof(decimal))
                return new CalculatedValue<decimal>(new CalculateHandle<decimal>(Calculate_Decimal));
            throw new System.Exception("System doesn't support this type");


        }

        /// <MetaDataID>{2979dc7a-6f7d-424c-b02f-1eada47c93eb}</MetaDataID>
        static short Calculate_Short(short left, object right)
        {
            return (short)(left / Convert.ToInt16(right));
        }
        /// <MetaDataID>{440163b8-ea70-4bbe-b76c-6dac8aeffc2c}</MetaDataID>
        static int Calculate_Int(int left, object right)
        {
            return ((int)left) / Convert.ToInt32(right);
        }
        /// <MetaDataID>{d5ac9916-c80e-4249-97dd-2c6f0f066ba3}</MetaDataID>
        static long Calculate_Long(long left, object right)
        {
            return ((long)left) / Convert.ToInt64(right);
        }
        /// <MetaDataID>{2193d4e2-4c1e-4d3e-8f88-972414dc311e}</MetaDataID>
        static double Calculate_Double(double left, object right)
        {
            return ((double)left) / Convert.ToDouble(right);
        }
        /// <MetaDataID>{e7d3afac-8844-4354-9d60-93ca86293af1}</MetaDataID>
        static float Calculate_Float(float left, object right)
        {
            return ((float)left) / Convert.ToSingle(right);
        }
        /// <MetaDataID>{d6ef4736-e2f5-45f9-b4fd-a83b7fc0ab8c}</MetaDataID>
        static decimal Calculate_Decimal(decimal left, object right)
        {
            return ((decimal)left) / Convert.ToDecimal(right);
        }


        /// <MetaDataID>{8e942446-d03a-4deb-af6c-be51a6632757}</MetaDataID>
        static short Calculate_Short(object left, object right)
        {
            return (short)(System.Convert.ToInt16(left) / System.Convert.ToInt16(right));
        }
        /// <MetaDataID>{01e65c48-acdb-421b-9e1e-2e2200ba7b71}</MetaDataID>
        static int Calculate_Int(object left, object right)
        {
            return System.Convert.ToInt32(left) / System.Convert.ToInt32(right);
        }
        /// <MetaDataID>{66f4c24d-b72b-47d4-b939-d7617cdd19df}</MetaDataID>
        static long Calculate_Long(object left, object right)
        {
            return System.Convert.ToInt64(left) / System.Convert.ToInt64(right);
        }
        /// <MetaDataID>{4a469025-a997-4a03-879b-0fc8a365ec4d}</MetaDataID>
        static double Calculate_Double(object left, object right)
        {
            return System.Convert.ToDouble(left) / System.Convert.ToDouble(right);
        }
        /// <MetaDataID>{89d025a4-c1ca-4d90-8d88-573636026ce5}</MetaDataID>
        static float Calculate_Float(object left, object right)
        {
            return System.Convert.ToSingle(left) / System.Convert.ToSingle(right);
        }
        /// <MetaDataID>{8e134ed0-909e-49d8-896f-a097a96a11af}</MetaDataID>
        static decimal Calculate_Decimal(object left, object right)
        {
            return System.Convert.ToDecimal(left) / System.Convert.ToDecimal(right);
        }

    }

}
