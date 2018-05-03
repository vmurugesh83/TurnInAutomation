Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities

Public Class SellYearFactory

    Public Shared Function ConstructBasic(ByVal reader As DB2DataReader) As SellYearInfo
        Dim SellYearInfo As New SellYearInfo()

        Dim col1 As Integer = reader.GetOrdinal("SEASON_YR_NUM")
        If Not reader.Item(col1) Is Nothing Then
            SellYearInfo.SellYear = CStr(reader.GetValue(col1))
        End If

        Return SellYearInfo
    End Function
End Class

