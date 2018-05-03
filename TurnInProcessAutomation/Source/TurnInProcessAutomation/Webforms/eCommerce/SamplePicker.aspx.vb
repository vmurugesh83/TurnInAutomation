Imports TurnInProcessAutomation.BusinessEntities
Imports System.Xml.Linq
Imports Telerik.Web.UI
Imports TurnInProcessAutomation.BLL

Public Class SamplePicker
    Inherits System.Web.UI.Page

    Dim _TUEcommSetupCreate As New TUEcommSetupCreate

    Public Property AvailableSamples() As IList(Of SampleRequestInfo)
        Get
            If Session("SamplePicker.AvailableSamples") Is Nothing Then
                Session("SamplePicker.AvailableSamples") = New List(Of SampleRequestInfo)
            End If
            Return CType(Session("SamplePicker.AvailableSamples"), IList(Of SampleRequestInfo))
        End Get
        Set(value As IList(Of SampleRequestInfo))
            Session("SamplePicker.AvailableSamples") = value
        End Set
    End Property

    Public Property ColorCode() As Integer
        Get
            If Session("SamplePicker.ColorCode") Is Nothing Then
                Session("SamplePicker.ColorCode") = 0
            End If
            Return CInt(Session("SamplePicker.ColorCode"))
        End Get
        Set(value As Integer)
            Session("SamplePicker.ColorCode") = value
        End Set
    End Property

    Public Property MerchID() As Integer
        Get
            If Session("SamplePicker.MerchID") Is Nothing Then
                Session("SamplePicker.MerchID(") = 0
            End If
            Return CInt(Session("SamplePicker.MerchID"))
        End Get
        Set(value As Integer)
            Session("SamplePicker.MerchID") = value
        End Set
    End Property
    Public Property PrimaryThumbURL() As String
        Get
            If Session("SamplePicker.PrimaryThumbURL") Is Nothing Then
                Session("SamplePicker.PrimaryThumbURL(") = ""
            End If
            Return CStr(Session("SamplePicker.PrimaryThumbURL"))
        End Get
        Set(value As String)
            Session("SamplePicker.PrimaryThumbURL") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim colKeys As NameValueCollection = Request.QueryString
            Dim ctlparnt As Control = Me.Page.Parent
            Dim internalStyleNumber As Decimal
            Dim sampleMerchId As Integer
            If Not Request.QueryString("ISN").ToString() Is Nothing And Not Request.QueryString("MerchId") Is Nothing Then
                If Not (Request.QueryString("ColorId").ToString() = "undefined") Then
                    ColorCode = CInt(Request.QueryString("ColorId"))
                    'And Not Request.QueryString("ColorId") Is Nothing 
                Else
                    ColorCode = 0
                End If
                sampleMerchId = CInt(Request.QueryString("MerchId"))
                Me.MerchID = sampleMerchId

                If sampleMerchId > 0 Then
                    rtbSampleMerchId.Text = sampleMerchId.ToString()
                Else
                    internalStyleNumber = CDec(Request.QueryString("ISN"))
                    rtbInternalStyle.Text = CStr(IIf(internalStyleNumber > 0, CStr(internalStyleNumber), String.Empty))
                    rtbSampleMerchId.Text = String.Empty
                End If

                Dim matchedSampleRequests As IList(Of SampleRequestInfo) = _TUEcommSetupCreate.GetAvailableSampleRequests(sampleMerchId, internalStyleNumber, String.Empty)
                BindAvailableSamplesGrid(matchedSampleRequests)
            End If
        End If

    End Sub

    Public Sub OnRetrieveClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbRetrieve.Click

        Dim sampleMerchId As Integer = 0
        If Not String.IsNullOrWhiteSpace(rtbSampleMerchId.Text) Then
            sampleMerchId = CInt(rtbSampleMerchId.Text)
        End If

        Dim internalStyleNumber As Decimal = 0D
        If Not String.IsNullOrWhiteSpace(rtbInternalStyle.Text) Then
            internalStyleNumber = CDec(rtbInternalStyle.Text)
        End If

        Dim vendorStyle As String = rtbVendorStyle.Text

        'KL TEMP comment out conditional
        If rtbInternalStyle.Text = Request.QueryString("ISN") Then
            If Not Request.QueryString("ColorId") Is Nothing Then
                ColorCode = CInt(Request.QueryString("ColorId"))
            Else
                ColorCode = 0
            End If
        End If
            Dim matchedSampleRequests As IList(Of SampleRequestInfo) = _TUEcommSetupCreate.GetAvailableSampleRequests(sampleMerchId, internalStyleNumber, vendorStyle)
            BindAvailableSamplesGrid(matchedSampleRequests)

    End Sub

    Private Sub BindAvailableSamplesGrid(ByVal matchedSampleRequests As IList(Of SampleRequestInfo))

        Try
            Dim orderedSamples As System.Linq.IOrderedEnumerable(Of SampleRequestInfo) = matchedSampleRequests.Where(Function(x) x.ColorCode = ColorCode).OrderBy(Function(x) x.SampleSizeDesc)
            ' AvailableSamples = orderedSamples.Concat(matchedSampleRequests.Where(Function(x) x.ColorCode <> ColorCode).OrderBy(Function(x) x.ColorCode)).ToList()

            grdAvailableSamples.DataSource = orderedSamples.Concat(matchedSampleRequests.Where(Function(x) x.ColorCode <> ColorCode).OrderBy(Function(x) x.ColorCode)).ToList()
            grdAvailableSamples.DataBind()
        Catch ex As Exception
            Session("ErrorMsg") = "Error in Method: " & System.Reflection.MethodBase.GetCurrentMethod.Name & "<br/>Error Message: " & ex.Message
            Response.Redirect("~/Error.aspx", False)
        End Try

    End Sub

    ''' <summary>
    ''' Sets the selected item to match the Admin Merch Number from the query string.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grdAvailableSamples_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles grdAvailableSamples.ItemDataBound

        If (TypeOf e.Item Is GridDataItem) Then

            Dim dataItem As GridDataItem = DirectCast(e.Item, GridDataItem)
            Dim qsMerchId As Integer = CInt(Request.QueryString("MerchId"))
            Dim diMerchId As Integer = CInt(dataItem.GetDataKeyValue("SampleMerchId"))
            If qsMerchId = diMerchId Then
                dataItem.Selected = True
            Else
                dataItem.Selected = False
            End If

            ' Approval Timestamp validation
            Dim approvalTimestamp As DateTime
            DateTime.TryParse(dataItem("SampleApprovalTimestamp").Text, approvalTimestamp)
            dataItem("SampleApprovalTimestamp").Text = CStr(IIf(approvalTimestamp > DateTime.MinValue, approvalTimestamp.ToString("MM/dd/yyyy"), String.Empty))

            ' Approval Timestamp validation
            Dim cmrCheckInDate As DateTime = DateTime.MinValue
            DateTime.TryParse(dataItem("CMRCheckInDate").Text, cmrCheckInDate)
            dataItem("CMRCheckInDate").Text = CStr(IIf(cmrCheckInDate > DateTime.MinValue, cmrCheckInDate.ToString("MM/dd/yyyy"), String.Empty))
        End If

    End Sub
End Class