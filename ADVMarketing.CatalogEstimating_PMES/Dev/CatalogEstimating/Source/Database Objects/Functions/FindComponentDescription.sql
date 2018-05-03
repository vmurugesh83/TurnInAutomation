IF OBJECT_ID('dbo.FindComponentDescription') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.FindComponentDescription'
	DROP FUNCTION dbo.FindComponentDescription
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.FindComponentDescription') IS NOT NULL
		PRINT '***********Drop of dbo.FindComponentDescription FAILED.'
END
GO
PRINT 'Creating dbo.FindComponentDescription'
GO

CREATE FUNCTION dbo.FindComponentDescription(@AdNumber int, @RunDate datetime, @VendorSupplied int)
RETURNS varchar(35)
/*
* PARAMETERS:
*
* DESCRIPTION:
*	Finds and returns the description of the first component found that matches the criteria.
*
* TABLES:
*   Table Name              Access
*   ==========              ======
*   None
*
* PROCEDURES CALLED:
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Component Description
*
* REVISION HISTORY:
*
* Date		Who	Comments
* ----------	---	-------------------------------------------------
* 11/19/2007	JRH	Initial Creation
*
*/
AS
BEGIN

DECLARE @AdDescription varchar(35)

	SELECT TOP 1
		@AdDescription = AdDesc
	FROM
		(SELECT DISTINCT
			e.est_estimate_id
		FROM
			EST_Estimate e 
				inner join vw_Estimate_ExcludeOldUploads eo 
					on e.EST_Estimate_ID = eo.EST_Estimate_ID
		WHERE
			e.EST_Status_ID = 2
			and e.RunDate = @RunDate) es
		inner join
			(SELECT DISTINCT
				est_component_id
				, est_estimate_id
				, c.description as AdDesc
			FROM
				EST_Component c 
			WHERE
				c.AdNumber = @AdNumber
				and (@VendorSupplied = 1 or (@VendorSupplied = 2 and c.VendorSupplied = 1) or (@VendorSupplied = 3 and c.VendorSupplied = 0))) cs
		on es.est_estimate_id = cs.est_estimate_id
	ORDER BY
		est_component_id


	RETURN(@AdDescription)
END
GO

GRANT EXECUTE ON [dbo].[FindComponentDescription]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
