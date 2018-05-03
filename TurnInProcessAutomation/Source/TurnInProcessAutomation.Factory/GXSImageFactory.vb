
Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory.Common

'Imports BonTon.DBUtility
'Imports BonTon.DBUtility.SqlHelper
Imports System.Data.SqlClient
Imports TurnInProcessAutomation.Factory

Public Class GXSImageFactory

    Public Shared Function Construct(ByVal reader As DB2DataReader) As GXSImageInfo
        Dim GXSImageInfo As New GXSImageInfo()
        With GXSImageInfo
            .ID = CDec(ReadColumn(reader, "ID"))
            .LargeURL = CStr(ReadColumn(reader, "Large URL"))
            .SmallURL = CStr(ReadColumn(reader, "Small URL"))
        End With
        Return GXSImageInfo
    End Function

    Public Shared Function Construct(ByVal reader As SqlDataReader) As GXSImageInfo
        Dim GXSImageInfo As New GXSImageInfo()
        With GXSImageInfo
            .ID = CDec(ReadColumn(reader, "ID"))
            .LargeURL = CStr(ReadColumn(reader, "Large URL"))
            .SmallURL = CStr(ReadColumn(reader, "Small URL"))
        End With
        Return GXSImageInfo

    End Function

End Class
