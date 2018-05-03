IF OBJECT_ID('dbo.PstPostalScenario_s_ByOldIDandRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PstPostalScenario_s_ByOldIDandRunDate'
	DROP PROCEDURE dbo.PstPostalScenario_s_ByOldIDandRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PstPostalScenario_s_ByOldIDandRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.PST_PostalScenario_d FAILED.'
END
GO
PRINT 'Creating dbo.PstPostalScenario_s_ByOldIDandRunDate'
GO

create proc dbo.PstPostalScenario_s_ByOldIDandRunDate
/*
* PARAMETERS:
* PST_PostalScenario_ID - required.
* RunDate - required.
*
* DESCRIPTION:
* Determines the parent vendor of the postal scenario specified.  Returns the Postal Scenario that matches the vendor on the date specified.
*
*
* TABLES:
*   Table Name                     Access
*   ==========                     ======
*   PST_PostalScenario             READ
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
@PST_PostalScenario_ID bigint,
@RunDate datetime
as

declare @VND_Vendor_ID bigint, @Description varchar(35), @PST_PostalMailerType_ID int, @PST_PostalClass_ID int

select @VND_Vendor_ID = pw.VND_Vendor_ID, @Description = Description, @PST_PostalMailerType_ID = ps.PST_PostalMailerType_ID,
	@PST_PostalClass_ID = ps.PST_PostalClass_ID
from PST_PostalScenario ps join PST_PostalCategoryScenario_Map pcsm on ps.PST_PostalScenario_ID = pcsm.PST_PostalScenario_ID
	join PST_PostalCategoryRate_Map pcrm on pcsm.PST_PostalCategoryRate_Map_ID = pcrm.PST_PostalCategoryRate_Map_ID
	join PST_PostalWeights pw on pcrm.PST_PostalWeights_ID = pw.PST_PostalWeights_ID
where ps.PST_PostalScenario_ID = @PST_PostalScenario_ID

select top 1 ps.*
from PST_PostalScenario ps join PST_PostalCategoryScenario_Map pcsm on ps.PST_PostalScenario_ID = pcsm.PST_PostalScenario_ID
	join PST_PostalCategoryRate_Map pcrm on pcsm.PST_PostalCategoryRate_Map_ID = pcrm.PST_PostalCategoryRate_Map_ID
	join PST_PostalWeights pw on pcrm.PST_PostalWeights_ID = pw.PST_PostalWeights_ID
where pw.VND_Vendor_ID = @VND_Vendor_ID and ps.Description = @Description and ps.PST_PostalMailerType_ID = @PST_PostalMailerType_ID
	and ps.PST_PostalClass_ID = @PST_PostalClass_ID and ps.EffectiveDate <= @RunDate
order by ps.EffectiveDate desc
GO

GRANT  EXECUTE  ON [dbo].[PstPostalScenario_s_ByOldIDandRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
