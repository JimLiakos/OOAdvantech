// TransactionEventConsumer.cpp : Implementation of CTransactionEventConsumer

#include "stdafx.h"
#include "TransactionEventConsumer.h"
#include ".\transactioneventconsumer.h"


// CTransactionEventConsumer


STDMETHODIMP CTransactionEventConsumer::get_ManagedObjectConsumer(IUnknown** pVal)
{
	// TODO: Add your implementation code here

	return S_OK; 
}

STDMETHODIMP CTransactionEventConsumer::put_ManagedObjectConsumer(IUnknown* newVal)
{
	ManagedObjectConsumer=newVal;
	return S_OK;
}

STDMETHODIMP CTransactionEventConsumer::put_Transaction(IUnknown* newVal)
{
	Transaction=newVal;
	IUnknown* pUnk=NULL;
	QueryInterface(__uuidof(IUnknown),(void**)&pUnk);
	HRESULT hr= AtlAdvise(Transaction,pUnk,__uuidof(ITransactionOutcomeEvents),&Cookie);
	pUnk->Release();
	return S_OK;
}
