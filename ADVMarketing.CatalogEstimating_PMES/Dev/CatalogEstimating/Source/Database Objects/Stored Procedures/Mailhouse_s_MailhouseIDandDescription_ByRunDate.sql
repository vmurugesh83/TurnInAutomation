IF OBJECT_ID('dbo.Mailhouse_s_MailhouseIDandDescription_ByRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.Mailhouse_s_MailhouseIDandDescription_ByRunDate'
	DROP PROCEDURE dbo.Mailhouse_s_MailhouseIDandDescription_ByRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.Mailhouse_s_MailhouseIDandDescription_ByRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.Mailhouse_s_MailhouseIDandDescription_ByRunDate FAILED.'
END
GO
PRINT 'Creating dbo.Mailhouse_s_MailhouseIDandDescription_ByRunDate'
GO

CREATE PROC dbo.Mailhouse_s_MailhouseIDandDescription_ByRunDate
/*
* PARAMETERS:
* RunDate - Required
*
* DESCRIPTION:
*		Returns a list of Vendor mailhouse ID's and Vendor Descriptions for the specified RunDate
*
* TABLES:
*		Table Name					Access
*		==========					======
*		Vnd_Vendor					READ
*		vnd_mailhouserate			READ
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
SELECT
	vendor.Description,
	mailhouse.vnd_vendor_id, 
    mailhouse.vnd_mailhouserate_id, 
	mailhouse.postaldropcwt, 
	mailhouse.gluetackdefault,
	mailhouse.tabbingdefault, 
	mailhouse.letterinsertiondefault
FROM
	vnd_vendor vendor
	INNER JOIN (
		SELECT 
			vnd_mailhouserate.vnd_vendor_id, 
			vnd_mailhouserate.vnd_mailhouserate_id, 
			vnd_mailhouserate.postaldropcwt, 
			vnd_mailhouserate.gluetackdefault,
			vnd_mailhouserate.tabbingdefault, 
			vnd_mailhouserate.letterinsertiondefault
		FROM 
			vnd_mailhouserate
			INNER JOIN ( 
				SELECT
					vnd_vendor_id,
					max(effectivedate) as effectivedate
				FROM
					vnd_mailhouserate
				WHERE
					effectivedate <= @RunDate
				GROUP BY
					vnd_vendor_id
				) effective
			ON vnd_mailhouserate.vnd_vendor_id = effective.vnd_vendor_id AND
			   vnd_mailhouserate.effectivedate = effective.effectivedate
		) mailhouse
	ON vendor.vnd_vendor_id = mailhouse.vnd_vendor_id

WHERE
	vendor.active = 1
ORDER BY
	vendor.Description ASC

GO

GRANT  EXECUTE  ON [dbo].[Mailhouse_s_MailhouseIDandDescription_ByRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
