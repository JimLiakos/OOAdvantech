using OOAdvantech.Collections.Generic;
using System;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

namespace OOAdvantech.MetaDataRepository
{



    /// <summary>
    /// Defines a cell of objects which are instantiated in memory
    /// </summary>
    /// <MetaDataID>{f5053c56-6a59-4a04-a6e6-f62c7243baee}</MetaDataID>
    [Serializable]
    public abstract class MemoryCell
    {



        public readonly Guid DataNodeIdentity;
        /// <summary>
        /// Defines the identity of contetext which contains the objects
        /// </summary>
        public readonly string ObjectsContextIdentity;

        /// <summary>
        /// Initialize MemoryCell 
        /// </summary>
        /// <param name="objects">
        /// Defines the object which belongs to the memory cell
        /// </param>
        /// <param name="type">
        /// Defines the type of objects
        /// </param>
        /// <param name="objectsContextIdentity">
        /// Defines the identity of contetext which contains the objects
        /// </param>
        public MemoryCell(List<ObjectData> objects, Type type, string objectsContextIdentity,Guid dataNodeIdentity)
            : this(objects, Classifier.GetClassifier(type), objectsContextIdentity,dataNodeIdentity)
        {

        }
        /// <summary>
        /// Initialize MemoryCell 
        /// </summary>
        /// <param name="objects">
        /// Defines the object which belongs to the memory cell
        /// </param>
        /// <param name="type">
        /// Defines the type of objects
        /// </param>
        /// <param name="objectsContextIdentity">
        /// Defines the identity of contetext which contains the objects
        /// </param>
        public MemoryCell(List<ObjectData> objects, Classifier type, string objectsContextIdentity,Guid dataNodeIdentity)
        {
            foreach (var objectData in objects)
                Objects[objectData._Object] = objectData;

            _Type = type;
            TypeFullName = type.GetExtensionMetaObject<Type>().FullName;
            AssemblyData = type.GetExtensionMetaObject<Type>().GetMetaData().Assembly.FullName;

            ObjectsContextIdentity = objectsContextIdentity;
            MemoryCellIdentity = Guid.NewGuid();
            DataNodeIdentity = dataNodeIdentity;
        }

        public  Guid MemoryCellIdentity;

        public MemoryCell(MemoryCell memoryCell)
        {
            Objects =new Dictionary<object,ObjectData>(memoryCell.Objects);
            ObjectsContextIdentity = memoryCell.ObjectsContextIdentity;
            MemoryCellIdentity = memoryCell.MemoryCellIdentity;
            TypeFullName =memoryCell.TypeFullName;
            AssemblyData = memoryCell.AssemblyData;
            DataNodeIdentity = memoryCell.DataNodeIdentity;

        }

        /// <summary>
        /// Defines the objects which contains the MemoryCell
        /// </summary>
        [Association("", Roles.RoleA, "db2f966c-d996-456f-952f-0ebba8cf38aa")]
        [IgnoreErrorCheck]
        public OOAdvantech.Collections.Generic.Dictionary<object, ObjectData> Objects = new Dictionary<object, ObjectData>();

        string TypeFullName;
        string AssemblyData;

        [NonSerialized]
        Classifier _Type;
        /// <summary>
        /// Defines the type of object
        /// </summary>

        [Association("ObjectsType", Roles.RoleA, "8a6d4b7f-8a74-480d-ae01-9df8a977b791")]
        public Classifier Type
        {
            get
            {
                if (_Type == null)
                {
                    Type type = ModulePublisher.ClassRepository.GetType(TypeFullName, AssemblyData);
                    _Type = MetaDataRepository.Classifier.GetClassifier(type);
                }
                return _Type;
            }
        }

        internal static MemoryCell GetProcessMemoryCell(OutProcessMemoryCell memoryCell)
        {
            bool partialLoaded = false;
            foreach (ObjectData objectData in memoryCell.Objects.Values)
            {
                objectData.CheckForPartialLoad();
                partialLoaded |= objectData.PartialLoaded;
            }
            MemoryCell newMemoryCell = null;
            if (partialLoaded)
                newMemoryCell = new PartialLoadedMemoryCell(memoryCell);
            else
                newMemoryCell = new InProcessMemoryCell(memoryCell);

            if (newMemoryCell.MemoryCellIdentity == Guid.Empty)
                newMemoryCell.MemoryCellIdentity = Guid.NewGuid();

            return newMemoryCell;

        }

        internal abstract  MemoryCell Clone(System.Collections.Generic.Dictionary<object, object> clonedObjects);
        
    }

    /// <summary>
    /// Defines the memory cell which contains object which are instantiated on other process 
    /// than the MemoryCell  process 
    /// </summary>
    /// <MetaDataID>{098030a1-53fc-4766-947b-e09da2e0b4d1}</MetaDataID>
    [Serializable]
    public class OutProcessMemoryCell : MemoryCell
    {

        internal override MemoryCell Clone(System.Collections.Generic.Dictionary<object, object> clonedObjects)
        {
            MemoryCell newMemoryCell = new OutProcessMemoryCell(this);
            return newMemoryCell;
        }

        OutProcessMemoryCell(OutProcessMemoryCell copyMemoryCell )
            : base(copyMemoryCell)
        {
            MemoryCellIdentity = copyMemoryCell.MemoryCellIdentity;
            Channeluri = copyMemoryCell.Channeluri;

        }

        public readonly string Channeluri;
        public OutProcessMemoryCell(List<ObjectData> objects, Type type, string objectsContextIdentity, string channeluri,Guid dataNodeIdentity)
            : base(objects, type, objectsContextIdentity,dataNodeIdentity)
        {
            Channeluri = channeluri;
            MemoryCellIdentity = Guid.Empty;
        }
        public OutProcessMemoryCell(PartialLoadedMemoryCell memoryCell, string channeluri, Guid dataNodeIdentity)
            :base(new List<ObjectData>(),memoryCell.Type,memoryCell.ObjectsContextIdentity,dataNodeIdentity)
        {
            Channeluri=channeluri;
            MemoryCellIdentity = Guid.Empty;

        }
        public OutProcessMemoryCell(InProcessMemoryCell memoryCell, string channeluri, Guid dataNodeIdentity)
            : base(new List<ObjectData>(), memoryCell.Type, memoryCell.ObjectsContextIdentity,dataNodeIdentity)
        {
            MemoryCellIdentity = memoryCell.MemoryCellIdentity;
            Channeluri=channeluri;
        }
    }

    /// <summary>
    /// Defines the memory cell which contains object which are instantiated on the same process 
    /// with the MemoryCell   
    /// </summary>
    /// <MetaDataID>{b7d1b447-2baa-4241-a694-1b435bd3c259}</MetaDataID>
    public class InProcessMemoryCell : MemoryCell
    {

        internal InProcessMemoryCell(MemoryCell memoryCell)
            : base(memoryCell)
        {
        }

        internal override MemoryCell Clone(System.Collections.Generic.Dictionary<object, object> clonedObjects)
        {
            return new InProcessMemoryCell(this);
        }
        InProcessMemoryCell(InProcessMemoryCell copyInProcessMemoryCell)
            : base(copyInProcessMemoryCell)
        {

        }
        public InProcessMemoryCell(List<ObjectData> objects, Type type, string objectsContextIdentity,Guid dataNodeIdentity)
            : base(objects, type, objectsContextIdentity,dataNodeIdentity)
        {

        }
        public InProcessMemoryCell(List<ObjectData> objects, Classifier type, string objectsContextIdentity, Guid dataNodeIdentity)
            : base(objects, type, objectsContextIdentity,dataNodeIdentity)
        {

        }
    }
    /// <summary>
    /// Defines the memory cell which contains persistent object which are partial loaded from storage
    /// </summary>
    /// <MetaDataID>{b7584537-c887-4b86-a5bb-22b9ed015b94}</MetaDataID>
    public class PartialLoadedMemoryCell : MemoryCell
    {

        internal override MemoryCell Clone(System.Collections.Generic.Dictionary<object, object> clonedObjects)
        {
            return new PartialLoadedMemoryCell(this);
        }

        public PartialLoadedMemoryCell(List<ObjectData> objects, Type type, string objectsContextIdentity, Guid dataNodeIdentity)
            : base(objects, type, objectsContextIdentity, dataNodeIdentity)
        {

        }
        public PartialLoadedMemoryCell(List<ObjectData> objects, Classifier type, string objectsContextIdentity, Guid dataNodeIdentity)
            : base(objects, type, objectsContextIdentity, dataNodeIdentity)
        {

        }
        internal PartialLoadedMemoryCell(MemoryCell memoryCell)
            : base(memoryCell)
        {

        }

        /// <summary>
        /// Defines the StorageCells where stored the objects which contains the MemoryCell
        /// </summary>
        public List<StorageCell> StorageCells
        {
            get
            {
                List<StorageCell> storageCells = new List<StorageCell>();
                foreach (ObjectData objectData in Objects.Values)
                {

                    if (objectData.PartialLoaded && !storageCells.Contains(objectData.StorageInstanceRef.StorageInstanceSet))
                        storageCells.Add(objectData.StorageInstanceRef.StorageInstanceSet);

                }
                return storageCells;
            }
        }
    }
    /// <MetaDataID>{ed16db10-0226-49b6-bdc1-ec30c0dff8e7}</MetaDataID>
    [Serializable]
    public class MemoryCellReference
    {
        public readonly Guid MemoryCellIdentity;
        public readonly string ObjectsContextIdentity;
        public readonly bool IsOutProcessMemoryCell;
        public readonly bool IsPartialLoadedMemoryCell;
        public MemoryCellReference(MemoryCell memoryCell)
        {
            IsOutProcessMemoryCell = memoryCell is OutProcessMemoryCell;
            IsPartialLoadedMemoryCell = memoryCell is PartialLoadedMemoryCell;
            MemoryCellIdentity = memoryCell.MemoryCellIdentity;
            ObjectsContextIdentity = memoryCell.ObjectsContextIdentity;
        }
         MemoryCellReference(MemoryCellReference copy)
        {
            IsOutProcessMemoryCell = copy.IsOutProcessMemoryCell;
            IsPartialLoadedMemoryCell =copy.IsPartialLoadedMemoryCell;
            MemoryCellIdentity = copy.MemoryCellIdentity;
            ObjectsContextIdentity =copy.ObjectsContextIdentity;
        }
        public override int GetHashCode()
        {
            int num = -1162279000;
            num = (-1521134295 * num) + MemoryCellIdentity.GetHashCode();
            num = (-1521134295 * num) + ObjectsContextIdentity.GetHashCode();
            return num;
        }


        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (((MemoryCellReference)obj).MemoryCellIdentity == MemoryCellIdentity && 
                ((MemoryCellReference)obj).ObjectsContextIdentity == ObjectsContextIdentity)
                return true;
            else
                return false;
        }

        internal MemoryCellReference Clone(System.Collections.Generic.Dictionary<object, object> clonedObjects)
        {
            MemoryCellReference newMemoryCellReference = new MemoryCellReference(this);
            return newMemoryCellReference;
        }
    }

}
