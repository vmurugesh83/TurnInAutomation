/* Drop all User Stored Procedures */

DECLARE @sp_name nvarchar(255)
DECLARE @exec_stmt nvarchar(300)

DECLARE sp_cursor CURSOR FOR
SELECT name FROM dbo.sysobjects
WHERE category = 0 and type = 'P'

OPEN sp_cursor
FETCH NEXT FROM sp_cursor INTO @sp_name
WHILE @@FETCH_STATUS = 0 BEGIN
	set @exec_stmt = 'DROP PROC ' + @sp_name
	EXEC sp_executesql @exec_stmt
	FETCH NEXT FROM sp_cursor INTO @sp_name
END

CLOSE sp_cursor
DEALLOCATE sp_cursor
GO

/* Drop All User-Defined Functions */
DECLARE @sp_name nvarchar(255)
DECLARE @exec_stmt nvarchar(300)

DECLARE sp_cursor CURSOR FOR
SELECT name FROM dbo.sysobjects
WHERE category = 0 and type in ('FN', 'TF')

OPEN sp_cursor
FETCH NEXT FROM sp_cursor INTO @sp_name
WHILE @@FETCH_STATUS = 0 BEGIN
	set @exec_stmt = 'DROP FUNCTION ' + @sp_name
	EXEC sp_executesql @exec_stmt
	FETCH NEXT FROM sp_cursor INTO @sp_name
END

CLOSE sp_cursor
DEALLOCATE sp_cursor
GO

/* Drop all Views */
DECLARE @sp_name nvarchar(255)
DECLARE @exec_stmt nvarchar(300)

DECLARE sp_cursor CURSOR FOR
SELECT name FROM dbo.sysobjects
WHERE category = 0 and type = 'V'

OPEN sp_cursor
FETCH NEXT FROM sp_cursor INTO @sp_name
WHILE @@FETCH_STATUS = 0 BEGIN
	set @exec_stmt = 'DROP VIEW ' + @sp_name
	EXEC sp_executesql @exec_stmt
	FETCH NEXT FROM sp_cursor INTO @sp_name
END

CLOSE sp_cursor
DEALLOCATE sp_cursor
GO
