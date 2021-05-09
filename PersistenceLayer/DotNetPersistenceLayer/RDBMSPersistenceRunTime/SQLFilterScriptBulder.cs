using System;
using System.Collections.Generic;
using System.Text;
using ComparisonTermsType = OOAdvantech.MetaDataRepository.ObjectQueryLanguage.Criterion.ComparisonTermsType;

namespace OOAdvantech.RDBMSPersistenceRunTime
{
    using MetaDataRepository.ObjectQueryLanguage;
    /// <MetaDataID>{3c5fcea1-64c0-4e20-9678-ed882188140b}</MetaDataID>
    public class SQLFilterScriptBuilder
    {

        /// <MetaDataID>{f40951a5-a976-4043-9e03-e76c7786d391}</MetaDataID>
        DataLoader DataLoader;
        /// <MetaDataID>{6dd92064-da4f-4076-aee8-8a79e5541964}</MetaDataID>
        public SQLFilterScriptBuilder(DataLoader dataLoader)
        {
            DataLoader = dataLoader;
        }



        /// <MetaDataID>{41b886c6-9f8e-4e2f-9836-a089929586d2}</MetaDataID>
        public string GetSQLFilterStatament(MetaDataRepository.ObjectQueryLanguage.SearchCondition searchCondition)
        {

            string sqlExpresion = null;
            foreach (MetaDataRepository.ObjectQueryLanguage.SearchTerm searchTerm in searchCondition.SearchTerms)
            {

                foreach (System.Collections.Generic.KeyValuePair<Criterion, System.Collections.Generic.Stack<DataNode>[]> entry in DataLoader.LocalResolvedCriterions)
                {
                    if (searchTerm.ContainsCriterion(entry.Key))
                    {
                        string searchTermSQLExpression = GetSearchTermSQLExpression(searchTerm);

                        if (sqlExpresion == null)
                        {
                            if (searchTermSQLExpression != null)
                                sqlExpresion = "(" + searchTermSQLExpression;
                        }
                        else
                        {

                            if (searchTermSQLExpression != null)
                                sqlExpresion += " OR " + searchTermSQLExpression;
                        }
                        break;

                    }
                }


            }
            if (sqlExpresion != null)
                sqlExpresion += ")";

            return sqlExpresion;

        }

        /// <MetaDataID>{f605904c-d5f5-4b98-ad72-e7d43e52632f}</MetaDataID>
        string GetSearchTermSQLExpression(MetaDataRepository.ObjectQueryLanguage.SearchTerm searchTerm)
        {
            string sqlExpression = null;
            foreach (MetaDataRepository.ObjectQueryLanguage.SearchFactor searchFactor in searchTerm.SearchFactors)
            {
                foreach (System.Collections.Generic.KeyValuePair<Criterion, System.Collections.Generic.Stack<DataNode>[]> entry in DataLoader.LocalResolvedCriterions)
                {
                    if (searchFactor.ContainsCriterion(entry.Key)&&DataLoader.LocalResolvedCriterions.ContainsKey(entry.Key) )
                    {

                        
                        if (sqlExpression == null)
                            sqlExpression = GetSearchFactorSQLExpression(searchFactor);
                        else
                        {
                            string searchFactorSQLExpression = GetSearchFactorSQLExpression(searchFactor);
                            if (searchFactorSQLExpression != null)
                                sqlExpression += " AND " + searchFactorSQLExpression;
                        }
                        break;
                    }
                }
            }
            if (searchTerm.IsNotExpression && sqlExpression != null)
                sqlExpression = "NOT " + sqlExpression;

            return sqlExpression;

        }

        /// <MetaDataID>{03c70ae6-df14-4748-95f7-0a73efd87525}</MetaDataID>
        string GetSearchFactorSQLExpression(MetaDataRepository.ObjectQueryLanguage.SearchFactor searchFactor)
        {
            string sqlExpression = null;
            if (searchFactor.SearchCondition != null)
                sqlExpression = GetSQLFilterStatament(searchFactor.SearchCondition);
            else
                sqlExpression = GetCriterionSQLExpression(searchFactor.Criterion);

            if (searchFactor.IsNotExpression)
                sqlExpression = "NOT " + sqlExpression;
            return sqlExpression;


        }
        /// <MetaDataID>{f62201ea-4598-4135-b137-f256d6adae2d}</MetaDataID>
        public string GetSQLScriptForName(string name)
        {
            return  DataLoader.SqlScriptsBuilder.GetSQLScriptForName(name);
        }
        /// <MetaDataID>{0c5be6ff-8bec-44c8-8382-4e899ea6b35f}</MetaDataID>
        string GetCriterionSQLExpression(MetaDataRepository.ObjectQueryLanguage.Criterion criterion)
        {
            string compareExpression = null;
            string comparisonOperator = null;
            if (criterion.ComparisonOperator == Criterion.ComparisonType.Equal)
                comparisonOperator = " = ";
            if (criterion.ComparisonOperator == Criterion.ComparisonType.NotEqual)
                comparisonOperator = " <> ";
            if (criterion.ComparisonOperator == Criterion.ComparisonType.GreaterThan)
                comparisonOperator = " > ";
            if (criterion.ComparisonOperator == Criterion.ComparisonType.GreaterThanEqual)
                comparisonOperator = " >= ";
            if (criterion.ComparisonOperator == Criterion.ComparisonType.LessThan)
                comparisonOperator = " < ";
            if (criterion.ComparisonOperator == Criterion.ComparisonType.LessThanEqual)
                comparisonOperator = " <= ";
            if (criterion.ComparisonOperator == Criterion.ComparisonType.Like)
                comparisonOperator += " LIKE ";
            if (criterion.ComparisonOperator == MetaDataRepository.ObjectQueryLanguage.Criterion.ComparisonType.Equal)
                comparisonOperator = " = ";
            if (criterion.IsNULL)
                comparisonOperator = " IS ";
            if (criterion.IsNotNULL)
                comparisonOperator = " IS NOT ";

            //if (criterion.CriterionType == ComparisonTermsType.ObjectWithParameter|| 
            //    criterion.CriterionType == ComparisonTermsType.ParameterWithObject||
            //    criterion.CriterionType == ComparisonTermsType.ObjectAttributeWithParameter|| 
            //    criterion.CriterionType == ComparisonTermsType.ParameterWithObjectAttribute)
            //{

            //}


            switch (criterion.CriterionType)
            {

                case OOAdvantech.MetaDataRepository.ObjectQueryLanguage.Criterion.ComparisonTermsType.ObjectsAttributes:
                    {
                        MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm firstComparisonTerm = criterion.ComparisonTerms[0] as MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm;
                        MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm secondComparisonTerm = criterion.ComparisonTerms[1] as MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm;
                        compareExpression += DataLoader.RDBMSSQLScriptGenarator.GetSQLScriptForName(firstComparisonTerm.DataNode.ParentDataNode.Alias);
                        compareExpression += ".";
                        compareExpression += DataLoader.RDBMSSQLScriptGenarator.GetSQLScriptForName(DataLoader.GetLocalDataColumnName(firstComparisonTerm.DataNode));
                        compareExpression += comparisonOperator;
                        compareExpression += DataLoader.RDBMSSQLScriptGenarator.GetSQLScriptForName(secondComparisonTerm.DataNode.ParentDataNode.Alias);
                        compareExpression += ".";
                        compareExpression += DataLoader.RDBMSSQLScriptGenarator.GetSQLScriptForName(DataLoader.GetLocalDataColumnName(secondComparisonTerm.DataNode));
                        return compareExpression;

                    }
                case ComparisonTermsType.ObjectAttributeWithLiteral:
                case ComparisonTermsType.LiteralWithObjectAttribute:
                    {
                        MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm objectAttributeComparisonTerm = null;
                        if (criterion.ComparisonTerms[0] is MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm)
                            objectAttributeComparisonTerm = criterion.ComparisonTerms[0] as MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm;
                        else
                            objectAttributeComparisonTerm = criterion.ComparisonTerms[1] as MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm;
                        MetaDataRepository.ObjectQueryLanguage.LiteralComparisonTerm literalComparisonTerm = null;
                        if (criterion.ComparisonTerms[0] is MetaDataRepository.ObjectQueryLanguage.LiteralComparisonTerm)
                            literalComparisonTerm = criterion.ComparisonTerms[0] as MetaDataRepository.ObjectQueryLanguage.LiteralComparisonTerm;
                        else
                            literalComparisonTerm = criterion.ComparisonTerms[1] as MetaDataRepository.ObjectQueryLanguage.LiteralComparisonTerm;

                        string literalValue = null;
                        if (criterion.ComparisonOperator == Criterion.ComparisonType.Like)
                            literalValue = GetLikeString(literalComparisonTerm.Value as string, literalValue);
                        else
                            literalValue = (DataLoader.ObjectStorage as ObjectStorage).TypeDictionary.ConvertToSQLString(literalComparisonTerm.Value);

                        if (criterion.ComparisonTerms[0] is MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm ||
                            (criterion.ComparisonTerms[1] is MetaDataRepository.ObjectQueryLanguage.LiteralComparisonTerm &&
                            (criterion.IsNotNULL || criterion.IsNULL)))
                        {
                            compareExpression = DataLoader.RDBMSSQLScriptGenarator.GetSQLScriptForName(objectAttributeComparisonTerm.DataNode.ParentDataNode.Alias);
                            compareExpression += ".";

                            if (objectAttributeComparisonTerm.DataNode.ParentDataNode.AssignedMetaObject is MetaDataRepository.Attribute &&
                            (objectAttributeComparisonTerm.DataNode.ParentDataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                            {
                                RDBMSMetaDataRepository.Attribute attribute = (DataLoader.Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(objectAttributeComparisonTerm.DataNode.ParentDataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.Attribute)) as RDBMSMetaDataRepository.Attribute;
                                string columnName = attribute.GetAttributeColumnName(objectAttributeComparisonTerm.DataNode.AssignedMetaObject as MetaDataRepository.Attribute);
                                compareExpression += DataLoader.RDBMSSQLScriptGenarator.GetSQLScriptForName(columnName);
                            }
                            else
                                compareExpression += DataLoader.RDBMSSQLScriptGenarator.GetSQLScriptForName((objectAttributeComparisonTerm.DataNode.AssignedMetaObject as MetaDataRepository.Attribute).Name);

                            compareExpression += comparisonOperator;
                            compareExpression += literalValue;
                        }
                        else
                        {
                            compareExpression += literalValue;
                            compareExpression += comparisonOperator;
                            compareExpression += DataLoader.RDBMSSQLScriptGenarator.GetSQLScriptForName(objectAttributeComparisonTerm.DataNode.ParentDataNode.Alias);
                            compareExpression += ".";
                            if (objectAttributeComparisonTerm.DataNode.ParentDataNode.AssignedMetaObject is MetaDataRepository.Attribute &&
                            (objectAttributeComparisonTerm.DataNode.ParentDataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                            {
                                RDBMSMetaDataRepository.Attribute attribute = (DataLoader.Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(objectAttributeComparisonTerm.DataNode.ParentDataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.Attribute)) as RDBMSMetaDataRepository.Attribute;
                                string columnName = attribute.GetAttributeColumnName(objectAttributeComparisonTerm.DataNode.AssignedMetaObject as MetaDataRepository.Attribute);
                                compareExpression += DataLoader.RDBMSSQLScriptGenarator.GetSQLScriptForName(columnName);
                            }
                            else
                                compareExpression += DataLoader.RDBMSSQLScriptGenarator.GetSQLScriptForName((objectAttributeComparisonTerm.DataNode.AssignedMetaObject as MetaDataRepository.Attribute).Name);

                        }
                        return compareExpression;
                    }
                case ComparisonTermsType.ObjectAttributeWithParameter:
                case ComparisonTermsType.ParameterWithObjectAttribute:
                    {
                        MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm objectAttributeComparisonTerm = null;
                        if (criterion.ComparisonTerms[0] is MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm)
                            objectAttributeComparisonTerm = criterion.ComparisonTerms[0] as MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm;
                        else
                            objectAttributeComparisonTerm = criterion.ComparisonTerms[1] as MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm;

                        MetaDataRepository.ObjectQueryLanguage.ParameterComparisonTerm parameterComparisonTerm = null;
                        if (criterion.ComparisonTerms[0] is MetaDataRepository.ObjectQueryLanguage.ParameterComparisonTerm)
                            parameterComparisonTerm = criterion.ComparisonTerms[0] as MetaDataRepository.ObjectQueryLanguage.ParameterComparisonTerm;
                        else
                            parameterComparisonTerm = criterion.ComparisonTerms[1] as MetaDataRepository.ObjectQueryLanguage.ParameterComparisonTerm;

                        string parameterValue = null;
                        if (criterion.ComparisonOperator == Criterion.ComparisonType.Like)
                            parameterValue = GetLikeString(parameterComparisonTerm.ParameterValue as string, parameterValue);
                        else
                            parameterValue = (DataLoader.ObjectStorage as ObjectStorage).TypeDictionary.ConvertToSQLString(parameterComparisonTerm.ParameterValue);

                        if ((parameterValue == null && criterion.ComparisonOperator == Criterion.ComparisonType.Equal))
                        {
                            DataNode parentDataNode = objectAttributeComparisonTerm.DataNode.ParentDataNode;
                            while (parentDataNode.AssignedMetaObject is MetaDataRepository.Attribute && (parentDataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                                parentDataNode = parentDataNode.ParentDataNode;

                            compareExpression = DataLoader.RDBMSSQLScriptGenarator.GetSQLScriptForName(parentDataNode.Alias);
                            compareExpression += ".";
                            compareExpression += DataLoader.RDBMSSQLScriptGenarator.GetSQLScriptForName(DataLoader.GetLocalDataColumnName(objectAttributeComparisonTerm.DataNode));
                            compareExpression += "IS NULL";
                            return compareExpression;
                        }
                        else if (criterion.ComparisonTerms[0] is MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm)
                        {
                            
                            DataNode parentDataNode = objectAttributeComparisonTerm.DataNode.ParentDataNode;
                            while (parentDataNode.AssignedMetaObject is MetaDataRepository.Attribute &&
                                ((parentDataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType ))
                                parentDataNode = parentDataNode.ParentDataNode;
                            if ((parentDataNode.AssignedMetaObject is MetaDataRepository.Attribute) && 
                                (parentDataNode.AssignedMetaObject as MetaDataRepository.Attribute).Type.FullName == "System.DateTime")
                            {
                                DataNode dateTimeDataNode = null;
                                dateTimeDataNode = parentDataNode;
                                parentDataNode = parentDataNode.ParentDataNode;
                                compareExpression = DataLoader.RDBMSSQLScriptGenarator.GetDatePartSqlScript(objectAttributeComparisonTerm.DataNode);
                            }
                            else
                            {
                                compareExpression = DataLoader.RDBMSSQLScriptGenarator.GetSQLScriptForName(parentDataNode.Alias);
                                compareExpression += ".";
                                compareExpression += DataLoader.RDBMSSQLScriptGenarator.GetSQLScriptForName((parentDataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.ObjectsContextIdentity] as DataLoader).GetColumn(objectAttributeComparisonTerm.DataNode).Name);
                            }
                            compareExpression += comparisonOperator;
                            compareExpression += parameterValue;
                            return compareExpression;
                        }
                        else
                        {
                            compareExpression += parameterValue;
                            compareExpression += comparisonOperator;

                            DataNode parentDataNode = objectAttributeComparisonTerm.DataNode.ParentDataNode;
                            while (parentDataNode.AssignedMetaObject is MetaDataRepository.Attribute && (parentDataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                                parentDataNode = parentDataNode.ParentDataNode;

                            compareExpression += DataLoader.RDBMSSQLScriptGenarator.GetSQLScriptForName(parentDataNode.Alias);
                            compareExpression += ".";
                            compareExpression += DataLoader.RDBMSSQLScriptGenarator.GetSQLScriptForName(objectAttributeComparisonTerm.DataNode.Name);
                            return compareExpression;
                        }
                    }
                case ComparisonTermsType.LiteralWithObject:
                case ComparisonTermsType.ObjectWithLiteral:
                    {

                        #region Build comparison expresion with ObjectIDComparisonTerm


                        MetaDataRepository.ObjectQueryLanguage.DataNode objectTermDataNode = null;
                        MetaDataRepository.ObjectQueryLanguage.ObjectIDComparisonTerm objectIDComparisonTerm = null;
                        if (criterion.LeftTermDataNode != null)
                        {
                            objectTermDataNode = criterion.LeftTermDataNode;
                            objectIDComparisonTerm = criterion.ComparisonTerms[1] as MetaDataRepository.ObjectQueryLanguage.ObjectIDComparisonTerm;
                        }
                        else
                        {
                            objectTermDataNode = criterion.RightTermDataNode;
                            objectIDComparisonTerm = criterion.ComparisonTerms[0] as MetaDataRepository.ObjectQueryLanguage.ObjectIDComparisonTerm;
                        }

                        MetaDataRepository.ObjectIdentityType objectIdentityType = null;
                        List<string> objectIDParts = new List<string>(objectIDComparisonTerm.MultiPartObjectID.Keys);
                        foreach (MetaDataRepository.ObjectIdentityType theObjectIdentityType in objectTermDataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.ObjectsContextIdentity].ObjectIdentityTypes)
                        {
                            objectIdentityType = theObjectIdentityType;
                            int i = 0;
                            foreach (MetaDataRepository.IIdentityPart part in theObjectIdentityType.Parts)
                            {
                                if (objectIDParts[i++] != part.PartTypeName || objectIDComparisonTerm.MultiPartObjectID[part.PartTypeName ].GetType()!=part.Type)
                                {
                                    objectIdentityType = null;
                                    break;
                                }
                            }
                            if (objectIdentityType != null)
                                break;
                        }
                        if (objectIdentityType == null)
                            return "(1=2)";

                        foreach (MetaDataRepository.IIdentityPart column in objectIdentityType.Parts)
                        {
                            if (compareExpression != null)
                            {
                                if (criterion.IsNULL)
                                    compareExpression += " OR ";
                                else if (criterion.IsNotNULL)
                                    compareExpression += " AND ";
                                else if (criterion.ComparisonOperator == Criterion.ComparisonType.Equal) //error prone
                                    compareExpression += " AND ";
                                else
                                    compareExpression += " OR ";
                            }
                            else
                                compareExpression += "(";

                            compareExpression += DataLoader.RDBMSSQLScriptGenarator.GetSQLScriptForName(objectTermDataNode.Alias) + "." + DataLoader.RDBMSSQLScriptGenarator.GetSQLScriptForName(column.Name);


                            object literalFieldValue = null;
                            if (!criterion.IsNULL && !criterion.IsNotNULL)
                            {
                                foreach (System.Collections.Generic.KeyValuePair<string, object> entry in objectIDComparisonTerm.MultiPartObjectID)
                                {
                                    if (entry.Key == column.PartTypeName)
                                    {
                                        literalFieldValue = entry.Value;
                                        break;
                                    }
                                }
                                if (literalFieldValue is string)
                                    compareExpression += " " + comparisonOperator + "  N'" + literalFieldValue + "'";
                                else if (literalFieldValue != null)
                                    compareExpression += " " + comparisonOperator + "  " + literalFieldValue + " ";
                                else if (literalFieldValue == null && criterion.ComparisonOperator == Criterion.ComparisonType.Equal)
                                    compareExpression += " " + comparisonOperator + " NULL ";
                                else if (literalFieldValue == null && criterion.ComparisonOperator == Criterion.ComparisonType.NotEqual)
                                    compareExpression += " " + comparisonOperator + " NULL ";

                            }
                            else if (criterion.IsNULL)
                                compareExpression += " " + comparisonOperator + " NULL ";
                            else if (criterion.IsNotNULL)
                                compareExpression += " " + comparisonOperator + " NULL ";
                        }
                        #endregion
                        return compareExpression + ")";

                    }

                case ComparisonTermsType.ParameterWithObject:
                case ComparisonTermsType.ObjectWithParameter:
                    {
                        #region Build comparison expresion with ParameterComparisonTerm
                        MetaDataRepository.ObjectQueryLanguage.DataNode objectTermDataNode = null;

                        object parameterValue = null;
                        if (criterion.LeftTermDataNode != null)
                        {
                            objectTermDataNode = criterion.LeftTermDataNode;
                            parameterValue = (criterion.ComparisonTerms[1] as MetaDataRepository.ObjectQueryLanguage.ParameterComparisonTerm).ParameterValue;
                        }
                        else
                        {
                            objectTermDataNode = criterion.RightTermDataNode;
                            parameterValue = (criterion.ComparisonTerms[0] as MetaDataRepository.ObjectQueryLanguage.ParameterComparisonTerm).ParameterValue;
                        }


                        if (criterion.ComparisonOperator == Criterion.ComparisonType.TypeIs)
                        {
                            MetaDataRepository.Classifier classifier = MetaDataRepository.Classifier.GetClassifier(parameterValue as Type);
                          
                            var storageDataLoader = objectTermDataNode.DataSource.DataLoaders[((objectTermDataNode.ObjectQuery as DistributedObjectQuery).ObjectsContext as PersistenceLayer.ObjectStorage).StorageMetaData.StorageIdentity] as StorageDataLoader;
                            foreach (var storageCell in storageDataLoader.DataLoaderMetadata.StorageCells)
                            {
                                MetaDataRepository.Classifier storageCellClassifier = MetaDataRepository.Classifier.GetClassifier(storageCell.Type.GetExtensionMetaObject(typeof(System.Type)) as System.Type);
                                if (storageCellClassifier.IsA(classifier) || storageCellClassifier == classifier)
                                {
                                    if (compareExpression != null)
                                        compareExpression += " OR ";
                                    else
                                        compareExpression += "(";
                                    compareExpression += DataLoader.RDBMSSQLScriptGenarator.GetSQLScriptForName(objectTermDataNode.Alias) + "." + DataLoader.RDBMSSQLScriptGenarator.GetSQLScriptForName("TypeID") + " = " + (storageCell.Type as RDBMSMetaDataRepository.Class).TypeID.ToString();
                                }
                            }
                            if(string.IsNullOrEmpty(compareExpression))
                                return compareExpression = "(1 = 2)";
                            else
                                return compareExpression + ")";
                        }
                        else
                        {
                            ObjectID objectID = null;
                            StorageInstanceRef storageInstanceRef = null;

                            if (parameterValue != null)
                            {
                                if (!objectTermDataNode.Classifier.GetExtensionMetaObject<Type>().GetMetaData().IsInstanceOfType(parameterValue))
                                {
                                    //TODO Πρέπει να γίνει κατά την διάρκεια του error check του query;
                                    throw new System.Exception("Type mismatch at ");// + ComparisonTermParserNode.ParentNode.Value);
                                }
                                try
                                {
                                    storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(parameterValue) as StorageInstanceRef;
                                    if (storageInstanceRef == null)
                                        objectID = new ObjectID(System.Guid.NewGuid(), 0);
                                }
                                catch
                                {

                                    objectID = new ObjectID(System.Guid.NewGuid(), 0);
                                }
                                objectID = storageInstanceRef.ObjectID as ObjectID;
                            }


                            foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in objectTermDataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.ObjectsContextIdentity].ObjectIdentityTypes)
                            {
                                if (objectID == null || objectIdentityType == objectID.ObjectIdentityType)
                                {
                                    foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                                    {
                                        if (compareExpression != null)
                                        {
                                            if (criterion.ComparisonOperator == Criterion.ComparisonType.Equal) //error prone
                                                compareExpression += " AND ";
                                            else
                                                compareExpression += " OR ";
                                        }
                                        else
                                            compareExpression += "(";
                                        DataNode parentDataNode = objectTermDataNode;
                                        while (parentDataNode.AssignedMetaObject is MetaDataRepository.Attribute && (parentDataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                                            parentDataNode = parentDataNode.ParentDataNode;
                                        compareExpression += DataLoader.RDBMSSQLScriptGenarator.GetSQLScriptForName(parentDataNode.Alias) + "." + DataLoader.RDBMSSQLScriptGenarator.GetSQLScriptForName(part.Name);
                                        if (objectID == null && criterion.ComparisonOperator == Criterion.ComparisonType.Equal)
                                            compareExpression += " IS NULL ";
                                        else if (objectID == null && criterion.ComparisonOperator != Criterion.ComparisonType.Equal)
                                            compareExpression += " IS NOT NULL ";
                                        else
                                            compareExpression += " " + comparisonOperator + "  '" + objectID.GetMemberValue(part.PartTypeName).ToString() + "'";
                                    }
                                    break;
                                }
                            }

                            if (objectID != null && (objectTermDataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.ObjectsContextIdentity] as DataLoader).DataLoaderMetadata.StorageCells.Count == 0)
                            {
                                foreach (MetaDataRepository.IIdentityPart part in DataLoader.NewObjectIdentityType.Parts)
                                {
                                    if (compareExpression != null)
                                    {
                                        if (criterion.ComparisonOperator == Criterion.ComparisonType.Equal) //error prone
                                            compareExpression += " AND ";
                                        else
                                            compareExpression += " OR ";
                                    }
                                    else
                                        compareExpression += "(";
                                    DataNode parentDataNode = objectTermDataNode;
                                    while (parentDataNode.AssignedMetaObject is MetaDataRepository.Attribute && (parentDataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                                        parentDataNode = parentDataNode.ParentDataNode;
                                    compareExpression += DataLoader.RDBMSSQLScriptGenarator.GetSQLScriptForName(parentDataNode.Alias) + "." + DataLoader.RDBMSSQLScriptGenarator.GetSQLScriptForName(part.Name);
                                    compareExpression += " IS NOT NULL ";
                                }

                            }
                        #endregion
                            if (compareExpression == null)
                                compareExpression = "(1 = 2";

                            return compareExpression + ")";
                        }
                    }
                case ComparisonTermsType.Objects:
                    {

                        #region Build comparison expresion with ObjectComparisonTerm
                        MetaDataRepository.ObjectQueryLanguage.DataNode firstTermDataNode = criterion.LeftTermDataNode;
                        MetaDataRepository.ObjectQueryLanguage.DataNode secondTermDataNode = criterion.RightTermDataNode;
                        System.Collections.Generic.IList<MetaDataRepository.IIdentityPart> firstTermIdentityParts = DataLoader.ObjectIdentityTypes[0].Parts;// (firstTermDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity].Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns;
                        System.Collections.Generic.IList<MetaDataRepository.IIdentityPart> secondTermIdentityParts = DataLoader.ObjectIdentityTypes[0].Parts;// (secondTermDataNode.DataSource.DataLoaders[LoadFromStorage.StorageIdentity].Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns;
                        foreach (MetaDataRepository.IIdentityPart CurrColumn in firstTermIdentityParts)
                        {
                            foreach (MetaDataRepository.IIdentityPart CorrespondingCurrColumn in secondTermIdentityParts)
                            {
                                if (CurrColumn.PartTypeName == CorrespondingCurrColumn.PartTypeName && CurrColumn.Type.FullName == CorrespondingCurrColumn.Type.FullName)
                                {
                                    if (compareExpression != null)
                                    {
                                        if (criterion.ComparisonOperator == Criterion.ComparisonType.Equal) //error prone
                                            compareExpression += " AND ";
                                        else
                                            compareExpression += " OR ";
                                    }
                                    else
                                        compareExpression += "(";
                                    DataNode parentDataNode = firstTermDataNode;
                                    while (parentDataNode.AssignedMetaObject is MetaDataRepository.Attribute && (parentDataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                                        parentDataNode = parentDataNode.ParentDataNode;
                                    compareExpression += DataLoader.RDBMSSQLScriptGenarator.GetSQLScriptForName(parentDataNode.Alias) + "." + DataLoader.RDBMSSQLScriptGenarator.GetSQLScriptForName(CurrColumn.Name);

                                    parentDataNode = secondTermDataNode;
                                    while (parentDataNode.AssignedMetaObject is MetaDataRepository.Attribute && (parentDataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                                        parentDataNode = parentDataNode.ParentDataNode;

                                    compareExpression += " " + comparisonOperator + "  " + DataLoader.RDBMSSQLScriptGenarator.GetSQLScriptForName(parentDataNode.Alias);
                                    compareExpression += "." + DataLoader.RDBMSSQLScriptGenarator.GetSQLScriptForName(CorrespondingCurrColumn.Name);
                                }
                            }
                        }
                        #endregion
                        return compareExpression + ")";
                    }
                case ComparisonTermsType.CollectionContainsAnyAll:
                    {
                        if (criterion.ComparisonOperator == Criterion.ComparisonType.ContainsAny)
                            return "(" + DataLoader.RDBMSSQLScriptGenarator.GetSQLScriptForName(DataLoader.DataNode.Alias + (criterion.ComparisonTerms[1] as ParameterComparisonTerm).ParameterName) + "." + DataLoader.RDBMSSQLScriptGenarator.GetSQLScriptForName("ContainsAny") + " = 1 )";
                        else
                            return "(" + DataLoader.RDBMSSQLScriptGenarator.GetSQLScriptForName(DataLoader.DataNode.Alias + (criterion.ComparisonTerms[1] as ParameterComparisonTerm).ParameterName) + "." + DataLoader.RDBMSSQLScriptGenarator.GetSQLScriptForName("ContainsAll") + " = 1 )";
                    }
            }
            return "";
        }

        /// <MetaDataID>{8ea24a75-a1b3-4353-9dff-42913012ecb9}</MetaDataID>
        string GetLikeString(string likeValue, string literalValue)
        {
            likeValue = likeValue.Replace("/", "//").Replace("%", "/%");
            int npos = 0;
            npos = likeValue.IndexOf("*");
            while (npos != -1)
            {
                if (likeValue.Length > npos + 1 && likeValue[npos + 1] == '*')
                    likeValue = likeValue.Remove(npos + 1, 1);
                else
                {
                    char[] chrs = likeValue.ToCharArray();
                    chrs[npos] = '%';
                    likeValue = new string(chrs);
                }
                npos = likeValue.IndexOf("*", npos + 1);
            }
            literalValue += "'" + likeValue + "' ESCAPE '/' ";
            return literalValue;
        }
    }

     
}
