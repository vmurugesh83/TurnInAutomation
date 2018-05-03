Imports TurnInProcessAutomation.SqlDAL
Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.SqlClient

Public Class TUCtlgAdPg
    Private dal As SqlDAL.CtlgAdPg = New SqlDAL.CtlgAdPg

    Public Function GetAllFromCtlgAdPg(ByVal AdNbr As Integer, Optional ByVal PageNbr As Integer = Nothing) As IList(Of CtlgAdPgInfo)
        Try
            Return dal.GetAllFromCtlgAdPg(AdNbr, PageNbr)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetAdPageDetail(ByVal AdNbr As Integer, ByVal PageNbr As Integer) As AdPageInfo
        Try
            Return dal.GetAdPageDetail(AdNbr, PageNbr)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub InsertCtlgAdPg(ByVal AdNbr As Integer, ByVal SysPgNbr As Integer, ByVal PgDesc As String, ByVal PgNbr As Integer, Optional ByVal tran As SqlTransaction = Nothing)
        dal.InsertCtlgAdPg(AdNbr, SysPgNbr, PgDesc, PgNbr, tran)
    End Sub

    Public Sub UpdateCtlgAdPg(ByVal CtlgAdPgInfo As CtlgAdPgInfo, Optional ByVal tran As SqlTransaction = Nothing)
        dal.UpdateCtlgAdPg(CtlgAdPgInfo, tran)
    End Sub

End Class
