using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.VSUMLMetaDataRepository
{
    /// <MetaDataID>{c94ca6c7-5c90-4025-89f2-d4f6d0b2cdc8}</MetaDataID>
    public class SynchronizationComponentItem : OOAdvantech.UserInterface.Runtime.PresentationObject<OOAdvantech.MetaDataRepository.MetaObject>
    {

        bool UpdateModel;
        /// <MetaDataID>{747c6a82-5976-4dc0-82b4-86152b05c605}</MetaDataID>
        public SynchronizationComponentItem(OOAdvantech.MetaDataRepository.MetaObject metaObject,bool updateModel)
            : base(metaObject)
        {
            UpdateModel = updateModel;
            if(UpdateModel && metaObject is IVSUMLModelItemWrapper)
                _Synchronize = true;
            if (!UpdateModel && !(metaObject is IVSUMLModelItemWrapper))
                _Synchronize = true;
        }

        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;
        /// <exclude>Excluded</exclude>
        bool _Synchronize;
        public bool Synchronize
        {
            get
            {
                return _Synchronize;
            }
            set
            {
                if (_Synchronize != value)
                {
                    _Synchronize = value;
                    if (ObjectChangeState != null)
                        ObjectChangeState(this, "Synchronize");
                }
            }
        }

        /// <MetaDataID>{423c3b3d-81e5-41ae-bf7c-ba6b19e5012b}</MetaDataID>
        public string Name
        {
            get
            {
                if (RealObject is MetaDataRepository.Classifier&&(RealObject as MetaDataRepository.Classifier).IsTemplate)
                {
                    string name = RealObject.Name;
                    if (name.IndexOf("`") != -1)
                        name = name.Substring(0, name.IndexOf("`"));
                    string templateParma = null;
                    foreach (var parameter in (RealObject as MetaDataRepository.Classifier).OwnedTemplateSignature.OwnedParameters)
                    {
                        if (templateParma == null)
                            templateParma += "<";
                        else
                            templateParma += ",";
                        templateParma += parameter.Name;
                    }
                    if (templateParma != null)
                        name += templateParma + ">";
                    return name;
                }
                return RealObject.Name;
            }
        }

        public System.Drawing.Image Image
        {
            get
            {
                if (RealObject is OOAdvantech.MetaDataRepository.Class)
                {
                    switch (RealObject.Visibility)
                    {
                        case OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic:
                            {
                                return Resource.VSObject_Class;
                                break;
                            }
                        case OOAdvantech.MetaDataRepository.VisibilityKind.AccessComponent:
                            {
                                return Resource.VSObject_Class_Friend;
                                break;
                            }

                    }
                }

                if (RealObject is OOAdvantech.MetaDataRepository.Interface)
                {
                    switch (RealObject.Visibility)
                    {
                        case OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic:
                            {
                                return Resource.VSObject_Interface;
                                break;
                            }
                        case OOAdvantech.MetaDataRepository.VisibilityKind.AccessComponent:
                            {
                                return Resource.VSObject_Interface_Friend;
                                break;
                            }
                    }
                }

                if (RealObject is OOAdvantech.MetaDataRepository.Structure)
                {
                    switch (RealObject.Visibility)
                    {
                        case OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic:
                            {
                                return Resource.VSObject_Structure;
                                break;
                            }
                        case OOAdvantech.MetaDataRepository.VisibilityKind.AccessComponent:
                            {
                                return Resource.VSObject_Structure_Friend;
                                break;
                            }
                    }
                }

                if (RealObject is OOAdvantech.MetaDataRepository.Enumeration)
                {
                    switch (RealObject.Visibility)
                    {
                        case OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic:
                            {
                                return Resource.VSObject_Enum;
                                break;
                            }
                        case OOAdvantech.MetaDataRepository.VisibilityKind.AccessComponent:
                            {
                                return Resource.VSObject_Enum_Friend;
                                break;
                            }
                    }
                }

                return null;
                //if (RealObject is OOAdvantech.MetaDataRepository.Operation)
                //{

                //    OOAdvantech.MetaDataRepository.VisibilityKind visibility = (RealObject as OOAdvantech.MetaDataRepository.Operation).Visibility;//RoseAccessTypeConverter.GetVisibilityKind((RoseItem as RationalRose.RoseOperation).ExportControl.Name);
                //    if ((visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessComponent
                //        || visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessComponentOrProtected))
                //        return Properties.Resources.ImplOperation;
                //    if ((visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessPrivate))
                //        return Properties.Resources.PrivateOperation;
                //    if (visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessProtected)
                //        return Properties.Resources.ProtectedOperation;
                //    if (visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic)
                //        return Properties.Resources.PublicOperation;
                //}

                //if (RealObject is OOAdvantech.MetaDataRepository.Attribute)
                //{
                //    OOAdvantech.MetaDataRepository.VisibilityKind visibility = (RealObject as OOAdvantech.MetaDataRepository.Attribute).Visibility;// RoseAccessTypeConverter.GetVisibilityKind((RoseItem as RationalRose.RoseAttribute).ExportControl.Name);
                //    if ((visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessComponent
                //        || visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessComponentOrProtected))
                //        return Properties.Resources.ImplAttr;
                //    if ((visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessPrivate))
                //        return Properties.Resources.PrivateAttribute;
                //    if (visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessProtected)
                //        return Properties.Resources.PublicAttribute;
                //    if (visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic)
                //        return Properties.Resources.PublicAttribute;

                //}
                //if (RealObject is OOAdvantech.MetaDataRepository.Method)
                //{

                //    OOAdvantech.MetaDataRepository.VisibilityKind visibility = (RealObject as OOAdvantech.MetaDataRepository.Method).Visibility;//RoseAccessTypeConverter.GetVisibilityKind((RoseItem as RationalRose.RoseOperation).ExportControl.Name);
                //    if ((visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessComponent
                //        || visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessComponentOrProtected))
                //        return Properties.Resources.ImplOperation;
                //    if ((visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessPrivate))
                //        return Properties.Resources.PrivateOperation;
                //    if (visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessProtected)
                //        return Properties.Resources.ProtectedOperation;
                //    if (visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic)
                //        return Properties.Resources.PublicOperation;
                //}

                //if (RealObject is OOAdvantech.MetaDataRepository.AttributeRealization)
                //{
                //    OOAdvantech.MetaDataRepository.VisibilityKind visibility = (RealObject as OOAdvantech.MetaDataRepository.AttributeRealization).Visibility;// RoseAccessTypeConverter.GetVisibilityKind((RoseItem as RationalRose.RoseAttribute).ExportControl.Name);
                //    if ((visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessComponent
                //        || visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessComponentOrProtected))
                //        return Properties.Resources.ImplAttr;
                //    if ((visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessPrivate))
                //        return Properties.Resources.PrivateAttribute;
                //    if (visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessProtected)
                //        return Properties.Resources.PublicAttribute;
                //    if (visibility == OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic)
                //        return Properties.Resources.PublicAttribute;

                //}
                //if (RealObject is OOAdvantech.MetaDataRepository.AssociationEnd)
                //    return Properties.Resources.NavigableRole;
                //if (RealObject is OOAdvantech.MetaDataRepository.AssociationEndRealization)
                //    return Properties.Resources.NavigableRole;

                //return Properties.Resources.PublicOperation;

            }
        }




    }

}
