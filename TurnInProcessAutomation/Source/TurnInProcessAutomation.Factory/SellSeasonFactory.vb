Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities

Public Class SellSeasonFactory

    Public Shared Function ConstructBasic(ByVal reader As DB2DataReader) As SellSeasonInfo
        Dim SellSeasonInfo As New SellSeasonInfo()

        Dim col1 As Integer = reader.GetOrdinal("SEASON_ID")
        If Not reader.Item(col1) Is Nothing Then
            SellSeasonInfo.SellSeasonId = CInt(reader.GetValue(col1))
        End If

        Dim col2 As Integer = reader.GetOrdinal("SEASON_DESC")
        If Not reader.Item(col2) Is Nothing Then
            SellSeasonInfo.SellSeasonDesc = CStr(reader.GetValue(col2))
        End If

        Return SellSeasonInfo
    End Function
End Class

