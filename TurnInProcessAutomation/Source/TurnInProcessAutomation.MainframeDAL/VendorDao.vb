Imports System.Configuration
Imports System.Collections.Generic
Imports System.Reflection
Imports System.Text
Imports log4net
Imports BonTon.DBUtility
Imports BonTon.DBUtility.DB2Helper
Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory

Partial Public Class VendorDao

    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")

    ''' <summary>
    ''' Method to get all VendorInfo records.
    ''' </summary>	    	 
    Public Function GetAllFromVendorByDepartment(ByVal DeptId As Integer) As IList(Of VendorInfo)
        Dim VendorInfos As New List(Of VendorInfo)

        Dim sql As String = _spSchema + ".TU1004SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@DEPT_ID", DB2Type.Integer)}
        parms(0).Value = DeptId

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new VendorInfo object via factory method and add to list
                    VendorInfos.Add(VendorFactory.ConstructBasic(rdr))
                End While
            End Using
            Return VendorInfos
        Catch ex As Exception
            Throw
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Method to get external vendor number by the upc number
    ''' </summary>	    	 
    Public Function GetExternalVendorNumberByUPC(ByVal UPC As Decimal) As Long
        Dim externalVendorNumber As Long = 0

        Dim sql As String = _spSchema + ".TU1198SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@UPC", DB2Type.Decimal)}
        parms(0).Value = UPC

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                If rdr.HasRows Then
                    rdr.Read()
                    If Common.HasColumn(rdr, "VEN_EXT_NUM") Then
                        externalVendorNumber = CType(rdr("VEN_EXT_NUM"), Long)
                    End If
                End If
            End Using
        Catch ex As Exception
            Throw
        End Try
        Return externalVendorNumber
    End Function

End Class

