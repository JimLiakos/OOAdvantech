using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.VSUMLMetaDataRepository
{
    /// <MetaDataID>{b4df141a-3b56-4d02-b51a-087c35a7470e}</MetaDataID>
    public class ClassifierAssignComponent
    {
        /// <MetaDataID>{d0b0bc14-de7b-42cf-a3af-2e0d8d839dcc}</MetaDataID>
        public readonly OOAdvantech.MetaDataRepository.Classifier Classifier;
        /// <MetaDataID>{2394db0a-f321-4afc-9329-620302112036}</MetaDataID>
        public readonly OOAdvantech.VSUMLMetaDataRepository.Component Component;

        /// <MetaDataID>{7852e587-a7c3-4b30-a061-31585689fe5a}</MetaDataID>
        public ClassifierAssignComponent(OOAdvantech.MetaDataRepository.Classifier @class, OOAdvantech.VSUMLMetaDataRepository.Component component)
        {
            Classifier = @class;
            Component = component;
            Classifier.Changed += new MetaDataRepository.MetaObjectChangedEventHandler(Classifier_Changed);
        }
        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;
        void Classifier_Changed(object sender)
        {
            if (ObjectChangeState != null)
                ObjectChangeState(this, null);

        }



        /// <MetaDataID>{b45b82cf-6563-4c7d-9dbf-d02b1165407a}</MetaDataID>
        public string Name
        {
            get
            {
                return Component.Name;
            }
        }


        /// <MetaDataID>{3c594c5b-737f-4a54-939f-e821c5daf0e6}</MetaDataID>
        System.Drawing.Bitmap AssignedImage = Resource.CheckedComponent16;
        /// <MetaDataID>{27597112-beb5-457a-908b-93e790ed4bf7}</MetaDataID>
        System.Drawing.Bitmap UnAssignedImage = Resource.Component16;
        /// <MetaDataID>{92e4dbd6-89a4-40c7-8fc8-9199f1d429c6}</MetaDataID>
        public System.Drawing.Bitmap Image
        {
            get
            {
                if (Assigned)
                    return AssignedImage;
                else
                    return UnAssignedImage;

            }
        }
        //bool _Assigned;
        /// <MetaDataID>{08b91da7-2292-40fc-9473-ab3b7bd1dda9}</MetaDataID>
        public bool Assigned
        {
            get
            {
                //return _Assigned;
                return Component.Residents.Contains(Classifier);
            }
            set
            {
                //_Assigned = value;
                if (value)
                {
                    if (!Component.Residents.Contains(Classifier))
                    {
                        if (Classifier.ImplementationUnit != null)
                        {

                            Classifier.ImplementationUnit.RemoveResident(Classifier);
                            Classifier.MetaObjectChangeState();
                            

                        }

                        Component.AddResident(Classifier);
                    }
                }
                else
                {
                    if (Component.Residents.Contains(Classifier))
                        Component.RemoveResident(Classifier);
                }
            }
        }

        /// <MetaDataID>{ecc5437c-074c-4b36-b4ce-69f386bb06d2}</MetaDataID>
        public void Assign()
        {
            Assigned = true;

            if (ObjectChangeState != null)
                ObjectChangeState(this, null);
        }
        /// <MetaDataID>{55dab941-b0f1-4ea2-9607-a30e2e066444}</MetaDataID>
        public void RemoveAssignment()
        {

            Assigned = false;

            if (ObjectChangeState != null)
                ObjectChangeState(this, null);

        }

        /// <MetaDataID>{01f62ad4-62f1-4f36-ae70-da13611719db}</MetaDataID>
        public bool IsMenuCommandVisible(OOAdvantech.UserInterface.MenuCommand menuCommand)
        {
            return true;
        }

        /// <MetaDataID>{21b7e11f-b5d9-43c6-9772-1de2e400db89}</MetaDataID>
        public bool IsMenuCommandEnabled(OOAdvantech.UserInterface.MenuCommand menuCommand)
        {

            if (Assigned && menuCommand.CommandID == "947ac61d-c5ee-421a-bb12-efc8b377064a")
                return false;

            if (!Assigned && menuCommand.CommandID == "78bed44e-12a7-4c1e-bbd4-fc3e547c3bba")
                return false;

            return true;
        }

        //public void Assigngh()
        //{

        //}
    }
}
