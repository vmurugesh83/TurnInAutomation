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

Public Class TEC998PARMDao
    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")

    ''' <summary>
    ''' Method to get all TEC998PARMInfo records.
    ''' </summary>	    	 
    Public Function GetAll998ParmValues(ByVal EntryText As String) As IList(Of TEC998PARMInfo)
        Dim TEC998PARMInfos As New List(Of TEC998PARMInfo)

        Dim sql As String = _spSchema + ".TU1047SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ENTRY_TEXT", DB2Type.VarChar)}
        parms(0).Value = EntryText

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new TEC998PARMInfo object via factory method and add to list
                    TEC998PARMInfos.Add(TEC998PARMFactory.Construct(rdr))
                End While
            End Using
            Return TEC998PARMInfos
        Catch ex As Exception
            Throw
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Method to get all TEC998PARMInfo records.
    ''' </summary>	    	 
    Public Function GetAll998DropShipValues(ByVal ColName As String) As IList(Of TEC998PARMInfo)
        Dim TEC998PARMInfos As New List(Of TEC998PARMInfo)

        Dim sql As String = _spSchema + ".TU1061SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@COL_NAME", DB2Type.VarChar)}
        parms(0).Value = ColName

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new TEC998PARMInfo object via factory method and add to list
                    TEC998PARMInfos.Add(TEC998PARMFactory.Construct(rdr))
                End While
            End Using
            Return TEC998PARMInfos
        Catch ex As Exception
            Throw
        End Try
        Return Nothing
    End Function
End Class
