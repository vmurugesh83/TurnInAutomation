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
Public Class ECommTurnInQueryToolDao
    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")
    Private Shared _dbSchema As String = ConfigurationManager.AppSettings("DB2Schema")

    Public Function GetEcommQueryResult(ByVal crgid As String, ByVal cmgid As String, ByVal cfgid As String,
                                        ByVal buyer As String, ByVal fobid As String, ByVal DeptID As String,
                                        ByVal classid As String, ByVal acode As String, ByVal vendorid As String,
                                        ByVal VndrStylID As String, ByVal adnumber As String, ByVal tistatusid As String,
                                        ByVal tintyp As String, ByVal tidatefrm As String, ByVal tidateto As String, ByVal viewid As String _
                                        , ByVal BatchNum As String) As List(Of ECommTurnInQueryToolInfo)
        Dim _ECommTurnInQueryToolInfo As New List(Of ECommTurnInQueryToolInfo)
        Try
            Dim sql As String = _spSchema + ".TU1060SP "
            Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@crgid", DB2Type.SmallInt), _
                                                              New DB2Parameter("@cmgid", DB2Type.SmallInt), _
                                                              New DB2Parameter("@cfgid", DB2Type.SmallInt), _
                                                              New DB2Parameter("@BuyerId", DB2Type.SmallInt), _
                                                              New DB2Parameter("@fobid", DB2Type.SmallInt), _
                                                              New DB2Parameter("@DeptID", DB2Type.SmallInt), _
                                                              New DB2Parameter("@classid", DB2Type.SmallInt), _
                                                              New DB2Parameter("@acode", DB2Type.Char), _
                                                              New DB2Parameter("@vendorid", DB2Type.Integer), _
                                                              New DB2Parameter("@VndrStylID", DB2Type.Char), _
                                                              New DB2Parameter("@adnumber", DB2Type.Integer), _
                                                              New DB2Parameter("@tistatusid", DB2Type.Char), _
                                                              New DB2Parameter("@tintyp", DB2Type.SmallInt), _
                                                              New DB2Parameter("@tidatefrm", DB2Type.Date), _
                                                              New DB2Parameter("@tidateto", DB2Type.Date), _
                                                              New DB2Parameter("@viewid", DB2Type.SmallInt), _
                                                              New DB2Parameter("@batchnum", DB2Type.Integer), _
                                                              New DB2Parameter("@SCHEMA", DB2Type.VarChar)}
            If Not String.IsNullOrEmpty(crgid) Then
                parms(0).Value = Convert.ToInt32(crgid)
            Else
                parms(0).Value = 0
            End If
            If Not String.IsNullOrEmpty(cmgid) Then
                parms(1).Value = Convert.ToInt32(cmgid)
            Else
                parms(1).Value = 0
            End If
            If Not String.IsNullOrEmpty(cfgid) Then
                parms(2).Value = Convert.ToInt32(cfgid)
            Else
                parms(2).Value = 0
            End If
            If Not String.IsNullOrEmpty(buyer) Then
                parms(3).Value = Convert.ToInt32(buyer)
            Else
                parms(3).Value = 0
            End If
            If Not String.IsNullOrEmpty(fobid) Then
                parms(4).Value = Convert.ToInt32(fobid)
            Else
                parms(4).Value = 0
            End If
            If Not String.IsNullOrEmpty(DeptID) And DeptID <> "0" Then
                parms(5).Value = Convert.ToInt32(DeptID)
            Else
                parms(5).Value = 0
            End If
            If Not String.IsNullOrEmpty(classid) And classid <> "0" Then
                parms(6).Value = Convert.ToInt32(classid)
            Else
                parms(6).Value = 0
            End If
            If Not String.IsNullOrEmpty(acode) Then
                parms(7).Value = acode
            Else
                parms(7).Value = String.Empty
            End If
            If Not String.IsNullOrEmpty(vendorid) And vendorid <> "0" Then
                parms(8).Value = Convert.ToInt32(vendorid)
            Else
                parms(8).Value = 0
            End If
            If Not String.IsNullOrEmpty(VndrStylID) Then
                parms(9).Value = VndrStylID
            Else
                parms(9).Value = String.Empty
            End If
            If Not String.IsNullOrEmpty(adnumber) Then
                parms(10).Value = Convert.ToInt32(adnumber)
            Else
                parms(10).Value = 0
            End If
            If Not String.IsNullOrEmpty(tistatusid) Then
                parms(11).Value = tistatusid
            Else
                parms(11).Value = String.Empty
            End If
            If Not String.IsNullOrEmpty(tintyp) Then
                parms(12).Value = Convert.ToInt32(tintyp)
            Else
                parms(12).Value = 0
            End If
            If Not String.IsNullOrEmpty(CStr(tidatefrm)) Then
                parms(13).Value = CDate(tidatefrm)
            Else
                parms(13).Value = Date.MinValue
            End If
            If Not String.IsNullOrEmpty(tidateto) Then
                parms(14).Value = CDate(tidateto)
            Else
                parms(14).Value = Date.MaxValue
            End If
            If Not String.IsNullOrEmpty(viewid) Then
                parms(15).Value = Convert.ToInt32(viewid)
            Else
                parms(15).Value = 0
            End If
            If Not String.IsNullOrEmpty(BatchNum) Then
                parms(16).Value = Convert.ToInt32(BatchNum)
            Else
                parms(16).Value = 0
            End If

            parms(17).Value = _dbSchema

            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    _ECommTurnInQueryToolInfo.Add(ECommTurnInQueryToolFactory.Construct(rdr))
                End While
            End Using
        Catch ex As Exception
            Throw ex
        End Try
        Return _ECommTurnInQueryToolInfo
    End Function

    Public Function GetEcommPreMediaResult(ByVal crgid As String, ByVal cmgid As String, ByVal cfgid As String, ByVal buyer As String,
                                           ByVal fobid As String, ByVal DeptID As String, ByVal classid As String, ByVal acode As String,
                                           ByVal vendorid As String, ByVal VndrStylID As String, ByVal adnumber As String, ByVal pagenumber As String,
                                           ByVal tistatusid As String, ByVal tintyp As String, ByVal InWebCat As String, ByVal ImageType As String,
                                           ByVal ModelCategoryCode As String, ByVal FeatureWebCat As String, ByVal BatchNum As String) As List(Of ECommTurnInQueryToolInfo)
        Dim _ECommTurnInQueryToolInfo As New List(Of ECommTurnInQueryToolInfo)
        Try
            Dim sql As String = _spSchema + ".TU1079SP "
            Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@crgid", DB2Type.SmallInt), _
                                                              New DB2Parameter("@cmgid", DB2Type.SmallInt), _
                                                              New DB2Parameter("@cfgid", DB2Type.SmallInt), _
                                                              New DB2Parameter("@BuyerId", DB2Type.SmallInt), _
                                                              New DB2Parameter("@fobid", DB2Type.SmallInt), _
                                                              New DB2Parameter("@DeptID", DB2Type.SmallInt), _
                                                              New DB2Parameter("@classid", DB2Type.SmallInt), _
                                                              New DB2Parameter("@acode", DB2Type.Char), _
                                                              New DB2Parameter("@vendorid", DB2Type.Integer), _
                                                              New DB2Parameter("@VndrStylID", DB2Type.Char), _
                                                              New DB2Parameter("@adnumber", DB2Type.Integer), _
                                                              New DB2Parameter("@tistatusid", DB2Type.Char), _
                                                              New DB2Parameter("@tintyp", DB2Type.SmallInt), _
                                                              New DB2Parameter("@InWebCat", DB2Type.VarChar), _
                                                              New DB2Parameter("@PageNumber", DB2Type.Integer), _
                                                              New DB2Parameter("@OnOffFigure", DB2Type.VarChar), _
                                                              New DB2Parameter("@ModelCategoryCode", DB2Type.VarChar), _
                                                              New DB2Parameter("@FeatureWebCat", DB2Type.VarChar), _
                                                              New DB2Parameter("@batchnum", DB2Type.Integer)}
            If Not String.IsNullOrEmpty(crgid) Then
                parms(0).Value = Convert.ToInt32(crgid)
            Else
                parms(0).Value = DBNull.Value
            End If
            If Not String.IsNullOrEmpty(cmgid) Then
                parms(1).Value = Convert.ToInt32(cmgid)
            Else
                parms(1).Value = DBNull.Value
            End If
            If Not String.IsNullOrEmpty(cfgid) Then
                parms(2).Value = Convert.ToInt32(cfgid)
            Else
                parms(2).Value = DBNull.Value
            End If
            If Not String.IsNullOrEmpty(buyer) Then
                parms(3).Value = Convert.ToInt32(buyer)
            Else
                parms(3).Value = DBNull.Value
            End If
            If Not String.IsNullOrEmpty(fobid) Then
                parms(4).Value = Convert.ToInt32(fobid)
            Else
                parms(4).Value = DBNull.Value
            End If
            If Not String.IsNullOrEmpty(DeptID) And DeptID <> "0" Then
                parms(5).Value = Convert.ToInt32(DeptID)
            Else
                parms(5).Value = DBNull.Value
            End If
            If Not String.IsNullOrEmpty(classid) And classid <> "0" Then
                parms(6).Value = Convert.ToInt32(classid)
            Else
                parms(6).Value = DBNull.Value
            End If
            If Not String.IsNullOrEmpty(acode) Then
                parms(7).Value = acode
            Else
                parms(7).Value = DBNull.Value
            End If
            If Not String.IsNullOrEmpty(vendorid) And vendorid <> "0" Then
                parms(8).Value = Convert.ToInt32(vendorid)
            Else
                parms(8).Value = DBNull.Value
            End If
            If Not String.IsNullOrEmpty(VndrStylID) Then
                parms(9).Value = VndrStylID
            Else
                parms(9).Value = DBNull.Value
            End If
            If Not String.IsNullOrEmpty(adnumber) Then
                parms(10).Value = Convert.ToInt32(adnumber)
            Else
                parms(10).Value = DBNull.Value
            End If
            If Not String.IsNullOrEmpty(tistatusid) Then
                parms(11).Value = tistatusid
            Else
                parms(11).Value = DBNull.Value
            End If
            If Not String.IsNullOrEmpty(tintyp) Then
                parms(12).Value = Convert.ToInt32(tintyp)
            Else
                parms(12).Value = DBNull.Value
            End If
            If Not String.IsNullOrEmpty(InWebCat) Then
                parms(13).Value = InWebCat
            Else
                parms(13).Value = DBNull.Value
            End If
            If Not String.IsNullOrEmpty(pagenumber) Then
                parms(14).Value = Convert.ToInt32(pagenumber)
            Else
                parms(14).Value = DBNull.Value
            End If
            If Not String.IsNullOrEmpty(ImageType) Then
                parms(15).Value = ImageType
            Else
                parms(15).Value = DBNull.Value
            End If
            If Not String.IsNullOrEmpty(ModelCategoryCode) Then
                parms(16).Value = ModelCategoryCode
            Else
                parms(16).Value = DBNull.Value
            End If
            If Not String.IsNullOrEmpty(FeatureWebCat) Then
                parms(17).Value = FeatureWebCat
            Else
                parms(17).Value = DBNull.Value
            End If
            If Not String.IsNullOrEmpty(BatchNum) Then
                parms(18).Value = Convert.ToInt32(BatchNum)
            Else
                parms(18).Value = DBNull.Value
            End If

            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    _ECommTurnInQueryToolInfo.Add(ECommTurnInQueryToolFactory.Construct(rdr))
                End While
            End Using
        Catch ex As Exception

        End Try
        Return _ECommTurnInQueryToolInfo
    End Function

    Public Sub UpdateStatus(ByVal TurnInMerchIDs As String, ByVal strStatusCde As String, ByVal UserId As String)
        Dim sql As String = _spSchema + ".TU1082SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@MERCH_IDS", DB2Type.VarChar), _
                                                          New DB2Parameter("@STATUS", DB2Type.VarChar), _
                                                          New DB2Parameter("@USERID", DB2Type.VarChar), _
                                                          New DB2Parameter("@SCHEMA", DB2Type.VarChar)}

        parms(0).Value = TurnInMerchIDs
        parms(1).Value = strStatusCde
        parms(2).Value = UserId
        parms(3).Value = _dbSchema

        Try
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function GetUPCReport(ByVal strTUWeek As String) As IList(Of AdPageInfo)
        Dim AdPageInfoReport As New List(Of AdPageInfo)

        Dim sql As String = _spSchema + ".TU1107SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@TU_WEEK_AD_NBR", DB2Type.Integer)}
        parms(0).Value = IIf(strTUWeek = "", DBNull.Value, strTUWeek)

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                While (rdr.Read())
                    Dim UPC As New AdPageInfo
                    UPC.adnbr = CInt(rdr("AD_NUM"))
                    UPC.PgNbr = CInt(rdr("AD_SYSTEM_PAGE_NUM"))
                    UPC.VendorISNNumber = CStr(rdr("VENDOR_STYLE_NUM"))
                    UPC.UPCNumber = CStr(rdr("UPC_NUM"))
                    AdPageInfoReport.Add(UPC)
                End While
            End Using
            Return AdPageInfoReport
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function
End Class
