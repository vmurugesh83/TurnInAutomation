Imports System.Data.SqlClient
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory.Common

Public Class ImageSuffixFactory

    Public Shared Function Construct(ByVal reader As SqlDataReader) As ImageSuffixInfo
        Dim ImagesuffixInfo As New ImageSuffixInfo()

        With ImagesuffixInfo
            .imgsfxcd = CStr(ReadColumn(reader, "img_sfx_cd"))
            .imgsfxdesc = CStr(ReadColumn(reader, "img_sfx_desc"))
        End With

        Return ImagesuffixInfo
    End Function
End Class

