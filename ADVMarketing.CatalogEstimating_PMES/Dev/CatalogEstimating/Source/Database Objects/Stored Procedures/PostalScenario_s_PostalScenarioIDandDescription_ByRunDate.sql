IF OBJECT_ID('dbo.PostalScenario_s_PostalScenarioIDandDescription_ByRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PostalScenario_s_PostalScenarioIDandDescription_ByRunDate'
	DROP PROCEDURE dbo.PostalScenario_s_PostalScenarioIDandDescription_ByRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PostalScenario_s_PostalScenarioIDandDescription_ByRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.PostalScenario_s_PostalScenarioIDandDescription_ByRunDate FAILED.'
END
GO
PRINT 'Creating dbo.PostalScenario_s_PostalScenarioIDandDescription_ByRunDate'
GO

CREATE PROC dbo.PostalScenario_s_PostalScenarioIDandDescription_ByRunDate
/*
* PARAMETERS:
* RunDate - Required
*
* DESCRIPTION:
*		Returns a list of postal scenario ID's and Descriptions for the specified RunDate
*
* TABLES:
*		Table Name					Access
*		==========					======
*		pst_postalscenario			READ
*		pst_postalclass				READ
*		pst_postalmailertype		READ
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
*	Date		Who		Comments
*	------------- 	--------        -------------------------------------------------
*	08/28/2007	NLS		Initial Creation 
*	11/20/2007	JRH		Fixed the selection based on effective date.
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@RunDate datetime

AS
SELECT
	max(postal.pst_postalscenario_id) pst_postalscenario_id,
	postal.description description,
	max(mailer.description) postalmailertype,
	max(class.description) postalclass
FROM
	(SELECT
			postal.description description,
			max(postal.effectivedate) effectivedate
		FROM
			pst_postalscenario postal 
		WHERE
			postal.effectivedate <= @RunDate
		GROUP BY
		 	postal.description) eps
	INNER JOIN pst_postalscenario postal
		ON eps.description = postal.description
		AND eps.effectivedate = postal.effectivedate
	INNER JOIN pst_postalmailertype mailer ON
		postal.pst_postalmailertype_id = mailer.pst_postalmailertype_id
	INNER JOIN pst_postalclass class ON
		postal.pst_postalclass_id = class.pst_postalclass_id
GROUP BY
	postal.description
ORDER BY
	postal.description
	
GO

GRANT  EXECUTE  ON [dbo].[PostalScenario_s_PostalScenarioIDandDescription_ByRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO