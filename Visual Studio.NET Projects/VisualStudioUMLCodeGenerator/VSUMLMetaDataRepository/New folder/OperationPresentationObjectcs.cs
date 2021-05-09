using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.UserInterface.Runtime;

namespace OOAdvantech.VSUMLMetaDataRepository
{
    public class OperationPresentationObjectcs : PresentationObject<Operation>
    {
        public OperationPresentationObjectcs(Operation operation)
            : base(operation)
        {
            operation.Changed += new MetaDataRepository.MetaObjectChangedEventHandler(operation_Changed);
            //RefreshPersistentMember();
        }

        void operation_Changed(object sender)
        {
            
        }
    }
}
