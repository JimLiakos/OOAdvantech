#include "StdAfx.h"
#include "TxDtc.h"
#include ".\transactionmanager.h"
//#import "PersistenceLayer\DotNetPersistenceLayer\ComPlusTransactionManager\_ComPlusTransactionManager.tlb"
//#include  "PersistenceLayer\DotNetPersistenceLayer\ComPlusTransactionManager\_ComPlusTransactionManager.h"
#import "RoseVisual.Net Studio Integrator\COMGeneralClasses\COMGeneralClasses.tlb" rename("GetObject","GetRunningObject")
#import "PersistenceLayer\DotNetPersistenceLayer\TransactionEventConsumerPrj\_TransactionEventConsumerPrj.tlb"



System::Byte TransactionManagmentSystem::TransactionManager::GetImportWhereabouts()[]
{
	HRESULT hr=0;
	ITransactionImportWhereabouts* pTransactionImportWhereabouts=NULL;
	hr= DtcGetTransactionManager(NULL,NULL,__uuidof(ITransactionImportWhereabouts),0,0,NULL,(void**)&pTransactionImportWhereabouts);
	System::Byte ByteStream[]=NULL;
	ULONG Size=0;
	pTransactionImportWhereabouts->GetWhereaboutsSize(&Size);
	BYTE* Whereabouts= (BYTE*)CoTaskMemAlloc(Size);
	ULONG UsedSize=0;
	pTransactionImportWhereabouts->GetWhereabouts(Size,Whereabouts,&UsedSize);
	pTransactionImportWhereabouts->Release();
	pTransactionImportWhereabouts=NULL;
	ByteStream=__gc new System::Byte[UsedSize];
	for(int i=0;i!=UsedSize;i++)
		ByteStream[i]=Whereabouts[i];
	CoTaskMemFree(Whereabouts);
	return ByteStream;

}

System::Byte TransactionManagmentSystem::TransactionManager::ExportTransaction(System::Object* obj,System::Byte whereAboutsStream __gc[])[]
{

	
	HRESULT hr=0;
	System::IntPtr p= System::Runtime::InteropServices::Marshal::GetIUnknownForObject(obj);
	IUnknown* pUnk=NULL;
	pUnk=(IUnknown*)p.ToPointer();
	
	ITransactionExportFactory* pTransactionExportFactory=NULL;
	ITransaction* pTransaction=NULL;
	pUnk->QueryInterface(__uuidof(ITransaction),(void**)&pTransaction);
	pUnk->Release();
	pUnk=NULL;
	ITransactionDispenser* pTransactionDispenser=NULL;
	hr=DtcGetTransactionManager(NULL,NULL,__uuidof(ITransactionDispenser),0,0,NULL,(void**)&pTransactionDispenser);

	ITransactionTransmitterFactory* pTxTransmitterFactory = NULL;
	hr = pTransactionDispenser->QueryInterface(__uuidof(ITransactionTransmitterFactory), (void**)&pTxTransmitterFactory);
	pTransactionDispenser->Release();
	pTransactionDispenser=NULL;

    ITransactionTransmitter* pTxTransmitter = NULL;
	hr = pTxTransmitterFactory->Create(&pTxTransmitter);
	hr = pTxTransmitter->Set(pTransaction);
	ULONG tokenSize = 0;
	hr = pTxTransmitter->GetPropagationTokenSize(&tokenSize);
	BYTE* token = (BYTE*)CoTaskMemAlloc(tokenSize);
	ULONG tokenSizeUsed = 0;
	hr = pTxTransmitter->MarshalPropagationToken(tokenSize, token, &tokenSizeUsed);
	hr = pTxTransmitter->Reset();
	pTxTransmitterFactory->Release();
	pTxTransmitterFactory=NULL;
	pTxTransmitter->Release();
	pTxTransmitter=NULL;

	System::Byte ByteStream[]=__gc new System::Byte[tokenSizeUsed];
	for(int i=0;i!=tokenSizeUsed;i++)
		ByteStream[i]=token[i];
	CoTaskMemFree(token);
	return ByteStream;





	




	/*
	HRESULT hr=0;
	System::IntPtr p= System::Runtime::InteropServices::Marshal::GetIUnknownForObject(obj);
	IUnknown* pUnk=NULL;
	pUnk=(IUnknown*)p.ToPointer();
	
	ITransactionExportFactory* pTransactionExportFactory=NULL;
	ITransaction* pTransaction=NULL;
	pUnk->QueryInterface(__uuidof(ITransaction),(void**)&pTransaction);

	hr= DtcGetTransactionManager(NULL,NULL,__uuidof(ITransactionExportFactory),0,0,NULL,(void**)&pTransactionExportFactory);
	System::Byte ByteStream[]=NULL;
	if(pTransactionExportFactory!=NULL)
	{

		ULONG UsedSize=0;

		BYTE* Whereabouts= (BYTE*)CoTaskMemAlloc(whereAboutsStream->get_Count());
		for(int i=0;i!=whereAboutsStream->get_Count();i++)
			Whereabouts[i]=whereAboutsStream[i];
		ITransactionExport* pTransactionExport=NULL;
		hr=pTransactionExportFactory->Create(whereAboutsStream->get_Count(),Whereabouts,&pTransactionExport);
		CoTaskMemFree(Whereabouts);

		ULONG TransCookieSize=0;
		pTransactionExport->Export(pTransaction,&TransCookieSize);
		pTransaction->Release();
		pTransaction=NULL;

		BYTE* TransCookie= (BYTE*)CoTaskMemAlloc(TransCookieSize);
		pTransactionExport->GetTransactionCookie(pUnk,TransCookieSize,TransCookie,&UsedSize);
		pTransactionExportFactory->Release();
		pTransactionExportFactory=NULL;
		pTransactionExport->Release();
		pTransactionExport=NULL;
		ByteStream=__gc new System::Byte[UsedSize];
		for(int i=0;i!=UsedSize;i++)
			ByteStream[i]=TransCookie[i];
		CoTaskMemFree(TransCookie);
	}
	
	if(pUnk!=NULL)
		System::Runtime::InteropServices::Marshal::Release(p);
	return ByteStream;*/
}



System::Object* TransactionManagmentSystem::TransactionManager::ImportTransaction(System::Byte transactionStream __gc[])
{

	BYTE* token=(BYTE*)CoTaskMemAlloc(transactionStream->get_Count());
    for(int i=0;i!=transactionStream->get_Count();i++)
		token[i]=transactionStream[i];


	ITransactionDispenser* pTransactionDispenser=NULL;
	HRESULT hr=DtcGetTransactionManager(NULL,NULL,__uuidof(ITransactionDispenser),0,0,NULL,(void**)&pTransactionDispenser);
	ITransactionReceiverFactory* pTxReceiverFactory = NULL;
	hr = pTransactionDispenser->QueryInterface(__uuidof(ITransactionReceiverFactory), (void**)&pTxReceiverFactory);
	pTransactionDispenser->Release();
	pTransactionDispenser=NULL;
	ITransactionReceiver* pTxReceiver = NULL;
	hr = pTxReceiverFactory->Create(&pTxReceiver);
	ITransaction* pTransaction = NULL; 
	hr = pTxReceiver->UnmarshalPropagationToken(transactionStream->Count, token, &pTransaction); 
	CoTaskMemFree(token);
	pTxReceiverFactory->Release();
	pTxReceiverFactory=NULL;
	pTxReceiver->Release();
	pTxReceiver==NULL;
	System::Object* Obj=System::Runtime::InteropServices::Marshal::GetObjectForIUnknown(pTransaction);
	pTransaction->Release();
	pTransaction=NULL;
	return Obj;

	/*

	ITransactionImport* pTransactionImport=NULL;
	DtcGetTransactionManager(NULL,NULL,__uuidof(ITransactionImport),0,0,NULL,(void**)&pTransactionImport);
	ITransaction* pTransaction=NULL;
	HRESULT hr=pTransactionImport->Import(transactionStream->get_Count(),TransCookie,(IID*)&IID_ITransaction,(void**)&pTransaction);
	System::Object* Obj=System::Runtime::InteropServices::Marshal::GetObjectForIUnknown(pTransaction);
    pTransaction->Release();
	CoTaskMemFree(TransCookie);
	return Obj;
	*/
}

void TransactionManagmentSystem::TransactionManager::RemoveFromROT(long ROTCookie)
{
	COMGENERALCLASSESLib::IRuntimeObjectTablePtr pRuntimeObjectTable(__uuidof(COMGENERALCLASSESLib::RuntimeObjectTable));
	pRuntimeObjectTable->Revoke(ROTCookie);
}


System::Object* TransactionManagmentSystem::TransactionManager::CreateTransaction(long Timeout,System::String* Description,System::String* TransactionURI)
{

	ITransactionDispenser* pTransactionDispenser=NULL;
	HRESULT hr=DtcGetTransactionManager(NULL,NULL,__uuidof(ITransactionDispenser),0,0,NULL,(void**)&pTransactionDispenser);

	ITransaction* pTransaction=NULL;
	//ISOLEVEL
	ITransactionOptions* pTransactionOptions=NULL;

	pTransactionDispenser->GetOptionsObject(&pTransactionOptions);
	XACTOPT Option;
	CString TransactionDescription=StringWr::ToCString(Description);
	char* pDescription=TransactionDescription.GetBuffer();
	strcpy(Option.szDescription,pDescription);
	TransactionDescription.ReleaseBuffer();
	Option.ulTimeout=Timeout;
	pTransactionOptions->SetOptions(&Option);
	

	/*
    ISOLATIONLEVEL_UNSPECIFIED      = 0xFFFFFFFF,
    ISOLATIONLEVEL_CHAOS            = 0x00000010,
    ISOLATIONLEVEL_READUNCOMMITTED  = 0x00000100,
    ISOLATIONLEVEL_BROWSE           = 0x00000100,
    ISOLATIONLEVEL_READCOMMITTED    = 0x00001000,
    ISOLATIONLEVEL_CURSORSTABILITY  = 0x00001000,
    ISOLATIONLEVEL_REPEATABLEREAD   = 0x00010000,
    ISOLATIONLEVEL_SERIALIZABLE     = 0x00100000,
    ISOLATIONLEVEL_ISOLATED         = 0x00100000
	*/ 
 
	hr=pTransactionDispenser->BeginTransaction(NULL,0x00100000,0,pTransactionOptions,(ITransaction**)&pTransaction);
	pTransactionOptions->Release();
	pTransactionOptions=NULL;
    System::Object* Obj=System::Runtime::InteropServices::Marshal::GetObjectForIUnknown(pTransaction);
	pTransaction->Release();
	return Obj;
}
long TransactionManagmentSystem::TransactionManager::AddToROT(System::Object* transaction,System::String* _transactionURI)
{
	COMGENERALCLASSESLib::IRuntimeObjectTablePtr pRuntimeObjectTable(__uuidof(COMGENERALCLASSESLib::RuntimeObjectTable));
	_bstr_t transactionURI=StringWr::ToCString(_transactionURI);
	IUnknown* pTransaction=reinterpret_cast<IUnknown*>( 
	System::Runtime::InteropServices::Marshal::GetIUnknownForObject(transaction).ToPointer());
	return pRuntimeObjectTable->RegisterObject(pTransaction,transactionURI);
}

void TransactionManagmentSystem::TransactionManager::SetEventsConsumer(System::Object* Transaction,System::Object* EventsConsumer)
{
	if(Transaction==NULL)
		throw new System::Exception("Transaction is null");
	if(EventsConsumer==NULL)
		throw new System::Exception("EventsConsumer is null");

	IUnknown* pTransaction=reinterpret_cast<IUnknown*>( 
		System::Runtime::InteropServices::Marshal::GetIUnknownForObject(Transaction).ToPointer());

	IUnknown* ManagedObjectConsumer=NULL;
	ManagedObjectConsumer=reinterpret_cast<IUnknown*>( 
		System::Runtime::InteropServices::Marshal::GetIUnknownForObject(EventsConsumer).ToPointer());
	TransactionEventConsumerPrj::ITransactionEventConsumerPtr pTransactionEventConsumer(__uuidof(TransactionEventConsumerPrj ::CTransactionEventConsumer));
	pTransactionEventConsumer->ManagedObjectConsumer=ManagedObjectConsumer;
	pTransactionEventConsumer->Transaction=pTransaction;
	ManagedObjectConsumer->Release();
	pTransaction->Release();


}




/*
void TransactionManagmentSystem::TransactionManager::PropagateTransaction(System::String* TransactionURI,System::String* ComputerName,System::String* channelUri)
{
	_bstr_t computerName=StringWr::ToCString(ComputerName);
	_bstr_t m_channelUri=StringWr::ToCString(channelUri);

	COSERVERINFO csi = { 0, computerName, 0, 0 };
	MULTI_QI mqi = { &__uuidof(IComPlusTransaction), 0, 0 };

	HRESULT hr = CoCreateInstanceEx(__uuidof(CComPlusTransaction), 0,
								CLSCTX_REMOTE_SERVER,
								&csi,1, &mqi);
	if(hr!=S_OK)
	{
		CString ErrorString="System can’t launch the COM+ transaction to the machine with identity ‘";
		ErrorString+=(TCHAR*)computerName;
		ErrorString+="’ for the transaction propagation.";
		throw new System::Exception(StringWr::ToManageString(ErrorString));
	}
	IComPlusTransaction* pIComPlusTransaction=NULL;
	mqi.pItf->QueryInterface(__uuidof(IComPlusTransaction),(void**)&pIComPlusTransaction);
	mqi.pItf->Release();
	mqi.pItf=NULL;
	_bstr_t transactionURI=StringWr::ToCString(TransactionURI);
	hr=pIComPlusTransaction->PropagateTransaction(transactionURI,m_channelUri);
    pIComPlusTransaction->Release();
	pIComPlusTransaction=NULL;
	if(hr!=S_OK)
	{
		CString ErrorString="The propagation of transaction to the machine with identity ‘";
		ErrorString+=(TCHAR*)computerName;
		ErrorString+="’ failed.";
		throw new System::Exception(StringWr::ToManageString(ErrorString));
	}

}
*/

CString TransactionManagmentSystem::StringWr::ToCString(System::String* mString)
{

	char* TempString=new char[mString->Length+1];
	int Count=mString->Length;
	for(int i=0;i!=Count;i++)
	{
		TempString[i]=(char)mString->Chars[i];
	}
	TempString[Count]=0;
	CString str(TempString);
	delete[] TempString;
	return str;
}


System::String* TransactionManagmentSystem::StringWr::ToManageString(CString &mString)
{
	int Count=mString.GetLength();
	System::String* retString=new System::String(mString);

	return retString;
}

System::Object* TransactionManagmentSystem::TransactionManager::GetTransaction(System::String* TransactionURI )
{
	COMGENERALCLASSESLib::IRuntimeObjectTablePtr pRuntimeObjectTable(__uuidof(COMGENERALCLASSESLib::RuntimeObjectTable));
	_bstr_t transactionURI=StringWr::ToCString(TransactionURI);
	IUnknownPtr pUnk=pRuntimeObjectTable->GetRunningObject(transactionURI);
	if(pUnk==NULL)
		return NULL;
	//pRuntimeObjectTable->Revoke(ROTCookie);
	System::Object* Obj=System::Runtime::InteropServices::Marshal::GetObjectForIUnknown((IUnknown*)pUnk) ;
	return Obj;
}
