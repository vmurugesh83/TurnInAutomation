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

Partial Public Class AdminDataDao
    Public Function AddAdminData(ByVal merchandiseInfo As ECommTurnInMeetCreateInfo, ByVal upc As Decimal, ByVal vendorColorCode As Integer, ByVal sizeDesc As String, ByVal status As String) As Integer
        Dim parms As SqlParameter() = New SqlParameter() {New SqlParameter("@in_admin_merch_num", SqlDbType.Int, 0), _
                                                          New SqlParameter("@sku_upc", SqlDbType.Decimal), _
                                                          New SqlParameter("@merch_desc", SqlDbType.Char, 60), _
                                                          New SqlParameter("@vend_style_nbr", SqlDbType.Char, 20), _
                                                          New SqlParameter("@size", SqlDbType.Char, 10), _
                                                          New SqlParameter("@color", SqlDbType.Char, 15), _
                                                          New SqlParameter("@vendor_color_code", SqlDbType.SmallInt), _
                                                          New SqlParameter("@dept_nbr", SqlDbType.SmallInt), _
                                                          New SqlParameter("@isn", SqlDbType.Decimal), _
                                                          New SqlParameter("@ad_nbr", SqlDbType.Int), _
                                                          New SqlParameter("@page_nbr", SqlDbType.SmallInt), _
                                                          New SqlParameter("@status", SqlDbType.VarChar, 2)}
        Dim strFriendlyProdDesc As String = String.Empty

        ' Set up the parameters 
        parms(0).Value = merchandiseInfo.MerchID
        parms(1).Value = upc

        'Prefix Label Name to the Friendly Product Description.
        If String.IsNullOrEmpty(merchandiseInfo.Label) Then
            strFriendlyProdDesc = merchandiseInfo.FriendlyProdDesc
        ElseIf String.IsNullOrEmpty(merchandiseInfo.FriendlyProdDesc) Then
            strFriendlyProdDesc = merchandiseInfo.Label
        Else
            strFriendlyProdDesc = merchandiseInfo.Label & " - " & merchandiseInfo.FriendlyProdDesc
        End If

        If strFriendlyProdDesc.Length > 60 Then
            parms(2).Value = strFriendlyProdDesc.Substring(0, 60).Trim()
        Else
            parms(2).Value = strFriendlyProdDesc.Trim()
        End If

        parms(3).Value = merchandiseInfo.VendorStyleNumber
        parms(4).Value = sizeDesc

        If merchandiseInfo.FriendlyColor.Length > 15 Then
            parms(5).Value = merchandiseInfo.FriendlyColor.Substring(0, 15).Trim()
        Else
            parms(5).Value = merchandiseInfo.FriendlyColor.Trim()
        End If

        parms(6).Value = vendorColorCode
        parms(7).Value = CShort(merchandiseInfo.DeptID)
        parms(8).Value = merchandiseInfo.ISN
        parms(9).Value = merchandiseInfo.AdNumber
        parms(10).Value = CShort(merchandiseInfo.PageNumber)
        parms(11).Value = status

        Try
            Dim MerchAdminNum As Integer = CInt(ExecuteScalar(ConnectionStringLocalTransaction, CommandType.StoredProcedure, "tu_admin_data_insert", parms))
            Return MerchAdminNum
        Catch ex As Exception
            Throw
        End Try
    End Function

    <Obsolete("Replaced with ECommTurnInMeetCreateInfo for calls from the Turn In Meeting application")> _
    Public Function AddAdminData(ByVal ColorSizeData As EcommSetupClrSzInfo, ByVal strAdNbr As String, ByVal strPageNbr As String) As Integer
        Dim parms As SqlParameter() = New SqlParameter() {New SqlParameter("@in_admin_merch_num", SqlDbType.Int, 0), _
                                                          New SqlParameter("@sku_upc", SqlDbType.Decimal), _
                                                          New SqlParameter("@merch_desc", SqlDbType.Char, 60), _
                                                          New SqlParameter("@vend_style_nbr", SqlDbType.Char, 20), _
                                                          New SqlParameter("@size", SqlDbType.Char, 10), _
                                                          New SqlParameter("@color", SqlDbType.Char, 15), _
                                                          New SqlParameter("@vendor_color_code", SqlDbType.SmallInt), _
                                                          New SqlParameter("@dept_nbr", SqlDbType.SmallInt), _
                                                          New SqlParameter("@isn", SqlDbType.Decimal), _
                                                          New SqlParameter("@ad_nbr", SqlDbType.Int), _
                                                          New SqlParameter("@page_nbr", SqlDbType.SmallInt)}
        Dim strFriendlyProdDesc As String = String.Empty

        ' Set up the parameters 
        parms(0).Value = ColorSizeData.AdminMerchNum
        parms(1).Value = ColorSizeData.UPC

        'Prefix Label Name to the Friendly Product Description.
        If String.IsNullOrEmpty(ColorSizeData.LabelName) Then
            strFriendlyProdDesc = ColorSizeData.FriendlyProdDesc
        ElseIf String.IsNullOrEmpty(ColorSizeData.FriendlyProdDesc) Then
            strFriendlyProdDesc = ColorSizeData.LabelName
        Else
            strFriendlyProdDesc = ColorSizeData.LabelName & " - " & ColorSizeData.FriendlyProdDesc
        End If

        If strFriendlyProdDesc.Length > 60 Then
            parms(2).Value = strFriendlyProdDesc.Substring(0, 60).Trim
        Else
            parms(2).Value = strFriendlyProdDesc.Trim
        End If

        parms(3).Value = ColorSizeData.VendorStyleNum.Trim

        parms(4).Value = ColorSizeData.SampleSize

        If ColorSizeData.FriendlyColor.Length > 15 Then
            parms(5).Value = ColorSizeData.FriendlyColor.Substring(0, 15).Trim
        Else
            parms(5).Value = ColorSizeData.FriendlyColor.Trim
        End If

        parms(6).Value = ColorSizeData.VendorColorCode
        parms(7).Value = CShort(ColorSizeData.DeptID)
        parms(8).Value = ColorSizeData.ISN
        parms(9).Value = CInt(strAdNbr)

        If strPageNbr.Contains("-"c) Then
            parms(10).Value = CShort(strPageNbr.Substring(0, strPageNbr.IndexOf("-"c)).Trim)
        Else
            parms(10).Value = CShort(strPageNbr.Trim)
        End If

        Try
            Dim MerchAdminNum As Integer = CInt(ExecuteScalar(ConnectionStringLocalTransaction, CommandType.StoredProcedure, "tu_admin_data_insert", parms))
            Return MerchAdminNum
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GenerateAdminData() As Integer
        Try
            Dim SQLResults As Integer = CInt(ExecuteScalar(ConnectionStringLocalTransaction, CommandType.Text, "Select next_merch_id From Informix.next_merch", Nothing))
            Return SQLResults
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function CreateImageAndAssignToAd(ByVal BuyerName As String, ByVal BuyerExt As String, ByVal FriendlyProdDesc As String, ByVal ImageClass As String, ByVal AdNumber As Integer, ByVal PageNumber As Short, ByVal ImageSuffix As String, ByVal ImageNotes As String) As Integer
        Try
            Dim parms As SqlParameter() = New SqlParameter() {New SqlParameter("@buyer_name", SqlDbType.VarChar), _
                                                              New SqlParameter("@buyer_ext", SqlDbType.VarChar), _
                                                              New SqlParameter("@frndly_Prod_Desc", SqlDbType.VarChar), _
                                                              New SqlParameter("@img_Class", SqlDbType.VarChar), _
                                                              New SqlParameter("@adnum", SqlDbType.Int), _
                                                              New SqlParameter("@pgnum", SqlDbType.SmallInt), _
                                                              New SqlParameter("@ImgSuffx", SqlDbType.VarChar), _
                                                              New SqlParameter("@ImgNotes", SqlDbType.VarChar), _
                                                              New SqlParameter("@NewImageId", SqlDbType.Int)}

            ' Set up the parameters 
            parms(0).Value = BuyerName
            parms(1).Value = BuyerExt
            parms(2).Value = FriendlyProdDesc
            parms(3).Value = ImageClass
            parms(4).Value = AdNumber
            parms(5).Value = PageNumber
            parms(6).Value = ImageSuffix
            parms(7).Value = ImageNotes
            parms(8).Direction = ParameterDirection.Output

            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, "[tu_create_image_assign_to_ad]", parms)

            Return CInt(parms(8).Value)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function CreateNewImageMerch(ByVal NewImageId As Integer, ByVal adminMdseNum As Integer, ByVal mdseGrpNum As Short, ByVal stylingNotes As String) As Boolean
        Try
            Dim parms As SqlParameter() = New SqlParameter() {New SqlParameter("@NewImageId", SqlDbType.Int), _
                                                              New SqlParameter("@adminMdseNum", SqlDbType.Int), _
                                                              New SqlParameter("@mdseGrpNum", SqlDbType.SmallInt), _
                                                              New SqlParameter("@stylingNotes", SqlDbType.VarChar)}

            ' Set up the parameters 
            parms(0).Value = NewImageId
            parms(1).Value = adminMdseNum
            parms(2).Value = mdseGrpNum
            parms(3).Value = stylingNotes

            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, "[tu_insert_image_merch_new]", parms)

            Return True
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Sub DeleteAdminData(ByVal intAdminMerchNum As Integer)
        Try
            Dim parms As SqlParameter() = New SqlParameter() {New SqlParameter("@admin_merch_num", SqlDbType.Int)}

            ' Set up the parameters
            parms(0).Value = intAdminMerchNum

            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, "[tu_admin_data_delete]", parms)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub UpdateAdminData(ByVal intAdminMerchID As Integer, ByVal intAdNbr As Integer, ByVal shPageNbr As Short)
        Try
            Dim parms As SqlParameter() = New SqlParameter() {New SqlParameter("@admin_merch_num", SqlDbType.Int), _
                                                          New SqlParameter("@ad_nbr", SqlDbType.Int), _
                                                          New SqlParameter("@page_nbr", SqlDbType.SmallInt)}

            ' Set up the parameters 
            parms(0).Value = intAdminMerchID
            parms(1).Value = intAdNbr
            parms(2).Value = shPageNbr

            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, "[tu_admin_data_update]", parms)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub UpdateAdminStatus(ByVal intAdminMerchID As Integer, ByVal intAdNbr As Integer, ByVal PageNumber As Integer, ByVal removeMerchFlag As String)
        Dim parms As SqlParameter() = New SqlParameter() {New SqlParameter("@admin_merch_num", SqlDbType.Int), _
                                                        New SqlParameter("@ad_nbr", SqlDbType.Int), _
                                                        New SqlParameter("@page_nbr", SqlDbType.Int), _
                                                        New SqlParameter("@status", SqlDbType.VarChar)}

        ' Set up the parameters 
        parms(0).Value = intAdminMerchID
        parms(1).Value = intAdNbr
        parms(2).Value = PageNumber
        parms(3).Value = If(removeMerchFlag = "N"c, "A", "K")

        ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, "[tu_admin_data_updatestatus]", parms)
    End Sub

    Public Sub UpdateBatchNumber(ByVal newAdNum As Integer, ByVal merchIds As List(Of String))
        Try
            Dim parms As SqlParameter() = New SqlParameter() {New SqlParameter("@newAdNum", SqlDbType.Int), _
                                                            New SqlParameter("@merchIds", SqlDbType.VarChar)}

            ' Set up the parameters 
            parms(0).Value = newAdNum
            parms(1).Value = String.Join(",", merchIds)

            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, "[tu_update_batch]", parms)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Function GetImageMerchAssociationByMerchID(ByVal adminMerchandiseID As Integer) As Integer
        Dim parms As SqlParameter() = Nothing
        Dim dataReader As SqlDataReader = Nothing
        Dim imageGroupNumber As Integer = 0

        Try
            parms = New SqlParameter() {New SqlParameter("@merchID", SqlDbType.Int)}

            ' Set up the parameters 
            parms(0).Value = adminMerchandiseID

            dataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, "tu_retrieve_image_merch_new", parms)

            If dataReader.HasRows Then
                dataReader.Read()

                imageGroupNumber = CInt(dataReader("merch_grp"))
            End If

        Catch ex As Exception
            Throw
        End Try

        Return imageGroupNumber
    End Function
    Public Sub UpdateImageNotesByImageID(ByVal adNumber As Integer, ByVal pageNumber As Integer, ByVal imageID As Integer, ByVal imageNotes As String)
        Try
            Dim parms As SqlParameter() = New SqlParameter() {New SqlParameter("@imageID", SqlDbType.Int), _
                                                            New SqlParameter("@adNumber", SqlDbType.Int), _
                                                            New SqlParameter("@pageNumber", SqlDbType.SmallInt), _
                                                            New SqlParameter("@imageNotes", SqlDbType.VarChar)}

            ' Set up the parameters 
            parms(0).Value = imageID
            parms(1).Value = adNumber
            parms(2).Value = pageNumber
            parms(3).Value = imageNotes

            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, "[tu_update_image_notes_by_image_id]", parms)
        Catch ex As Exception
            Throw
        End Try
    End Sub

End Class
