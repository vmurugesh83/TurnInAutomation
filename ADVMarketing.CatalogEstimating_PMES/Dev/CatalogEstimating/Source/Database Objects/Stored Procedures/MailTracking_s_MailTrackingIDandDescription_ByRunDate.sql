IF OBJECT_ID('dbo.MailTracking_s_MailTrackingIDandDescription_ByRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.MailTracking_s_MailTrackingIDandDescription_ByRunDate'
	DROP PROCEDURE dbo.MailTracking_s_MailTrackingIDandDescription_ByRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.MailTracking_s_MailTrackingIDandDescription_ByRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.MailTracking_s_MailTrackingIDandDescription_ByRunDate FAILED.'
END
GO
PRINT 'Creating dbo.MailTracking_s_MailTrackingIDandDescription_ByRunDate'
GO

CREATE PROC dbo.MailTracking_s_MailTrackingIDandDescription_ByRunDate
/*
* PARAMETERS:
* RunDate - Required
*
* DESCRIPTION:
*		Returns a list of Vendor mailtracking ID's and Vendor Descriptions for the specified RunDate
*
* TABLES:
*		Table Name					Access
*		==========					======
*		Vnd_Vendor					READ
*		vnd_mailtrackingrate	    READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*		All
*
* RETURN VALUE:
* None 
*
* REVISION HISTORY:
*
*	Date				Who					Comments
*	------------- 	--------        -------------------------------------------------
*	08/28/2007			NLS					Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@RunDate datetime

AS

SELECT v.vnd_vendor_id, max(m.vnd_mailtrackingrate_id) vnd_mailtrackingrate_id, max(v.Description) Description
FROM Vnd_Vendor v JOIN vnd_mailtrackingrate m ON v.vnd_vendor_id = m.vnd_vendor_id
	LEFT JOIN vnd_mailtrackingrate newer_m ON v.vnd_vendor_id = newer_m.vnd_vendor_id and newer_m.EffectiveDate <= @RunDate and newer_m.EffectiveDate > m.EffectiveDate
WHERE m.EffectiveDate <= @RunDate and newer_m.vnd_mailtrackingrate_id is null
GROUP BY v.vnd_vendor_id
ORDER BY v.Description
GO

GRANT  EXECUTE  ON [dbo].[MailTracking_s_MailTrackingIDandDescription_ByRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
