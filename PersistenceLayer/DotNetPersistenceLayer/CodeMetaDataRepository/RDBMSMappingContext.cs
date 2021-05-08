namespace OOAdvantech.CodeMetaDataRepository
{
    /// <MetaDataID>{bb94e46c-2f22-40fd-b6e9-a4a4c03b9aa2}</MetaDataID>
    public class RDBMSMappingContext
    {
        /// <MetaDataID>{4ff14857-952e-4f94-8a06-358f394a8705}</MetaDataID>
          public EnvDTE.ProjectItem VSProjectItem;
          /// <MetaDataID>{69feb1df-d38f-462c-999b-43cdc665264c}</MetaDataID>
          public readonly OOAdvantech.PersistenceLayer.ObjectStorage ObjectStorage = null;
          /// <MetaDataID>{ad4f7a56-b042-44cb-adb0-0b816748cd30}</MetaDataID>
          System.Xml.XmlDocument document;
          /// <MetaDataID>{00ed6bbe-2bfd-4440-8c04-3cd2b6daa052}</MetaDataID>
          public RDBMSMappingContext(EnvDTE.ProjectItem vsProjectItem)
          {

              document = new System.Xml.XmlDocument();
              VSProjectItem = vsProjectItem;
              document.Load(VSProjectItem.get_FileNames(1));
              ObjectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("RDBMSMapingStorage", document, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");

          }

          /// <MetaDataID>{cda730f5-cc41-4920-8fc8-e8a396a0ed6a}</MetaDataID>
          public string Name
          {
              get
              {
                  string name = VSProjectItem.Name;
                  int npos = name.LastIndexOf(".");
                  if (npos != -1)
                      name = name.Substring(0, npos);
                  return name;
              }
              set
              {
                  string extention = null;
                  string name = VSProjectItem.Name;
                  int npos = name.LastIndexOf(".");
                  if (npos != -1)
                      extention = name.Substring(npos);
                  VSProjectItem.Name = value+extention;
              }
          }



          /// <MetaDataID>{7672479e-e285-4e68-819e-905b981e18d7}</MetaDataID>
          public void Save()
          {
              
              document.Save(VSProjectItem.get_FileNames(1));
          }
    }
}
