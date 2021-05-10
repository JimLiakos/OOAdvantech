using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.MetaDataRepository;
using System.Drawing;

namespace VSMetadataRepositoryBrowser
{
    /// <MetaDataID>{38766910-5b10-4e86-8d59-cecf82f15ce6}</MetaDataID>
    public class MetaObjectTreeNode
    {

        /// <MetaDataID>{273713a4-26d9-48f8-ba29-bfe2e24ea110}</MetaDataID>
        internal MetaObject MetaObject;
        /// <MetaDataID>{dcf45acf-ed0d-4ef4-b0d5-b469a3a078e8}</MetaDataID>
        public readonly MetaObjectTreeNode Parent;
        /// <MetaDataID>{452bc4b7-a403-4440-922d-c8986799ee1b}</MetaDataID>
        protected MetaObjectTreeNode(MetaObject metaObject, MetaObjectTreeNode parent)
        {
            Parent = parent;
            MetaObject = metaObject;
            MetaObject.Changed += new MetaObjectChangedEventHandler(OnMetaObjectChanged);
        }

        /// <MetaDataID>{1fa613ae-85df-44c2-840e-537dbe76ac92}</MetaDataID>
        protected virtual void OnMetaObjectChanged(object sender)
        {
     
                ObjectChangeState?.Invoke(this, null);
        }
        /// <MetaDataID>{9b741a28-cd07-4fa5-ab4a-89f0ba593481}</MetaDataID>
        public virtual List<MetaObjectTreeNode> ContainedObjects
        {
            get
            {
                return new List<MetaObjectTreeNode>();
            }
        }

        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;


        /// <MetaDataID>{5b4615dd-da5e-471d-9ee7-7e1ef835a196}</MetaDataID>
        public virtual string Name
        {
            get
            {


                if (MetaObject is OOAdvantech.CodeMetaDataRepository.CodeElementContainer)
                {
                    return MetaObject.Name + " (" + (MetaObject as OOAdvantech.CodeMetaDataRepository.CodeElementContainer).Line.ToString() + ")";
                }
                return MetaObject.Name;
            }
        }
        /// <MetaDataID>{56e1d285-099a-45f5-ad83-00024ad4138e}</MetaDataID>
        public virtual Image Image
        {
            get
            {
                return null;
            }
        }

        internal virtual void LazyLoad()
        {
            
        }
    }




    /// <MetaDataID>{d993a74d-f4a0-4166-a1c8-70d4f6d462ee}</MetaDataID>
    class NodesLoader : MetaObjectTreeNode
    {
        public NodesLoader(MetaObject metaObject, MetaObjectTreeNode parent) : base(metaObject, parent)
        {

        }
        public override List<MetaObjectTreeNode> ContainedObjects
        {
            get
            {
                Parent.LazyLoad();
                return base.ContainedObjects;
            }
        }
    }










}
