
Imports TurnInProcessAutomation.MainframeDAL
Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.SqlClient

Public Class TUGXSCatalog
    Private dal As MainframeDAL.GXSCatalogDao = New MainframeDAL.GXSCatalogDao

    Public Function GetAllFromTU1135SP(ByVal isn As Decimal) As GXSCatalogInfo
        Try
            Return dal.GetAllFromTU1135SP(isn)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class


