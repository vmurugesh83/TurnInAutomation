IF OBJECT_ID('dbo.Paper_s_PaperIDandDescription_ByRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.Paper_s_PaperIDandDescription_ByRunDate'
	DROP PROCEDURE dbo.Paper_s_PaperIDandDescription_ByRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.Paper_s_PaperIDandDescription_ByRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.Paper_s_PaperIDandDescription_ByRunDate FAILED.'
END
GO
PRINT 'Creating dbo.Paper_s_PaperIDandDescription_ByRunDate'
GO

create proc dbo.Paper_s_PaperIDandDescription_ByRunDate
/*
* PARAMETERS:
* RunDate - Required
*
*
* DESCRIPTION:
*		Returns a list of Vendor Paper ID's and Vendor Descriptions for the specified RunDate
*
*
* TABLES:
*  Table Name     Access
*  ==========     ======
*  VND_Vendor     READ
*  VND_Paper      READ
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
* Date              Who         Comments
* -------------     --------    -------------------------------------------------
* 08/16/2007        BJS         Initial Creation 
* 09/24/2007        BJS         Added VND_Paper_ID parameter
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@VND_Paper_ID bigint,
@RunDate datetime
as

select v.VND_Vendor_ID, max(p.VND_Paper_ID) VND_Paper_ID, max(v.Description) Description
from VND_Vendor v join VND_Paper p on v.VND_Vendor_ID = p.VND_Vendor_ID
	left join VND_Paper newer_p on v.VND_Vendor_ID = newer_p.VND_Vendor_ID and newer_p.EffectiveDate <= @RunDate and newer_p.EffectiveDate > p.EffectiveDate
where (v.Active = 1 or p.VND_Paper_ID = @VND_Paper_ID) and p.EffectiveDate <= @RunDate and newer_p.VND_Paper_ID is null
group by v.VND_Vendor_ID
order by v.Description
GO

GRANT  EXECUTE  ON [dbo].[Paper_s_PaperIDandDescription_ByRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
