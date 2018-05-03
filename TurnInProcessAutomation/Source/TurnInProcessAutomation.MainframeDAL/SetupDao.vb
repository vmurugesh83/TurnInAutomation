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

Partial Public Class SetupDao

    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")

    Public Sub SaveAdSetup(ByVal categoryCde As Integer, ByVal parentCde As Integer, ByVal startDte As DateTime, ByVal endDte As DateTime, ByVal activeFlg As String, ByVal activeDte As DateTime?, ByVal ordinalNum As Integer, ByVal imageIdNum As Integer, ByVal displayOnlyFlg As String, ByVal sesTitle As String, ByVal createDte As DateTime, ByVal modifyDte As DateTime, ByVal logonUser As String, ByVal parentDefaultFlg As String, ByVal sesUrlValue As String, ByVal inactiveTs As DateTime?, ByVal categoryDesc As String, ByVal categoryNme As String, ByVal sesMetaDesc As String, ByVal sesMetaKeyWords As String, ByVal templateId As Integer, ByVal revenueTierCde As Integer, ByVal dfltSeoTitleNme As String, ByVal cusSeoTitleNme As String, ByVal seoTitleCde As Integer, ByVal addTitleNameCde As Integer, ByVal defaultSeoDesc As String, ByVal customSeoDesc As String, ByVal seoDescCde As Integer, ByVal addDescNameCde As Integer, ByVal addShipInfoCde As Integer, ByVal seoShipInfoTxt As String, ByVal affilCategoryTxt As String, ByVal catgySrtOrdrNum As Integer, ByVal disColorFamCde As Integer, ByVal disSizeFamCde As Integer)
        ' ' Get each commands parameter arrays 
        '' Dim parms As DB2Parameter() = GetInsertParameters()
        ' Dim sql As String = _spSchema + ".XX0000SP"

        ' ' Set up the parameters 
        ' parms(0).Value = categoryCde
        ' parms(1).Value = parentCde
        ' parms(2).Value = startDte
        ' parms(3).Value = endDte
        ' parms(4).Value = activeFlg
        ' parms(5).Value = activeDte
        ' parms(6).Value = ordinalNum
        ' parms(7).Value = imageIdNum
        ' parms(8).Value = displayOnlyFlg
        ' parms(9).Value = sesTitle
        ' parms(10).Value = createDte
        ' parms(11).Value = modifyDte
        ' parms(12).Value = logonUser
        ' parms(13).Value = parentDefaultFlg
        ' parms(14).Value = sesUrlValue
        ' parms(15).Value = inactiveTs
        ' parms(16).Value = categoryDesc
        ' parms(17).Value = categoryNme
        ' parms(18).Value = sesMetaDesc
        ' parms(19).Value = sesMetaKeyWords
        ' parms(20).Value = templateId
        ' parms(21).Value = revenueTierCde
        ' parms(22).Value = dfltSeoTitleNme
        ' parms(23).Value = cusSeoTitleNme
        ' parms(24).Value = seoTitleCde
        ' parms(25).Value = addTitleNameCde
        ' parms(26).Value = defaultSeoDesc
        ' parms(27).Value = customSeoDesc
        ' parms(28).Value = seoDescCde
        ' parms(29).Value = addDescNameCde
        ' parms(30).Value = addShipInfoCde
        ' parms(31).Value = seoShipInfoTxt
        ' parms(32).Value = affilCategoryTxt
        ' parms(33).Value = catgySrtOrdrNum
        ' parms(34).Value = disColorFamCde
        ' parms(35).Value = disSizeFamCde

        ' ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
    End Sub
End Class

