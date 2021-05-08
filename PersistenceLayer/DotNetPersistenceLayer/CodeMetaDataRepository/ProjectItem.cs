using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.CodeMetaDataRepository
{
    /// <MetaDataID>{57B9C9DE-6665-4a80-AA3E-FDA5643B60D5}</MetaDataID>
    public class ProjectItem : OOAdvantech.MetaDataRepository.MetaObject
    {
        /// <MetaDataID>{c0980412-6201-438b-bcd7-69dd0d1be49d}</MetaDataID>
        public static ProjectItem AddMetaObject(EnvDTE.ProjectItem vsProjectItem, OOAdvantech.MetaDataRepository.MetaObject metaObject)
        {
            if (vsProjectItem == null)
                return null;
            ProjectItem projectItem = MetaObjectMapper.FindMetaObjectFor(vsProjectItem) as ProjectItem;
            if (projectItem == null)
                projectItem = new ProjectItem(vsProjectItem, metaObject.ImplementationUnit as Project);
            if (!projectItem.MetaObjectImplementations.Contains(metaObject))
                projectItem.MetaObjectImplementations.Add(metaObject);
            return projectItem;

        }
        /// <MetaDataID>{ec36d6ad-cdce-419e-aba2-dbc5fec72062}</MetaDataID>
        public static ProjectItem GetProjectItem(EnvDTE.ProjectItem vsProjectItem)
        {
            if (vsProjectItem == null)
                return null;
            ProjectItem projectItem = MetaObjectMapper.FindMetaObjectFor(vsProjectItem) as ProjectItem;
            if (projectItem == null)
                projectItem = new ProjectItem(vsProjectItem,MetaObjectMapper.FindMetaObjectFor(vsProjectItem.ContainingProject) as Project);
            return projectItem;

        }


        /// <MetaDataID>{1d357299-0163-4a26-b5c9-414b634c228d}</MetaDataID>
        public void RemoveMetaObject(OOAdvantech.MetaDataRepository.MetaObject metaObject)
        {

            if (MetaObjectImplementations.Contains(metaObject))
                MetaObjectImplementations.Remove(metaObject);
        }


        /// <MetaDataID>{4ea83c21-408e-4369-bf39-37322f7cf948}</MetaDataID>
        public static void RemoveMetaObject(EnvDTE.ProjectItem vsProjectItem, OOAdvantech.MetaDataRepository.MetaObject metaObject)
        {
            if (vsProjectItem == null)
                return;
            ProjectItem projectItem = MetaObjectMapper.FindMetaObjectFor(vsProjectItem) as ProjectItem;
            if (projectItem == null)
                return;
            if (projectItem.MetaObjectImplementations.Contains(metaObject))
                projectItem.MetaObjectImplementations.Remove(metaObject);
        }




        /// <MetaDataID>{558cd289-5ac6-40b7-9987-1d1793f2a49d}</MetaDataID>
        public void ProjectItemRemoved()
        {
            foreach (MetaDataRepository.MetaObject metaObject in new System.Collections.Generic.List<MetaDataRepository.MetaObject> (MetaObjectImplementations))
            {
                if (metaObject is CodeElementContainer)
                    (metaObject as CodeElementContainer).CodeElementRemoved(this.VSProjectItem);

            }
            MetaObjectMapper.RemoveMetaObject(this);

        }
        /// <MetaDataID>{AE174664-9921-4B65-B143-1062EA823A72}</MetaDataID>
        public System.Collections.Generic.List<MetaDataRepository.MetaObject> MetaObjectImplementations = new System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject>();
        /// <MetaDataID>{303021CD-D95D-4E5C-BC34-40242C332E95}</MetaDataID>
        public EnvDTE.ProjectItem VSProjectItem;

        /// <MetaDataID>{b101958f-4e06-4653-9cbf-917091fcf7aa}</MetaDataID>
        public readonly Project Project;
        /// <MetaDataID>{679A4A6E-EAEC-469F-8082-C6468A884097}</MetaDataID>
        public ProjectItem(EnvDTE.ProjectItem vsProjectItem,Project project )
        {
            Project = project;
            VSProjectItem = vsProjectItem;
            MetaObjectMapper.AddTypeMap(vsProjectItem, this);
        }
         /// <MetaDataID>{9790E39F-D7DD-4F7E-9234-921B6F36E00C}</MetaDataID>
        public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
        {
            return new System.Collections.Generic.List<object>();
        }

        /// <MetaDataID>{6302638b-02fb-4e7f-a9d0-795a58856240}</MetaDataID>
        internal OOAdvantech.MetaDataRepository.MetaObject FindMetaObjectFor(EnvDTE.CodeElement codeElement)
        {
            foreach (CodeElementContainer metaObject in MetaObjectImplementations)
            {
                if (metaObject.ContainCodeElement(codeElement,null))
                    return metaObject as MetaDataRepository.MetaObject;
            }
            return null;
        }
    }
}
