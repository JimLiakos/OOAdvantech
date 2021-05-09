declare @distinctCount integer
declare @roleAIsMany bit
declare @roleBIsMany bit
declare @count integer
SELECT     @count =COUNT(*) 
FROM         (SELECT IDB FROM Table1) mTable

SELECT     @distinctCount =COUNT(*) 
FROM         (SELECT DISTINCT IDB FROM Table1) mTable

if @distinctCount = @count
	set @roleBIsMany=0
else
	set @roleBIsMany=1


SELECT     @count =COUNT(*) 
FROM         (SELECT IDA FROM Table1) mTable

SELECT     @distinctCount =COUNT(*) 
FROM         (SELECT DISTINCT IDA FROM Table1) mTable

if @distinctCount = @count
	set @roleAIsMany=0
else
	set @roleAIsMany=1


SELECT     @roleAIsMany as RoleAIsMany,@roleBIsMany RoleBIsMany

 