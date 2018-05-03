IF OBJECT_ID('dbo.VndVendor_s_ByDescriptionandVendorType') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.VndVendor_s_ByDescriptionandVendorType'
	DROP PROCEDURE dbo.VndVendor_s_ByDescriptionandVendorType
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.VndVendor_s_ByDescriptionandVendorType') IS NOT NULL
		PRINT '***********Drop of dbo.VndVendor_s_ByDescriptionandVendorType FAILED.'
END
GO
PRINT 'Creating dbo.VndVendor_s_ByDescriptionandVendorType'
GO

create proc dbo.VndVendor_s_ByDescriptionandVendorType
/*
* PARAMETERS:
* Description       - Required
* VND_VendorType_ID - Required
*
*
* DESCRIPTION:
*		Returns a Vendor Record matching the Description and Vendor Type
*
*
* TABLES:
*   Table Name      Access
*   ==========      ======
*   VND_Vendor      READ
*   VND_Printer     READ
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
*	Date            Who             Comments
*	-------------   --------        -------------------------------------------------
*   09/04/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@Description varchar(35),
@VND_VendorType_ID int
as
select v.*
from VND_Vendor v join VND_VendorVendorType_Map vvtm on v.VND_Vendor_ID = vvtm.VND_Vendor_ID
where v.Description = @Description and vvtm.VND_VendorType_ID = @VND_VendorType_ID
GO

GRANT  EXECUTE  ON [dbo].[VndVendor_s_ByDescriptionandVendorType]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
