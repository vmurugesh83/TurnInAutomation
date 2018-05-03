IF OBJECT_ID('dbo.PubPubGroup_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubPubGroup_i'
	DROP PROCEDURE dbo.PubPubGroup_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubPubGroup_i') IS NOT NULL
		PRINT '***********Drop of dbo.PubPubGroup_i FAILED.'
END
GO
PRINT 'Creating dbo.PubPubGroup_i'
GO

create PROCEDURE dbo.PubPubGroup_i
/*
* PARAMETERS:
*	PUB_PubGroup_ID - The new Pub Group ID
*   Description
*   Comments
*   EffectiveDate
*   SortOrder
*   CreatedBy
*
* DESCRIPTION:
* Inserts a new record into the PUB_PubGroup table.  If any est_package records
* referenced a pub_pubgroup with the same description reference the new pub_group
* if applicable (If Run Date is on or after the pub_group effective date AND prior to the
* next effective date.)
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_PubGroup
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 06/25/2007      BJS             Initial Creation
* 06/29/2007      BJS             Modified how SortOrder is determined. 
*
*/
@PUB_PubGroup_ID bigint output,
@Description varchar(35),
@Comments varchar(255),
@Active bit,
@EffectiveDate datetime,
@CreatedBy varchar(50)
as

/*You cannot insert a record without a description*/
if (@Description = '') begin
	raiserror('A Publication Group must have a description.', 16, 1)
	return
end


/*If a pub group of the same description an effective date already exist return an error */
if exists(select 1 from PUB_PubGroup (holdlock) where Description = @Description and EffectiveDate = @EffectiveDate and CustomGroupForPackage = 0) begin
	raiserror('A PUB_PubGroup with the same description and effective date already exists.', 16, 1)
	return
end

else begin

	declare @SortOrder int, @PreviousPubGroupID bigint

	select @SortOrder = SortOrder
	from PUB_PubGroup
	where Description = @Description and CustomGroupForPackage = 0

	if (@SortOrder is null) begin
		select @SortOrder = max(SortOrder) + 1
		from PUB_PubGroup
		where CustomGroupForPackage = 0
	end

	if (@SortOrder is null)
		set @SortOrder = 0

	select top 1 @PreviousPubGroupID = PUB_PubGroup_ID
	from PUB_PubGroup
	where Description = @Description and CustomGroupForPackage = 0 and EffectiveDate < @EffectiveDate
	order by EffectiveDate desc

	/* If the PUB Group is being created as "inactive".  Make sure that no estimates will need to use it.
     * An estimate cannot reference an inactive pub group. */
	if (@Active = 0
		and exists(select 1 from EST_Estimate e join EST_Package p on e.EST_Estimate_ID = p.EST_Estimate_ID
			where p.PUB_PubGroup_ID = @PreviousPubGroupID and e.RunDate >= @EffectiveDate)) begin

		raiserror('Cannot create inactive group.  There are distribution mapping record(s) which reference it.', 16, 1)
		return
	end


	insert into PUB_PubGroup(Description, Comments, Active, EffectiveDate, SortOrder, CustomGroupForPackage, CreatedBy, CreatedDate)
	values(@Description, @Comments, @Active, @EffectiveDate, @SortOrder, 0, @CreatedBy, getdate())
	set @PUB_PubGroup_ID = @@identity
	if (@@error <> 0) begin
		raiserror('Error inserting PUB_PubGroup record.', 16, 1)
		return
	end

	/* Remove any EST_PubIssueDate records referencing pub_rate_maps that were included by the old group */
	delete pid
	from EST_PubIssueDates pid join EST_Package p on pid.EST_Estimate_ID = p.EST_Estimate_ID
		join EST_Estimate e on pid.EST_Estimate_ID = pid.EST_Estimate_ID
		join PUB_PubPubGroup_Map ppgm on p.PUB_PubGroup_ID = ppgm.PUB_PubGroup_ID
	where p.PUB_PubGroup_ID = @PreviousPubGroupID and e.RunDate >= @EffectiveDate
	if (@@error <> 0) begin
		raiserror('Error removing EST_PubIssueDates record(s) referencing the old PUB_PubGroup record.', 16, 1)
		return
	end

	/* Update any estimates referencing the Pub Group */
	update e
		set
			e.ModifiedBy = @CreatedBy,
			e.ModifiedDate = getdate()
	from EST_Estimate e join EST_Package p on e.EST_Estimate_ID = p.EST_Estimate_ID
		join PUB_PubGroup g on p.PUB_PubGroup_ID = g.PUB_PubGroup_ID
	where g.PUB_PubGroup_ID = @PreviousPubGroupID and g.CustomGroupForPackage = 0 and e.RunDate >= @EffectiveDate
	if (@@error <> 0) begin
		raiserror('Error updating EST_Estimate last modified user/date.', 16, 1)
		return
	end

	update p
		set
			p.PUB_PubGroup_ID = @PUB_PubGroup_ID,
			p.ModifiedBy = @CreatedBy,
			p.ModifiedDate = getdate()
	from EST_Estimate e join EST_Package p on e.EST_Estimate_ID = p.EST_Estimate_ID
		join PUB_PubGroup g on p.PUB_PubGroup_ID = g.PUB_PubGroup_ID
	where g.PUB_PubGroup_ID = @PreviousPubGroupID and g.CustomGroupForPackage = 0 and e.RunDate >= @EffectiveDate
	if (@@error <> 0) begin
		raiserror('Error updating EST_Packages with new PUB_PubGroup_ID.', 16, 1)
		return
	end
end
GO

GRANT  EXECUTE  ON [dbo].[PubPubGroup_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
