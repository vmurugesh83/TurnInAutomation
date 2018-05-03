
Imports System.ComponentModel

<Serializable()> _
Partial Public Class ClassInfo

    'Internal member variables
    Private _deptId As Integer
    Private _classId As Integer
    Private _classShortDesc As String

	
    'Default constructor
    Public Sub New()
    End Sub

    <DataObjectField(True)> _
    Public Property DeptId() As Integer
        Get
            Return _deptId
        End Get
        Set(ByVal value As Integer)
            _deptId = value
        End Set
    End Property
    
    <DataObjectField(True)> _
    Public Property ClassId() As Integer
        Get
            Return _classId
        End Get
        Set(ByVal value As Integer)
            _classId = value
        End Set
    End Property
    
    <DataObjectField(True)> _
    Public Property ClassShortDesc() As String
        Get
            Return _classShortDesc
        End Get
        Set(ByVal value As String)
            _classShortDesc = value
        End Set
    End Property

    Public ReadOnly Property ClassIdDesc As String
        Get
            Return ClassId & " - " & ClassShortDesc
        End Get
    End Property

End Class

