
Imports System.ComponentModel

<Serializable()> _
Partial Public Class SubClassInfo

    'Internal member variables
    Private _deptId As Integer
    Private _classId As Integer
    Private _subClassId As Integer
    Private _subClassShortDesc As String

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
    Public Property SubClassId() As Integer
        Get
            Return _subClassId
        End Get
        Set(ByVal value As Integer)
            _subClassId = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property SubClassShortDesc() As String
        Get
            Return _subClassShortDesc
        End Get
        Set(ByVal value As String)
            _subClassShortDesc = value
        End Set
    End Property

    Public ReadOnly Property SubClassIdDesc As String
        Get
            Return SubClassId & " - " & SubClassShortDesc
        End Get
    End Property

End Class

