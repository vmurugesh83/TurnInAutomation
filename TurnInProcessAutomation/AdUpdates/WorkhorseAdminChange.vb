''' <summary>
''' POCO for the Tally Table in the Admin Database
''' </summary>
''' <remarks></remarks>
Public Class WorkhorseAdminChange

    Public Property RowId As Integer
    Public Property WHaction As String
    Public Property wh_ad_nbr As Nullable(Of Integer)
    Public Property wh_sys_pg_nbr As Nullable(Of Integer)
    Public Property wh_shot_nbr As Nullable(Of Integer)
    Public Property wh_img_id As Nullable(Of Integer)
    Public Property wh_merch_id As Nullable(Of Integer)
    Public Property TriggerEvent As String
    Public Property Action As String
    Public Property TallyDateTime As DateTime
    Public Property Source As String
    Public Property old_ad_nbr As Nullable(Of Integer)
    Public Property old_sys_pg_nbr As Nullable(Of Integer)
    Public Property old_shot_nbr As Nullable(Of Integer)
    Public Property old_img_id As Nullable(Of Integer)
    Public Property old_merch_id As Nullable(Of Integer)

End Class
