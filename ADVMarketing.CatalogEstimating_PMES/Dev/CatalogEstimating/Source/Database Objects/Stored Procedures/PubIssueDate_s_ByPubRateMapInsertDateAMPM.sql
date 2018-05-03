IF OBJECT_ID('dbo.PubIssueDate_s_ByPubRateMapInsertDateAMPM') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubIssueDate_s_ByPubRateMapInsertDateAMPM'
	DROP PROCEDURE dbo.PubIssueDate_s_ByPubRateMapInsertDateAMPM
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubIssueDate_s_ByPubRateMapInsertDateAMPM') IS NOT NULL
		PRINT '***********Drop of dbo.PubIssueDate_s_ByPubRateMapInsertDateAMPM FAILED.'
END
GO
PRINT 'Creating dbo.PubIssueDate_s_ByPubRateMapInsertDateAMPM'
GO

create PROCEDURE dbo.PubIssueDate_s_ByPubRateMapInsertDateAMPM
/*
* PARAMETERS:
*	none
*
* DESCRIPTION:
*	Retrieves a list of effective dates and the active status for a pub location.
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   PUB_PubRate_Map
*   PUB_PubRate_Map_Activate
*
* FUNCTIONS CALLED:
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
* Date		Who	Comments
* ----------	---	-------------------------------------------------
* 09/13/2007	JRH	Initial Creation 
* 10/19/2007	JRH	@InsertDatePM should be previous date
*
*/
@pub_pubrate_map_id	bigint,
@InsertDate		datetime,
@AMPM			bit

AS

DECLARE @InsertDatePM datetime
SELECT @InsertDatePM = dateadd(day, -1, @InsertDate)

SELECT
	pub_id
	, publoc_id
	, issuedate = dbo.CalcIssueDate(@InsertDate, @AMPM, AM_edition, AM_offset, PM_edition, PM_offset)
	, issuedow = datepart(w, dbo.CalcIssueDate(@InsertDate, @AMPM, AM_edition, AM_offset, PM_edition, PM_offset))
FROM
	(SELECT 
		pl.pub_id
		, pl.publoc_id
		, AM_edition = 
			CASE datepart(w, @InsertDate)
				WHEN 1 THEN sun_edtn_cd
				WHEN 2 THEN mon_edtn_cd
				WHEN 3 THEN tue_edtn_cd
				WHEN 4 THEN wed_edtn_cd
				WHEN 5 THEN thu_edtn_cd
				WHEN 6 THEN fri_edtn_cd
				WHEN 7 THEN sat_edtn_cd
			END
		, AM_offset = 
			CASE datepart(w, @InsertDate)
				WHEN 1 THEN no_sun_edtn_nbr
				WHEN 2 THEN no_mon_edtn_nbr
				WHEN 3 THEN no_tue_edtn_nbr
				WHEN 4 THEN no_wed_edtn_nbr
				WHEN 5 THEN no_thu_edtn_nbr
				WHEN 6 THEN no_fri_edtn_nbr
				WHEN 7 THEN no_sat_edtn_nbr
			END
		, PM_edition = 
			CASE datepart(w, @InsertDatePM)
				WHEN 1 THEN sun_edtn_cd
				WHEN 2 THEN mon_edtn_cd
				WHEN 3 THEN tue_edtn_cd
				WHEN 4 THEN wed_edtn_cd
				WHEN 5 THEN thu_edtn_cd
				WHEN 6 THEN fri_edtn_cd
				WHEN 7 THEN sat_edtn_cd
			END
		, PM_offset = 
			CASE datepart(w, @InsertDatePM)
				WHEN 1 THEN no_sun_edtn_nbr
				WHEN 2 THEN no_mon_edtn_nbr
				WHEN 3 THEN no_tue_edtn_nbr
				WHEN 4 THEN no_wed_edtn_nbr
				WHEN 5 THEN no_thu_edtn_nbr
				WHEN 6 THEN no_fri_edtn_nbr
				WHEN 7 THEN no_sat_edtn_nbr
			END
	FROM
		DBADVProd.informix.pub_loc pl (nolock)
		INNER JOIN dbo.pub_pubrate_map rm (nolock)
			ON pl.pub_id = rm.pub_id
			AND pl.publoc_id = rm.publoc_id
	WHERE
		rm.pub_pubrate_map_id = @pub_pubrate_map_id) a
GO

GRANT  EXECUTE  ON [dbo].[PubIssueDate_s_ByPubRateMapInsertDateAMPM]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
