IF OBJECT_ID('dbo.Pub_s') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.Pub_s'
	DROP PROCEDURE dbo.Pub_s
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.Pub_s') IS NOT NULL
		PRINT '***********Drop of dbo.Pub_s FAILED.'
END
GO
PRINT 'Creating dbo.Pub_s'
GO

create PROCEDURE dbo.Pub_s
/*
* PARAMETERS:
*	none
*
* DESCRIPTION:
*	Retrieves a list of all of the pubs available in the admin system.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   ADMINSYSTEM.pub
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 06/26/2007      BJS             Initial Creation
* 07/25/2007      BJS             Modified reference to admin system.  No longer uses a linked server 
*
*/
as

select Pub_ID, Pub_NM
from DBADVPROD.informix.pub
where pub_type_cd = 'N' /*Newspaper*/
order by Pub_NM
GO

GRANT  EXECUTE  ON [dbo].[Pub_s]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
