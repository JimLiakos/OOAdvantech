using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
namespace VSMetadataRepositoryBrowser
{

    /// <MetaDataID>{a7d47dd3-f65c-4810-867f-cdea28016482}</MetaDataID>
    public class DependencyTreeNode  : MetaObjectTreeNode
    {
        /// <MetaDataID>{7e71bd6d-b1d5-40f5-95b4-8e7411e5392c}</MetaDataID>
        OOAdvantech.MetaDataRepository.Dependency Dependency;
        public DependencyTreeNode(OOAdvantech.MetaDataRepository.Dependency dependency, MetaObjectTreeNode parent)
            : base(dependency, parent)
        {
            Dependency = dependency;
        }
        /// <MetaDataID>{8603e4e8-99b9-4716-b39b-5de5725eedc2}</MetaDataID>
        Bitmap _Image;
        /// <MetaDataID>{3fb8d249-f16e-4956-a87b-b57715bcf3a1}</MetaDataID>
        public override Image Image
        {
            get
            {
                if (_Image == null)
                {

                    _Image = Resources.VSProject_reference;
                    _Image.MakeTransparent(Color.FromArgb(255, 0, 255));
                }
                return _Image;
            }
        }
        /// <MetaDataID>{6ef79955-d420-4e29-b809-59ce96ba2ed7}</MetaDataID>
        public override string Name
        {
            get
            {

                return Dependency.Supplier.Name;
            }
        }
     

    }
}
