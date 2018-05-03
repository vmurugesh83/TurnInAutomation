Namespace BonTon.Web.Controls
    ''' <summary> 
    ''' This control inherits the ASP.NET ValidationSummary control but adds 
    ''' the ability to dynamically add error messages without requiring 
    ''' validation controls. 
    ''' </summary> 
    Public Class ValidationSummary
        Inherits System.Web.UI.WebControls.ValidationSummary
        ''' <summary>
        ''' Allows the caller to place custom text messages inside the validation 
        ''' summary control 
        ''' </summary> 
        ''' <param name="msg">The message you want to appear in the summary</param>
        ''' <remarks></remarks>
        Public Sub AddValidationMessage(ByVal msg As String)
            Me.Page.Validators.Add(New DummyValidator(msg))
        End Sub
    End Class

    ''' <summary> 
    ''' The validation summary control works by iterating over the Page.Validators 
    ''' collection and displaying the ErrorMessage property of each validator 
    ''' that return false for the IsValid() property. This class will act 
    ''' like all the other validators except it always is invalid and thus the 
    ''' ErrorMessage property will always be displayed. 
    ''' </summary> 
    Friend Class DummyValidator
        Implements IValidator

        Private errorMsg As String

        Public Sub New(ByVal msg As String)
            errorMsg = msg
        End Sub

        Public Property ErrorMessage() As String Implements IValidator.ErrorMessage
            Get
                Return errorMsg
            End Get
            Set(ByVal value As String)
                errorMsg = value
            End Set
        End Property

        Public Property IsValid() As Boolean Implements IValidator.IsValid
            Get
                Return False
            End Get
            Set(ByVal value As Boolean)
            End Set
        End Property

        Public Sub Validate() Implements IValidator.Validate
        End Sub
    End Class

End Namespace
