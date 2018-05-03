IF OBJECT_ID('dbo.IsItemOverWeight') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.IsItemOverWeight'
	DROP FUNCTION dbo.IsItemOverWeight
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.IsItemOverWeight') IS NOT NULL
		PRINT '***********Drop of dbo.IsItemOverWeight FAILED.'
END
GO
PRINT 'Creating dbo.IsItemOverWeight'
GO

create function dbo.IsItemOverWeight(@ItemWeight decimal(12,6), @PST_PostalScenario_ID bigint)
returns bit
/*
* PARAMETERS:
*   ItemWeight - weight of each piece
*   PST_PostalScenario_ID - postal scenario
*
* DESCRIPTION:
*	Determines over or under weight based on postal scenario
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
*	1 = Overweight
*	0 = Underweight
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 11/20/2007      JRH             Initial Creation
*
*/
as
begin
	return(
		select top 1
			case pr.PST_PostalClass_ID
				when 1 then /* First Class */
					case
						when @ItemWeight < pw.FirstOverWeightLimit then 0
						else 1
					end
				else /* Standard */
					case
						when @ItemWeight < pw.StandardOverweightLimit then 0
						else 1
					end
			end
		from PST_PostalCategoryScenario_Map srm join PST_PostalCategoryRate_Map pr on srm.PST_PostalCategoryRate_Map_ID = pr.PST_PostalCategoryRate_Map_ID
			join PST_PostalWeights pw on pr.PST_PostalWeights_ID = pw.PST_PostalWeights_ID
		where srm.PST_PostalScenario_ID = @PST_PostalScenario_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[IsItemOverWeight]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO