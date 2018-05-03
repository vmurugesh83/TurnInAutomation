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
