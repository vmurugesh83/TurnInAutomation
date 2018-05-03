IF OBJECT_ID('dbo.EstPubIssueDates_i_ByEstimateID') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.EstPubIssueDates_i_ByEstimateID'
	DROP PROCEDURE dbo.EstPubIssueDates_i_ByEstimateID
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.EstPubIssueDates_i_ByEstimateID') IS NOT NULL
		PRINT '***********Drop of dbo.EstPubIssueDates_i_ByEstimateID FAILED.'
END
GO
PRINT 'Creating dbo.EstPubIssueDates_i_ByEstimateID'
GO

create proc dbo.EstPubIssueDates_i_ByEstimateID
/*
* PARAMETERS:
* EST_Estimate_ID
* CreatedBy
*
* DESCRIPTION:
*		Inserts an Est_PubIssueDate record for each PubRate Map referenced by the estimate.
*
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   EST_Component       READ
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
* 09/12/2007      BJS             Initial Creation 
* 10/25/2007      BJS             Added logic to determine issue date offset
*
**********************************************************************************************************
*
**********************************************************************************************************
*/
@EST_Estimate_ID bigint,
@CreatedBy varchar(50)
as

insert into EST_PubIssueDates(EST_Estimate_ID, PUB_PubRate_Map_ID, [Override], IssueDOW, IssueDate, CreatedBy, CreatedDate)
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
		JOIN PUB_PubPubGroup_Map ppgm on rm.PUB_PubRate_Map_ID = ppgm.PUB_PubRate_Map_ID
		JOIN EST_Package pkg on ppgm.PUB_PubGroup_ID = pkg.PUB_PubGroup_ID
		JOIN EST_Estimate e on pkg.EST_Estimate_ID = e.EST_Estimate_ID
		JOIN EST_AssemDistribOptions ad on e.EST_Estimate_ID = ad.EST_Estimate_ID
		LEFT JOIN EST_PubIssueDates pid on pid.EST_Estimate_ID = @EST_Estimate_ID and ppgm.PUB_PubRate_Map_ID = pid.PUB_PubRate_Map_ID
	WHERE e.EST_Estimate_ID = @EST_Estimate_ID and pid.EST_Estimate_ID is null) a
GO

GRANT  EXECUTE  ON [dbo].[EstPubIssueDates_i_ByEstimateID]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
