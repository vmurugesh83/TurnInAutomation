Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities

Public Class VendorStyleFactory

    Public Shared Function ConstructBasic(ByVal reader As DB2DataReader) As VendorStyleInfo
        Dim VendorStyleInfo As New VendorStyleInfo()

        Dim col1 As Integer = reader.GetOrdinal("VENDOR_STYLE_NUM")
        If Not reader.Item(col1) Is Nothing Then
            VendorStyleInfo.VendorStyleNumber = CStr(reader.GetValue(col1))
        End If

        'Dim col2 As Integer = reader.GetOrdinal("IS_RESERVE")
        'If Not reader.Item(col2) Is Nothing Then
        '    VendorStyleInfo.IsReserve = CStr(reader.GetValue(col2))
        'End If

        Return VendorStyleInfo
    End Function
End Class

