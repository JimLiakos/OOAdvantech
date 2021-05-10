using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
namespace VSMetadataRepositoryBrowser
{
    /// <MetaDataID>{6b49e504-6e1b-4660-8017-39f961be3688}</MetaDataID>
    public class ProjectTreeNode : ComponentTreeNode
    {
        /// <MetaDataID>{9217ad85-f78b-4bbb-b9bb-78ad9d0dddd5}</MetaDataID>
        OOAdvantech.CodeMetaDataRepository.Project Project;
        /// <MetaDataID>{7621f6bb-52ee-4af2-96d7-1e7d1ab690da}</MetaDataID>
        public ProjectTreeNode(OOAdvantech.CodeMetaDataRepository.Project project, MetaObjectTreeNode parent)
            : base(project,parent)
        {
            Project = project;
        }
        /// <MetaDataID>{fb8322e1-bd10-48d1-9b00-22e40291c782}</MetaDataID>
        protected override void OnMetaObjectChanged(object sender)
        {
            base.OnMetaObjectChanged(sender);
        }

        /// <MetaDataID>{bd142819-4f0e-4f99-a3ed-a9a4191e9ec0}</MetaDataID>
        Bitmap _Image;
        /// <MetaDataID>{c1a0ad1f-82e2-42e9-af5d-47e5b01f7b7a}</MetaDataID>
        public override Image Image
        {
            get
            {
                if (_Image == null)
                {

                    _Image = Resources.VSProject_genericproject;
                    _Image.MakeTransparent(Color.FromArgb(255, 0, 255));
                }
                return _Image;
            }
        }
    }

}
