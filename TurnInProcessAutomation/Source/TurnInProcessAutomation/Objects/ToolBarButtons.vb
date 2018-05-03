Imports System
Imports System.Text.RegularExpressions

''' <summary>
''' Class to build an xml string to use with the RadToolBar control.
''' </summary>
''' <remarks>
''' Implemented using Singleton Object concept as defined in MS Patterns And Practices.
''' This is not purely a singleton, however within the context of a session, it behaves as a singleton.
''' </remarks>
Public Class ToolBarButtons

#Region "Constants"

    Private Const _IMGURL As String = "ImageUrl='~/Images/{0}' "
    Private Const _DISIMGURL As String = "DisabledImageUrl='~/Images/{0}' "
    Private Const _ENABLED As String = "Enabled='{0}' "
    Private Const _TEXT As String = "Text='{0}' "
    Private Const _ISSEPARATOR As String = "<Button IsSeparator='True' />"

#End Region



#Region "Variables"

    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private _sb As StringBuilder
    Private _XMLButtonList As String

    'Name that will be used as key for Session object
    Private Const _SessionObjName As String = "ToolBarButtons"

#End Region



#Region "Enumerators"

    Enum ButtonText
        Delete
        Print
        Reset
        Retrieve
        Save
        EOD
        'Export_Excel
        Export
    End Enum



    Enum ButtonAttributes
        CssClass
        DisabledImageUrl
        Enabled
        ImageUrl
        IsSeperator
        Text
    End Enum

#End Region



#Region "Properties"
    ''' <summary>
    ''' Gets or Sets the xml string that contains all button information
    ''' </summary>    
    Public Property XMLButtonList() As String
        Get
            Return _XMLButtonList
        End Get
        Set(ByVal value As String)
            _XMLButtonList = value.Replace("""", "'")
        End Set
    End Property

    Public Shared ReadOnly Property Instance() As ToolBarButtons
        Get
            Try
                Dim obj As ToolBarButtons

                If System.Web.HttpContext.Current.Session(_SessionObjName) Is Nothing Then
                    'No current session object exists.
                    'Use private constructor to create an instance and place it into the session
                    obj = New ToolBarButtons
                    System.Web.HttpContext.Current.Session(_SessionObjName) = obj
                Else
                    'Retrieve the already instance that was already created
                    obj = CType(System.Web.HttpContext.Current.Session(_SessionObjName), ToolBarButtons)
                End If

                'Return the single instance of this class that was stored in the session
                Return obj

            Catch ex As Exception
                Log.Error(ex)
                Throw
            End Try
        End Get
    End Property
#End Region



#Region "Constructor"

    Sub New()
        Try
            'set xml string header
            _sb = New StringBuilder
            _sb.AppendLine("<?xml version='1.0' encoding='utf-16' ?>")
            _sb.AppendLine("<ToolBar ImagesDir='~/Images/'>")
        Catch ex As Exception
            Log.Error("Error in constructor:", ex)
            Throw
        End Try
    End Sub

#End Region



#Region "Methods"
    ''' <summary>
    ''' adds button selection to xml string
    ''' </summary>
    Public Overloads Sub Add(ByVal text As ButtonText)
        Try
            'set button header
            _sb.Append("<Button ")

            'set appropriate properties based on text
            Select Case text
                Case ButtonText.Save
                    _sb.Append(String.Format(_TEXT, ButtonText.Save))
                    _sb.Append(String.Format(_IMGURL, "Save.gif"))
                    _sb.Append(String.Format(_DISIMGURL, "Save_d.gif"))
                Case ButtonText.Delete
                    _sb.Append(String.Format(_TEXT, ButtonText.Delete))
                    _sb.Append(String.Format(_IMGURL, "Delete.gif"))
                    _sb.Append(String.Format(_DISIMGURL, "Delete_d.gif"))
                Case ButtonText.Reset
                    _sb.Append(String.Format(_TEXT, ButtonText.Reset))
                    _sb.Append(String.Format(_IMGURL, "Reset.gif"))
                    _sb.Append(String.Format(_DISIMGURL, "Reset_d.gif"))
                Case ButtonText.Print
                    _sb.Append(String.Format(_TEXT, ButtonText.Print))
                    _sb.Append(String.Format(_IMGURL, "Print.gif"))
                    _sb.Append(String.Format(_DISIMGURL, "Print_d.gif"))
                Case ButtonText.Retrieve
                    _sb.Append(String.Format(_TEXT, ButtonText.Retrieve))
                    _sb.Append(String.Format(_IMGURL, "Retrieve1.gif"))
                    _sb.Append(String.Format(_DISIMGURL, "Retrieve1_d.gif"))
                Case ButtonText.Export
                    'Case ButtonText.Export_Excel
                    '_sb.Append(String.Format(_TEXT, ButtonText.Export_Excel.ToString().Replace("_", " ")))
                    _sb.Append(String.Format(_TEXT, ButtonText.Export))
                    _sb.Append(String.Format(_IMGURL, "Export.gif"))
                    _sb.Append(String.Format(_DISIMGURL, "Export_d.gif"))
            End Select

            'close button string
            _sb.AppendLine("/>")
        Catch ex As Exception
            Log.Error("Error adding button information", ex)
            Throw
        End Try
    End Sub
    ''' <summary>
    ''' adds button selection to xml string
    ''' </summary>
    Public Overloads Sub Add(ByVal text As ButtonText, ByVal insertSeparator As Boolean)
        Try
            'set button header
            _sb.Append("<Button ")

            'set appropriate properties based on text
            Select Case text
                Case ButtonText.Save
                    _sb.Append(String.Format(_TEXT, ButtonText.Save))
                    _sb.Append(String.Format(_IMGURL, "Save.gif"))
                    _sb.Append(String.Format(_DISIMGURL, "Save_d.gif"))
                Case ButtonText.Delete
                    _sb.Append(String.Format(_TEXT, ButtonText.Delete))
                    _sb.Append(String.Format(_IMGURL, "Delete.gif"))
                    _sb.Append(String.Format(_DISIMGURL, "Delete_d.gif"))
                Case ButtonText.Reset
                    _sb.Append(String.Format(_TEXT, ButtonText.Reset))
                    _sb.Append(String.Format(_IMGURL, "Reset.gif"))
                    _sb.Append(String.Format(_DISIMGURL, "Reset_d.gif"))
                Case ButtonText.Print
                    _sb.Append(String.Format(_TEXT, ButtonText.Print))
                    _sb.Append(String.Format(_IMGURL, "Print.gif"))
                    _sb.Append(String.Format(_DISIMGURL, "Print_d.gif"))
                Case ButtonText.Retrieve
                    _sb.Append(String.Format(_TEXT, ButtonText.Retrieve))
                    _sb.Append(String.Format(_IMGURL, "Retrieve1.gif"))
                    _sb.Append(String.Format(_DISIMGURL, "Retrieve1_d.gif"))
                Case ButtonText.Export
                    'Case ButtonText.Export_Excel
                    '_sb.Append(String.Format(_TEXT, ButtonText.Export_Excel.ToString.Replace("_", " ")))
                    _sb.Append(String.Format(_TEXT, ButtonText.Export))
                    _sb.Append(String.Format(_IMGURL, "Export.gif"))
                    _sb.Append(String.Format(_DISIMGURL, "Export_d.gif"))
            End Select

            'close button string
            _sb.AppendLine("/>")

            'insert separator
            If insertSeparator Then
                _sb.AppendLine(_ISSEPARATOR)
            End If
        Catch ex As Exception
            Log.Error("Error adding button information", ex)
            Throw
        End Try
    End Sub
    ''' <summary>
    ''' adds button selection to xml string
    ''' </summary>
    Public Overloads Sub Add(ByVal text As ButtonText, ByVal insertSeparator As Boolean, _
        ByVal enabled As Boolean)
        Try
            'set button header
            _sb.Append("<Button ")

            'set appropriate properties based on text
            _sb.Append(String.Format(_ENABLED, enabled.ToString))
            Select Case text
                Case ButtonText.Save
                    _sb.Append(String.Format(_TEXT, ButtonText.Save))
                    _sb.Append(String.Format(_IMGURL, "Save.gif"))
                    _sb.Append(String.Format(_DISIMGURL, "Save_d.gif"))
                Case ButtonText.Delete
                    _sb.Append(String.Format(_TEXT, ButtonText.Delete))
                    _sb.Append(String.Format(_IMGURL, "Delete.gif"))
                    _sb.Append(String.Format(_DISIMGURL, "Delete_d.gif"))
                Case ButtonText.Reset
                    _sb.Append(String.Format(_TEXT, ButtonText.Reset))
                    _sb.Append(String.Format(_IMGURL, "Reset.gif"))
                    _sb.Append(String.Format(_DISIMGURL, "Reset_d.gif"))
                Case ButtonText.Print
                    _sb.Append(String.Format(_TEXT, ButtonText.Print))
                    _sb.Append(String.Format(_IMGURL, "Print.gif"))
                    _sb.Append(String.Format(_DISIMGURL, "Print_d.gif"))
                Case ButtonText.Retrieve
                    _sb.Append(String.Format(_TEXT, ButtonText.Retrieve))
                    _sb.Append(String.Format(_IMGURL, "Retrieve1.gif"))
                    _sb.Append(String.Format(_DISIMGURL, "Retrieve1_d.gif"))
                Case ButtonText.Export
                    'Case ButtonText.Export_Excel
                    '_sb.Append(String.Format(_TEXT, ButtonText.Export_Excel.ToString.Replace("_", " ")))
                    _sb.Append(String.Format(_TEXT, ButtonText.Export))
                    _sb.Append(String.Format(_IMGURL, "Export.gif"))
                    _sb.Append(String.Format(_DISIMGURL, "Export_d.gif"))
            End Select

            'close button string
            _sb.AppendLine("/>")

            'insert separator
            If insertSeparator Then
                _sb.AppendLine(_ISSEPARATOR)
            End If
        Catch ex As Exception
            Log.Error("Error adding button information", ex)
            Throw
        End Try
    End Sub
    ''' <summary>
    ''' adds custom button to xml string
    ''' </summary>    
    Public Overloads Sub Add(ByVal customText As String)
        Try
            'set button header
            _sb.Append("<Button ")

            'set appropriate properties based on text
            _sb.Append(String.Format(_TEXT, customText))

            'close button string
            _sb.AppendLine("/>")
        Catch ex As Exception
            Log.Error("Error adding button information", ex)
            Throw
        End Try
    End Sub
    ''' <summary>
    ''' adds custom button to xml string
    ''' </summary>    
    Public Overloads Sub Add(ByVal customText As String, ByVal insertSeparator As Boolean)
        Try
            'set button header
            _sb.Append("<Button ")

            'set appropriate properties based on text
            _sb.Append(String.Format(_TEXT, customText))

            'close button string
            _sb.AppendLine("/>")

            'insert separator
            If insertSeparator Then
                _sb.AppendLine(_ISSEPARATOR)
            End If
        Catch ex As Exception
            Log.Error("Error adding button information", ex)
            Throw
        End Try
    End Sub
    ''' <summary>
    ''' adds custom button to xml string
    ''' </summary>    
    Public Overloads Sub Add(ByVal customText As String, ByVal insertSeparator As Boolean, _
        ByVal enabled As Boolean)
        Try
            'set button header
            _sb.Append("<Button ")

            'set appropriate properties based on text
            _sb.Append(String.Format(_TEXT, customText))
            _sb.Append(String.Format(_ENABLED, enabled.ToString))

            'close button string
            _sb.AppendLine("/>")

            'insert separator
            If insertSeparator Then
                _sb.AppendLine(_ISSEPARATOR)
            End If
        Catch ex As Exception
            Log.Error("Error adding button information", ex)
            Throw
        End Try
    End Sub
    ''' <summary>
    ''' adds custom button with additional properties to xml string
    ''' </summary>    
    Public Overloads Sub Add(ByVal customText As String, ByVal insertSeparator As Boolean, _
        ByVal enabled As Boolean, ByVal imgURL As String, ByVal disImgUrl As String)
        Try
            'set button header
            _sb.Append("<Button ")

            'set appropriate properties based on text
            _sb.Append(String.Format(_TEXT, customText))
            _sb.Append(String.Format(_IMGURL, imgURL))
            _sb.Append(String.Format(_DISIMGURL, disImgUrl))
            _sb.Append(String.Format(_ENABLED, enabled.ToString))


            'close button string
            _sb.AppendLine("/>")

            'insert separator
            If insertSeparator Then
                _sb.AppendLine(_ISSEPARATOR)
            End If
        Catch ex As Exception
            Log.Error("Error adding button information", ex)
            Throw
        End Try
    End Sub
    ''' <summary>
    ''' adds seperator in between buttons
    ''' </summary>
    Public Sub AddSeparator()
        Try
            _sb.AppendLine(_ISSEPARATOR)
        Catch ex As Exception
            Log.Error("Error adding button separator", ex)
            Throw
        End Try
    End Sub
    ''' <summary>
    ''' Finalizes and stores xml string.  Reference xml string in XMLButtonList property
    ''' </summary>
    Public Sub Build()
        Try
            'close xml string
            If _sb.ToString.EndsWith("</ToolBar>") = False Then
                _sb.Append("</ToolBar>")
            Else
                Throw New Exception("XML string is already built.  Run the Clear method first.")
            End If

            'update xml button list
            _XMLButtonList = _sb.ToString
        Catch ex As Exception
            Log.Error("Error finalizing xml string", ex)
            Throw
        End Try
    End Sub
    ''' <summary>
    ''' Clears xml string for new buttons
    ''' </summary>
    Public Sub Clear()
        Try
            'reset xml button list
            _XMLButtonList = String.Empty

            'set xml string header
            _sb = New StringBuilder
            _sb.AppendLine("<?xml version='1.0' encoding='utf-16' ?>")
            _sb.AppendLine("<ToolBar ImagesDir='~/Images/'>")
        Catch ex As Exception
            Log.Error("Error clearing xml string", ex)
            Throw
        End Try
    End Sub
    ''' <summary>
    ''' Modifies xml string with existing button
    ''' </summary>        
    Public Sub Modify(ByVal text As ButtonText, ByVal attribute As ButtonAttributes, ByVal value As Object)
        'declare variables
        Dim attrName As String
        Dim btnName As String
        Dim result As String
        Dim oldValue As String
        Dim newValue As String
        Dim newLine As String

        Try
            'reformat enum types
            attrName = [Enum].GetName(GetType(ButtonAttributes), attribute)
            btnName = [Enum].GetName(GetType(ButtonText), text)

            'set new value based on property
            newValue = String.Format("{0}='{1}' ", attrName, value.ToString)

            'search xml string for button
            result = Regex.Match(_XMLButtonList, String.Format("<.*Text='{0}'.*/>", btnName)).ToString
            If result = "" Then
                Throw New Exception("Button does not exist")
            End If

            'search result for property
            If result.Contains(attrName) Then
                oldValue = Regex.Match(result, String.Format("{0}='\w*'\s", attrName)).ToString
                newLine = result.Replace(oldValue, newValue)
            Else
                newLine = result.Insert(result.Length - 2, newValue)
            End If

            'update xml string
            _XMLButtonList = _XMLButtonList.Replace(result, newLine)
        Catch ex As Exception
            Log.Error("Error modifying xml string", ex)
            Throw
        End Try
    End Sub
    ''' <summary>
    ''' Modifies xml string with existing button
    ''' </summary>    
    Public Sub Modify(ByVal customText As String, ByVal attribute As ButtonAttributes, ByVal value As Object)
        'declare variables
        Dim attrName As String
        Dim result As String
        Dim oldValue As String
        Dim newValue As String
        Dim newLine As String

        Try
            'reformat enum types
            attrName = [Enum].GetName(GetType(ButtonAttributes), attribute)

            'set new value based on property
            newValue = String.Format("{0}='{1}' ", attrName, value.ToString)

            'search xml string for button
            result = Regex.Match(_XMLButtonList, String.Format("<.*Text='{0}'.*/>", customText)).ToString
            If result = "" Then
                Throw New Exception("Button does not exist")
            End If

            'search result for property
            If result.Contains(attrName) Then
                oldValue = Regex.Match(result, String.Format("{0}='[^']*'\s", attrName)).ToString
                newLine = result.Replace(oldValue, newValue)
            Else
                newLine = result.Insert(result.Length - 2, newValue)
            End If

            'update xml string
            _XMLButtonList = _XMLButtonList.Replace(result, newLine)
        Catch ex As Exception
            Log.Error("Error modifying xml string", ex)
            Throw
        End Try
    End Sub
#End Region



#Region "Functions"
#End Region


End Class
