using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.MetaDataRepository
{
    /// <MetaDataID>{FB155ED4-4B29-4c55-930C-3E14401BABC5}</MetaDataID>
    [Serializable]
    public class ValueTypePath
    {

        /// <MetaDataID>{07426d55-5d58-4ac5-9c6e-8b272a92d73e}</MetaDataID>
        System.Collections.Generic.Stack<MetaObjectID> IdentityStack = new Stack<MetaObjectID>();
        /// <MetaDataID>{436119c7-cfb1-45f0-a4ae-884ec78f9633}</MetaDataID>
        public ValueTypePath(ValueTypePath copyValueTypePath)
        {
            _ToString = copyValueTypePath._ToString;
            MetaDataRepository.MetaObjectID[] identities = copyValueTypePath.IdentityStack.ToArray();
            for (int i = 0; i != identities.Length; i++)
                IdentityStack.Push(identities[identities.Length - i - 1]);

            Multilingual = copyValueTypePath.Multilingual;

        }

     
        /// <MetaDataID>{c7457d1b-870a-4aae-b591-ad6dcb472b9b}</MetaDataID>
        public IEnumerator<MetaObjectID> GetEnumerator()
        {
            return IdentityStack.GetEnumerator();
        }

        /// <MetaDataID>{79d64521-b84f-4cae-bf3b-c62f214f521d}</MetaDataID>
        public ValueTypePath(System.Collections.Generic.ICollection<MetaDataRepository.MetaObjectID> identitiesCollection)
        {
            MetaDataRepository.MetaObjectID[] identities = new MetaObjectID[identitiesCollection.Count];
            int k = 0;
            foreach (MetaObjectID identity in identitiesCollection)
            {
                identities[k++] = identity;
            }
            for (int i = 0; i != identities.Length; i++)
            {
                IdentityStack.Push(identities[identities.Length - i - 1]);
            }
        }
        /// <MetaDataID>{4d09bba3-9312-465e-ac6c-b6dc5172a275}</MetaDataID>
        public ValueTypePath()
        {
        }
        /// <MetaDataID>{deb8bf7a-2ef6-4ac0-958a-c9308cac9452}</MetaDataID>
        string _ToString;
        internal bool Multilingual;

        /// <MetaDataID>{48198a85-bd24-4adc-be7f-86112eb61c01}</MetaDataID>
        public override string ToString()
        {
            if (_ToString == null)
            {
                string returnValue = "";
                foreach (MetaDataRepository.MetaObjectID identity in IdentityStack)
                {
                    if (!string.IsNullOrEmpty(returnValue))
                        returnValue = "." + returnValue;
                    returnValue = "(" + identity.ToString() + ")" + returnValue;
                }
                _ToString = returnValue;
            }
            return _ToString;
        }
        /// <MetaDataID>{35f87ec7-509b-4e76-b683-de6c1b4605a4}</MetaDataID>
        public ValueTypePath(string valueTypePath)
        {
           // _ToString = valueTypePath;
            int npos=valueTypePath.IndexOf(").");
            string identityStr=null;
            while(npos!=-1)
            {
                identityStr = valueTypePath.Substring(0, npos+1);
                valueTypePath = valueTypePath.Substring(npos + 2);
                if (identityStr.Length >= 2 &&
                  identityStr[0] == '(' &&
                  identityStr[identityStr.Length - 1] == ')')
                {
                    identityStr = identityStr.Substring(1, identityStr.Length - 2);
                    Push(new MetaObjectID(identityStr));
                }
                npos = valueTypePath.IndexOf(").");
            }
            //"({498CB7C2-E5B4-4a0b-9180-F93A5984112E}.6).(3396968a-9891-4233-8861-0d9fda43dad2RoleB)"
            if (npos == -1)
            {
                identityStr = valueTypePath;
                if (identityStr.Length >= 2 &&
                    identityStr[0] == '(' &&
                    identityStr[identityStr.Length - 1] == ')')
                {
                    identityStr = identityStr.Substring(1, identityStr.Length - 2);
                    Push(new MetaObjectID(identityStr));
                }
            }
            
         
        }
        /// <MetaDataID>{5806ea3d-707f-4dbd-a1c1-1aad9e85e772}</MetaDataID>
        public static ValueTypePath GetValueTypePathFromString(string valueTypePathAsString)
        {
            if (string.IsNullOrEmpty(valueTypePathAsString))
                return new ValueTypePath();
            else
            {
                ValueTypePath valueTypePath=new ValueTypePath();

                while(valueTypePathAsString.IndexOf(")")!=-1)
                {
                    valueTypePath.IdentityStack.Push(new MetaDataRepository.MetaObjectID(valueTypePathAsString.Substring(1,valueTypePathAsString.IndexOf(")")-1)));

                    if (valueTypePathAsString.Length > valueTypePathAsString.IndexOf(")") + 2)
                        valueTypePathAsString=valueTypePathAsString.Substring(valueTypePathAsString.IndexOf(")")+2);
                    else
                        break;

                }
                return valueTypePath;
            }

        }

        /// <MetaDataID>{b063fe6a-7c53-495a-a1fd-c4ac97a6104e}</MetaDataID>
        public static bool operator ==(ValueTypePath left, ValueTypePath right)
        {


            if (object.ReferenceEquals(left, null) && object.ReferenceEquals(right, null))
                return true;
            if (object.ReferenceEquals(left, null))
                return false;
            if (object.ReferenceEquals(right, null))
                return false;
            return left.ToString() == right.ToString();

        }

        /// <MetaDataID>{e01f1241-1210-416e-8083-947dabcf3b8b}</MetaDataID>
        public static bool operator !=(ValueTypePath left, ValueTypePath right)
        {
            return !(left == right);
        }




        /// <MetaDataID>{83f10ee4-9e25-47bd-8db2-a922285786ad}</MetaDataID>
        public int Count
        {
            get
            {
                return IdentityStack.Count;
            }
        }

        /// <MetaDataID>{83c290a2-ec1b-465c-a4f4-1ded4dcbf4e9}</MetaDataID>
        public MetaObjectID[] ToArray()
        {
            return IdentityStack.ToArray();
        }

        /// <MetaDataID>{e7cfc443-01bf-4b69-b05b-3edb2abaa125}</MetaDataID>
        public MetaObjectID Peek()
        {
            return IdentityStack.Peek();
        }

        /// <MetaDataID>{0021fe93-b082-45a0-89dd-5a83cf6dbdf3}</MetaDataID>
        public MetaObjectID Pop()
        {
            _ToString = null;
            return IdentityStack.Pop();
            
        }

        /// <MetaDataID>{8979bcb2-ae23-44c1-949f-80f1c4ec3037}</MetaDataID>
        public void Push(MetaObjectID metaObjectID)
        {
            _ToString = null;
            IdentityStack.Push(metaObjectID);
        }

        internal ValueTypePath Clone(Dictionary<object, object> clonedObjects)
        {
            return new ValueTypePath(this);
        }
    }


    /// <MetaDataID>{6a8a3115-d6b7-4754-aa90-ae7b5d556827}</MetaDataID>
    public struct ValueTypePathMember
    {
        /// <MetaDataID>{daae0bdd-1b99-42f1-abbe-4d9cd5fbf346}</MetaDataID>
        public readonly ValueTypePath ValueTypePath;
        /// <MetaDataID>{76988aa9-94a8-41fe-b33b-6391a247259f}</MetaDataID>
        public readonly MetaObject Member;
        /// <MetaDataID>{f83e3aa9-5bba-431e-8a4c-59f6c923a5d5}</MetaDataID>
        public readonly string Path;

        /// <MetaDataID>{451c703b-d8c2-4864-9bcf-0b529914cf62}</MetaDataID>
        public ValueTypePathMember(ValueTypePath valueTypePath,string path, MetaDataRepository.MetaObject member)
        {
            Path=path;
            ValueTypePath=valueTypePath;
            Member = member;
        }
    }
}
