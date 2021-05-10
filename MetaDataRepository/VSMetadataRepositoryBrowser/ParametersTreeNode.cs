using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.MetaDataRepository;
using System.Drawing;

namespace VSMetadataRepositoryBrowser
{
    /// <MetaDataID>{bc71a42f-2c33-489a-af7c-fd9b28b01e5f}</MetaDataID>
    public class ParametersTreeNode : MetaObjectTreeNode
    {
        /// <MetaDataID>{8a0d54bb-9caf-4313-8626-a5db52a0ac82}</MetaDataID>
        Operation Operation;
        /// <MetaDataID>{975f6d04-6c9d-4c20-8be5-c469c9a5f028}</MetaDataID>
        public ParametersTreeNode(Operation operation, MetaObjectTreeNode parent)
            : base(operation,parent)
        {
            Operation = operation;
        }

        /// <MetaDataID>{89d51ed4-8516-4069-bb25-f1f566e790a8}</MetaDataID>
        public override string Name
        {
            get
            {
                return "Parameters";
            }
        }
        /// <MetaDataID>{80603c4a-651b-4c6d-9312-c403d498b643}</MetaDataID>
        Dictionary<MetaObject, MetaObjectTreeNode> _ContainedObjects = new Dictionary<MetaObject, MetaObjectTreeNode>();
        /// <MetaDataID>{ebb4cb2a-8101-4ce5-a526-651401019ee6}</MetaDataID>
        public override List<MetaObjectTreeNode> ContainedObjects
        {
            get
            {
                List<MetaObject> removedObjects = new List<MetaObject>();
                removedObjects.AddRange(_ContainedObjects.Keys);

                foreach (Parameter parameter in Operation.Parameters)
                {
                        removedObjects.Remove(parameter);
                        if (!_ContainedObjects.ContainsKey(parameter))
                            _ContainedObjects.Add(parameter, new ParameterTreeNode(parameter,Operation,this));
                }
               
                foreach (MetaObject metaObject in removedObjects)
                    _ContainedObjects.Remove(metaObject);

                return new List<MetaObjectTreeNode>(_ContainedObjects.Values);
            }
        }

        /// <MetaDataID>{e1526956-c814-434b-8ea1-57776f9665c0}</MetaDataID>
        static Bitmap _Image;
        /// <MetaDataID>{b1813bd0-04c8-463a-b136-1ea264acd102}</MetaDataID>
        public override Image Image
        {
            get
            {
                if (_Image == null)
                {

                    _Image = Resources.VSObject_Operation_Parametrs;
                    _Image.MakeTransparent(Color.FromArgb(255, 0, 255));
                }
                return _Image;
            }
        }


    }
}
