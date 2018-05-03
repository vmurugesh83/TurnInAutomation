IF OBJECT_ID('dbo.PubLoc_s') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubLoc_s'
	DROP PROCEDURE dbo.PubLoc_s
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubLoc_s') IS NOT NULL
		PRINT '***********Drop of dbo.PubLoc_s FAILED.'
END
GO
PRINT 'Creating dbo.PubLoc_s'
GO

create PROCEDURE dbo.PubLoc_s
/*
* PARAMETERS:
*
* DESCRIPTION:
*	Retrieves a list of all publications.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   ADMINSYSTEM.pub
*   ADMINSYSTEM.pub_loc
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
* 06/29/2007      BJS             Changed to return ALL pub locations.  Filtering is on front-end
* 07/25/2007      BJS             Modified reference to admin db.  No longer uses linked server
* 11/19/2007      JRH             Filter out location "99" from all pubs.  This is used for the
*                                 the old spreadsheets and means all locations.
*
*/
as
select p.Pub_ID, l.PubLoc_ID
from DBADVPROD.informix.pub p
	join DBADVPROD.informix.pub_loc l on p.Pub_ID = l.Pub_ID
where p.Pub_Type_CD = 'N'
	and l.PubLoc_ID <> 99
GO

GRANT  EXECUTE  ON [dbo].[PubLoc_s]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
