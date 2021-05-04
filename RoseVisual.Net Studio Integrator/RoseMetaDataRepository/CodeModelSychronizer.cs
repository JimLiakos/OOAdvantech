using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoseMetaDataRepository
{
    /// <MetaDataID>{a2b44b93-075c-4c36-b16e-75295fb4b08f}</MetaDataID>
    internal class CodeModelSychronizer : OOAdvantech.UserInterface.Runtime.PresentationObject<OOAdvantech.MetaDataRepository.MetaObject>
    {
        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{2360c2bb-f6a1-435c-a510-21fcaa29d5da}</MetaDataID>
        OOAdvantech.MetaDataRepository.MetaObject ModelMetaObject;
        /// <MetaDataID>{3c579f8c-ebdc-42e3-ab72-85f554198122}</MetaDataID>
        OOAdvantech.MetaDataRepository.MetaObject CodeMetaObject;
        /// <MetaDataID>{5f0a6364-8b15-4c0e-a383-24912be4feed}</MetaDataID>
        public CodeModelSychronizer(OOAdvantech.MetaDataRepository.MetaObject modelMetaObject, OOAdvantech.MetaDataRepository.MetaObject codeMetaObject)
            : base(modelMetaObject)
        {
            ModelMetaObject = modelMetaObject;
            CodeMetaObject = codeMetaObject;
        }

        /// <MetaDataID>{ab4e6236-821e-4926-bd80-e99f48d2f7e5}</MetaDataID>
        List<SychronizationItem> _UnAssignedModelItems;
        /// <MetaDataID>{c59c685a-0761-4a20-bdfd-187da1a72f25}</MetaDataID>
        public List<SychronizationItem> UnAssignedModelItems
        {
            get
            {
                if (_UnAssignedModelItems == null)
                {
                    List<SychronizationItem> unAssignedModelItems = new List<SychronizationItem>();
                    List<SychronizationItem> unAssignedCodeItems = new List<SychronizationItem>();
                    if (ModelMetaObject is OOAdvantech.MetaDataRepository.Classifier)
                    {
                        OOAdvantech.MetaDataRepository.Classifier modelClasifier = ModelMetaObject as OOAdvantech.MetaDataRepository.Classifier;
                        OOAdvantech.MetaDataRepository.Classifier codeClasifier = CodeMetaObject as OOAdvantech.MetaDataRepository.Classifier;
                        OOAdvantech.MetaDataRepository.ContainedItemsSynchronizer FeatureSynchronizer = MetaObjectsStack.CurrentMetaObjectCreator.BuildItemsSychronizer(modelClasifier.Features, codeClasifier.Features, modelClasifier);
                        FeatureSynchronizer.FindModifications();

                        foreach (OOAdvantech.MetaDataRepository.DeleteCommand command in FeatureSynchronizer.DeletedObjectsCommands)
                            unAssignedCodeItems.Add(new SychronizationItem(command.CandidateForDeleteObject));
                        foreach (OOAdvantech.MetaDataRepository.AddCommand command in FeatureSynchronizer.AddedObjectsCommands)
                            unAssignedModelItems.Add(new SychronizationItem(command.MissingMetaObject));
                    }
                    _UnAssignedModelItems = unAssignedModelItems;
                    _UnAssignedCodeItems = unAssignedCodeItems;
                }

                return _UnAssignedModelItems;
            }
        }
        /// <MetaDataID>{d2819a38-4887-4312-b0dc-049870d89a34}</MetaDataID>
        List<SychronizationItem> _UnAssignedCodeItems;

        /// <MetaDataID>{1ec9b75d-c7f5-4747-b5c7-03d38414edc5}</MetaDataID>
        public List<SychronizationItem > UnAssignedCodeItems
        {
            get
            {
                if (_UnAssignedCodeItems == null)
                {
                    List<SychronizationItem> unAssignedModelItems = new List<SychronizationItem>();
                    List<SychronizationItem> unAssignedCodeItems = new List<SychronizationItem>();
                    if (ModelMetaObject is OOAdvantech.MetaDataRepository.Classifier)
                    {
                        OOAdvantech.MetaDataRepository.Classifier modelClasifier = ModelMetaObject as OOAdvantech.MetaDataRepository.Classifier;
                        OOAdvantech.MetaDataRepository.Classifier codeClasifier = CodeMetaObject as OOAdvantech.MetaDataRepository.Classifier;
                        OOAdvantech.MetaDataRepository.ContainedItemsSynchronizer featureSynchronizer = MetaObjectsStack.CurrentMetaObjectCreator.BuildItemsSychronizer(modelClasifier.Features, codeClasifier.Features, modelClasifier);
                        featureSynchronizer.FindModifications();

                        foreach (OOAdvantech.MetaDataRepository.DeleteCommand command in featureSynchronizer.DeletedObjectsCommands)
                            unAssignedCodeItems.Add(new SychronizationItem(command.CandidateForDeleteObject));
                        foreach (OOAdvantech.MetaDataRepository.AddCommand command in featureSynchronizer.AddedObjectsCommands)
                            unAssignedModelItems.Add(new SychronizationItem(command.MissingMetaObject));


                        //OOAdvantech.MetaDataRepository.ContainedItemsSynchronizer rolesSynchronizer = MetaObjectsStack.CurrentMetaObjectCreator.BuildItemsSychronizer(modelClasifier.GetAssociateRoles(false), codeClasifier.GetAssociateRoles(false), modelClasifier);
                        //rolesSynchronizer.FindModifications();
                        //foreach (OOAdvantech.MetaDataRepository.DeleteCommand command in rolesSynchronizer.DeletedObjectsCommands)
                        //{
                        //    if(command.CandidateForDeleteObject
                        //    unAssignedCodeItems.Add(new SychronizationItem(command.CandidateForDeleteObject));
                        //}
                        //foreach (OOAdvantech.MetaDataRepository.AddCommand command in rolesSynchronizer.AddedObjectsCommands)
                        //    unAssignedModelItems.Add(new SychronizationItem(command.MissingMetaObject));



                    }
                    _UnAssignedModelItems = unAssignedModelItems;
                    _UnAssignedCodeItems = unAssignedCodeItems;
                }

                return _UnAssignedCodeItems;
            }
        }

        //public 

        /// <MetaDataID>{1e09cad7-1f18-4051-a92f-aae65253af18}</MetaDataID>
        public void DeleteSelectedItems()
        {
            foreach (var modelItem in new List<SychronizationItem> (_UnAssignedModelItems))
            {
                if (modelItem.Delete)
                {
                    if (modelItem.RealObject is RoseMetaDataRepository.Operation)
                    {
                        
                        (modelItem.RealObject as RoseMetaDataRepository.Operation).RoseOperation.ParentClass.DeleteOperation((modelItem.RealObject as RoseMetaDataRepository.Operation).RoseOperation);
                        _UnAssignedModelItems.Remove(modelItem);
                    }

                    if (modelItem.RealObject is RoseMetaDataRepository.Attribute)
                    {

                        (modelItem.RealObject as RoseMetaDataRepository.Attribute).RoseAttribute.ParentClass.DeleteAttribute((modelItem.RealObject as RoseMetaDataRepository.Attribute).RoseAttribute);
                        _UnAssignedModelItems.Remove(modelItem);
                    }

                    if (modelItem.RealObject is RoseMetaDataRepository.AttributeRealization)
                    {

                        (modelItem.RealObject as RoseMetaDataRepository.AttributeRealization).RoseAttribute.ParentClass.DeleteAttribute((modelItem.RealObject as RoseMetaDataRepository.AttributeRealization).RoseAttribute);
                        _UnAssignedModelItems.Remove(modelItem);
                    }

                    if (modelItem.RealObject is RoseMetaDataRepository.Method)
                    {

                        (modelItem.RealObject as RoseMetaDataRepository.Method).RoseOperation.ParentClass.DeleteOperation((modelItem.RealObject as RoseMetaDataRepository.Method).RoseOperation);
                        _UnAssignedModelItems.Remove(modelItem);
                    }

                    if (modelItem.RealObject is RoseMetaDataRepository.AssociationEndRealization)
                    {
                        (modelItem.RealObject as RoseMetaDataRepository.AssociationEndRealization).RoseAttribute.ParentClass.DeleteAttribute((modelItem.RealObject as RoseMetaDataRepository.AssociationEndRealization).RoseAttribute);
                        _UnAssignedModelItems.Remove(modelItem);
                    }

                }
            }

            foreach (var modelItem in new List<SychronizationItem>(_UnAssignedCodeItems))
            {
                if (modelItem.Delete)
                {
                    if (modelItem.RealObject is OOAdvantech.MetaDataRepository.Operation)
                    {
                        (modelItem.RealObject as OOAdvantech.MetaDataRepository.Operation).Owner.RemoveOperation(modelItem.RealObject as OOAdvantech.MetaDataRepository.Operation);
                        _UnAssignedCodeItems.Remove(modelItem);
                    }
                    if (modelItem.RealObject is OOAdvantech.MetaDataRepository.Method)
                    {
                        if((modelItem.RealObject as OOAdvantech.MetaDataRepository.Method).Owner is OOAdvantech.MetaDataRepository.Class)
                            ((modelItem.RealObject as OOAdvantech.MetaDataRepository.Method).Owner as OOAdvantech.MetaDataRepository.Class).RemoveMethod(modelItem.RealObject as OOAdvantech.MetaDataRepository.Method);
                        if ((modelItem.RealObject as OOAdvantech.MetaDataRepository.Method).Owner is OOAdvantech.MetaDataRepository.Structure)
                            ((modelItem.RealObject as OOAdvantech.MetaDataRepository.Method).Owner as OOAdvantech.MetaDataRepository.Structure).RemoveMethod(modelItem.RealObject as OOAdvantech.MetaDataRepository.Method);
                        _UnAssignedCodeItems.Remove(modelItem);
                    }
                    if (modelItem.RealObject is OOAdvantech.MetaDataRepository.Attribute)
                    {
                        (modelItem.RealObject as OOAdvantech.MetaDataRepository.Attribute).Owner.RemoveAttribute(modelItem.RealObject as OOAdvantech.MetaDataRepository.Attribute);
                        _UnAssignedCodeItems.Remove(modelItem);
                    }
                    if (modelItem.RealObject is OOAdvantech.MetaDataRepository.AttributeRealization)
                    {
                        if ((modelItem.RealObject as OOAdvantech.MetaDataRepository.AttributeRealization).Owner is OOAdvantech.MetaDataRepository.Class)
                            ((modelItem.RealObject as OOAdvantech.MetaDataRepository.AttributeRealization).Owner as OOAdvantech.MetaDataRepository.Class).RemoveAttributeRealization(modelItem.RealObject as OOAdvantech.MetaDataRepository.AttributeRealization);
                        if ((modelItem.RealObject as OOAdvantech.MetaDataRepository.AttributeRealization).Owner is OOAdvantech.MetaDataRepository.Structure)
                            ((modelItem.RealObject as OOAdvantech.MetaDataRepository.AttributeRealization).Owner as OOAdvantech.MetaDataRepository.Structure).RemoveAttributeRealization(modelItem.RealObject as OOAdvantech.MetaDataRepository.AttributeRealization);
                        _UnAssignedCodeItems.Remove(modelItem);
                    }

                    if (modelItem.RealObject is OOAdvantech.MetaDataRepository.AssociationEndRealization)
                    {
                        if ((modelItem.RealObject as OOAdvantech.MetaDataRepository.AssociationEndRealization).Owner is OOAdvantech.MetaDataRepository.Class)
                            ((modelItem.RealObject as OOAdvantech.MetaDataRepository.AssociationEndRealization).Owner as OOAdvantech.MetaDataRepository.Class).RemoveAssociationEndRealization(modelItem.RealObject as OOAdvantech.MetaDataRepository.AssociationEndRealization);
                        if ((modelItem.RealObject as OOAdvantech.MetaDataRepository.AssociationEndRealization).Owner is OOAdvantech.MetaDataRepository.Structure)
                            ((modelItem.RealObject as OOAdvantech.MetaDataRepository.AssociationEndRealization).Owner as OOAdvantech.MetaDataRepository.Structure).RemoveAssociationEndRealization(modelItem.RealObject as OOAdvantech.MetaDataRepository.AssociationEndRealization);
                        _UnAssignedCodeItems.Remove(modelItem);
                    }
                }
            }
            if (ObjectChangeState != null)
                ObjectChangeState(this, null);


        }
    }
 
}


