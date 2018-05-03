Imports TurnInProcessAutomation.MainframeDAL
Imports TurnInProcessAutomation.BusinessEntities

Public Class TUBatch
    Private dal As MainframeDAL.BatchDao = New MainframeDAL.BatchDao
    Private dalSQLBatch As SqlDAL.AdminDataDao = New SqlDAL.AdminDataDao

    Public Function GetAllBatches(Status As String) As IList(Of BatchInfo)
        Try
            Return dal.GetAllBatches(Status)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetBatches(ByVal strAdNbr As String, ByVal strPageNbr As String, _
                               ByVal strBuyerID As String, ByVal strDeptID As String, _
                               ByVal strLblID As String, ByVal strVndrStylID As String, _
                               ByVal strEMMID As String) As IList(Of BatchInfo)
        Try
            Return dal.GetBatches(strAdNbr, strPageNbr, strBuyerID, strDeptID, strLblID, strVndrStylID, strEMMID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function IsBatchExists(ByVal AdNumber As Integer, ByVal PageNumber As Integer, ByVal UserId As String) As Boolean
        Try
            Return dal.GetBatchesByUser(AdNumber, PageNumber, UserId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetMaintenanceBatches(ByVal AdNumber As Integer, ByVal PageNumber As Integer, ByVal BuyerId As String, ByVal DeptId As String, ByVal BatchId As String) As List(Of BatchInfo)
        Try
            Return dal.GetMaintenanceBatches(AdNumber, PageNumber, BuyerId, DeptId, BatchId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub UpdateBatchSequence(ByVal BatchId As Integer, ByVal ISN As Decimal, ByVal Sequence As Integer)
        Try
            dal.UpdateBatchSequence(BatchId, ISN, Sequence)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function UpdateBatchNumber(ByVal BatchId As Integer, ByVal oldAdNum As Integer, ByVal newAdNum As Integer) As Integer
        Try
            Dim merchIds As List(Of String) = dal.UpdateBatchNumber(BatchId, oldAdNum, newAdNum)
            If merchIds.Count > 0 Then
                dalSQLBatch.UpdateBatchNumber(newAdNum, merchIds)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub UpdateColorSequence(ByVal MerchId As Integer, ByVal ColorSequence As Integer)
        Try
            dal.UpdateColorSequence(MerchId, ColorSequence)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class
