#pragma once

using namespace System;
namespace TransactionManagmentSystem
{
	public __gc class TransactionManager
	{
	public:
		static System::Object*  CreateTransaction(long Timeout,System::String* Description,System::String* TransactionURI);
		//static void PropagateTransaction(System::String* TransactionURI,System::String* ComputerName,System::String* channelUri);
		static void SetEventsConsumer(System::Object* Transaction,System::Object* TransactionOutcomeEvents);
		static System::Object* GetTransaction(System::String* TransactionURI);
		static void RemoveFromROT(long ROTCookie);
		static long AddToROT(System::Object* Transaction,System::String* transactionURI);
		static System::Object* ImportTransaction(System::Byte transactionStream __gc[]);
		static System::Byte ExportTransaction(System::Object* obj,System::Byte whereAboutsStream __gc[])[];
		static System::Byte GetImportWhereabouts()[];
		

	};

	class StringWr
	{
	public:
		static CString ToCString(System::String* mString);
		static System::String* ToManageString(CString &mString);
	};
}
