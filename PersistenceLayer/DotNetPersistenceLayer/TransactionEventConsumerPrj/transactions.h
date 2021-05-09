// Created by Microsoft (R) C/C++ Compiler Version 13.10.3077 (fe554c7e).
//
// x:\source\persistencelayer\dotnetpersistencelayer\transactioneventconsumerprj\debug\transactions.tlh
//
// C++ source equivalent of Win32 type library PersistenceLayer\DotNetPersistenceLayer\Transactions\bin\debug\Transactions.tlb
// compiler-generated file created 10/09/04 at 09:54:53 - DO NOT EDIT!

#pragma once
#pragma pack(push, 8)

#include <comdef.h>

namespace Transactions {

//
// Forward references and typedefs
//

struct __declspec(uuid("4fba74e9-4b91-3d17-bb7f-916abd294708"))
/* LIBID */ __Transactions;
struct __declspec(uuid("799d5869-e7de-41b4-8040-102417983710"))
	/* dual interface */ ITransactionOutcomeEvents;

_COM_SMARTPTR_TYPEDEF(ITransactionOutcomeEvents, __uuidof(ITransactionOutcomeEvents));

//
// Type library items
//
enum __declspec(uuid("df0998da-22f3-30b0-a397-3bd8d35b0043"))
XACTHEURISTIC
{
    XACTHEURISTIC_XACTHEURISTIC_ABORT = 0,
    XACTHEURISTIC_XACTHEURISTIC_COMMIT = 1,
    XACTHEURISTIC_XACTHEURISTIC_DAMAGE = 2,
    XACTHEURISTIC_XACTHEURISTIC_DANGER = 3
};

struct __declspec(uuid("799d5869-e7de-41b4-8040-102417983710"))
ITransactionOutcomeEvents : IDispatch
{
    //
    // Wrapper methods for error-handling
    //

    HRESULT Aborted (
        _bstr_t Reason );
    HRESULT Committed ( );
    HRESULT HeuristicDecision (
        enum XACTHEURISTIC dwDecision,
        _bstr_t Reason );
    HRESULT Indoubt ( );

    //
    // Raw methods provided by interface
    //

      virtual HRESULT __stdcall raw_Aborted (
        /*[in]*/ BSTR Reason ) = 0;
      virtual HRESULT __stdcall raw_Committed ( ) = 0;
      virtual HRESULT __stdcall raw_HeuristicDecision (
        /*[in]*/ enum XACTHEURISTIC dwDecision,
        /*[in]*/ BSTR Reason ) = 0;
      virtual HRESULT __stdcall raw_Indoubt ( ) = 0;
};


//
// Wrapper method implementations
//

#include "transactions.cpp"

} // namespace Transactions

#pragma pack(pop)
