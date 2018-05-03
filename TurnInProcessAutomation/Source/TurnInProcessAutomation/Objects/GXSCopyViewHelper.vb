Public NotInheritable Class GXSCopyViewHelper

    Public Shared Function GetView(ByVal crg As Integer, ByVal cmg As Integer, ByVal cfg As Integer, ByVal dept As Integer) As String
        Dim view As String = ""
        Dim file As String = HttpContext.Current.Server.MapPath("..\..\WebUserControls\GXS") & "\GXSCatalogViews.xml"
        Dim dt As New DataTable
        dt.ReadXml(file)
        Dim dr() As DataRow
        'Dept
        dr = dt.Select("CRG_ID=" & crg & " AND CMG_ID=" & cmg & " AND CFG_ID=" & cfg & " AND DEPT_ID=" & dept)
        If dr.Count = 1 Then
            view = CStr(dr(0)("View"))
        Else
            'CFG
            dr = dt.Select("CRG_ID=" & crg & " AND CMG_ID=" & cmg & " AND CFG_ID=" & cfg)
            If dr.Count = 1 AndAlso CInt(dr(0)("DEPT_ID")) = 0 Then
                view = CStr(dr(0)("View"))
            Else
                'CMG
                dr = dt.Select("CRG_ID=" & crg & " AND CMG_ID=" & cmg)
                If dr.Count = 1 AndAlso CInt(dr(0)("CFG_ID")) = 0 AndAlso CInt(dr(0)("DEPT_ID")) = 0 Then
                    view = CStr(dr(0)("View"))
                Else
                    'CRG
                    dr = dt.Select("CRG_ID=" & crg)
                    If dr.Count = 1 AndAlso CInt(dr(0)("CMG_ID")) = 0 AndAlso CInt(dr(0)("CFG_ID")) = 0 AndAlso CInt(dr(0)("DEPT_ID")) = 0 Then
                        view = CStr(dr(0)("View"))
                    Else
                        'CFG Exclusions
                        dr = dt.Select("CRG_ID=" & crg & " AND CFG_ID<0")
                        If dr.Count = 1 AndAlso CInt(dr(0)("CMG_ID")) = 0 AndAlso CInt(dr(0)("CFG_ID")) * -1 <> cfg AndAlso CInt(dr(0)("DEPT_ID")) = 0 Then
                            view = CStr(dr(0)("View"))
                        End If
                    End If
                End If
            End If
        End If
        Return view
    End Function

End Class
