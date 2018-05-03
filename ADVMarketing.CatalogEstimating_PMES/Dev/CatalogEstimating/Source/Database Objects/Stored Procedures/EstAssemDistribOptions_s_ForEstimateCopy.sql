IF OBJECT_ID('dbo.EstAssemDistribOptions_s_ForEstimateCopy') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstAssemDistribOptions_s_ForEstimateCopy'
	DROP PROCEDURE dbo.EstAssemDistribOptions_s_ForEstimateCopy
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstAssemDistribOptions_s_ForEstimateCopy') IS NOT NULL
		PRINT '***********Drop of dbo.EstAssemDistribOptions_s_ForEstimateCopy FAILED.'
END
GO
PRINT 'Creating dbo.EstAssemDistribOptions_s_ForEstimateCopy'
GO

create proc dbo.EstAssemDistribOptions_s_ForEstimateCopy
/*
* PARAMETERS:
* EST_Estimate_ID
*
* DESCRIPTION:
*		Returns an estimate's assembly and distribution options and it's vendor and rate descriptions
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Component       READ
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
* 09/04/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EST_Estimate_ID bigint
as
select ad.*, f_vnd.Description InsertFreightDesc, ps.Description PostalScenarioDesc, mh_vnd.Description MailHouseDesc,
	mt_vnd.Description MailTrackingDesc, ml_vnd.Description MailListDesc
from EST_AssemDistribOptions ad join VND_Vendor f_vnd on ad.InsertFreightVendor_ID = f_vnd.VND_Vendor_ID
	join PST_PostalScenario ps on ad.PST_PostalScenario_ID = ps.PST_PostalScenario_ID
	join VND_MailHouseRate mh_rate on ad.MailHouse_ID = mh_rate.VND_MailHouseRate_ID
	join VND_Vendor mh_vnd on mh_rate.VND_Vendor_ID = mh_vnd.VND_Vendor_ID
	left join VND_MailTrackingRate mt_rate on ad.MailTracking_ID = mt_rate.VND_MailTrackingRate_ID
	left join VND_Vendor mt_vnd on mt_rate.VND_Vendor_ID = mt_vnd.VND_Vendor_ID
	left join VND_MaillistResourceRate ml_rate on ad.MailListResource_ID = ml_rate.VND_MailListResourceRate_ID
	left join VND_Vendor ml_vnd on ml_rate.VND_Vendor_ID = ml_vnd.VND_Vendor_ID
where ad.EST_Estimate_ID = @EST_Estimate_ID
GO

GRANT  EXECUTE  ON [dbo].[EstAssemDistribOptions_s_ForEstimateCopy]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
