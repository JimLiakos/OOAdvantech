using System;

namespace OOAdvantech.MSSQLPersistenceRunTime.Commands
{
	/// <summary>
	/// </summary>
	/// <MetaDataID>{3DD64C52-273A-4ED4-AB5B-BEF4B679D621}</MetaDataID>
	public class SplitClassActiveStorageCell : OOAdvantech.PersistenceLayerRunTime.Commands.Command
	{

		public override string Identity
		{
			get
			{
				return "Split"+Class.Identity+ObjectStorage.GetHashCode().ToString();
			}
		}
		public static string GetIdentity(RDBMSMetaDataRepository.Class _class, ObjectStorage storage)
		{
			return "Split"+_class.Identity+storage.GetHashCode().ToString();
		}


		/// <MetaDataID>{8E6A0F13-9555-40C0-BD65-C1AE4E83FF8D}</MetaDataID>
		public OOAdvantech.RDBMSMetaDataRepository.Class Class;
		/// <MetaDataID>{4A4C9B94-4E3C-4CAB-85C9-96B9CCBB51C3}</MetaDataID>
		ObjectStorage ObjectStorage;
		/// <MetaDataID>{734D27D3-DA99-4643-9F0D-00D5FE5BBFC4}</MetaDataID>
		public SplitClassActiveStorageCell(OOAdvantech.RDBMSMetaDataRepository.Class _Class,ObjectStorage objectStorage)
		{
			ObjectStorage=objectStorage;
			Class=_Class;
		}
		/// <MetaDataID>{814701BE-3D55-4379-BEC2-B3DB4EEF3757}</MetaDataID>
		public static int CommandOrder
		{
			get
			{
				return 30;
			}
		}
		/// <MetaDataID>{1F4BB202-CAE1-4BA9-8137-F59C4415F5AF}</MetaDataID>
		public override int ExecutionOrder
		{
			get
			{
				return CommandOrder;
			}
		}
		/// <MetaDataID>{89DF8515-2718-4376-9C1B-3EB4C256EEAE}</MetaDataID>
		bool SubTransactionCmdsProduced=false;
		/// <MetaDataID>{814F39F8-FEE2-485E-A4E9-09B9FECAA454}</MetaDataID>
		public override void GetSubCommands(int currentExecutionOrder)
		{
			if(!SubTransactionCmdsProduced&&currentExecutionOrder>=20)
			{
				PersistenceLayerRunTime.ITransactionContext transactionContext =PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
				SubTransactionCmdsProduced=true;
				Class.SplitActiveStorageCell();
                if (!transactionContext.ContainCommand(RDBMSPersistenceRunTime.Commands.UpdateStorageSchema.GetIdentity(ObjectStorage)))
                    transactionContext.EnlistCommand(new RDBMSPersistenceRunTime.Commands.UpdateStorageSchema(ObjectStorage));
					
			}
		}
		
		/// <MetaDataID>{97346733-78DD-4E63-8FAF-2907F095AE91}</MetaDataID>
		public override void Execute()
		{
		}


	}
}
