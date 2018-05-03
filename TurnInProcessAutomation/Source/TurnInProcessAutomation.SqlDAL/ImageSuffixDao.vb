Imports System.Configuration
Imports System.Collections.Generic
Imports System.Reflection
Imports System.Text
Imports log4net
Imports BonTon.DBUtility
Imports BonTon.DBUtility.SqlHelper
Imports System.Data.SqlClient
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory

Partial Public Class ImageSuffixDao
    Public Function GetAllFromImageSuffix() As IList(Of ImageSuffixInfo)

        Dim ImagesuffixInfos As IList(Of ImageSuffixInfo) = New List(Of ImageSuffixInfo)()

        'Execute a query to read the ImageSuffixInfos
        Using rdr As SqlDataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, "tu_get_image_suffix", Nothing)
            While (rdr.Read())
                'instantiate new ImageSuffixInfo object via factory method and add to list
                ImagesuffixInfos.Add(ImageSuffixFactory.Construct(rdr))
            End While
        End Using
        Return ImagesuffixInfos
    End Function
End Class
