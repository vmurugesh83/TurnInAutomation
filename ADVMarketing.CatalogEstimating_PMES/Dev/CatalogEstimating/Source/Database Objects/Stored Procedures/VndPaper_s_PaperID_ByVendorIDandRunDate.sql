IF OBJECT_ID('dbo.VndPaper_s_PaperID_ByVendorIDandRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.VndPaper_s_PaperID_ByVendorIDandRunDate'
	DROP PROCEDURE dbo.VndPaper_s_PaperID_ByVendorIDandRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.VndPaper_s_PaperID_ByVendorIDandRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.VndPaper_s_PaperID_ByVendorIDandRunDate FAILED.'
END
GO
PRINT 'Creating dbo.VndPaper_s_PaperID_ByVendorIDandRunDate'
GO

create proc dbo.VndPaper_s_PaperID_ByVendorIDandRunDate
/*
* PARAMETERS:
* VND_Vendor_ID - Required.  The Vendor.
* RunDate - Required.  The Estimate Run Date.
* VND_Paper_ID - Output.
*
*
* DESCRIPTION:
*		Returns the PaperID that is active on the RunDate for the VND_Vendor_ID.
*
*
* TABLES:
*		Table Name			Access
*		==========			======
*		VND_Paper
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
*	Date						Who							Comments
*	------------- 	--------        -------------------------------------------------
* 05/23/2007			BJS							Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@VND_Vendor_ID bigint,
@RunDate datetime,
@VND_Paper_ID bigint output
as

select top 1 @VND_Paper_ID = VND_Paper_ID
from VND_Paper
where VND_Vendor_ID = @VND_Vendor_ID and EffectiveDate <= @RunDate
order by EffectiveDate desc


GO

GRANT  EXECUTE  ON [dbo].[VndPaper_s_PaperID_ByVendorIDandRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
