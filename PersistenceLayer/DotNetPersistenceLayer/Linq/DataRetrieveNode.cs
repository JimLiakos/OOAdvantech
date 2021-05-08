using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

namespace OOAdvantech.Linq
{
    
    /// <summary>
    /// DataRetrieveNode objects used to keeps index the infos 
    /// the objects used from the system to retrieve the data and then 
    /// system will use data to initialize properties of dynamic type object
    /// </summary>
    /// <MetaDataID>{f96a8fe2-3130-46e7-ba5e-79f960489c9f}</MetaDataID>
    internal class DataRetrieveNode
    {
       
       
        
     
          /// <MetaDataID>{dacc9ebd-ca9d-4bdc-9c6d-be079d07dd0d}</MetaDataID>
        public readonly DataNode DataNode;

        /// <summary>
        /// Define the index of row in composite row.
        /// This row keeps the data for the data node  	
        /// </summary>
        /// <MetaDataID>{b234e897-f625-416b-a4a0-b1cc9f44966e}</MetaDataID>
        public readonly int DataRowIndex;

        /// <MetaDataID>{2238dfd9-4f64-48d0-9b1a-a73075b138e1}</MetaDataID>
        public DataRetrieveNode(DataNode dataNode, int dataRowIndex)
        {
            DataNode = dataNode;
            DataRowIndex = dataRowIndex;
        }
    }


   
}
