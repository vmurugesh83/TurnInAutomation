IF OBJECT_ID('dbo.getFiscalYear') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.getFiscalYear'
	DROP FUNCTION dbo.getFiscalYear
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.getFiscalYear') IS NOT NULL
		PRINT '***********Drop of dbo.getFiscalYear FAILED.'
END
GO
PRINT 'Creating dbo.getFiscalYear'
GO

CREATE FUNCTION dbo.getFiscalYear (@inDate datetime)
RETURNS int
AS  
/*
* PARAMETERS:
* @inDate - Date to process
*
* DESCRIPTION:
*	Determines fiscal year inputted date.
*	NOTE: This logic was copied from legacy PowerBuilder code.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	curr_fiscal_yr - The Fiscal Year for the inputted date.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 06/21/2007      BJS             Initial Creation.
* 
*
*/
BEGIN 

DECLARE @curr_mo_start_dt datetime
DECLARE @curr_fiscal_yr int
DECLARE @curr_fiscal_mth int
DECLARE @curr_mth_nbr_of_wks int
DECLARE @next_mth_nbr_of_wks int
DECLARE @found int

SET @curr_mo_start_dt = '1/31/93'
SET @curr_fiscal_yr = 1992
SET @curr_mth_nbr_of_wks = 5
SET @next_mth_nbr_of_wks = 4
SET @found = 0

While @found = 0
	BEGIN
	
	SET @curr_fiscal_mth = 1
	SET @curr_fiscal_yr = @curr_fiscal_yr + 1

	While @curr_fiscal_mth < 13 And @found = 0
		BEGIN
		
		-- Account for 53 week years.
		-- Here we supply the first day of the last fiscal
		-- month of the 53 week year.

		IF DATEDIFF(Day, @curr_mo_start_dt, '12/31/1995') = 0 OR 
		   DATEDIFF(Day, @curr_mo_start_dt, '12/31/2000') = 0 OR 
		   DATEDIFF(Day, @curr_mo_start_dt, '12/31/2006') = 0 OR 
		   DATEDIFF(Day, @curr_mo_start_dt, '12/30/2012') = 0 OR
		   DATEDIFF(Day, @curr_mo_start_dt, '12/31/2017') = 0
			BEGIN
			SET @curr_mth_nbr_of_wks = 6
			END

		IF @curr_mth_nbr_of_wks = 4 And @next_mth_nbr_of_wks = 4
			BEGIN
			SET @curr_mo_start_dt = DATEADD(Day, 35, @curr_mo_start_dt) 
			SET @next_mth_nbr_of_wks = 5
			END
		ELSE IF @curr_mth_nbr_of_wks = 4 And @next_mth_nbr_of_wks = 5
			BEGIN
			SET @curr_mo_start_dt = DATEADD(Day, 28, @curr_mo_start_dt)
			SET @curr_mth_nbr_of_wks = 5
			SET @next_mth_nbr_of_wks = 4
			END
		ELSE IF @curr_mth_nbr_of_wks = 5 And @next_mth_nbr_of_wks = 4
			BEGIN
			SET @curr_mo_start_dt = DATEADD(Day, 28, @curr_mo_start_dt)
			SET @curr_mth_nbr_of_wks = 4
			END
		ELSE IF @curr_mth_nbr_of_wks = 6 And @next_mth_nbr_of_wks = 5
			BEGIN
			SET @curr_mo_start_dt = DATEADD(Day, 35, @curr_mo_start_dt) 
			SET @curr_mth_nbr_of_wks = 5
			SET @next_mth_nbr_of_wks = 4
			END

		IF DATEDIFF(Day, @inDate, @curr_mo_start_dt) > 0
			BEGIN
			SET @found = 1
			END
		ELSE
			BEGIN
			SET @curr_fiscal_mth = @curr_fiscal_mth + 1
			END
		END
	END


-- Return the data
return(@curr_fiscal_yr)
END
GO

GRANT  EXECUTE  ON [dbo].[getFiscalYear]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
