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

Partial Public Class WebCatDao

    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")

    ''' <summary>
    ''' Method to get all WebCat records.
    ''' </summary>	    	 
    Public Function GetWebCatByISN(ByVal ISN As Decimal) As IList(Of WebCat)
        Dim WebCats As New List(Of WebCat)

        Dim sql As String = _spSchema + ".TU1022SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ISN", DB2Type.Decimal)}
        parms(0).Value = ISN

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new WebCat object via factory method and add to list
                    WebCats.Add(WebCatFactory.Construct(rdr))
                End While
            End Using
            Return WebCats
        Catch ex As Exception
            Throw
        End Try
        Return Nothing
    End Function

    Public Function GetAllWebCat() As IList(Of WebCat)
        Dim WebCats As New List(Of WebCat)

        Dim sql As String = _spSchema + ".TU1104SP"

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, Nothing)
                While (rdr.Read())
                    'instantiate new WebCat object via factory method and add to list
                    WebCats.Add(WebCatFactory.Construct(rdr))
                End While
            End Using
            Return WebCats
        Catch ex As Exception
            Throw
        End Try
        Return Nothing
    End Function

    Public Function GetFeatureWebCat() As IList(Of WebCat)
        Dim WebCats As New List(Of WebCat)

        Dim sql As String = _spSchema + ".TU1093SP"

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, Nothing)
                While (rdr.Read())
                    'instantiate new WebCat object via factory method and add to list
                    WebCats.Add(WebCatFactory.Construct(rdr))
                End While
            End Using
            Return WebCats
        Catch ex As Exception
            Throw
        End Try
        Return Nothing
    End Function

    Public Function GetWebCatByParentCde(ByVal parentCde As Integer) As IList(Of WebCat)
        Dim WebCats As IList(Of WebCat) = New List(Of WebCat)
        Dim sql As String = _spSchema + ".TU1016SP"

        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@PARENT_CDE", DB2Type.Integer, 4)}
        parms(0).Value = parentCde

        'Execute a query to read the WebCats
        Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
            While (rdr.Read())
                'instantiate new WebCat object via factory method
                WebCats.Add(WebCatFactory.Construct(rdr))
            End While
        End Using
        Return WebCats
    End Function


    Public Function GetProductAndUPCDetailsByProductCode(ByVal productCode As Integer) As IList(Of WebcatProductInfo)
        Dim productInfo As New List(Of WebcatProductInfo)

        Dim sql As String = _spSchema + ".TU1199SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@PRODUCT_CDE", DB2Type.Integer)}
        parms(0).Value = productCode

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    'instantiate new WebCat object via factory method and add to list
                    productInfo.Add(WebCatFactory.ConstructProductAndUPC(rdr))
                End While
            End Using
        Catch ex As Exception
            Throw
        End Try
        Return productInfo
    End Function
End Class

