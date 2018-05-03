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

Public Class ImageDao
    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")
    Private Shared _dbSchema As String = ConfigurationManager.AppSettings("DB2Schema")

    ''' <summary>
    ''' Method to get all BuyerInfo records.
    ''' </summary>	    	 
    Public Function GetAllImages(ByVal Status As String, ByVal FilterText As String) As IList(Of ImageInfo)
        Dim ImageInfos As New List(Of ImageInfo)

        Dim sql As String = _spSchema + ".TU1053SP"

        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@STATUS", DB2Type.VarChar), _
                                                          New DB2Parameter("@FILTER", DB2Type.VarChar), _
                                                          New DB2Parameter("@SCHEMA", DB2Type.VarChar)}
        parms(0).Value = IIf(Status = "", DBNull.Value, Status)
        parms(1).Value = IIf(FilterText = "", DBNull.Value, FilterText)
        parms(2).Value = _dbSchema

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new BuyerInfo object via factory method and add to list
                    ImageInfos.Add(ImageFactory.Construct(rdr))
                End While
            End Using
            Return ImageInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function
    Public Function GetRendersSwatchesByFeatureImageID(ByVal FeatureImageID As Integer) As IList(Of CopyPrioritizationImageInfo)
        Dim imageDetails As New List(Of CopyPrioritizationImageInfo)

        Dim sql As String = _spSchema + ".TU1197SP"

        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ImageID", DB2Type.Integer)}

        parms(0).Value = FeatureImageID

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    imageDetails.Add(CopyPrioritizationFactory.ConstructImages(rdr))
                End While
            End Using
            Return imageDetails
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function
End Class

