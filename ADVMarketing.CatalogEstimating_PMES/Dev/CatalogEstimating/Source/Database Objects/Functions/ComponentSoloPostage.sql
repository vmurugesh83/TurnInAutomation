IF OBJECT_ID('dbo.ComponentSoloPostage') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.ComponentSoloPostage'
	DROP FUNCTION dbo.ComponentSoloPostage
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.ComponentSoloPostage') IS NOT NULL
		PRINT '***********Drop of dbo.ComponentSoloPostage FAILED.'
END
GO
PRINT 'Creating dbo.ComponentSoloPostage'
GO

create function dbo.ComponentSoloPostage(@EST_Component_ID bigint, @PieceWeight decimal(12,6), @PST_PostalScenario_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Component_ID
*
* DESCRIPTION:
*	Returns the the component's solo mail postage cost.
*
* TABLES:
*   Table Name                    Access
*   ==========                    ======
*   EST_PackageComponentMapping   READ
*   EST_Package                   READ
*
* PROCEDURES CALLED:
*	dbo.PackageWeight
*   dbo.CalcItemPostage
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Component other quantity
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 10/10/2007      BJS             Initial Creation 
* 11/20/2007      JRH             Implement new rules.
*
*/
as
begin

declare @Cost money
declare @ComponentType int
declare @PkgWeight decimal(12,6)

select @Cost = 0

select	@ComponentType = est_componenttype_id
from	dbo.est_component
where	est_component_id = @EST_Component_ID

select @Cost = sum(
		case
			when dbo.PackageWeight(p.EST_Package_ID) = 0 then 0
			when dbo.IsItemOverWeight(dbo.PackageWeight(p.EST_Package_ID), @PST_PostalScenario_ID) = 1
				then dbo.CalcItemPostage(p.SoloQuantity, dbo.PackageWeight(p.EST_Package_ID), @PST_PostalScenario_ID)
					* @PieceWeight / dbo.PackageWeight(p.EST_Package_ID)
			else
				case
					when @ComponentType = 1
						then dbo.CalcItemPostage(p.SoloQuantity, dbo.PackageWeight(p.EST_Package_ID), @PST_PostalScenario_ID)
					else 0
					end
		end)
	from EST_PackageComponentMapping pcm join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
	where pcm.EST_Component_ID = @EST_Component_ID

return(@Cost)

end
GO

GRANT  EXECUTE  ON [dbo].[ComponentSoloPostage]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
