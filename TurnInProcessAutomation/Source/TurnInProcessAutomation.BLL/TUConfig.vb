Imports TurnInProcessAutomation.MainframeDAL
Imports TurnInProcessAutomation.BusinessEntities

Public Class TUConfig
    Dim tuConfigDAO As TTUConfigDAO = Nothing
    Public Sub New()
        tuConfigDAO = New TTUConfigDAO()
    End Sub
    Public Function GetConfigurationByKey(ByVal ConfigKey As String) As IList(Of TTUConfig)
        Dim turnInConfigurations As List(Of TTUConfig) = Nothing

        Try
            turnInConfigurations = tuConfigDAO.GetConfigurationByKey(ConfigKey)
            Return turnInConfigurations
        Catch ex As Exception
            Throw
        End Try
    End Function
End Class
