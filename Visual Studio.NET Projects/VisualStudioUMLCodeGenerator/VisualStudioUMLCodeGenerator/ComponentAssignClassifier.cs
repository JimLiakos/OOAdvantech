using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisualStudioUMLCodeGenerator
{
    public class ComponentAssignClassifier
    {
        public readonly OOAdvantech.MetaDataRepository.Classifier Classifier;
        public readonly OOAdvantech.VSUMLMetaDataRepository.Component Component;

        public ComponentAssignClassifier(OOAdvantech.VSUMLMetaDataRepository.Class @class,OOAdvantech.VSUMLMetaDataRepository.Component component)
        {
            Classifier=@class;
            Component = component;
        }

        public string Name
        {
            get
            {
                return Classifier.Name;
            }
        }

        public bool Assigned
        {
            get
            {
                return Component.Residents.Contains(Classifier);
            }
            set
            {
                if (value)
                {
                    if(!Component.Residents.Contains(Classifier))
                        Component.AddResident(Classifier);
                }
                else
                {
                     if(Component.Residents.Contains(Classifier))
                        Component.RemoveResident(Classifier);
                }
            }
        }


    }
}
