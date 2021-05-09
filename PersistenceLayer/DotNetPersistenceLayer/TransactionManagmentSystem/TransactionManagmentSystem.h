// TransactionManagmentSystem.h : main header file for the TransactionManagmentSystem DLL
//

#pragma once

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"		// main symbols


// CTransactionManagmentSystemApp
// See TransactionManagmentSystem.cpp for the implementation of this class
//

class CTransactionManagmentSystemApp : public CWinApp
{
public:
	CTransactionManagmentSystemApp();

// Overrides
public:
	virtual BOOL InitInstance();

	DECLARE_MESSAGE_MAP()
};
