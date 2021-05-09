declare @distinctCount integer
declare @OneToOne bit
declare @count integer

SELECT     @count =COUNT(*) 
FROM         (SELECT IDB
                       FROM          Table1) mTable


SELECT     @distinctCount =COUNT(*) 
FROM         (SELECT DISTINCT IDB
                       FROM          Table1) mTable

if @distinctCount = @count
	set @OneToOne=1
else
	set @OneToOne=0


SELECT     @OneToOne

 