IF OBJECT_ID('dbo.PubRateMap_s_withPubDescription') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubRateMap_s_withPubDescription'
	DROP PROCEDURE dbo.PubRateMap_s_withPubDescription
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubRateMap_s_withPubDescription') IS NOT NULL
		PRINT '***********Drop of dbo.PubRateMap_s_withPubDescription FAILED.'
END
GO
PRINT 'Creating dbo.PubRateMap_s_withPubDescription'
GO

CREATE PROCEDURE dbo.PubRateMap_s_withPubDescription
/*
* PARAMETERS:
*	None
*
* DESCRIPTION:
*	Returns all publication locations in the database along with the publication description from the admin system.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_PubRate_Map
*   ADMINSYSTEM.Pub
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
* 06/22/2007      BJS             Initial Creation
* 06/28/2007      BJS             Added join to admin system
*
*/
as
select prm.*, p.Pub_NM
from PUB_PubRate_Map prm join DBADVPROD.informix.pub p on prm.Pub_ID = p.Pub_ID
where p.Pub_Type_CD = 'N'

GO

GRANT  EXECUTE  ON [dbo].[PubRateMap_s_withPubDescription]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
