BEGIN TRANSACTION

CREATE TABLE dbo.T_Relation
	(
	IDA int NULL,
	IDB int NULL
	)  ON [PRIMARY]
GO
INSERT INTO T_Relation
                      (IDA, IDB)
SELECT     IDA, IDB
FROM         Relation

GO


ALTER TABLE dbo.TableA
	DROP COLUMN IDB
GO

COMMIT