Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities

Public Class VendorFactory

    Public Shared Function ConstructBasic(ByVal reader As DB2DataReader) As VendorInfo
        Dim VendorInfo As New VendorInfo()

        Dim col1 As Integer = reader.GetOrdinal("Vendor_ID")
        If Not reader.Item(col1) Is Nothing Then
            VendorInfo.VendorId = CInt(reader.GetValue(col1))
        End If

        Dim col2 As Integer = reader.GetOrdinal("VENDOR_NME")
        If Not reader.Item(col2) Is Nothing Then
            VendorInfo.VendorName = CStr(reader.GetValue(col2))
        End If

        Return VendorInfo
    End Function
End Class

