Imports System.Text.RegularExpressions
Imports System.Xml.Linq
Imports System.Xml.Serialization
Imports Telerik.Web.UI
Imports TurnInProcessAutomation.BLL.MiscConstants
Imports TurnInProcessAutomation.BLL.Enumerations

Public MustInherit Class PageBase
    Inherits System.Web.UI.Page

#Region "Members"
    Private Const PreviousUrlHistoryMaxDepth As Integer = 1 'Back button should navigate back and forth between two pages, as per the standard.
    Private Const SessionKeyPreviousUrlHistory As String = "RTSMAPS_PreviousUrlHistory"

    Public _log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private _previousUrlHistory As Stack(Of String)
#End Region

#Region "Properties"
    ''' <summary>
    ''' Gets the Action value from the query string. 
    ''' </summary>    
    ''' <remarks>Action defaults to Inquiry when query string not found.</remarks>
    Protected Friend ReadOnly Property ActionQueryStringValue() As String
        Get
            If Request.QueryString("ACTION") IsNot Nothing Then
                Return Request.QueryString("ACTION").ToString.ToUpper
            Else
                Return INQUIRE_MODE
            End If
        End Get
    End Property

    ''' <summary>
    ''' Determines if the page is in add mode by checking the session action value.
    ''' </summary>    
    Protected Friend ReadOnly Property IsInAddMode() As Boolean
        Get
            Return SessionWrapper.Action = Modes.ADD.ToString()
        End Get
    End Property

    ''' <summary>
    ''' Determines if the page is in add or maintenance mode by checking the session action value.
    ''' </summary>    
    Protected Friend ReadOnly Property IsInEditMode() As Boolean
        Get
            Return IsInAddMode OrElse IsInMaintenanceMode
        End Get
    End Property

    ''' <summary>
    ''' Determines if the page is in maintenance mode by checking the session action value.
    ''' </summary>    
    Protected Friend ReadOnly Property IsInMaintenanceMode() As Boolean
        Get
            Return SessionWrapper.Action = Modes.MAINTENANCE.ToString()
        End Get
    End Property

    ''' <summary>
    ''' Determines if the user navigated to the current page via the Back button.
    ''' </summary>    
    Protected Friend ReadOnly Property IsNavigatedBackTo() As Boolean
        Get
            Return Request.QueryString("BACK") IsNot Nothing
        End Get
    End Property

    Protected Friend Property PreviousPageUrl() As String
        Get
            If PreviousUrlHistory Is Nothing _
            OrElse PreviousUrlHistory.Count = 0 _
            Then Return Nothing

            'This no longer needs tracked, since the max history depth is only 1.
            'The original sample can be found in the NavigationHistory class, in the Samples directory.
            '
            'Ryan Bergeman, 2010-12-08
            '
            'PreviousUrlHistoryLastPoppedValue = poppedValue ' poppedValue is equal to PreviousUrlHistory.Pop()

            Dim builder As New StringBuilder(PreviousUrlHistory.Pop())
            Dim uri As New Uri(builder.ToString())

            'Checks for the existence of the back query string indicator, and appends it if it does not exist.
            Const backIndicator As String = "BACK=BACK"
            If Not Regex.IsMatch(uri.Query, backIndicator, RegexOptions.IgnoreCase) Then
                builder.Append(If(String.IsNullOrEmpty(uri.Query), "?", "&"))
                builder.Append(backIndicator)
            End If

            Return builder.ToString()
        End Get
        Private Set(ByVal value As String)
            If PreviousUrlHistory.Count >= PreviousUrlHistoryMaxDepth Then
                'What really should be done here is removing the value on the bottom of the stack,
                'but since the max depth is only 1 (as of 2010-12-08), popping the one element off
                'of the top of the stack achieves the same result.
                'Ryan Bergeman, 2010-12-08
                PreviousUrlHistory.Pop()
            End If

            'Only push a value onto the stack if it is new (in comparison to what is currently on top). (Or if the stack is empty, of course.)
            If PreviousUrlHistory.Count = 0 OrElse value <> PreviousUrlHistory.Peek() Then
                PreviousUrlHistory.Push(value)
            End If
        End Set
    End Property

    Protected ReadOnly Property PeekPreviousPageUrl() As String
        Get
            Return If(PreviousUrlHistory IsNot Nothing AndAlso PreviousUrlHistory.Count > 0, PreviousUrlHistory.Peek(), String.Empty)
        End Get
    End Property

    Private ReadOnly Property PreviousUrlHistory() As Stack(Of String)
        Get
            If Session(SessionKeyPreviousUrlHistory) Is Nothing _
            Then Session(SessionKeyPreviousUrlHistory) = New Stack(Of String)

            _previousUrlHistory = DirectCast(Session(SessionKeyPreviousUrlHistory), Stack(Of String))
            Return _previousUrlHistory
        End Get
    End Property
#End Region

#Region "Events"
    ''' <summary>Checks Request.UrlReferrer and (if it exists) stores it in the PreviousPageUrl history.</summary>
    Protected Friend Sub PageBase_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If IsPostBack OrElse Request.UrlReferrer Is Nothing Then Return

        'If the last navigated history URL is this page's URL, don't add it to the history (it would loop indefinitely).
        '
        'This check has been excluded, because the desired functionality is to be able to
        'indefinitely move back and forth between two pages, which this check helped prevent.
        'It has been kept here in case the desired functionality changes.
        '
        'Note: the PreviousUrlHistoryLastPoppedValue referred to in this check has been removed from this class,
        'but is located in the NavigationHistory sample in the Samples directory.
        '
        'Ryan Bergeman, 2010-12-08
        '
        'If PreviousUrlHistoryLastPoppedValue = Request.Url.AbsoluteUri Then Return

        'If the referrer of this page is this page itself (ie. the user navigates to the same page multiple times in a row through the menu), don't add it to the history (this eliminates redundant history).
        If Request.Url.AbsoluteUri = Request.UrlReferrer.AbsoluteUri Then Return

        PreviousPageUrl = Request.UrlReferrer.AbsoluteUri
    End Sub

    Protected Friend Sub RadToolBarBackButton_Load(ByVal sender As Object, ByVal e As EventArgs)
        DirectCast(sender, RadToolBarButton).Visible = PreviousUrlHistory IsNot Nothing AndAlso PreviousUrlHistory.Count > 0
    End Sub
#End Region

#Region "Methods"
    ''' <summary>
    ''' Moves page to the previous screen
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overridable Sub BackPage()
        If Not ViewState(PREV_PAGE_URL) Is Nothing Then
            Dim questionMarkPosition As Integer = 0
            Dim backPosition As Integer = 0
            Dim prevPageURL As String = ViewState(PREV_PAGE_URL).ToString
            questionMarkPosition = prevPageURL.IndexOf("?")
            backPosition = prevPageURL.IndexOf("BACK=")
            If (questionMarkPosition > 0 AndAlso backPosition > 0) Then
                Response.Redirect(prevPageURL, False)
            ElseIf (questionMarkPosition > 0 And backPosition = -1) Then
                Response.Redirect(prevPageURL & "&BACK=BACK", False)
            ElseIf questionMarkPosition = -1 Then
                Response.Redirect(prevPageURL & "?BACK=BACK", False)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Returns a collection of ListItems which contains all error messages of invalid ToolTipValidators for the specified validation group.
    ''' </summary>    
    Protected Function GetErrorDetails() As List(Of ListItem)
        Return GetErrorDetails(String.Empty)
    End Function

    ''' <summary>
    ''' Returns a collection of ListItems which contains all error messages of invalid ToolTipValidators for the specified validation group.
    ''' </summary>    
    Protected Function GetErrorDetails(ByVal validationGroup As String) As List(Of ListItem)
        Dim items As New List(Of ListItem)

        For Each errorMessage As String In GetToolTipValidators(validationGroup).Where(Function(x) Not x.IsValid).Select(Function(x) x.ErrorMessage)
            items.Add(New ListItem(errorMessage))
        Next

        Return items
    End Function

    ''' <summary>
    ''' Returns a collection of ToolTipValidator controls for the specified validation group.
    ''' </summary>    
    Protected Function GetToolTipValidators() As List(Of ToolTipValidator)
        Return GetToolTipValidators(String.Empty)
    End Function

    ''' <summary>
    ''' Returns a collection of ToolTipValidator controls for the specified validation group.
    ''' </summary>    
    Protected Function GetToolTipValidators(ByVal validationGroup As String) As List(Of ToolTipValidator)
        Dim toolTipValidators As New List(Of ToolTipValidator)

        For Each validator As BaseValidator In Page.GetValidators(validationGroup)
            If TypeOf validator.Parent Is ToolTipValidator Then
                toolTipValidators.Add(DirectCast(validator.Parent, ToolTipValidator))
            End If
        Next

        Return toolTipValidators
    End Function

    ''' <summary>
    ''' Returns the message for the specified code.
    ''' </summary>    
    Public Shared Function GetMessage(ByVal code As MessageCode) As Message
        Return SessionWrapper.Messages.Where(Function(x) x.Code = CInt(code).ToString().PadLeft(3, "0"c)).SingleOrDefault()
    End Function

    ''' <summary>
    ''' Returns the formatted description for the specified code.
    ''' </summary>    
    Public Shared Function GetValidationMessage(ByVal code As MessageCode, Optional ByVal includeErrorCode As Boolean = True) As String
        Dim message As Message = GetMessage(code)
        If message IsNot Nothing Then
            Return message.CodeWithDescription(includeErrorCode)
        Else
            Return String.Empty
        End If
    End Function

    ''' <summary>
    ''' Searches the naming container for a server control with the specified identifier.
    ''' </summary>    
    Public Shared Function FindControlRecursive(ByVal rootControl As Control, ByVal controlId As String) As Control
        If rootControl.ID = controlId Then
            Return rootControl
        End If

        For Each controlToSearch As Control In rootControl.Controls
            Dim controlToReturn As Control = FindControlRecursive(controlToSearch, controlId)
            If controlToReturn IsNot Nothing Then
                Return controlToReturn
            End If
        Next
        Return Nothing
    End Function
#End Region
End Class