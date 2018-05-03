IF OBJECT_ID('dbo.VndMailListResourceRate_s_ByOldIDandRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.VndMailListResourceRate_s_ByOldIDandRunDate'
	DROP PROCEDURE dbo.VndMailListResourceRate_s_ByOldIDandRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.VndMailListResourceRate_s_ByOldIDandRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.VndMailListResourceRate_s_ByOldIDandRunDate FAILED.'
END
GO
PRINT 'Creating dbo.VndMailListResourceRate_s_ByOldIDandRunDate'
GO

CREATE proc dbo.VndMailListResourceRate_s_ByOldIDandRunDate
/*
* PARAMETERS:
* VND_MailListResourceRate_ID - required
* RunDate              - required
*
* DESCRIPTION:
*		Determines the parent vendor record of VND_MailListResourceRate_ID.  Returns a MailListResource record with the same parent vendor for the specified run date
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   VND_MailListResourceRate   READ
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
* 11/02/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@VND_MailListResourceRate_ID bigint,
@RunDate datetime
as

declare @VND_Vendor_ID bigint

select @VND_Vendor_ID = VND_Vendor_ID
from VND_MailListResourceRate
where VND_MailListResourceRate_ID = @VND_MailListResourceRate_ID

select top 1 ml.*
from VND_MailListResourceRate ml
where ml.VND_Vendor_ID = @VND_Vendor_ID and ml.EffectiveDate <= @RunDate
order by ml.EffectiveDate desc
GO

GRANT  EXECUTE  ON [dbo].[VndMailListResourceRate_s_ByOldIDandRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
