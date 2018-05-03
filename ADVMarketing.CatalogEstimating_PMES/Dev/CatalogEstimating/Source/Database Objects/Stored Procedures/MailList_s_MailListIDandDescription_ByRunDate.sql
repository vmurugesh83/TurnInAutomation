IF OBJECT_ID('dbo.MailList_s_MailListIDandDescription_ByRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.MailList_s_MailListIDandDescription_ByRunDate'
	DROP PROCEDURE dbo.MailList_s_MailListIDandDescription_ByRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.MailList_s_MailListIDandDescription_ByRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.MailList_s_MailListIDandDescription_ByRunDate FAILED.'
END
GO
PRINT 'Creating dbo.MailList_s_MailListIDandDescription_ByRunDate'
GO

CREATE PROC dbo.MailList_s_MailListIDandDescription_ByRunDate
/*
* PARAMETERS:
* RunDate - Required
*
* DESCRIPTION:
*		Returns a list of Vendor MailList ID's and Vendor Descriptions for the specified RunDate
*
* TABLES:
*		Table Name					Access
*		==========					======
*		Vnd_Vendor					READ
*		vnd_maillistresourcerate    READ
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

SELECT v.vnd_vendor_id, max(m.vnd_maillistresourcerate_id) vnd_maillistresourcerate_id, max(v.Description) Description
FROM Vnd_Vendor v JOIN vnd_maillistresourcerate m ON v.vnd_vendor_id = m.vnd_vendor_id
	LEFT JOIN vnd_maillistresourcerate newer_m ON v.vnd_vendor_id = newer_m.vnd_vendor_id and newer_m.EffectiveDate <= @RunDate and newer_m.EffectiveDate > m.EffectiveDate
WHERE m.EffectiveDate <= @RunDate and newer_m.vnd_maillistresourcerate_id is null
GROUP BY v.vnd_vendor_id
ORDER BY v.Description
GO

GRANT  EXECUTE  ON [dbo].[MailList_s_MailListIDandDescription_ByRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
