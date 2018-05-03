IF OBJECT_ID('dbo.CalcGrossInsertCostforPackageandPub') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.CalcGrossInsertCostforPackageandPub'
	DROP FUNCTION dbo.CalcGrossInsertCostforPackageandPub
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.CalcGrossInsertCostforPackageandPub') IS NOT NULL
		PRINT '***********Drop of dbo.CalcGrossInsertCostforPackageandPub FAILED.'
END
GO
PRINT 'Creating dbo.CalcGrossInsertCostforPackageandPub'
GO

CREATE function dbo.CalcGrossInsertCostforPackageandPub(@PUB_PubRate_ID bigint, @InsertDate datetime, @InsertDOW int, @PageCount int,
	@BilledQuantity int, @PackageWeight decimal(12,6), @PackageSize decimal(10,4))
returns money
/*
* PARAMETERS:
*	PUB_PubRateMap_ID - the pub rate map
*   InsertDate        - Insert Date
*   InsertDOW         - Insert DOW
*   PageCount         - Tab Page Count
*   BilledQuantity    - Insert Quantity
*   PackageWeight     - Piece Weight of package being inserted
*   PackageSize       - Size of Host in square inches
*
* DESCRIPTION:
*	Determines the Insertion Cost for a package before the insert discounts are applied.
*
* TABLES:
*   Table Name              Access
*   ==========              ======
*   PUB_DayOfWeekRateTypes  READ
*   PUB_DayOfWeekRates      READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Gross Insertion Cost
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 10/09/2007      BJS             Initial Creation
*
*/
as
begin
	declare @retval money
	declare @RateType int
	select @RateType = PUB_RateType_ID from PUB_PubRate where PUB_PubRate_ID = @PUB_PubRate_ID
	/*You could use a case statement, but the if blocks seem to be easier to read.*/
	if (@RateType = 1) --Tab Rate
		select top 1 @retval = wr.Rate * @BilledQuantity / cast(1000 as decimal)
		from PUB_DayOfWeekRateTypes wrt join PUB_DayOfWeekRates wr on wrt.PUB_DayOfWeekRateTypes_ID = wr.PUB_DayOfWeekRateTypes_ID
		where wrt.PUB_PubRate_ID = @PUB_PubRate_ID and cast(wrt.RateTypeDescription as int) >= @PageCount and wr.InsertDOW = @InsertDOW
		order by wrt.RateTypeDescription
	else if (@RateType = 2) -- Flat
		select @retval = wr.Rate
		from PUB_DayOfWeekRateTypes wrt join PUB_DayOfWeekRates wr on wrt.PUB_DayOfWeekRateTypes_ID = wr.PUB_DayOfWeekRateTypes_ID
		where wrt.PUB_PubRate_ID = @PUB_PubRate_ID and wr.InsertDOW = @InsertDOW
	else if (@RateType = 3) -- CPM
		select top 1 @retval = wr.Rate * @BilledQuantity / cast(1000 as decimal)
		from PUB_DayOfWeekRateTypes wrt join PUB_DayOfWeekRates wr on wrt.PUB_DayOfWeekRateTypes_ID = wr.PUB_DayOfWeekRateTypes_ID
		where wrt.PUB_PubRate_ID = @PUB_PubRate_ID and wrt.RateTypeDescription >= @BilledQuantity and wr.InsertDOW = @InsertDOW
		order by RateTypeDescription
		
	else if (@RateType = 4) -- Cost Per Weight
		select top 1 @retval = wr.Rate * @PackageWeight
		from PUB_DayOfWeekRateTypes wrt join PUB_DayOfWeekRates wr on wrt.PUB_DayOfWeekRateTypes_ID = wr.PUB_DayOfWeekRateTypes_ID
		where wrt.PUB_PubRate_ID = @PUB_PubRate_ID and wrt.RateTypeDescription >= @PackageWeight and wr.InsertDOW = @InsertDOW
		order by wrt.RateTypeDescription
	else if (@RateType = 5) -- Cost Per Size
		select top 1 @retval = wr.Rate * @PackageSize
		from PUB_DayOfWeekRateTypes wrt join PUB_DayOfWeekRates wr on wrt.PUB_DayOfWeekRateTypes_ID = wr.PUB_DayOfWeekRateTypes_ID
		where wrt.PUB_PubRate_ID = @PUB_PubRate_ID and wrt.RateTypeDescription >= @PackageSize and wr.InsertDOW = @InsertDOW
		order by wrt.RateTypeDescription

	return @retval
end
GO

GRANT  EXECUTE  ON [dbo].[CalcGrossInsertCostforPackageandPub]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO