
Imports System.ComponentModel

<Serializable()> _
Partial Public Class DepartmentInfo

    'Internal member variables
    Private _deptId As Integer
    Private _deptShortDesc As String

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
    Public Property DeptShortDesc() As String
        Get
            Return _deptShortDesc
        End Get
        Set(ByVal value As String)
            _deptShortDesc = value
        End Set
    End Property

    Public ReadOnly Property DeptIdDesc As String
        Get
            Return DeptId & " - " & DeptShortDesc
        End Get
    End Property
    
End Class

