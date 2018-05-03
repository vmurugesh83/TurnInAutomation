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

Partial Public Class VendorStyleDao

    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")

    ''' <summary>
    ''' Method to get all VendorStyleInfo records.
    ''' </summary>	    	 
    Public Function GetAllFromVendorStyle(ByVal DeptId As Integer, ByVal VendorId As Integer, ByVal ClassId As Integer, ByVal SubClassId As Integer, ByVal VendorStyleNumber As String) As IList(Of VendorStyleInfo)
        Dim VendorStyleInfos As New List(Of VendorStyleInfo)

        Dim sql As String = _spSchema + ".TU1008SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {
                                                            New DB2Parameter("@DEPT_ID", DB2Type.Integer),
                                                            New DB2Parameter("@VENDOR_ID", DB2Type.Integer),
                                                            New DB2Parameter("@CLASS_ID", DB2Type.Integer),
                                                            New DB2Parameter("@SUBCLASS_ID", DB2Type.Integer),
                                                            New DB2Parameter("@VENDOR_STYLE_NUM", DB2Type.VarChar, 20)
                                                        }
        parms(0).Value = DeptId
        parms(1).Value = VendorId
        parms(2).Value = ClassId
        parms(3).Value = SubClassId
        parms(4).Value = VendorStyleNumber

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new VendorStyleInfo object via factory method and add to list
                    VendorStyleInfos.Add(VendorStyleFactory.ConstructBasic(rdr))
                End While
            End Using
            Return VendorStyleInfos
        Catch ex As Exception
            Throw
        End Try
        Return Nothing
    End Function

    Private Shared _dbSchema As String = ConfigurationManager.AppSettings("DB2Schema")

    Function GetAllFromVendorStyle(ByVal DepartmentID As Integer, ByVal VendorStyleNumber As String) As Object
        Dim VendorStyleInfos As New List(Of VendorStyleInfo)

        Dim sql As String = _spSchema + ".TU1027SP"

        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@DEPT_ID", DB2Type.Integer),
                                                        New DB2Parameter("@VENDOR_STYLE_NUM", DB2Type.VarChar),
                                                          New DB2Parameter("@SCHEMA", DB2Type.VarChar)}
        parms(0).Value = DepartmentID
        parms(1).Value = IIf(VendorStyleNumber = "", DBNull.Value, VendorStyleNumber)
        parms(2).Value = _dbSchema

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new VendorStyleInfo object via factory method and add to list
                    VendorStyleInfos.Add(VendorStyleFactory.ConstructBasic(rdr))
                End While
            End Using
            Return VendorStyleInfos
        Catch ex As Exception
            Throw
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Method to get all VendorStyleInfo records.
    ''' </summary>	    	 
    Public Function GetVendorStyleNumPrioritization(ByVal Status As String) As IList(Of VendorStyleInfo)
        Dim VendorStyleInfos As New List(Of VendorStyleInfo)

        Dim sql As String = _spSchema + ".TU1054SP"

        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@STATUS", DB2Type.VarChar)}
        parms(0).Value = IIf(Status = "", DBNull.Value, Status)

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new VendorStyleInfo object via factory method and add to list
                    VendorStyleInfos.Add(VendorStyleFactory.ConstructBasic(rdr))
                End While
            End Using
            Return VendorStyleInfos
        Catch ex As Exception
            Throw
        End Try
        Return Nothing
    End Function
End Class

