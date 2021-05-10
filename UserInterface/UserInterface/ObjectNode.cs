namespace OOAdvantech.UserInterface
{
    using MetaDataRepository;
    using Transactions;

    ///<summary>
    /// The main work of PathNode is to translate a collection of paths in a tree of path nodes. 
    /// With the AddPath method we add a full path and after the method call,
    /// there is PathNode object in PathNode tree where the FullName is the path.
    /// </summary>
    /// <example>
    /// PathNode pathNode = new PathNode();
    /// pathNode.AddPath("Client.Name");
    /// pathNode.AddPath("Client.Address.Street");
    /// pathNode.AddPath("Client.Address.City");
    ///
    /// //The result is the tree.
    /// //Root|
    /// //    |__Client|
    /// //             |
    /// //             |___Name
    /// //             |   
    /// //             |___Address|
    /// //                        |__Street
    /// //                        |
    /// //                        |__City
    /// </example>
    /// <MetaDataID>{A20B46EE-43F3-4BB6-BD30-8A9468A93A71}</MetaDataID>
    public class PathNode
    {

        /// <exclude>Excluded</exclude>
        private OOAdvantech.Collections.Generic.Set<PathNode> _Members = new OOAdvantech.Collections.Generic.Set<PathNode>();
        /// <MetaDataID>{680A4CBE-25FC-4608-8BB5-CE3671993E55}</MetaDataID>
        /// <summary>
        /// This member defines the contained path nodes.  
        /// There is constraint witch doesn’t allow to contained PathNode with the same name.
        /// </summary>
        [Association("NodeMember",typeof(OOAdvantech.UserInterface.PathNode),Roles.RoleA,"{901C60F3-E237-420E-AEE5-DBE8573C6F1D}")]
        [RoleAMultiplicityRange(0)]
        public OOAdvantech.Collections.Generic.Set<PathNode> Members
        {
            get
            {
                return new OOAdvantech.Collections.Generic.Set<PathNode>(_Members,OOAdvantech.Collections.CollectionAccessType.ReadOnly);
            }
        }
        /// <MetaDataID>{333006D3-5CB0-47A6-8F09-8C06FF6E26B4}</MetaDataID>
        /// <summary>
        /// This method returns a PathNode with name the name parameter. If there isn't, 
        /// adds one with the name of parameter and return the new PathNode. 
        /// </summary>
        PathNode GetMember(string name)
        {
            if (name == "(ViewControlObject)")
                return this;
            foreach (PathNode objectNode in _Members)
            {
                if (objectNode.Name == name)
                    return objectNode;
                if(objectNode.Recursive&&objectNode.RecursiveSteps>0&&"Recursive("+objectNode.Name+","+objectNode.RecursiveSteps.ToString()+")"==name)
                    return objectNode;
            }
            PathNode newMember = new PathNode(name, this);
            _Members.Add(newMember);
            return newMember;
        }

       
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{34E2047C-AC6E-43B3-BA94-7F586E1CE959}</MetaDataID>
        readonly private string _Name="Root";
        /// <MetaDataID>{376B515D-D9C6-4546-A9EB-F45DA893F9E4}</MetaDataID>
        /// <summary>
        /// This member defines the name of PathNode. 
        /// </summary>
        public string Name
        {
            get
            {
              return _Name;
            }
       
        }
        /// <MetaDataID>{6CAFC7DD-F080-4D70-9129-42A7D0422DCE}</MetaDataID>
        protected PathNode()
        {

        }
        /// <MetaDataID>{528dffec-2ea3-4887-9c55-a696e5f7cbff}</MetaDataID>
        public readonly int RecursiveSteps = 0;
        /// <MetaDataID>{ff18c003-7973-48be-b930-2f4569d8a1c3}</MetaDataID>
        public readonly bool Recursive = false;
 
        /// <MetaDataID>{6452f4a7-9597-4e8b-a9f0-4e2fa6990975}</MetaDataID>
        public readonly PathNode Parent;
        /// <MetaDataID>{9DBF7534-2E90-471B-96B0-657411C379B4}</MetaDataID>
        ///<summary> 
        ///This Constractor creates an sub PathNode
        ///</summary>
        public PathNode(string name, PathNode parent)
        {
            _Name = name;
            Parent = parent;


            name = name.Trim();
            name = name.Replace(" ", "");
            // int recursiveSteps =0;
            if (name.IndexOf("Recursive(") == 0)
            {
                Recursive = true;
                name = name.Remove(0, "Recursive(".Length);
                name = name.Replace(")", "");
                int nPos = name.IndexOf(',');
                if (nPos != -1)
                    RecursiveSteps = int.Parse(name.Substring(nPos + 1));
                name = name.Substring(0, nPos);
            }
            _Name = name;
        }

        /// <MetaDataID>{2e40fb8f-77c0-4f5c-ad27-7128d33282cf}</MetaDataID>
        public string PathName
        {
            get
            {
                if (Parent == null)
                {
                    if (Recursive && RecursiveSteps == 0)
                        return "Recursive(" + _Name + ")";
                    if (Recursive && RecursiveSteps > 0)
                        return "Recursive(" + _Name + "," + RecursiveSteps.ToString() + ")";
                    return _Name;
                }
                else
                {
                    if (Recursive && RecursiveSteps == 0)
                        return Parent.PathName + ".Recursive(" + _Name + ")";
                    if (Recursive && RecursiveSteps > 0)
                        return Parent.PathName + ".Recursive(" + _Name + "," + RecursiveSteps.ToString() + ")";


                    return Parent.PathName + "." + _Name;
                }
            }
        }


        /// <MetaDataID>{87265640-f4b2-45c7-9233-a6b0a313c328}</MetaDataID>
        public string FullName
        {
            get
            {
                if (Parent == null)
                    return _Name;
                else
                    return Parent.FullName+ "." + _Name;
            }
        }

        /// <MetaDataID>{1270b58e-78aa-4935-ab97-1fa312a0bdfb}</MetaDataID>
        string _FullNameAsAlias;
        /// <MetaDataID>{e13daa98-2891-46b3-8f98-8acf21112913}</MetaDataID>
        public string FullNameAsAlias
        {
            get
            {
                if(_FullNameAsAlias==null)
                    _FullNameAsAlias=FullName.Replace(".", "_");
                return _FullNameAsAlias;
            }
        }

        /// <MetaDataID>{eaed4139-96ab-4c64-bd51-ed96c2d8dc83}</MetaDataID>
        bool IsPath = false;

        
        /// <MetaDataID>{8576778a-712d-492b-9c54-2d1812c6b3d6}</MetaDataID>
        public PathNode AddPath(string path)
        {
            if (path == Name&& PathName == path)
                return this;
            if (string.IsNullOrEmpty(path))
                return null;
            int nPos = path.IndexOf(".");
            if (nPos == -1)
            {
                PathNode member = GetMember(path);
                member.IsPath = true;
                return member;
            }
            else
            {
                string memberName = path.Substring(0, nPos);
                return GetMember(memberName).AddPath(path.Substring(nPos + 1));
            }
            
        }

        /// <MetaDataID>{62f9a1ac-cfec-4ca6-9221-4ae3b224f8d1}</MetaDataID>
        /// <summary>
        /// Defines all paths where have this PathNode as part of them.
        /// </summary>
        public OOAdvantech.Collections.Generic.List<string> Paths

        {
            get
            {
                OOAdvantech.Collections.Generic.List<string> paths = new OOAdvantech.Collections.Generic.List<string>();
                if (IsPath)
                    paths.Add(PathName);

                foreach (PathNode member in Members)
                    paths.AddRange(member.Paths);

                return paths;
            }
            
        }

        /// <MetaDataID>{b8d53f57-b792-4bca-aff8-f57f8c65ea94}</MetaDataID>
        public OOAdvantech.Collections.Generic.List<string> GetSubPaths(string path)
        {
            OOAdvantech.Collections.Generic.List<string> subPaths =new OOAdvantech.Collections.Generic.List<string>();
            path = "Root." + path;
            foreach (string subPath in Paths)
            {
                if(subPath.IndexOf(path)==0)
                    subPaths.Add(subPath.Substring(path.Length));
            }
            return subPaths;
        }
    }
}
