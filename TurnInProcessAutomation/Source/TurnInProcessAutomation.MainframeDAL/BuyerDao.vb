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

Partial Public Class BuyerDao

    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")

    ''' <summary>
    ''' Method to get all BuyerInfo records.
    ''' </summary>	    	 
    Public Function GetAllFromBuyer() As IList(Of BuyerInfo)
        Dim BuyerInfos As New List(Of BuyerInfo)

        Dim sql As String = _spSchema + ".TU1023SP"
        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, Nothing)
                While (rdr.Read())
                    'instantiate new BuyerInfo object via factory method and add to list
                    BuyerInfos.Add(BuyerFactory.ConstructBasic(rdr))
                End While
            End Using
            Return BuyerInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Method to get all BuyerInfo records related to CMG.
    ''' </summary>	    	 
    Public Function GetBuyersForCMG(ByVal strCMGId As String) As IList(Of BuyerInfo)
        Dim BuyerInfos As New List(Of BuyerInfo)

        Dim sql As String = _spSchema + ".TU1085SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@CMG_ID", DB2Type.SmallInt)}

        parms(0).Value = IIf(strCMGId = "", DBNull.Value, strCMGId)

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new BuyerInfo object via factory method and add to list
                    BuyerInfos.Add(BuyerFactory.ConstructBasic(rdr))
                End While
            End Using
            Return BuyerInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function
    ''' <summary>
    ''' Method to get BuyerInfo by DMM
    ''' </summary>	    	 
    Public Function GetBuyersByDMM(ByVal dmmNumber As Integer) As IList(Of BuyerInfo)
        Dim BuyerInfos As New List(Of BuyerInfo)

        Dim sql As String = _spSchema + ".TU1143SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@DMMID", DB2Type.Integer)}

        parms(0).Value = IIf(dmmNumber = 0, DBNull.Value, dmmNumber)

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new BuyerInfo object via factory method and add to list
                    BuyerInfos.Add(BuyerFactory.ConstructBasic(rdr))
                End While
            End Using
            Return BuyerInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function
End Class

