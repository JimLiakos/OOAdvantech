

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 6.00.0366 */
/* at Thu Jan 12 10:39:29 2006
 */
/* Compiler settings for _TransactionEventConsumerPrj.idl:
    Oicf, W1, Zp8, env=Win32 (32b run)
    protocol : dce , ms_ext, c_ext, robust
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
//@@MIDL_FILE_HEADING(  )

#pragma warning( disable: 4049 )  /* more than 64k source lines */


/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 475
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif // __RPCNDR_H_VERSION__

#ifndef COM_NO_WINDOWS_H
#include "windows.h"
#include "ole2.h"
#endif /*COM_NO_WINDOWS_H*/

#ifndef ___TransactionEventConsumerPrj_h__
#define ___TransactionEventConsumerPrj_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef __ITransactionEventConsumer_FWD_DEFINED__
#define __ITransactionEventConsumer_FWD_DEFINED__
typedef interface ITransactionEventConsumer ITransactionEventConsumer;
#endif 	/* __ITransactionEventConsumer_FWD_DEFINED__ */


#ifndef __CTransactionEventConsumer_FWD_DEFINED__
#define __CTransactionEventConsumer_FWD_DEFINED__

#ifdef __cplusplus
typedef class CTransactionEventConsumer CTransactionEventConsumer;
#else
typedef struct CTransactionEventConsumer CTransactionEventConsumer;
#endif /* __cplusplus */

#endif 	/* __CTransactionEventConsumer_FWD_DEFINED__ */


/* header files for imported files */
#include "prsht.h"
#include "mshtml.h"
#include "mshtmhst.h"
#include "exdisp.h"
#include "objsafe.h"
#include "transact.h"

#ifdef __cplusplus
extern "C"{
#endif 

void * __RPC_USER MIDL_user_allocate(size_t);
void __RPC_USER MIDL_user_free( void * ); 

#ifndef __ITransactionEventConsumer_INTERFACE_DEFINED__
#define __ITransactionEventConsumer_INTERFACE_DEFINED__

/* interface ITransactionEventConsumer */
/* [unique][helpstring][dual][uuid][object] */ 


EXTERN_C const IID IID_ITransactionEventConsumer;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("C693EA9A-E5A0-4318-9C88-CC5E9F61677B")
    ITransactionEventConsumer : public IDispatch
    {
    public:
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ManagedObjectConsumer( 
            /* [retval][out] */ IUnknown **pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_ManagedObjectConsumer( 
            /* [in] */ IUnknown *newVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Transaction( 
            /* [in] */ IUnknown *newVal) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct ITransactionEventConsumerVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            ITransactionEventConsumer * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            ITransactionEventConsumer * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            ITransactionEventConsumer * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            ITransactionEventConsumer * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            ITransactionEventConsumer * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            ITransactionEventConsumer * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            ITransactionEventConsumer * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ManagedObjectConsumer )( 
            ITransactionEventConsumer * This,
            /* [retval][out] */ IUnknown **pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_ManagedObjectConsumer )( 
            ITransactionEventConsumer * This,
            /* [in] */ IUnknown *newVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Transaction )( 
            ITransactionEventConsumer * This,
            /* [in] */ IUnknown *newVal);
        
        END_INTERFACE
    } ITransactionEventConsumerVtbl;

    interface ITransactionEventConsumer
    {
        CONST_VTBL struct ITransactionEventConsumerVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ITransactionEventConsumer_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define ITransactionEventConsumer_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define ITransactionEventConsumer_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define ITransactionEventConsumer_GetTypeInfoCount(This,pctinfo)	\
    (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo)

#define ITransactionEventConsumer_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo)

#define ITransactionEventConsumer_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)

#define ITransactionEventConsumer_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)


#define ITransactionEventConsumer_get_ManagedObjectConsumer(This,pVal)	\
    (This)->lpVtbl -> get_ManagedObjectConsumer(This,pVal)

#define ITransactionEventConsumer_put_ManagedObjectConsumer(This,newVal)	\
    (This)->lpVtbl -> put_ManagedObjectConsumer(This,newVal)

#define ITransactionEventConsumer_put_Transaction(This,newVal)	\
    (This)->lpVtbl -> put_Transaction(This,newVal)

#endif /* COBJMACROS */


#endif 	/* C style interface */



/* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE ITransactionEventConsumer_get_ManagedObjectConsumer_Proxy( 
    ITransactionEventConsumer * This,
    /* [retval][out] */ IUnknown **pVal);


void __RPC_STUB ITransactionEventConsumer_get_ManagedObjectConsumer_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE ITransactionEventConsumer_put_ManagedObjectConsumer_Proxy( 
    ITransactionEventConsumer * This,
    /* [in] */ IUnknown *newVal);


void __RPC_STUB ITransactionEventConsumer_put_ManagedObjectConsumer_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE ITransactionEventConsumer_put_Transaction_Proxy( 
    ITransactionEventConsumer * This,
    /* [in] */ IUnknown *newVal);


void __RPC_STUB ITransactionEventConsumer_put_Transaction_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __ITransactionEventConsumer_INTERFACE_DEFINED__ */



#ifndef __TransactionEventConsumerPrj_LIBRARY_DEFINED__
#define __TransactionEventConsumerPrj_LIBRARY_DEFINED__

/* library TransactionEventConsumerPrj */
/* [helpstring][uuid][version] */ 


EXTERN_C const IID LIBID_TransactionEventConsumerPrj;

EXTERN_C const CLSID CLSID_CTransactionEventConsumer;

#ifdef __cplusplus

class DECLSPEC_UUID("F2263E18-DB20-4418-9A27-B761DD4E9690")
CTransactionEventConsumer;
#endif
#endif /* __TransactionEventConsumerPrj_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


