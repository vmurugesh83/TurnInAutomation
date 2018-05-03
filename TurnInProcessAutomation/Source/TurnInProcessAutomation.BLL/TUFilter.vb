''' <summary>
''' Class for holding turn in filter user selections
''' </summary>
''' <remarks>
''' Created on August 2015 as a part of Turn in Filter left hand navigation changes
''' </remarks>
Public Class TUFilter
    ''' <summary>
    ''' Property for persisting the "Sample available and approved" checkbox selection in turn in filter
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AvailableForTurnIn As Boolean
    ''' <summary>
    ''' Property for persisting the "Sample Not available" checkbox selection in turn in filter
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NotAvailableForTurnIn As Boolean
    ''' <summary>
    ''' Property for persisting the "Active on Web" checkbox selection in turn in filter
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ActiveOnWeb As Boolean
    ''' <summary>
    ''' Property for persisting the "Not Active on Web" checkbox selection in turn in filter
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NotActiveOnWeb As Boolean
    ''' <summary>
    ''' Property for persisting the "Not Turned in" checkbox selection in turn in filter
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NotTurnedIn As Boolean
End Class
