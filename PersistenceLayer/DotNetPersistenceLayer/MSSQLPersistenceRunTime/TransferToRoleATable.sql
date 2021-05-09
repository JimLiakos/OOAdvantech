BEGIN TRANSACTION



UPDATE    TableB
SET              TableB.IDB = Relation.IDB
FROM         TableA INNER JOIN
                      Relation ON TableA.IDA = Relation.IDA
GO
ALTER TABLE dbo.TableB
	DROP COLUMN IDA

COMMIT




UPDATE   [T_Order] 
set [T_Order].[ClientOrders_ObjectIDB]=[T_Client].[ObjectID]
FROM  [T_Client] INNER JOIN [T_Order] on [T_Client].[ClientOrders_ObjectIDA]=[T_Order].[ObjectID]         


UPDATE   [T_Order] 
set [T_Order].[ClientOrders_ObjectIDB]=[T_Client].[ObjectID]
FROM [T_Client]  INNER JOIN   [T_Order] ON [T_Client].[ClientOrders_ObjectIDA]=[T_Order].[ObjectID]