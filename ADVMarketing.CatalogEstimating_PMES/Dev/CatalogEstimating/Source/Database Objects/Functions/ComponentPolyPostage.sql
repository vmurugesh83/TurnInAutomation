IF OBJECT_ID('dbo.ComponentPolyPostage') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.ComponentPolyPostage'
	DROP FUNCTION dbo.ComponentPolyPostage
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.ComponentPolyPostage') IS NOT NULL
		PRINT '***********Drop of dbo.ComponentPolyPostage FAILED.'
END
GO
PRINT 'Creating dbo.ComponentPolyPostage'
GO

create function dbo.ComponentPolyPostage(@EST_Component_ID bigint, @PieceWeight decimal(12,6))
returns money
/*
* PARAMETERS:
*	EST_Component_ID - A Component ID
*   PieceWeight      - The Component Weight
*
* DESCRIPTION:
*	Calculates the Component's Polybag Postage Cost.
*
* TABLES:
*   Table Name                    Access
*   ==========                    ======
*   EST_PackageComponentMapping   READ
*
* PROCEDURES CALLED:
*   dbo.PackageWeight
*   dbo.PackagePolyPostage
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Component Polybag Postage.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 10/05/2007      BJS             Initial Creation 
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
			when dbo.IsItemOverWeight(dbo.PolybagWeightIncludeBagWeight(p.EST_Polybag_ID), pb.PST_PostalScenario_ID) = 0
				then case
					when @ComponentType = 1
						then dbo.PackagePolyPostage(p.EST_Package_ID, dbo.PackageWeight(p.EST_Package_ID), p.EST_Polybag_ID)
					else 0
					end
			else
				dbo.PackagePolyPostage(p.EST_Package_ID, dbo.PackageWeight(p.EST_Package_ID), p.EST_Polybag_ID) 
				* @PieceWeight / dbo.PackageWeight(p.EST_Package_ID)
		end)
	from EST_PackageComponentMapping m (nolock)
		inner join EST_PackagePolybag_Map p (nolock)
			on m.EST_Package_ID = p.EST_Package_ID
		inner join EST_Polybag pb (nolock)
			on p.EST_Polybag_ID = pb.EST_Polybag_ID
	where EST_Component_ID = @EST_Component_ID

return(@Cost)

end
GO

GRANT  EXECUTE  ON [dbo].[ComponentPolyPostage]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
