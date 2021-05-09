using System;
using System.Collections.Specialized;
using PersistenceLayer;

namespace PersistenceLayerRunTime
{
	public class ObjectLifeTimeController
	//##ModelId={D5930FC1-F397-48FD-BC02-8AE11B42112A}
	{
	
		
		//##ModelId={B5E4F27A-E603-42CF-B90A-D44EE513BF77}
		public void ObjectDestroyed(object ObjectID, PersistentObject theObject)
		{
			ObjectDictionary.Remove(ObjectID);
		}
		//##ModelId={38C731FC-7654-43E9-8265-6841B07D2BA4}
		public  ObjectLifeTimeController()
		{
			ObjectDictionary=new HybridDictionary();
		}
		//##ModelId={545A9158-B530-4BEA-BADF-2A7B23630126}
		private HybridDictionary ObjectDictionary;

	
		//##ModelId={2A63595D-FD6A-4CEF-8208-5F799E0CB0F1}
		public void ObjectCostructed(object ObjectID, PersistentObject theObject)
		{
			WeakReference TempWeakReference=new WeakReference(theObject);
			ObjectDictionary.Add(ObjectID,TempWeakReference);
		}
	}
}
