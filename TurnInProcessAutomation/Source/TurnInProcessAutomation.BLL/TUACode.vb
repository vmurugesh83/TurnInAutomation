Imports TurnInProcessAutomation.MainframeDAL
Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.SqlClient

Public Class TUVendor
    Private dal As MainframeDAL.VendorDao = New MainframeDAL.VendorDao

    Public Function GetAllFromVendorByDepartment(ByVal deptId As Integer) As IList(Of VendorInfo)
        Try
            Return dal.GetAllFromVendorByDepartment(deptId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetExternalVendorNumberByUPC(ByVal UPC As Decimal) As Long
        Try
            Return dal.GetExternalVendorNumberByUPC(UPC)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
