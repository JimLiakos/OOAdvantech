// AssemblyCryptoNativeCode.cpp : Defines the initialization routines for the DLL.
//

#include "stdafx.h"
#include "AssemblyCryptoNativeCode.h"
#include "atlsafe.h"
#import "C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\mscorlib.tlb" no_namespace
//typedef mscorlib::_AppDomainPtr _AppDomainPtr;
//typedef mscorlib::_AppDomain _AppDomain;
#import "..\..\AssemblyCrypto\bin\Debug\AssemblyNativeCode.tlb"
#include "Crypto.h"



#ifdef _DEBUG
#define new DEBUG_NEW
#endif
 
//
//TODO: If this DLL is dynamically linked against the MFC DLLs,
//		any functions exported from this DLL which call into
//		MFC must have the AFX_MANAGE_STATE macro added at the
//		very beginning of the function.
//
//		For example:
//
//		extern "C" BOOL PASCAL EXPORT ExportedFunction()
//		{
//			AFX_MANAGE_STATE(AfxGetStaticModuleState());
//			// normal function body here
//		}
//
//		It is very important that this macro appear in each
//		function, prior to any calls into MFC.  This means that
//		it must appear as the first statement within the 
//		function, even before any object variable declarations
//		as their constructors may generate calls into the MFC
//		DLL.
//
//		Please see MFC Technical Notes 33 and 58 for additional
//		details.
//


// CAssemblyCryptoNativeCodeApp

BEGIN_MESSAGE_MAP(CAssemblyCryptoNativeCodeApp, CWinApp)
END_MESSAGE_MAP()


// CAssemblyCryptoNativeCodeApp construction

CAssemblyCryptoNativeCodeApp::CAssemblyCryptoNativeCodeApp()
{
	// TODO: add construction code here,
	// Place all significant initialization in InitInstance
}


// The one and only CAssemblyCryptoNativeCodeApp object

CAssemblyCryptoNativeCodeApp theApp;
void Clear(CByteArray& arData)
{
	CByteArray outData;
	outData.SetSize(arData.GetSize());
	for(int i=0;i<arData.GetSize();i++)
		outData.SetAt(i,arData.GetAt(arData.GetSize()-1-i));
	arData.RemoveAll();
	arData.SetSize(0);
	arData.Append(outData);
}

// CAssemblyCryptoNativeCodeApp initialization

BOOL CAssemblyCryptoNativeCodeApp::InitInstance()
{

	CWinApp::InitInstance();

	return TRUE;
}

STDAPI MakeNative(WCHAR* fileName)
{
	MFC::CCrypto m_crypto;
	char* pass=new char [33];
	
	pass[0]='C';
	pass[1]='4';
	pass[2]='C';
	pass[3]='2';
	pass[4]='C';
	pass[5]='E';
	pass[6]='E';
	pass[7]='C';
	pass[8]='0';
	pass[9]='6';
	pass[10]='B';

	pass[11]='6';
	pass[12]='4';
	pass[13]='2';
	pass[14]='a';
	pass[15]='0';
	pass[16]='A';
	pass[17]='5';
	pass[18]='5';
	pass[19]='6';
	pass[20]='F';
	pass[21]='F';
	pass[22]='7';
	pass[23]='3';
	pass[24]='E';

	pass[25]='3';
	pass[26]='A';
	pass[27]='E';
	pass[28]='B';
	pass[29]='C';
	pass[30]='F';
	pass[31]='1';
	pass[32]=0;
	pass[33]=0;
	_bstr_t passw(pass);
	m_crypto.DeriveKey(passw.GetBSTR());

	
	CFile m_file;
	m_file.Open(fileName,CFile::modeReadWrite,NULL);

	
	ULONG length =m_file.GetLength();
	BYTE* buffer=new BYTE[length];
	m_file.Read(buffer,length);

	CByteArray inData; 
	inData.SetSize(length);
	for(int i=0;i<length;i++)
		inData.SetAt(i,buffer[i]);

	delete buffer;
	CByteArray arData;
	m_file.Close();
	m_crypto.Encrypt(inData,arData);
	DeleteFile(fileName);

	m_file.Open(fileName,CFile::modeReadWrite|CFile::modeCreate,NULL);

	m_file.SetLength(0);
	Clear(inData);
	//m_file.Write(arData.GetData(), static_cast<UINT>(arData.GetCount()));
	m_file.Write(inData.GetData(), static_cast<UINT>(inData.GetCount()));
	
	m_file.Flush();
	m_file.Close();
	
    return 0;
}

STDAPI LoadAssemblyDebug(WCHAR* assemblyFileName,WCHAR* debugInfoFileName,_variant_t nativeCodeBridgeObject)
{
	AssemblyNativeCode::INativeCodeBridgePtr assemblyCryptoBridge(nativeCodeBridgeObject);
	//AfxMessageBox(L"LoadAssemblyDebug");
	CFile m_file;
	

	CByteArray arData;
	m_file.Open(assemblyFileName,CFile::modeRead,NULL);
	arData.SetSize(static_cast<INT_PTR>(m_file.GetLength()));
	m_file.Read(arData.GetData(), static_cast<UINT>(m_file.GetLength()));
	m_file.Close();

	CByteArray debugInfoData;
	m_file.Open(debugInfoFileName,CFile::modeRead,NULL);
	debugInfoData.SetSize(static_cast<INT_PTR>(m_file.GetLength()));
	m_file.Read(debugInfoData.GetData(), static_cast<UINT>(m_file.GetLength()));
	m_file.Close();

	CByteArray outData;

	MFC::CCrypto m_crypto;
	char* pass=new char [33];
	
	pass[0]='C';
	pass[1]='4';
	pass[2]='C';
	pass[3]='2';
	pass[4]='C';
	pass[5]='E';
	pass[6]='E';
	pass[7]='C';
	pass[8]='0';
	pass[9]='6';
	pass[10]='B';

	pass[11]='6';
	pass[12]='4';
	pass[13]='2';
	pass[14]='a';
	pass[15]='0';
	pass[16]='A';
	pass[17]='5';
	pass[18]='5';
	pass[19]='6';
	pass[20]='F';
	pass[21]='F';
	pass[22]='7';
	pass[23]='3';
	pass[24]='E';

	pass[25]='3';
	pass[26]='A';
	pass[27]='E';
	pass[28]='B';
	pass[29]='C';
	pass[30]='F';
	pass[31]='1';
	pass[32]=0;
	pass[33]=0;
	_bstr_t passw(pass);
	m_crypto.DeriveKey(passw.GetBSTR());
	int tt= arData.GetSize();
	m_crypto.Decrypt(arData, outData) ;
		//throw CString("System can't load Assembly");
//	CFile m_fileout;
//	m_fileout.Open(L"C:\\tmp.dll",CFile::modeCreate|CFile::modeReadWrite,NULL);
	outData.RemoveAll();
	outData.SetSize(0);
	

	Clear(arData);
	outData.Append(arData);
//	m_fileout.Write(arData.GetData(),arData.GetCount());
//	m_fileout.Close();




	 // Define an array of character pointers
   CComSafeArray<BYTE> *pSar;

   
  
   // Define the array bound structure
   CComSafeArrayBound bound[1];
   bound[0].SetCount(outData.GetSize());
   bound[0].SetLowerBound(0);
   
   // Create a new 2 dimensional array
   // each dimension size is 3
   pSar = new CComSafeArray<BYTE>(bound,1); 
   for(int i=0;i!=outData.GetSize();i++)
	   pSar->SetAt(i,outData.GetAt(i));


	CComSafeArray<BYTE> *pdebugInfoSar;

   
  
   // Define the array bound structure
   CComSafeArrayBound debegInfoBound[1];
   debegInfoBound[0].SetCount(debugInfoData.GetSize());
   debegInfoBound[0].SetLowerBound(0);
   
   // Create a new 2 dimensional array
   // each dimension size is 3
   pdebugInfoSar = new CComSafeArray<BYTE>(debegInfoBound,1); 
   for(int i=0;i!=debugInfoData.GetSize();i++)
	   pdebugInfoSar->SetAt(i,debugInfoData.GetAt(i));

   

//AfxMessageBox(L"assemblyCryptoBridge->AssemblyLoaded");
   //AssemblyNativeCode::INativeCodeBridgePtr assemblyCryptoBridge(__uuidof( AssemblyNativeCode::NativeCodeBridge));
	assemblyCryptoBridge->AssemblyLoaded( assemblyCryptoBridge->CurrentDomain->Load_4(pSar->m_psa,pdebugInfoSar->m_psa));
	pSar->Destroy();
	pdebugInfoSar->Destroy();

	//AfxMessageBox(fileName);
    return 0;
}



STDAPI LoadAssembly(WCHAR* fileName,_variant_t nativeCodeBridgeObject)
{
	AssemblyNativeCode::INativeCodeBridgePtr assemblyCryptoBridge(nativeCodeBridgeObject);
	CFile m_file;
	m_file.Open(fileName,CFile::modeRead,NULL);
 
	CByteArray arData;
	arData.SetSize(static_cast<INT_PTR>(m_file.GetLength()));
	m_file.Read(arData.GetData(), static_cast<UINT>(m_file.GetLength()));
	m_file.Close();
	CByteArray outData;

	MFC::CCrypto m_crypto;
	char* pass=new char [33];
	
	pass[0]='C';
	pass[1]='4';
	pass[2]='C';
	pass[3]='2';
	pass[4]='C';
	pass[5]='E';
	pass[6]='E';
	pass[7]='C';
	pass[8]='0';
	pass[9]='6';
	pass[10]='B';

	pass[11]='6';
	pass[12]='4';
	pass[13]='2';
	pass[14]='a';
	pass[15]='0';
	pass[16]='A';
	pass[17]='5';
	pass[18]='5';
	pass[19]='6';
	pass[20]='F';
	pass[21]='F';
	pass[22]='7';
	pass[23]='3';
	pass[24]='E';

	pass[25]='3';
	pass[26]='A';
	pass[27]='E';
	pass[28]='B';
	pass[29]='C';
	pass[30]='F';
	pass[31]='1';
	pass[32]=0;
	pass[33]=0;
	_bstr_t passw(pass);
	m_crypto.DeriveKey(passw.GetBSTR());
	int tt= arData.GetSize();
	
	m_crypto.Decrypt(arData, outData) ;

		//throw CString("System can't load Assembly");
//	CFile m_fileout;
//	m_fileout.Open(L"C:\\tmp.dll",CFile::modeCreate|CFile::modeReadWrite,NULL);
	outData.RemoveAll();
	outData.SetSize(0);

	Clear(arData);
	outData.Append(arData);
//	m_fileout.Write(arData.GetData(),arData.GetCount());
//	m_fileout.Close();




	 // Define an array of character pointers
   CComSafeArray<BYTE> *pSar;

   
  
   // Define the array bound structure
   CComSafeArrayBound bound[1];
   bound[0].SetCount(outData.GetSize());
   bound[0].SetLowerBound(0);
   
   // Create a new 2 dimensional array
   // each dimension size is 3
   pSar = new CComSafeArray<BYTE>(bound,1); 
   for(int i=0;i!=outData.GetSize();i++)
	   pSar->SetAt(i,outData.GetAt(i));
   


   //AssemblyNativeCode::INativeCodeBridgePtr assemblyCryptoBridge(__uuidof( AssemblyNativeCode::NativeCodeBridge));
	assemblyCryptoBridge->AssemblyLoaded( assemblyCryptoBridge->CurrentDomain->Load_3(pSar->m_psa));
	pSar->Destroy();

	//AfxMessageBox(fileName);
    return 0;
}





STDAPI LoadRawAssembly(BYTE* rawAssembly,int rawAssemblySize,_variant_t nativeCodeBridgeObject)
{
	AssemblyNativeCode::INativeCodeBridgePtr assemblyCryptoBridge(nativeCodeBridgeObject);

	CByteArray arData;
	arData.SetSize(static_cast<INT_PTR>(rawAssemblySize));
	for(int i=0;i!=rawAssemblySize;i++)
		arData.SetAt(static_cast<INT_PTR>(i),rawAssembly[i]);
	
	CByteArray outData;

	MFC::CCrypto m_crypto;
	char* pass=new char [33];
	
	pass[0]='C';
	pass[1]='4';
	pass[2]='C';
	pass[3]='2';
	pass[4]='C';
	pass[5]='E';
	pass[6]='E';
	pass[7]='C';
	pass[8]='0';
	pass[9]='6';
	pass[10]='B';

	pass[11]='6';
	pass[12]='4';
	pass[13]='2';
	pass[14]='a';
	pass[15]='0';
	pass[16]='A';
	pass[17]='5';
	pass[18]='5';
	pass[19]='6';
	pass[20]='F';
	pass[21]='F';
	pass[22]='7';
	pass[23]='3';
	pass[24]='E';

	pass[25]='3';
	pass[26]='A';
	pass[27]='E';
	pass[28]='B';
	pass[29]='C';
	pass[30]='F';
	pass[31]='1';
	pass[32]=0;
	pass[33]=0;
	_bstr_t passw(pass);
	m_crypto.DeriveKey(passw.GetBSTR());
	int tt= arData.GetSize();
	
	m_crypto.Decrypt(arData, outData) ;

		//throw CString("System can't load Assembly");
//	CFile m_fileout;
//	m_fileout.Open(L"C:\\tmp.dll",CFile::modeCreate|CFile::modeReadWrite,NULL);
	outData.RemoveAll();
	outData.SetSize(0);

	Clear(arData);
	outData.Append(arData);
//	m_fileout.Write(arData.GetData(),arData.GetCount());
//	m_fileout.Close();




	 // Define an array of character pointers
   CComSafeArray<BYTE> *pSar;

   
  
   // Define the array bound structure
   CComSafeArrayBound bound[1];
   bound[0].SetCount(outData.GetSize());
   bound[0].SetLowerBound(0);
   
   // Create a new 2 dimensional array
   // each dimension size is 3
   pSar = new CComSafeArray<BYTE>(bound,1); 
   for(int i=0;i!=outData.GetSize();i++)
	   pSar->SetAt(i,outData.GetAt(i));
   


   //AssemblyNativeCode::INativeCodeBridgePtr assemblyCryptoBridge(__uuidof( AssemblyNativeCode::NativeCodeBridge));
	assemblyCryptoBridge->AssemblyLoaded( assemblyCryptoBridge->CurrentDomain->Load_3(pSar->m_psa));
	pSar->Destroy();

	//AfxMessageBox(fileName);
    return 0;
}



STDAPI LoadRawAssemblyDebug(BYTE* rawAssembly,int rawAssemblySize,BYTE* rawDebugInfo,int rawDebugInfoSize,_variant_t nativeCodeBridgeObject)
{
	AssemblyNativeCode::INativeCodeBridgePtr assemblyCryptoBridge(nativeCodeBridgeObject);
	//AfxMessageBox(L"LoadAssemblyDebug");
	//CFile m_file;
	

	CByteArray arData;
	arData.SetSize(static_cast<INT_PTR>(rawAssemblySize));
	for(int i=0;i!=rawAssemblySize;i++)
		arData.SetAt(static_cast<INT_PTR>(i),rawAssembly[i]);

	//m_file.Open(assemblyFileName,CFile::modeRead,NULL);
	//arData.SetSize(static_cast<INT_PTR>(m_file.GetLength()));
	//m_file.Read(arData.GetData(), static_cast<UINT>(m_file.GetLength()));
	//m_file.Close();

	CByteArray debugInfoData;

	
	debugInfoData.SetSize(static_cast<INT_PTR>(rawDebugInfoSize));
	for(int i=0;i!=rawDebugInfoSize;i++)
		debugInfoData.SetAt(static_cast<INT_PTR>(i),rawDebugInfo[i]);

	CByteArray outData;

	MFC::CCrypto m_crypto;
	char* pass=new char [33];
	
	pass[0]='C';
	pass[1]='4';
	pass[2]='C';
	pass[3]='2';
	pass[4]='C';
	pass[5]='E';
	pass[6]='E';
	pass[7]='C';
	pass[8]='0';
	pass[9]='6';
	pass[10]='B';

	pass[11]='6';
	pass[12]='4';
	pass[13]='2';
	pass[14]='a';
	pass[15]='0';
	pass[16]='A';
	pass[17]='5';
	pass[18]='5';
	pass[19]='6';
	pass[20]='F';
	pass[21]='F';
	pass[22]='7';
	pass[23]='3';
	pass[24]='E';

	pass[25]='3';
	pass[26]='A';
	pass[27]='E';
	pass[28]='B';
	pass[29]='C';
	pass[30]='F';
	pass[31]='1';
	pass[32]=0;
	pass[33]=0;
	_bstr_t passw(pass);
	m_crypto.DeriveKey(passw.GetBSTR());
	int tt= arData.GetSize();
	m_crypto.Decrypt(arData, outData) ;
		//throw CString("System can't load Assembly");
//	CFile m_fileout;
//	m_fileout.Open(L"C:\\tmp.dll",CFile::modeCreate|CFile::modeReadWrite,NULL);
	outData.RemoveAll();
	outData.SetSize(0);
	

	Clear(arData);
	outData.Append(arData);
//	m_fileout.Write(arData.GetData(),arData.GetCount());
//	m_fileout.Close();




	 // Define an array of character pointers
   CComSafeArray<BYTE> *pSar;

   
  
   // Define the array bound structure
   CComSafeArrayBound bound[1];
   bound[0].SetCount(outData.GetSize());
   bound[0].SetLowerBound(0);
   
   // Create a new 2 dimensional array
   // each dimension size is 3
   pSar = new CComSafeArray<BYTE>(bound,1); 
   for(int i=0;i!=outData.GetSize();i++)
	   pSar->SetAt(i,outData.GetAt(i));


	CComSafeArray<BYTE> *pdebugInfoSar;

   
  
   // Define the array bound structure
   CComSafeArrayBound debegInfoBound[1];
   debegInfoBound[0].SetCount(debugInfoData.GetSize());
   debegInfoBound[0].SetLowerBound(0);
   
   // Create a new 2 dimensional array
   // each dimension size is 3
   pdebugInfoSar = new CComSafeArray<BYTE>(debegInfoBound,1); 
   for(int i=0;i!=debugInfoData.GetSize();i++)
	   pdebugInfoSar->SetAt(i,debugInfoData.GetAt(i));

   

//AfxMessageBox(L"assemblyCryptoBridge->AssemblyLoaded");
   //AssemblyNativeCode::INativeCodeBridgePtr assemblyCryptoBridge(__uuidof( AssemblyNativeCode::NativeCodeBridge));
	assemblyCryptoBridge->AssemblyLoaded( assemblyCryptoBridge->CurrentDomain->Load_4(pSar->m_psa,pdebugInfoSar->m_psa));
	pSar->Destroy();
	pdebugInfoSar->Destroy();

	//AfxMessageBox(fileName);
    return 0;
}












