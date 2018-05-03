IF OBJECT_ID('dbo.getSeasonID_ByDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.getSeasonID_ByDate'
	DROP FUNCTION dbo.getSeasonID_ByDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.getSeasonID_ByDate') IS NOT NULL
		PRINT '***********Drop of dbo.getSeasonID_ByDate FAILED.'
END
GO
PRINT 'Creating dbo.getSeasonID_ByDate'
GO

CREATE FUNCTION dbo.getSeasonID_ByDate (@inDate datetime)
RETURNS int
AS  
/*
* PARAMETERS:
* @inDate - Date to process
*
* DESCRIPTION:
*	Determines the Season for the inputted date.
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
*	curr_fiscal_mth - The Fiscal Month for the inputted date.
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

DECLARE @ret_season_id int

If @curr_fiscal_mth < 7
	SET @ret_season_id = 1 /*Spring*/
Else
	SET @ret_season_id = 2 /*Fall*/

-- Return the data
return(@ret_season_id)
END
GO

GRANT  EXECUTE  ON [dbo].[getSeasonID_ByDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
