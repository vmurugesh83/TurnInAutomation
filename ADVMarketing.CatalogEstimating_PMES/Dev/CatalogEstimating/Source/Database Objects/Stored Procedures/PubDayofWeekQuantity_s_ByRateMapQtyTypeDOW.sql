IF OBJECT_ID('dbo.PubDayofWeekQuantity_s_ByRateMapQtyTypeDOW') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubDayofWeekQuantity_s_ByRateMapQtyTypeDOW'
	DROP PROCEDURE dbo.PubDayofWeekQuantity_s_ByRateMapQtyTypeDOW
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubDayofWeekQuantity_s_ByRateMapQtyTypeDOW') IS NOT NULL
		PRINT '***********Drop of dbo.PubDayofWeekQuantity_s_ByRateMapQtyTypeDOW FAILED.'
END
GO
PRINT 'Creating dbo.PubDayofWeekQuantity_s_ByRateMapQtyTypeDOW'
GO

CREATE PROC dbo.PubDayofWeekQuantity_s_ByRateMapQtyTypeDOW
/*
* PARAMETERS:
* RunDate - Required
*
* DESCRIPTION:
*		Returns distribution quantity for a publoc (@PubRateMap_ID),
*			quantity type (@QuantityType), 
*			and insert day of week (@InsertDOW).
*
* TABLES:
*		Table Name					Access
*		==========					======
*		pub_pubquantity             READ
*		pub_dayofweekquantity       READ
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
*	Date            Who             Comments
*	-------------   --------        -------------------------------------------------
*	09/12/2007      JRH             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@PubRateMap_ID	bigint,
@QuantityType	int,
@InsertDOW	int

AS

SELECT
	pq.pub_pubrate_map_id
	, dowq.pub_pubquantitytype_id
	, dowq.insertdow
	, dowq.pub_dayofweekquantity_id
	, dowq.pub_pubquantity_id
	, dowq.quantity
FROM
dbo.pub_pubquantity pq (nolock)
	INNER JOIN dbo.pub_dayofweekquantity dowq (nolock)
		ON pq.pub_pubquantity_id = dowq.pub_pubquantity_id
		AND @QuantityType = dowq.pub_pubquantitytype_id
		AND @InsertDOW = dowq.insertdow

WHERE
	pq.pub_pubrate_map_id = @PubRateMap_ID
GO

GRANT  EXECUTE  ON [dbo].[PubDayofWeekQuantity_s_ByRateMapQtyTypeDOW]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO