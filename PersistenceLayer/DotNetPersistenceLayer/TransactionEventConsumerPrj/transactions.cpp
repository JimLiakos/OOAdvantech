// Created by Microsoft (R) C/C++ Compiler Version 13.10.3077 (fe554c7e).
//
// x:\source\persistencelayer\dotnetpersistencelayer\transactioneventconsumerprj\debug\transactions.tli
//
// Wrapper implementations for Win32 type library PersistenceLayer\DotNetPersistenceLayer\Transactions\bin\debug\Transactions.tlb
// compiler-generated file created 10/09/04 at 09:54:53 - DO NOT EDIT!

#pragma once

//
// interface ITransactionEnlistment wrapper method implementations
//

//
// interface ITransactionOutcomeEvents wrapper method implementations
//

inline HRESULT ITransactionOutcomeEvents::Aborted ( _bstr_t Reason ) {
    HRESULT _hr = raw_Aborted(Reason);
    if (FAILED(_hr)) _com_issue_errorex(_hr, this, __uuidof(this));
    return _hr;
}

inline HRESULT ITransactionOutcomeEvents::Committed ( ) {
    HRESULT _hr = raw_Committed();
    if (FAILED(_hr)) _com_issue_errorex(_hr, this, __uuidof(this));
    return _hr;
}

inline HRESULT ITransactionOutcomeEvents::HeuristicDecision ( enum XACTHEURISTIC dwDecision, _bstr_t Reason ) {
    HRESULT _hr = raw_HeuristicDecision(dwDecision, Reason);
    if (FAILED(_hr)) _com_issue_errorex(_hr, this, __uuidof(this));
    return _hr;
}

inline HRESULT ITransactionOutcomeEvents::Indoubt ( ) {
    HRESULT _hr = raw_Indoubt();
    if (FAILED(_hr)) _com_issue_errorex(_hr, this, __uuidof(this));
    return _hr;
}
