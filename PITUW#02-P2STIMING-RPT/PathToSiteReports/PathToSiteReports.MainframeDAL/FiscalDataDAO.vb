Imports System.Configuration
Imports BonTon.DBUtility.DB2Helper
Imports IBM.Data.DB2
Imports PathToSiteReports.BusinessEntities
Imports PathToSiteReports.Factory

Public Class FiscalDataDAO
    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")
    Private Shared _dbSchema As String = ConfigurationManager.AppSettings("DB2Schema")

    Public Function GetFiscalMonthByDate(ByVal calendarDate As Date) As Integer
        Dim fiscalMonth As Integer = 0
        Dim sql As String = String.Empty

        Try
            sql = String.Format("SELECT COALESCE(FISC_MO,0) AS FISC_MO FROM {0}.TMS100FISCAL_CAL " &
                                " WHERE CAL_DATE = '{1}' FETCH FIRST 1 ROWS ONLY WITH UR ;",
                                _dbSchema, calendarDate.ToString("yyyy-MM-dd"))
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.Text, sql, Nothing)
                If rdr.HasRows Then
                    rdr.Read()
                    fiscalMonth = rdr("FISC_MO")
                End If
            End Using
        Catch ex As Exception
            Throw
        End Try

        Return fiscalMonth
    End Function
    Public Function GetFiscalWeekByDate(ByVal calendarDate As Date) As Integer
        Dim fiscalWeek As Integer = 0
        Dim sql As String = String.Empty

        Try
            sql = String.Format("SELECT COALESCE(FISC_WK,0) AS FISC_WK FROM {0}.TMS100FISCAL_CAL " &
                                " WHERE CAL_DATE = '{1}' FETCH FIRST 1 ROWS ONLY WITH UR ;",
                                _dbSchema, calendarDate.ToString("yyyy-MM-dd"))
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.Text, sql, Nothing)
                If rdr.HasRows Then
                    rdr.Read()
                    fiscalWeek = rdr("FISC_WK")
                End If
            End Using
        Catch ex As Exception
            Throw
        End Try

        Return fiscalWeek
    End Function
    Public Function GetFiscalYearByDate(ByVal calendarDate As Date) As Integer
        Dim fiscalYear As Integer = 0
        Dim sql As String = String.Empty

        Try
            sql = String.Format("SELECT COALESCE(FISC_YR_NUM,0) AS FISC_YR_NUM FROM {0}.TMS100FISCAL_CAL " &
                                " WHERE CAL_DATE = '{1}' FETCH FIRST 1 ROWS ONLY WITH UR ;",
                                _dbSchema, calendarDate.ToString("yyyy-MM-dd"))
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.Text, sql, Nothing)
                If rdr.HasRows Then
                    rdr.Read()
                    fiscalYear = rdr("FISC_YR_NUM")
                End If
            End Using
        Catch ex As Exception
            Throw
        End Try

        Return fiscalYear
    End Function

    Public Function GetFiscalMonthStartDate(ByVal fiscalyear As Integer, ByVal fiscalMonth As Integer) As Date
        Dim startDate As Date = Date.MinValue
        Dim sql As String = String.Empty

        Try
            sql = String.Format("SELECT  COALESCE(MIN(CAL_DATE),'0001/01/01') AS CAL_DATE FROM {0}.TMS100FISCAL_CAL " &
                                " WHERE FISC_YR_NUM = {1} AND FISC_MO = {2} WITH UR ;",
                                _dbSchema, fiscalyear, fiscalMonth)
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.Text, sql, Nothing)
                If rdr.HasRows Then
                    rdr.Read()
                    startDate = CDate(rdr("CAL_DATE"))
                End If
            End Using
        Catch ex As Exception
            Throw
        End Try

        Return startDate
    End Function

    Public Function GetFiscalMonthEndDate(ByVal fiscalyear As Integer, ByVal fiscalMonth As Integer) As Date
        Dim endDate As Date = Date.MinValue
        Dim sql As String = String.Empty

        Try
            sql = String.Format("SELECT  COALESCE(MAX(CAL_DATE),'0001/01/01') AS CAL_DATE FROM {0}.TMS100FISCAL_CAL " &
                                " WHERE FISC_YR_NUM = {1} AND FISC_MO = {2} WITH UR ;",
                                _dbSchema, fiscalyear, fiscalMonth)
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.Text, sql, Nothing)
                If rdr.HasRows Then
                    rdr.Read()
                    endDate = CDate(rdr("CAL_DATE"))
                End If
            End Using
        Catch ex As Exception
            Throw
        End Try

        Return endDate
    End Function
End Class
