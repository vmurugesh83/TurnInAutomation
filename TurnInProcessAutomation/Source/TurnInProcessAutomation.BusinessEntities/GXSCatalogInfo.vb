Public Class GXSCatalogInfo

    Private _GXSCFG As IList(Of GXSCFGInfo)
    Private _GXSUPC As IList(Of GXSUPCInfo)

    Public Property GXSCFG() As IList(Of GXSCFGInfo)
        Get
            Return _GXSCFG
        End Get
        Set(value As IList(Of GXSCFGInfo))
            _GXSCFG = value
        End Set
    End Property

    Public Property GXSUPC() As IList(Of GXSUPCInfo)
        Get
            Return _GXSUPC
        End Get
        Set(value As IList(Of GXSUPCInfo))
            _GXSUPC = value
        End Set
    End Property

End Class
