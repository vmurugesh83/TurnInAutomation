IF OBJECT_ID('dbo.VndMailHouseRate_s_ByOldIDandRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.VndMailHouseRate_s_ByOldIDandRunDate'
	DROP PROCEDURE dbo.VndMailHouseRate_s_ByOldIDandRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.VndMailHouseRate_s_ByOldIDandRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.VndMailHouseRate_s_ByOldIDandRunDate FAILED.'
END
GO
PRINT 'Creating dbo.VndMailHouseRate_s_ByOldIDandRunDate'
GO

CREATE proc dbo.VndMailHouseRate_s_ByOldIDandRunDate
/*
* PARAMETERS:
* VND_MailHouseRate_ID - required
* RunDate              - required
*
* DESCRIPTION:
*		Determines the parent vendor record of VND_MailHouseRate_ID.  Returns a mailhouse record with the same parent vendor for the specified run date
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   VND_MailHouseRate   READ
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
@VND_MailHouseRate_ID bigint,
@RunDate datetime
as

declare @VND_Vendor_ID bigint

select @VND_Vendor_ID = VND_Vendor_ID
from VND_MailHouseRate
where VND_MailHouseRate_ID = @VND_MailHouseRate_ID

select top 1 mh.*
from VND_MailHouseRate mh
where mh.VND_Vendor_ID = @VND_Vendor_ID and mh.EffectiveDate <= @RunDate
order by mh.EffectiveDate desc
GO

GRANT  EXECUTE  ON [dbo].[VndMailHouseRate_s_ByOldIDandRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
