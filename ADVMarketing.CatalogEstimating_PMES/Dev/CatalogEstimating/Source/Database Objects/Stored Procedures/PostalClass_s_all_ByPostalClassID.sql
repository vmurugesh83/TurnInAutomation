IF OBJECT_ID('dbo.PostalClass_s_all_ByPostalClassID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PostalClass_s_all_ByPostalClassID'
	DROP PROCEDURE dbo.PostalClass_s_all_ByPostalClassID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PostalClass_s_all_ByPostalClassID') IS NOT NULL
		PRINT '***********Drop of dbo.PostalClass_s_all_ByPostalClassID FAILED.'
END
GO
PRINT 'Creating dbo.PostalClass_s_all_ByPostalClassID'
GO

create proc dbo.PostalClass_s_all_ByPostalClassID
/*
* PARAMETERS:
* PST_PostalClass_ID - Required.  The PostalClassID.
*
*
* DESCRIPTION:
*		Returns the PST_PostalClass record matching PST_PostalClass_ID
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PST_PostalClass
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
@PST_PostalClass_ID bigint
as

select * from PST_PostalClass
where PST_PostalClass_ID = @PST_PostalClass_ID


GO

GRANT  EXECUTE  ON [dbo].[PostalClass_s_all_ByPostalClassID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
