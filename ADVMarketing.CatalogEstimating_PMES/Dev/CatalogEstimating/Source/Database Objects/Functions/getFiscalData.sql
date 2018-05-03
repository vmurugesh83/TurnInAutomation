IF OBJECT_ID('dbo.getFiscalData') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.getFiscalData'
	DROP FUNCTION dbo.getFiscalData
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.getFiscalData') IS NOT NULL
		PRINT '***********Drop of dbo.getFiscalData FAILED.'
END
GO
PRINT 'Creating dbo.getFiscalData'
GO

CREATE FUNCTION dbo.getFiscalData (@inDate datetime)
RETURNS @fiscal_data TABLE (
	  fiscal_yr smallint
	, fiscal_mth_nbr char(2)
	, season_cd char(1)
)
AS  
/*
* PARAMETERS:
* @inDate - Date to process
*
* DESCRIPTION:
*	Determines fiscal year, month, and season for an inputted date
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
*	Table containing Fiscal Year, Fiscal Month and Season ID
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

DECLARE @ret_fiscal_mth char(2)
DECLARE @ret_season_cd char(1)

If @curr_fiscal_mth < 10
	BEGIN
	SET @ret_fiscal_mth = '0' + CONVERT(char(1), @curr_fiscal_mth)
	END
ELSE
	BEGIN
	SET @ret_fiscal_mth = CONVERT(char(2), @curr_fiscal_mth)
	END

If @curr_fiscal_mth < 7
	BEGIN
	SET @ret_season_cd = '1'
	END
Else
	BEGIN
	SET @ret_season_cd = '2'
	END

-- Return the data

INSERT INTO @fiscal_data (
	  fiscal_yr
	, fiscal_mth_nbr
	, season_cd
) VALUES (
    ISNULL(@curr_fiscal_yr, 0)
  , ISNULL(@ret_fiscal_mth, '00')
  , ISNULL(@ret_season_cd, '0')
)
RETURN
END
GO

GRANT  SELECT  ON [dbo].[getFiscalData]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
