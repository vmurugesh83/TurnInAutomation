IF OBJECT_ID('dbo.Paper_s_PaperID_ByOldPaperIDandRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.Paper_s_PaperID_ByOldPaperIDandRunDate'
	DROP PROCEDURE dbo.Paper_s_PaperID_ByOldPaperIDandRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.Paper_s_PaperID_ByOldPaperIDandRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.Paper_s_PaperID_ByOldPaperIDandRunDate FAILED.'
END
GO
PRINT 'Creating dbo.Paper_s_PaperID_ByOldPaperIDandRunDate'
GO

CREATE proc dbo.Paper_s_PaperID_ByOldPaperIDandRunDate
/*
* PARAMETERS:
* VND_Paper_ID - required
* RunDate - required
*
* DESCRIPTION:
*		Determines the parent vendor record of VND_Paper_ID.  Returns a paper record with the same parent vendor for the specified run date
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   VND_Paper         READ
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
@VND_Paper_ID bigint,
@RunDate datetime
as

declare @VND_Vendor_ID bigint

select @VND_Vendor_ID = VND_Vendor_ID
from VND_Paper
where VND_Paper_ID = @VND_Paper_ID

select top 1 p.*
from VND_Paper p
where p.VND_Vendor_ID = @VND_Vendor_ID and p.EffectiveDate <= @RunDate
order by p.EffectiveDate desc
GO

GRANT  EXECUTE  ON [dbo].[Paper_s_PaperID_ByOldPaperIDandRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
