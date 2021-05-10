using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.MetaDataRepository;
using System.Drawing;
namespace VSMetadataRepositoryBrowser
{
    /// <MetaDataID>{baf66231-48b8-49a0-bfb8-c5b81f5e67fc}</MetaDataID>
    public class SolutionTreeNode : MetaObjectTreeNode
    {
        /// <MetaDataID>{551a9563-774b-4f8c-a541-c89dc6d357ad}</MetaDataID>
        OOAdvantech.CodeMetaDataRepository.Solution Solution;
        /// <MetaDataID>{9d8f47d0-44d7-48a0-a24e-b3412146e5a5}</MetaDataID>
        public SolutionTreeNode(OOAdvantech.CodeMetaDataRepository.Solution solution, MetaObjectTreeNode parent)
            : base(solution,parent)
        {
            Solution = solution;
        }


        /// <MetaDataID>{db5aefa8-c868-4803-b0e2-55e107ccd236}</MetaDataID>
        Dictionary<MetaObject, MetaObjectTreeNode> _ContainedObjects = new Dictionary<MetaObject, MetaObjectTreeNode>();
        /// <MetaDataID>{488b0fd2-c44a-4b4e-a494-0269657fa48c}</MetaDataID>
        public override List<MetaObjectTreeNode> ContainedObjects
        {
            get
            {
                foreach (OOAdvantech.CodeMetaDataRepository.Project project in Solution.Projects)
                {
                    if (!_ContainedObjects.ContainsKey(project))
                        _ContainedObjects.Add(project, new ProjectTreeNode(project,this));
                }
                List<MetaObjectTreeNode> items = new List<MetaObjectTreeNode>(_ContainedObjects.Values);
                items.Sort(new MetaObjectsSort());
                return items;

                //return new List<MetaObjectTreeNode>(_ContainedObjects.Values);

            }
        }

        /// <MetaDataID>{a2f9db27-e046-405e-affe-f29ad15a6ac5}</MetaDataID>
        Bitmap _Image;
        /// <MetaDataID>{a875cfa0-dfd5-4dc4-8c76-90bb2d1f500a}</MetaDataID>
        public override Image Image
        {
            get
            {
                if (_Image == null)
                {

                    _Image = Resources.VSObject_Solution;
                    _Image.MakeTransparent(Color.FromArgb(255, 0, 255));
                }
                return _Image;
            }
        }


    }


}
