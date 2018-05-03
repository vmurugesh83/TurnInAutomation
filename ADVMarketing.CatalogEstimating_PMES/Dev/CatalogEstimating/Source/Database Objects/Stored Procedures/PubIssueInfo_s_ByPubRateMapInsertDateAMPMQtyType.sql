IF OBJECT_ID('dbo.PubIssueInfo_s_ByPubRateMapInsertDateAMPMQtyType') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.PubIssueInfo_s_ByPubRateMapInsertDateAMPMQtyType'
	DROP PROCEDURE dbo.PubIssueInfo_s_ByPubRateMapInsertDateAMPMQtyType
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.PubIssueInfo_s_ByPubRateMapInsertDateAMPMQtyType') IS NOT NULL
		PRINT '***********Drop of dbo.PubIssueInfo_s_ByPubRateMapInsertDateAMPMQtyType FAILED.'
END
GO
PRINT 'Creating dbo.PubIssueInfo_s_ByPubRateMapInsertDateAMPMQtyType'
GO

create PROCEDURE dbo.PubIssueInfo_s_ByPubRateMapInsertDateAMPMQtyType
/*
* PARAMETERS:
*	none
*
* DESCRIPTION:
*	Retrieves a list of effective dates and the active status for a pub location.
*
* TABLES:
*	Table Name          		Access
*	==========          		======
*	DBADVProd.informix.pub_loc	read
*	PUB_PubRate_Map
*	PUB_PubRate_Map_Activate
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
* ----------	----	-------------------------------------------------
* 09/13/2007	JRH	Initial Creation 
* 11/07/2007	JRH	PM is the day before so needed to subtract one day.
*
*/
@pub_pubrate_map_id	bigint,
@pub_pubquantitytype_id	int,
@InsertDate		datetime,
@AMPM			bit

AS

DECLARE @InsertDatePM datetime
SELECT @InsertDatePM = dateadd(day, -1, @InsertDate)

SELECT
	pub_id
	, pub_nm
	, publoc_id
	, publoc_nm
	, issuedate
	, issuedow = datepart(w, issuedate)
	, quantity = ISNULL(dbo.PubRateMapInsertQuantityByInsertDate(issuedate, @pub_pubquantitytype_id, @pub_pubrate_map_id), 0)
FROM
	(SELECT
		pub_id
		, pub_nm
		, publoc_id
		, publoc_nm
		, issuedate = dbo.CalcIssueDate(@InsertDate, @AMPM, AM_edition, AM_offset, PM_edition, PM_offset)
	FROM
		(SELECT 
			pl.pub_id
			, p.pub_nm
			, pl.publoc_id
			, pl.publoc_nm
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
			DBADVProd.informix.pub p (nolock)
			INNER JOIN DBADVProd.informix.pub_loc pl (nolock)
				ON p.pub_id = pl.pub_id
			INNER JOIN dbo.pub_pubrate_map rm (nolock)
				ON pl.pub_id = rm.pub_id
				AND pl.publoc_id = rm.publoc_id
		WHERE
			rm.pub_pubrate_map_id = @pub_pubrate_map_id) a) b
GO

GRANT  EXECUTE  ON [dbo].[PubIssueInfo_s_ByPubRateMapInsertDateAMPMQtyType]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO
