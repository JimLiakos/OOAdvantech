// Connect.h : Declaration of the CConnect

#pragma once
#include "resource.h"       // main symbols

#pragma warning( disable : 4278 )
//The following #import imports DTE based on it's LIBID
#import "libid:80cc9f66-e7d8-4ddd-85b6-d9e6cd0e93e2" version("8.0") lcid("0")  rename("DTEEvents","MDTEEvents")
//raw_interfaces_only named_guidsre
#pragma warning( default : 4278 )
//#import "C:\Program Files\Rational\Rose\RationalRose.tlb" nonamespace
//#import "x:\Source\PersistenceLayer\DotNetMetaObjects\Debug\DotNetMetaObjects.dll" nonamespace
#import "X:\source\Visual Studio.NET Projects\CipboardLoaderPrj\CipboardLoaderPrj.dll"
#include "X:\source\RoseVisual.Net Studio Integrator\RoseBridgeVisualStudioDotNet\RoseBridgeVisualStudioDotNet.h"
#import "MSXML3.dll"
#import "Wshom.ocx" rename("FreeSpace","DiskFreeSpace")


 
// CConnect
class ATL_NO_VTABLE CConnect : 
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CConnect, &CLSID_Connect>,
	
	public IDispatchImpl<AddInDesignerObjects::_IDTExtensibility2, &AddInDesignerObjects::IID__IDTExtensibility2, &AddInDesignerObjects::LIBID_AddInDesignerObjects, 1, 0>
{
public:
	CConnect()
	{
	}

DECLARE_REGISTRY_RESOURCEID(IDR_ADDIN)
DECLARE_NOT_AGGREGATABLE(CConnect)



BEGIN_COM_MAP(CConnect)
	COM_INTERFACE_ENTRY(IDispatch)
	COM_INTERFACE_ENTRY(AddInDesignerObjects::IDTExtensibility2)
END_COM_MAP()


	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct()
	{
		return S_OK;
	}
	
	void FinalRelease() 
	{
	}
	class CCommandBarControlEventsSink : public IDispEventImpl<1, CCommandBarControlEventsSink, &__uuidof(EnvDTE::_dispCommandBarControlEvents), &__uuidof(EnvDTE::__EnvDTE), 7, 0>
	{
	public:
		BEGIN_SINK_MAP(CCommandBarControlEventsSink)
			SINK_ENTRY_EX(1, __uuidof(EnvDTE::_dispCommandBarControlEvents), 1, Click)
		END_SINK_MAP()
	public:
		CComPtr<EnvDTE::_DTE> m_pDTE;
		DotNetSynchronizerPtr pDotNetSynchronizer;
		

		void __stdcall Click(IDispatch* CommandBarControl, VARIANT_BOOL* handled, VARIANT_BOOL* CancelDefault)
		{

			EnvDTE::_DTEPtr applicationObject=m_pDTE.p;
			try
			{
			

				EnvDTE::TextSelectionPtr mTextSelection=applicationObject->ActiveDocument->Selection;
				EnvDTE::TextPointPtr TextPoint=mTextSelection->TopPoint;
				EnvDTE::CodeElementPtr  mCodeElement=TextPoint->GetCodeElement(EnvDTE::vsCMElementFunction);
				if(mCodeElement==NULL)
				{
					mCodeElement=TextPoint->GetCodeElement(EnvDTE::vsCMElementProperty);
					if(mCodeElement==NULL)
					{
						mCodeElement=TextPoint->GetCodeElement(EnvDTE::vsCMElementVariable);
						if(mCodeElement==NULL)
						{
							mCodeElement=TextPoint->GetCodeElement(EnvDTE::vsCMElementClass);
							if(mCodeElement==NULL)
							{
								mCodeElement=TextPoint->GetCodeElement(EnvDTE::vsCMElementInterface);
							}
						}
					}

				}
				if(mCodeElement==NULL)
					return;
			
				String MemberID;
				

				if(mCodeElement!=NULL&&mCodeElement->Kind==EnvDTE::vsCMElementFunction)
				{
					EnvDTE::CodeFunctionPtr mCodeFunction=mCodeElement;
					mCodeElement=mCodeFunction->Parent;
					MSXML2::IXMLDOMDocumentPtr mXmlDocument(__uuidof(MSXML2::DOMDocument30));
					mXmlDocument->loadXML(mCodeFunction->GetDocComment());
					MSXML2::IXMLDOMNodePtr mXmlNode=mXmlDocument->firstChild->selectSingleNode("//MetaDataID");
					if(mXmlNode!=NULL)
						MemberID=mXmlNode->text;
				}

				if(mCodeElement->Kind==EnvDTE::vsCMElementProperty)
				{
					EnvDTE::CodePropertyPtr mCodeProperty=mCodeElement;
					mCodeElement=mCodeProperty->Parent;
					MSXML2::IXMLDOMDocumentPtr mXmlDocument(__uuidof(MSXML2::DOMDocument30));
					mXmlDocument->loadXML(mCodeProperty->GetDocComment());
					MSXML2::IXMLDOMNodePtr mXmlNode=mXmlDocument->firstChild->selectSingleNode("//MetaDataID");
					if(mXmlNode!=NULL)
						MemberID=mXmlNode->text;


				
				}
				if(mCodeElement->Kind==EnvDTE::vsCMElementVariable)
				{
					EnvDTE::CodeVariablePtr mCodeVariable=mCodeElement;
					mCodeElement=mCodeVariable->Parent;

					MSXML2::IXMLDOMDocumentPtr mXmlDocument(__uuidof(MSXML2::DOMDocument30));
					mXmlDocument->loadXML(mCodeVariable->GetDocComment());
					MSXML2::IXMLDOMNodePtr mXmlNode=mXmlDocument->firstChild->selectSingleNode("//MetaDataID");
					if(mXmlNode!=NULL)
						MemberID=mXmlNode->text;
				}
					

				if(mCodeElement->Kind==EnvDTE::vsCMElementClass||mCodeElement->Kind==EnvDTE::vsCMElementInterface)
				{ 
					String ClassID;

					EnvDTE::CodeClassPtr mCodeClass=mCodeElement;
					EnvDTE::CodeInterfacePtr mCodeInterface=mCodeElement;
					MSXML2::IXMLDOMDocumentPtr mXmlDocument(__uuidof(MSXML2::DOMDocument30));
					if(mCodeClass!=NULL)
						mXmlDocument->loadXML(mCodeClass->GetDocComment());
					if(mCodeInterface!=NULL)
						mXmlDocument->loadXML(mCodeInterface->GetDocComment());

					MSXML2::IXMLDOMNodePtr mXmlNode=mXmlDocument->firstChild->selectSingleNode("//MetaDataID");
					if(mXmlNode!=NULL)
					{
					
						ClassID=mXmlNode->text;
						
						if(pDotNetSynchronizer!=NULL)
						{
							pDotNetSynchronizer->BrowseRoseItem(ClassID,MemberID);
							IWshRuntimeLibrary::IWshShell2Ptr WshShell(__uuidof(IWshRuntimeLibrary::WshShell));
							_variant_t ApplicationName=pDotNetSynchronizer->RoseTitleBar;
							_variant_t Wait;
							WshShell->AppActivate(&ApplicationName,&vtMissing);
						}
					}
				}
			}
			catch(...)
			{
			}
		}
	};


public:
	//IDTExtensibility2 implementation:
	STDMETHOD(OnConnection)(IDispatch * Application, AddInDesignerObjects::ext_ConnectMode ConnectMode, IDispatch *AddInInst, SAFEARRAY **custom);
	STDMETHOD(OnDisconnection)(AddInDesignerObjects::ext_DisconnectMode RemoveMode, SAFEARRAY **custom );
	STDMETHOD(OnAddInsUpdate)(SAFEARRAY **custom );
	STDMETHOD(OnStartupComplete)(SAFEARRAY **custom );
	STDMETHOD(OnBeginShutdown)(SAFEARRAY **custom );
	
	

	CComPtr<EnvDTE::_DTE> m_pDTE;
	CComPtr<EnvDTE::AddIn> m_pAddInInstance;
	CCommandBarControlEventsSink m_CommandBarControlEventsSink;
	CComPtr<EnvDTE::_CommandBarControlEvents> m_pCommandBarEvents;
	DWORD m_dwCookie;
	DotNetSynchronizerPtr pDotNetSynchronizer;

	
};

OBJECT_ENTRY_AUTO(__uuidof(Connect), CConnect)
