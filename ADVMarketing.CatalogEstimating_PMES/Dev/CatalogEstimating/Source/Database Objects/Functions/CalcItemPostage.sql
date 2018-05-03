IF OBJECT_ID('dbo.CalcItemPostage') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.CalcItemPostage'
	DROP FUNCTION dbo.CalcItemPostage
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.CalcItemPostage') IS NOT NULL
		PRINT '***********Drop of dbo.CalcItemPostage FAILED.'
END
GO
PRINT 'Creating dbo.CalcItemPostage'
GO

create function dbo.CalcItemPostage(@MailQuantity int, @ItemWeight decimal(12,6), @PST_PostalScenario_ID bigint)
returns money
/*
* PARAMETERS:
*	MailQuantity - total quantity to be mailed
*   ItemWeight - weight of each piece
*   PST_PostalScenario_ID - postal scenario
*
* DESCRIPTION:
*	Calculates the total postage cost for a given quantity, weight and postal scenario
*
* TABLES:
*   Table Name              Access
*   ==========              ======
*   PST_Scenario_Rate_Map   READ
*   PST_PostalRate          READ
*   PST_PostalWeight        READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total postage cost
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/24/2007      BJS             Initial Creation
* 08/30/2007      BJS             Updated Postal table names
*
*/
as
begin
	return(
		select sum(
			case pr.PST_PostalClass_ID
				when 1 then /* First Class */
					case
						when @ItemWeight < pw.FirstOverWeightLimit then @MailQuantity * pr.UnderWeightPieceRate * srm.Percentage
						else (@MailQuantity * pr.OverWeightPieceRate + (@MailQuantity * @ItemWeight) * pr.OverWeightPoundRate) * srm.Percentage
					end
				else /* Standard */
					case
						when @ItemWeight < pw.StandardOverweightLimit then @MailQuantity * pr.UnderWeightPieceRate * srm.Percentage
						else (@MailQuantity * pr.OverWeightPieceRate + (@MailQuantity * @ItemWeight) * pr.OverWeightPoundRate) * srm.Percentage
					end
			end)
		from PST_PostalCategoryScenario_Map srm join PST_PostalCategoryRate_Map pr on srm.PST_PostalCategoryRate_Map_ID = pr.PST_PostalCategoryRate_Map_ID
			join PST_PostalWeights pw on pr.PST_PostalWeights_ID = pw.PST_PostalWeights_ID
		where srm.PST_PostalScenario_ID = @PST_PostalScenario_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[CalcItemPostage]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO