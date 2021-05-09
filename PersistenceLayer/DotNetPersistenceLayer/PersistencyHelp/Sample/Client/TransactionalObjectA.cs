using System;
using OOAdvantech.Transactions;
namespace Client
{
	/// <summary>
	/// 
	/// </summary>
	[Transactional]
	public class TransactionalObjectA
	{
		public TransactionalObjectA()
		{
		}

		public void Foo()
		{
			using(SystemStateTransition stateTransition=new SystemStateTransition())
			{
				TransactionalObjectB transactionalObjectB=new TransactionalObjectB();
				transactionalObjectB.Foo(this);
				stateTransition.Consistent=true;
			}

		}
		public void FooB()
		{
			//using(ObjectStateTransition stateTransition=new ObjectStateTransition(this))
			ObjectStateTransition stateTransition=new ObjectStateTransition(this);
			{
				TransactionalObjectB transactionalObjectB=new TransactionalObjectB();
				transactionalObjectB.Foo(null);
				stateTransition.Consistent=true;
			}

		}

	}

	[Transactional]
	public class TransactionalObjectB
	{
		public TransactionalObjectB()
		{

		}

		public void Foo(TransactionalObjectA transactionalObjectA )
		{
			using(ObjectStateTransition stateTransition=new ObjectStateTransition(this))
			{
				if(transactionalObjectA!=null)
					transactionalObjectA.FooB();
				stateTransition.Consistent=true;
			}
		}

	}

}
