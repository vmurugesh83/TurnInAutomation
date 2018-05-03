IF OBJECT_ID('dbo.PubPubGroup_u_ByID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubPubGroup_u_ByID'
	DROP PROCEDURE dbo.PubPubGroup_u_ByID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubPubGroup_u_ByID') IS NOT NULL
		PRINT '***********Drop of dbo.PubPubGroup_u_ByID FAILED.'
END
GO
PRINT 'Creating dbo.PubPubGroup_u_ByID'
GO

create PROCEDURE dbo.PubPubGroup_u_ByID
/*
* PARAMETERS:
*	PUB_PubGroup_ID
* Description
* Comments
* Active
* ModifiedBy
*
* DESCRIPTION:
*	Updates a PUB_PubGroup record and the sort order of all other groups with the same description.
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
* 06/29/2007      BJS             Added SortOrder update
* 10/08/2007      BJS             Added logic to link the update with Estimates
*
*/
@PUB_PubGroup_ID bigint,
@Comments varchar(255),
@Active bit,
@SortOrder int,
@ModifiedBy varchar(50)
as

declare @pActive bit, @pEffectiveDate datetime
select @pActive = Active, @pEffectiveDate = EffectiveDate
from PUB_PubGroup
where PUB_PubGroup_ID = @PUB_PubGroup_ID

/* Identify the pub group immediately preceding the pubgroup being updated. */
declare @previous_PUB_PubGroup_ID bigint
select top 1 @previous_PUB_PubGroup_ID = old_pg.PUB_PubGroup_ID
from PUB_PubGroup pg join PUB_PubGroup old_pg on pg.Description = old_pg.Description and old_pg.EffectiveDate < pg.EffectiveDate and old_pg.CustomGroupForPackage = 0
where pg.PUB_PubGroup_ID = @PUB_PubGroup_ID
order by old_pg.EffectiveDate desc

/* The user is trying to inactivate a pub group that is currently active. */
if (@Active = 0 and @pActive = 1) begin
	/* If any packages reference the pubgroup they need to be modified to reference the prior one.
       If a prior pubgroup does not exist or is inactive return an error. */
	if ((@previous_PUB_PubGroup_ID is null
			or exists (select 1 from PUB_PubGroup where PUB_PubGroup_ID = @previous_PUB_PubGroup_ID and Active = 0))
			and exists (select 1 from EST_Package p where PUB_PubGroup_ID = @PUB_PubGroup_ID)) begin
		
		raiserror('Cannot inactivate pub group record.  It is being reference by distribution mapping record(s).', 16, 1)
		return
	end

	/* Packages linked to this pub group will need to reference the prior pub group.  If any overlaps would occur this cannot be done.
     * Return an exception */
	if exists (select 1 from EST_Package source_p join EST_Estimate e on source_p.EST_Estimate_ID = e.EST_Estimate_ID
				join EST_Package other_p on e.EST_Estimate_ID = other_p.EST_Estimate_ID and source_p.EST_Package_ID <> other_p.EST_Package_ID
				join PUB_PubPubGroup_Map other_ppgm on other_p.PUB_PubGroup_ID = other_ppgm.PUB_PubGroup_ID
				join PUB_PubPubGroup_Map previous_ppgm on other_ppgm.PUB_PubRate_Map_ID = previous_ppgm.PUB_PubRate_Map_ID
			where source_p.PUB_PubGroup_ID = @PUB_PubGroup_ID and previous_ppgm.PUB_PubGroup_ID = @previous_PUB_PubGroup_ID) begin

		raiserror('Cannot inactivate pub group record.  Conflicts would be created in distribution mapping record(s).', 16, 1)
		return
	end

	/* Remove any EST_PubIssueDate records referencing pub-locs in this group */
	delete pid
	from EST_PubIssueDates pid join EST_Package p on pid.EST_Estimate_ID = p.EST_Estimate_ID
		join PUB_PubPubGroup_Map ppgm on p.PUB_PubGroup_ID = ppgm.PUB_PubGroup_ID
	where p.PUB_PubGroup_ID = @PUB_PubGroup_ID
	if (@@error <> 0) begin
		raiserror('Error removing EST_PubIssueDates record(s) referencing the PUB_PubGroup record.', 16, 1)
		return
	end

	update e
		set
			e.ModifiedBy = @ModifiedBy,
			e.ModifiedDate = getdate()
	from EST_Estimate e join EST_Package p on e.EST_Estimate_ID = p.EST_Estimate_ID
	where p.PUB_PubGroup_ID = @PUB_PubGroup_ID
	if (@@error <> 0) begin
		raiserror('Error updating Modified user/date on Estimate record(s).', 16, 1)
		return
	end

	/* Update packages with previous group */
	update EST_Package
		set
			PUB_PubGroup_ID = @previous_PUB_PubGroup_ID,
			ModifiedBy = @ModifiedBy,
			ModifiedDate = getdate()
	where PUB_PubGroup_ID = @PUB_PubGroup_ID
	if (@@error <> 0) begin
		raiserror('Error updating package record(s).', 16, 1)
		return
	end
	
	/* Create Pub Issue Date records for the previous group */
	insert into EST_PubIssueDates(EST_Estimate_ID, PUB_PubRate_Map_ID, [override], IssueDOW, IssueDate, CreatedBy, CreatedDate)
	SELECT
		EST_Estimate_ID,
		PUB_PubRate_Map_ID,
		0,
		datepart(w, dbo.CalcIssueDate(InsertDate, InsertTime, AM_edition, AM_offset, PM_edition, PM_offset)),
		dbo.CalcIssueDate(InsertDate, InsertTime, AM_edition, AM_offset, PM_edition, PM_offset),
		@ModifiedBy,
		getdate()
	FROM
		(SELECT e.EST_Estimate_ID
			, rm.PUB_PubRate_Map_ID
			, InsertDate =
				case
					when datepart(dw, e.RunDate) > ad.InsertDOW then dateadd(d, -1 * (datepart(dw, e.RunDate) - ad.InsertDOW), e.RunDate)
					when datepart(dw, e.RunDate) < ad.InsertDOW then dateadd(d, ad.InsertDOW - datepart(dw, e.RunDate) - 7, e.RunDate)
					else e.RunDate
				end
			, ad.InsertTime
			, pl.pub_id
			, p.pub_nm
			, pl.publoc_id
			, pl.publoc_nm
			, AM_edition = 
				CASE ad.InsertDOW
					WHEN 1 THEN sun_edtn_cd
					WHEN 2 THEN mon_edtn_cd
					WHEN 3 THEN tue_edtn_cd
					WHEN 4 THEN wed_edtn_cd
					WHEN 5 THEN thu_edtn_cd
					WHEN 6 THEN fri_edtn_cd
					WHEN 7 THEN sat_edtn_cd
				END
			, AM_offset = 
				CASE ad.InsertDOW
					WHEN 1 THEN no_sun_edtn_nbr
					WHEN 2 THEN no_mon_edtn_nbr
					WHEN 3 THEN no_tue_edtn_nbr
					WHEN 4 THEN no_wed_edtn_nbr
					WHEN 5 THEN no_thu_edtn_nbr
					WHEN 6 THEN no_fri_edtn_nbr
					WHEN 7 THEN no_sat_edtn_nbr
				END
			, PM_edition = 
				CASE (ad.InsertDOW - 1) % 7
					WHEN 1 THEN sun_edtn_cd
					WHEN 2 THEN mon_edtn_cd
					WHEN 3 THEN tue_edtn_cd
					WHEN 4 THEN wed_edtn_cd
					WHEN 5 THEN thu_edtn_cd
					WHEN 6 THEN fri_edtn_cd
					WHEN 0 THEN sat_edtn_cd -- 7 modulus 7 is a zero
				END
			, PM_offset = 
				CASE (ad.InsertDOW - 1) % 7
					WHEN 1 THEN no_sun_edtn_nbr
					WHEN 2 THEN no_mon_edtn_nbr
					WHEN 3 THEN no_tue_edtn_nbr
					WHEN 4 THEN no_wed_edtn_nbr
					WHEN 5 THEN no_thu_edtn_nbr
					WHEN 6 THEN no_fri_edtn_nbr
					WHEN 0 THEN no_sat_edtn_nbr -- 7 modulus 7 is a zero
				END
		FROM
			DBADVProd.informix.pub p (nolock)
			INNER JOIN DBADVProd.informix.pub_loc pl (nolock)
				ON p.pub_id = pl.pub_id
			INNER JOIN dbo.pub_pubrate_map rm (nolock)
				ON pl.pub_id = rm.pub_id
				AND pl.publoc_id = rm.publoc_id
			JOIN PUB_PubPubGroup_Map ppgm on rm.PUB_PubRate_Map_ID = ppgm.PUB_PubRate_Map_ID
			JOIN EST_Package pkg on pkg.PUB_PubGroup_ID = ppgm.PUB_PubGroup_ID and pkg.PUB_PubGroup_ID = @PUB_PubGroup_ID
			JOIN EST_Estimate e on pkg.EST_Estimate_ID = e.EST_Estimate_ID
			JOIN EST_AssemDistribOptions ad on e.EST_Estimate_ID = ad.EST_Estimate_ID) a
	if (@@error <> 0) begin
		raiserror('Error inserting est_pubissuedate record(s).', 16, 1)
		return
	end
end

/* The user is trying to activate a group that is currently inactive */
else if (@Active = 1 and @pActive = 0) begin
	/* Packages referencing the previous Pub Group may need to reference this pub group.  If any overlaps would occur, return an error. */
	if exists (select 1 from EST_Package source_p join EST_Estimate e on source_p.EST_Estimate_ID = e.EST_Estimate_ID
				join EST_package other_p on e.EST_Estimate_ID = other_p.EST_Estimate_ID and source_p.EST_Package_ID <> other_p.EST_Package_ID
				join PUB_PubPubGroup_Map other_ppgm on other_p.PUB_PubGroup_ID = other_ppgm.PUB_PubGroup_ID
				join PUB_PubPubGroup_Map new_ppgm on other_ppgm.PUB_PubRate_Map_ID = new_ppgm.PUB_PubRate_Map_ID
			where source_p.PUB_PubGroup_ID = @previous_PUB_PubGroup_ID and new_ppgm.PUB_PubGroup_ID = @PUB_PubGroup_ID and e.RunDate >= @pEffectiveDate) begin

		raiserror('Cannot activate pub group record.  Conflicts would be created in distribution mapping record(s).', 16, 1)
		return
	end

	/* Remove any EST_PubIssueDate records referencing pub-locs in the prior group */
	delete pid
	from EST_PubIssueDates pid join EST_Package p on pid.EST_Estimate_ID = p.EST_Estimate_ID
		join PUB_PubPubGroup_Map ppgm on p.PUB_PubGroup_ID = ppgm.PUB_PubGroup_ID
		join EST_Estimate e on pid.EST_Estimate_ID = e.EST_Estimate_ID
	where p.PUB_PubGroup_ID = @previous_PUB_PubGroup_ID and e.RunDate >= @pEffectiveDate
	if (@@error <> 0) begin
		raiserror('Error removing EST_PubIssueDates record(s) referencing the prior PUB_PubGroup record.', 16, 1)
		return
	end
	
	/* Create Pub Issue Date records for estimates that will reference the current group */
	insert into EST_PubIssueDates(EST_Estimate_ID, PUB_PubRate_Map_ID, [override], IssueDOW, IssueDate, CreatedBy, CreatedDate)
	SELECT
		EST_Estimate_ID,
		PUB_PubRate_Map_ID,
		0,
		datepart(w, dbo.CalcIssueDate(InsertDate, InsertTime, AM_edition, AM_offset, PM_edition, PM_offset)),
		dbo.CalcIssueDate(InsertDate, InsertTime, AM_edition, AM_offset, PM_edition, PM_offset),
		@ModifiedBy,
		getdate()
	FROM
		(SELECT e.EST_Estimate_ID
			, rm.PUB_PubRate_Map_ID
			, InsertDate =
				case
					when datepart(dw, e.RunDate) > ad.InsertDOW then dateadd(d, -1 * (datepart(dw, e.RunDate) - ad.InsertDOW), e.RunDate)
					when datepart(dw, e.RunDate) < ad.InsertDOW then dateadd(d, ad.InsertDOW - datepart(dw, e.RunDate) - 7, e.RunDate)
					else e.RunDate
				end
			, ad.InsertTime
			, pl.pub_id
			, p.pub_nm
			, pl.publoc_id
			, pl.publoc_nm
			, AM_edition = 
				CASE ad.InsertDOW
					WHEN 1 THEN sun_edtn_cd
					WHEN 2 THEN mon_edtn_cd
					WHEN 3 THEN tue_edtn_cd
					WHEN 4 THEN wed_edtn_cd
					WHEN 5 THEN thu_edtn_cd
					WHEN 6 THEN fri_edtn_cd
					WHEN 7 THEN sat_edtn_cd
				END
			, AM_offset = 
				CASE ad.InsertDOW
					WHEN 1 THEN no_sun_edtn_nbr
					WHEN 2 THEN no_mon_edtn_nbr
					WHEN 3 THEN no_tue_edtn_nbr
					WHEN 4 THEN no_wed_edtn_nbr
					WHEN 5 THEN no_thu_edtn_nbr
					WHEN 6 THEN no_fri_edtn_nbr
					WHEN 7 THEN no_sat_edtn_nbr
				END
			, PM_edition = 
				CASE (ad.InsertDOW - 1) % 7
					WHEN 1 THEN sun_edtn_cd
					WHEN 2 THEN mon_edtn_cd
					WHEN 3 THEN tue_edtn_cd
					WHEN 4 THEN wed_edtn_cd
					WHEN 5 THEN thu_edtn_cd
					WHEN 6 THEN fri_edtn_cd
					WHEN 0 THEN sat_edtn_cd -- 7 modulus 7 is a zero
				END
			, PM_offset = 
				CASE (ad.InsertDOW - 1) % 7
					WHEN 1 THEN no_sun_edtn_nbr
					WHEN 2 THEN no_mon_edtn_nbr
					WHEN 3 THEN no_tue_edtn_nbr
					WHEN 4 THEN no_wed_edtn_nbr
					WHEN 5 THEN no_thu_edtn_nbr
					WHEN 6 THEN no_fri_edtn_nbr
					WHEN 0 THEN no_sat_edtn_nbr -- 7 modulus 7 is a zero
				END
		FROM
			DBADVProd.informix.pub p (nolock)
			INNER JOIN DBADVProd.informix.pub_loc pl (nolock)
				ON p.pub_id = pl.pub_id
			INNER JOIN dbo.pub_pubrate_map rm (nolock)
				ON pl.pub_id = rm.pub_id
				AND pl.publoc_id = rm.publoc_id
			JOIN PUB_PubPubGroup_Map ppgm on rm.PUB_PubRate_Map_ID = ppgm.PUB_PubRate_Map_ID
			JOIN EST_Package pkg on ppgm.PUB_PubGroup_ID = pkg.PUB_PubGroup_ID and pkg.PUB_PubGroup_ID = @previous_PUB_PubGroup_ID
			JOIN EST_Estimate e on pkg.EST_Estimate_ID = e.EST_Estimate_ID and e.RunDate >= @pEffectiveDate
			JOIN EST_AssemDistribOptions ad on e.EST_Estimate_ID = ad.EST_Estimate_ID) a
	if (@@error <> 0) begin
		raiserror('Error inserting est_pubissuedate record(s).', 16, 1)
		return
	end


	update e
		set
			e.ModifiedBy = @ModifiedBy,
			e.ModifiedDate = getdate()
	from EST_Estimate e join EST_Package p on e.EST_Estimate_ID = p.EST_Estimate_ID
	where p.PUB_PubGroup_ID = @previous_PUB_PubGroup_ID and e.RunDate >= @pEffectiveDate
	if (@@error <> 0) begin
		raiserror('Error updating Modified user/date on Estimate record(s).', 16, 1)
		return
	end

	/* Update packages with current group */
	update p
		set
			PUB_PubGroup_ID = @PUB_PubGroup_ID,
			ModifiedBy = @ModifiedBy,
			ModifiedDate = getdate()
	from EST_Estimate e join EST_Package p on e.EST_Estimate_ID = p.EST_Estimate_ID
	where PUB_PubGroup_ID = @previous_PUB_PubGroup_ID and e.RunDate >= @pEffectiveDate
	if (@@error <> 0) begin
		raiserror('Error updating package record(s).', 16, 1)
		return
	end
end

update PUB_PubGroup
	set
		Comments = @Comments,
		Active = @Active,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = getdate()
where PUB_PubGroup_ID = @PUB_PubGroup_ID
if (@@error <> 0) begin
	raiserror('Error updating PUB_PubGroup record.', 16, 1)
	return
end

update g_samedescription
	set SortOrder = @SortOrder
from PUB_PubGroup g join PUB_PubGroup g_samedescription on g.Description = g_samedescription.Description
where g.PUB_PubGroup_ID = @PUB_PubGroup_ID and g_samedescription.CustomGroupForPackage = 0
if (@@error <> 0) begin
	raiserror('Error updating PUB_PubGroup sort order.', 16, 1)
	return
end

GO

GRANT  EXECUTE  ON [dbo].[PubPubGroup_u_ByID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
