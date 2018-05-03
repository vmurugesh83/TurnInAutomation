IF OBJECT_ID('dbo.PaperMap_s_PaperMapID_ByOldPaperMapIDandRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PaperMap_s_PaperMapID_ByOldPaperMapIDandRunDate'
	DROP PROCEDURE dbo.PaperMap_s_PaperMapID_ByOldPaperMapIDandRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PaperMap_s_PaperMapID_ByOldPaperMapIDandRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.PaperMap_s_PaperMapID_ByOldPaperMapIDandRunDate FAILED.'
END
GO
PRINT 'Creating dbo.PaperMap_s_PaperMapID_ByOldPaperMapIDandRunDate'
GO

CREATE proc dbo.PaperMap_s_PaperMapID_ByOldPaperMapIDandRunDate
/*
* PARAMETERS:
* PPR_Paper_Map_ID - required
* RunDate - required
*
* DESCRIPTION:
*		Determines the parent vendor record of PPR_Paper_Map_ID.  Returns a paper map record with the same parent vendor for the specified run date
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   VND_Paper           READ
*
*
* PROCEDURES CALLED:
*
*
* DATABASE:
*		All
*
*
* RETURN VALUE:
* None 
*
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 08/16/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@PPR_Paper_Map_ID bigint,
@RunDate datetime
as

declare @VND_Vendor_ID bigint, @VND_Paper_ID bigint, @PaperMapDesc varchar(35)

-- Identify the VendorID and the original Paper Map description
select @VND_Vendor_ID = p.VND_Vendor_ID, @PaperMapDesc = pm.Description
from VND_Paper p join PPR_Paper_Map pm on p.VND_Paper_ID = pm.VND_Paper_ID
where pm.PPR_Paper_Map_ID = @PPR_Paper_Map_ID

-- Find the new PaperID
select top 1 @VND_Paper_ID = p.VND_Paper_ID
from VND_Paper p
where p.VND_Vendor_ID = @VND_Vendor_ID and p.EffectiveDate <= @RunDate
order by p.EffectiveDate desc

-- If the original Paper Map is still available return it
if exists(select 1 from PPR_Paper_Map where VND_Paper_ID = @VND_Paper_ID and PPR_Paper_Map_ID = @PPR_Paper_Map_ID) begin
	select * from PPR_Paper_Map where PPR_Paper_Map_ID = @PPR_Paper_Map_ID
end
--Otherwise try to return a paper map with an identical description
else if exists(select 1 from PPR_Paper_Map where VND_Paper_ID = @VND_Paper_ID and Description = @PaperMapDesc) begin
	select * from PPR_Paper_Map where VND_Paper_ID = @VND_Paper_ID and Description = @PaperMapDesc
end
-- The last resort is to try to return the default paper map
else begin
	select * From PPR_Paper_Map where VND_Paper_ID = @VND_Paper_ID and [default] = 1
end
GO

GRANT  EXECUTE  ON [dbo].[PaperMap_s_PaperMapID_ByOldPaperMapIDandRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
