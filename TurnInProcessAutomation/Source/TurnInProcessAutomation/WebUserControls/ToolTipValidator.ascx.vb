Imports System.ComponentModel
Imports Telerik.Web.UI

Partial Public Class ToolTipValidator
    Inherits System.Web.UI.UserControl

#Region "Members"
    Public Shared ReadOnly ErrorColor As Drawing.Color = Drawing.ColorTranslator.FromHtml("#FFC0CB")
    Private _controlToEvaluate As String = String.Empty
    Private _errorMessage As String = String.Empty
    Private _messagePanelID As String = String.Empty
    Private _isValid As Boolean = True
#End Region

#Region "Properties"
    ''' <summary>
    ''' Gets or sets the input control to validate.
    ''' </summary>    
    Public Property ControlToEvaluate() As String
        Get
            Return _controlToEvaluate
        End Get
        Set(ByVal value As String)
            _controlToEvaluate = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets value indicating if the validation succeeded.
    ''' </summary>
    ''' <remarks>The ToolTipValidator is valid when the custom validator and manual indicator are true.</remarks>    
    <Browsable(False), _
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Property IsValid() As Boolean
        Get
            Return (cvToolTipValidator.IsValid AndAlso _isValid)
        End Get
        Set(ByVal value As Boolean)
            _isValid = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the error message to display in the ToolTip.
    ''' </summary>    
    Public Property ErrorMessage() As String
        Get
            Return _errorMessage
        End Get
        Set(ByVal value As String)
            _errorMessage = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets a value indicating whether empty input text should be validated.
    ''' </summary>    
    Public Property ValidateEmptyText() As Boolean
        Get
            Return cvToolTipValidator.ValidateEmptyText
        End Get
        Set(ByVal value As Boolean)
            cvToolTipValidator.ValidateEmptyText = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the name of the validation group for this control.
    ''' </summary>    
    Public Property ValidationGroup() As String
        Get
            Return cvToolTipValidator.ValidationGroup
        End Get
        Set(ByVal value As String)
            cvToolTipValidator.ValidationGroup = value
        End Set
    End Property

    ''' <summary>
    ''' Gets a reference to the input control that is validated.
    ''' </summary>    
    Public ReadOnly Property EvaluatedControl() As WebControl
        Get
            Return If(Not String.IsNullOrEmpty(ControlToEvaluate), TryCast(Me.Parent.FindControl(ControlToEvaluate.Trim), WebControl), Nothing)
        End Get
    End Property
#End Region

#Region "Events"
    Public Event ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)

    Private Sub cvToolTipValidator_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvToolTipValidator.ServerValidate
        'Override validator IsValid setting with the manual indicator incase it is already invalid.
        'Raise the ServerValidate event when the validator is valid.
        args.IsValid = _isValid
        If args.IsValid Then
            RaiseEvent ServerValidate(Me, args) 'Supply the user control as source.
        End If
    End Sub

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not String.IsNullOrEmpty(ControlToEvaluate) AndAlso TryCast(Me.Parent.FindControl(ControlToEvaluate.Trim), WebControl) Is Nothing Then
            Throw New ApplicationException(String.Format("Unable to find control with id of '{0}' in the same naming container.", ControlToEvaluate))
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        'Display error message in tool tip.
        rttError.Text = If(Not cvToolTipValidator.IsValid, ErrorMessage, String.Empty)

        'Highlight control back color when not valid.
        If EvaluatedControl IsNot Nothing Then
            HighlightBackColor(EvaluatedControl, If(cvToolTipValidator.IsValid, Nothing, ErrorColor))
        End If
    End Sub
#End Region

#Region "Methods"
    Public Sub Validate()
        cvToolTipValidator.Validate()
    End Sub

    ''' <summary>
    ''' Highlights the specified control with the color provided.
    ''' </summary>    
    Public Shared Sub HighlightBackColor(ByVal control As WebControl, ByVal color As Drawing.Color)
        If TypeOf control Is RadDateTimePicker Then
            DirectCast(control, RadDateTimePicker).DateInput.BackColor = color
        ElseIf TypeOf control Is RadDatePicker Then
            DirectCast(control, RadDatePicker).DateInput.BackColor = color
        ElseIf TypeOf control Is RadTimePicker Then
            DirectCast(control, RadTimePicker).DateInput.BackColor = color
        Else
            control.BackColor = color
        End If
    End Sub
#End Region
End Class