Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory.Common
Public Class LocationFactory
    Public Shared Function Construct(ByVal reader As DB2DataReader) As LocationInfo
        Dim LocInfo As New LocationInfo()
        With LocInfo
            .DTE_MNT_TS = CStr(ReadColumn(reader, "DATE_MAINT_TS"))
            .LOC_ID = CInt(ReadColumn(reader, "LOC_ID"))
            .LOC_NME = Trim(CStr(ReadColumn(reader, "LOC_NME")))
            .TYP_CDE = CStr(ReadColumn(reader, "TYP_CDE"))
            .TYP_SUB_CDE = CStr(ReadColumn(reader, "TYP_SUB_CDE"))
            If HasColumn(reader, "STOR_ADDR") Then
                .STOR_ADDR = Trim(CStr(reader.Item("STOR_ADDR")))
            End If
            If HasColumn(reader, "STOR_CITY_ADDR") Then
                .STOR_CITY_ADDR = Trim(CStr(reader.Item("STOR_CITY_ADDR")))
            End If
            If HasColumn(reader, "STOR_STAT_ADDR") Then
                .STOR_STAT_ADDR = Trim(CStr(reader.Item("STOR_STAT_ADDR")))
            End If
            If HasColumn(reader, "STOR_ZIP_ADDR") Then
                .STOR_ZIP_ADDR = Trim(CStr(reader.Item("STOR_ZIP_ADDR")))
            End If
            If HasColumn(reader, "COUNTRY") Then
                .COUNTRY = CStr(reader.Item("COUNTRY"))
            End If
        End With
        Return LocInfo
    End Function
End Class
