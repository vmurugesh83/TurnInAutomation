Public Interface IExportHandler

    Function CanExport(ByVal dictionary As Dictionary(Of String, Object)) As Boolean

    Sub ExportToPDF(ByVal tableList As Dictionary(Of String, Object), ByVal pageheader As String, ByVal isLandscape As Boolean, ByVal IsTabbed As Boolean)

    Sub ExportToExcel(ByVal tableList As Dictionary(Of String, Object), ByVal pageheader As String, ByVal IsTabbed As Boolean)

End Interface
