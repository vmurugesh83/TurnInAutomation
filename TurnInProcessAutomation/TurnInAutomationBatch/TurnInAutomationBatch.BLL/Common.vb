Imports System.Xml
Imports System.Configuration
Imports TurnInAutomationBatch.BusinessEntities
Imports TurnInAutomationBatch.MainframeDAL
Public Class Common
    Dim defaultValuesConfigurationFileName = ConfigurationManager.AppSettings("DefaultValuesConfigurationFileName")
    Dim commonDAO As CommonDAO
    Public Sub New()
        commonDAO = New CommonDAO()
    End Sub
    ''' <summary>
    ''' Loads the default values XML and reads the default values from the XML for the XPath
    ''' </summary>
    ''' <param name="xPath">XPath of the table name defined in the default values XML</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDefaultValues(ByVal xPath As String) As XmlNode
        Dim turnInDetaultValues As XmlElement = Nothing
        Dim defaultValuesDocument As XmlDocument = Nothing

        Try
            defaultValuesDocument = New XmlDocument()
            defaultValuesDocument.Load(defaultValuesConfigurationFileName)
            turnInDetaultValues = defaultValuesDocument.GetElementsByTagName(xPath)(0)
            Return turnInDetaultValues
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    ''' <summary>
    ''' Writes to the turn in automation log table in DB2
    ''' </summary>
    ''' <param name="TurnInBatchID"></param>
    ''' <param name="TurnInItemTypeID"></param>
    ''' <param name="TurnInItemType"></param>
    ''' <param name="BatchStatusCode"></param>
    ''' <param name="BatchStatusMessage"></param>
    ''' <param name="LastModifiedBy"></param>
    ''' <remarks></remarks>
    Public Sub WriteToLogTable(ByVal TurnInBatchID As Integer, ByVal TurnInItemTypeID As Integer, ByVal TurnInItemType As String, ByVal BatchStatusCode As String, ByVal BatchStatusMessage As String, ByVal LastModifiedBy As String)
        Dim logDetail As LogDetail = Nothing
        Try
            logDetail = New LogDetail()

            logDetail.TurnInBatchID = TurnInBatchID
            logDetail.TurnInItemTypeID = TurnInItemTypeID
            logDetail.TurnInItemType = TurnInItemType
            logDetail.BatchStatusCode = BatchStatusCode
            logDetail.BatchStatusMessage = BatchStatusMessage
            logDetail.LastModifiedBy = LastModifiedBy

            commonDAO.WriteToLogTable(logDetail)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function GetTIAEnabledDepartments(ByVal ConfigurationKey As String) As List(Of DeptPageNumber)
        Dim departments As List(Of DeptPageNumber) = Nothing
        Try
            departments = commonDAO.GetTIAEnabledDepartments(ConfigurationKey)
        Catch ex As Exception
            Throw ex
        End Try
        Return departments
    End Function
    Public Sub GetRecipients(ByVal configurationKeyName As String, recipients As List(Of String))
        Dim emailAddresses As String = ConfigurationManager.AppSettings(configurationKeyName)
        Try

            If Not String.IsNullOrEmpty(emailAddresses) Then
                For Each emailAddress As String In emailAddresses.Split(",").ToList()
                    recipients.Add(emailAddress)
                Next
            Else
                Throw New Exception(String.Format("No email address is configured for the key : {0}.", configurationKeyName))
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Sub
End Class
