IF OBJECT_ID('dbo.PubPubGroupMap_i') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubPubGroupMap_i'
	DROP PROCEDURE dbo.PubPubGroupMap_i
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubPubGroupMap_i') IS NOT NULL
		PRINT '***********Drop of dbo.PubPubGroupMap_i FAILED.'
END
GO
PRINT 'Creating dbo.PubPubGroupMap_i'
GO

CREATE PROCEDURE dbo.PubPubGroupMap_i
/*
* PARAMETERS:
*	PUB_PubRate_Map_ID
*   PUB_PubGroup_ID
*   CreatedBy
*
* DESCRIPTION:
*	Adds pub locations to the specified pub group.
*
* TABLES:
*   Table Name                 Access
*   ==========                 ======
*   DBADVProd.informix.pub     READ
*   DBADVProd.informix.pub_loc READ
*   PUB_PubRate_Map            READ
*   EST_Package                READ
*   EST_Estimate               READ
*   EST_AssemDistribOptions    READ
*   EST_PubIssueDates          INSERT
*   PUB_PubPubGroup_Map        INSERT
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
* 10/08/2007      BJS             Initial Creation
*
*/
@PUB_PubRate_Map_ID bigint,
@PUB_PubGroup_ID bigint,
@CreatedBy varchar(50)
as

/* Add the record */
if not exists(select 1 from PUB_PubPubGroup_Map where PUB_PubRate_Map_ID = @PUB_PubRate_Map_ID and PUB_PubGroup_ID = @PUB_PubGroup_ID) begin
	/* Adding the pubratemap to the pub group cannot cause an overlap in the distribution mappings */
	if exists(
		select 1 from EST_Package source_p join EST_Package other_p on source_p.EST_Estimate_ID = other_p.EST_Estimate_ID and source_p.EST_Package_ID <> other_p.EST_Package_ID
			join PUB_PubPubGroup_Map other_ppgm on other_p.PUB_PubGroup_ID = other_ppgm.PUB_PubGroup_ID
		where source_p.PUB_PubGroup_ID = @PUB_PubGroup_ID and other_ppgm.PUB_PubRate_Map_ID = @PUB_PubRate_Map_ID) begin

		raiserror('Cannot add pub-loc to group.  It would cause a conflict in distribution mapping record(s).', 16, 1)
		return
	end

	insert into PUB_PubPubGroup_Map(PUB_PubRate_Map_ID, PUB_PubGroup_ID, CreatedBy, CreatedDate)
	values(@PUB_PubRate_Map_ID, @PUB_PubGroup_ID, @CreatedBy, getdate())
	if (@@error <> 0) begin
		raiserror('Error inserting pub_pubpubgroup_map record.', 16, 1)
		return
	end

	/* Code copied from PubIssueInfo_s_ByPubRateMapInsertDateAMPMQtyType */
	/* Create corresponding EST_PubIssueDate records */
	insert into EST_PubIssueDates(EST_Estimate_ID, PUB_PubRate_Map_ID, [override], IssueDOW, IssueDate, CreatedBy, CreatedDate)
	SELECT
		EST_Estimate_ID,
		PUB_PubRate_Map_ID,
		0,
		datepart(w, dbo.CalcIssueDate(InsertDate, InsertTime, AM_edition, AM_offset, PM_edition, PM_offset)),
		dbo.CalcIssueDate(InsertDate, InsertTime, AM_edition, AM_offset, PM_edition, PM_offset),
		@CreatedBy,
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
			JOIN EST_Package pkg on pkg.PUB_PubGroup_ID = @PUB_PubGroup_ID
			JOIN EST_Estimate e on pkg.EST_Estimate_ID = e.EST_Estimate_ID
			JOIN EST_AssemDistribOptions ad on e.EST_Estimate_ID = ad.EST_Estimate_ID
		WHERE
			rm.pub_pubrate_map_id = @PUB_PubRate_Map_ID) a
	if (@@error <> 0) begin
		raiserror('Error inserting est_pubissuedate record(s).', 16, 1)
		return
	end
end
GO

GRANT  EXECUTE  ON [dbo].[PubPubGroupMap_i]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
