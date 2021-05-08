using OOAdvantech.Remoting;
#if DeviceDotNet
#if PORTABLE
using System;
#endif
#else
using System;
#endif



namespace OOAdvantech.MetaDataRepository
{


    /// <MetaDataID>{89FDC9E5-534B-40A2-AC45-062ECDEAC33B}</MetaDataID>
    /// <summary>When placed on a target end, specifies the number of target instances that may be associated with a single source instance across the given Association.</summary>
    /// <MetaDataID>{C296E8B8-561C-4552-8414-4DC819AAD44C}</MetaDataID>
    [MetaDataRepository.BackwardCompatibilityID("{89FDC9E5-534B-40A2-AC45-062ECDEAC33B}")]
    [MetaDataRepository.Persistent()]
    public class MultiplicityRange : MarshalByRefObject
    {
        public override string ToString()
        {
            if (Unspecified)
                return "*";
            if (NoHighLimit)
                return string.Format("{0}..*", LowLimit);
            return string.Format("{0}..{1}", LowLimit, HighLimit);
        }

        /// <MetaDataID>{180BB267-504B-4D01-A9C9-471232F24433}</MetaDataID>
        public bool IsMany
        {
            get
            {
                if (HighLimit == 1 && (LowLimit == 1 || LowLimit == 0))
                    return false;
                else
                    return true;
            }
        }
        /// <MetaDataID>{68C48A11-698A-4BC2-8B2A-21BEAC9CC315}</MetaDataID>
        private ObjectStateManagerLink Properties;

        /// <MetaDataID>{D6E4E74B-5A47-46AA-9B4E-0155C25A83B2}</MetaDataID>
        public MultiplicityRange(ulong lowLimit)
        {
            LowLimit = lowLimit;
            HighLimit = 0;
            NoHighLimit = true;
            Unspecified = false;
        }

        /// <MetaDataID>{2DFE27F0-C42D-475D-A38F-614111706D29}</MetaDataID>
        public MultiplicityRange(ulong lowLimit, ulong highLimit)
        {
            LowLimit = lowLimit;
            HighLimit = highLimit;
        }

        /// <MetaDataID>{ECFC4B71-DF01-49ED-8721-8D1858AA3CB6}</MetaDataID>
        public MultiplicityRange()
        {
            LowLimit = 0;
            HighLimit = 0;
            NoHighLimit = false;
            Unspecified = true;
        }
        /// <MetaDataID>{611447DD-9BC5-4469-8F6B-51BDF2FD9ABA}</MetaDataID>
        public MultiplicityRange(ulong lowLimit, ulong highLimit, bool InitNoHighLimit, bool InitUnspecified)
        {
            LowLimit = lowLimit;
            HighLimit = highLimit;
            NoHighLimit = InitNoHighLimit;
            Unspecified = InitUnspecified;
        }

        /// <MetaDataID>{91508B06-0A82-44B0-9040-D712DA8FEE5B}</MetaDataID>
        public MultiplicityRange(MultiplicityRange CopiedMultiplicityRange)
        {

            LowLimit = CopiedMultiplicityRange.LowLimit;
            HighLimit = CopiedMultiplicityRange.HighLimit;
            NoHighLimit = CopiedMultiplicityRange.NoHighLimit;
            Unspecified = CopiedMultiplicityRange.Unspecified;
        }

        /// <MetaDataID>{3086C51A-6362-42F9-98FD-215319B48832}</MetaDataID>
        [MetaDataRepository.PersistentMember]
        public ulong LowLimit;
        /// <MetaDataID>{CBCB042F-70B1-465B-BC9D-7C380F749DBC}</MetaDataID>
        [MetaDataRepository.PersistentMember]
        public ulong HighLimit;
        /// <MetaDataID>{2FCA1588-A0E6-46F0-BFFB-9CD9DC00CF65}</MetaDataID>
        [MetaDataRepository.PersistentMember]
        public bool NoHighLimit;
        /// <MetaDataID>{176C3DB3-224E-4C85-AE1A-E8B994598A89}</MetaDataID>
        [MetaDataRepository.PersistentMember]
        public bool Unspecified;
    }
}
