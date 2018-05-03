IF OBJECT_ID('dbo.PubRateMap_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubRateMap_i'
	DROP PROCEDURE dbo.PubRateMap_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubRateMap_i') IS NOT NULL
		PRINT '***********Drop of dbo.PubRateMap_i FAILED.'
END
GO
PRINT 'Creating dbo.PubRateMap_i'
GO

CREATE PROCEDURE dbo.PubRateMap_i
/*
* PARAMETERS:
*	PUB_PubRate_Map_ID
* Pub_ID
* PubLoc_ID
* CreatedBy
*
* DESCRIPTION:
*	Inserts a record into the PUB_PubRate_Map table
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_PubRate_Map			Insert
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
* 06/29/2007      BJS             Initial Creation 
*
*/
@PUB_PubRate_Map_ID bigint output,
@Pub_ID char(3),
@PubLoc_ID int,
@CreatedBy varchar(50)
AS

insert into PUB_PubRate_Map(Pub_ID, PubLoc_ID, CreatedBy, CreatedDate)
values(@Pub_ID, @PubLoc_ID, @CreatedBy, getdate())
set @PUB_PubRate_Map_ID = @@identity

GO

GRANT  EXECUTE  ON [dbo].[PubRateMap_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
