IF OBJECT_ID('dbo.PstPostalScenario_s_ByDescriptionandRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PstPostalScenario_s_ByDescriptionandRunDate'
	DROP PROCEDURE dbo.PstPostalScenario_s_ByDescriptionandRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PstPostalScenario_s_ByDescriptionandRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.PST_PostalScenario_d FAILED.'
END
GO
PRINT 'Creating dbo.PstPostalScenario_s_ByDescriptionandRunDate'
GO

create proc dbo.PstPostalScenario_s_ByDescriptionandRunDate
/*
* PARAMETERS:
* Description - required.
* RunDate - required.
*
* DESCRIPTION:
* Returns the Postal Scenario that matches the Description on the date specified.
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
* 09/04/2007      BJS             Initial Creation
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@Description varchar(35),
@RunDate datetime
as

select top 1 * from PST_PostalScenario
where Description = @Description and EffectiveDate <= @RunDate
order by EffectiveDate desc

GO

GRANT  EXECUTE  ON [dbo].[PstPostalScenario_s_ByDescriptionandRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
