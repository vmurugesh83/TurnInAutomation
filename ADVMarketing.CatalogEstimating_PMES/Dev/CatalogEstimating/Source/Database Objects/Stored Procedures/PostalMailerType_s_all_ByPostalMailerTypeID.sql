IF OBJECT_ID('dbo.PostalMailerType_s_all_ByPostalMailerTypeID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PostalMailerType_s_all_ByPostalMailerTypeID'
	DROP PROCEDURE dbo.PostalMailerType_s_all_ByPostalMailerTypeID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PostalMailerType_s_all_ByPostalMailerTypeID') IS NOT NULL
		PRINT '***********Drop of dbo.PostalMailerType_s_all_ByPostalMailerTypeID FAILED.'
END
GO
PRINT 'Creating dbo.PostalMailerType_s_all_ByPostalMailerTypeID'
GO

create proc dbo.PostalMailerType_s_all_ByPostalMailerTypeID
/*
* PARAMETERS:
* PST_PostalMailerType_ID - Required.  The PostalMailerTypeID.
*
*
* DESCRIPTION:
*		Returns the PST_PostalMailerType record matching PST_PostalMailerType_ID
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PST_PostalMailerType
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
* 05/23/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@PST_PostalMailerType_ID bigint
as

select * from PST_PostalMailerType
where PST_PostalMailerType_ID = @PST_PostalMailerType_ID


GO

GRANT  EXECUTE  ON [dbo].[PostalMailerType_s_all_ByPostalMailerTypeID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
