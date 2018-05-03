IF OBJECT_ID('dbo.PubRateMapInsertQuantityByInsertDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubRateMapInsertQuantityByInsertDate'
	DROP FUNCTION dbo.PubRateMapInsertQuantityByInsertDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubRateMapInsertQuantityByInsertDate') IS NOT NULL
		PRINT '***********Drop of dbo.PubRateMapInsertQuantityByInsertDate FAILED.'
END
GO
PRINT 'Creating dbo.PubRateMapInsertQuantityByInsertDate'
GO

CREATE function dbo.PubRateMapInsertQuantityByInsertDate(@InsertDate datetime, @PUB_PubQuantityType_ID int, @PUB_PubRate_Map_ID bigint)
/*
* PARAMETERS:
*	InsertDate
*	PUB_PubQuantityType_ID
*	PUB_PubRate_Map_ID
*
* DESCRIPTION:
*	Returns the insert quantity for a Pub-Location on a given insert date.
*
* TABLES:
*   Table Name                   Access
*   ==========                   ======
*   pub_pubquantity			     READ
*	pub_dayofweekquantity		 READ
*	pub_pubquantitytype			 READ
*
* PROCEDURES CALLED:
*   None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	int - insert quantity
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 09/07/2007      BJS             Initial Creation
*
*/
returns int
as
begin
	return(select top 1 dowqty.Quantity
	from PUB_PubQuantity ppq join PUB_DayOfWeekQuantity dowqty on ppq.PUB_PubQuantity_ID = dowqty.PUB_PubQuantity_ID
		join PUB_PubQuantityType pqt on dowqty.PUB_PubQuantityType_ID = pqt.PUB_PubQuantityType_ID
	where ppq.PUB_PubRate_Map_ID = @PUB_PubRate_Map_ID and dowqty.PUB_PubQuantityType_ID = @PUB_PubQuantityType_ID
		and (pqt.Special = 1 or datepart(dw, @InsertDate) = dowqty.InsertDow)
		and ppq.EffectiveDate <= @InsertDate
	order by EffectiveDate desc)
end
GO

GRANT  EXECUTE  ON [dbo].[PubRateMapInsertQuantityByInsertDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
