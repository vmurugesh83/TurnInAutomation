
Imports TurnInProcessAutomation.BLL.CommonBO
Imports TurnInProcessAutomation.MainframeDAL
Imports TurnInProcessAutomation.SqlDAL
Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.SqlClient
Imports System.Xml.Linq
Public Class TUEcommQueryTool
    Private dalDB2 As MainframeDAL.ECommTurnInQueryToolDao = New MainframeDAL.ECommTurnInQueryToolDao
    Private dalSQL As SqlDAL.IsTurnedInDao = New SqlDAL.IsTurnedInDao
    Private dalSQLImageSuffix As SqlDAL.ImageSuffixDao = New SqlDAL.ImageSuffixDao
    Private dalSQLCtlgAdPg As SqlDAL.CtlgAdPg = New SqlDAL.CtlgAdPg

    Function GetEcommQueryResult(ByVal crgid As String, ByVal cmgid As String, ByVal cfgid As String, ByVal buyer As String, ByVal fobid As String, ByVal DeptID As String, ByVal classid As String, ByVal acode As String, ByVal vendorid As String, ByVal VndrStylID As String, ByVal adnumber As String, ByVal tistatusid As String, ByVal tintyp As String, ByVal tidatefrm As String, ByVal tidateto As String, ByVal viewid As String _
                                 , ByVal BatchNum As String) As List(Of ECommTurnInQueryToolInfo)
        Try
            Dim DB2Results As IList(Of ECommTurnInQueryToolInfo) = dalDB2.GetEcommQueryResult(crgid, cmgid, cfgid, buyer, fobid, DeptID, classid, acode, vendorid, VndrStylID, adnumber, tistatusid, tintyp, tidatefrm, tidateto, viewid, BatchNum)

            If Not DB2Results Is Nothing AndAlso DB2Results.Count > 0 Then
                'Add TurnInDate field, Where Clause for TurnInDate, and Orderby Clause
                Return DB2Results.Where(Function(y) IIf(tidatefrm > Date.MinValue, y.Turn_in_Date >= tidatefrm, True) And IIf(tidateto < Date.MaxValue, y.Turn_in_Date <= tidateto, True)) _
                                                .OrderBy(Function(b) b.Feature_Render_Swatch) _
                                                .ThenBy(Function(d) d.Feature_Image_ID).ToList()
            Else
                Return DB2Results
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetEcommPreMediaResult(ByVal crgid As String, ByVal cmgid As String, ByVal cfgid As String, ByVal buyer As String, ByVal fobid As String, ByVal DeptID As String, ByVal classid As String, ByVal acode As String, ByVal vendorid As String, ByVal VndrStylID As String, ByVal adnumber As String, ByVal pagenumber As String, ByVal tistatusid As String, ByVal tintyp As String, ByVal tidatefrm As Date, ByVal tidateto As Date, ByVal InWebCat As String, ByVal Suffix As String, ByVal ImageType As String, ByVal ModelCategoryCode As String, ByVal FeatureWebCat As String, ByVal BatchNum As String) As List(Of ECommTurnInQueryToolInfo)
        Try
            Dim DB2Results As IList(Of ECommTurnInQueryToolInfo) = dalDB2.GetEcommPreMediaResult(crgid, cmgid, cfgid, buyer, fobid, DeptID, classid, acode, vendorid, VndrStylID, adnumber, pagenumber, tistatusid, tintyp, InWebCat, ImageType, ModelCategoryCode, FeatureWebCat, BatchNum)
            If Not DB2Results Is Nothing AndAlso DB2Results.Count > 0 Then
                DB2Results = DB2Results.Where(Function(x) x.ImageSuffix = Suffix Or Suffix = "").ToList
                'Add TurnInDate field, Where Clause for TurnInDate, and Orderby Clause
                Return DB2Results.Where(Function(y) IIf(tidatefrm > Date.MinValue, y.Turn_in_Date >= tidatefrm, True) And IIf(tidateto < Date.MaxValue, y.Turn_in_Date <= tidateto, True)) _
                                                .OrderBy(Function(b) b.Feature_Render_Swatch) _
                                                .ThenBy(Function(d) d.Feature_Image_ID).ToList()
            Else
                Return DB2Results
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Sub UpdateStatus(ByVal TurnInMerchIDs As String, ByVal strStatusCde As String, ByVal UserId As String)
        Try
            dalDB2.UpdateStatus(TurnInMerchIDs, strStatusCde, UserId)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function GetUPCReport(ByVal strTUWeek As String) As IList(Of AdPageInfo)
        Try
            Dim DB2Results As IList(Of AdPageInfo) = dalDB2.GetUPCReport(strTUWeek)
            Return DB2Results
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
