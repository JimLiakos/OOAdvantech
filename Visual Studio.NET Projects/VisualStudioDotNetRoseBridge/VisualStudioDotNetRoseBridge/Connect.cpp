// Connect.cpp : Implementation of CConnect
#include "stdafx.h"
#include "AddIn.h"
#include "Connect.h"
#include "X:\source\RoseVisual.Net Studio Integrator\COMGeneralClasses\COMGeneralClasses.h"
#include <string>
extern CAddInModule _AtlModule;
#import "DotNetClient.tlb"

// When run, the Add-in wizard prepared the registry for the Add-in.
// At a later time, if the Add-in becomes unavailable for reasons such as:
//   1) You moved this project to a computer other than which is was originally created on.
//   2) You chose 'Yes' when presented with a message asking if you wish to remove the Add-in.
//   3) Registry corruption.
// you will need to re-register the Add-in by building the MyAddin21Setup project 
// by right clicking the project in the Solution Explorer, then choosing install.


// CConnect
STDMETHODIMP CConnect::OnConnection(IDispatch *pApplication, AddInDesignerObjects::ext_ConnectMode ConnectMode, IDispatch *pAddInInst, SAFEARRAY ** /*custom*/ )
{
	HRESULT hr = S_OK;
	pApplication->QueryInterface(__uuidof(EnvDTE::_DTE), (LPVOID*)&m_pDTE);
	pAddInInst->QueryInterface(__uuidof(EnvDTE::AddIn), (LPVOID*)&m_pAddInInstance);
	ClipboardLoaderPrj::_ClipboarBitmapLoaderPtr pClipboarBitmapLoader(__uuidof(ClipboardLoaderPrj::ClipboarBitmapLoader));
	//_bstr_t Filename=;
	HKEY hKey=NULL;
	String  LockRegistryPath="CLSID\\{92F667F6-BC55-4855-9FEE-1EA5AEBBEBCE}\\InprocServer32\\";
	std::string  Path;

	long Resault=RegOpenKeyEx(HKEY_CLASSES_ROOT,(TCHAR*)LockRegistryPath,0,KEY_ALL_ACCESS ,&hKey);
	if(Resault==ERROR_SUCCESS)
	{
		ULONG ByteLength=0;
		DWORD Type=REG_SZ ;
		RegQueryValueEx(hKey,NULL,0,&Type,NULL,&ByteLength);
		BYTE* ByteBuffer=new BYTE[ByteLength+1];
		RegQueryValueEx(hKey,NULL,0,&Type,ByteBuffer,&ByteLength);
		ByteBuffer[ByteLength]=0;
		Path=(char*)ByteBuffer;
		delete[] ByteBuffer;
		long nPos=Path.find("VisualStudioDotNetRoseBridge.dll");
		if(nPos!=-1)
		{
			Path=Path.substr(0,nPos);
			Path+="Browse Model.bmp";
		}
		else
			Path.clear();

	}
	CComBSTR Filename=Path.c_str();
	try
	{
		pClipboarBitmapLoader->LoadBitmapFromFile(&Filename);
	}
	catch(_com_error Error)
	{
	}
	
	RuntimeObjectTablePtr pRuntimeObjectTable(__uuidof(_RuntimeObjectTable));
	pDotNetSynchronizer=_dynamic_cast<DotNetSynchronizerPtr>(pRuntimeObjectTable->GetObject("{18F9E380-E8DB-4d42-9B5B-5F3128E089FD}"));
	if(pDotNetSynchronizer!=NULL)
	{
		pRuntimeObjectTable->UnRegisterObject("{18F9E380-E8DB-4d42-9B5B-5F3128E089FD}");
		pDotNetSynchronizer->SetDTE(pApplication);/**/

	}

	DotNetClient::IDotNetClientPtr DotNetClien=pRuntimeObjectTable->GetObject("{2C71EA9E-0F06-449c-B3F8-2A41B2A9CA9A}");
	if(DotNetClien!=NULL)
	{
		pRuntimeObjectTable->UnRegisterObject("{2C71EA9E-0F06-449c-B3F8-2A41B2A9CA9A}");
		DotNetClien->SetDTE(pApplication);/**/
	}


	m_CommandBarControlEventsSink.m_pDTE=m_pDTE;
	m_CommandBarControlEventsSink.pDotNetSynchronizer=pDotNetSynchronizer;



	CComPtr<Office::_CommandBars> pCommandBars;
	CComPtr<Office::CommandBar> pToolsCommandBar;
	CComPtr<Office::CommandBarControls> pCommandBarControls;
	CComPtr<Office::CommandBarControl> pCommandBarControl;
	Office::_CommandBarButtonPtr pCommandBarButton;
	

	
	
	
	CComPtr<EnvDTE::Events> pEvents;
	CComVariant varFalse;
	varFalse.vt = VT_BOOL;
	varFalse.boolVal = VARIANT_FALSE;


	m_pDTE->get_Events(&pEvents);
	m_pDTE->get_CommandBars((IDispatch**)&pCommandBars);
	pCommandBars->get_Item(CComVariant(L"Code Window"), &pToolsCommandBar);
	pToolsCommandBar->get_Controls(&pCommandBarControls);
	pCommandBarControls->Add(CComVariant(Office::msoControlButton), CComVariant(1), CComVariant(), CComVariant(30), varFalse, &pCommandBarControl);

	pCommandBarControl->put_Visible(VARIANT_TRUE);
	pCommandBarControl->put_Caption(CComBSTR("Browse Model"));
	
	pCommandBarButton=pCommandBarControl.p;
	try
	{
		pCommandBarButton->PasteFace();
	}
	catch(...)
	{
	}

	if(SUCCEEDED(pEvents->get_CommandBarEvents(pCommandBarControl, (IDispatch**)&m_pCommandBarEvents)))
	{
		m_CommandBarControlEventsSink.DispEventAdvise((IUnknown*)m_pCommandBarEvents.p);
	}
	return hr;
	
}

STDMETHODIMP CConnect::OnDisconnection(AddInDesignerObjects::ext_DisconnectMode /*RemoveMode*/, SAFEARRAY ** /*custom*/ )
{
	
	m_CommandBarControlEventsSink.m_pDTE=NULL;
	m_CommandBarControlEventsSink.pDotNetSynchronizer=NULL;

	m_CommandBarControlEventsSink.DispEventUnadvise((IUnknown*)m_pCommandBarEvents.p);
	m_pDTE = NULL;
	m_pCommandBarEvents = NULL;
	pDotNetSynchronizer=NULL;

	return S_OK;
}

STDMETHODIMP CConnect::OnAddInsUpdate (SAFEARRAY ** /*custom*/ )
{
	return S_OK;
}

STDMETHODIMP CConnect::OnStartupComplete (SAFEARRAY ** /*custom*/ )
{
	return S_OK;
}

STDMETHODIMP CConnect::OnBeginShutdown (SAFEARRAY ** /*custom*/ )
{
	return S_OK;
}

