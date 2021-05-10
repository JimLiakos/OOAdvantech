using OOAdvantech.Collections.Generic;
using OOAdvantech.MetaDataRepository;
namespace OOAdvantech.UserInterface.Runtime
{
	/// <MetaDataID>{FFCC2846-3A25-4142-BA74-BED0016D4973}</MetaDataID>
    public interface IObjectMemberViewControl : IOperationCallerSource
    {
        /// <MetaDataID>{d6515558-c371-4abf-8d8f-a530ec830f38}</MetaDataID>
        string Name
        {
            get;
            set;
        }
        /// <MetaDataID>{beff5a4e-783a-4dd9-9d8c-939856678e63}</MetaDataID>
        bool AllowDrag
        {
            get;
        }
        /// <MetaDataID>{e04795a6-0aa3-482c-8e03-cd98a3fa7ff1}</MetaDataID>
        bool AllowDrop
        {
            get;
        }


        /// <summary>
        /// Checks metaData for error.
        /// </summary>
        /// <param name="errors">
        /// errors parameter used from operation to append the errors
        /// </param>
        /// <returns>
        /// If there are errors return true else return false
        /// </returns>
        /// <MetaDataID>{562ed461-c05b-46f8-8539-b03788c0e017}</MetaDataID>
        bool ErrorCheck(ref System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject.MetaDataError> errors);

    
        /// <MetaDataID>{BE8BD1ED-2F91-40E6-B63C-0B9BF67E03CE}</MetaDataID>
        //

        ///// <MetaDataID>{422E12B2-396A-4F56-8343-BEEE7DD02458}</MetaDataID>
        //bool ConnectedObjectAutoUpdate
        //{
        //    //TODO δεν χριάζεται
        //    get;
        //    set;
        //}

        ///// <MetaDataID>{6403693C-6A4B-4F3B-B8E3-4A75D33DDAF0}</MetaDataID>
        //object Value
        //{
        //    get;
        //    set;
        //}
        ///// <MetaDataID>{E50D30E3-0AF9-42DE-8A9A-1DDD8996F28A}</MetaDataID>
        //OOAdvantech.MetaDataRepository.Classifier ValueType
        //{
        //    get;
        //}
        /// <MetaDataID>{2F3B4B2D-C2B3-47A4-8D01-BEF1DBDDF659}</MetaDataID>
        //object Path
        //{
        //    get;
        //    set;
        //}
     

        ///// <MetaDataID>{064D7141-BE8B-48F3-86FE-9B3FF628A564}</MetaDataID>
        //void LoadControlValues();
        ///// <MetaDataID>{0FB3954F-B99F-429D-ACC8-CAE9F83EA9CF}</MetaDataID>
        //void SaveControlValues();
    }
}
