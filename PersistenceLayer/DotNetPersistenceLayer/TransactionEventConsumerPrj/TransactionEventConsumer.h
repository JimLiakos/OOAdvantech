// TransactionEventConsumer.h : Declaration of the CTransactionEventConsumer

#pragma once
#include "resource.h"       // main symbols
#include "transact.h"
//#import "PersistenceLayer\DotNetPersistenceLayer\Transactions\bin\debug\Transactions.tlb"
#include "Transactions.h"
// ITransactionEventConsumer
[
	object,
	uuid("C693EA9A-E5A0-4318-9C88-CC5E9F61677B"),
	dual,	helpstring("ITransactionEventConsumer Interface"),
	pointer_default(unique)
]
__interface ITransactionEventConsumer : IDispatch
{
	[propget, id(1), helpstring("property ManagedObjectConsumer")] HRESULT ManagedObjectConsumer([out, retval] IUnknown** pVal);
	[propput, id(1), helpstring("property ManagedObjectConsumer")] HRESULT ManagedObjectConsumer([in] IUnknown* newVal);
	[propput, id(2), helpstring("property Transaction")] HRESULT Transaction([in] IUnknown* newVal);
};



// CTransactionEventConsumer

[
	coclass,
	threading("apartment"),
	vi_progid("TransactionEventConsumerPrj.Transaction"),
	progid("TransactionEventConsumerPrj.Transacti.1"),
	version(1.0),
	uuid("F2263E18-DB20-4418-9A27-B761DD4E9690"),
	helpstring("TransactionEventConsumer Class")
]
class ATL_NO_VTABLE CTransactionEventConsumer : 
	public ITransactionEventConsumer,
	public ITransactionOutcomeEvents
{
public:
	CTransactionEventConsumer()
	{
		ManagedObjectConsumer=NULL;
		Cookie=0;
	}


	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct()
	{
		return S_OK;
	}

	void FinalRelease() 
	{
		ManagedObjectConsumer->Release();
	}

public:
	IUnknownPtr ManagedObjectConsumer;
	IUnknownPtr Transaction; 
	DWORD Cookie;


  STDMETHOD(Committed)( 
            /* [in] */ BOOL fRetaining,
            /* [unique][in] */ XACTUOW *pNewUOW,
            /* [in] */ HRESULT hr)
  {

        
		Transactions::ITransactionOutcomeEventsPtr TransactionOutcomeEvents=ManagedObjectConsumer;
		TransactionOutcomeEvents->Committed();
		AtlUnadvise(Transaction,__uuidof(ITransactionOutcomeEvents),Cookie);
		return S_OK;

  }
        
	STDMETHOD(Aborted)( 
            /* [unique][in] */ BOID *pboidReason,
            /* [in] */ BOOL fRetaining,
            /* [unique][in] */ XACTUOW *pNewUOW,
            /* [in] */ HRESULT hr)
  {
	  Transactions::ITransactionOutcomeEventsPtr TransactionOutcomeEvents=ManagedObjectConsumer;
	  TransactionOutcomeEvents->Aborted("");
	  return S_OK;

  }
  
  STDMETHOD(HeuristicDecision)( 
            /* [in] */ DWORD dwDecision,
            /* [unique][in] */ BOID *pboidReason,
            /* [in] */ HRESULT hr)
  {
  	  Transactions::ITransactionOutcomeEventsPtr TransactionOutcomeEvents=ManagedObjectConsumer;
	  Transactions::XACTHEURISTIC Decision=Transactions::XACTHEURISTIC_XACTHEURISTIC_ABORT;
	  
	 switch(dwDecision)
	 {
	  case 1:
		  {
			  Decision=Transactions::XACTHEURISTIC_XACTHEURISTIC_ABORT;
									 
		  }
	  case 2:
		  {
			  Decision=Transactions::XACTHEURISTIC_XACTHEURISTIC_COMMIT;
		  }
	  case 3:
		  {
			  Decision=Transactions::XACTHEURISTIC_XACTHEURISTIC_DAMAGE;
		  }
	  case 4:
		  {
			  Decision=Transactions::XACTHEURISTIC_XACTHEURISTIC_DANGER;
		  }
	  }
	  TransactionOutcomeEvents->HeuristicDecision(Decision,"");
	  return S_OK;

  }
STDMETHOD(Indoubt)( void)
{
      
	Transactions::ITransactionOutcomeEventsPtr TransactionOutcomeEvents=ManagedObjectConsumer;
	TransactionOutcomeEvents->Indoubt();
	return S_OK;

  }
/*	// IQEBase Methods
public:
	STDMETHOD(get_Session)(IQESession * * ppSession)
	{
		return E_NOTIMPL;
	}*/
  STDMETHOD(get_ManagedObjectConsumer)(IUnknown** pVal);
  STDMETHOD(put_ManagedObjectConsumer)(IUnknown* newVal);
  STDMETHOD(put_Transaction)(IUnknown* newVal);
};

