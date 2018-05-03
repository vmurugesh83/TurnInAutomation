Imports System.Configuration
Imports log4net
Imports BonTon.DBUtility.DB2Helper
Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory

Imports System.Web

Public Class GlobalObjectDao

    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")

    Public Sub GetAllApplicationObjects()
        Dim applWebCats As New List(Of WebCat)

        Dim sql As String = _spSchema + ".TU1104SP"

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, Nothing)
                While (rdr.Read())
                    'instantiate new WebCat object via factory method and add to list
                    applWebCats.Add(WebCatFactory.Construct(rdr))
                End While
            End Using

            'load data into application objects
            HttpContext.Current.Application("ApplWebCatsObject") = applWebCats
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

End Class
