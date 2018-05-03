IF OBJECT_ID('dbo.PubInsertDiscounts_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubInsertDiscounts_i'
	DROP PROCEDURE dbo.PubInsertDiscounts_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubInsertDiscounts_i') IS NOT NULL
		PRINT '***********Drop of dbo.PubInsertDiscounts_i FAILED.'
END
GO
PRINT 'Creating dbo.PubInsertDiscounts_i'
GO

create proc dbo.PubInsertDiscounts_i
/*
* PARAMETERS:
*
*
* DESCRIPTION:
*   Inserts a record into the PUB_InsertDiscounts table.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_InsertDiscounts INSERT
*
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
* 05/25/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@PUB_PubRate_ID bigint,
@PUB_InsertDiscount_ID bigint,
@Insert int,
@Discount decimal,
@CreatedBy varchar(50)
as

insert into PUB_InsertDiscounts(PUB_PubRate_ID, PUB_InsertDiscount_ID, [Insert], Discount, CreatedBy, CreatedDate)
values(@PUB_PubRate_ID, @PUB_InsertDiscount_ID, @Insert, @Discount, @CreatedBy, getdate())


GO

GRANT  EXECUTE  ON [dbo].[PubInsertDiscounts_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
