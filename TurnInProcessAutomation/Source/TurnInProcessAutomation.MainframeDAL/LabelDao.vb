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

Partial Public Class LabelDao

    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")

    ''' <summary>
    ''' Method to get all LabelInfo records.
    ''' </summary>	    	 
    Public Function GetLabelsByBrand(ByVal BrandId As String) As IList(Of LabelInfo)
        Dim LabelInfos As New List(Of LabelInfo)

        Dim sql As String = _spSchema + ".TU1009SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@BRAND_ID", DB2Type.Integer)}
        parms(0).Value = IIf(BrandId = "", DBNull.Value, BrandId)

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new LabelInfo object via factory method and add to list
                    LabelInfos.Add(LabelFactory.Construct(rdr))
                End While
            End Using
            Return LabelInfos
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

End Class

