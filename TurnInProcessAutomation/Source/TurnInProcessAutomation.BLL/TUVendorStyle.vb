Imports TurnInProcessAutomation.MainframeDAL
Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.SqlClient

Public Class TUVendorStyle
    Private dal As MainframeDAL.VendorStyleDao = New MainframeDAL.VendorStyleDao



    Public Function GetAllFromVendorStyle(ByVal DeptId As Integer, ByVal VendorId As String, ByVal ClassId As String, ByVal SubClassId As String, ByVal VendorStyleNumber As String) As IList(Of VendorStyleInfo)
        Try
            If (ClassId = "") Then
                ClassId = 0
            End If
            If (SubClassId = "") Then
                SubClassId = 0
            End If
            If (VendorId = "") Then
                VendorId = 0
            End If
            Return dal.GetAllFromVendorStyle(DeptId, VendorId, ClassId, SubClassId, VendorStyleNumber)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetAllFromVendorStyle(ByVal DepartmentID As Integer, ByVal VendorStyleNumber As String) As Object
        Try
            Return dal.GetAllFromVendorStyle(DepartmentID, VendorStyleNumber)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetVendorStyleNumPrioritization(ByVal SelectedStatus As String) As IList(Of VendorStyleInfo)
        Try
            Return dal.GetVendorStyleNumPrioritization(SelectedStatus)
        Catch ex As Exception
            Throw ex
        End Try
    End Function


End Class
