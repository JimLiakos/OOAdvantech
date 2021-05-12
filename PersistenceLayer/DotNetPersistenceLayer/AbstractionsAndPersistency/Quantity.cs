

namespace AbstractionsAndPersistency
{
    using OOAdvantech.MetaDataRepository;
    using OOAdvantech.Transactions;

    /// <MetaDataID>{0536322a-cf72-443d-8c04-2b88946b130c}</MetaDataID>
    [BackwardCompatibilityID("{0536322a-cf72-443d-8c04-2b88946b130c}"), System.Serializable, Persistent()]
    public struct Quantity :OOAdvantech.IAccount<Quantity>
    {


        /// <MetaDataID>{edce654a-2c86-4546-b326-613b33c3332d}</MetaDataID>
        public Quantity(decimal amount, AbstractionsAndPersistency.IUnitMeasure unitMeasure)
        {
            Amount = amount;
            _UnitMeasure = unitMeasure;
            //UnitMeasures = new OOAdvantech.Collections.Generic.Set<IUnitMeasure>();
        }
        /// <MetaDataID>{8fa159c0-a5b7-4143-8fa6-6054fb4bdb3c}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+1")]
        public decimal Amount;


        /// <exclude>Excluded</exclude>
        AbstractionsAndPersistency.IUnitMeasure _UnitMeasure;

        [AssociationEndBehavior( PersistencyFlag.OnConstruction| PersistencyFlag.ReferentialIntegrity)]
        [PersistentMember("_UnitMeasure")]
        [RoleBMultiplicityRange(0)]
        [RoleAMultiplicityRange(1, 1)]
        [Association("QuantityUnit", typeof(IUnitMeasure), Roles.RoleA, "3396968a-9891-4233-8861-0d9fda43dad2")]
        public AbstractionsAndPersistency.IUnitMeasure UnitMeasure
        {
            get
            {
                return _UnitMeasure;
            }
            set
            {
                _UnitMeasure = value;
            }
        }



        /// <MetaDataID>{56c1d33c-ed30-474d-8174-2f3cdf91c245}</MetaDataID>
        public static Quantity operator +(Quantity left, Quantity right)
        {
            //return left.Amount + left.UnitMeasure.Convert(right).Amount;
            return new Quantity(left.Amount + right.Amount, left.UnitMeasure);
        }

        /// <MetaDataID>{611850e7-3e2e-4aae-bf42-16e433b1f69c}</MetaDataID>
        public static Quantity operator -(Quantity left, Quantity right)
        {
            //return left.Amount - left.UnitMeasure.Convert(right).Amount;

            return new Quantity(left.Amount - right.Amount, left.UnitMeasure);
        }
        /// <MetaDataID>{ab7c9a28-e799-4af7-9bbc-b9b179557a35}</MetaDataID>
        public static Quantity operator -(Quantity left, decimal amount)
        {
            //return left.Amount - left.UnitMeasure.Convert(right).Amount;
            return new Quantity(left.Amount - amount, left._UnitMeasure);
        }

        /// <MetaDataID>{bac33bbd-d51f-45f4-9b72-b69f39d60316}</MetaDataID>
        public static Quantity operator +(Quantity left, decimal amount)
        {
            //return left.Amount + left.UnitMeasure.Convert(right).Amount;
            return new Quantity(left.Amount + amount, left._UnitMeasure);
        }


        /// <MetaDataID>{ce485441-71e8-433b-b522-da01111185d6}</MetaDataID>
        public static bool operator ==(Quantity left, Quantity right)
        {

            //if (left == null && right == null)
            //    return true;
            //if (left == null && right != null)
            //    return false;
            //if (left != null && right == null)
            //    return false;
            if (left.Amount == right.Amount)// && left.UnitMeasure == right.UnitMeasure)
                return true;
            else
                return false;
        }

        /// <MetaDataID>{dce374ba-26ee-4dd5-bbc7-dd11d3901ed2}</MetaDataID>
        public static bool operator >(Quantity left, Quantity right)
        {

            //if (left == null && right == null)
            //    return true;
            //if (left == null && right != null)
            //    return false;
            //if (left != null && right == null)
            //    return false;
            if (left.Amount > right.Amount)// && left.UnitMeasure == right.UnitMeasure)
                return true;
            else
                return false;
        }


        /// <MetaDataID>{4f1b967c-54a6-4668-ad17-9bd31166f392}</MetaDataID>
        public static bool operator <(Quantity left, Quantity right)
        {

            //if (left == null && right == null)
            //    return true;
            //if (left == null && right != null)
            //    return false;
            //if (left != null && right == null)
            //    return false;
            if (left.Amount < right.Amount)// && left.UnitMeasure == right.UnitMeasure)
                return true;
            else
                return false;
        }

        /// <MetaDataID>{93cbeb27-5500-434c-9798-3ea33ebff0a4}</MetaDataID>
        public static bool operator !=(Quantity left, Quantity right)
        {
            return !(left == right);
        }


        /// <MetaDataID>{30a0f03f-51e1-46fa-8dc3-544d8c7aef08}</MetaDataID>
        object OOAdvantech.IAccount<Quantity>.GetTransaction(Quantity newValue)
        {
            if (_UnitMeasure == null && newValue.UnitMeasure != null)
                return null;
            if (_UnitMeasure != null && newValue.UnitMeasure == null)
                return null;

            if (_UnitMeasure != newValue.UnitMeasure && !_UnitMeasure.CanConvert(newValue))
                return null;



            return this - newValue;
        }

        /// <MetaDataID>{4b036535-14fe-4d4d-9744-b6af8a6cbed8}</MetaDataID>
        Quantity OOAdvantech.IAccount<Quantity>.MakeTransaction(object transaction)
        {
            return this - ((Quantity)transaction);
        }



        /// <MetaDataID>{6f854546-4abc-43b2-b930-528fb4b2dc9d}</MetaDataID>
        public static Quantity GetQuantity(decimal amount, IUnitMeasure unitMeasure)
        {
            return new Quantity(amount, unitMeasure);
        }
    }

    /// <MetaDataID>{9449664d-3ed7-42eb-b1d1-c6f12fe28aff}</MetaDataID>
    [BackwardCompatibilityID("{0536322a-cf72-443d-8c04-2c88946b130d}"),  Persistent]
    public struct SupperQuantity
    {

        /// <MetaDataID>{6d79a174-d5e1-40f3-8e1f-3f123d8c46ea}</MetaDataID>
        public SupperQuantity(string name, Quantity quantity)
        {
            _Name = name;
            _Quantity = quantity;
        }
        /// <MetaDataID>{192bdabb-41e8-483d-8011-828dac118e26}</MetaDataID>
        string _Name;
        /// <MetaDataID>{22210e9f-7497-4ef2-9ac1-e033a527741b}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        [PersistentMember(255, "_Name")]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }



        /// <exclude>Excluded</exclude>
        private Quantity _Quantity;

        /// <MetaDataID>{59184ac8-d7d7-4e2d-94a7-b3594047d795}</MetaDataID>
        [PersistentMember("_Quantity")]
        [BackwardCompatibilityID("+2")]
        public AbstractionsAndPersistency.Quantity Quantity
        {
            get
            {
                return _Quantity;
            }
            set
            {
                if (_Quantity != value)
                {
                    using (ObjectStateTransition objStateTransition = new ObjectStateTransition(this, "Quantity"))
                    {
                        _Quantity = value;
                        objStateTransition.Consistent = true;
                    }
                }
            }
        }

    }

}
