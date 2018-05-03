IF OBJECT_ID('dbo.CalcGrossInsertCostforPackageandPub') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.CalcGrossInsertCostforPackageandPub'
	DROP FUNCTION dbo.CalcGrossInsertCostforPackageandPub
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.CalcGrossInsertCostforPackageandPub') IS NOT NULL
		PRINT '***********Drop of dbo.CalcGrossInsertCostforPackageandPub FAILED.'
END
GO
PRINT 'Creating dbo.CalcGrossInsertCostforPackageandPub'
GO

CREATE function dbo.CalcGrossInsertCostforPackageandPub(@PUB_PubRate_ID bigint, @InsertDate datetime, @InsertDOW int, @PageCount int,
	@BilledQuantity int, @PackageWeight decimal(12,6), @PackageSize decimal(10,4))
returns money
/*
* PARAMETERS:
*	PUB_PubRateMap_ID - the pub rate map
*   InsertDate        - Insert Date
*   InsertDOW         - Insert DOW
*   PageCount         - Tab Page Count
*   BilledQuantity    - Insert Quantity
*   PackageWeight     - Piece Weight of package being inserted
*   PackageSize       - Size of Host in square inches
*
* DESCRIPTION:
*	Determines the Insertion Cost for a package before the insert discounts are applied.
*
* TABLES:
*   Table Name              Access
*   ==========              ======
*   PUB_DayOfWeekRateTypes  READ
*   PUB_DayOfWeekRates      READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Gross Insertion Cost
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 10/09/2007      BJS             Initial Creation
*
*/
as
begin
	declare @retval money
	declare @RateType int
	select @RateType = PUB_RateType_ID from PUB_PubRate where PUB_PubRate_ID = @PUB_PubRate_ID
	/*You could use a case statement, but the if blocks seem to be easier to read.*/
	if (@RateType = 1) --Tab Rate
		select top 1 @retval = wr.Rate * @BilledQuantity / cast(1000 as decimal)
		from PUB_DayOfWeekRateTypes wrt join PUB_DayOfWeekRates wr on wrt.PUB_DayOfWeekRateTypes_ID = wr.PUB_DayOfWeekRateTypes_ID
		where wrt.PUB_PubRate_ID = @PUB_PubRate_ID and cast(wrt.RateTypeDescription as int) >= @PageCount and wr.InsertDOW = @InsertDOW
		order by wrt.RateTypeDescription
	else if (@RateType = 2) -- Flat
		select @retval = wr.Rate
		from PUB_DayOfWeekRateTypes wrt join PUB_DayOfWeekRates wr on wrt.PUB_DayOfWeekRateTypes_ID = wr.PUB_DayOfWeekRateTypes_ID
		where wrt.PUB_PubRate_ID = @PUB_PubRate_ID and wr.InsertDOW = @InsertDOW
	else if (@RateType = 3) -- CPM
		select top 1 @retval = wr.Rate * @BilledQuantity / cast(1000 as decimal)
		from PUB_DayOfWeekRateTypes wrt join PUB_DayOfWeekRates wr on wrt.PUB_DayOfWeekRateTypes_ID = wr.PUB_DayOfWeekRateTypes_ID
		where wrt.PUB_PubRate_ID = @PUB_PubRate_ID and wrt.RateTypeDescription >= @BilledQuantity and wr.InsertDOW = @InsertDOW
		order by RateTypeDescription
		
	else if (@RateType = 4) -- Cost Per Weight
		select top 1 @retval = wr.Rate * @PackageWeight
		from PUB_DayOfWeekRateTypes wrt join PUB_DayOfWeekRates wr on wrt.PUB_DayOfWeekRateTypes_ID = wr.PUB_DayOfWeekRateTypes_ID
		where wrt.PUB_PubRate_ID = @PUB_PubRate_ID and wrt.RateTypeDescription >= @PackageWeight and wr.InsertDOW = @InsertDOW
		order by wrt.RateTypeDescription
	else if (@RateType = 5) -- Cost Per Size
		select top 1 @retval = wr.Rate * @PackageSize
		from PUB_DayOfWeekRateTypes wrt join PUB_DayOfWeekRates wr on wrt.PUB_DayOfWeekRateTypes_ID = wr.PUB_DayOfWeekRateTypes_ID
		where wrt.PUB_PubRate_ID = @PUB_PubRate_ID and wrt.RateTypeDescription >= @PackageSize and wr.InsertDOW = @InsertDOW
		order by wrt.RateTypeDescription

	return @retval
end
GO

GRANT  EXECUTE  ON [dbo].[CalcGrossInsertCostforPackageandPub]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
IF OBJECT_ID('dbo.CalcIssueDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.CalcIssueDate'
	DROP FUNCTION dbo.CalcIssueDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.CalcIssueDate') IS NOT NULL
		PRINT '***********Drop of dbo.CalcIssueDate FAILED.'
END
GO
PRINT 'Creating dbo.CalcIssueDate'
GO

CREATE FUNCTION dbo.CalcIssueDate(@InsertDate datetime, @AMPM bit, @AMEditionCode int, @AMOffset int, @PMEditionCode int, @PMOffset int)
RETURNS datetime
/*
* PARAMETERS:
*	InsertDate
*   AMPM              - 0 = AM, 1 = PM
*   AMEditionCode
*   AMOffset
*   PMEditionCode
*   PMOffset
*
* DESCRIPTION:
*	Determines the Issue Date given the Insert Date and the AM/PM distribution model.
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
*	Issue Date
*
* REVISION HISTORY:
*
* Date		Who	Comments
* ----------	---	-------------------------------------------------
* 10/10/2007	BJS	Initial Creation
* 10/18/2007	JRH	Reversed logic
* 10/19/2007	JRH	PM/AM correction should be previous date.
* 11/05/2007	JRH	PM/AM correction should be previous date for PM-Only edition.
*
*/
AS
BEGIN
	DECLARE @IssueDate datetime
	SELECT @IssueDate = @InsertDate

	DECLARE @Offset int
	SELECT @Offset = 0

	IF @AMPM = 0 
		BEGIN
			IF @AMEditionCode = 4
				SELECT @Offset = @AMOffset
		END
	ELSE
		BEGIN
			IF @AMEditionCode = 2 
				BEGIN
					IF @PMEditionCode = 4
						SELECT @Offset = -1 + @PMOffset
					ELSE
						SELECT @Offset = -1
				END
			ELSE IF @AMEditionCode = 4
				SELECT @Offset = @AMOffset
		END

	SELECT @IssueDate = dateadd(day, @Offset, @InsertDate)

	RETURN(@IssueDate)
END
GO

GRANT EXECUTE ON [dbo].[CalcIssueDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
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
GO
IF OBJECT_ID('dbo.CalcPubQuantityID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.CalcPubQuantityID'
	DROP FUNCTION dbo.CalcPubQuantityID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.CalcPubQuantityID') IS NOT NULL
		PRINT '***********Drop of dbo.CalcPubQuantityID FAILED.'
END
GO
PRINT 'Creating dbo.CalcPubQuantityID'
GO

create function dbo.CalcPubQuantityID(@PubRate_Map_ID bigint, @InsertDate datetime)
returns bigint
/*
* PARAMETERS:
*	PUB_PubRateMap_ID - the pub rate map
*   InsertDate        - Insert Date
*
* DESCRIPTION:
*	Determines the PUB Quantity record for the given pub rate map and insert date
*
* TABLES:
*   Table Name              Access
*   ==========              ======
*   PUB_PubQuantity         READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	PubQuantityID
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 10/10/2007      BJS             Initial Creation
*
*/
as
begin
	return(select top 1 PUB_PubQuantity_ID
		from PUB_PubQuantity
		where PUB_PubRate_Map_ID = @PubRate_Map_ID and EffectiveDate <= @InsertDate
		order by EffectiveDate desc)
end
GO

GRANT  EXECUTE  ON [dbo].[CalcPubQuantityID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
IF OBJECT_ID('dbo.CalcPubRateID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.CalcPubRateID'
	DROP FUNCTION dbo.CalcPubRateID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.CalcPubRateID') IS NOT NULL
		PRINT '***********Drop of dbo.CalcPubRateID FAILED.'
END
GO
PRINT 'Creating dbo.CalcPubRateID'
GO

create function dbo.CalcPubRateID(@PubRate_Map_ID bigint, @InsertDate datetime)
returns bigint
/*
* PARAMETERS:
*	PUB_PubRateMap_ID - the pub rate map
*   InsertDate        - Insert Date
*
* DESCRIPTION:
*	Determines the PUB Rate record for the given pub rate map and insert date
*
* TABLES:
*   Table Name              Access
*   ==========              ======
*   PUB_PubRate             READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	PubRateID
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 10/10/2007      BJS             Initial Creation
*
*/
as
begin
	declare @retval bigint
	declare @active bit

	select top 1 @active = Active
	from PUB_PubRate_Map_Activate
	where PUB_PubRate_Map_ID = @PubRate_Map_ID and EffectiveDate <= @InsertDate
	if (@active is null or @active = 1)
		begin
			select top 1 @retval = PUB_PubRate_ID
			from PUB_PubRate
			where PUB_PubRate_Map_ID = @PubRate_Map_ID and EffectiveDate <= @InsertDate
			order by EffectiveDate desc
		end
	else begin
		set @retval = null
	end
	return(@retval)
end
GO

GRANT  EXECUTE  ON [dbo].[CalcPubRateID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.ComponentInsertQuantity') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.ComponentInsertQuantity'
	DROP FUNCTION dbo.ComponentInsertQuantity
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.ComponentInsertQuantity') IS NOT NULL
		PRINT '***********Drop of dbo.ComponentInsertQuantity FAILED.'
END
GO
PRINT 'Creating dbo.ComponentInsertQuantity'
GO

CREATE function dbo.ComponentInsertQuantity(@EST_Component_ID bigint)
returns int
/*
* PARAMETERS:
*	EST_Component_ID - The Component ID
*
* DESCRIPTION:
*	Returns the total insert quantity for the specified component
*
* TABLES:
*   Table Name                   Access
*   ==========                   ======
*   EST_PackageComponentMapping  READ
*   PUB_PubGroup                 READ
*   PUB_PubPubGroup              READ
*   PUB_PubRate_Map              READ
*   PUB_PubQuantity              READ
*   PUB_DayOfWeekQuantity        READ
*   EST_PubIssueDates            READ
*   IsPubRateMapActive           READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total insert quantity for the specified component.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/23/2007      BJS             Initial Creation
* 09/13/2007      BJS             Updated reference to EST_PubInsertDates to EST_PubIssueDates
*                                 Removed join to PUB_PubRate_Map
*/
as
begin
	return(select sum(dowqty.Quantity)
	from EST_PackageComponentMapping pcm join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
		join PUB_PubGroup pg on p.PUB_PubGroup_ID = pg.PUB_PubGroup_ID
		join PUB_PubPubGroup_Map pppgm on pg.PUB_PubGroup_ID = pppgm.PUB_PubGroup_ID
		join EST_PubIssueDates pid on p.EST_Estimate_ID = pid.EST_Estimate_ID and pppgm.PUB_PubRate_Map_ID = pid.PUB_PubRate_Map_ID
		--join PUB_PubRate_Map pprm on pppgm.PUB_PubRate_Map_ID = pprm.PUB_PubRate_Map_ID
		join PUB_PubQuantity ppq on pppgm.PUB_PubRate_Map_ID = ppq.PUB_PubRate_Map_ID
			and ppq.PUB_PubQuantity_ID = dbo.CalcPubQuantityID(pppgm.PUB_PubRate_Map_ID, pid.IssueDate)
		join PUB_DayOfWeekQuantity dowqty on ppq.PUB_PubQuantity_ID = dowqty.PUB_PubQuantity_ID
			and p.PUB_PubQuantityType_ID = dowqty.PUB_PubQuantityType_ID
	where pcm.EST_Component_ID = @EST_Component_ID and dbo.IsPubRateMapActive(pppgm.PUB_PubRate_Map_ID, pid.IssueDate) = 1
		and (/*holidays*/ p.PUB_PubQuantityType_ID > 3 or /*fullrun / contract send*/ pid.IssueDOW = dowqty.InsertDow))
end
GO

GRANT  EXECUTE  ON [dbo].[ComponentInsertQuantity]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.ComponentMediaQuantity') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.ComponentMediaQuantity'
	DROP FUNCTION dbo.ComponentMediaQuantity
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.ComponentMediaQuantity') IS NOT NULL
		PRINT '***********Drop of dbo.ComponentMediaQuantity FAILED.'
END
GO
PRINT 'Creating dbo.ComponentMediaQuantity'
GO

create function dbo.ComponentMediaQuantity(@EST_Component_ID bigint)
returns int
/*
* PARAMETERS:
*	EST_Component_ID - The Component ID
*
* DESCRIPTION:
*	Returns the total media quantity for the specified component
*
* TABLES:
*   Table Name                   Access
*   ==========                   ======
*
* PROCEDURES CALLED:
*   dbo.ComponentInsertQuantity
*   dbo.ComponentSoloMailQuantity
*   dbo.ComponentPolyBagQuantity
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total media quantity for the specified component.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/23/2007      BJS             Initial Creation 
*
*/
as
begin
	return isnull(dbo.ComponentInsertQuantity(@EST_Component_ID), 0)
		+ isnull(dbo.ComponentSoloMailQuantity(@EST_Component_ID), 0)
		+ isnull(dbo.ComponentPolyBagQuantity(@EST_Component_ID), 0)
end
GO

GRANT  EXECUTE  ON [dbo].[ComponentMediaQuantity]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.ComponentOtherQuantity') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.ComponentOtherQuantity'
	DROP FUNCTION dbo.ComponentOtherQuantity
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.ComponentOtherQuantity') IS NOT NULL
		PRINT '***********Drop of dbo.ComponentOtherQuantity FAILED.'
END
GO
PRINT 'Creating dbo.ComponentOtherQuantity'
GO

/* Component Other (sometimes referred to as Sample) Quantity */
create function dbo.ComponentOtherQuantity(@EST_Component_ID bigint)
returns int
/*
* PARAMETERS:
*	EST_Component_ID
*
* DESCRIPTION:
*	Returns the other quantity of a component.
*
* TABLES:
*   Table Name                    Access
*   ==========                    ======
*   EST_PackageComponentMapping   READ
*   EST_Package                   READ
*
* PROCEDURES CALLED:
*	None
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
*
*/
as
begin
	return(select sum(p.OtherQuantity)
	from EST_PackageComponentMapping pcm join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
	where pcm.EST_Component_ID = @EST_Component_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[ComponentOtherQuantity]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.ComponentPolybagInkjetMakereadyCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.ComponentPolybagInkjetMakereadyCost'
	DROP FUNCTION dbo.ComponentPolybagInkjetMakereadyCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.ComponentPolybagInkjetMakereadyCost') IS NOT NULL
		PRINT '***********Drop of dbo.ComponentPolybagInkjetMakereadyCost FAILED.'
END
GO
PRINT 'Creating dbo.ComponentPolybagInkjetMakereadyCost'
GO

CREATE function dbo.ComponentPolybagInkjetMakereadyCost(@EST_Component_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Component_ID - A Host Component ID
*
* DESCRIPTION:
*	Calculates the Component's portion of the polybag inkjet makeready cost.
*   Totals each of the costs and returns it as the Total Component Polybag inkjet makeready cost.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_PackageComponentMapping
*
* PROCEDURES CALLED:
*	  dbo.PolybagInkjetMakereadyCost
*   dbo.PackageWeight
*   dbo.PolyBagWeight
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Component Polybag inkjet makeready cost.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/18/2007      BJS             Initial Creation 
*
*/
as
begin
	return(select sum(dbo.PolybagInkjetMakereadyCost(pb.EST_Polybag_ID) * isnull(ppm.DistributionPct, dbo.PackageWeight(p.EST_Package_ID) / dbo.PolyBagWeight(pb.EST_Polybag_ID)))
		from EST_Component c join EST_PackageComponentMapping pcm on c.EST_Component_ID = pcm.EST_Component_ID
			join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
			join EST_PackagePolyBag_Map ppm on p.EST_Package_ID = ppm.EST_Package_ID
			join EST_Polybag pb on ppm.EST_Polybag_ID = pb.EST_Polybag_ID
		where c.EST_Component_ID = @EST_Component_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[ComponentPolybagInkjetMakereadyCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.ComponentPolybagMailHouseAdminFee') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.ComponentPolybagMailHouseAdminFee'
	DROP FUNCTION dbo.ComponentPolybagMailHouseAdminFee
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.ComponentPolybagMailHouseAdminFee') IS NOT NULL
		PRINT '***********Drop of dbo.ComponentPolybagMailHouseAdminFee FAILED.'
END
GO
PRINT 'Creating dbo.ComponentPolybagMailHouseAdminFee'
GO

create function dbo.ComponentPolybagMailHouseAdminFee(@EST_Component_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Component_ID - A Host Component ID
*
* DESCRIPTION:
*	Calculates the Component's portion of the polybag mailhouse admin fee.
* Totals each of the costs and returns it as the Total Component Polybag admin fee.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_PackageComponentMapping
*
* PROCEDURES CALLED:
*	dbo.PolybagMailHouseAdminFee
*   dbo.PackageWeight
*   dbo.PolyBagWeight
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Component Polybag mailhouse admin fee cost.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/12/2007      BJS             Initial Creation 
* 08/16/2007      TJU             Corrected call to table join EST_Polybag pb (not EST_Polybag_ID)
*
*/
begin
	return(select sum(dbo.PolybagMailHouseAdminFee(pb.EST_Polybag_ID) * isnull(ppm.DistributionPct, dbo.PackageWeight(p.EST_Package_ID) / dbo.PolyBagWeight(pb.EST_Polybag_ID)))
	from EST_PackageComponentMapping pcm join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
		join EST_PackagePolybag_Map ppm on p.EST_Package_ID = ppm.EST_Package_ID
		join EST_Polybag pb on ppm.EST_Polybag_ID = pb.EST_Polybag_ID
	where pcm.EST_Component_ID = @EST_Component_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[ComponentPolybagMailHouseAdminFee]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.ComponentPolybagMailHouseGlueTackCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.ComponentPolybagMailHouseGlueTackCost'
	DROP FUNCTION dbo.ComponentPolybagMailHouseGlueTackCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.ComponentPolybagMailHouseGlueTackCost') IS NOT NULL
		PRINT '***********Drop of dbo.ComponentPolybagMailHouseGlueTackCost FAILED.'
END
GO
PRINT 'Creating dbo.ComponentPolybagMailHouseGlueTackCost'
GO

create function dbo.ComponentPolybagMailHouseGlueTackCost(@EST_Component_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Component_ID - A Host Component ID
*
* DESCRIPTION:
*	Calculates the Component's portion of the polybag mailhouse glue tack cost.
* Totals each of the costs and returns it as the Total Component Polybag glue tack cost.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_PackageComponentMapping
*
* PROCEDURES CALLED:
*	dbo.PolybagMailHouseGlueTackCost
*   dbo.PackageWeight
*   dbo.PolyBagWeight
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Component Polybag mailhouse gluetack cost.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/12/2007      BJS             Initial Creation 
* 08/16/2007      TJU             Corrected call to table join EST_Polybag pb (not EST_Polybag_ID)
* 10/15/2007      BJS             Removed multiply by quantity, it is handled in the PolybagMailHouseGlueTackCost function
*/
begin
	return(select sum(dbo.PolybagMailHouseGlueTackCost(pb.EST_Polybag_ID) * isnull(ppm.DistributionPct, dbo.PackageWeight(p.EST_Package_ID) / dbo.PolyBagWeight(pb.EST_Polybag_ID)))
	from EST_PackageComponentMapping pcm join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
		join EST_PackagePolybag_Map ppm on p.EST_Package_ID = ppm.EST_Package_ID
		join EST_Polybag pb on ppm.EST_Polybag_ID = pb.EST_Polybag_ID
	where pcm.EST_Component_ID = @EST_Component_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[ComponentPolybagMailHouseGlueTackCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.ComponentPolybagMailHouseLetterInsertionCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.ComponentPolybagMailHouseLetterInsertionCost'
	DROP FUNCTION dbo.ComponentPolybagMailHouseLetterInsertionCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.ComponentPolybagMailHouseLetterInsertionCost') IS NOT NULL
		PRINT '***********Drop of dbo.ComponentPolybagMailHouseLetterInsertionCost FAILED.'
END
GO
PRINT 'Creating dbo.ComponentPolybagMailHouseLetterInsertionCost'
GO

CREATE function dbo.ComponentPolybagMailHouseLetterInsertionCost(@EST_Component_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Component_ID - A Host Component ID
*
* DESCRIPTION:
*	Calculates the Component's portion of the polybag LetterInsertion cost.
*   Totals each of the costs and returns it as the Total Component Polybag LetterInsertion cost.
*
* TABLES:
*   Table Name                   Access
*   ==========                   ======
*   EST_PackageComponentMapping  READ
*   EST_Component                READ
*   EST_Package                  READ
*   EST_PackagePolybag_Map       READ
*   EST_Polybag                  READ
*
* PROCEDURES CALLED:
*	dbo.PolybagMailHouseLetterInsertionCost
*   dbo.PackageWeight
*   dbo.PolybagWeight
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Component Polybag LetterInsertion cost.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/12/2007      BJS             Initial Creation 
* 10/15/2007      BJS             Removed multiply by quantity, it is already handled in the PolybagMailHouseLetterInsertionCost function
*
*/
as
begin
	return(select sum(dbo.PolybagMailHouseLetterInsertionCost(pb.EST_Polybag_ID) * isnull(ppm.DistributionPct, dbo.PackageWeight(p.EST_Package_ID) / dbo.PolybagWeight(pb.EST_Polybag_ID)))
		from EST_Component c join EST_PackageComponentMapping pcm on c.EST_Component_ID = pcm.EST_Component_ID
			join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
			join EST_PackagePolybag_Map ppm on p.EST_Package_ID = ppm.EST_Package_ID
			join EST_Polybag pb on ppm.EST_Polybag_ID = pb.EST_Polybag_ID
		where c.EST_Component_ID = @EST_Component_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[ComponentPolybagMailHouseLetterInsertionCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.ComponentPolybagMailHouseOtherDirectMailHandlingCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.ComponentPolybagMailHouseOtherDirectMailHandlingCost'
	DROP FUNCTION dbo.ComponentPolybagMailHouseOtherDirectMailHandlingCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.ComponentPolybagMailHouseOtherDirectMailHandlingCost') IS NOT NULL
		PRINT '***********Drop of dbo.ComponentPolybagMailHouseOtherDirectMailHandlingCost FAILED.'
END
GO
PRINT 'Creating dbo.ComponentPolybagMailHouseOtherDirectMailHandlingCost'
GO

CREATE function dbo.ComponentPolybagMailHouseOtherDirectMailHandlingCost(@EST_Component_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Component_ID - A Host Component ID
*
* DESCRIPTION:
*	Calculates the Component's portion of the polybag OtherDirectMailHandling cost.
*   Totals each of the costs and returns it as the Total Component Polybag OtherDirectMailHandling cost.
*
* TABLES:
*   Table Name                   Access
*   ==========                   ======
*   EST_Component                READ
*   EST_PackageComponentMapping  READ
*   EST_Package                  READ
*   EST_PackagePolybag_Map       READ
*   EST_Polybag                  READ
*
* PROCEDURES CALLED:
*	dbo.PolybagMailHouseOtherDirectMailHandlingCost
*   dbo.PackageWeight
*   dbo.PolybagWeight
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Component Polybag OtherDirectMailHandling cost.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/12/2007      BJS             Initial Creation
* 10/15/2007      BJS             Removed multiply by quantity
*
*/
as
begin
	return(select sum(dbo.PolybagMailHouseOtherDirectMailHandlingCost(pb.EST_Polybag_ID) * isnull(ppm.DistributionPct, dbo.PackageWeight(p.EST_Package_ID) / dbo.PolybagWeight(pb.EST_Polybag_ID)))
		from EST_Component c join EST_PackageComponentMapping pcm on c.EST_Component_ID = pcm.EST_Component_ID
			join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
			join EST_PackagePolybag_Map ppm on p.EST_Package_ID = ppm.EST_Package_ID
			join EST_Polybag pb on ppm.EST_Polybag_ID = pb.EST_Polybag_ID
		where c.EST_Component_ID = @EST_Component_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[ComponentPolybagMailHouseOtherDirectMailHandlingCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO


GO
IF OBJECT_ID('dbo.ComponentPolybagMailHouseTabbingCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.ComponentPolybagMailHouseTabbingCost'
	DROP FUNCTION dbo.ComponentPolybagMailHouseTabbingCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.ComponentPolybagMailHouseTabbingCost') IS NOT NULL
		PRINT '***********Drop of dbo.ComponentPolybagMailHouseTabbingCost FAILED.'
END
GO
PRINT 'Creating dbo.ComponentPolybagMailHouseTabbingCost'
GO

CREATE function dbo.ComponentPolybagMailHouseTabbingCost(@EST_Component_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Component_ID - A Host Component ID
*
* DESCRIPTION:
*	Calculates the Component's portion of the polybag tabbing cost.
*   Totals each of the costs and returns it as the Total Component Polybag tabbing cost.
*
* TABLES:
*   Table Name                   Access
*   ==========                   ======
*   EST_Component                READ
*   EST_PackageComponentMapping  READ
*   EST_Package                  READ
*   EST_PackagePolybag_Map       READ
*   EST_Polybag                  READ
*
* PROCEDURES CALLED:
*	dbo.PolybagMailHouseTabbingCost
*   dbo.PackageWeight
*   dbo.PolybagWeight
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Component Polybag Tabbing cost.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/12/2007      BJS             Initial Creation 
* 10/15/2007      BJS             Removed multiply by quantity, it is already handled in the PolybagMailHouseTabbingCost function
*
*/
as
begin
	return(select sum(dbo.PolybagMailHouseTabbingCost(pb.EST_Polybag_ID) * isnull(ppm.DistributionPct, dbo.PackageWeight(p.EST_Package_ID) / dbo.PolybagWeight(pb.EST_Polybag_ID)))
		from EST_Component c join EST_PackageComponentMapping pcm on c.EST_Component_ID = pcm.EST_Component_ID
			join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
			join EST_PackagePolybag_Map ppm on p.EST_Package_ID = ppm.EST_Package_ID
			join EST_Polybag pb on ppm.EST_Polybag_ID = pb.EST_Polybag_ID
		where c.EST_Component_ID = @EST_Component_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[ComponentPolybagMailHouseTabbingCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.ComponentPolybagMailHouseTimeValueSlipsCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.ComponentPolybagMailHouseTimeValueSlipsCost'
	DROP FUNCTION dbo.ComponentPolybagMailHouseTimeValueSlipsCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.ComponentPolybagMailHouseTimeValueSlipsCost') IS NOT NULL
		PRINT '***********Drop of dbo.ComponentPolybagMailHouseTimeValueSlipsCost FAILED.'
END
GO
PRINT 'Creating dbo.ComponentPolybagMailHouseTimeValueSlipsCost'
GO

CREATE function dbo.ComponentPolybagMailHouseTimeValueSlipsCost(@EST_Component_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Component_ID - A Host Component ID
*
* DESCRIPTION:
*	Calculates the Component's portion of the polybag time value slips cost.
*   Totals each of the costs and returns it as the Total Component Polybag Time Value Slips cost.
*
* TABLES:
*   Table Name                   Access
*   ==========                   ======
*   EST_Component                READ
*   EST_PackageComponentMapping  READ
*   EST_Package                  READ
*   EST_PackagePolybag_Map       READ
*   EST_Polybag                  READ
*
* PROCEDURES CALLED:
*	dbo.PolybagMailHouseTimeValueSlipsCost
*   dbo.PackageWeight
*   dbo.PolybagWeight
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Component Polybag Time Value Slips cost.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/12/2007      BJS             Initial Creation 
* 10/15/2007      BJS             Removed multiply by quantity, already handled in the PolybagMailHouseTimeValueSlipsCost function
*
*/
as
begin
	return(select sum(dbo.PolybagMailHouseTimeValueSlipsCost(pb.EST_Polybag_ID) * isnull(ppm.DistributionPct, dbo.PackageWeight(p.EST_Package_ID) / dbo.PolybagWeight(pb.EST_Polybag_ID)))
		from EST_Component c join EST_PackageComponentMapping pcm on c.EST_Component_ID = pcm.EST_Component_ID
			join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
			join EST_PackagePolybag_Map ppm on p.EST_Package_ID = ppm.EST_Package_ID
			join EST_Polybag pb on ppm.EST_Polybag_ID = pb.EST_Polybag_ID
		where c.EST_Component_ID = @EST_Component_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[ComponentPolybagMailHouseTimeValueSlipsCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.ComponentPolybagMailListCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.ComponentPolybagMailListCost'
	DROP FUNCTION dbo.ComponentPolybagMailListCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.ComponentPolybagMailListCost') IS NOT NULL
		PRINT '***********Drop of dbo.ComponentPolybagMailListCost FAILED.'
END
GO
PRINT 'Creating dbo.ComponentPolybagMailListCost'
GO

CREATE function dbo.ComponentPolybagMailListCost(@EST_Component_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Component_ID
*
* DESCRIPTION:
*	Calculates the Component's portion of the polybag mail list cost.
* Totals each of the costs and returns it as the Total Component Polybag Mail List cost.
*
* TABLES:
*   Table Name                   Access
*   ==========                   ======
*   EST_Component                READ
*   EST_PackageComponentMapping  READ
*   EST_Package                  READ
*   EST_PackagePolybag_Map       READ
*   EST_Polybag                  READ
*
* PROCEDURES CALLED:
*   dbo.PolybagMailListCost
*	dbo.PackageWeight
*   dbo.PolybagWeight
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Component Polybag Mail List cost.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/09/2007      BJS             Initial Creation 
* 10/16/2007      BJS             Added Reference to PolybagMailList cost, removed blended mail list rate parameter
*
*/
as
begin
	return(select sum(dbo.PolybagMailListCost(pb.EST_Polybag_ID) * isnull(ppm.DistributionPct, dbo.PackageWeight(p.EST_Package_ID) / dbo.PolybagWeight(ppm.EST_PolyBag_ID)))
		from EST_Component c join EST_PackageComponentMapping pcm on c.EST_Component_ID = pcm.EST_Component_ID
			join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
			join EST_PackagePolybag_Map ppm on p.EST_Package_ID = ppm.EST_Package_ID
			join EST_Polybag pb on ppm.EST_Polybag_ID = pb.EST_Polybag_ID
		where c.EST_Component_ID = @EST_Component_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[ComponentPolybagMailListCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.ComponentPolybagMailTrackingCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.ComponentPolybagMailTrackingCost'
	DROP FUNCTION dbo.ComponentPolybagMailTrackingCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.ComponentPolybagMailTrackingCost') IS NOT NULL
		PRINT '***********Drop of dbo.ComponentPolybagMailTrackingCost FAILED.'
END
GO
PRINT 'Creating dbo.ComponentPolybagMailTrackingCost'
GO

CREATE function dbo.ComponentPolybagMailTrackingCost(@EST_Component_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Component_ID - A Host Component ID
*
* DESCRIPTION:
*	Calculates the Component's portion of the polybag mail tracking cost.
*   Totals each of the costs and returns it as the Total Component Polybag mail tracking cost.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_PackageComponentMapping
*
* PROCEDURES CALLED:
*	dbo.PolybagMailTrackingCost
*   dbo.PackageWeight
*   dbo.PolybagWeight
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Component Polybag mail tracking cost.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/12/2007      BJS             Initial Creation
* 10/15/2007      BJS             Removed multiply by quantity, it is already handled in the PolybagMailTrackingCost function
*
*/
as
begin
	return(select sum(dbo.PolybagMailTrackingCost(pb.EST_Polybag_ID) * isnull(ppm.DistributionPct, dbo.PackageWeight(p.EST_Package_ID) / dbo.PolybagWeight(pb.EST_Polybag_ID)))
		from EST_Component c join EST_PackageComponentMapping pcm on c.EST_Component_ID = pcm.EST_Component_ID
			join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
			join EST_PackagePolybag_Map ppm on p.EST_Package_ID = ppm.EST_Package_ID
			join EST_Polybag pb on ppm.EST_Polybag_ID = pb.EST_Polybag_ID
		where c.EST_Component_ID = @EST_Component_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[ComponentPolybagMailTrackingCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.ComponentPolybagOtherFreightCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.ComponentPolybagOtherFreightCost'
	DROP FUNCTION dbo.ComponentPolybagOtherFreightCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.ComponentPolybagOtherFreightCost') IS NOT NULL
		PRINT '***********Drop of dbo.ComponentPolybagOtherFreightCost FAILED.'
END
GO
PRINT 'Creating dbo.ComponentPolybagOtherFreightCost'
GO

CREATE function dbo.ComponentPolybagOtherFreightCost(@EST_Component_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Component_ID - A Host Component ID
*
* DESCRIPTION:
*	Calculates the Component's portion of the polybag other freight cost.
*   Totals each of the costs and returns it as the Total Component Polybag other freight cost.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_PackageComponentMapping
*
* PROCEDURES CALLED:
*	dbo.PolybagOtherFreightCost
*   dbo.PackageWeight
*   dbo.PolyBagWeight
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Component Polybag other freight cost.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/18/2007      BJS             Initial Creation 
*
*/
as
begin
	return(select sum(dbo.PolybagOtherFreightCost(pb.EST_Polybag_ID) * pb.Quantity / cast(1000 as decimal)
			* isnull(ppm.DistributionPct, dbo.PackageWeight(p.EST_Package_ID) / dbo.PolyBagWeight(pb.EST_Polybag_ID)))
		from EST_Component c join EST_PackageComponentMapping pcm on c.EST_Component_ID = pcm.EST_Component_ID
			join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
			join EST_PackagePolyBag_Map ppm on p.EST_Package_ID = ppm.EST_Package_ID
			join EST_Polybag pb on ppm.EST_Polybag_ID = pb.EST_Polybag_ID
		where c.EST_Component_ID = @EST_Component_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[ComponentPolybagOtherFreightCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.ComponentPolybagPostalDropCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.ComponentPolybagPostalDropCost'
	DROP FUNCTION dbo.ComponentPolybagPostalDropCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.ComponentPolybagPostalDropCost') IS NOT NULL
		PRINT '***********Drop of dbo.ComponentPolybagPostalDropCost FAILED.'
END
GO
PRINT 'Creating dbo.ComponentPolybagPostalDropCost'
GO

CREATE function dbo.ComponentPolybagPostalDropCost(@EST_Component_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Component_ID - A Host Component ID
*
* DESCRIPTION:
*	Calculates the Component's portion of the polybag postal drop cost.
*   Totals each of the costs and returns it as the Total Component Polybag postal drop cost.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_PackageComponentMapping
*
* PROCEDURES CALLED:
*	  dbo.PolybagPostalDropCost
*   dbo.PackageWeight
*   dbo.PolyBagWeight
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Component Polybag postal drop cost.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/18/2007      BJS             Initial Creation 
*
*/
as
begin
	return(select sum(dbo.PolybagPostalDropCost(pb.EST_Polybag_ID) * isnull(ppm.DistributionPct, dbo.PackageWeight(p.EST_Package_ID) / dbo.PolyBagWeight(pb.EST_Polybag_ID)))
		from EST_Component c join EST_PackageComponentMapping pcm on c.EST_Component_ID = pcm.EST_Component_ID
			join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
			join EST_PackagePolyBag_Map ppm on p.EST_Package_ID = ppm.EST_Package_ID
			join EST_Polybag pb on ppm.EST_Polybag_ID = pb.EST_Polybag_ID
		where c.EST_Component_ID = @EST_Component_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[ComponentPolybagPostalDropCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.ComponentPolybagQuantity') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.ComponentPolybagQuantity'
	DROP FUNCTION dbo.ComponentPolybagQuantity
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.ComponentPolybagQuantity') IS NOT NULL
		PRINT '***********Drop of dbo.ComponentPolybagQuantity FAILED.'
END
GO
PRINT 'Creating dbo.ComponentPolybagQuantity'
GO

/* Component PolyBag Quantity */
create function dbo.ComponentPolybagQuantity(@EST_Component_ID bigint)
/*
* PARAMETERS:
*	EST_Component_ID - A Component ID
*
* DESCRIPTION:
*	Calculates the Component's polybag quantity.
*
* TABLES:
*   Table Name                    Access
*   ==========                    ======
*   EST_PackageComponentMapping   READ
*   EST_PackagePolybag_Map        READ
*   EST_Polybag                   READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Component Polybag Quantity.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 10/05/2007      BJS             Initial Creation 
*
*/
returns int
as
begin
	return(select sum(pb.Quantity)
	from EST_PackageComponentMapping pcm join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
		join EST_PackagePolybag_Map ppbm on p.EST_Package_ID = ppbm.EST_Package_ID
		join EST_Polybag pb on ppbm.EST_PolyBag_ID = pb.EST_PolyBag_ID
	where pcm.EST_Component_ID = @EST_Component_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[ComponentPolybagQuantity]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
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

GO
IF OBJECT_ID('dbo.ComponentSampleQuantity') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.ComponentSampleQuantity'
	DROP FUNCTION dbo.ComponentSampleQuantity
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.ComponentSampleQuantity') IS NOT NULL
		PRINT '***********Drop of dbo.ComponentSampleQuantity FAILED.'
END
GO
PRINT 'Creating dbo.ComponentSampleQuantity'
GO

/* Component Sample Quantity */
create function dbo.ComponentSampleQuantity(@EST_Component_ID bigint)
returns int
/*
* PARAMETERS:
*	EST_Component_ID
*
* DESCRIPTION:
*	Returns the sample quantity of a component.
*
* TABLES:
*   Table Name                    Access
*   ==========                    ======
*   EST_Component                 READ
*   EST_Samples                   READ
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Component sample quantity
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 10/10/2007      BJS             Initial Creation 
* 11/26/2007      JRH             For Host components only
*
*/
as
begin
	return(select sum(s.Quantity)
	from EST_Component c join EST_Samples s on c.EST_Estimate_ID = s.EST_Estimate_ID
	where c.EST_Component_ID = @EST_Component_ID
		and c.EST_ComponentType_ID = 1)
end
GO

GRANT  EXECUTE  ON [dbo].[ComponentSampleQuantity]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.ComponentSoloMailQuantity') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.ComponentSoloMailQuantity'
	DROP FUNCTION dbo.ComponentSoloMailQuantity
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.ComponentSoloMailQuantity') IS NOT NULL
		PRINT '***********Drop of dbo.ComponentSoloMailQuantity FAILED.'
END
GO
PRINT 'Creating dbo.ComponentSoloMailQuantity'
GO

/* Component Solo Mail Quantity */
create function dbo.ComponentSoloMailQuantity(@EST_Component_ID bigint)
returns int
/*
* PARAMETERS:
*	EST_Component_ID
*
* DESCRIPTION:
*	Returns the solo quantity of a component.
*
* TABLES:
*   Table Name                    Access
*   ==========                    ======
*   EST_PackageComponentMapping   READ
*   EST_Package                   READ
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Component solo mail quantity
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 10/10/2007      BJS             Initial Creation 
*
*/
as
begin
	return(select sum(p.SoloQuantity)
	from EST_PackageComponentMapping pcm join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
	where pcm.EST_Component_ID = @EST_Component_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[ComponentSoloMailQuantity]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
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

GO
IF OBJECT_ID('dbo.ComponentWeight') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.ComponentWeight'
	DROP FUNCTION dbo.ComponentWeight
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.ComponentWeight') IS NOT NULL
		PRINT '***********Drop of dbo.ComponentWeight FAILED.'
END
GO
PRINT 'Creating dbo.ComponentWeight'
GO

create function dbo.ComponentWeight(@EST_Component_ID bigint)
returns decimal(12,6)
/*
* PARAMETERS:
*	EST_Component_ID - A Component ID
*
* DESCRIPTION:
*	Calculates the Component's Piece Weight
*
* TABLES:
*   Table Name                    Access
*   ==========                    ======
*   EST_Component                 READ
*   PPR_PaperWeight               READ
*
* PROCEDURES CALLED:
*   None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Component Piece Weight
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 10/05/2007      BJS             Initial Creation 
*
*/
as
begin
	return(select c.Width * c.Height / cast(950000 as decimal) * c.PageCount * pw.Weight * 1.03
	from EST_Component c join PPR_PaperWeight pw on c.PaperWeight_ID = pw.PPR_PaperWeight_ID
	where c.EST_Component_ID = @EST_Component_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[ComponentWeight]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.EstimateDirectMailQuantity') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstimateDirectMailQuantity'
	DROP FUNCTION dbo.EstimateDirectMailQuantity
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstimateDirectMailQuantity') IS NOT NULL
		PRINT '***********Drop of dbo.EstimateDirectMailQuantity FAILED.'
END
GO
PRINT 'Creating dbo.EstimateDirectMailQuantity'
GO

create function dbo.EstimateDirectMailQuantity(@EST_Estimate_ID bigint)
returns int
/*
* PARAMETERS:
*	EST_Estimate_ID
*
* DESCRIPTION:
*	Returns the total mail quantity for the estimate.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Package
*   EST_PackagePolyBag_Map
*   EST_PolyBag
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Mail Quantity for Estimate (Solo + Poly).
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 06/21/2007      BJS             Initial Creation 
*
*/
as
begin

declare @SoloMailQuantity int
declare @PolyBagQuantity int

select @SoloMailQuantity = sum(SoloQuantity)
from EST_Package
where EST_Estimate_ID = @EST_Estimate_ID

select @PolyBagQuantity = sum(pb.Quantity)
from EST_Package p join EST_PackagePolyBag_Map ppm on p.EST_Package_ID = ppm.EST_Package_ID
	join EST_PolyBag pb on ppm.EST_PolyBag_ID = pb.EST_PolyBag_ID
where p.EST_Estimate_ID = @EST_Estimate_ID

return (isnull(@SoloMailQuantity, 0) + isnull(@PolyBagQuantity, 0))
end
GO

GRANT  EXECUTE  ON [dbo].[EstimateDirectMailQuantity]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.EstimateHostAdNumber') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstimateHostAdNumber'
	DROP FUNCTION dbo.EstimateHostAdNumber
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstimateHostAdNumber') IS NOT NULL
		PRINT '***********Drop of dbo.EstimateHostAdNumber FAILED.'
END
GO
PRINT 'Creating dbo.EstimateHostAdNumber'
GO

CREATE FUNCTION dbo.EstimateHostAdNumber(@EST_Estimate_ID bigint)
returns int
/*
* PARAMETERS:
*	EST_Estimate_ID
*
* DESCRIPTION:
*	Returns the ad number of the estimate's host component.
*
* TABLES:
*   Table Name              Access
*   ==========              ======
*   EST_Component           READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	The ad number of the estimate's host component.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/25/2007      BJS             Initial Creation 
*
*/
as
begin
	return(select c.AdNumber
		from EST_Component c
		where c.EST_Estimate_ID = @EST_Estimate_ID and c.EST_ComponentType_ID = 1)
end
GO

GRANT  EXECUTE  ON [dbo].[EstimateHostAdNumber]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
IF OBJECT_ID('dbo.EstimatePieceWeight') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstimatePieceWeight'
	DROP FUNCTION dbo.EstimatePieceWeight
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstimatePieceWeight') IS NOT NULL
		PRINT '***********Drop of dbo.EstimatePieceWeight FAILED.'
END
GO
PRINT 'Creating dbo.EstimatePieceWeight'
GO

create function dbo.EstimatePieceWeight(@EST_Estimate_ID bigint)
returns decimal(12,6)
/*
* PARAMETERS:
* EST_Estimate_ID - Required.  The EstimateID.
*
*
* DESCRIPTION:
*		Returns the estimate piece weight.  (The total piece weight of each of its components).
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Component
*   PPR_PaperWeight
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
* 06/20/2007      BJS             Initial Creation 
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
as
begin
	return(
		select sum((c.Width * c.Height) / cast(950000 as decimal) * c.PageCount * pw.Weight * 1.03)
		from EST_Component c join PPR_PaperWeight pw on c.PaperWeight_ID = pw.PPR_PaperWeight_ID
		where EST_Estimate_ID = @EST_Estimate_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[EstimatePieceWeight]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.EstimatePolybagCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstimatePolybagCost'
	DROP FUNCTION dbo.EstimatePolybagCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstimatePolybagCost') IS NOT NULL
		PRINT '***********Drop of dbo.EstimatePolybagCost FAILED.'
END
GO
PRINT 'Creating dbo.EstimatePolybagCost'
GO

CREATE function dbo.EstimatePolybagCost(@EST_Estimate_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Estimate_ID - An Estimate_ID
*
* DESCRIPTION:
*	Calculates an estimate's portion of polybag costs.
*
* TABLES:
*   Table Name                   Access
*   ==========                   ======
*   EST_Component                READ
*   EST_PackageComponentMapping  READ
*   EST_Package                  READ
*   EST_PackagePolybag_Map       READ
*   EST_Polybag                  READ
*   EST_PolybagGroup             READ
*   VND_Printer                  READ
*   PRT_PrinterRate              READ
*   EST_EstimatePolybagGroup_Map READ
*
* PROCEDURES CALLED:
*   None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Polybag Cost (Bag Cost and Message Cost).
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/12/2007      BJS             Initial Creation
* 07/26/2007      BJS             Renamed ComponentPolybagCost -> EstimatePolybagCost
*
*/
begin
	return(select sum(dbo.PolybagCost(ppm.EST_Polybag_ID) * isnull(ppm.DistributionPct, dbo.PackageWeight(p.EST_Package_ID) / dbo.PolyBagWeight(ppm.EST_Polybag_ID)))
		from EST_Package p join EST_PackagePolybag_Map ppm on p.EST_Package_ID = ppm.EST_Package_ID
		where p.EST_Estimate_ID = @EST_Estimate_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[EstimatePolybagCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.EstimatePolybagInkjetCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstimatePolybagInkjetCost'
	DROP FUNCTION dbo.EstimatePolybagInkjetCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstimatePolybagInkjetCost') IS NOT NULL
		PRINT '***********Drop of dbo.EstimatePolybagInkjetCost FAILED.'
END
GO
PRINT 'Creating dbo.EstimatePolybagInkjetCost'
GO

CREATE function dbo.EstimatePolybagInkjetCost(@EST_Estimate_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Estimate_ID - An Estimate ID
*
* DESCRIPTION:
*	Calculates the Estimate's portion of the polybag inkjet cost.
*   Totals each of the costs and returns it as the Total Estimate Polybag inkjet cost.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_PackageComponentMapping
*
* PROCEDURES CALLED:
*	  dbo.PolybagInkjetCost
*   dbo.PackageWeight
*   dbo.PolyBagWeight
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Estimate Polybag inkjet cost.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/18/2007      BJS             Initial Creation
* 07/26/2007      BJS             Renamed ComponentPolybagInkjetCost->EstimatePolybagInkjetCost
* 10/15/2007      BJS             Removed multiply by quantity, it is already handled in the PolybagInkjetCost function
*
*/
as
begin
	return(select sum(dbo.PolybagInkjetCost(pb.EST_Polybag_ID) * isnull(ppm.DistributionPct, dbo.PackageWeight(p.EST_Package_ID) / dbo.PolyBagWeight(pb.EST_Polybag_ID)))
		from EST_Package p join EST_PackagePolyBag_Map ppm on p.EST_Package_ID = ppm.EST_Package_ID
			join EST_Polybag pb on ppm.EST_Polybag_ID = pb.EST_Polybag_ID
		where p.EST_Estimate_ID = @EST_Estimate_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[EstimatePolybagInkjetCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.EstimatePostalDropCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstimatePostalDropCost'
	DROP FUNCTION dbo.EstimatePostalDropCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstimatePostalDropCost') IS NOT NULL
		PRINT '***********Drop of dbo.EstimatePostalDropCost FAILED.'
END
GO
PRINT 'Creating dbo.EstimatePostalDropCost'
GO

CREATE function dbo.EstimatePostalDropCost(@EST_Estimate_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Estimate_ID - An Estimate ID.
*
* DESCRIPTION:
*	Calculates the Estimate's portion of the polybag postal drop cost.
*   Totals each of the polybag postal drop costs.
*   Calculates the solo mail postal drop cost.
*   Returns the total estimate postal drop cost.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_PackageComponentMapping
*
* PROCEDURES CALLED:
*   dbo.PolybagPostalDropCost
*   dbo.PackageWeight
*   dbo.PolyBagWeight
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Estimate postal drop cost.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/18/2007      BJS             Initial Creation 
* 07/23/2007      BJS             Removed fuel surcharge calculation
* 07/24/2007	  JRH		      Get PostalDropCWT from VND_MailhouseRate
* 09/19/2007      BJS             Check for Postal Drop Flat Override
*/
as
begin
	declare
		@PolybagPostalDropCost money,
		@SoloPostalDropCost    money

	select @PolybagPostalDropCost = sum(dbo.PolybagPostalDropCost(pb.EST_Polybag_ID) * isnull(ppm.DistributionPct, dbo.PackageWeight(p.EST_Package_ID) / dbo.PolyBagWeight(pb.EST_Polybag_ID)))
	from EST_Package p join EST_PackagePolyBag_Map ppm on p.EST_Package_ID = ppm.EST_Package_ID
		join EST_Polybag pb on ppm.EST_Polybag_ID = pb.EST_Polybag_ID
	where p.EST_Estimate_ID = @EST_Estimate_ID

	if exists (select 1 from EST_AssemDistribOptions where EST_Estimate_ID = @EST_Estimate_ID and PostalDropFlat is not null) begin
		declare
			@PostalDropFlat              money,
			@SoloWeight                  decimal(14,6),
			@SoloAndPrimaryPolybagWeight decimal(14,6)

		select @PostalDropFlat = PostalDropFlat
		from EST_AssemDistribOptions
		where EST_Estimate_ID = @EST_Estimate_ID

		set @SoloAndPrimaryPolybagWeight = isnull(dbo.EstimateSoloAndPrimaryPolybagWeight(@EST_Estimate_ID), 0)

		select @SoloWeight = sum(isnull(p.SoloQuantity, 0) * (c.Width * c.Height) / cast(950000 as decimal) * c.PageCount * pw.Weight * 1.03)
		from EST_Package p join EST_PackageComponentMapping pcm on p.EST_Package_ID = pcm.EST_Package_ID
			join EST_Component c on pcm.EST_Component_ID = c.EST_Component_ID
			join PPR_PaperWeight pw on c.PaperWeight_ID = pw.PPR_PaperWeight_ID
		where p.EST_Estimate_ID = @EST_Estimate_ID

		if (@SoloAndPrimaryPolybagWeight = 0)
			set @SoloPostalDropCost = @PostalDropFlat
		else
			set @SoloPostalDropCost = @PostalDropFlat * @SoloWeight / @SoloAndPrimaryPolybagWeight

	end
	else begin
		select @SoloPostalDropCost = sum(
			isnull(p.SoloQuantity, 0)
				* isnull(dbo.PackageWeight(p.EST_Package_ID), 0)
				* mh.PostalDropCWT / 100)
		from EST_Estimate e 
			join EST_AssemDistribOptions ad on e.EST_Estimate_ID = ad.EST_Estimate_ID
			join EST_Package p on e.EST_Estimate_ID = p.EST_Estimate_ID
			join VND_MailhouseRate mh on ad.Mailhouse_ID = mh.VND_MailhouseRate_ID
		where e.EST_Estimate_ID = @EST_Estimate_ID
	end

	return (isnull(@PolybagPostalDropCost, 0) + isnull(@SoloPostalDropCost, 0))
end
GO

GRANT  EXECUTE  ON [dbo].[EstimatePostalDropCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO


GO
IF OBJECT_ID('dbo.EstimatePostalDropFuelSurchargeCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstimatePostalDropFuelSurchargeCost'
	DROP FUNCTION dbo.EstimatePostalDropFuelSurchargeCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstimatePostalDropFuelSurchargeCost') IS NOT NULL
		PRINT '***********Drop of dbo.EstimatePostalDropFuelSurchargeCost FAILED.'
END
GO
PRINT 'Creating dbo.EstimatePostalDropFuelSurchargeCost'
GO

CREATE function dbo.EstimatePostalDropFuelSurchargeCost(@EST_Estimate_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Estimate_ID - An Estimate ID.
*
* DESCRIPTION:
*	1 - Calculates the Estimate's portion of the polybag postal drop fuel surcharge cost.
*	2 - Totals each of the polybag postal drop fuel surcharge costs.
*	3 - Calculates the solo mail postal drop fuel surcharge cost.
*	4 - Returns the total estimate postal drop fuel surcharge cost.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_PackageComponentMapping
*
* PROCEDURES CALLED:
*	  dbo.PolybagPostalDropFuelSurchargeCost
*   dbo.PackageWeight
*   dbo.PolyBagWeight
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Estimate postal drop cost.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/18/2007      BJS             Initial Creation 
* 07/24/2007	  JRH		  Get PostalDropCWT from VND_MailhouseRate
* 10/15/2007      BJS             SoloPostalDropCost -- divide total weight by 100
*
*/
as
begin
	declare @retval money
	
	select @retval = dbo.EstimatePostalDropCost(@EST_Estimate_ID) * isnull(MailFuelSurcharge, 0)
	from EST_AssemDistribOptions
	where EST_Estimate_ID = @EST_Estimate_ID
	
	return(isnull(@retval, 0))
end
GO

GRANT  EXECUTE  ON [dbo].[EstimatePostalDropFuelSurchargeCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.EstimatePrimaryPolybagQuantity') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstimatePrimaryPolybagQuantity'
	DROP FUNCTION dbo.EstimatePrimaryPolybagQuantity
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstimatePrimaryPolybagQuantity') IS NOT NULL
		PRINT '***********Drop of dbo.EstimatePrimaryPolybagQuantity FAILED.'
END
GO
PRINT 'Creating dbo.EstimatePrimaryPolybagQuantity'
GO

create function dbo.EstimatePrimaryPolybagQuantity(@EST_Estimate_ID bigint)
returns int
/*
* PARAMETERS:
*	EST_Estimate_ID - An estimate ID.
*
* DESCRIPTION:
*	Returns the Estimate's Primary Polybag Quantity
*
* TABLES:
*   Table Name                      Access
*   ==========                      ======
*   EST_Estimate                    READ
*   EST_EstimatePolybagGroup_Map    READ
*   EST_Polybag                     READ
*
* PROCEDURES CALLED:
*   dbo.PolybagPrimaryEstimate
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Mail Quantity used to perform A&D calculations.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 10/17/2007      BJS             Initial Creation - Logic copied from EstimateSoloandPrimaryPolybagQuantity
*
*/
begin
	declare @retval int

	select @retval = sum(
		case
			when dbo.PolybagPrimaryEstimate(pb.EST_Polybag_ID) = e.EST_Estimate_ID then pb.Quantity
			else 0
		end)
	from EST_Estimate e join EST_EstimatePolybagGroup_Map epgm on e.EST_Estimate_ID = epgm.EST_Estimate_ID
		join EST_Polybag pb on epgm.EST_PolybagGroup_ID = pb.EST_PolybagGroup_ID
	where e.EST_Estimate_ID = @EST_Estimate_ID

	return(isnull(@retval, 0))
end
GO

GRANT  EXECUTE  ON [dbo].[EstimatePrimaryPolybagQuantity]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.EstimateSoloAndPrimaryPolybagQuantity') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstimateSoloAndPrimaryPolybagQuantity'
	DROP FUNCTION dbo.EstimateSoloAndPrimaryPolybagQuantity
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstimateSoloAndPrimaryPolybagQuantity') IS NOT NULL
		PRINT '***********Drop of dbo.EstimateSoloAndPrimaryPolybagQuantity FAILED.'
END
GO
PRINT 'Creating dbo.EstimateSoloAndPrimaryPolybagQuantity'
GO

create function dbo.EstimateSoloAndPrimaryPolybagQuantity(@EST_Estimate_ID bigint)
returns int
/*
* PARAMETERS:
*	EST_Estimate_ID - An estimate ID.
*
* DESCRIPTION:
*	Returns the total mailing quantity that will use this estimate's a&d rates.
*
* TABLES:
*   Table Name                      Access
*   ==========                      ======
*   EST_Component                   READ
*   EST_PackageComponentMapping     READ
*   EST_Package                     READ
*   EST_PackagePolybag_Map          READ
*   EST_Polybag                     READ
*
* PROCEDURES CALLED:
*   dbo.PolybagPrimaryEstimate
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Mail Quantity used to perform A&D calculations.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/12/2007      BJS             Initial Creation
* 07/26/2007      BJS             Renamed ComponentSoloAndPrimaryPolybagQuantity -> EstimateSoloAndPrimaryPolybagQuantity
* 10/17/2007      BJS             Changed reference to dbo.PolybagPrimaryHost -> dbo.PolybagPrimaryEstimate
*
*/
begin
	declare @SoloQuantity int, @PrimaryPolybagQuantity int, @retval int

	select @SoloQuantity = sum(p.SoloQuantity)
	from EST_Package p
	where p.EST_Estimate_ID = @EST_Estimate_ID

	select @PrimaryPolybagQuantity = sum(
		case
			when dbo.PolybagPrimaryEstimate(pb.EST_Polybag_ID) = e.EST_Estimate_ID then pb.Quantity
			else 0
		end)
	from EST_Estimate e join EST_EstimatePolybagGroup_Map epgm on e.EST_Estimate_ID = epgm.EST_Estimate_ID
		join EST_Polybag pb on epgm.EST_PolybagGroup_ID = pb.EST_PolybagGroup_ID
	where e.EST_Estimate_ID = @EST_Estimate_ID


	set @retval = isnull(@SoloQuantity, 0) + isnull(@PrimaryPolybagQuantity, 0)
	return(@retval)
end
GO

GRANT  EXECUTE  ON [dbo].[EstimateSoloAndPrimaryPolybagQuantity]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.EstimateSoloAndPrimaryPolybagWeight') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstimateSoloAndPrimaryPolybagWeight'
	DROP FUNCTION dbo.EstimateSoloAndPrimaryPolybagWeight
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstimateSoloAndPrimaryPolybagWeight') IS NOT NULL
		PRINT '***********Drop of dbo.EstimateSoloAndPrimaryPolybagWeight FAILED.'
END
GO
PRINT 'Creating dbo.EstimateSoloAndPrimaryPolybagWeight'
GO

create function dbo.EstimateSoloAndPrimaryPolybagWeight(@EST_Estimate_ID bigint)
returns decimal(14,6)
/*
* PARAMETERS:
*	EST_Estimate_ID - An estimate ID.
*
* DESCRIPTION:
*	Returns the total mailing quantity that will use this estimate's a&d rates.
*
* TABLES:
*   Table Name                      Access
*   ==========                      ======
*   EST_Component                   READ
*   EST_PackageComponentMapping     READ
*   EST_Package                     READ
*   EST_PackagePolybag_Map          READ
*   EST_Polybag                     READ
*
* PROCEDURES CALLED:
*   dbo.PolybagPrimaryEstimate
*   dbo.PolybagWeightIncludeBagWeight
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Mail Weight used to perform A&D calculations.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 09/19/2007      BJS             Initial Creation
* 10/17/2007      BJS             Changed reference to dbo.PolybagPrimaryHost -> dbo.PolybagPrimaryEstimate
*
*/
begin
	declare @SoloWeight decimal(14,6), @PrimaryPolybagWeight decimal(14,6), @retval decimal(14,6)

	select @SoloWeight = sum(isnull(p.SoloQuantity, 0) * (c.Width * c.Height) / 950000 * c.PageCount * pw.Weight * 1.03)
	from EST_Package p join EST_PackageComponentMapping pcm on p.EST_Package_ID = pcm.EST_Package_ID
		join EST_Component c on pcm.EST_Component_ID = c.EST_Component_ID
		join PPR_PaperWeight pw on c.PaperWeight_ID = pw.PPR_PaperWeight_ID
	where p.EST_Estimate_ID = @EST_Estimate_ID

	select @PrimaryPolybagWeight = sum(
		case
			when dbo.PolybagPrimaryEstimate(pb.EST_Polybag_ID) = e.EST_Estimate_ID then dbo.PolybagWeightIncludeBagWeight(pb.EST_Polybag_ID)
			else 0
		end)
	from EST_Estimate e join EST_EstimatePolybagGroup_Map epgm on e.EST_Estimate_ID = epgm.EST_Estimate_ID
		join EST_Polybag pb on epgm.EST_PolybagGroup_ID = pb.EST_PolybagGroup_ID
	where e.EST_Estimate_ID = @EST_Estimate_ID

	set @retval = isnull(@SoloWeight, 0) + isnull(@PrimaryPolybagWeight, 0)
	return(@retval)
end
GO

GRANT  EXECUTE  ON [dbo].[EstimateSoloAndPrimaryPolybagWeight]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
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

GO
IF OBJECT_ID('dbo.getFiscalData') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.getFiscalData'
	DROP FUNCTION dbo.getFiscalData
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.getFiscalData') IS NOT NULL
		PRINT '***********Drop of dbo.getFiscalData FAILED.'
END
GO
PRINT 'Creating dbo.getFiscalData'
GO

CREATE FUNCTION dbo.getFiscalData (@inDate datetime)
RETURNS @fiscal_data TABLE (
	  fiscal_yr smallint
	, fiscal_mth_nbr char(2)
	, season_cd char(1)
)
AS  
/*
* PARAMETERS:
* @inDate - Date to process
*
* DESCRIPTION:
*	Determines fiscal year, month, and season for an inputted date
*	NOTE: This logic was copied from legacy PowerBuilder code.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Table containing Fiscal Year, Fiscal Month and Season ID
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 06/21/2007      BJS             Initial Creation.
* 
*
*/
BEGIN 

DECLARE @curr_mo_start_dt datetime
DECLARE @curr_fiscal_yr int
DECLARE @curr_fiscal_mth int
DECLARE @curr_mth_nbr_of_wks int
DECLARE @next_mth_nbr_of_wks int
DECLARE @found int

SET @curr_mo_start_dt = '1/31/93'
SET @curr_fiscal_yr = 1992
SET @curr_mth_nbr_of_wks = 5
SET @next_mth_nbr_of_wks = 4
SET @found = 0

While @found = 0
	BEGIN
	
	SET @curr_fiscal_mth = 1
	SET @curr_fiscal_yr = @curr_fiscal_yr + 1

	While @curr_fiscal_mth < 13 And @found = 0
		BEGIN
		
		-- Account for 53 week years.
		-- Here we supply the first day of the last fiscal
		-- month of the 53 week year.

		IF DATEDIFF(Day, @curr_mo_start_dt, '12/31/1995') = 0 OR 
		   DATEDIFF(Day, @curr_mo_start_dt, '12/31/2000') = 0 OR 
		   DATEDIFF(Day, @curr_mo_start_dt, '12/31/2006') = 0 OR 
		   DATEDIFF(Day, @curr_mo_start_dt, '12/30/2012') = 0 OR
		   DATEDIFF(Day, @curr_mo_start_dt, '12/31/2017') = 0
			BEGIN
			SET @curr_mth_nbr_of_wks = 6
			END

		IF @curr_mth_nbr_of_wks = 4 And @next_mth_nbr_of_wks = 4
			BEGIN
			SET @curr_mo_start_dt = DATEADD(Day, 35, @curr_mo_start_dt) 
			SET @next_mth_nbr_of_wks = 5
			END
		ELSE IF @curr_mth_nbr_of_wks = 4 And @next_mth_nbr_of_wks = 5
			BEGIN
			SET @curr_mo_start_dt = DATEADD(Day, 28, @curr_mo_start_dt)
			SET @curr_mth_nbr_of_wks = 5
			SET @next_mth_nbr_of_wks = 4
			END
		ELSE IF @curr_mth_nbr_of_wks = 5 And @next_mth_nbr_of_wks = 4
			BEGIN
			SET @curr_mo_start_dt = DATEADD(Day, 28, @curr_mo_start_dt)
			SET @curr_mth_nbr_of_wks = 4
			END
		ELSE IF @curr_mth_nbr_of_wks = 6 And @next_mth_nbr_of_wks = 5
			BEGIN
			SET @curr_mo_start_dt = DATEADD(Day, 35, @curr_mo_start_dt) 
			SET @curr_mth_nbr_of_wks = 5
			SET @next_mth_nbr_of_wks = 4
			END

		IF DATEDIFF(Day, @inDate, @curr_mo_start_dt) > 0
			BEGIN
			SET @found = 1
			END
		ELSE
			BEGIN
			SET @curr_fiscal_mth = @curr_fiscal_mth + 1
			END
		END
	END

DECLARE @ret_fiscal_mth char(2)
DECLARE @ret_season_cd char(1)

If @curr_fiscal_mth < 10
	BEGIN
	SET @ret_fiscal_mth = '0' + CONVERT(char(1), @curr_fiscal_mth)
	END
ELSE
	BEGIN
	SET @ret_fiscal_mth = CONVERT(char(2), @curr_fiscal_mth)
	END

If @curr_fiscal_mth < 7
	BEGIN
	SET @ret_season_cd = '1'
	END
Else
	BEGIN
	SET @ret_season_cd = '2'
	END

-- Return the data

INSERT INTO @fiscal_data (
	  fiscal_yr
	, fiscal_mth_nbr
	, season_cd
) VALUES (
    ISNULL(@curr_fiscal_yr, 0)
  , ISNULL(@ret_fiscal_mth, '00')
  , ISNULL(@ret_season_cd, '0')
)
RETURN
END
GO

GRANT  SELECT  ON [dbo].[getFiscalData]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.getFiscalMonth') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.getFiscalMonth'
	DROP FUNCTION dbo.getFiscalMonth
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.getFiscalMonth') IS NOT NULL
		PRINT '***********Drop of dbo.getFiscalMonth FAILED.'
END
GO
PRINT 'Creating dbo.getFiscalMonth'
GO

CREATE FUNCTION dbo.getFiscalMonth (@inDate datetime)
RETURNS int
AS  
/*
* PARAMETERS:
* @inDate - Date to process
*
* DESCRIPTION:
*	Determines fiscal month inputted date.
*	NOTE: This logic was copied from legacy PowerBuilder code.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	curr_fiscal_mth - The Fiscal Month for the inputted date.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 06/21/2007      BJS             Initial Creation.
* 
*
*/
BEGIN 

DECLARE @curr_mo_start_dt datetime
DECLARE @curr_fiscal_yr int
DECLARE @curr_fiscal_mth int
DECLARE @curr_mth_nbr_of_wks int
DECLARE @next_mth_nbr_of_wks int
DECLARE @found int

SET @curr_mo_start_dt = '1/31/93'
SET @curr_fiscal_yr = 1992
SET @curr_mth_nbr_of_wks = 5
SET @next_mth_nbr_of_wks = 4
SET @found = 0

While @found = 0
	BEGIN
	
	SET @curr_fiscal_mth = 1
	SET @curr_fiscal_yr = @curr_fiscal_yr + 1

	While @curr_fiscal_mth < 13 And @found = 0
		BEGIN
		
		-- Account for 53 week years.
		-- Here we supply the first day of the last fiscal
		-- month of the 53 week year.

		IF DATEDIFF(Day, @curr_mo_start_dt, '12/31/1995') = 0 OR 
		   DATEDIFF(Day, @curr_mo_start_dt, '12/31/2000') = 0 OR 
		   DATEDIFF(Day, @curr_mo_start_dt, '12/31/2006') = 0 OR 
		   DATEDIFF(Day, @curr_mo_start_dt, '12/30/2012') = 0 OR
		   DATEDIFF(Day, @curr_mo_start_dt, '12/31/2017') = 0
			BEGIN
			SET @curr_mth_nbr_of_wks = 6
			END

		IF @curr_mth_nbr_of_wks = 4 And @next_mth_nbr_of_wks = 4
			BEGIN
			SET @curr_mo_start_dt = DATEADD(Day, 35, @curr_mo_start_dt) 
			SET @next_mth_nbr_of_wks = 5
			END
		ELSE IF @curr_mth_nbr_of_wks = 4 And @next_mth_nbr_of_wks = 5
			BEGIN
			SET @curr_mo_start_dt = DATEADD(Day, 28, @curr_mo_start_dt)
			SET @curr_mth_nbr_of_wks = 5
			SET @next_mth_nbr_of_wks = 4
			END
		ELSE IF @curr_mth_nbr_of_wks = 5 And @next_mth_nbr_of_wks = 4
			BEGIN
			SET @curr_mo_start_dt = DATEADD(Day, 28, @curr_mo_start_dt)
			SET @curr_mth_nbr_of_wks = 4
			END
		ELSE IF @curr_mth_nbr_of_wks = 6 And @next_mth_nbr_of_wks = 5
			BEGIN
			SET @curr_mo_start_dt = DATEADD(Day, 35, @curr_mo_start_dt) 
			SET @curr_mth_nbr_of_wks = 5
			SET @next_mth_nbr_of_wks = 4
			END

		IF DATEDIFF(Day, @inDate, @curr_mo_start_dt) > 0
			BEGIN
			SET @found = 1
			END
		ELSE
			BEGIN
			SET @curr_fiscal_mth = @curr_fiscal_mth + 1
			END
		END
	END


-- Return the data
return(@curr_fiscal_mth)
END
GO

GRANT  EXECUTE  ON [dbo].[getFiscalMonth]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.getFiscalYear') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.getFiscalYear'
	DROP FUNCTION dbo.getFiscalYear
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.getFiscalYear') IS NOT NULL
		PRINT '***********Drop of dbo.getFiscalYear FAILED.'
END
GO
PRINT 'Creating dbo.getFiscalYear'
GO

CREATE FUNCTION dbo.getFiscalYear (@inDate datetime)
RETURNS int
AS  
/*
* PARAMETERS:
* @inDate - Date to process
*
* DESCRIPTION:
*	Determines fiscal year inputted date.
*	NOTE: This logic was copied from legacy PowerBuilder code.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	curr_fiscal_yr - The Fiscal Year for the inputted date.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 06/21/2007      BJS             Initial Creation.
* 
*
*/
BEGIN 

DECLARE @curr_mo_start_dt datetime
DECLARE @curr_fiscal_yr int
DECLARE @curr_fiscal_mth int
DECLARE @curr_mth_nbr_of_wks int
DECLARE @next_mth_nbr_of_wks int
DECLARE @found int

SET @curr_mo_start_dt = '1/31/93'
SET @curr_fiscal_yr = 1992
SET @curr_mth_nbr_of_wks = 5
SET @next_mth_nbr_of_wks = 4
SET @found = 0

While @found = 0
	BEGIN
	
	SET @curr_fiscal_mth = 1
	SET @curr_fiscal_yr = @curr_fiscal_yr + 1

	While @curr_fiscal_mth < 13 And @found = 0
		BEGIN
		
		-- Account for 53 week years.
		-- Here we supply the first day of the last fiscal
		-- month of the 53 week year.

		IF DATEDIFF(Day, @curr_mo_start_dt, '12/31/1995') = 0 OR 
		   DATEDIFF(Day, @curr_mo_start_dt, '12/31/2000') = 0 OR 
		   DATEDIFF(Day, @curr_mo_start_dt, '12/31/2006') = 0 OR 
		   DATEDIFF(Day, @curr_mo_start_dt, '12/30/2012') = 0 OR
		   DATEDIFF(Day, @curr_mo_start_dt, '12/31/2017') = 0
			BEGIN
			SET @curr_mth_nbr_of_wks = 6
			END

		IF @curr_mth_nbr_of_wks = 4 And @next_mth_nbr_of_wks = 4
			BEGIN
			SET @curr_mo_start_dt = DATEADD(Day, 35, @curr_mo_start_dt) 
			SET @next_mth_nbr_of_wks = 5
			END
		ELSE IF @curr_mth_nbr_of_wks = 4 And @next_mth_nbr_of_wks = 5
			BEGIN
			SET @curr_mo_start_dt = DATEADD(Day, 28, @curr_mo_start_dt)
			SET @curr_mth_nbr_of_wks = 5
			SET @next_mth_nbr_of_wks = 4
			END
		ELSE IF @curr_mth_nbr_of_wks = 5 And @next_mth_nbr_of_wks = 4
			BEGIN
			SET @curr_mo_start_dt = DATEADD(Day, 28, @curr_mo_start_dt)
			SET @curr_mth_nbr_of_wks = 4
			END
		ELSE IF @curr_mth_nbr_of_wks = 6 And @next_mth_nbr_of_wks = 5
			BEGIN
			SET @curr_mo_start_dt = DATEADD(Day, 35, @curr_mo_start_dt) 
			SET @curr_mth_nbr_of_wks = 5
			SET @next_mth_nbr_of_wks = 4
			END

		IF DATEDIFF(Day, @inDate, @curr_mo_start_dt) > 0
			BEGIN
			SET @found = 1
			END
		ELSE
			BEGIN
			SET @curr_fiscal_mth = @curr_fiscal_mth + 1
			END
		END
	END


-- Return the data
return(@curr_fiscal_yr)
END
GO

GRANT  EXECUTE  ON [dbo].[getFiscalYear]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.getSeasonID_ByDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.getSeasonID_ByDate'
	DROP FUNCTION dbo.getSeasonID_ByDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.getSeasonID_ByDate') IS NOT NULL
		PRINT '***********Drop of dbo.getSeasonID_ByDate FAILED.'
END
GO
PRINT 'Creating dbo.getSeasonID_ByDate'
GO

CREATE FUNCTION dbo.getSeasonID_ByDate (@inDate datetime)
RETURNS int
AS  
/*
* PARAMETERS:
* @inDate - Date to process
*
* DESCRIPTION:
*	Determines the Season for the inputted date.
*	NOTE: This logic was copied from legacy PowerBuilder code.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	curr_fiscal_mth - The Fiscal Month for the inputted date.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 06/21/2007      BJS             Initial Creation.
* 
*
*/
BEGIN 

DECLARE @curr_mo_start_dt datetime
DECLARE @curr_fiscal_yr int
DECLARE @curr_fiscal_mth int
DECLARE @curr_mth_nbr_of_wks int
DECLARE @next_mth_nbr_of_wks int
DECLARE @found int

SET @curr_mo_start_dt = '1/31/93'
SET @curr_fiscal_yr = 1992
SET @curr_mth_nbr_of_wks = 5
SET @next_mth_nbr_of_wks = 4
SET @found = 0

While @found = 0
	BEGIN
	
	SET @curr_fiscal_mth = 1
	SET @curr_fiscal_yr = @curr_fiscal_yr + 1

	While @curr_fiscal_mth < 13 And @found = 0
		BEGIN
		
		-- Account for 53 week years.
		-- Here we supply the first day of the last fiscal
		-- month of the 53 week year.

		IF DATEDIFF(Day, @curr_mo_start_dt, '12/31/1995') = 0 OR 
		   DATEDIFF(Day, @curr_mo_start_dt, '12/31/2000') = 0 OR 
		   DATEDIFF(Day, @curr_mo_start_dt, '12/31/2006') = 0 OR 
		   DATEDIFF(Day, @curr_mo_start_dt, '12/30/2012') = 0 OR
		   DATEDIFF(Day, @curr_mo_start_dt, '12/31/2017') = 0
			BEGIN
			SET @curr_mth_nbr_of_wks = 6
			END

		IF @curr_mth_nbr_of_wks = 4 And @next_mth_nbr_of_wks = 4
			BEGIN
			SET @curr_mo_start_dt = DATEADD(Day, 35, @curr_mo_start_dt) 
			SET @next_mth_nbr_of_wks = 5
			END
		ELSE IF @curr_mth_nbr_of_wks = 4 And @next_mth_nbr_of_wks = 5
			BEGIN
			SET @curr_mo_start_dt = DATEADD(Day, 28, @curr_mo_start_dt)
			SET @curr_mth_nbr_of_wks = 5
			SET @next_mth_nbr_of_wks = 4
			END
		ELSE IF @curr_mth_nbr_of_wks = 5 And @next_mth_nbr_of_wks = 4
			BEGIN
			SET @curr_mo_start_dt = DATEADD(Day, 28, @curr_mo_start_dt)
			SET @curr_mth_nbr_of_wks = 4
			END
		ELSE IF @curr_mth_nbr_of_wks = 6 And @next_mth_nbr_of_wks = 5
			BEGIN
			SET @curr_mo_start_dt = DATEADD(Day, 35, @curr_mo_start_dt) 
			SET @curr_mth_nbr_of_wks = 5
			SET @next_mth_nbr_of_wks = 4
			END

		IF DATEDIFF(Day, @inDate, @curr_mo_start_dt) > 0
			BEGIN
			SET @found = 1
			END
		ELSE
			BEGIN
			SET @curr_fiscal_mth = @curr_fiscal_mth + 1
			END
		END
	END

DECLARE @ret_season_id int

If @curr_fiscal_mth < 7
	SET @ret_season_id = 1 /*Spring*/
Else
	SET @ret_season_id = 2 /*Fall*/

-- Return the data
return(@ret_season_id)
END
GO

GRANT  EXECUTE  ON [dbo].[getSeasonID_ByDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.IsEstimateUsingPostalVendor') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.IsEstimateUsingPostalVendor'
	DROP FUNCTION dbo.IsEstimateUsingPostalVendor
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.IsEstimateUsingPostalVendor') IS NOT NULL
		PRINT '***********Drop of dbo.IsEstimateUsingPostalVendor FAILED.'
END
GO
PRINT 'Creating dbo.IsEstimateUsingPostalVendor'
GO

CREATE FUNCTION dbo.IsEstimateUsingPostalVendor(@EST_Estimate_ID bigint, @VND_Vendor_ID bigint)
returns bit
/*
* PARAMETERS:
*	EST_Estimate_ID
*   VND_Vendor_ID
*
* DESCRIPTION:
*	Determines whether the Estimate utilizes the postal vendor.
*
* TABLES:
*   Table Name                     Access
*   ==========                     ======
*   EST_AssemDistribOptions        READ
*   PST_PostalScenario             READ
*   PST_PostalCategoryScenario_Map READ
*   PST_PostalCategoryRate_Map     READ
*   PST_PostalWeights              READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	0 - Estimate does not utilize the specified postal vendor
*   1 - Estimate does utilize the specified postal vendor
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/25/2007      BJS             Initial Creation 
*
*/
as
begin
	declare @retval bit
	if exists(
			select 1
			from EST_AssemDistribOptions ad join PST_PostalScenario ps on ad.PST_PostalScenario_ID = ps.PST_PostalScenario_ID
				join PST_PostalCategoryScenario_Map pcsm on ps.PST_PostalScenario_ID = pcsm.PST_PostalScenario_ID
				join PST_PostalCategoryRate_Map pcrm on pcsm.PST_PostalCategoryRate_Map_ID = pcrm.PST_PostalCategoryRate_Map_ID
				join PST_PostalWeights pw on pcrm.PST_PostalWeights_ID = pw.PST_PostalWeights_ID
			where ad.EST_Estimate_ID = @EST_Estimate_ID and pw.VND_Vendor_ID = @VND_Vendor_ID)
		set @retval = 1
	else
		set @retval = 0

	return(@retval)
end
GO

GRANT  EXECUTE  ON [dbo].[IsEstimateUsingPostalVendor]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
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
GO
IF OBJECT_ID('dbo.IsPubRateMapActive') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.IsPubRateMapActive'
	DROP FUNCTION dbo.IsPubRateMapActive
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.IsPubRateMapActive') IS NOT NULL
		PRINT '***********Drop of dbo.IsPubRateMapActive FAILED.'
END
GO
PRINT 'Creating dbo.IsPubRateMapActive'
GO

CREATE function dbo.IsPubRateMapActive(@PUB_PubRate_Map_ID bigint, @InsertDate datetime)
/*
* PARAMETERS:
*	PUB_PubRate_Map_ID
*   InsertDate
*
* DESCRIPTION:
*	Returns whether a Pub Rate Map is active on a specific date.
*
* TABLES:
*   Table Name                   Access
*   ==========                   ======
*   PUB_PubRate_Map_Activate     READ
*
* PROCEDURES CALLED:
*   None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	0 - Inactive
*   1 - Active
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 09/07/2007      BJS             Initial Creation
*
*/
returns bit
as
begin
	declare @retval bit
	if not exists(select 1 from PUB_PubRate_Map_Activate where PUB_PubRate_Map_ID = @PUB_PubRate_Map_ID and EffectiveDate <= @InsertDate)
		set @retval = 0
	else
		select top 1 @retval = Active
		from PUB_PubRate_Map_Activate
		where PUB_PubRate_Map_ID = @PUB_PubRate_Map_ID and EffectiveDate <= @InsertDate
		order by EffectiveDate desc
	return(@retval)
end
GO

GRANT  EXECUTE  ON [dbo].[IsPubRateMapActive]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PackageInsertIndex') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PackageInsertIndex'
	DROP FUNCTION dbo.PackageInsertIndex
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PackageInsertIndex') IS NOT NULL
		PRINT '***********Drop of dbo.PackageInsertIndex FAILED.'
END
GO
PRINT 'Creating dbo.PackageInsertIndex'
GO

create function dbo.PackageInsertIndex(@EST_Package_ID bigint, @PubRate_Map_ID bigint, @InsertDate datetime)
/*
* PARAMETERS:
*	EST_Package_ID
*   PubRate_Map_ID
*	InsertDate
*
* DESCRIPTION:
*	Returns the order an insert is placed into a package, by weight, with the heaviest first and the lightest last.
*
* TABLES:
*   Table Name                   Access
*   ==========                   ======
*   est_estimate			     READ
*	pub_pubgroup				 READ
*	pub_pubpubgroup_map			 READ
*	est_pubissuedates			 READ
*
* PROCEDURES CALLED:
*   None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	int - 0 for the heaviest and 1 for the lightest
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 09/07/2007      BJS             Initial Creation
* 11/19/2007      JRH             Performance improvements.
* 12/06/2007      JRH             Use vpw.PackageWeight to compare to @MyPackageWeight.
*
*/
returns int
as
begin

	declare @MyPackageWeight decimal(12,6)
	select @MyPackageWeight = dbo.PackageWeight(@EST_Package_ID)

	return(select count(*) + 1 -- Because the discount table is 1 based.
		from EST_Estimate e 
			join EST_Package p  
				on e.EST_Estimate_ID = p.EST_Estimate_ID  
				and e.EST_Status_ID = 1
			join dbo.vwPackageWeight vpw  
				on p.EST_Package_ID = vpw.EST_Package_ID
			join PUB_PubGroup pg  
				on p.PUB_PubGroup_ID = pg.PUB_PubGroup_ID
			join PUB_PubPubGroup_Map ppgm  
				on pg.PUB_PubGroup_ID = ppgm.PUB_PubGroup_ID  
				and ppgm.PUB_PubRate_Map_ID = @PubRate_Map_ID
			join EST_PubIssueDates pid  
				on e.EST_Estimate_ID = pid.EST_Estimate_ID  
				and ppgm.PUB_PubRate_Map_ID = pid.PUB_PubRate_Map_ID  
				and pid.IssueDate = @InsertDate
		/* Two packageweight comparisons needed in case separate packages have identical weights. */
		where p.EST_Package_ID <> @EST_Package_ID and
			(vpw.PackageWeight > @MyPackageWeight  
				or (vpw.PackageWeight = @MyPackageWeight and p.EST_Package_ID < @EST_Package_ID)))

end
GO

GRANT  EXECUTE  ON [dbo].[PackageInsertIndex]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PackagePageCount') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PackagePageCount'
	DROP FUNCTION dbo.PackagePageCount
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PackagePageCount') IS NOT NULL
		PRINT '***********Drop of dbo.PackagePageCount FAILED.'
END
GO
PRINT 'Creating dbo.PackagePageCount'
GO

create function dbo.PackagePageCount(@EST_Package_ID bigint)
/*
* PARAMETERS:
*	EST_Package_iD
*
* DESCRIPTION:
*	Returns the total number of pages for all components in a package.
*
* TABLES:
*   Table Name                   Access
*   ==========                   ======
*   est_component			     READ
*	est_packagecomponentmapping  READ
*
* PROCEDURES CALLED:
*   None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	int - Number of pages
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 09/07/2007      BJS             Initial Creation
*
*/
returns int
as
begin
	return(select sum(c.PageCount)
		from EST_Component c join EST_PackageComponentMapping pcm on c.EST_Component_ID = pcm.EST_Component_ID
		where pcm.EST_Package_ID = @EST_Package_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[PackagePageCount]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PackagePolyPostage') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PackagePolyPostage'
	DROP FUNCTION dbo.PackagePolyPostage
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PackagePolyPostage') IS NOT NULL
		PRINT '***********Drop of dbo.PackagePolyPostage FAILED.'
END
GO
PRINT 'Creating dbo.PackagePolyPostage'
GO

create function dbo.PackagePolyPostage(@EST_Package_ID bigint, @PackageWeight decimal(12,6), @EST_Polybag_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Package_ID
*	PackageWeight
*	EST_Polybag_ID
*
* DESCRIPTION:
*	Returns the total polybag postage for a package.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_PolyBag
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Polybag postage for Package
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/18/2007      BJS             Initial Creation 
* 10/03/2007      BJS             Modified select to select sum(...) so that one value is returned
* 10/26/2007      BJS             Added 6 points of precision to the Distribution Percentage
* 11/25/2007      JRH             Modified to return the postage per polybag.
*
*/
as
begin
return (
	select dbo.CalcItemPostage(pb.Quantity, dbo.PolyBagWeight(pb.EST_Polybag_ID) + pr.PolybagBagWeight, pb.PST_PostalScenario_ID)
		* isnull(cast(ppm.DistributionPct as decimal(12,6)), @PackageWeight / dbo.PolyBagWeight(pb.EST_Polybag_ID))
	from EST_PackagePolybag_Map ppm join EST_Polybag pb on ppm.EST_Polybag_ID = pb.EST_Polybag_ID
		join EST_PolybagGroup pbg on pb.EST_PolybagGroup_ID = pbg.EST_PolybagGroup_ID
		join VND_Printer pr on pbg.VND_Printer_ID = pr.VND_Printer_ID
	where ppm.EST_Package_ID = @EST_Package_ID
		and pb.EST_Polybag_ID = @EST_Polybag_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[PackagePolyPostage]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PackagePolyQuantity') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PackagePolyQuantity'
	DROP FUNCTION dbo.PackagePolyQuantity
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PackagePolyQuantity') IS NOT NULL
		PRINT '***********Drop of dbo.PackagePolyQuantity FAILED.'
END
GO
PRINT 'Creating dbo.PackagePolyQuantity'
GO

create function dbo.PackagePolyQuantity(@EST_Package_ID bigint)
returns int
/*
* PARAMETERS:
*	EST_Package_ID
*
* DESCRIPTION:
*	Returns the total polybag quantity for a package.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_PackagePolyBag_Map
*   EST_PolyBag
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Polybag Quantity for Package
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 06/21/2007      BJS             Initial Creation 
*
*/
as
begin
return (select sum(pb.Quantity)
	from EST_PackagePolyBag_Map ppm	join EST_PolyBag pb on ppm.EST_PolyBag_ID = pb.EST_PolyBag_ID
	where ppm.EST_Package_ID = @EST_Package_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[PackagePolyQuantity]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PackageSize') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PackageSize'
	DROP FUNCTION dbo.PackageSize
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PackageSize') IS NOT NULL
		PRINT '***********Drop of dbo.PackageSize FAILED.'
END
GO
PRINT 'Creating dbo.PackageSize'
GO

create function dbo.PackageSize(@EST_Package_ID bigint)
returns decimal(10,4)
/*
* PARAMETERS:
*	EST_Package_ID
*
* DESCRIPTION:
*	Calculates the size of a package
*
* TABLES:
*   Table Name                   Access
*   ==========                   ======
*   EST_Component                READ
*   EST_Package                  READ
*
* PROCEDURES CALLED:
*   None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	The package size
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/12/2007      BJS             Initial Creation
* 10/12/2007      BJS             Added logic to handle packages w/o a Host Component 
*
*/
as
begin
	declare @retval decimal(10,4)
	
	-- Package Size is the size of the host component
	select @retval = c.Height * c.Width
	from EST_Component c join EST_Package p on c.EST_Estimate_ID = p.EST_Estimate_ID
	where p.EST_Package_ID = @EST_Package_ID and c.EST_ComponentType_ID = 1
	
	-- If there is no host component in the package, return the size of the first component
	if (@retval is null) begin
		select @retval = c.Height * c.Width
		from EST_Component c join EST_Package p on c.EST_Estimate_ID = p.EST_Estimate_ID
		where p.EST_Package_ID = @EST_Package_ID and c.EST_ComponentType_ID = 1
	end	
			
	return(@retval)
end
GO

GRANT  EXECUTE  ON [dbo].[PackageSize]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PackageTabPageCount') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PackageTabPageCount'
	DROP FUNCTION dbo.PackageTabPageCount
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PackageTabPageCount') IS NOT NULL
		PRINT '***********Drop of dbo.PackageTabPageCount FAILED.'
END
GO
PRINT 'Creating dbo.PackageTabPageCount'
GO

create function dbo.PackageTabPageCount(@EST_Package_ID bigint, @BlowInRate int)
returns int
/*
* PARAMETERS:
*	EST_Package_ID
*   PUB_PubRate_ID
*
* DESCRIPTION:
*	Determines the tab page count of the package and calculates blow-in page counts according to the BlowInRate.
*
* TABLES:
*   Table Name              Access
*   ==========              ======
*   EST_Component           READ
*   EST_Package             READ
*
*
* PROCEDURES CALLED:
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	The package tab page count.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 10/15/2007      BJS             Initial Creation
*
*/
as
begin
	return(select sum(
			case
				when c.EST_EstimateMediaType_ID = 2 then 2 -- Broadsheet
				else 1
			end
			*
			case
				when c.EST_ComponentType_ID = 4 and @BlowInRate = 0 then 0 --Blow-In not charged
				when c.EST_ComponentType_ID = 4 and @BlowInRate = 1 then cast(c.PageCount as decimal) / 2 -- Blow-In charged at 1/2 page
				else c.PageCount
			end)
	from EST_PackageComponentMapping pcm join EST_Component c on pcm.EST_Component_ID = c.EST_Component_ID
	where pcm.EST_Package_ID = @EST_Package_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[PackageTabPageCount]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PackageWeight') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PackageWeight'
	DROP FUNCTION dbo.PackageWeight
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PackageWeight') IS NOT NULL
		PRINT '***********Drop of dbo.PackageWeight FAILED.'
END
GO
PRINT 'Creating dbo.PackageWeight'
GO

create function dbo.PackageWeight(@EST_Package_ID bigint)
/*
* PARAMETERS:
*	EST_Package_ID
*
* DESCRIPTION:
*	Returns the total weight of all the components in a package.
*
* TABLES:
*   Table Name                   Access
*   ==========                   ======
*   est_component			     READ
*	est_packagecomponentmapping	 READ
*	ppr_paperweight				 READ
*
* PROCEDURES CALLED:
*   None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	decimal(12,6) - Package Weight
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 09/07/2007      BJS             Initial Creation
*
*/
returns decimal(12,6)
as
begin
	return(select sum(c.Width * c.Height / cast(950000 as decimal) * c.PageCount * pw.Weight * 1.03)
		from EST_Component c join EST_PackageComponentMapping pcm on c.EST_Component_ID = pcm.EST_Component_ID
			join PPR_PaperWeight pw on c.PaperWeight_ID = pw.PPR_PaperWeight_ID
		where pcm.EST_Package_ID = @EST_Package_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[PackageWeight]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PolybagCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PolybagCost'
	DROP FUNCTION dbo.PolybagCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PolybagCost') IS NOT NULL
		PRINT '***********Drop of dbo.PolybagCost FAILED.'
END
GO
PRINT 'Creating dbo.PolybagCost'
GO

CREATE function dbo.PolybagCost(@EST_Polybag_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Component_ID - A Host Component ID
*
* DESCRIPTION:
*	Calculates the cost of a polybag.
*
* TABLES:
*   Table Name                   Access
*   ==========                   ======
*   EST_Component                READ
*   EST_PackageComponentMapping  READ
*   EST_Package                  READ
*   EST_PackagePolybag_Map       READ
*   EST_Polybag                  READ
*   EST_PolybagGroup             READ
*   VND_Printer                  READ
*   PRT_PrinterRate              READ
*   EST_EstimatePolybagGroup_Map READ
*
* PROCEDURES CALLED:
*   None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Polybag Cost (Bag Cost and Message Cost).
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/12/2007      BJS             Initial Creation 
*
*/
begin
	return(select top 1 (pb.Quantity + (isnull(c.SpoilagePct, 0) * pb.Quantity)) / 1000 * (br.Rate + case when pbg.UseMessage = 1 then pr.PolybagMessage else 0 end)
			+ c.NumberOfPlants * (mrr.Rate + case when pbg.UseMessage = 1 then pr.PolybagMessageMakeready else 0 end)
		from EST_Polybag pb join EST_PolybagGroup pbg on pb.EST_PolybagGroup_ID = pbg.EST_PolybagGroup_ID
			join VND_Printer pr on pbg.VND_Printer_ID = pr.VND_Printer_ID
			join PRT_PrinterRate br on pbg.PRT_BagRate_ID = br.PRT_PrinterRate_ID
			join PRT_PrinterRate mrr on pbg.PRT_BagMakereadyRate_ID = mrr.PRT_PrinterRate_ID
			join EST_EstimatePolybagGroup_Map pbgm on pb.EST_PolybagGroup_ID = pbgm.EST_PolybagGroup_ID
			join EST_Component c on pbgm.EST_Estimate_ID = c.EST_Estimate_ID
		where pb.EST_Polybag_ID = @EST_Polybag_ID and c.EST_ComponentType_ID = 1
		order by pbgm.EstimateOrder)
end	
GO

GRANT  EXECUTE  ON [dbo].[PolybagCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
IF OBJECT_ID('dbo.PolybagGroupRunDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PolybagGroupRunDate'
	DROP FUNCTION dbo.PolybagGroupRunDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PolybagGroupRunDate') IS NOT NULL
		PRINT '***********Drop of dbo.PolybagGroupRunDate FAILED.'
END
GO
PRINT 'Creating dbo.PolybagGroupRunDate'
GO

create function dbo.PolybagGroupRunDate(@EST_PolybagGroup_ID bigint)
returns datetime
/*
* PARAMETERS:
*	EST_PolybagGroup_ID
*
* DESCRIPTION:
*	Returns the "newest" run date of each of the estimate(s) in the Polybag Group.
*
* TABLES:
*   Table Name                   Access
*   ==========                   ======
*   EST_EstimatePolybagGroup_Map READ
*   EST_Estimate                 READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	The polybag run date.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 09/27/2007      BJS             Initial Creation 
*
*/
as
begin
	return(
	select max(e.RunDate)
	from EST_EstimatePolybagGroup_Map pbgm join EST_Estimate e on pbgm.EST_Estimate_ID = e.EST_Estimate_ID
	where pbgm.EST_PolybagGroup_ID = @EST_PolybagGroup_ID)
end

GO

GRANT  EXECUTE  ON [dbo].[PolybagGroupRunDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
GO
IF OBJECT_ID('dbo.PolybagInkjetCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PolybagInkjetCost'
	DROP FUNCTION dbo.PolybagInkjetCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PolybagInkjetCost') IS NOT NULL
		PRINT '***********Drop of dbo.PolybagInkjetCost FAILED.'
END
GO
PRINT 'Creating dbo.PolybagInkjetCost'
GO

create function dbo.PolybagInkjetCost(@EST_Polybag_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Polybag_ID
*
* DESCRIPTION:
*	Returns the polybag's total inkjet cost.
*
* TABLES:
*   Table Name                      Access
*   ==========                      ======
*   EST_Component                   READ
*   EST_AssemDistribOptions         READ
*   VND_MailHouseRate               READ
*   EST_PackageComponentMapping     READ
*   EST_Package                     READ
*   EST_PackagePolybag_Map          READ
*   EST_Polybag                     READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total inkjet cost for polybag.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/18/2007      BJS             Initial Creation 
*
*/
begin
	declare @EST_Estimate_ID bigint, @EST_Component_ID bigint, @InkjetCPM money, @SoloQuantity int, @PrimaryPolybagQuantity int, @retval money

	select @EST_Component_ID = dbo.PolybagPrimaryHost(@EST_Polybag_ID)
	select @EST_Estimate_ID = EST_Estimate_ID
	from EST_Component
	where EST_Component_ID = @EST_Component_ID

	select @InkjetCPM = mh.InkjetRate
	from EST_Component c join EST_AssemDistribOptions ad on c.EST_Estimate_ID = ad.EST_Estimate_ID
		join VND_MailHouseRate mh on ad.MailHouse_ID = mh.VND_MailHouseRate_ID
	where c.EST_Component_ID = @EST_Component_ID

	select @SoloQuantity = sum(p.SoloQuantity)
	from EST_PackageComponentMapping pcm join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
	where pcm.EST_Component_ID = @EST_Component_ID

	select @PrimaryPolybagQuantity = dbo.EstimatePrimaryPolybagQuantity(@EST_Estimate_ID)

	select @retval = (@InkjetCPM * pb.Quantity / 1000) * pb.Quantity / (isnull(@SoloQuantity, 0) + isnull(@PrimaryPolybagQuantity, 0))
	from EST_Polybag pb
	where pb.EST_Polybag_ID = @EST_Polybag_ID

	return(@retval)
end
GO

GRANT  EXECUTE  ON [dbo].[PolybagInkjetCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PolybagInkjetMakereadyCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PolybagInkjetMakereadyCost'
	DROP FUNCTION dbo.PolybagInkjetMakereadyCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PolybagInkjetMakereadyCost') IS NOT NULL
		PRINT '***********Drop of dbo.PolybagInkjetMakereadyCost FAILED.'
END
GO
PRINT 'Creating dbo.PolybagInkjetMakereadyCost'
GO

create function dbo.PolybagInkjetMakereadyCost(@EST_Polybag_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Polybag_ID
*
* DESCRIPTION:
*	Returns the polybag's total inkjet makeready cost.
*
* TABLES:
*   Table Name                      Access
*   ==========                      ======
*   EST_Component                   READ
*   EST_AssemDistribOptions         READ
*   VND_MailHouseRate               READ
*   EST_PackageComponentMapping     READ
*   EST_Package                     READ
*   EST_PackagePolybag_Map          READ
*   EST_Polybag                     READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total inkjet makeready cost for polybag.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/18/2007      BJS             Initial Creation 
*
*/
begin
	declare @EST_Estimate_ID bigint, @EST_Component_ID bigint, @InkjetMakereadyCost money, @SoloQuantity int, @PrimaryPolybagQuantity int, @retval money

	select @EST_Component_ID = dbo.PolybagPrimaryHost(@EST_Polybag_ID)
	select @EST_Estimate_ID = EST_Estimate_ID
	from EST_Component
	where EST_Component_ID = @EST_Component_ID

	select @InkjetMakereadyCost = mh.InkjetMakeready * c.NumberOfPlants
	from EST_Component c join EST_AssemDistribOptions ad on c.EST_Estimate_ID = ad.EST_Estimate_ID
		join VND_MailHouseRate mh on ad.MailHouse_ID = mh.VND_MailHouseRate_ID
	where c.EST_Component_ID = @EST_Component_ID

	select @SoloQuantity = sum(p.SoloQuantity)
	from EST_PackageComponentMapping pcm join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
	where pcm.EST_Component_ID = @EST_Component_ID

	select @PrimaryPolybagQuantity = dbo.EstimatePrimaryPolybagQuantity(@EST_Estimate_ID)

	select @retval = @InkjetMakereadyCost * pb.Quantity / (isnull(@SoloQuantity, 0) + isnull(@PrimaryPolybagQuantity, 0))
	from EST_Polybag pb
	where pb.EST_Polybag_ID = @EST_Polybag_ID

	return(@retval)
end
GO

GRANT  EXECUTE  ON [dbo].[PolybagInkjetMakereadyCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PolybagMailHouseAdminFee') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PolybagMailHouseAdminFee'
	DROP FUNCTION dbo.PolybagMailHouseAdminFee
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PolybagMailHouseAdminFee') IS NOT NULL
		PRINT '***********Drop of dbo.PolybagMailHouseAdminFee FAILED.'
END
GO
PRINT 'Creating dbo.PolybagMailHouseAdminFee'
GO

create function dbo.PolybagMailHouseAdminFee(@EST_Polybag_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Component_ID - A host component ID.
*
* DESCRIPTION:
*	Returns the total mailhouse admin fee for a polybag.
*
* TABLES:
*   Table Name                      Access
*   ==========                      ======
*   EST_Component                   READ
*   EST_AssemDistribOptions         READ
*   EST_PackageComponentMapping     READ
*   EST_Package                     READ
*   EST_PackagePolybag_Map          READ
*   EST_Polybag                     READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Mail House Admin Fee for a Polybag
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/12/2007      BJS             Initial Creation 
*
*/
begin
	declare @EST_Estimate_ID bigint, @EST_Component_ID bigint, @MailHouseAdminFee money, @SoloQuantity int, @PrimaryPolybagQuantity int, @retval money

	select @EST_Component_ID = dbo.PolybagPrimaryHost(@EST_Polybag_ID)
	select @EST_Estimate_ID = EST_Estimate_ID
	from EST_Component
	where EST_Component_ID = @EST_Component_ID

	select @MailHouseAdminFee = mh.AdminFee
	from EST_Component c join EST_AssemDistribOptions ad on c.EST_Estimate_ID = ad.EST_Estimate_ID
		join VND_MailHouseRate mh on ad.MailHouse_ID = mh.VND_MailHouseRate_ID
	where c.EST_Component_ID = @EST_Component_ID

	select @SoloQuantity = sum(p.SoloQuantity)
	from EST_PackageComponentMapping pcm join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
	where pcm.EST_Component_ID = @EST_Component_ID

	select @PrimaryPolybagQuantity = dbo.EstimatePrimaryPolybagQuantity(@EST_Estimate_ID)

	select @retval = @MailHouseAdminFee * pb.Quantity / (@SoloQuantity + @PrimaryPolybagQuantity)
	from EST_Polybag pb
	where pb.EST_Polybag_ID = @EST_Polybag_ID

	return(@retval)
end
GO

GRANT  EXECUTE  ON [dbo].[PolybagMailHouseAdminFee]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PolybagMailHouseGlueTackCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PolybagMailHouseGlueTackCost'
	DROP FUNCTION dbo.PolybagMailHouseGlueTackCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PolybagMailHouseGlueTackCost') IS NOT NULL
		PRINT '***********Drop of dbo.PolybagMailHouseGlueTackCost FAILED.'
END
GO
PRINT 'Creating dbo.PolybagMailHouseGlueTackCost'
GO

create function dbo.PolybagMailHouseGlueTackCost(@EST_Polybag_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Polybag_ID
*
* DESCRIPTION:
*	Returns the polybag's glue tack cost.
*
* TABLES:
*   Table Name                      Access
*   ==========                      ======
*   EST_Component                   READ
*   EST_AssemDistribOptions         READ
*   EST_PackageComponentMapping     READ
*   EST_Package                     READ
*   EST_PackagePolybag_Map          READ
*   EST_Polybag                     READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Mail House glue tack cost for polybag.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/12/2007      BJS             Initial Creation 
*
*/
begin
	declare @EST_Estimate_ID bigint, @EST_Component_ID bigint, @GlueTackCPM money, @SoloQuantity int, @PrimaryPolybagQuantity int, @retval money

	select @EST_Component_ID = dbo.PolybagPrimaryHost(@EST_Polybag_ID)
	select @EST_Estimate_ID = EST_Estimate_ID
	from EST_Component
	where EST_Component_ID = @EST_Component_ID

	select @GlueTackCPM = mh.GlueTackRate
	from EST_Component c join EST_AssemDistribOptions ad on c.EST_Estimate_ID = ad.EST_Estimate_ID
		join VND_MailHouseRate mh on ad.MailHouse_ID = mh.VND_MailHouseRate_ID
	where c.EST_Component_ID = @EST_Component_ID and ad.UseGlueTack = 1

	select @SoloQuantity = sum(p.SoloQuantity)
	from EST_PackageComponentMapping pcm join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
	where pcm.EST_Component_ID = @EST_Component_ID

	select @PrimaryPolybagQuantity = dbo.EstimatePrimaryPolybagQuantity(@EST_Estimate_ID)

	select @retval = (@GlueTackCPM * pb.Quantity / 1000) * pb.Quantity / (isnull(@SoloQuantity, 0) + isnull(@PrimaryPolybagQuantity, 0))
	from EST_Polybag pb
	where pb.EST_Polybag_ID = @EST_Polybag_ID

	return(@retval)
end
GO

GRANT  EXECUTE  ON [dbo].[PolybagMailHouseGlueTackCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PolybagMailHouseLetterInsertionCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PolybagMailHouseLetterInsertionCost'
	DROP FUNCTION dbo.PolybagMailHouseLetterInsertionCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PolybagMailHouseLetterInsertionCost') IS NOT NULL
		PRINT '***********Drop of dbo.PolybagMailHouseLetterInsertionCost FAILED.'
END
GO
PRINT 'Creating dbo.PolybagMailHouseLetterInsertionCost'
GO

create function dbo.PolybagMailHouseLetterInsertionCost(@EST_Polybag_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Polybag_ID
*
* DESCRIPTION:
*	Returns the polybag's total letter insertion cost.
*
* TABLES:
*   Table Name                      Access
*   ==========                      ======
*   EST_Component                   READ
*   EST_AssemDistribOptions         READ
*   EST_PackageComponentMapping     READ
*   EST_Package                     READ
*   EST_PackagePolybag_Map          READ
*   EST_Polybag                     READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Mail House letter insertion cost for polybag.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/12/2007      BJS             Initial Creation 
*
*/
begin
	declare @EST_Estimate_ID bigint, @EST_Component_ID bigint, @LetterInsertionCPM money, @SoloQuantity int, @PrimaryPolybagQuantity int, @retval money

	select @EST_Component_ID = dbo.PolybagPrimaryHost(@EST_Polybag_ID)
	select @EST_Estimate_ID = EST_Estimate_ID
	from EST_Component
	where EST_Component_ID = @EST_Component_ID

	select @LetterInsertionCPM = mh.LetterInsertionRate
	from EST_Component c join EST_AssemDistribOptions ad on c.EST_Estimate_ID = ad.EST_Estimate_ID
		join VND_MailHouseRate mh on ad.MailHouse_ID = mh.VND_MailHouseRate_ID
	where c.EST_Component_ID = @EST_Component_ID and ad.UseLetterInsertion = 1

	select @SoloQuantity = sum(p.SoloQuantity)
	from EST_PackageComponentMapping pcm join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
	where pcm.EST_Component_ID = @EST_Component_ID

	select @PrimaryPolybagQuantity = dbo.EstimatePrimaryPolybagQuantity(@EST_Estimate_ID)

	select @retval = (@LetterInsertionCPM * pb.Quantity / 1000) * pb.Quantity / (isnull(@SoloQuantity, 0) + isnull(@PrimaryPolybagQuantity, 0))
	from EST_Polybag pb
	where pb.EST_Polybag_ID = @EST_Polybag_ID

	return(@retval)
end
GO

GRANT  EXECUTE  ON [dbo].[PolybagMailHouseLetterInsertionCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PolybagMailHouseOtherDirectMailHandlingCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PolybagMailHouseOtherDirectMailHandlingCost'
	DROP FUNCTION dbo.PolybagMailHouseOtherDirectMailHandlingCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PolybagMailHouseOtherDirectMailHandlingCost') IS NOT NULL
		PRINT '***********Drop of dbo.PolybagMailHouseOtherDirectMailHandlingCost FAILED.'
END
GO
PRINT 'Creating dbo.PolybagMailHouseOtherDirectMailHandlingCost'
GO

create function dbo.PolybagMailHouseOtherDirectMailHandlingCost(@EST_Polybag_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Polybag_ID
*
* DESCRIPTION:
*	Returns the polybag's total other direct mail handling cost.
*
* TABLES:
*   Table Name                      Access
*   ==========                      ======
*   EST_Component                   READ
*   EST_AssemDistribOptions         READ
*   EST_PackageComponentMapping     READ
*   EST_Package                     READ
*   EST_PackagePolybag_Map          READ
*   EST_Polybag                     READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Mail House other direct mail handling cost for polybag.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/12/2007      BJS             Initial Creation 
*
*/
begin
	declare @EST_Estimate_ID bigint, @EST_Component_ID bigint, @MailHouseOtherDirectMailHandlingCost money, @SoloQuantity int, @PrimaryPolybagQuantity int, @retval money

	select @EST_Component_ID = dbo.PolybagPrimaryHost(@EST_Polybag_ID)
	select @EST_Estimate_ID = EST_Estimate_ID
	from EST_Component
	where EST_Component_ID = @EST_Component_ID

	select @MailHouseOtherDirectMailHandlingCost = ad.MailHouseOtherHandling
	from EST_Component c join EST_AssemDistribOptions ad on c.EST_Estimate_ID = ad.EST_Estimate_ID
	where c.EST_Component_ID = @EST_Component_ID

	select @SoloQuantity = sum(p.SoloQuantity)
	from EST_PackageComponentMapping pcm join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
	where pcm.EST_Component_ID = @EST_Component_ID

	select @PrimaryPolybagQuantity = dbo.EstimatePrimaryPolybagQuantity(@EST_Estimate_ID)

	select @retval = (@MailHouseOtherDirectMailHandlingCost * pb.Quantity / 1000) * pb.Quantity / (isnull(@SoloQuantity, 0) + isnull(@PrimaryPolybagQuantity, 0))
	from EST_Polybag pb
	where pb.EST_Polybag_ID = @EST_Polybag_ID

	return(@retval)
end
GO

GRANT  EXECUTE  ON [dbo].[PolybagMailHouseOtherDirectMailHandlingCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PolybagMailHouseTabbingCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PolybagMailHouseTabbingCost'
	DROP FUNCTION dbo.PolybagMailHouseTabbingCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PolybagMailHouseTabbingCost') IS NOT NULL
		PRINT '***********Drop of dbo.PolybagMailHouseTabbingCost FAILED.'
END
GO
PRINT 'Creating dbo.PolybagMailHouseTabbingCost'
GO

create function dbo.PolybagMailHouseTabbingCost(@EST_Polybag_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Polybag_ID
*
* DESCRIPTION:
*	Returns the polybag's total tabbing cost.
*
* TABLES:
*   Table Name                      Access
*   ==========                      ======
*   EST_Component                   READ
*   EST_AssemDistribOptions         READ
*   VND_MailHouseRate               READ
*   EST_PackageComponentMapping     READ
*   EST_Package                     READ
*   EST_PackagePolybag_Map          READ
*   EST_Polybag                     READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Mail House tabbing cost for polybag.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/12/2007      BJS             Initial Creation 
*
*/
begin
	declare @EST_Estimate_ID bigint, @EST_Component_ID bigint, @TabbingCPM money, @SoloQuantity int, @PrimaryPolybagQuantity int, @retval money

	select @EST_Component_ID = dbo.PolybagPrimaryHost(@EST_Polybag_ID)
	select @EST_Estimate_ID = EST_Estimate_ID
	from EST_Component
	where EST_Component_ID = @EST_Component_ID

	select @TabbingCPM = mh.TabbingRate
	from EST_Component c join EST_AssemDistribOptions ad on c.EST_Estimate_ID = ad.EST_Estimate_ID
		join VND_MailHouseRate mh on ad.MailHouse_ID = mh.VND_MailHouseRate_ID
	where c.EST_Component_ID = @EST_Component_ID and ad.UseTabbing = 1

	select @SoloQuantity = sum(p.SoloQuantity)
	from EST_PackageComponentMapping pcm join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
	where pcm.EST_Component_ID = @EST_Component_ID

	select @PrimaryPolybagQuantity = dbo.EstimatePrimaryPolybagQuantity(@EST_Estimate_ID)

	select @retval = (@TabbingCPM * pb.Quantity / 1000) * pb.Quantity / (isnull(@SoloQuantity, 0) + isnull(@PrimaryPolybagQuantity, 0))
	from EST_Polybag pb
	where pb.EST_Polybag_ID = @EST_Polybag_ID

	return(@retval)
end
GO

GRANT  EXECUTE  ON [dbo].[PolybagMailHouseTabbingCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PolybagMailHouseTimeValueSlipsCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PolybagMailHouseTimeValueSlipsCost'
	DROP FUNCTION dbo.PolybagMailHouseTimeValueSlipsCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PolybagMailHouseTimeValueSlipsCost') IS NOT NULL
		PRINT '***********Drop of dbo.PolybagMailHouseTimeValueSlipsCost FAILED.'
END
GO
PRINT 'Creating dbo.PolybagMailHouseTimeValueSlipsCost'
GO

create function dbo.PolybagMailHouseTimeValueSlipsCost(@EST_Polybag_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Polybag_ID
*
* DESCRIPTION:
*	Returns the polybag's total time value slips cost.
*
* TABLES:
*   Table Name                      Access
*   ==========                      ======
*   EST_Component                   READ
*   EST_AssemDistribOptions         READ
*   EST_PackageComponentMapping     READ
*   EST_Package                     READ
*   EST_PackagePolybag_Map          READ
*   EST_Polybag                     READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Mail House time value slips cost for polybag.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/12/2007      BJS             Initial Creation 
*
*/
begin
	declare @EST_Estimate_ID bigint, @EST_Component_ID bigint, @TimeValueSlipsCPM money, @SoloQuantity int, @PrimaryPolybagQuantity int, @retval money
	
	select @EST_Component_ID = dbo.PolybagPrimaryHost(@EST_Polybag_ID)
	select @EST_Estimate_ID = EST_Estimate_ID
	from EST_Component
	where EST_Component_ID = @EST_Component_ID

	select @TimeValueSlipsCPM = mh.TimeValueSlips
	from EST_Component c join EST_AssemDistribOptions ad on c.EST_Estimate_ID = ad.EST_Estimate_ID
		join VND_MailHouseRate mh on ad.MailHouse_ID = mh.VND_MailHouseRate_ID
	where c.EST_Component_ID = @EST_Component_ID

	select @SoloQuantity = sum(p.SoloQuantity)
	from EST_PackageComponentMapping pcm join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
	where pcm.EST_Component_ID = @EST_Component_ID

	select @PrimaryPolybagQuantity = dbo.EstimatePrimaryPolybagQuantity(@EST_Estimate_ID)

	select @retval = (@TimeValueSlipsCPM * pb.Quantity / 1000) * pb.Quantity / (isnull(@SoloQuantity, 0) + isnull(@PrimaryPolybagQuantity, 0))
	from EST_Polybag pb
	where pb.EST_Polybag_ID = @EST_Polybag_ID

	return(@retval)
end
GO

GRANT  EXECUTE  ON [dbo].[PolybagMailHouseTimeValueSlipsCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PolybagMailListCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PolybagMailListCost'
	DROP FUNCTION dbo.PolybagMailListCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PolybagMailListCost') IS NOT NULL
		PRINT '***********Drop of dbo.PolybagMailListCost FAILED.'
END
GO
PRINT 'Creating dbo.PolybagMailListCost'
GO

create function dbo.PolybagMailListCost(@EST_Polybag_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Polybag_ID
*
* DESCRIPTION:
*	Returns the polybag's mail list cost.
*
* TABLES:
*   Table Name                      Access
*   ==========                      ======
*   EST_Component                   READ
*   EST_AssemDistribOptions         READ
*   EST_PackageComponentMapping     READ
*   EST_Package                     READ
*   EST_PackagePolybag_Map          READ
*   EST_Polybag                     READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total Mail House time value slips cost for polybag.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/12/2007      BJS             Initial Creation 
*
*/
begin
	declare @retval money

	declare @EST_Estimate_ID bigint, @EST_Component_ID bigint,
		@TotalMailQuantity int, @ExternalMailQuantity int, @InternalMailQuantity int, @PrimaryMailQuantity int,
		@InternalMailCPM money, @ExternalMailCPM money, @BlendedMailListCPM money

	/* Determine the Primary Host Component */
	select @EST_Component_ID = dbo.PolybagPrimaryHost(@EST_Polybag_ID)

	select @EST_Estimate_ID = EST_Estimate_ID
	from EST_Component
	where EST_Component_ID = @EST_Component_ID

	select @ExternalMailQuantity = ad.ExternalMailQty, @ExternalMailCPM = ad.ExternalMailCPM, @InternalMailCPM = isnull(ml.InternalListRate, 0)
	from EST_AssemDistribOptions ad
		left join VND_MailListResourceRate ml on ad.MailListResource_ID = ml.VND_MailListResourceRate_ID
	where ad.EST_Estimate_ID = @EST_Estimate_ID

	select @PrimaryMailQuantity = dbo.EstimateSoloAndPrimaryPolybagQuantity(@EST_Estimate_ID)
	select @InternalMailQuantity = @PrimaryMailQuantity - @ExternalMailQuantity

	/* Determine the Blended Rate */
	set @BlendedMailListCPM =
		case
			when @PrimaryMailQuantity = 0 then 0
			else @InternalMailCPM
					* cast(@InternalMailQuantity as decimal)
					/ cast(@PrimaryMailQuantity as decimal)
				+ @ExternalMailCPM
					* cast(@ExternalMailQuantity as decimal)
					/ cast(@PrimaryMailQuantity as decimal)
		end

	select @retval = @BlendedMailListCPM * Quantity / 1000
	from EST_Polybag
	where EST_Polybag_ID = @EST_Polybag_ID

	return(@retval)
end
GO

GRANT  EXECUTE  ON [dbo].[PolybagMailListCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PolybagMailTrackingCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PolybagMailTrackingCost'
	DROP FUNCTION dbo.PolybagMailTrackingCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PolybagMailTrackingCost') IS NOT NULL
		PRINT '***********Drop of dbo.PolybagMailTrackingCost FAILED.'
END
GO
PRINT 'Creating dbo.PolybagMailTrackingCost'
GO

create function dbo.PolybagMailTrackingCost(@EST_Polybag_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Polybag_ID
*
* DESCRIPTION:
*	Returns the polybag's total mail tracking cost.
*
* TABLES:
*   Table Name                      Access
*   ==========                      ======
*   EST_Component                   READ
*   EST_AssemDistribOptions         READ
*   VND_MailTrackingRate            READ
*   EST_PackageComponentMapping     READ
*   EST_Package                     READ
*   EST_PackagePolybag_Map          READ
*   EST_Polybag                     READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total mail tracking cost for polybag.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/12/2007      BJS             Initial Creation 
* 11/12/2007      JRH             Multiply @MailTrackingCPM by total PB Qty.
*
*/
begin
	declare @EST_Estimate_ID bigint, @EST_Component_ID bigint, @MailTrackingCPM money, @PrimaryPolybagQuantity int, @retval money

	select @EST_Component_ID = dbo.PolybagPrimaryHost(@EST_Polybag_ID)
	select @EST_Estimate_ID = EST_Estimate_ID
	from EST_Component
	where EST_Component_ID = @EST_Component_ID

	select @MailTrackingCPM = mr.MailTracking
	from EST_Component c join EST_AssemDistribOptions ad on c.EST_Estimate_ID = ad.EST_Estimate_ID
		join VND_MailTrackingRate mr on ad.MailTracking_ID = mr.VND_MailTrackingRate_ID
	where c.EST_Component_ID = @EST_Component_ID and ad.UseMailTracking = 1

	select @PrimaryPolybagQuantity = isnull(dbo.EstimatePrimaryPolybagQuantity(@EST_Estimate_ID), 0)


	select @retval = case when @PrimaryPolybagQuantity = 0 then 0
				else (@MailTrackingCPM * cast(@PrimaryPolybagQuantity as decimal(18,6)) / cast(1000 as decimal(18,6))) * pb.Quantity / @PrimaryPolybagQuantity
			end
	from EST_Polybag pb
	where pb.EST_Polybag_ID = @EST_Polybag_ID

	return(@retval)
end
GO

GRANT  EXECUTE  ON [dbo].[PolybagMailTrackingCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PolybagOtherFreightCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PolybagOtherFreightCost'
	DROP FUNCTION dbo.PolybagOtherFreightCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PolybagOtherFreightCost') IS NOT NULL
		PRINT '***********Drop of dbo.PolybagOtherFreightCost FAILED.'
END
GO
PRINT 'Creating dbo.PolybagOtherFreightCost'
GO

create function dbo.PolybagOtherFreightCost(@EST_Polybag_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Polybag_ID
*
* DESCRIPTION:
*	Returns the polybag's total other freight cost.
*
* TABLES:
*   Table Name                      Access
*   ==========                      ======
*   EST_Component                   READ
*   EST_AssemDistribOptions         READ
*   EST_PackageComponentMapping     READ
*   EST_Package                     READ
*   EST_PackagePolybag_Map          READ
*   EST_Polybag                     READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total other freight cost for polybag.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/18/2007      BJS             Initial Creation 
*
*/
begin
	declare @EST_Estimate_ID bigint, @EST_Component_ID bigint, @OtherFreightCost money, @SoloQuantity int, @PrimaryPolybagQuantity int, @retval money

	select @EST_Component_ID = dbo.PolybagPrimaryHost(@EST_Polybag_ID)
	select @EST_Estimate_ID = EST_Estimate_ID
	from EST_Component
	where EST_Component_ID = @EST_Component_ID

	select @OtherFreightCost = ad.OtherFreight
	from EST_Component c join EST_AssemDistribOptions ad on c.EST_Estimate_ID = ad.EST_Estimate_ID
	where c.EST_Component_ID = @EST_Component_ID

	select @SoloQuantity = sum(p.SoloQuantity)
	from EST_PackageComponentMapping pcm join EST_Package p on pcm.EST_Package_ID = p.EST_Package_ID
	where pcm.EST_Component_ID = @EST_Component_ID

	select @PrimaryPolybagQuantity = dbo.EstimatePrimaryPolybagQuantity(@EST_Estimate_ID)

	select @retval = @OtherFreightCost * pb.Quantity / (isnull(@SoloQuantity, 0) + isnull(@PrimaryPolybagQuantity, 0))
	from EST_Polybag pb
	where pb.EST_Polybag_ID = @EST_Polybag_ID

	return(@retval)
end
GO

GRANT  EXECUTE  ON [dbo].[PolybagOtherFreightCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PolybagPostalDropCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PolybagPostalDropCost'
	DROP FUNCTION dbo.PolybagPostalDropCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PolybagPostalDropCost') IS NOT NULL
		PRINT '***********Drop of dbo.PolybagPostalDropCost FAILED.'
END
GO
PRINT 'Creating dbo.PolybagPostalDropCost'
GO

create function dbo.PolybagPostalDropCost(@EST_Polybag_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Polybag_ID
*
* DESCRIPTION:
*	Returns the polybag's total postal drop cost.
*
* TABLES:
*   Table Name                      Access
*   ==========                      ======
*   EST_Component                   READ
*   EST_AssemDistribOptions         READ
*
* PROCEDURES CALLED:
*   PolybagPrimaryHost
*   PolybagWeightIncludeBagWeight
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total postal drop cost for polybag.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/18/2007      BJS             Initial Creation 
* 07/23/2007      BJS             Now references PolybagWeightIncludeBagWeight to include the weight
*                                   of the plastic bag.
*                                 Removed calculation for fuel surcharge
* 07/24/2007      JRH             Get PostalDropCWT from VND_MailhouseRate
* 10/15/2007      BJS             When using PostalDropCWT, calculate totalweight / 100 * rate
*
*/
begin
	declare
		@EST_Estimate_ID  bigint,
		@EST_Component_ID bigint,
		@Quantity         int,
		@PostalDropCost   money,
		@PostalDropCWT    money,
		@retval           money

	-- Get the values needed to perform the calculations
	select @EST_Component_ID = dbo.PolybagPrimaryHost(@EST_Polybag_ID)

	select @EST_Estimate_ID = EST_Estimate_ID
	from EST_Component
	where EST_Component_ID = @EST_Component_ID

	select @Quantity = Quantity
	from EST_Polybag
	where EST_Polybag_ID = @EST_Polybag_ID

	-- The Primary Host is using a flat postal drop cost
	if exists(select 1 from EST_AssemDistribOptions where EST_Estimate_ID = @EST_Estimate_ID and PostalDropFlat is not null) begin
		select @PostalDropCost = PostalDropFlat
		from EST_AssemDistribOptions
		where EST_Estimate_ID = @EST_Estimate_ID

		set @retval = @PostalDropCost * dbo.PolybagWeightIncludeBagWeight(@EST_Polybag_ID) / dbo.EstimateSoloAndPrimaryPolybagWeight(@EST_Estimate_ID)
	end
	-- The Primary Host is using the Mailhouse Postal Drop rate
	else begin
		select @PostalDropCWT = mh.PostalDropCWT
		from EST_Component c 
			join EST_AssemDistribOptions ad on c.EST_Estimate_ID = ad.EST_Estimate_ID
			join VND_MailhouseRate mh on ad.Mailhouse_ID = mh.VND_MailhouseRate_ID
		where c.EST_Component_ID = @EST_Component_ID

		select @retval = isnull(@PostalDropCWT, 0) * dbo.PolybagWeightIncludeBagWeight(@EST_Polybag_ID) * @Quantity / 100
	end

	return(@retval)
end
GO

GRANT  EXECUTE  ON [dbo].[PolybagPostalDropCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PolybagPostalDropFuelSurchargeCost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PolybagPostalDropFuelSurchargeCost'
	DROP FUNCTION dbo.PolybagPostalDropFuelSurchargeCost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PolybagPostalDropFuelSurchargeCost') IS NOT NULL
		PRINT '***********Drop of dbo.PolybagPostalDropFuelSurchargeCost FAILED.'
END
GO
PRINT 'Creating dbo.PolybagPostalDropFuelSurchargeCost'
GO

create function dbo.PolybagPostalDropFuelSurchargeCost(@EST_Polybag_ID bigint)
returns money
/*
* PARAMETERS:
*	EST_Polybag_ID
*
* DESCRIPTION:
*	Returns the polybag's total postal drop fuel surcharge cost.
*
* TABLES:
*   Table Name                      Access
*   ==========                      ======
*   EST_Component                   READ
*   EST_AssemDistribOptions         READ
*
* PROCEDURES CALLED:
*   dbo.PolybagPrimaryHost
*   dbo.PolybagWeightIncludeBagWeight
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total postal drop fuel surcharge cost for polybag.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/23/2007      BJS             Initial Creation 
* 07/24/2007      JRH             Get PostalDropCWT from VND_MailhouseRate
* 10/15/2007      BJS             References PolybagPostalDropCost
*
*/
begin
	declare @EST_Component_ID bigint, @PostalDropFuelSurcharge money, @retval money

	select @EST_Component_ID = dbo.PolybagPrimaryHost(@EST_Polybag_ID)

	select @PostalDropFuelSurcharge = ad.MailFuelSurcharge
	from EST_Component c 
		join EST_AssemDistribOptions ad on c.EST_Estimate_ID = ad.EST_Estimate_ID
		join VND_MailhouseRate mh on ad.Mailhouse_ID = mh.VND_MailhouseRate_ID
	where c.EST_Component_ID = @EST_Component_ID

	select @retval = isnull(dbo.PolybagPostalDropCost(@EST_Polybag_ID), 0) * isnull(@PostalDropFuelSurcharge, 0)

	return(@retval)
end
GO

GRANT  EXECUTE  ON [dbo].[PolybagPostalDropFuelSurchargeCost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PolybagPrimaryEstimate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PolybagPrimaryEstimate'
	DROP FUNCTION dbo.PolybagPrimaryEstimate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PolybagPrimaryEstimate') IS NOT NULL
		PRINT '***********Drop of dbo.PolybagPrimaryEstimate FAILED.'
END
GO
PRINT 'Creating dbo.PolybagPrimaryEstimate'
GO

create function dbo.PolybagPrimaryEstimate(@EST_Polybag_ID bigint)
returns bigint
/*
* PARAMETERS:
*	EST_Polybag_ID
*
* DESCRIPTION:
*	Returns the primary Estimate ID for the specified polybag.
*
* TABLES:
*   Table Name                      Access
*   ==========                      ======
*   EST_Component                   READ
*   EST_Polybag                     READ
*   EST_PolybagGroup                READ
*   EST_EstimatePolybagGroup_Map    READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Polybag Primary Host Component
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 10/17/2007      BJS             Initial Creation - Logic Copied from PolybagPrimaryHost
*
*/
as
begin
	declare @retval bigint
	select top 1 @retval = pbgm.EST_Estimate_ID
	from EST_Polybag pb join EST_PolybagGroup pbg on pb.EST_PolybagGroup_ID = pbg.EST_PolybagGroup_ID
		join EST_EstimatePolybagGroup_Map pbgm on pbg.EST_PolybagGroup_ID = pbgm.EST_PolybagGroup_ID
	where pb.EST_Polybag_ID = @EST_Polybag_ID
	order by pbgm.EstimateOrder

	return(@retval)
end
GO

GRANT  EXECUTE  ON [dbo].[PolybagPrimaryEstimate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PolybagPrimaryHost') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PolybagPrimaryHost'
	DROP FUNCTION dbo.PolybagPrimaryHost
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PolybagPrimaryHost') IS NOT NULL
		PRINT '***********Drop of dbo.PolybagPrimaryHost FAILED.'
END
GO
PRINT 'Creating dbo.PolybagPrimaryHost'
GO

create function dbo.PolybagPrimaryHost(@EST_Polybag_ID bigint)
returns bigint
/*
* PARAMETERS:
*	EST_Polybag_ID
*
* DESCRIPTION:
*	Returns the primary host component ID for the specified polybag.
*
* TABLES:
*   Table Name                      Access
*   ==========                      ======
*   EST_Component                   READ
*   EST_Polybag                     READ
*   EST_PolybagGroup                READ
*   EST_EstimatePolybagGroup_Map    READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	Polybag Primary Host Component
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/12/2007      BJS             Initial Creation 
*
*/
as
begin
	declare @retval bigint
	select top 1 @retval = EST_Component_ID
	from EST_Polybag pb join EST_PolybagGroup pbg on pb.EST_PolybagGroup_ID = pbg.EST_PolybagGroup_ID
		join EST_EstimatePolybagGroup_Map pbgm on pbg.EST_PolybagGroup_ID = pbgm.EST_PolybagGroup_ID
		join EST_Component c on pbgm.EST_Estimate_ID = c.EST_Estimate_ID
	where pb.EST_Polybag_ID = @EST_Polybag_ID and c.EST_ComponentType_ID = 1
	order by pbgm.EstimateOrder

	return(@retval)
end
GO

GRANT  EXECUTE  ON [dbo].[PolybagPrimaryHost]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PolybagWeight') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PolybagWeight'
	DROP FUNCTION dbo.PolybagWeight
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PolybagWeight') IS NOT NULL
		PRINT '***********Drop of dbo.PolybagWeight FAILED.'
END
GO
PRINT 'Creating dbo.PolybagWeight'
GO

create function dbo.PolybagWeight(@EST_Polybag_ID bigint)
/*
* PARAMETERS:
*	EST_Polybag_ID
*
* DESCRIPTION:
*	Returns the sum of the package weights contained by the polybag (not including the bag weight).
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_PackagePolyBag_Map
*   EST_PolyBag
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	The polybag weight
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 09/28/2007      BJS             Initial Creation 
*
*/
returns decimal(12,6)
as
begin
	return(select sum(dbo.PackageWeight(ppm.EST_Package_ID))
		from EST_PackagePolybag_Map ppm
		where ppm.EST_Polybag_ID = @EST_Polybag_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[PolybagWeight]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PolybagWeightIncludeBagWeight') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PolybagWeightIncludeBagWeight'
	DROP FUNCTION dbo.PolybagWeightIncludeBagWeight
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PolybagWeightIncludeBagWeight') IS NOT NULL
		PRINT '***********Drop of dbo.PolybagWeightIncludeBagWeight FAILED.'
END
GO
PRINT 'Creating dbo.PolybagWeightIncludeBagWeight'
GO

create function dbo.PolybagWeightIncludeBagWeight(@EST_Polybag_ID bigint)
returns decimal(12,6)
/*
* PARAMETERS:
*	EST_Polybag_ID
*
* DESCRIPTION:
*	Returns the polybag weight including the weight of the bag (default is .0213).
*
* TABLES:
*   Table Name                      Access
*   ==========                      ======
*   EST_Component                   READ
*   EST_PackageComponentMapping     READ
*   EST_Package                     READ
*   EST_PackagePolybag_Map          READ
*   EST_Polybag                     READ
*
* PROCEDURES CALLED:
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	The polybag weight including the weight of the bag.
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/23/2007      BJS             Initial Creation 
*
*/
as
begin
	declare @TotalPackageWeight decimal(12,6)
	declare @PolybagBagWeight decimal(12,6)

	select @TotalPackageWeight = sum(dbo.PackageWeight(ppm.EST_Package_ID))
	from EST_PackagePolybag_Map ppm
	where ppm.EST_Polybag_ID = @EST_Polybag_ID

	select @PolybagBagWeight = prt.PolybagBagWeight
	from EST_Polybag pb join EST_PolybagGroup pbg on pb.EST_PolybagGroup_ID = pbg.EST_PolybagGroup_ID
		join VND_Printer prt on pbg.VND_Printer_ID = prt.VND_Printer_ID
	where pb.EST_Polybag_ID = @EST_Polybag_ID

	return(isnull(@TotalPackageWeight, 0) + isnull(@PolybagBagWeight, 0))
end
GO

GRANT  EXECUTE  ON [dbo].[PolybagWeightIncludeBagWeight]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.PubRateMapInsertQuantityByInsertDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubRateMapInsertQuantityByInsertDate'
	DROP FUNCTION dbo.PubRateMapInsertQuantityByInsertDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubRateMapInsertQuantityByInsertDate') IS NOT NULL
		PRINT '***********Drop of dbo.PubRateMapInsertQuantityByInsertDate FAILED.'
END
GO
PRINT 'Creating dbo.PubRateMapInsertQuantityByInsertDate'
GO

CREATE function dbo.PubRateMapInsertQuantityByInsertDate(@InsertDate datetime, @PUB_PubQuantityType_ID int, @PUB_PubRate_Map_ID bigint)
/*
* PARAMETERS:
*	InsertDate
*	PUB_PubQuantityType_ID
*	PUB_PubRate_Map_ID
*
* DESCRIPTION:
*	Returns the insert quantity for a Pub-Location on a given insert date.
*
* TABLES:
*   Table Name                   Access
*   ==========                   ======
*   pub_pubquantity			     READ
*	pub_dayofweekquantity		 READ
*	pub_pubquantitytype			 READ
*
* PROCEDURES CALLED:
*   None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	int - insert quantity
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 09/07/2007      BJS             Initial Creation
*
*/
returns int
as
begin
	return(select top 1 dowqty.Quantity
	from PUB_PubQuantity ppq join PUB_DayOfWeekQuantity dowqty on ppq.PUB_PubQuantity_ID = dowqty.PUB_PubQuantity_ID
		join PUB_PubQuantityType pqt on dowqty.PUB_PubQuantityType_ID = pqt.PUB_PubQuantityType_ID
	where ppq.PUB_PubRate_Map_ID = @PUB_PubRate_Map_ID and dowqty.PUB_PubQuantityType_ID = @PUB_PubQuantityType_ID
		and (pqt.Special = 1 or datepart(dw, @InsertDate) = dowqty.InsertDow)
		and ppq.EffectiveDate <= @InsertDate
	order by EffectiveDate desc)
end
GO

GRANT  EXECUTE  ON [dbo].[PubRateMapInsertQuantityByInsertDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
IF OBJECT_ID('dbo.TotalEstimateWeight') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.TotalEstimateWeight'
	DROP FUNCTION dbo.TotalEstimateWeight
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.TotalEstimateWeight') IS NOT NULL
		PRINT '***********Drop of dbo.TotalEstimateWeight FAILED.'
END
GO
PRINT 'Creating dbo.TotalEstimateWeight'
GO

CREATE function dbo.TotalEstimateWeight(@EST_Estimate_ID bigint)
returns decimal(14,6)
/*
* PARAMETERS:
*	EST_Estimate_ID - The Estimate ID
*
* DESCRIPTION:
*	Returns the total estimate weight (total quantity * piece weight)
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Component       READ
*   EST_Samples         READ
*
* PROCEDURES CALLED:
*   dbo.ComponentWeight
*   dbo.ComponentMediaQuantity
*   dbo.ComponentOtherQuantity
* DATABASE:
*	All
*
* RETURN VALUE:
*	Total estimate weight
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/24/2007      BJS             Initial Creation 
*
*/
as
begin
	return(
		select sum(
			dbo.ComponentWeight(c.EST_Component_ID) * (isnull(dbo.ComponentMediaQuantity(c.EST_Component_ID), 0) * (1 + isnull(c.SpoilagePct, 0))
			+ isnull(dbo.ComponentOtherQuantity(c.EST_Component_ID), 0) + isnull(s.Quantity, 0)))
		from EST_Component c left join EST_Samples s on c.EST_Estimate_ID = s.EST_Estimate_ID
		where c.EST_Estimate_ID = @EST_Estimate_ID)
end
GO

GRANT  EXECUTE  ON [dbo].[TotalEstimateWeight]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO

GO
